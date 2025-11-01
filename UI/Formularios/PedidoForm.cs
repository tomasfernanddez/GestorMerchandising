using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using BLL.Helpers;
using BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using Services.BLL.Interfaces;
using UI.Localization;
using UI.ViewModels;
using UI.Helpers;

namespace UI
{
    public partial class PedidoForm : Form
    {
        private readonly IPedidoService _pedidoService;
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly ICategoriaProductoService _categoriaService;
        private readonly IProveedorService _proveedorService;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogService _logService;
        private readonly Guid? _pedidoId;
        private readonly bool _abrirEnProductos;

        private Pedido _pedidoOriginal;
        private BindingList<PedidoDetalleViewModel> _detalles;
        private List<PedidoNota> _notas;
        private List<PedidoEstadoHistorial> _historial;

        private List<Cliente> _clientes;
        private List<CategoriaProducto> _categorias;
        private List<Proveedor> _proveedoresProductos;
        private List<Proveedor> _proveedoresPersonalizacion;
        private List<EstadoPedido> _estadosPedido;
        private List<EstadoProducto> _estadosProducto;
        private List<TecnicaPersonalizacion> _tecnicas;
        private List<UbicacionLogo> _ubicaciones;
        private List<TipoPago> _tiposPago;
        private decimal _montoPagadoBase;
        private decimal _montoPagadoActual;
        private readonly List<PagoRegistrado> _pagosRegistrados = new List<PagoRegistrado>();
        private ContextMenuStrip _contextMenuEstados;
        private Guid? _estadoCanceladoId;
        private bool _pedidoCancelado;
        private Guid? _estadoPedidoActual;

        private sealed class PagoRegistrado
        {
            public PagoRegistrado(decimal monto, decimal? porcentaje)
            {
                Monto = monto;
                Porcentaje = porcentaje.HasValue && porcentaje.Value > 0
                    ? Math.Round(porcentaje.Value, 2)
                    : (decimal?)null;
                Fecha = ArgentinaDateTimeHelper.Now();
            }

            public decimal Monto { get; }
            public decimal? Porcentaje { get; }
            public DateTime Fecha { get; }

            public string ObtenerDescripcion()
            {
                var fechaTexto = Fecha.ToString("g");
                string detalle;
                if (Porcentaje.HasValue)
                {
                    detalle = $"{Porcentaje.Value:0.##}% - {Monto.ToString("C2")}";
                }
                else
                {
                    detalle = $"Monto personalizado / Custom amount - {Monto.ToString("C2")}";
                }
                return string.Format("order.payment.entry".Traducir(), fechaTexto, detalle);
            }

            public override string ToString() => ObtenerDescripcion();
        }

        private sealed class PagoOpcion
        {
            public PagoOpcion(decimal porcentaje, decimal monto)
            {
                Porcentaje = porcentaje;
                Monto = monto;
            }

            public decimal Porcentaje { get; }
            public decimal Monto { get; }
        }

        public PedidoForm(
            IPedidoService pedidoService,
            IClienteService clienteService,
            IProductoService productoService,
            ICategoriaProductoService categoriaService,
            IProveedorService proveedorService,
            IBitacoraService bitacoraService,
            ILogService logService,
            Guid? pedidoId = null,
            bool abrirEnProductos = false)
        {
            _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _categoriaService = categoriaService ?? throw new ArgumentNullException(nameof(categoriaService));
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _pedidoId = pedidoId;
            _abrirEnProductos = abrirEnProductos;

            InitializeComponent();

            btnAgregarPago.Click += btnAgregarPago_Click;
            btnDeshacerPago.Click += btnCancelarPago_Click;
            btnCancelarPedido.Click += btnCancelarPedido_Click;
        }

        private void PedidoForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarDatosReferencia();
            ConfigurarGrillaDetalles();

            if (_pedidoId.HasValue)
            {
                CargarPedidoExistente(_pedidoId.Value);
            }
            else
            {
                IniciarPedidoNuevo();
            }

            RefrescarNotas();
            RefrescarHistorial();
            ActualizarResumen();

            if (_abrirEnProductos)
            {
                tabControl.SelectedTab = tabDetalles;
            }
        }

        private void ApplyTexts()
        {
            Text = _pedidoId.HasValue ? "order.edit.title".Traducir() : "order.new.title".Traducir();
            tabGeneral.Text = "order.tab.general".Traducir();
            tabDetalles.Text = "order.tab.details".Traducir();
            tabNotas.Text = "order.tab.tracking".Traducir();

            lblNumeroPedido.Text = "order.number".Traducir();
            lblCliente.Text = "order.client".Traducir();
            lblTipoPago.Text = "order.paymentType".Traducir();
            lblEstadoPedido.Text = "order.state".Traducir();
            lblFechaEntrega.Text = "order.deadline".Traducir();
            chkFechaEntrega.Text = "order.deadline.enable".Traducir();
            lblMontoPagado.Text = "order.paidAmount".Traducir();
            lblFacturado.Text = "order.invoiced".Traducir();
            btnSeleccionarFactura.Text = "order.invoice.select".Traducir();
            lblOC.Text = "order.purchaseOrder".Traducir();
            lblContacto.Text = "order.contact".Traducir();
            lblEmail.Text = "order.contact.email".Traducir();
            lblTelefono.Text = "order.contact.phone".Traducir();
            lblDireccionEntrega.Text = "order.contact.address".Traducir();
            lblNumeroRemito.Text = "order.deliveryNotes".Traducir();
            lblObservaciones.Text = "order.notes".Traducir();

            btnAgregarDetalle.Text = "order.detail.add".Traducir();
            btnEditarDetalle.Text = "order.detail.edit".Traducir();
            btnEliminarDetalle.Text = "order.detail.delete".Traducir();
            lblTotalSinIva.Text = "order.summary.net".Traducir();
            lblMontoIva.Text = "order.summary.tax".Traducir();
            lblTotalConIva.Text = "order.summary.total".Traducir();
            lblSaldoPendiente.Text = "order.summary.balance".Traducir();
            btnAgregarPago.Text = "order.payment.addPercent".Traducir();
            btnDeshacerPago.Text = "order.payment.cancel".Traducir();

            gbHistorialEstados.Text = "order.timeline".Traducir();
            columnFecha.Text = "order.timeline.date".Traducir();
            columnEstado.Text = "order.timeline.state".Traducir();
            columnComentario.Text = "order.timeline.comment".Traducir();
            gbNotas.Text = "order.internalNotes".Traducir();
            btnAgregarNota.Text = "order.note.add".Traducir();

            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
            btnCancelarPedido.Text = "order.cancel.button".Traducir();

            if (_clientes != null)
            {
                ConfigurarCombosGenerales(true);
            }

            if (dgvDetalles.Columns.Count > 0)
            {
                ActualizarEncabezadosDetalles();
            }
        }

        private void CargarDatosReferencia()
        {
            try
            {
                _clientes = _clienteService.ObtenerClientesActivos().OrderBy(c => c.RazonSocial).ToList();
                _categorias = _categoriaService.ObtenerTodas().Where(c => c.Activo).OrderBy(c => c.NombreCategoria).ToList();
                var proveedoresActivos = _proveedorService.ObtenerProveedoresActivos().OrderBy(p => p.RazonSocial).ToList();
                _proveedoresProductos = proveedoresActivos
                    .Where(EsProveedorProducto)
                    .OrderBy(p => p.RazonSocial)
                    .ToList();
                _proveedoresPersonalizacion = proveedoresActivos
                    .Where(EsProveedorPersonalizador)
                    .OrderBy(p => p.RazonSocial)
                    .ToList();

                _estadosPedido = _pedidoService.ObtenerEstadosPedido().OrderBy(e => e.NombreEstadoPedido).ToList();
                var estadoCancelado = _estadosPedido.FirstOrDefault(e => EsEstadoCancelado(e.NombreEstadoPedido));
                _estadoCanceladoId = estadoCancelado?.IdEstadoPedido;
                _tiposPago = _pedidoService.ObtenerTiposPago().OrderBy(t => t.NombreTipoPago).ToList();

                _estadosProducto = _pedidoService.ObtenerEstadosProducto().OrderBy(e => e.NombreEstadoProducto).ToList();
                _tecnicas = _pedidoService.ObtenerTecnicasPersonalizacion().OrderBy(t => t.NombreTecnicaPersonalizacion).ToList();
                _ubicaciones = _pedidoService.ObtenerUbicacionesLogo().OrderBy(u => u.NombreUbicacionLogo).ToList();

                ConfigurarCombosGenerales();
                if (_contextMenuEstados != null)
                    ConfigurarMenuContextualEstados();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cargando datos de referencia para pedidos / Error loading order reference data", ex, "Pedidos", SessionContext.NombreUsuario);
                MessageBox.Show("order.loadReferences.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void ConfigurarCombosGenerales(bool mantenerSeleccion = false)
        {
            var clienteSeleccionado = mantenerSeleccion && cmbCliente.SelectedValue is Guid clienteId && clienteId != Guid.Empty
                ? (Guid?)clienteId
                : null;
            var tipoPagoSeleccionado = mantenerSeleccion && cmbTipoPago.SelectedValue is Guid pagoId && pagoId != Guid.Empty
                ? (Guid?)pagoId
                : null;
            cmbCliente.DataSource = null;
            var clientes = new List<Cliente>
            {
                new Cliente { IdCliente = Guid.Empty, RazonSocial = "form.select.optional".Traducir() }
            };
            clientes.AddRange(_clientes);
            cmbCliente.DisplayMember = nameof(Cliente.RazonSocial);
            cmbCliente.ValueMember = nameof(Cliente.IdCliente);
            cmbCliente.DataSource = clientes;
            if (mantenerSeleccion && clienteSeleccionado.HasValue)
                cmbCliente.SelectedValue = clienteSeleccionado.Value;
            else
                cmbCliente.SelectedIndex = 0;

            cmbTipoPago.DataSource = null;
            var tiposPago = new List<TipoPago>
            {
                new TipoPago { IdTipoPago = Guid.Empty, NombreTipoPago = "form.select.optional".Traducir() }
            };
            tiposPago.AddRange(_tiposPago);
            cmbTipoPago.DisplayMember = nameof(TipoPago.NombreTipoPago);
            cmbTipoPago.ValueMember = nameof(TipoPago.IdTipoPago);
            cmbTipoPago.DataSource = tiposPago;
            if (mantenerSeleccion && tipoPagoSeleccionado.HasValue)
                cmbTipoPago.SelectedValue = tipoPagoSeleccionado.Value;
            else
                cmbTipoPago.SelectedIndex = 0;

            cmbEstadoPedido.DataSource = null;
            var estados = new List<EstadoPedido>();

            var estadosDisponibles = _estadosPedido?
                .Where(e => !_estadoCanceladoId.HasValue
                             || e.IdEstadoPedido != _estadoCanceladoId.Value
                             || (_pedidoCancelado && e.IdEstadoPedido == _estadoCanceladoId.Value))
                .ToList() ?? new List<EstadoPedido>();

            estados.AddRange(estadosDisponibles);
            cmbEstadoPedido.DisplayMember = nameof(EstadoPedido.NombreEstadoPedido);
            cmbEstadoPedido.ValueMember = nameof(EstadoPedido.IdEstadoPedido);
            cmbEstadoPedido.DataSource = estados;

            cmbEstadoPedido.Enabled = false;
            ActualizarAccionesCancelacion();
            ActualizarEstadoPedidoAutomatico();
        }

        private bool EsProveedorProducto(Proveedor proveedor)
        {
            return proveedor?.TiposProveedor != null && proveedor.TiposProveedor.Any(ProveedorCatalogoHelper.EsTipoProducto);
        }

        private bool EsProveedorPersonalizador(Proveedor proveedor)
        {
            return proveedor?.TiposProveedor != null && proveedor.TiposProveedor.Any(ProveedorCatalogoHelper.EsTipoPersonalizador);
        }

        private void ActualizarAccionesCancelacion()
        {
            if (btnCancelarPedido == null)
                return;

            var visible = _pedidoId.HasValue;
            var habilitado = visible && !_pedidoCancelado && _estadoCanceladoId.HasValue;

            btnCancelarPedido.Visible = visible;
            btnCancelarPedido.Enabled = habilitado;
        }

        private static bool EsEstadoCancelado(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var compare = CultureInfo.InvariantCulture.CompareInfo;
            return compare.IndexOf(nombre, "cancelado", CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0
                || compare.IndexOf(nombre, "cancelled", CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }

        private void ConfigurarGrillaDetalles()
        {
            _detalles = new BindingList<PedidoDetalleViewModel>();
            dgvDetalles.AutoGenerateColumns = false;
            dgvDetalles.Columns.Clear();

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoDetalleViewModel.NombreProducto),
                Name = nameof(PedidoDetalleViewModel.NombreProducto),
                HeaderText = "order.detail.product".Traducir(),
                FillWeight = 220,
                MinimumWidth = 160
            });
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoDetalleViewModel.Proveedor),
                Name = nameof(PedidoDetalleViewModel.Proveedor),
                HeaderText = "order.detail.provider".Traducir(),
                FillWeight = 150,
                MinimumWidth = 120
            });
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoDetalleViewModel.Cantidad),
                Name = nameof(PedidoDetalleViewModel.Cantidad),
                HeaderText = "order.detail.quantity".Traducir(),
                FillWeight = 70,
                MinimumWidth = 70
            });
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoDetalleViewModel.PrecioUnitario),
                Name = nameof(PedidoDetalleViewModel.PrecioUnitario),
                HeaderText = "order.detail.price".Traducir(),
                FillWeight = 90,
                MinimumWidth = 80,
                DefaultCellStyle = { Format = "N2" }
            });
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoDetalleViewModel.EstadoProducto),
                Name = nameof(PedidoDetalleViewModel.EstadoProducto),
                HeaderText = "order.detail.state".Traducir(),
                FillWeight = 100,
                MinimumWidth = 100
            });
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoDetalleViewModel.FechaLimite),
                Name = nameof(PedidoDetalleViewModel.FechaLimite),
                HeaderText = "order.detail.deadline".Traducir(),
                FillWeight = 90,
                MinimumWidth = 90,
                DefaultCellStyle = { Format = "d" }
            });
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoDetalleViewModel.CantidadLogos),
                Name = nameof(PedidoDetalleViewModel.CantidadLogos),
                HeaderText = "order.detail.logo.count".Traducir(),
                FillWeight = 80,
                MinimumWidth = 80
            });
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoDetalleViewModel.ProveedorPersonalizacion),
                Name = nameof(PedidoDetalleViewModel.ProveedorPersonalizacion),
                HeaderText = "order.detail.provider.personalization".Traducir(),
                FillWeight = 160,
                MinimumWidth = 140
            });

            dgvDetalles.DataSource = _detalles;
            dgvDetalles.CellDoubleClick += dgvDetalles_CellDoubleClick;
            dgvDetalles.CellMouseDown += DgvDetalles_CellMouseDown;

            if (_contextMenuEstados == null)
            {
                _contextMenuEstados = new ContextMenuStrip();
                _contextMenuEstados.Opening += ContextMenuEstados_Opening;
                dgvDetalles.ContextMenuStrip = _contextMenuEstados;
            }

            ConfigurarMenuContextualEstados();
        }

        private void ActualizarEncabezadosDetalles()
        {
            SetDetalleHeader(nameof(PedidoDetalleViewModel.NombreProducto), "order.detail.product");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.Proveedor), "order.detail.provider");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.Cantidad), "order.detail.quantity");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.PrecioUnitario), "order.detail.price");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.EstadoProducto), "order.detail.state");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.FechaLimite), "order.detail.deadline");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.CantidadLogos), "order.detail.logo.count");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.ProveedorPersonalizacion), "order.detail.provider.personalization");
        }

        private void ConfigurarMenuContextualEstados()
        {
            if (_contextMenuEstados == null)
                return;

            _contextMenuEstados.Items.Clear();

            if (_estadosProducto == null)
                return;

            foreach (var estado in _estadosProducto)
            {
                var item = new ToolStripMenuItem(estado.NombreEstadoProducto)
                {
                    Tag = estado
                };
                item.Click += ContextMenuEstado_Click;
                _contextMenuEstados.Items.Add(item);
            }
        }

        private void SetDetalleHeader(string columnName, string resourceKey)
        {
            var column = dgvDetalles.Columns[columnName];
            if (column != null)
            {
                column.HeaderText = resourceKey.Traducir();
            }
        }

        private void dgvDetalles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var detalle = ObtenerDetalleSeleccionado();
            if (detalle != null)
            {
                AbrirDetalle(detalle);
            }
        }

        private void DgvDetalles_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvDetalles.ClearSelection();
                dgvDetalles.Rows[e.RowIndex].Selected = true;
                var columnIndex = e.ColumnIndex >= 0 ? e.ColumnIndex : 0;
                dgvDetalles.CurrentCell = dgvDetalles.Rows[e.RowIndex].Cells[columnIndex];
            }
        }

        private void ContextMenuEstados_Opening(object sender, CancelEventArgs e)
        {
            var detalle = ObtenerDetalleSeleccionado();
            if (detalle == null || _contextMenuEstados == null || _contextMenuEstados.Items.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            foreach (var item in _contextMenuEstados.Items.OfType<ToolStripMenuItem>())
            {
                if (item.Tag is EstadoProducto estado)
                {
                    var idDetalle = detalle.IdEstadoProducto ?? Guid.Empty;
                    item.Checked = idDetalle != Guid.Empty && idDetalle == estado.IdEstadoProducto;
                }
            }
        }

        private void ContextMenuEstado_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is EstadoProducto estado)
            {
                var detalle = ObtenerDetalleSeleccionado();
                if (detalle == null)
                    return;

                if (detalle.IdEstadoProducto.HasValue && detalle.IdEstadoProducto.Value == estado.IdEstadoProducto)
                    return;

                CambiarEstadoDetalle(detalle, estado);
            }
        }

        private void CargarPedidoExistente(Guid idPedido)
        {
            try
            {
                _pedidoOriginal = _pedidoService.ObtenerPedido(idPedido, incluirDetalles: true);
                if (_pedidoOriginal == null)
                {
                    MessageBox.Show("order.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                _estadoPedidoActual = _pedidoOriginal.IdEstadoPedido;
                _pedidoCancelado = _estadoCanceladoId.HasValue
                    && _pedidoOriginal.IdEstadoPedido.HasValue
                    && _pedidoOriginal.IdEstadoPedido.Value == _estadoCanceladoId.Value;
                ConfigurarCombosGenerales(true);

                txtNumeroPedido.Text = _pedidoOriginal.NumeroPedido;
                cmbCliente.SelectedValue = _pedidoOriginal.IdCliente;
                if (_pedidoOriginal.IdTipoPago.HasValue)
                    cmbTipoPago.SelectedValue = _pedidoOriginal.IdTipoPago.Value;
                if (_pedidoOriginal.IdEstadoPedido.HasValue)
                    cmbEstadoPedido.SelectedValue = _pedidoOriginal.IdEstadoPedido.Value;

                if (_pedidoOriginal.FechaLimiteEntrega.HasValue)
                {
                    chkFechaEntrega.Checked = true;
                    dtpFechaEntrega.Value = ArgentinaDateTimeHelper.ToArgentina(_pedidoOriginal.FechaLimiteEntrega.Value);
                }

                _montoPagadoBase = _pedidoOriginal.MontoPagado;
                _montoPagadoActual = _montoPagadoBase;
                _pagosRegistrados.Clear();
                ActualizarMontoPagadoUI();
                chkFacturado.Checked = _pedidoOriginal.Facturado;
                txtFactura.Text = _pedidoOriginal.RutaFacturaPdf;

                txtOC.Text = _pedidoOriginal.Cliente_OC;
                txtContacto.Text = _pedidoOriginal.Cliente_PersonaNombre;
                txtEmail.Text = _pedidoOriginal.Cliente_PersonaEmail;
                txtTelefono.Text = _pedidoOriginal.Cliente_PersonaTelefono;
                txtDireccionEntrega.Text = _pedidoOriginal.Cliente_DireccionEntrega;
                txtNumeroRemito.Text = _pedidoOriginal.NumeroRemito;
                txtObservaciones.Text = _pedidoOriginal.Observaciones;

                _detalles = new BindingList<PedidoDetalleViewModel>(
                    _pedidoOriginal.Detalles.Select(MapearDetalle).ToList());
                dgvDetalles.DataSource = _detalles;

                _notas = _pedidoOriginal.Notas?.OrderBy(n => n.Fecha).Select(CloneNota).ToList() ?? new List<PedidoNota>();
                _historial = _pedidoOriginal.HistorialEstados?.OrderBy(h => h.FechaCambio).Select(CloneHistorial).ToList() ?? new List<PedidoEstadoHistorial>();
                
                ActualizarAccionesCancelacion();
                ActualizarEstadoPedidoAutomatico();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cargando pedido existente / Error loading order", ex, "Pedidos", SessionContext.NombreUsuario);
                MessageBox.Show("order.load.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void IniciarPedidoNuevo()
        {
            txtNumeroPedido.Text = _pedidoService.GenerarProximoNumeroPedido();
            _detalles = new BindingList<PedidoDetalleViewModel>();
            dgvDetalles.DataSource = _detalles;
            _notas = new List<PedidoNota>();
            _historial = new List<PedidoEstadoHistorial>();
            _montoPagadoBase = 0m;
            _montoPagadoActual = 0m;
            _pagosRegistrados.Clear();
            ActualizarMontoPagadoUI();
            _pedidoCancelado = false;
            ActualizarAccionesCancelacion();
            ActualizarEstadoPedidoAutomatico();
        }

        private PedidoDetalleViewModel MapearDetalle(PedidoDetalle detalle)
        {
            return new PedidoDetalleViewModel
            {
                IdDetallePedido = detalle.IdDetallePedido,
                IdProducto = detalle.IdProducto,
                NombreProducto = detalle.Producto?.NombreProducto,
                IdCategoria = detalle.Producto?.IdCategoria,
                Categoria = detalle.Producto?.Categoria?.NombreCategoria,
                IdProveedor = detalle.Producto?.IdProveedor,
                Proveedor = detalle.Producto?.Proveedor?.RazonSocial,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                IdEstadoProducto = detalle.IdEstadoProducto,
                EstadoProducto = detalle.EstadoProducto?.NombreEstadoProducto,
                FechaLimite = ArgentinaDateTimeHelper.ToArgentina(detalle.FechaLimiteProduccion),
                FichaAplicacion = detalle.FichaAplicacion,
                Notas = detalle.Notas,
                IdProveedorPersonalizacion = detalle.IdProveedorPersonalizacion,
                ProveedorPersonalizacion = detalle.ProveedorPersonalizacion?.RazonSocial,
                Logos = detalle.LogosPedido?.Select(MapearLogo).ToList() ?? new List<PedidoLogoViewModel>()
            };
        }

        private PedidoLogoViewModel MapearLogo(LogosPedido logo)
        {
            return new PedidoLogoViewModel
            {
                IdLogoPedido = logo.IdLogoPedido,
                IdTecnica = logo.IdTecnicaPersonalizacion,
                Tecnica = logo.TecnicaPersonalizacion?.NombreTecnicaPersonalizacion,
                IdUbicacion = logo.IdUbicacionLogo,
                Ubicacion = logo.UbicacionLogo?.NombreUbicacionLogo,
                IdProveedor = logo.IdProveedor,
                Proveedor = logo.Proveedor?.RazonSocial,
                Cantidad = logo.Cantidad,
                Costo = logo.CostoPersonalizacion,
                Descripcion = logo.Descripcion
            };
        }

        private PedidoNota CloneNota(PedidoNota nota)
        {
            return new PedidoNota
            {
                IdNota = nota.IdNota,
                IdPedido = nota.IdPedido,
                Nota = nota.Nota,
                Fecha = ArgentinaDateTimeHelper.ToArgentina(nota.Fecha),
                Usuario = nota.Usuario
            };
        }

        private PedidoEstadoHistorial CloneHistorial(PedidoEstadoHistorial historial)
        {
            return new PedidoEstadoHistorial
            {
                IdHistorial = historial.IdHistorial,
                IdPedido = historial.IdPedido,
                IdEstadoPedido = historial.IdEstadoPedido,
                EstadoPedido = historial.EstadoPedido,
                Comentario = historial.Comentario,
                FechaCambio = ArgentinaDateTimeHelper.ToArgentina(historial.FechaCambio),
                Usuario = historial.Usuario
            };
        }

        private void RefrescarNotas()
        {
            lstNotas.Items.Clear();
            if (_notas == null) return;

            foreach (var nota in _notas.OrderByDescending(n => n.Fecha))
            {
                var texto = $"{nota.Fecha:g} - {nota.Usuario}: {nota.Nota}";
                lstNotas.Items.Add(texto);
            }
        }

        private void RefrescarHistorial()
        {
            lvHistorialEstados.Items.Clear();
            if (_historial == null) return;

            foreach (var item in _historial.OrderBy(h => h.FechaCambio))
            {
                var estado = item.EstadoPedido?.NombreEstadoPedido ??
                             _estadosPedido.FirstOrDefault(e => e.IdEstadoPedido == item.IdEstadoPedido)?.NombreEstadoPedido ?? "-";
                var fecha = item.FechaCambio.ToString("g");
                var comentario = string.IsNullOrWhiteSpace(item.Comentario)
                    ? string.Format("order.timeline.entry".Traducir(), estado, item.Usuario)
                    : item.Comentario;
                var listItem = new ListViewItem(new[] { fecha, estado, comentario })
                {
                    Tag = item
                };
                lvHistorialEstados.Items.Add(listItem);
            }
        }

        private (decimal totalSinIva, decimal iva, decimal totalConIva, decimal saldoPendiente) CalcularTotales()
        {
            if (_detalles == null)
                return (0m, 0m, 0m, 0m);

            decimal totalProductos = _detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            decimal totalSinIva = Math.Round(totalProductos, 2);
            decimal iva = Math.Round(totalSinIva * 0.21m, 2);
            decimal totalConIva = Math.Round(totalSinIva + iva, 2);
            decimal saldo = Math.Max(0, Math.Round(totalConIva - _montoPagadoActual, 2));

            return (totalSinIva, iva, totalConIva, saldo);
        }

        private void ActualizarResumen()
        {
            var totales = CalcularTotales();

            lblTotalSinIvaValor.Text = totales.totalSinIva.ToString("N2");
            lblMontoIvaValor.Text = totales.iva.ToString("N2");
            lblTotalConIvaValor.Text = totales.totalConIva.ToString("N2");
            lblSaldoPendienteValor.Text = totales.saldoPendiente.ToString("N2");
            lblMontoPagadoValor.Text = _montoPagadoActual.ToString("N2");
            lblMontoPagadoValor.Visible = true;
            btnDeshacerPago.Enabled = _pagosRegistrados.Count > 0;

            ActualizarEstadoPedidoAutomatico();
        }

        private void ActualizarEstadoPedidoAutomatico()
        {
            if (_estadosPedido == null || _estadosPedido.Count == 0)
                return;

            var estadosDetalle = _detalles?
                .Select(d => d.EstadoProducto
                              ?? ObtenerEstadoProducto(d.IdEstadoProducto)?.NombreEstadoProducto
                              ?? string.Empty)
                .ToList() ?? new List<string>();

            var nuevoEstado = PedidoEstadoHelper.CalcularEstadoPedido(estadosDetalle, _estadosPedido);
            if (!nuevoEstado.HasValue || nuevoEstado.Value == Guid.Empty)
                return;

            _estadoPedidoActual = nuevoEstado;

            if (cmbEstadoPedido.DataSource != null)
            {
                var estadoActual = cmbEstadoPedido.SelectedValue is Guid actual ? actual : Guid.Empty;
                if (estadoActual != nuevoEstado.Value)
                {
                    if (cmbEstadoPedido.Items.OfType<object>().Any(item =>
                        item is EstadoPedido estado && estado.IdEstadoPedido == nuevoEstado.Value))
                    {
                        cmbEstadoPedido.SelectedValue = nuevoEstado.Value;
                    }
                }
            }
        }

        private void ActualizarMontoPagadoUI()
        {
            ActualizarResumen();
        }

        private void btnAgregarPago_Click(object sender, EventArgs e)
        {
            if (!TrySeleccionarMontoPago(out var monto, out var porcentaje))
                return;

            var detalle = porcentaje.HasValue
                ? string.Format("order.payment.confirm.percentInfo".Traducir(), porcentaje.Value.ToString("0.##"))
                : "order.payment.confirm.manualInfo".Traducir();
            var mensaje = string.Format("order.payment.confirm".Traducir(), monto.ToString("C2"), detalle);
            if (MessageBox.Show(mensaje, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var registro = new PagoRegistrado(monto, porcentaje);
            _pagosRegistrados.Add(registro);
            _montoPagadoActual = Math.Round(_montoPagadoActual + monto, 2);
            ActualizarMontoPagadoUI();
            RegistrarPagoAgregado(registro);
        }

        private void btnCancelarPago_Click(object sender, EventArgs e)
        {
            if (_pagosRegistrados.Count == 0)
            {
                MessageBox.Show("order.payment.cancel.empty".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!TrySeleccionarPagoParaCancelar(out var pago))
                return;

            var mensaje = string.Format("order.payment.cancel.confirm".Traducir(), pago.Monto);
            if (MessageBox.Show(mensaje, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _pagosRegistrados.Remove(pago);
            _montoPagadoActual = Math.Max(0, Math.Round(_montoPagadoActual - pago.Monto, 2));
            ActualizarMontoPagadoUI();
            RegistrarPagoCancelado(pago);
        }

        private bool TrySeleccionarMontoPago(out decimal monto, out decimal? porcentaje)
        {
            monto = 0m;
            porcentaje = null;

            var (_, _, _, saldoPendiente) = CalcularTotales();
            if (saldoPendiente <= 0)
            {
                MessageBox.Show("order.payment.nonePending".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            using (var dialog = new Form())
            {
                dialog.Text = "order.payment.percent.title".Traducir();
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ShowIcon = false;
                dialog.ClientSize = new Size(420, 280);

                var table = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 7,
                    Padding = new Padding(12)
                };
                for (int i = 0; i < 6; i++)
                {
                    table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                }
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

                var lblSaldo = new Label
                {
                    AutoSize = true,
                    Text = string.Format("order.payment.percent.remaining".Traducir(), saldoPendiente.ToString("C2"))
                };
                table.Controls.Add(lblSaldo, 0, 0);

                var lblHint = new Label
                {
                    AutoSize = true,
                    Margin = new Padding(0, 6, 0, 6),
                    Text = "order.payment.input.hint".Traducir()
                };
                table.Controls.Add(lblHint, 0, 1);

                var panelMonto = new FlowLayoutPanel
                {
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    WrapContents = false
                };
                var lblMonto = new Label { AutoSize = true, Text = "order.payment.amount.label".Traducir(), Margin = new Padding(0, 6, 6, 0) };
                var txtMonto = new TextBox { Width = 140, TextAlign = HorizontalAlignment.Right };
                panelMonto.Controls.Add(lblMonto);
                panelMonto.Controls.Add(txtMonto);
                table.Controls.Add(panelMonto, 0, 2);

                var panelPorcentaje = new FlowLayoutPanel
                {
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    WrapContents = false
                };
                var lblPorcentaje = new Label { AutoSize = true, Text = "order.payment.percent.label".Traducir(), Margin = new Padding(0, 6, 6, 0) };
                var txtPorcentaje = new TextBox { Width = 80, TextAlign = HorizontalAlignment.Right };
                var lblSimbolo = new Label { AutoSize = true, Text = "%", Margin = new Padding(6, 6, 0, 0) };
                panelPorcentaje.Controls.Add(lblPorcentaje);
                panelPorcentaje.Controls.Add(txtPorcentaje);
                panelPorcentaje.Controls.Add(lblSimbolo);
                table.Controls.Add(panelPorcentaje, 0, 3);

                var panelOpciones = new FlowLayoutPanel
                {
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    WrapContents = true
                };
                var lblOpciones = new Label
                {
                    AutoSize = true,
                    Text = "order.payment.quickOptions".Traducir(),
                    Margin = new Padding(0, 6, 6, 0)
                };
                panelOpciones.Controls.Add(lblOpciones);
                foreach (var porcentajeRapido in new[] { 30m, 50m, 100m })
                {
                    var btnOpcion = new Button
                    {
                        AutoSize = true,
                        Text = string.Format("order.payment.quickOption".Traducir(), porcentajeRapido.ToString("0")),
                        Margin = new Padding(0, 3, 6, 3)
                    };
                    btnOpcion.Click += (s, e) =>
                    {
                        txtPorcentaje.Text = porcentajeRapido.ToString("0.##", CultureInfo.CurrentCulture);
                        var montoCalculado = Math.Round(saldoPendiente * (porcentajeRapido / 100m), 2);
                        txtMonto.Text = montoCalculado.ToString("N2");
                        txtPorcentaje.Focus();
                        txtPorcentaje.SelectionStart = txtPorcentaje.Text.Length;
                    };
                    panelOpciones.Controls.Add(btnOpcion);
                }
                table.Controls.Add(panelOpciones, 0, 4);

                var lblResultado = new Label
                {
                    AutoSize = true,
                    Margin = new Padding(0, 6, 0, 6),
                    Text = string.Format("order.payment.preview".Traducir(), "--")
                };
                table.Controls.Add(lblResultado, 0, 5);

                var panelBotones = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Dock = DockStyle.Fill,
                    AutoSize = true
                };
                var btnConfirmar = new Button
                {
                    AutoSize = true,
                    Text = "order.payment.amount.confirmButton".Traducir()
                };
                var btnCancelar = new Button
                {
                    AutoSize = true,
                    Text = "form.cancel".Traducir(),
                    DialogResult = DialogResult.Cancel
                };
                panelBotones.Controls.Add(btnConfirmar);
                panelBotones.Controls.Add(btnCancelar);
                table.Controls.Add(panelBotones, 0, 6);

                dialog.Controls.Add(table);
                dialog.AcceptButton = btnConfirmar;
                dialog.CancelButton = btnCancelar;

                void ActualizarResultado()
                {
                    decimal? preview = null;
                    if (TryParseDecimalFlexible(txtPorcentaje.Text, out var porcentajePreview) && porcentajePreview > 0)
                    {
                        preview = Math.Round(saldoPendiente * (porcentajePreview / 100m), 2);
                    }
                    else if (TryParseDecimalFlexible(txtMonto.Text, out var montoPreview) && montoPreview > 0)
                    {
                        preview = Math.Round(montoPreview, 2);
                    }

                    var texto = preview.HasValue ? preview.Value.ToString("C2") : "--";
                    lblResultado.Text = string.Format("order.payment.preview".Traducir(), texto);
                }

                txtMonto.TextChanged += (s, e) => ActualizarResultado();
                txtPorcentaje.TextChanged += (s, e) => ActualizarResultado();
                ActualizarResultado();

                decimal montoSeleccionado = 0m;
                decimal? porcentajeSeleccionado = null;

                btnConfirmar.Click += (s, e) =>
                {
                    decimal? montoIngresado = null;
                    decimal? porcentajeIngresado = null;

                    if (!string.IsNullOrWhiteSpace(txtMonto.Text))
                    {
                        if (!TryParseDecimalFlexible(txtMonto.Text, out var montoParseado))
                        {
                            MessageBox.Show("order.payment.input.invalid".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        montoIngresado = Math.Round(montoParseado, 2);
                    }

                    if (!string.IsNullOrWhiteSpace(txtPorcentaje.Text))
                    {
                        if (!TryParseDecimalFlexible(txtPorcentaje.Text, out var porcentajeParseado))
                        {
                            MessageBox.Show("order.payment.input.invalid".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        porcentajeIngresado = Math.Round(porcentajeParseado, 2);
                    }

                    if (!montoIngresado.HasValue && !porcentajeIngresado.HasValue)
                    {
                        MessageBox.Show("order.payment.input.required".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (porcentajeIngresado.HasValue)
                    {
                        if (porcentajeIngresado.Value <= 0 || porcentajeIngresado.Value > 100)
                        {
                            MessageBox.Show("order.payment.input.percentRange".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var montoCalculado = Math.Round(saldoPendiente * (porcentajeIngresado.Value / 100m), 2);
                        if (!montoIngresado.HasValue)
                        {
                            montoIngresado = montoCalculado;
                        }
                        else if (Math.Abs(montoIngresado.Value - montoCalculado) > 0.01m)
                        {
                            MessageBox.Show(string.Format("order.payment.input.mismatch".Traducir(), montoCalculado.ToString("C2")), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    if (!montoIngresado.HasValue || montoIngresado.Value <= 0)
                    {
                        MessageBox.Show("order.payment.input.required".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (montoIngresado.Value > saldoPendiente)
                    {
                        MessageBox.Show(string.Format("order.payment.input.exceed".Traducir(), saldoPendiente.ToString("C2")), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!porcentajeIngresado.HasValue && saldoPendiente > 0)
                    {
                        porcentajeIngresado = Math.Round((montoIngresado.Value / saldoPendiente) * 100m, 2);
                    }

                    montoSeleccionado = montoIngresado.Value;
                    porcentajeSeleccionado = porcentajeIngresado;
                    dialog.DialogResult = DialogResult.OK;
                    dialog.Close();
                };

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    monto = montoSeleccionado;
                    porcentaje = porcentajeSeleccionado;
                    return true;
                }
            }

            monto = 0m;
            porcentaje = null;
            return false;
        }
        private bool TrySeleccionarPagoParaCancelar(out PagoRegistrado pago)
        {
            pago = null;

            using (var dialog = new Form())
            {
                dialog.Text = "order.payment.cancel.title".Traducir();
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ShowIcon = false;
                dialog.ClientSize = new Size(380, 260);

                var table = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 3,
                    Padding = new Padding(12)
                };
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var lbl = new Label
                {
                    AutoSize = true,
                    Text = "order.payment.cancel.prompt".Traducir()
                };
                table.Controls.Add(lbl, 0, 0);

                var lista = new ListBox
                {
                    Dock = DockStyle.Fill,
                    SelectionMode = SelectionMode.One
                };
                foreach (var registro in _pagosRegistrados)
                {
                    lista.Items.Add(registro);
                }
                if (lista.Items.Count > 0)
                {
                    lista.SelectedIndex = 0;
                }
                table.Controls.Add(lista, 0, 1);

                var panelBotones = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Dock = DockStyle.Fill,
                    AutoSize = true
                };
                var btnAceptar = new Button
                {
                    Text = "order.payment.cancel.confirmButton".Traducir(),
                    DialogResult = DialogResult.OK,
                    AutoSize = true
                };
                var btnCancelar = new Button
                {
                    Text = "form.cancel".Traducir(),
                    DialogResult = DialogResult.Cancel,
                    AutoSize = true
                };
                panelBotones.Controls.Add(btnAceptar);
                panelBotones.Controls.Add(btnCancelar);
                table.Controls.Add(panelBotones, 0, 2);

                dialog.Controls.Add(table);
                dialog.AcceptButton = btnAceptar;
                dialog.CancelButton = btnCancelar;

                if (dialog.ShowDialog(this) == DialogResult.OK && lista.SelectedItem is PagoRegistrado seleccionado)
                {
                    pago = seleccionado;
                    return true;
                }
            }

            return false;
        }

        private void RegistrarPagoAgregado(PagoRegistrado pago)
        {
            if (pago == null)
                return;

            var detalleEs = pago.Porcentaje.HasValue
                ? $"({pago.Porcentaje.Value:0.##}% saldo)"
                : "(monto personalizado)";
            var detalleEn = pago.Porcentaje.HasValue
                ? $"({pago.Porcentaje.Value:0.##}% balance)"
                : "(custom amount)";
            var mensaje = $"Pago registrado {detalleEs} / Payment registered {detalleEn}: {pago.Monto.ToString("C2")}";
            RegistrarMovimientoPago("Pedido.Pago.Agregar", mensaje);
        }

        private void RegistrarPagoCancelado(PagoRegistrado pago)
        {
            if (pago == null)
                return;

            var detalleEs = pago.Porcentaje.HasValue
                ? $"({pago.Porcentaje.Value:0.##}% saldo)"
                : "(monto personalizado)";
            var detalleEn = pago.Porcentaje.HasValue
                ? $"({pago.Porcentaje.Value:0.##}% balance)"
                : "(custom amount)";
            var mensaje = $"Pago cancelado {detalleEs} / Payment cancelled {detalleEn}: {pago.Monto.ToString("C2")}";
            RegistrarMovimientoPago("Pedido.Pago.Cancelar", mensaje);
        }

        private void RegistrarMovimientoPago(string accion, string mensaje)
        {
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Pedidos");
                _logService.LogInfo(mensaje, "Pedidos", SessionContext.NombreUsuario);
            }
            catch
            {
                // Evitar que fallos en registro de auditoría interrumpan el flujo de pagos.
            }
        }

        private void chkFechaEntrega_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaEntrega.Enabled = chkFechaEntrega.Checked;
        }

        private void chkFacturado_CheckedChanged(object sender, EventArgs e)
        {
            var habilitado = chkFacturado.Checked;
            txtFactura.Enabled = habilitado;
            btnSeleccionarFactura.Enabled = habilitado;
        }

        private void btnSeleccionarFactura_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "PDF|*.pdf";
                dialog.Title = "order.invoice.select.title".Traducir();
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtFactura.Text = dialog.FileName;
                }
            }
        }

        private void btnAgregarDetalle_Click(object sender, EventArgs e)
        {
            AbrirDetalle(null);
        }

        private void btnEditarDetalle_Click(object sender, EventArgs e)
        {
            var detalle = ObtenerDetalleSeleccionado();
            if (detalle != null)
            {
                AbrirDetalle(detalle);
            }
        }

        private void btnEliminarDetalle_Click(object sender, EventArgs e)
        {
            var detalle = ObtenerDetalleSeleccionado();
            if (detalle == null)
                return;

            if (MessageBox.Show("order.detail.delete.confirm".Traducir(), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _detalles.Remove(detalle);
                ActualizarResumen();
            }
        }

        private PedidoDetalleViewModel ObtenerDetalleSeleccionado()
        {
            return dgvDetalles.CurrentRow?.DataBoundItem as PedidoDetalleViewModel;
        }

        private void CambiarEstadoDetalle(PedidoDetalleViewModel detalle, EstadoProducto nuevoEstado)
        {
            if (detalle == null || nuevoEstado == null || _detalles == null)
                return;

            detalle.IdEstadoProducto = nuevoEstado.IdEstadoProducto;
            detalle.EstadoProducto = nuevoEstado.NombreEstadoProducto;

            var index = _detalles.IndexOf(detalle);
            if (index >= 0)
                _detalles.ResetItem(index);
            else
                dgvDetalles.Refresh();

            ActualizarResumen();
            RegistrarHistorialProducto(detalle, nuevoEstado);
        }

        private void AbrirDetalle(PedidoDetalleViewModel detalle)
        {
            var form = new PedidoDetalleForm(
                _productoService,
                _categorias,
                _proveedoresProductos,
                _estadosProducto,
                _tecnicas,
                _ubicaciones,
                _proveedoresPersonalizacion,
                detalle,
                fechaLimitePedido: ObtenerFechaLimitePedido());

            if (form.ShowDialog(this) == DialogResult.OK && form.DetalleResult != null)
            {
                var result = form.DetalleResult;
                var estadoAnteriorId = detalle?.IdEstadoProducto;

                if (detalle == null)
                {
                    if (result.IdDetallePedido == Guid.Empty)
                        result.IdDetallePedido = Guid.NewGuid();
                    _detalles.Add(result);
                }
                else
                {
                    var index = _detalles.IndexOf(detalle);
                    _detalles[index] = result;

                    if ((estadoAnteriorId ?? Guid.Empty) != (result.IdEstadoProducto ?? Guid.Empty))
                    {
                        var nuevoEstado = ObtenerEstadoProducto(result.IdEstadoProducto);
                        if (nuevoEstado != null)
                            RegistrarHistorialProducto(result, nuevoEstado);
                    }
                }

                ActualizarResumen();
            }
        }

        private DateTime? ObtenerFechaLimitePedido()
        {
            if (chkFechaEntrega.Checked)
                return dtpFechaEntrega.Value.Date;

            return _pedidoOriginal?.FechaLimiteEntrega;
        }

        private void btnAgregarNota_Click(object sender, EventArgs e)
        {
            var texto = txtNuevaNota.Text?.Trim();
            if (string.IsNullOrWhiteSpace(texto))
                return;

            var nota = new PedidoNota
            {
                IdNota = Guid.NewGuid(),
                Nota = texto,
                Fecha = ArgentinaDateTimeHelper.Now(),
                Usuario = SessionContext.NombreUsuario ?? "Sistema"
            };

            _notas.Add(nota);
            txtNuevaNota.Clear();
            RefrescarNotas();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            try
            {
                var pedido = ConstruirPedido();
                ResultadoOperacion resultado;

                if (_pedidoId.HasValue)
                {
                    resultado = _pedidoService.ActualizarPedido(pedido);
                }
                else
                {
                    resultado = _pedidoService.CrearPedido(pedido);
                }

                if (!resultado.EsValido)
                {
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var accion = _pedidoId.HasValue ? "Pedido.Editar" : "Pedido.Alta";
                var mensaje = _pedidoId.HasValue
                    ? $"Pedido actualizado / Order updated: {pedido.NumeroPedido}"
                    : $"Pedido creado / Order created: {pedido.NumeroPedido}";

                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Pedidos");
                _logService.LogInfo(mensaje, "Pedidos", SessionContext.NombreUsuario);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error guardando pedido / Error saving order", ex, "Pedidos", SessionContext.NombreUsuario);
                MessageBox.Show("order.save.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelarPedido_Click(object sender, EventArgs e)
        {
            if (!_pedidoId.HasValue || !_estadoCanceladoId.HasValue)
                return;

            var numeroPedido = txtNumeroPedido.Text?.Trim();
            if (string.IsNullOrWhiteSpace(numeroPedido))
                numeroPedido = _pedidoOriginal?.NumeroPedido;

            var confirmacion = MessageBox.Show(
                string.Format("order.cancel.confirm".Traducir(), numeroPedido),
                Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                var usuario = SessionContext.NombreUsuario ?? "Sistema";
                var comentario = $"Pedido cancelado por {usuario} / Order cancelled by {usuario}";
                var resultado = _pedidoService.CancelarPedido(_pedidoId.Value, usuario, comentario);
                if (!resultado.EsValido)
                {
                    MessageBox.Show("order.cancel.error".Traducir(resultado.Mensaje), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var totales = CalcularTotales();
                _montoPagadoActual = totales.totalConIva;
                ActualizarResumen();

                var mensaje = $"Pedido cancelado / Order cancelled: {numeroPedido}";
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, "Pedido.Cancelar", mensaje, "Pedidos");
                _logService.LogInfo(mensaje, "Pedidos", SessionContext.NombreUsuario);

                MessageBox.Show("order.cancel.success".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cancelando pedido / Error cancelling order", ex, "Pedidos", SessionContext.NombreUsuario);
                MessageBox.Show("order.cancel.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarDatos()
        {
            if (!(cmbCliente.SelectedValue is Guid idCliente) || idCliente == Guid.Empty)
            {
                MessageBox.Show("order.validation.client".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabControl.SelectedTab = tabGeneral;
                cmbCliente.Focus();
                return false;
            }

            if (!(cmbTipoPago.SelectedValue is Guid idTipoPago) || idTipoPago == Guid.Empty)
            {
                MessageBox.Show("order.validation.paymentType".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabControl.SelectedTab = tabGeneral;
                cmbTipoPago.Focus();
                return false;
            }

            if (!_estadoPedidoActual.HasValue || _estadoPedidoActual.Value == Guid.Empty)
            {
                MessageBox.Show("order.validation.state".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabControl.SelectedTab = tabGeneral;
                return false;
            }

            if (_detalles.Count == 0)
            {
                MessageBox.Show("order.validation.details".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabControl.SelectedTab = tabDetalles;
                return false;
            }

            if (chkFacturado.Checked)
            {
                var path = txtFactura.Text?.Trim();
                if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path) && !Uri.IsWellFormedUriString(path, UriKind.Absolute)))
                {
                    MessageBox.Show("order.validation.invoice".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabControl.SelectedTab = tabGeneral;
                    txtFactura.Focus();
                    return false;
                }
            }

            return true;
        }

        private Pedido ConstruirPedido()
        {
            var pedidoId = _pedidoOriginal?.IdPedido;
            var pedido = new Pedido
            {
                IdPedido = pedidoId ?? Guid.NewGuid()
            };

            pedido.NumeroPedido = txtNumeroPedido.Text?.Trim();
            pedido.IdCliente = (Guid)cmbCliente.SelectedValue;
            pedido.IdTipoPago = (Guid?)cmbTipoPago.SelectedValue;
            pedido.IdEstadoPedido = _estadoPedidoActual;
            pedido.FechaLimiteEntrega = chkFechaEntrega.Checked ? dtpFechaEntrega.Value.Date : (DateTime?)null;
            pedido.MontoPagado = _montoPagadoActual;
            pedido.Facturado = chkFacturado.Checked;
            pedido.RutaFacturaPdf = chkFacturado.Checked ? txtFactura.Text?.Trim() : null;
            pedido.Cliente_OC = txtOC.Text?.Trim();
            pedido.Cliente_PersonaNombre = txtContacto.Text?.Trim();
            pedido.Cliente_PersonaEmail = txtEmail.Text?.Trim();
            pedido.Cliente_PersonaTelefono = txtTelefono.Text?.Trim();
            pedido.Cliente_DireccionEntrega = txtDireccionEntrega.Text?.Trim();
            pedido.NumeroRemito = txtNumeroRemito.Text?.Trim();
            pedido.Observaciones = txtObservaciones.Text?.Trim();

            pedido.Detalles = _detalles.Select(MapearDetalleDominio).ToList();
            pedido.Notas = _notas.Select(CloneNota).ToList();

            var totales = CalcularTotales();
            pedido.TotalSinIva = totales.totalSinIva;
            pedido.MontoIva = totales.iva;
            pedido.TotalConIva = totales.totalConIva;
            pedido.SaldoPendiente = totales.saldoPendiente;

            if (_pedidoOriginal != null && pedido.IdEstadoPedido != _pedidoOriginal.IdEstadoPedido)
            {
                var estado = _estadosPedido.FirstOrDefault(e => e.IdEstadoPedido == pedido.IdEstadoPedido)?.NombreEstadoPedido;
                _historial.Add(new PedidoEstadoHistorial
                {
                    IdHistorial = Guid.NewGuid(),
                    IdPedido = pedidoId ?? pedido.IdPedido,
                    IdEstadoPedido = pedido.IdEstadoPedido ?? Guid.Empty,
                    Comentario = string.Format("order.timeline.autoChange".Traducir(), estado, SessionContext.NombreUsuario),
                    FechaCambio = ArgentinaDateTimeHelper.Now(),
                    Usuario = SessionContext.NombreUsuario ?? "Sistema"
                });
            }

            if (_historial != null)
            {
                foreach (var historial in _historial.Where(h => h.IdPedido == Guid.Empty))
                {
                    historial.IdPedido = pedido.IdPedido;
                }
            }

            pedido.HistorialEstados = _historial.Select(CloneHistorial).ToList();

            return pedido;
        }

        private PedidoDetalle MapearDetalleDominio(PedidoDetalleViewModel vm)
        {
            return new PedidoDetalle
            {
                IdDetallePedido = vm.IdDetallePedido == Guid.Empty ? Guid.NewGuid() : vm.IdDetallePedido,
                IdProducto = vm.IdProducto ?? Guid.Empty,
                Cantidad = vm.Cantidad,
                PrecioUnitario = vm.PrecioUnitario,
                IdEstadoProducto = vm.IdEstadoProducto,
                FechaLimiteProduccion = vm.FechaLimite,
                FichaAplicacion = vm.FichaAplicacion,
                Notas = vm.Notas,
                IdProveedorPersonalizacion = vm.IdProveedorPersonalizacion,
                Producto = new Producto
                {
                    IdProducto = vm.IdProducto ?? Guid.Empty,
                    NombreProducto = vm.NombreProducto,
                    IdCategoria = vm.IdCategoria,
                    IdProveedor = vm.IdProveedor,
                    Activo = true
                },
                LogosPedido = vm.Logos.Select(MapearLogoDominio).ToList()
            };
        }

        private LogosPedido MapearLogoDominio(PedidoLogoViewModel vm)
        {
            return new LogosPedido
            {
                IdLogoPedido = vm.IdLogoPedido == Guid.Empty ? Guid.NewGuid() : vm.IdLogoPedido,
                IdTecnicaPersonalizacion = vm.IdTecnica,
                IdUbicacionLogo = vm.IdUbicacion,
                IdProveedor = vm.IdProveedor,
                Cantidad = vm.Cantidad,
                CostoPersonalizacion = vm.Costo,
                Descripcion = vm.Descripcion
            };
        }

        private void RegistrarHistorialProducto(PedidoDetalleViewModel detalle, EstadoProducto estado)
        {
            if (detalle == null || estado == null)
                return;

            if (_historial == null)
                _historial = new List<PedidoEstadoHistorial>();

            ActualizarEstadoPedidoAutomatico();

            var usuario = SessionContext.NombreUsuario ?? "Sistema";
            var comentario = string.Format("order.timeline.productChange".Traducir(),
                detalle.NombreProducto ?? string.Empty,
                estado.NombreEstadoProducto,
                usuario);

            var historial = new PedidoEstadoHistorial
            {
                IdHistorial = Guid.NewGuid(),
                IdPedido = _pedidoOriginal?.IdPedido ?? _pedidoId ?? Guid.Empty,
                IdEstadoPedido = ObtenerEstadoPedidoActual(),
                Comentario = comentario,
                FechaCambio = ArgentinaDateTimeHelper.Now(),
                Usuario = usuario
            };

            _historial.Add(historial);
            RefrescarHistorial();
        }

        private Guid ObtenerEstadoPedidoActual()
        {
            if (_estadoPedidoActual.HasValue && _estadoPedidoActual.Value != Guid.Empty)
                return _estadoPedidoActual.Value;

            if (_pedidoOriginal?.IdEstadoPedido.HasValue == true)
                return _pedidoOriginal.IdEstadoPedido.Value;

            if (cmbEstadoPedido.SelectedValue is Guid estadoId && estadoId != Guid.Empty)
                return estadoId;

            return Guid.Empty;
        }

        private EstadoProducto ObtenerEstadoProducto(Guid? idEstado)
        {
            if (!idEstado.HasValue || idEstado.Value == Guid.Empty)
                return null;

            return _estadosProducto?.FirstOrDefault(e => e.IdEstadoProducto == idEstado.Value);
        }

        private static bool TryParseDecimalFlexible(string texto, out decimal valor)
        {
            valor = 0m;
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            var candidato = texto.Trim();

            if (decimal.TryParse(candidato, NumberStyles.Number, CultureInfo.CurrentCulture, out valor))
                return true;

            if (decimal.TryParse(candidato, NumberStyles.Number, CultureInfo.InvariantCulture, out valor))
                return true;

            return false;
        }
    }
}