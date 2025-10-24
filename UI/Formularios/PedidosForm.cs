using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using BLL.Interfaces;
using Services.BLL.Interfaces;
using UI.Localization;
using UI.ViewModels;
using DomainModel;
using DomainModel.Entidades;

namespace UI
{
    public partial class PedidosForm : Form
    {
        private readonly IPedidoService _pedidoService;
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly ICategoriaProductoService _categoriaService;
        private readonly IProveedorService _proveedorService;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogService _logService;

        private BindingList<PedidoRow> _rows;
        private List<EstadoPedido> _estados;

        private sealed class PedidoRow
        {
            public Guid IdPedido { get; set; }
            public string NumeroPedido { get; set; }
            public string Cliente { get; set; }
            public string Estado { get; set; }
            public DateTime FechaCreacion { get; set; }
            public DateTime? FechaEntrega { get; set; }
            public decimal Total { get; set; }
            public bool Facturado { get; set; }
            public decimal SaldoPendiente { get; set; }
        }

        public PedidosForm(
            IPedidoService pedidoService,
            IClienteService clienteService,
            IProductoService productoService,
            ICategoriaProductoService categoriaService,
            IProveedorService proveedorService,
            IBitacoraService bitacoraService,
            ILogService logService)
        {
            _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _categoriaService = categoriaService ?? throw new ArgumentNullException(nameof(categoriaService));
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));

            InitializeComponent();
        }

        private void PedidosForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            ConfigurarGrid();
            CargarEstados();
            InicializarFiltros();
            BuscarPedidos();
        }

        private void ApplyTexts()
        {
            Text = "order.list.title".Traducir();
            tsbNuevo.Text = "order.list.new".Traducir();
            tsbEditar.Text = "order.list.edit".Traducir();
            tsbActualizar.Text = "form.refresh".Traducir();
            tslBuscar.Text = "form.search".Traducir();
            btnBuscar.Text = "form.filter".Traducir();
            tslEstado.Text = "order.state".Traducir();
            tslFacturado.Text = "order.invoiced".Traducir();
            tslSaldo.Text = "order.summary.balance".Traducir();

            if (dgvPedidos.Columns.Count > 0)
            {
                ActualizarEncabezadosGrid();
            }

            if (_estados != null)
            {
                InicializarFiltros(true);
            }
        }

        private void ConfigurarGrid()
        {
            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.Columns.Clear();

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.NumeroPedido),
                Name = nameof(PedidoRow.NumeroPedido),
                HeaderText = "order.number".Traducir(),
                FillWeight = 110,
                MinimumWidth = 100
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.Cliente),
                Name = nameof(PedidoRow.Cliente),
                HeaderText = "order.client".Traducir(),
                FillWeight = 180,
                MinimumWidth = 160
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.Estado),
                Name = nameof(PedidoRow.Estado),
                HeaderText = "order.state".Traducir(),
                FillWeight = 110,
                MinimumWidth = 110
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.FechaCreacion),
                Name = nameof(PedidoRow.FechaCreacion),
                HeaderText = "order.createdAt".Traducir(),
                FillWeight = 110,
                MinimumWidth = 110,
                DefaultCellStyle = { Format = "g" }
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.FechaEntrega),
                Name = nameof(PedidoRow.FechaEntrega),
                HeaderText = "order.deadline".Traducir(),
                FillWeight = 110,
                MinimumWidth = 110,
                DefaultCellStyle = { Format = "d" }
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.Total),
                Name = nameof(PedidoRow.Total),
                HeaderText = "order.summary.total".Traducir(),
                FillWeight = 100,
                MinimumWidth = 100,
                DefaultCellStyle = { Format = "C2" }
            });
            dgvPedidos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.Facturado),
                Name = nameof(PedidoRow.Facturado),
                HeaderText = "order.invoiced".Traducir(),
                FillWeight = 80,
                MinimumWidth = 80
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.SaldoPendiente),
                Name = nameof(PedidoRow.SaldoPendiente),
                HeaderText = "order.summary.balance".Traducir(),
                FillWeight = 100,
                MinimumWidth = 100,
                DefaultCellStyle = { Format = "N2" }
            });

            _rows = new BindingList<PedidoRow>();
            bindingSource.DataSource = _rows;
        }

        private void ActualizarEncabezadosGrid()
        {
            SetHeaderText(nameof(PedidoRow.NumeroPedido), "order.number");
            SetHeaderText(nameof(PedidoRow.Cliente), "order.client");
            SetHeaderText(nameof(PedidoRow.Estado), "order.state");
            SetHeaderText(nameof(PedidoRow.FechaCreacion), "order.createdAt");
            SetHeaderText(nameof(PedidoRow.FechaEntrega), "order.deadline");
            SetHeaderText(nameof(PedidoRow.Total), "order.summary.total");
            SetHeaderText(nameof(PedidoRow.Facturado), "order.invoiced");
            SetHeaderText(nameof(PedidoRow.SaldoPendiente), "order.summary.balance");
        }

        private void SetHeaderText(string columnName, string resourceKey)
        {
            var column = dgvPedidos.Columns[columnName];
            if (column != null)
            {
                column.HeaderText = resourceKey.Traducir();
            }
        }

        private void CargarEstados()
        {
            _estados = _pedidoService.ObtenerEstadosPedido().OrderBy(e => e.NombreEstadoPedido).ToList();
        }

        private void InicializarFiltros(bool mantenerSeleccion = false)
        {
            if (_estados == null)
                return;

            var estadoSeleccionado = mantenerSeleccion ? ObtenerEstadoFiltro() : (Guid?)null;
            var facturadoSeleccion = mantenerSeleccion ? cmbFacturado.SelectedIndex : 0;
            var saldoSeleccion = mantenerSeleccion ? cmbSaldo.SelectedIndex : 0;

            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("order.filter.all".Traducir());
            foreach (var estado in _estados)
            {
                cmbEstado.Items.Add(new ComboItem(estado.IdEstadoPedido, estado.NombreEstadoPedido));
            }

            var seleccionado = false;
            if (mantenerSeleccion && estadoSeleccionado.HasValue)
            {
                for (var i = 1; i < cmbEstado.Items.Count; i++)
                {
                    if (cmbEstado.Items[i] is ComboItem item && item.Id == estadoSeleccionado.Value)
                    {
                        cmbEstado.SelectedIndex = i;
                        seleccionado = true;
                        break;
                    }
                }
            }
            if (!seleccionado)
            {
                cmbEstado.SelectedIndex = 0;
            }

            cmbFacturado.Items.Clear();
            cmbFacturado.Items.Add("order.filter.all".Traducir());
            cmbFacturado.Items.Add("order.filter.invoiced".Traducir());
            cmbFacturado.Items.Add("order.filter.notInvoiced".Traducir());
            if (mantenerSeleccion && facturadoSeleccion >= 0 && facturadoSeleccion < cmbFacturado.Items.Count)
            {
                cmbFacturado.SelectedIndex = facturadoSeleccion;
            }
            else
            {
                cmbFacturado.SelectedIndex = 0;
            }

            cmbSaldo.Items.Clear();
            cmbSaldo.Items.Add("order.filter.all".Traducir());
            cmbSaldo.Items.Add("order.filter.balancePending".Traducir());
            cmbSaldo.Items.Add("order.filter.balanceClear".Traducir());
            if (mantenerSeleccion && saldoSeleccion >= 0 && saldoSeleccion < cmbSaldo.Items.Count)
            {
                cmbSaldo.SelectedIndex = saldoSeleccion;
            }
            else
            {
                cmbSaldo.SelectedIndex = 0;
            }
        }

        private void BuscarPedidos()
        {
            try
            {
                var filtro = new PedidoFiltro
                {
                    NumeroPedido = string.IsNullOrWhiteSpace(txtBuscar.Text) ? null : txtBuscar.Text.Trim(),
                    IdEstado = ObtenerEstadoFiltro(),
                    Facturado = ObtenerFacturadoFiltro(),
                    ConSaldoPendiente = ObtenerSaldoFiltro()
                };

                var pedidos = _pedidoService.ObtenerPedidos(filtro).ToList();
                _rows.Clear();

                foreach (var pedido in pedidos)
                {
                    _rows.Add(new PedidoRow
                    {
                        IdPedido = pedido.IdPedido,
                        NumeroPedido = pedido.NumeroPedido,
                        Cliente = pedido.Cliente?.RazonSocial,
                        Estado = pedido.EstadoPedido?.NombreEstadoPedido ?? _estados.FirstOrDefault(e => e.IdEstadoPedido == pedido.IdEstadoPedido)?.NombreEstadoPedido,
                        FechaCreacion = pedido.FechaCreacion,
                        FechaEntrega = pedido.FechaLimiteEntrega,
                        Total = pedido.TotalConIva,
                        Facturado = pedido.Facturado,
                        SaldoPendiente = pedido.SaldoPendiente
                    });
                }

                tslResumen.Text = "order.list.summary".Traducir(_rows.Count);
            }
            catch (Exception ex)
            {
                _logService.LogError("Error buscando pedidos / Error listing orders", ex, "Pedidos", SessionContext.NombreUsuario);
                MessageBox.Show("order.list.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Guid? ObtenerEstadoFiltro()
        {
            if (cmbEstado.SelectedItem is ComboItem item)
            {
                return item.Id;
            }

            return null;
        }

        private bool? ObtenerFacturadoFiltro()
        {
            switch (cmbFacturado.SelectedIndex)
            {
                case 1: return true;
                case 2: return false;
                default: return null;
            }
        }

        private bool? ObtenerSaldoFiltro()
        {
            switch (cmbSaldo.SelectedIndex)
            {
                case 1: return true;
                case 2: return false;
                default: return null;
            }
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            AbrirFormularioPedido(null);
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            var row = ObtenerFilaSeleccionada();
            if (row == null)
                return;

            AbrirFormularioPedido(row.IdPedido);
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            BuscarPedidos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarPedidos();
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarPedidos();
                e.SuppressKeyPress = true;
            }
        }

        private void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuscarPedidos();
        }

        private void cmbFacturado_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuscarPedidos();
        }

        private void cmbSaldo_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuscarPedidos();
        }

        private void dgvPedidos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = ObtenerFilaSeleccionada();
                if (row != null)
                {
                    AbrirFormularioPedido(row.IdPedido);
                }
            }
        }

        private PedidoRow ObtenerFilaSeleccionada()
        {
            return bindingSource.Current as PedidoRow;
        }

        private void AbrirFormularioPedido(Guid? idPedido)
        {
            try
            {
                using (var form = new PedidoForm(
                    _pedidoService,
                    _clienteService,
                    _productoService,
                    _categoriaService,
                    _proveedorService,
                    _bitacoraService,
                    _logService,
                    idPedido))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        BuscarPedidos();
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error abriendo formulario de pedidos / Error opening order form", ex, "Pedidos", SessionContext.NombreUsuario);
                MessageBox.Show("order.form.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private sealed class ComboItem
        {
            public ComboItem(Guid id, string texto)
            {
                Id = id;
                Texto = texto;
            }

            public Guid Id { get; }
            public string Texto { get; }
            public override string ToString() => Texto;
        }
    }
}