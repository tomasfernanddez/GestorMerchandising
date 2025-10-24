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
        private readonly List<decimal> _pagosRegistrados = new List<decimal>();
        private ContextMenuStrip _contextMenuEstados;

        public PedidoForm(
            IPedidoService pedidoService,
            IClienteService clienteService,
            IProductoService productoService,
            ICategoriaProductoService categoriaService,
            IProveedorService proveedorService,
            IBitacoraService bitacoraService,
            ILogService logService,
            Guid? pedidoId = null)
        {
            _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _categoriaService = categoriaService ?? throw new ArgumentNullException(nameof(categoriaService));
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _pedidoId = pedidoId;

            InitializeComponent();

            btnAgregarPago.Click += btnAgregarPago_Click;
            btnDeshacerPago.Click += btnDeshacerPago_Click;
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
            btnAgregarPago.Text = "order.payment.add".Traducir();
            btnDeshacerPago.Text = "order.payment.undo".Traducir();

            gbHistorialEstados.Text = "order.timeline".Traducir();
            columnFecha.Text = "order.timeline.date".Traducir();
            columnEstado.Text = "order.timeline.state".Traducir();
            columnComentario.Text = "order.timeline.comment".Traducir();
            gbNotas.Text = "order.internalNotes".Traducir();
            btnAgregarNota.Text = "order.note.add".Traducir();

            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();

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
            var estadoSeleccionado = mantenerSeleccion && cmbEstadoPedido.SelectedValue is Guid estadoId && estadoId != Guid.Empty
                ? (Guid?)estadoId
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
            var estados = new List<EstadoPedido>
            {
                new EstadoPedido { IdEstadoPedido = Guid.Empty, NombreEstadoPedido = "form.select.optional".Traducir() }
            };
            estados.AddRange(_estadosPedido);
            cmbEstadoPedido.DisplayMember = nameof(EstadoPedido.NombreEstadoPedido);
            cmbEstadoPedido.ValueMember = nameof(EstadoPedido.IdEstadoPedido);
            cmbEstadoPedido.DataSource = estados;

            if (mantenerSeleccion && estadoSeleccionado.HasValue && estados.Any(e => e.IdEstadoPedido == estadoSeleccionado.Value))
            {
                cmbEstadoPedido.SelectedValue = estadoSeleccionado.Value;
            }
            else
            {
                SeleccionarEstadoProduccion();
            }
        }

        private bool EsProveedorProducto(Proveedor proveedor)
        {
            return proveedor?.TiposProveedor != null && proveedor.TiposProveedor.Any(ProveedorCatalogoHelper.EsTipoProducto);
        }

        private bool EsProveedorPersonalizador(Proveedor proveedor)
        {
            return proveedor?.TiposProveedor != null && proveedor.TiposProveedor.Any(ProveedorCatalogoHelper.EsTipoPersonalizador);
        }

        private void SeleccionarEstadoProduccion()
        {
            var estadoProduccion = _estadosPedido?.FirstOrDefault(e => EsEstadoProduccion(e.NombreEstadoPedido));
            if (estadoProduccion != null)
            {
                cmbEstadoPedido.SelectedValue = estadoProduccion.IdEstadoPedido;
            }
            else if (cmbEstadoPedido.Items.Count > 0)
            {
                cmbEstadoPedido.SelectedIndex = 0;
            }
        }

        private static bool EsEstadoProduccion(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var compare = CultureInfo.InvariantCulture.CompareInfo;
            return compare.IndexOf(nombre, "producción", CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0
                || compare.IndexOf(nombre, "produccion", CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
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
                DataPropertyName = nameof(PedidoDetalleViewModel.Categoria),
                Name = nameof(PedidoDetalleViewModel.Categoria),
                HeaderText = "order.detail.category".Traducir(),
                FillWeight = 140,
                MinimumWidth = 110
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
            SetDetalleHeader(nameof(PedidoDetalleViewModel.Categoria), "order.detail.category");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.Proveedor), "order.detail.provider");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.Cantidad), "order.detail.quantity");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.PrecioUnitario), "order.detail.price");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.EstadoProducto), "order.detail.state");
            SetDetalleHeader(nameof(PedidoDetalleViewModel.FechaLimite), "order.detail.deadline");
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

                txtNumeroPedido.Text = _pedidoOriginal.NumeroPedido;
                cmbCliente.SelectedValue = _pedidoOriginal.IdCliente;
                if (_pedidoOriginal.IdTipoPago.HasValue)
                    cmbTipoPago.SelectedValue = _pedidoOriginal.IdTipoPago.Value;
                if (_pedidoOriginal.IdEstadoPedido.HasValue)
                    cmbEstadoPedido.SelectedValue = _pedidoOriginal.IdEstadoPedido.Value;

                if (_pedidoOriginal.FechaLimiteEntrega.HasValue)
                {
                    chkFechaEntrega.Checked = true;
                    dtpFechaEntrega.Value = _pedidoOriginal.FechaLimiteEntrega.Value;
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
            SeleccionarEstadoProduccion();
            _montoPagadoBase = 0m;
            _montoPagadoActual = 0m;
            _pagosRegistrados.Clear();
            ActualizarMontoPagadoUI();
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
                FechaLimite = detalle.FechaLimiteProduccion,
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
                Fecha = nota.Fecha,
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
                FechaCambio = historial.FechaCambio,
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

        private void ActualizarResumen()
        {
            if (_detalles == null)
                return;

            decimal totalProductos = _detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            decimal totalSinIva = Math.Round(totalProductos, 2);
            decimal iva = Math.Round(totalSinIva * 0.21m, 2);
            decimal totalConIva = Math.Round(totalSinIva + iva, 2);
            decimal saldo = Math.Max(0, Math.Round(totalConIva - _montoPagadoActual, 2));

            lblTotalSinIvaValor.Text = totalSinIva.ToString("N2");
            lblMontoIvaValor.Text = iva.ToString("N2");
            lblTotalConIvaValor.Text = totalConIva.ToString("N2");
            lblSaldoPendienteValor.Text = saldo.ToString("N2");
            lblMontoPagadoValor.Text = _montoPagadoActual.ToString("N2");
            btnDeshacerPago.Enabled = _pagosRegistrados.Count > 0;
        }

        private void ActualizarMontoPagadoUI()
        {
            ActualizarResumen();
        }

        private void btnAgregarPago_Click(object sender, EventArgs e)
        {
            if (!TrySolicitarMontoPago(out var monto))
                return;

            if (monto <= 0)
            {
                MessageBox.Show("order.payment.validation.amount".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var mensaje = string.Format("order.payment.confirm".Traducir(), monto);
            if (MessageBox.Show(mensaje, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _pagosRegistrados.Add(monto);
            _montoPagadoActual = Math.Round(_montoPagadoActual + monto, 2);
            ActualizarMontoPagadoUI();
        }

        private void btnDeshacerPago_Click(object sender, EventArgs e)
        {
            if (_pagosRegistrados.Count == 0)
            {
                MessageBox.Show("order.payment.undo.empty".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var monto = _pagosRegistrados[_pagosRegistrados.Count - 1];
            var mensaje = string.Format("order.payment.undo.confirm".Traducir(), monto);
            if (MessageBox.Show(mensaje, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _pagosRegistrados.RemoveAt(_pagosRegistrados.Count - 1);
            _montoPagadoActual = Math.Max(0, Math.Round(_montoPagadoActual - monto, 2));
            ActualizarMontoPagadoUI();
        }

        private bool TrySolicitarMontoPago(out decimal monto)
        {
            using (var dialog = new Form())
            {
                dialog.Text = "order.payment.title".Traducir();
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ShowIcon = false;
                dialog.ClientSize = new Size(260, 130);

                var lbl = new Label
                {
                    AutoSize = true,
                    Text = "order.payment.amount".Traducir(),
                    Location = new Point(12, 15)
                };

                var nud = new NumericUpDown
                {
                    DecimalPlaces = 2,
                    Minimum = 0,
                    Maximum = 1000000000,
                    Location = new Point(15, 40),
                    Width = 220,
                    ThousandsSeparator = true
                };

                var btnOk = new Button
                {
                    Text = "form.accept".Traducir(),
                    DialogResult = DialogResult.OK,
                    Location = new Point(45, 85),
                    AutoSize = true
                };

                var btnCancel = new Button
                {
                    Text = "form.cancel".Traducir(),
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(135, 85),
                    AutoSize = true
                };

                dialog.Controls.AddRange(new Control[] { lbl, nud, btnOk, btnCancel });
                dialog.AcceptButton = btnOk;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    monto = nud.Value;
                    return true;
                }
            }

            monto = 0m;
            return false;
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
                Fecha = DateTime.UtcNow,
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

            if (!(cmbEstadoPedido.SelectedValue is Guid idEstado) || idEstado == Guid.Empty)
            {
                MessageBox.Show("order.validation.state".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabControl.SelectedTab = tabGeneral;
                cmbEstadoPedido.Focus();
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
            pedido.IdEstadoPedido = (Guid?)cmbEstadoPedido.SelectedValue;
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

            if (_pedidoOriginal != null && pedido.IdEstadoPedido != _pedidoOriginal.IdEstadoPedido)
            {
                var estado = _estadosPedido.FirstOrDefault(e => e.IdEstadoPedido == pedido.IdEstadoPedido)?.NombreEstadoPedido;
                _historial.Add(new PedidoEstadoHistorial
                {
                    IdHistorial = Guid.NewGuid(),
                    IdPedido = pedidoId ?? pedido.IdPedido,
                    IdEstadoPedido = pedido.IdEstadoPedido ?? Guid.Empty,
                    Comentario = string.Format("order.timeline.manualChange".Traducir(), estado, SessionContext.NombreUsuario),
                    FechaCambio = DateTime.UtcNow,
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
                FechaCambio = DateTime.UtcNow,
                Usuario = usuario
            };

            _historial.Add(historial);
            RefrescarHistorial();
        }

        private Guid ObtenerEstadoPedidoActual()
        {
            if (cmbEstadoPedido.SelectedValue is Guid estadoId && estadoId != Guid.Empty)
                return estadoId;

            if (_pedidoOriginal?.IdEstadoPedido.HasValue == true)
                return _pedidoOriginal.IdEstadoPedido.Value;

            return Guid.Empty;
        }

        private EstadoProducto ObtenerEstadoProducto(Guid? idEstado)
        {
            if (!idEstado.HasValue || idEstado.Value == Guid.Empty)
                return null;

            return _estadosProducto?.FirstOrDefault(e => e.IdEstadoProducto == idEstado.Value);
        }
    }
}