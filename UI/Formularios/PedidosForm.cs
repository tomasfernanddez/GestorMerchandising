using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;
using BLL.Interfaces;
using Services.BLL.Interfaces;
using UI.Localization;
using UI.ViewModels;
using DomainModel;
using DomainModel.Entidades;
using UI.Helpers;

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
        private bool _suspendSearch;
        private Guid? _estadoCanceladoId;

        private sealed class PedidoRow
        {
            public Guid IdPedido { get; set; }
            public string NumeroPedido { get; set; }
            public string Cliente { get; set; }
            public string Estado { get; set; }
            public Guid? IdEstado { get; set; }
            public DateTime FechaCreacion { get; set; }
            public DateTime? FechaEntrega { get; set; }
            public int CantidadProductos { get; set; }
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
            ActualizarAccionesCancelacion();
            _suspendSearch = true;
            InicializarFiltros();
            _suspendSearch = false;
            txtBuscar.TextChanged += (s, args) => { if (!_suspendSearch) BuscarPedidos(); };
            BuscarPedidos();
        }

        private void ApplyTexts()
        {
            Text = "order.list.title".Traducir();
            tsbNuevo.Text = "order.list.new".Traducir();
            tsbEditar.Text = "order.list.edit".Traducir();
            tsbActualizar.Text = "form.refresh".Traducir();
            tsbCancelarPedido.Text = "order.cancel.button".Traducir();
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
                DataPropertyName = nameof(PedidoRow.CantidadProductos),
                Name = nameof(PedidoRow.CantidadProductos),
                HeaderText = "order.list.items".Traducir(),
                FillWeight = 90,
                MinimumWidth = 90
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
            bindingSource.CurrentChanged += (s, e) => ActualizarAccionesCancelacion();
        }

        private void ActualizarEncabezadosGrid()
        {
            SetHeaderText(nameof(PedidoRow.NumeroPedido), "order.number");
            SetHeaderText(nameof(PedidoRow.Cliente), "order.client");
            SetHeaderText(nameof(PedidoRow.Estado), "order.state");
            SetHeaderText(nameof(PedidoRow.CantidadProductos), "order.list.items");
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

        private void ActualizarAccionesCancelacion()
        {
            if (tsbCancelarPedido == null)
                return;

            var habilitado = _estadoCanceladoId.HasValue;
            if (habilitado)
            {
                var row = ObtenerFilaSeleccionada();
                if (row == null || (row.IdEstado.HasValue && _estadoCanceladoId.HasValue && row.IdEstado.Value == _estadoCanceladoId.Value))
                {
                    habilitado = false;
                }
            }

            tsbCancelarPedido.Enabled = habilitado;
        }

        private static bool EsEstadoCancelado(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var compare = CultureInfo.InvariantCulture.CompareInfo;
            return compare.IndexOf(nombre, "cancelado", CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0
                   || compare.IndexOf(nombre, "cancelled", CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }

        private void CargarEstados()
        {
            _estados = _pedidoService.ObtenerEstadosPedido().OrderBy(e => e.NombreEstadoPedido).ToList();
            var cancelado = _estados.FirstOrDefault(e => EsEstadoCancelado(e.NombreEstadoPedido));
            _estadoCanceladoId = cancelado?.IdEstadoPedido;
        }

        private void InicializarFiltros(bool mantenerSeleccion = false)
        {
            if (_estados == null)
                return;

            var estadoIndice = mantenerSeleccion ? cmbEstado.SelectedIndex : 1;
            var facturadoIndice = mantenerSeleccion ? cmbFacturado.SelectedIndex : 1;
            var saldoIndice = mantenerSeleccion ? cmbSaldo.SelectedIndex : 1;

            var previousSuspend = _suspendSearch;
            _suspendSearch = true;

            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("form.select.optional".Traducir());
            cmbEstado.Items.Add("order.filter.all".Traducir());
            foreach (var estado in _estados)
            {
                cmbEstado.Items.Add(new ComboItem(estado.IdEstadoPedido, estado.NombreEstadoPedido));
            }

           if (estadoIndice >= 0 && estadoIndice < cmbEstado.Items.Count)
                cmbEstado.SelectedIndex = estadoIndice;
            else
                cmbEstado.SelectedIndex = Math.Min(1, cmbEstado.Items.Count - 1);

            cmbFacturado.Items.Clear();
            cmbFacturado.Items.Add("form.select.optional".Traducir());
            cmbFacturado.Items.Add("order.filter.all".Traducir());
            cmbFacturado.Items.Add("order.filter.invoiced".Traducir());
            cmbFacturado.Items.Add("order.filter.notInvoiced".Traducir());
            if (facturadoIndice >= 0 && facturadoIndice < cmbFacturado.Items.Count)
                cmbFacturado.SelectedIndex = facturadoIndice;
            else
                cmbFacturado.SelectedIndex = 1;

            cmbSaldo.Items.Clear();
            cmbSaldo.Items.Add("form.select.optional".Traducir());
            cmbSaldo.Items.Add("order.filter.all".Traducir());
            cmbSaldo.Items.Add("order.filter.balancePending".Traducir());
            cmbSaldo.Items.Add("order.filter.balanceClear".Traducir());
            if (saldoIndice >= 0 && saldoIndice < cmbSaldo.Items.Count)
                cmbSaldo.SelectedIndex = saldoIndice;
            else
                cmbSaldo.SelectedIndex = 1;

            _suspendSearch = previousSuspend;
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
                    ConSaldoPendiente = ObtenerSaldoFiltro(),
                    IncluirDetalles = true
                };

                var pedidos = _pedidoService.ObtenerPedidos(filtro).ToList();
                var mostrarSoloCancelados = _estadoCanceladoId.HasValue
                    && filtro.IdEstado.HasValue
                    && filtro.IdEstado.Value == _estadoCanceladoId.Value;

                if (!mostrarSoloCancelados && _estadoCanceladoId.HasValue)
                {
                    pedidos = pedidos
                        .Where(p => p.IdEstadoPedido != _estadoCanceladoId.Value)
                        .ToList();
                }

                _rows.Clear();

                foreach (var pedido in pedidos)
                {
                    var cantidadProductos = pedido.Detalles?.Count ?? 0;
                    _rows.Add(new PedidoRow
                    {
                        IdPedido = pedido.IdPedido,
                        NumeroPedido = pedido.NumeroPedido,
                        Cliente = FormatearNombreCliente(pedido.Cliente),
                        Estado = pedido.EstadoPedido?.NombreEstadoPedido ?? _estados.FirstOrDefault(e => e.IdEstadoPedido == pedido.IdEstadoPedido)?.NombreEstadoPedido,
                        IdEstado = pedido.IdEstadoPedido,
                        FechaCreacion = ArgentinaDateTimeHelper.ToArgentina(pedido.FechaCreacion),
                        FechaEntrega = ArgentinaDateTimeHelper.ToArgentina(pedido.FechaLimiteEntrega),
                        CantidadProductos = cantidadProductos,
                        Total = pedido.TotalConIva,
                        Facturado = pedido.Facturado,
                        SaldoPendiente = pedido.SaldoPendiente
                    });
                }

                tslResumen.Text = "order.list.summary".Traducir(_rows.Count);
                ActualizarAccionesCancelacion();
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
                case 2: return true;
                case 3: return false;
                default: return null;
            }
        }

        private bool? ObtenerSaldoFiltro()
        {
            switch (cmbSaldo.SelectedIndex)
            {
                case 2: return true;
                case 3: return false;
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

        private void tsbCancelarPedido_Click(object sender, EventArgs e)
        {
            var row = ObtenerFilaSeleccionada();
            if (row == null || !_estadoCanceladoId.HasValue)
                return;

            var confirmacion = MessageBox.Show(
                string.Format("order.cancel.confirm".Traducir(), row.NumeroPedido),
                Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                var usuario = SessionContext.NombreUsuario ?? "Sistema";
                var comentario = $"Pedido cancelado por {usuario} / Order cancelled by {usuario}";
                var resultado = _pedidoService.CancelarPedido(row.IdPedido, usuario, comentario);
                if (!resultado.EsValido)
                {
                    MessageBox.Show("order.cancel.error".Traducir(resultado.Mensaje), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var mensaje = $"Pedido cancelado / Order cancelled: {row.NumeroPedido}";
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, "Pedido.Cancelar", mensaje, "Pedidos");
                _logService.LogInfo(mensaje, "Pedidos", SessionContext.NombreUsuario);

                MessageBox.Show("order.cancel.success".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                BuscarPedidos();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cancelando pedido / Error cancelling order", ex, "Pedidos", SessionContext.NombreUsuario);
                MessageBox.Show("order.cancel.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            if (!_suspendSearch)
                BuscarPedidos();
        }

        private void cmbFacturado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_suspendSearch)
                BuscarPedidos();
        }

        private void cmbSaldo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_suspendSearch)
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
                    idPedido,
                    abrirEnProductos: idPedido.HasValue))
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

        private static string FormatearNombreCliente(Cliente cliente)
        {
            if (cliente == null)
                return string.Empty;

            var razon = cliente.RazonSocial?.Trim();
            var alias = cliente.Alias?.Trim();

            if (!string.IsNullOrWhiteSpace(razon) && !string.IsNullOrWhiteSpace(alias))
                return $"{razon} ({alias})";

            if (!string.IsNullOrWhiteSpace(razon))
                return razon;

            if (!string.IsNullOrWhiteSpace(alias))
                return alias;

            return string.Empty;
        }
    }
}