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
using Services;
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
        private readonly Dictionary<string, (string Es, string En)> _diccionarioMensajes;

        private Pedido _pedidoOriginal;
        private BindingList<PedidoDetalleViewModel> _detalles;
        private List<PedidoEstadoHistorial> _historial;
        private BindingList<ArchivoAdjuntoViewModel> _adjuntos;

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
        private readonly BindingList<PagoRegistrado> _pagosRegistrados = new BindingList<PagoRegistrado>();
        private ContextMenuStrip _contextMenuEstados;
        private Guid? _estadoCanceladoId;
        private bool _pedidoCancelado;
        private Guid? _estadoPedidoActual;

        private static readonly HashSet<string> ExtensionesAdjuntosPermitidas = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf",
            ".doc",
            ".docx",
            ".jpg",
            ".jpeg",
            ".png"
        };

        private static readonly Dictionary<string, string> TiposContenidoAdjuntos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = "application/pdf",
            [".doc"] = "application/msword",
            [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".png"] = "image/png"
        };

        private const string ColAdjuntoNombre = "colAdjuntoNombre";
        private const string ColAdjuntoExtension = "colAdjuntoExtension";
        private const string ColAdjuntoTamano = "colAdjuntoTamano";
        private const string ColAdjuntoFecha = "colAdjuntoFecha";
        private const string ColAdjuntoUsuario = "colAdjuntoUsuario";
        private const string ColAdjuntoDescripcion = "colAdjuntoDescripcion";

        private sealed class PagoRegistrado
        {
            public PagoRegistrado(decimal monto, decimal? porcentaje, DateTime? fecha = null, bool esPrevio = false, Guid? idPago = null)
            {
                IdPago = idPago ?? Guid.Empty;
                Monto = Math.Round(monto, 2);
                Porcentaje = porcentaje.HasValue && porcentaje.Value > 0
                    ? Math.Round(porcentaje.Value, 2)
                    : (decimal?)null;
                Fecha = fecha.HasValue
                    ? ArgentinaDateTimeHelper.ToArgentina(fecha.Value)
                    : ArgentinaDateTimeHelper.Now();
                EsPrevio = esPrevio;
            }

            public Guid IdPago { get; }
            public decimal Monto { get; }
            public decimal? Porcentaje { get; }
            public DateTime Fecha { get; }
            public bool EsPrevio { get; }

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
            _diccionarioMensajes = CrearDiccionarioMensajes();

            InitializeComponent();
            cmbCliente.Format += CmbCliente_Format;
            panelScroll.ClientSizeChanged += PanelScroll_ClientSizeChanged;
            layoutContenido.SizeChanged += (s, e) => ActualizarAreaScroll();
            lstPagos.DataSource = _pagosRegistrados;

            btnAgregarPago.Click += btnAgregarPago_Click;
            btnDeshacerPago.Click += btnCancelarPago_Click;
            btnCancelarPedido.Click += btnCancelarPedido_Click;

            btnAgregarAdjunto.Click += BtnAgregarAdjunto_Click;
            btnDescargarAdjunto.Click += BtnDescargarAdjunto_Click;
            btnEliminarAdjunto.Click += BtnEliminarAdjunto_Click;

            dgvAdjuntos.SelectionChanged += (s, e) => ActualizarEstadoAdjuntos();
            dgvAdjuntos.CellValidating += dgvAdjuntos_CellValidating;
            dgvAdjuntos.DataError += dgvAdjuntos_DataError;
            dgvAdjuntos.CellDoubleClick += DgvAdjuntos_CellDoubleClick;

            dgvAdjuntos.AllowDrop = true;
            dgvAdjuntos.DragEnter += Adjuntos_DragEnter;
            dgvAdjuntos.DragDrop += Adjuntos_DragDrop;

            tableAdjuntos.AllowDrop = true;
            tableAdjuntos.DragEnter += Adjuntos_DragEnter;
            tableAdjuntos.DragDrop += Adjuntos_DragDrop;

            grpFacturas.AllowDrop = true;
            grpFacturas.DragEnter += Adjuntos_DragEnter;
            grpFacturas.DragDrop += Adjuntos_DragDrop;

            ConfigurarGrillaAdjuntos();
            ActualizarEstadoAdjuntos();
        }

        private void PedidoForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarDatosReferencia();
            ConfigurarGrillaDetalles();
            ActualizarTextoColumnasAdjuntos();

            if (_pedidoId.HasValue)
            {
                CargarPedidoExistente(_pedidoId.Value);
            }
            else
            {
                IniciarPedidoNuevo();
            }

            RefrescarHistorial();
            ActualizarResumen();
            ActualizarAreaScroll();

            if (_abrirEnProductos)
            {
                dgvDetalles.Focus();
            }
        }

        private void ConfigurarGrillaAdjuntos()
        {
            dgvAdjuntos.AutoGenerateColumns = false;
            dgvAdjuntos.Columns.Clear();

            var colNombre = new DataGridViewTextBoxColumn
            {
                Name = ColAdjuntoNombre,
                DataPropertyName = nameof(ArchivoAdjuntoViewModel.NombreArchivo),
                ReadOnly = true,
                Width = 200
            };

            var colExtension = new DataGridViewTextBoxColumn
            {
                Name = ColAdjuntoExtension,
                DataPropertyName = nameof(ArchivoAdjuntoViewModel.Extension),
                ReadOnly = true,
                Width = 80
            };

            var colTamano = new DataGridViewTextBoxColumn
            {
                Name = ColAdjuntoTamano,
                DataPropertyName = nameof(ArchivoAdjuntoViewModel.TamanoLegible),
                ReadOnly = true,
                Width = 110
            };

            var colFecha = new DataGridViewTextBoxColumn
            {
                Name = ColAdjuntoFecha,
                DataPropertyName = nameof(ArchivoAdjuntoViewModel.FechaSubidaTexto),
                ReadOnly = true,
                Width = 140
            };

            var colUsuario = new DataGridViewTextBoxColumn
            {
                Name = ColAdjuntoUsuario,
                DataPropertyName = nameof(ArchivoAdjuntoViewModel.NombreUsuario),
                ReadOnly = true,
                Width = 120
            };

            var colDescripcion = new DataGridViewTextBoxColumn
            {
                Name = ColAdjuntoDescripcion,
                DataPropertyName = nameof(ArchivoAdjuntoViewModel.Descripcion),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = false
            };

            dgvAdjuntos.Columns.AddRange(colNombre, colExtension, colTamano, colFecha, colUsuario, colDescripcion);
            dgvAdjuntos.MultiSelect = false;
            dgvAdjuntos.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        private void ActualizarTextoColumnasAdjuntos()
        {
            if (dgvAdjuntos.Columns.Count == 0)
                return;

            dgvAdjuntos.Columns[ColAdjuntoNombre].HeaderText = "order.attachments.column.name".Traducir();
            dgvAdjuntos.Columns[ColAdjuntoExtension].HeaderText = "order.attachments.column.extension".Traducir();
            dgvAdjuntos.Columns[ColAdjuntoTamano].HeaderText = "order.attachments.column.size".Traducir();
            dgvAdjuntos.Columns[ColAdjuntoFecha].HeaderText = "order.attachments.column.date".Traducir();
            dgvAdjuntos.Columns[ColAdjuntoUsuario].HeaderText = "order.attachments.column.user".Traducir();
            dgvAdjuntos.Columns[ColAdjuntoDescripcion].HeaderText = "order.attachments.column.description".Traducir();
        }

        private void ApplyTexts()
        {
            Text = _pedidoId.HasValue ? "order.edit.title".Traducir() : "order.new.title".Traducir();
            tabGeneral.Text = "order.tab.general".Traducir();
            grpDetalles.Text = "order.tab.details".Traducir();
            grpFacturas.Text = "order.tab.attachments".Traducir();
            gbHistorialEstados.Text = "order.tab.tracking".Traducir();

            lblNumeroPedido.Text = "order.number".Traducir();
            lblCliente.Text = "order.client".Traducir();
            lblTipoPago.Text = "order.paymentType".Traducir();
            lblEstadoPedido.Text = "order.state".Traducir();
            lblFechaEntrega.Text = "order.deadline".Traducir();
            chkFechaEntrega.Text = "order.deadline.enable".Traducir();
            lblMontoPagado.Text = "order.paidAmount".Traducir();
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

            columnFecha.Text = "order.timeline.date".Traducir();
            columnEstado.Text = "order.timeline.state".Traducir();
            columnComentario.Text = "order.timeline.comment".Traducir();

            lblAdjuntosInstrucciones.Text = "order.attachments.instructions".Traducir();
            btnAgregarAdjunto.Text = "order.attachments.add".Traducir();
            btnDescargarAdjunto.Text = "order.attachments.download".Traducir();
            btnEliminarAdjunto.Text = "order.attachments.remove".Traducir();

            ActualizarTextoColumnasAdjuntos();

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

        private void ActualizarEstadoAdjuntos()
        {
            var adjunto = ObtenerAdjuntoSeleccionado();
            var habilitado = adjunto != null;
            btnDescargarAdjunto.Enabled = habilitado;
            btnEliminarAdjunto.Enabled = habilitado;
        }

        private void PanelScroll_ClientSizeChanged(object sender, EventArgs e)
        {
            ActualizarAreaScroll();
        }

        private void ActualizarAreaScroll()
        {
            if (panelScroll == null || layoutContenido == null)
                return;

            var anchoDisponible = panelScroll.ClientSize.Width - panelScroll.Padding.Horizontal;
            if (anchoDisponible > 0 && layoutContenido.Width != anchoDisponible)
            {
                layoutContenido.Width = anchoDisponible;
            }

            var preferred = layoutContenido.GetPreferredSize(new Size(panelScroll.ClientSize.Width, 0));
            var altoMinimo = Math.Max(preferred.Height, panelScroll.ClientSize.Height);
            panelScroll.AutoScrollMinSize = new Size(Math.Max(preferred.Width, panelScroll.ClientSize.Width), altoMinimo);
        }

        private void BtnAgregarAdjunto_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "order.attachments.openTitle".Traducir();
                var filterPermitidos = "order.attachments.filter.allowed".Traducir();
                var filterTodos = "order.attachments.filter.all".Traducir();
                dialog.Filter = $"{filterPermitidos}|*.pdf;*.doc;*.docx;*.jpg;*.jpeg;*.png|{filterTodos}|*.*";
                dialog.Multiselect = true;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    AgregarArchivos(dialog.FileNames);
                }
            }
        }

        private void BtnDescargarAdjunto_Click(object sender, EventArgs e)
        {
            var adjunto = ObtenerAdjuntoSeleccionado();
            if (adjunto == null)
            {
                MessageBox.Show("order.attachments.select".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.FileName = adjunto.NombreArchivo;
                var extension = NormalizarExtensionAdjunto(adjunto.Extension) ?? NormalizarExtensionAdjunto(Path.GetExtension(adjunto.NombreArchivo));
                var filtroPrincipal = extension == null
                    ? string.Empty
                    : string.Format("order.attachments.filter.single".Traducir(), extension.TrimStart('.'));
                var filterTodos = "order.attachments.filter.all".Traducir();
                if (string.IsNullOrEmpty(filtroPrincipal))
                {
                    dialog.Filter = $"{filterTodos}|*.*";
                }
                else
                {
                    var patron = "*" + extension.ToLowerInvariant();
                    dialog.Filter = $"{filtroPrincipal}|{patron}|{filterTodos}|*.*";
                }

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    File.WriteAllBytes(dialog.FileName, adjunto.Contenido ?? Array.Empty<byte>());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("order.attachments.save.error".Traducir(ErrorMessageHelper.GetFriendlyMessage(ex)), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEliminarAdjunto_Click(object sender, EventArgs e)
        {
            var adjunto = ObtenerAdjuntoSeleccionado();
            if (adjunto == null)
            {
                MessageBox.Show("order.attachments.select".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var mensaje = string.Format("order.attachments.delete.confirm".Traducir(), adjunto.NombreArchivo);
            if (MessageBox.Show(mensaje, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _adjuntos?.Remove(adjunto);
            ActualizarEstadoAdjuntos();
        }

        private void Adjuntos_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Adjuntos_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            if (e.Data.GetData(DataFormats.FileDrop) is string[] rutas)
            {
                AgregarArchivos(rutas);
            }
        }

        private void dgvAdjuntos_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvAdjuntos.Columns[e.ColumnIndex].Name != ColAdjuntoDescripcion)
                return;

            var texto = e.FormattedValue?.ToString();
            if (!string.IsNullOrEmpty(texto) && texto.Length > 500)
            {
                e.Cancel = true;
                MessageBox.Show("order.attachments.description.max".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (e.RowIndex >= 0 && e.RowIndex < dgvAdjuntos.Rows.Count)
            {
                dgvAdjuntos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = texto?.Trim();
            }
        }

        private void dgvAdjuntos_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void DgvAdjuntos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnDescargarAdjunto_Click(sender, EventArgs.Empty);
            }
        }

        private void AgregarArchivos(IEnumerable<string> rutas)
        {
            if (rutas == null)
                return;

            if (_adjuntos == null)
            {
                _adjuntos = new BindingList<ArchivoAdjuntoViewModel>();
                dgvAdjuntos.DataSource = _adjuntos;
            }

            var agregado = false;

            foreach (var ruta in rutas)
            {
                if (string.IsNullOrWhiteSpace(ruta) || !File.Exists(ruta))
                    continue;

                var extension = NormalizarExtensionAdjunto(Path.GetExtension(ruta));
                if (string.IsNullOrEmpty(extension) || !ExtensionesAdjuntosPermitidas.Contains(extension))
                {
                    MessageBox.Show("order.attachments.invalidType".Traducir(Path.GetFileName(ruta)), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                var info = new FileInfo(ruta);
                if (info.Length > ArchivoAdjuntoHelper.MaxFileSizeBytes)
                {
                    MessageBox.Show("order.attachments.invalidSize".Traducir(Path.GetFileName(ruta)), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                byte[] contenido;
                try
                {
                    contenido = File.ReadAllBytes(ruta);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("order.attachments.read.error".Traducir(Path.GetFileName(ruta), ErrorMessageHelper.GetFriendlyMessage(ex)), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                var adjunto = new ArchivoAdjuntoViewModel
                {
                    IdArchivoAdjunto = Guid.Empty,
                    NombreArchivo = Path.GetFileName(ruta),
                    Extension = extension,
                    TipoContenido = ObtenerTipoContenido(extension),
                    TamanoBytes = contenido.LongLength,
                    FechaSubida = DateTime.UtcNow,
                    IdUsuario = SessionContext.IdUsuario,
                    NombreUsuario = SessionContext.NombreUsuario ?? "Sistema",
                    Descripcion = string.Empty,
                    Contenido = contenido
                };

                _adjuntos.Add(adjunto);
                agregado = true;
            }

            if (agregado)
            {
                dgvAdjuntos.Refresh();
                ActualizarEstadoAdjuntos();
            }
        }

        private ArchivoAdjuntoViewModel ObtenerAdjuntoSeleccionado()
        {
            return dgvAdjuntos.CurrentRow?.DataBoundItem as ArchivoAdjuntoViewModel;
        }

        private static string NormalizarExtensionAdjunto(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                return null;

            var valor = extension.StartsWith(".") ? extension : "." + extension;
            if (valor.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                valor = ".jpg";

            return valor.ToUpperInvariant();
        }

        private string ObtenerTipoContenido(string extension)
        {
            if (!string.IsNullOrEmpty(extension) && TiposContenidoAdjuntos.TryGetValue(extension, out var tipo))
                return tipo;

            return "application/octet-stream";
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
                RegistrarError("Pedido.Referencias", "order.form.log.references.error", ex, ErrorMessageHelper.GetFriendlyMessage(ex));
                MessageBox.Show("order.loadReferences.error".Traducir(ErrorMessageHelper.GetFriendlyMessage(ex)), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void CmbCliente_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is Cliente cliente)
                e.Value = DisplayNameHelper.FormatearNombreConAlias(cliente.RazonSocial, cliente.Alias);
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
                CargarPagosPersistentes(_pedidoOriginal);
                ActualizarMontoPagadoUI();
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

                _historial = _pedidoOriginal.HistorialEstados?.OrderBy(h => h.FechaCambio).Select(CloneHistorial).ToList() ?? new List<PedidoEstadoHistorial>();

                var adjuntos = _pedidoOriginal.Adjuntos?.Select(MapearAdjunto).Where(a => a != null).ToList() ?? new List<ArchivoAdjuntoViewModel>();
                _adjuntos = new BindingList<ArchivoAdjuntoViewModel>(adjuntos);
                dgvAdjuntos.DataSource = _adjuntos;
                ActualizarEstadoAdjuntos();

                ActualizarAccionesCancelacion();
                ActualizarEstadoPedidoAutomatico();
                ActualizarAreaScroll();
            }
            catch (Exception ex)
            {
                RegistrarError("Pedido.Cargar", "order.form.log.load.error", ex, idPedido.ToString());
                MessageBox.Show("order.load.error".Traducir(ErrorMessageHelper.GetFriendlyMessage(ex)), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void CargarPagosPersistentes(Pedido pedido)
        {
            if (pedido == null)
                return;

            var pagos = pedido.Pagos?.OrderBy(p => p.FechaRegistro).ToList();
            if (pagos != null && pagos.Count > 0)
            {
                foreach (var pago in pagos)
                {
                    var registro = new PagoRegistrado(
                        pago.Monto,
                        pago.Porcentaje,
                        pago.FechaRegistro,
                        esPrevio: true,
                        idPago: pago.IdPedidoPago);
                    _pagosRegistrados.Add(registro);
                }
            }
            else if (pedido.MontoPagado > 0)
            {
                var porcentaje = CalcularPorcentajePersistido(pedido);
                var fechaBase = pedido.FechaEntrega
                    ?? pedido.FechaEnvio
                    ?? pedido.FechaFinalizacion
                    ?? pedido.FechaProduccion
                    ?? pedido.FechaConfirmacion
                    ?? pedido.FechaCreacion;

                var pagoPrevio = new PagoRegistrado(pedido.MontoPagado, porcentaje, fechaBase, esPrevio: true);
                _pagosRegistrados.Add(pagoPrevio);
            }

            if (_pagosRegistrados.Count > 0)
            {
                _montoPagadoActual = Math.Round(_pagosRegistrados.Sum(p => p.Monto), 2);
                lstPagos.SelectedItem = _pagosRegistrados.Last();
            }
        }

        private decimal? CalcularPorcentajePersistido(Pedido pedido)
        {
            if (pedido == null || pedido.TotalConIva <= 0)
                return null;

            var porcentaje = Math.Round((pedido.MontoPagado / pedido.TotalConIva) * 100m, 2);
            if (porcentaje <= 0)
                return null;

            return Math.Min(porcentaje, 100m);
        }

        private void IniciarPedidoNuevo()
        {
            txtNumeroPedido.Text = _pedidoService.GenerarProximoNumeroPedido();
            _detalles = new BindingList<PedidoDetalleViewModel>();
            dgvDetalles.DataSource = _detalles;
            _historial = new List<PedidoEstadoHistorial>();
            _adjuntos = new BindingList<ArchivoAdjuntoViewModel>();
            dgvAdjuntos.DataSource = _adjuntos;
            _montoPagadoBase = 0m;
            _montoPagadoActual = 0m;
            _pagosRegistrados.Clear();
            ActualizarMontoPagadoUI();
            _pedidoCancelado = false;
            ActualizarAccionesCancelacion();
            ActualizarEstadoPedidoAutomatico();
            ActualizarEstadoAdjuntos();
            ActualizarAreaScroll();
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
                Proveedor = DisplayNameHelper.FormatearNombreConAlias(
                    detalle.Producto?.Proveedor?.RazonSocial,
                    detalle.Producto?.Proveedor?.Alias),
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                IdEstadoProducto = detalle.IdEstadoProducto,
                EstadoProducto = detalle.EstadoProducto?.NombreEstadoProducto,
                FechaLimite = ArgentinaDateTimeHelper.ToArgentina(detalle.FechaLimiteProduccion),
                FichaAplicacion = detalle.FichaAplicacion,
                Notas = detalle.Notas,
                IdProveedorPersonalizacion = detalle.IdProveedorPersonalizacion,
                ProveedorPersonalizacion = DisplayNameHelper.FormatearNombreConAlias(
                    detalle.ProveedorPersonalizacion?.RazonSocial,
                    detalle.ProveedorPersonalizacion?.Alias),
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
                Proveedor = DisplayNameHelper.FormatearNombreConAlias(
                    logo.Proveedor?.RazonSocial,
                    logo.Proveedor?.Alias),
                Cantidad = logo.Cantidad,
                Costo = logo.CostoPersonalizacion,
                Descripcion = logo.Descripcion
            };
        }

        private ArchivoAdjuntoViewModel MapearAdjunto(ArchivoAdjunto adjunto)
        {
            if (adjunto == null)
                return null;

            var extension = NormalizarExtensionAdjunto(adjunto.Extension ?? Path.GetExtension(adjunto.NombreArchivo));
            var fecha = adjunto.FechaSubida;
            if (fecha != default)
            {
                if (fecha.Kind == DateTimeKind.Local)
                    fecha = fecha.ToUniversalTime();
                else if (fecha.Kind == DateTimeKind.Unspecified)
                    fecha = DateTime.SpecifyKind(fecha, DateTimeKind.Utc);
            }

            return new ArchivoAdjuntoViewModel
            {
                IdArchivoAdjunto = adjunto.IdArchivoAdjunto,
                IdPedido = adjunto.IdPedido,
                NombreArchivo = adjunto.NombreArchivo,
                Extension = extension,
                TipoContenido = adjunto.TipoContenido,
                TamanoBytes = adjunto.TamanoBytes,
                FechaSubida = fecha == default ? DateTime.UtcNow : fecha,
                IdUsuario = adjunto.IdUsuario,
                NombreUsuario = adjunto.NombreUsuario,
                Descripcion = adjunto.Descripcion,
                Contenido = adjunto.Contenido
            };
        }

        private ArchivoAdjunto MapearAdjuntoDominio(ArchivoAdjuntoViewModel vm, Guid pedidoId)
        {
            if (vm == null)
                return null;

            var extension = NormalizarExtensionAdjunto(vm.Extension ?? Path.GetExtension(vm.NombreArchivo));
            var fecha = vm.FechaSubida;
            if (fecha.Kind == DateTimeKind.Local)
            {
                fecha = fecha.ToUniversalTime();
            }
            else if (fecha.Kind == DateTimeKind.Unspecified)
            {
                fecha = DateTime.SpecifyKind(fecha, DateTimeKind.Utc);
            }

            return new ArchivoAdjunto
            {
                IdArchivoAdjunto = vm.IdArchivoAdjunto,
                IdPedido = pedidoId == Guid.Empty ? (Guid?)null : pedidoId,
                IdPedidoMuestra = null,
                NombreArchivo = vm.NombreArchivo,
                Extension = extension,
                TipoContenido = vm.TipoContenido ?? ObtenerTipoContenido(extension),
                TamanoBytes = vm.TamanoBytes,
                FechaSubida = fecha == default ? DateTime.UtcNow : fecha,
                IdUsuario = vm.IdUsuario,
                NombreUsuario = vm.NombreUsuario,
                Descripcion = string.IsNullOrWhiteSpace(vm.Descripcion) ? null : vm.Descripcion.Trim(),
                Contenido = vm.Contenido ?? Array.Empty<byte>()
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

            var calculado = PedidoEstadoResolver.CalcularEstado(estadosDetalle, _estadosPedido);
            if (calculado == null || !calculado.IdEstado.HasValue || calculado.IdEstado.Value == Guid.Empty)
                return;

            _estadoPedidoActual = calculado.IdEstado;

            if (cmbEstadoPedido.DataSource != null)
            {
                var estadoActual = cmbEstadoPedido.SelectedValue is Guid actual ? actual : Guid.Empty;
                if (estadoActual != calculado.IdEstado.Value)
                {
                    if (cmbEstadoPedido.Items.OfType<object>().Any(item =>
                        item is EstadoPedido estado && estado.IdEstadoPedido == calculado.IdEstado.Value))
                    {
                        cmbEstadoPedido.SelectedValue = calculado.IdEstado.Value;
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
            lstPagos.SelectedItem = registro;
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

            if (!(lstPagos.SelectedItem is PagoRegistrado pago))
            {
                MessageBox.Show("order.payment.cancel.prompt".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var mensaje = string.Format("order.payment.cancel.confirm".Traducir(), pago.Monto.ToString("C2"));
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
        private void RegistrarPagoAgregado(PagoRegistrado pago)
        {
            if (pago == null)
                return;

            var monto = pago.Monto.ToString("C2");
            var fecha = pago.Fecha.ToString("g");

            if (pago.Porcentaje.HasValue)
            {
                var porcentaje = pago.Porcentaje.Value.ToString("0.##");
                RegistrarMovimientoPago("Pedido.Pago.Agregar", "order.form.log.payment.add.percent", porcentaje, monto, fecha);
            }
            else
            {
                RegistrarMovimientoPago("Pedido.Pago.Agregar", "order.form.log.payment.add.manual", monto, fecha);
            }
        }

        private void RegistrarPagoCancelado(PagoRegistrado pago)
        {
            if (pago == null)
                return;

            var monto = pago.Monto.ToString("C2");
            var fecha = pago.Fecha.ToString("g");

            if (pago.Porcentaje.HasValue)
            {
                var porcentaje = pago.Porcentaje.Value.ToString("0.##");
                RegistrarMovimientoPago("Pedido.Pago.Cancelar", "order.form.log.payment.cancel.percent", porcentaje, monto, fecha);
            }
            else
            {
                RegistrarMovimientoPago("Pedido.Pago.Cancelar", "order.form.log.payment.cancel.manual", monto, fecha);
            }
        }

        private void RegistrarMovimientoPago(string accion, string claveMensaje, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Pedidos");
            }
            catch
            {
                // Evitar que fallos en registro de auditoría interrumpan el flujo de pagos.
            }

            _logService.LogInfo(mensaje, "Pedidos", SessionContext.NombreUsuario);
        }

        private void chkFechaEntrega_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaEntrega.Enabled = chkFechaEntrega.Checked;
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            Pedido pedido = null;
            try
            {
                pedido = ConstruirPedido();
                ResultadoOperacion resultado = _pedidoId.HasValue
                    ? _pedidoService.ActualizarPedido(pedido)
                    : _pedidoService.CrearPedido(pedido);

                if (!resultado.EsValido)
                {
                    var numeroFallido = pedido?.NumeroPedido ?? txtNumeroPedido.Text?.Trim();
                    RegistrarFallo("Pedido.Guardar", "order.form.log.save.failure", resultado.Mensaje, numeroFallido ?? "-", resultado.Mensaje);
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var accion = _pedidoId.HasValue ? "Pedido.Editar" : "Pedido.Alta";
                var clave = _pedidoId.HasValue ? "order.form.log.update" : "order.form.log.create";
                RegistrarAccion(accion, clave, pedido.NumeroPedido);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                var numeroPedido = pedido?.NumeroPedido ?? txtNumeroPedido.Text?.Trim() ?? _pedidoId?.ToString();
                RegistrarError("Pedido.Guardar", "order.form.log.save.error", ex, numeroPedido ?? "-");
                MessageBox.Show("order.save.error".Traducir(ErrorMessageHelper.GetFriendlyMessage(ex)), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var comentario = ObtenerMensaje("order.form.log.cancel.comment", usuario);
                var resultado = _pedidoService.CancelarPedido(_pedidoId.Value, usuario, comentario);
                if (!resultado.EsValido)
                {
                    RegistrarFallo("Pedido.Cancelar", "order.form.log.cancel.failure", resultado.Mensaje, numeroPedido ?? "-", resultado.Mensaje);
                    MessageBox.Show("order.cancel.error".Traducir(resultado.Mensaje), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var totales = CalcularTotales();
                _montoPagadoActual = totales.totalConIva;
                ActualizarResumen();

                RegistrarAccion("Pedido.Cancelar", "order.form.log.cancel.success", numeroPedido ?? "-");

                MessageBox.Show("order.cancel.success".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                RegistrarError("Pedido.Cancelar", "order.form.log.cancel.error", ex, numeroPedido ?? "-");
                MessageBox.Show("order.cancel.error".Traducir(ErrorMessageHelper.GetFriendlyMessage(ex)), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                tabControl.SelectedIndex = 0;
                dgvDetalles.Focus();
                return false;
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
            pedido.Facturado = _pedidoOriginal?.Facturado ?? false;
            pedido.RutaFacturaPdf = _pedidoOriginal?.RutaFacturaPdf;
            pedido.Cliente_OC = txtOC.Text?.Trim();
            pedido.Cliente_PersonaNombre = txtContacto.Text?.Trim();
            pedido.Cliente_PersonaEmail = txtEmail.Text?.Trim();
            pedido.Cliente_PersonaTelefono = txtTelefono.Text?.Trim();
            pedido.Cliente_DireccionEntrega = txtDireccionEntrega.Text?.Trim();
            pedido.NumeroRemito = txtNumeroRemito.Text?.Trim();
            pedido.Observaciones = txtObservaciones.Text?.Trim();

            pedido.Detalles = _detalles.Select(MapearDetalleDominio).ToList();
            pedido.Notas = _pedidoOriginal?.Notas?.ToList() ?? new List<PedidoNota>();

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

            if (_adjuntos != null)
            {
                pedido.Adjuntos = _adjuntos
                    .Select(vm => MapearAdjuntoDominio(vm, pedido.IdPedido))
                    .Where(a => a != null)
                    .ToList();
            }
            else
            {
                pedido.Adjuntos = new List<ArchivoAdjunto>();
            }

            pedido.Pagos = ConstruirPagosDominio(pedido.IdPedido);

            return pedido;
        }

        private List<PedidoPago> ConstruirPagosDominio(Guid idPedido)
        {
            return _pagosRegistrados
                .Select(p => new PedidoPago
                {
                    IdPedidoPago = p.IdPago == Guid.Empty ? Guid.NewGuid() : p.IdPago,
                    IdPedido = idPedido,
                    Monto = p.Monto,
                    Porcentaje = p.Porcentaje,
                    FechaRegistro = ArgentinaDateTimeHelper.ToUtc(p.Fecha)
                })
                .ToList();
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

        private void RegistrarAccion(string accion, string claveMensaje, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Pedidos");
            }
            catch
            {
                // Evitar que errores de auditoría interrumpan el flujo
            }

            _logService.LogInfo(mensaje, "Pedidos", SessionContext.NombreUsuario);
        }

        private void RegistrarFallo(string accion, string claveMensaje, string detalle, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Pedidos", false, detalle);
            }
            catch
            {
                // Ignorar errores de bitácora
            }

            _logService.LogWarning(mensaje, "Pedidos", SessionContext.NombreUsuario);
        }

        private void RegistrarError(string accion, string claveMensaje, Exception ex, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Pedidos", false, ex?.Message);
            }
            catch
            {
                // Ignorar errores de bitácora
            }

            _logService.LogError(mensaje, ex, "Pedidos", SessionContext.NombreUsuario);
        }

        private Dictionary<string, (string Es, string En)> CrearDiccionarioMensajes()
        {
            return new Dictionary<string, (string Es, string En)>
            {
                ["order.form.log.references.error"] = ("Error al cargar datos de referencia: {0}.", "Error loading reference data: {0}."),
                ["order.form.log.load.error"] = ("Error al cargar el pedido {0}.", "Error loading order {0}."),
                ["order.form.log.save.failure"] = ("No se pudo guardar el pedido {0}: {1}.", "Could not save order {0}: {1}."),
                ["order.form.log.save.error"] = ("Error al guardar el pedido {0}.", "Error saving order {0}."),
                ["order.form.log.create"] = ("Se creó el pedido {0}.", "Order {0} was created."),
                ["order.form.log.update"] = ("Se actualizó el pedido {0}.", "Order {0} was updated."),
                ["order.form.log.cancel.comment"] = ("Pedido cancelado por {0}.", "Order cancelled by {0}."),
                ["order.form.log.cancel.success"] = ("Se canceló el pedido {0}.", "Order {0} was cancelled."),
                ["order.form.log.cancel.failure"] = ("No se pudo cancelar el pedido {0}: {1}.", "Could not cancel order {0}: {1}."),
                ["order.form.log.cancel.error"] = ("Error al cancelar el pedido {0}.", "Error cancelling order {0}."),
                ["order.form.log.payment.add.percent"] = ("Se registró un pago del {0}% por {1} el {2}.", "Recorded a payment of {0}% for {1} on {2}."),
                ["order.form.log.payment.add.manual"] = ("Se registró un pago manual por {0} el {1}.", "Recorded a manual payment of {0} on {1}."),
                ["order.form.log.payment.cancel.percent"] = ("Se anuló un pago del {0}% por {1} registrado el {2}.", "Voided a payment of {0}% for {1} recorded on {2}."),
                ["order.form.log.payment.cancel.manual"] = ("Se anuló un pago manual por {0} registrado el {1}.", "Voided a manual payment of {0} recorded on {1}."),
            };
        }

        private string ObtenerMensaje(string clave, params object[] args)
        {
            if (_diccionarioMensajes.TryGetValue(clave, out var textos))
            {
                var mensajeEs = args != null && args.Length > 0 ? string.Format(textos.Es, args) : textos.Es;
                var mensajeEn = args != null && args.Length > 0 ? string.Format(textos.En, args) : textos.En;
                return string.Concat(mensajeEs, " / ", mensajeEn);
            }

            if (args != null && args.Length > 0)
            {
                return string.Format(clave, args);
            }

            return clave;
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

        private void panelResumen_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblMontoPagadoValor_Click(object sender, EventArgs e)
        {

        }

        private void lstPagos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvAdjuntos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}