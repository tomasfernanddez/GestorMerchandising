using BLL.Interfaces;
using Services.BLL.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UI.Localization;

namespace UI
{
    public partial class ABMPedidosForm : Form
    {
        private readonly IPedidoService _pedidoService;
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly IProveedorService _proveedorService;
        private readonly IBitacoraService _bitacora;

        private class Item { public Guid Id { get; set; } public string Nombre { get; set; } }

        public ABMPedidosForm()
        {
            InitializeComponent();
        }

        public ABMPedidosForm(IPedidoService pedidoService, IClienteService clienteService,
            IProductoService productoService, IProveedorService proveedorService, IBitacoraService bitacora)
        {
            _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacora = bitacora ?? throw new ArgumentNullException(nameof(bitacora));

            InitializeComponent();
            this.Load += ABMPedidosForm_Load;
        }

        private sealed class PedidoGridRow
        {
            public Guid IdPedido { get; set; }
            public string Numero { get; set; }
            public string Cliente { get; set; }
            public DateTime Fecha { get; set; }
            public string FechaLimite { get; set; }
            public string Estado { get; set; }
            public decimal Total { get; set; }
            public bool Facturado { get; set; }
            public bool Vencido { get; set; }
        }

        private void ABMPedidosForm_Load(object sender, EventArgs e)
        {
            EnsureColumns();
            ApplyTexts();
            CargarFiltros();
            WireUp();
            CargarPedidos();
        }

        private void EnsureColumns()
        {
            if (dgvPedidos.Columns.Count > 0) return;

            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPedidos.AllowUserToResizeColumns = true;

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoGridRow.IdPedido),
                Name = "IdPedido",
                Visible = false
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoGridRow.Numero),
                Name = "Numero",
                HeaderText = "Número",
                FillWeight = 100,
                MinimumWidth = 90
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoGridRow.Cliente),
                Name = "Cliente",
                HeaderText = "Cliente",
                FillWeight = 180,
                MinimumWidth = 150
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoGridRow.Fecha),
                Name = "Fecha",
                HeaderText = "Fecha",
                FillWeight = 100,
                MinimumWidth = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoGridRow.FechaLimite),
                Name = "FechaLimite",
                HeaderText = "Fecha Límite",
                FillWeight = 100,
                MinimumWidth = 90
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoGridRow.Estado),
                Name = "Estado",
                HeaderText = "Estado",
                FillWeight = 120,
                MinimumWidth = 100
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoGridRow.Total),
                Name = "Total",
                HeaderText = "Total",
                FillWeight = 100,
                MinimumWidth = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvPedidos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(PedidoGridRow.Facturado),
                Name = "Facturado",
                HeaderText = "Facturado",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                MinimumWidth = 70
            });

            // Columna oculta para alertas
            dgvPedidos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(PedidoGridRow.Vencido),
                Name = "Vencido",
                Visible = false
            });
        }

        private void ApplyTexts()
        {
            Text = "abm.orders.title".Traducir();
            SetHeaderSafe("Numero", "order.number");
            SetHeaderSafe("Cliente", "order.client");
            SetHeaderSafe("Fecha", "order.date");
            SetHeaderSafe("FechaLimite", "order.deadline");
            SetHeaderSafe("Estado", "order.status");
            SetHeaderSafe("Total", "order.total");
            SetHeaderSafe("Facturado", "order.invoiced");

            grpFiltros.Text = "Filtros";
            lblFiltroCliente.Text = "order.client".Traducir();
            lblFiltroEstado.Text = "order.status".Traducir();
            chkSoloVencidos.Text = "Solo Vencidos";
            btnFiltrar.Text = "Filtrar";
            btnLimpiarFiltros.Text = "Limpiar Filtros";
        }

        private void SetHeaderSafe(string columnName, string i18nKey)
        {
            var col = dgvPedidos?.Columns?[columnName];
            if (col != null)
                col.HeaderText = i18nKey.Traducir();
        }

        private void CargarFiltros()
        {
            // Cargar clientes
            var clientes = _clienteService.ObtenerClientesActivos()?.ToList() ?? new List<Cliente>();
            cboFiltroCliente.Items.Add(new Item { Id = Guid.Empty, Nombre = "-- Todos --" });
            foreach (var cliente in clientes)
            {
                cboFiltroCliente.Items.Add(new Item { Id = cliente.IdCliente, Nombre = cliente.RazonSocial });
            }
            cboFiltroCliente.DisplayMember = "Nombre";
            cboFiltroCliente.ValueMember = "Id";
            cboFiltroCliente.SelectedIndex = 0;

            // Cargar estados
            var estados = _pedidoService.ObtenerEstadosPedido()?.ToList() ?? new List<DomainModel.Entidades.EstadoPedido>();
            cboFiltroEstado.Items.Add(new Item { Id = Guid.Empty, Nombre = "-- Todos --" });
            foreach (var estado in estados)
            {
                cboFiltroEstado.Items.Add(new Item { Id = estado.IdEstadoPedido, Nombre = estado.NombreEstadoPedido });
            }
            cboFiltroEstado.DisplayMember = "Nombre";
            cboFiltroEstado.ValueMember = "Id";
            cboFiltroEstado.SelectedIndex = 0;
        }

        private void WireUp()
        {
            tsbActualizar.Click += (s, e) => CargarPedidos();
            tsbBuscar.Click += (s, e) => Buscar();
            txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Buscar(); } };

            tsbNuevo.Click += (s, e) => NuevoPedido();
            tsbEditar.Click += (s, e) => EditarSeleccionado();
            dgvPedidos.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };

            btnFiltrar.Click += (s, e) => AplicarFiltros();
            btnLimpiarFiltros.Click += (s, e) => LimpiarFiltros();

            // Resaltar pedidos vencidos en rojo
            dgvPedidos.CellFormatting += DgvPedidos_CellFormatting;
        }

        private void DgvPedidos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvPedidos.Rows[e.RowIndex];
            var vencidoCell = row.Cells["Vencido"];

            if (vencidoCell != null && vencidoCell.Value != null && (bool)vencidoCell.Value)
            {
                e.CellStyle.BackColor = System.Drawing.Color.LightPink;
                e.CellStyle.ForeColor = System.Drawing.Color.DarkRed;
            }
        }

        private void CargarPedidos()
        {
            try
            {
                var pedidos = _pedidoService.ObtenerTodosLosPedidos()?.ToList() ?? new List<Pedido>();
                var hoy = DateTime.Today;

                var rows = pedidos.Select(p => new PedidoGridRow
                {
                    IdPedido = p.IdPedido,
                    Numero = p.NumeroPedido,
                    Cliente = p.Cliente?.RazonSocial ?? "-",
                    Fecha = p.Fecha,
                    FechaLimite = p.TieneFechaLimite && p.FechaLimite.HasValue ? p.FechaLimite.Value.ToString("dd/MM/yyyy") : "-",
                    Estado = p.EstadoPedido?.NombreEstadoPedido ?? "-",
                    Total = _pedidoService.CalcularTotalPedido(p.IdPedido),
                    Facturado = p.Facturado,
                    Vencido = p.TieneFechaLimite && p.FechaLimite.HasValue && p.FechaLimite.Value < hoy
                }).OrderByDescending(r => r.Fecha).ToList();

                dgvPedidos.DataSource = rows;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pedidos: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Buscar()
        {
            try
            {
                var texto = txtBuscar.Text?.Trim();
                if (string.IsNullOrWhiteSpace(texto))
                {
                    CargarPedidos();
                    return;
                }

                // Buscar por número de pedido
                var pedido = _pedidoService.ObtenerPedidoPorNumero(texto);
                if (pedido != null)
                {
                    var hoy = DateTime.Today;
                    var rows = new List<PedidoGridRow>
                    {
                        new PedidoGridRow
                        {
                            IdPedido = pedido.IdPedido,
                            Numero = pedido.NumeroPedido,
                            Cliente = pedido.Cliente?.RazonSocial ?? "-",
                            Fecha = pedido.Fecha,
                            FechaLimite = pedido.TieneFechaLimite && pedido.FechaLimite.HasValue ? pedido.FechaLimite.Value.ToString("dd/MM/yyyy") : "-",
                            Estado = pedido.EstadoPedido?.NombreEstadoPedido ?? "-",
                            Total = _pedidoService.CalcularTotalPedido(pedido.IdPedido),
                            Facturado = pedido.Facturado,
                            Vencido = pedido.TieneFechaLimite && pedido.FechaLimite.HasValue && pedido.FechaLimite.Value < hoy
                        }
                    };
                    dgvPedidos.DataSource = rows;
                }
                else
                {
                    MessageBox.Show("No se encontró ningún pedido con ese número", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFiltros()
        {
            try
            {
                var pedidos = _pedidoService.ObtenerTodosLosPedidos()?.ToList() ?? new List<Pedido>();
                var hoy = DateTime.Today;

                // Filtro por cliente
                var clienteSeleccionado = cboFiltroCliente.SelectedItem as Item;
                if (clienteSeleccionado != null && clienteSeleccionado.Id != Guid.Empty)
                {
                    pedidos = pedidos.Where(p => p.IdCliente == clienteSeleccionado.Id).ToList();
                }

                // Filtro por estado
                var estadoSeleccionado = cboFiltroEstado.SelectedItem as Item;
                if (estadoSeleccionado != null && estadoSeleccionado.Id != Guid.Empty)
                {
                    pedidos = pedidos.Where(p => p.IdEstadoPedido == estadoSeleccionado.Id).ToList();
                }

                // Filtro por vencidos
                if (chkSoloVencidos.Checked)
                {
                    pedidos = pedidos.Where(p => p.TieneFechaLimite && p.FechaLimite.HasValue && p.FechaLimite.Value < hoy).ToList();
                }

                var rows = pedidos.Select(p => new PedidoGridRow
                {
                    IdPedido = p.IdPedido,
                    Numero = p.NumeroPedido,
                    Cliente = p.Cliente?.RazonSocial ?? "-",
                    Fecha = p.Fecha,
                    FechaLimite = p.TieneFechaLimite && p.FechaLimite.HasValue ? p.FechaLimite.Value.ToString("dd/MM/yyyy") : "-",
                    Estado = p.EstadoPedido?.NombreEstadoPedido ?? "-",
                    Total = _pedidoService.CalcularTotalPedido(p.IdPedido),
                    Facturado = p.Facturado,
                    Vencido = p.TieneFechaLimite && p.FechaLimite.HasValue && p.FechaLimite.Value < hoy
                }).OrderByDescending(r => r.Fecha).ToList();

                dgvPedidos.DataSource = rows;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al aplicar filtros: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFiltros()
        {
            cboFiltroCliente.SelectedIndex = 0;
            cboFiltroEstado.SelectedIndex = 0;
            chkSoloVencidos.Checked = false;
            CargarPedidos();
        }

        private void NuevoPedido()
        {
            var form = new PedidoForm(_pedidoService, _clienteService, _productoService, _proveedorService, _bitacora, pedido: null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                CargarPedidos();
            }
        }

        private void EditarSeleccionado()
        {
            if (dgvPedidos.CurrentRow == null) return;
            var row = dgvPedidos.CurrentRow.DataBoundItem as PedidoGridRow;
            if (row == null) return;

            try
            {
                var pedido = _pedidoService.ObtenerPedidoCompleto(row.IdPedido);
                if (pedido == null)
                {
                    MessageBox.Show("Pedido no encontrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var form = new PedidoForm(_pedidoService, _clienteService, _productoService, _proveedorService, _bitacora, pedido);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    CargarPedidos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
