using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BLL.Interfaces;
using BLL.Helpers;
using DomainModel;
using DomainModel.Entidades;
using Services;
using Services.BLL.Interfaces;
using UI.Helpers;
using UI.Localization;
using UI.ViewModels;

namespace UI
{
    public partial class PedidoMuestraForm : Form
    {
        private readonly IPedidoMuestraService _pedidoMuestraService;
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogService _logService;
        private readonly Guid? _pedidoId;
        private readonly Dictionary<string, (string Es, string En)> _diccionarioMensajes;

        private PedidoMuestra _pedidoOriginal;
        private List<Cliente> _clientes;
        private List<Producto> _productos;
        private List<EstadoMuestra> _estadosMuestra;
        private List<EstadoPedidoMuestra> _estadosPedido;
        private BindingList<PedidoMuestraDetalleViewModel> _detalles;
        private readonly BindingList<PagoRegistrado> _pagosRegistrados = new BindingList<PagoRegistrado>();
        private decimal _montoPagadoActual;
        private ContextMenuStrip _menuDetalles;
        private ToolStripMenuItem _menuExtenderDetalle;
        private ToolStripMenuItem _menuCambiarEstadoDetalle;
        private Guid? _estadoPendienteMuestraId;
        private DateTime _fechaPedido;
        private string _numeroPedido;
        private Guid? _estadoPedidoActual;
        private string _estadoPedidoNombre;
        private bool _estadoPedidoInicializado;

        private BindingList<ArchivoAdjuntoViewModel> _adjuntos;

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

        private const string ESTADO_DEVUELTO = "Devuelto";
        private const string ESTADO_A_FACTURAR = "Pendiente de Pago";
        private const string ESTADO_PENDIENTE_ENVIO = "Pendiente de Envío";
        private const string ESTADO_FACTURADO = "Facturado";
        private const string ESTADO_PAGADO = "Pagado";

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
                    detalle = string.Format(
                        "sampleOrder.payment.entry.percentDetail".Traducir(),
                        Porcentaje.Value.ToString("0.##"),
                        Monto.ToString("C2"));
                }
                else
                {
                    detalle = string.Format(
                        "sampleOrder.payment.entry.manualDetail".Traducir(),
                        Monto.ToString("C2"));
                }

                return string.Format("sampleOrder.payment.entry".Traducir(), fechaTexto, detalle);
            }

            public override string ToString() => ObtenerDescripcion();
        }

        public PedidoMuestraForm(
            IPedidoMuestraService pedidoMuestraService,
            IClienteService clienteService,
            IProductoService productoService,
            IBitacoraService bitacoraService,
            ILogService logService,
            Guid? pedidoId = null)
        {
            _pedidoMuestraService = pedidoMuestraService ?? throw new ArgumentNullException(nameof(pedidoMuestraService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _pedidoId = pedidoId;
            _diccionarioMensajes = CrearDiccionarioMensajes();
            _estadoPedidoInicializado = false;

            InitializeComponent();

            lstPagos.DataSource = _pagosRegistrados;
            ConfigurarMenuDetalles();

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

            grpAdjuntos.AllowDrop = true;
            grpAdjuntos.DragEnter += Adjuntos_DragEnter;
            grpAdjuntos.DragDrop += Adjuntos_DragDrop;

            btnDescargarAdjunto.Enabled = false;
            btnEliminarAdjunto.Enabled = false;
        }

        private void PedidoMuestraForm_Load(object sender, EventArgs e)
        {
            ConfigurarGrillaAdjuntos();
            ApplyTexts();
            CargarDatosReferencia();
            ConfigurarGrillaDetalles();

            if (_pedidoId.HasValue)
            {
                CargarPedidoExistente(_pedidoId.Value);
            }
            else
            {
                InicializarNuevoPedido();
            }

            ActualizarResumen();
            VerificarVencimiento();
            dgvDetalles_SelectionChanged(this, EventArgs.Empty);
            ActualizarEstadoAdjuntos();
        }

        private void ApplyTexts()
        {
            Text = _pedidoId.HasValue ? "sampleOrder.edit.title".Traducir() : "sampleOrder.new.title".Traducir();
            grpGeneral.Text = "sampleOrder.group.general".Traducir();
            grpDetalles.Text = "sampleOrder.group.details".Traducir();
            grpPagos.Text = "sampleOrder.group.payments".Traducir();
            grpAdjuntos.Text = "sampleOrder.group.invoices".Traducir();

            lblCliente.Text = "sampleOrder.client".Traducir();
            lblContacto.Text = "sampleOrder.contact.name".Traducir();
            lblEmail.Text = "sampleOrder.contact.email".Traducir();
            lblTelefono.Text = "sampleOrder.contact.phone".Traducir();
            lblDireccion.Text = "sampleOrder.contact.address".Traducir();
            lblNumeroPedido.Text = "sampleOrder.number".Traducir();
            lblFechaPedido.Text = "sampleOrder.created.date".Traducir();
            lblEstadoPedido.Text = "sampleOrder.state".Traducir();
            lblObservaciones.Text = "sampleOrder.notes".Traducir();
            btnAgregarDetalle.Text = "sampleOrder.detail.add".Traducir();
            btnEditarDetalle.Text = "sampleOrder.detail.edit".Traducir();
            btnEliminarDetalle.Text = "sampleOrder.detail.delete".Traducir();
            btnPedirFacturacion.Text = "sampleOrder.request.billing".Traducir();
            btnExtenderDias.Text = "sampleOrder.extend.due".Traducir();

            lblTotal.Text = "sampleOrder.summary.total".Traducir();
            lblPagado.Text = "sampleOrder.summary.paid".Traducir();
            lblSaldo.Text = "sampleOrder.summary.balance".Traducir();
            btnAgregarPago.Text = "sampleOrder.payment.addPercent".Traducir();
            btnEliminarPago.Text = "sampleOrder.payment.remove".Traducir();
            lblExtenderDias.Text = "sampleOrder.extend.days".Traducir();

            lblAdjuntosInstrucciones.Text = "order.attachments.instructions".Traducir();
            btnAgregarAdjunto.Text = "order.attachments.add".Traducir();
            btnDescargarAdjunto.Text = "order.attachments.download".Traducir();
            btnEliminarAdjunto.Text = "order.attachments.remove".Traducir();

            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();

            if (dgvDetalles.Columns.Count > 0)
            {
                ActualizarEncabezadosGrid();
            }

            ActualizarTextoColumnasAdjuntos();

            if (_menuExtenderDetalle != null)
            {
                _menuExtenderDetalle.Text = "sampleOrder.extend.due".Traducir();
            }

            if (_menuCambiarEstadoDetalle != null)
            {
                _menuCambiarEstadoDetalle.Text = "sampleOrder.detail.changeState".Traducir();
                ActualizarMenuEstados();
            }

            ActualizarCabeceraPedido();
        }

        private void CargarDatosReferencia()
        {
            try
            {
                _clientes = _clienteService.ObtenerClientesActivos().OrderBy(c => c.RazonSocial).ToList();
                _productos = _productoService.ObtenerTodos().OrderBy(p => p.NombreProducto).ToList();
                _estadosMuestra = _pedidoMuestraService.ObtenerEstadosMuestra().OrderBy(e => e.NombreEstadoMuestra).ToList();
                _estadosPedido = _pedidoMuestraService.ObtenerEstadosPedido().OrderBy(e => e.NombreEstadoPedidoMuestra).ToList();

                var placeholder = "form.select.optional".Traducir();

                var clientes = new List<Cliente> { new Cliente { IdCliente = Guid.Empty, RazonSocial = placeholder } };
                clientes.AddRange(_clientes);
                cmbCliente.DisplayMember = nameof(Cliente.RazonSocial);
                cmbCliente.ValueMember = nameof(Cliente.IdCliente);
                cmbCliente.DataSource = clientes;
                cmbCliente.SelectedValue = Guid.Empty;

                _estadoPendienteMuestraId = _estadosMuestra
                    .FirstOrDefault(e => string.Equals(e.NombreEstadoMuestra, ESTADO_PENDIENTE_ENVIO, StringComparison.OrdinalIgnoreCase))?
                    .IdEstadoMuestra;

                ActualizarMenuEstados();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cargando datos de referencia para pedidos de muestra / Error loading sample order reference data", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sampleOrder.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrillaDetalles()
        {
            dgvDetalles.AutoGenerateColumns = false;
            dgvDetalles.Columns.Clear();

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.NombreProducto),
                HeaderText = "sampleOrder.detail.product".Traducir(),
                Width = 180
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.EstadoMuestra),
                HeaderText = "sampleOrder.detail.state".Traducir(),
                Width = 120
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.PrecioUnitario),
                HeaderText = "sampleOrder.detail.price".Traducir(),
                DefaultCellStyle = { Format = "C2" },
                Width = 90
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.Subtotal),
                HeaderText = "sampleOrder.detail.subtotal".Traducir(),
                DefaultCellStyle = { Format = "C2" },
                Width = 90
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.FechaDevolucion),
                HeaderText = "sampleOrder.detail.returnDate".Traducir(),
                DefaultCellStyle = { Format = "d" },
                Width = 120
            });

            _detalles = new BindingList<PedidoMuestraDetalleViewModel>();
            dgvDetalles.DataSource = _detalles;
            dgvDetalles.CellMouseDown += DgvDetalles_CellMouseDown;
            dgvDetalles.SelectionChanged += dgvDetalles_SelectionChanged;
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

            _adjuntos = new BindingList<ArchivoAdjuntoViewModel>();
            dgvAdjuntos.DataSource = _adjuntos;
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

        private void ActualizarEstadoAdjuntos()
        {
            var adjunto = ObtenerAdjuntoSeleccionado();
            var habilitado = adjunto != null;

            btnDescargarAdjunto.Enabled = habilitado;
            btnEliminarAdjunto.Enabled = habilitado;
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
                    AgregarAdjuntosDesdeArchivos(dialog.FileNames);
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
                AgregarAdjuntosDesdeArchivos(rutas);
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

        private void AgregarAdjuntosDesdeArchivos(IEnumerable<string> rutas)
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
                IdPedidoMuestra = adjunto.IdPedidoMuestra,
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
                IdPedido = null,
                IdPedidoMuestra = pedidoId == Guid.Empty ? (Guid?)null : pedidoId,
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

        private void ConfigurarMenuDetalles()
        {
            _menuDetalles = new ContextMenuStrip();
            _menuExtenderDetalle = new ToolStripMenuItem("sampleOrder.extend.due".Traducir());
            _menuExtenderDetalle.Click += (s, e) => ExtenderDetalleDesdeContexto();

            _menuCambiarEstadoDetalle = new ToolStripMenuItem("sampleOrder.detail.changeState".Traducir());

            _menuDetalles.Items.AddRange(new ToolStripItem[]
            {
                _menuExtenderDetalle,
                _menuCambiarEstadoDetalle
            });

            dgvDetalles.ContextMenuStrip = _menuDetalles;
        }

        private void ActualizarMenuEstados()
        {
            if (_menuCambiarEstadoDetalle == null)
                return;

            _menuCambiarEstadoDetalle.DropDownItems.Clear();

            if (_estadosMuestra == null)
                return;

            foreach (var estado in _estadosMuestra.OrderBy(e => e.NombreEstadoMuestra))
            {
                var item = new ToolStripMenuItem(estado.NombreEstadoMuestra)
                {
                    Tag = estado
                };
                item.Click += (s, e) => CambiarEstadoDetalle((EstadoMuestra)((ToolStripMenuItem)s).Tag);
                _menuCambiarEstadoDetalle.DropDownItems.Add(item);
            }
        }

        private void ActualizarEncabezadosGrid()
        {
            dgvDetalles.Columns[nameof(PedidoMuestraDetalleViewModel.NombreProducto)].HeaderText = "sampleOrder.detail.product".Traducir();
            dgvDetalles.Columns[nameof(PedidoMuestraDetalleViewModel.EstadoMuestra)].HeaderText = "sampleOrder.detail.state".Traducir();
            dgvDetalles.Columns[nameof(PedidoMuestraDetalleViewModel.PrecioUnitario)].HeaderText = "sampleOrder.detail.price".Traducir();
            dgvDetalles.Columns[nameof(PedidoMuestraDetalleViewModel.Subtotal)].HeaderText = "sampleOrder.detail.subtotal".Traducir();
            dgvDetalles.Columns[nameof(PedidoMuestraDetalleViewModel.FechaDevolucion)].HeaderText = "sampleOrder.detail.returnDate".Traducir();
        }

        private void ActualizarCabeceraPedido()
        {
            lblNumeroPedidoValor.Text = string.IsNullOrWhiteSpace(_numeroPedido)
                ? "sampleOrder.number.pending".Traducir()
                : _numeroPedido;

            var fechaLocal = ArgentinaDateTimeHelper.ToArgentina(_fechaPedido);
            lblFechaPedidoValor.Text = fechaLocal.ToString("g");

            var nombreEstado = !string.IsNullOrWhiteSpace(_estadoPedidoNombre)
                ? _estadoPedidoNombre
                : _estadosPedido?
                    .FirstOrDefault(e => e.IdEstadoPedidoMuestra == _estadoPedidoActual)?.NombreEstadoPedidoMuestra;

            lblEstadoPedidoValor.Text = string.IsNullOrWhiteSpace(nombreEstado)
                ? "sampleOrder.state.auto".Traducir()
                : nombreEstado;
        }

        private void ActualizarEstadoPedidoDesdeDetalles()
        {
            var estadosDetalle = _detalles
                .Select(d => d.EstadoMuestra ?? ObtenerNombreEstado(d.IdEstadoMuestra))
                .ToList();

            var calculado = PedidoMuestraEstadoResolver.CalcularEstado(estadosDetalle, _estadosPedido);
            var estadoAnteriorId = _estadoPedidoActual;
            var estadoAnteriorNombre = _estadoPedidoNombre;
            if (calculado != null)
            {
                _estadoPedidoActual = calculado.IdEstado;
                _estadoPedidoNombre = calculado.NombreEstado;
            }
            else
            {
                _estadoPedidoActual = null;
                _estadoPedidoNombre = null;
            }
            ActualizarCabeceraPedido();

            var cambioEstado = estadoAnteriorId != _estadoPedidoActual
                || !string.Equals(estadoAnteriorNombre, _estadoPedidoNombre, StringComparison.OrdinalIgnoreCase);

            if (_estadoPedidoInicializado && cambioEstado && !string.IsNullOrWhiteSpace(_estadoPedidoNombre))
            {
                RegistrarAccion("PedidoMuestra.Estado.Automatico", "sampleOrder.log.orderStateChange", _estadoPedidoNombre);
            }

            if (!_estadoPedidoInicializado)
            {
                _estadoPedidoInicializado = true;
            }
        }

        private void InicializarNuevoPedido()
        {
            _estadoPedidoInicializado = false;
            _detalles.Clear();
            _pagosRegistrados.Clear();
            _montoPagadoActual = 0;

            _adjuntos?.Clear();
            dgvAdjuntos.Refresh();
            ActualizarEstadoAdjuntos();

            _fechaPedido = ArgentinaDateTimeHelper.Now();
            _numeroPedido = null;
            _estadoPedidoActual = null;
            _estadoPedidoNombre = null;

            txtContacto.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtObservaciones.Text = string.Empty;
            nudDiasExtension.Value = 1;

            if (cmbCliente.Items.Count > 0)
            {
                cmbCliente.SelectedIndex = 0;
            }

            ActualizarEstadoPedidoDesdeDetalles();
        }

        private void CargarPedidoExistente(Guid idPedido)
        {
            _estadoPedidoInicializado = false;
            try
            {
                _pedidoOriginal = _pedidoMuestraService.ObtenerPedidoMuestra(idPedido, incluirDetalles: true);
                if (_pedidoOriginal == null)
                {
                    MessageBox.Show("sampleOrder.notfound".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                _numeroPedido = _pedidoOriginal.NumeroPedidoMuestra;
                _fechaPedido = ArgentinaDateTimeHelper.ToArgentina(_pedidoOriginal.FechaCreacion);
                _estadoPedidoActual = _pedidoOriginal.IdEstadoPedidoMuestra;
                _estadoPedidoNombre = _pedidoOriginal.EstadoPedidoMuestra?.NombreEstadoPedidoMuestra;

                cmbCliente.SelectedValue = _pedidoOriginal.IdCliente;

                txtContacto.Text = _pedidoOriginal.PersonaContacto;
                txtEmail.Text = _pedidoOriginal.EmailContacto;
                txtTelefono.Text = _pedidoOriginal.TelefonoContacto;
                txtDireccion.Text = _pedidoOriginal.DireccionEntrega;
                txtObservaciones.Text = _pedidoOriginal.Observaciones;

                var adjuntosExistentes = _pedidoOriginal.Adjuntos?
                    .Select(MapearAdjunto)
                    .Where(a => a != null)
                    .ToList() ?? new List<ArchivoAdjuntoViewModel>();

                _adjuntos = new BindingList<ArchivoAdjuntoViewModel>(adjuntosExistentes);
                dgvAdjuntos.DataSource = _adjuntos;
                dgvAdjuntos.Refresh();
                ActualizarEstadoAdjuntos();

                _detalles.Clear();
                foreach (var detalle in _pedidoOriginal.Detalles)
                {
                    var producto = _productos.FirstOrDefault(p => p.IdProducto == detalle.IdProducto);
                    var estado = _estadosMuestra.FirstOrDefault(e => e.IdEstadoMuestra == detalle.IdEstadoMuestra);

                    _detalles.Add(new PedidoMuestraDetalleViewModel
                    {
                        IdDetalleMuestra = detalle.IdDetalleMuestra,
                        IdProducto = detalle.IdProducto,
                        NombreProducto = producto?.NombreProducto ?? string.Empty,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Subtotal = detalle.Subtotal,
                        IdEstadoMuestra = detalle.IdEstadoMuestra,
                        EstadoMuestra = estado?.NombreEstadoMuestra,
                        FechaDevolucion = detalle.FechaDevolucion
                    });
                }

                _pagosRegistrados.Clear();
                _montoPagadoActual = 0m;
                CargarPagosPersistentes(_pedidoOriginal);

                foreach (var detalle in _detalles)
                {
                    RecalcularSubtotal(detalle);
                }

                ActualizarResumen();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cargando pedido de muestra / Error loading sample order", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sampleOrder.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPagosPersistentes(PedidoMuestra pedido)
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
                        idPago: pago.IdPedidoMuestraPago);
                    _pagosRegistrados.Add(registro);
                }
            }
            else if (pedido.MontoPagado > 0)
            {
                decimal? porcentaje = null;
                if (pedido.MontoTotal > 0)
                {
                    porcentaje = Math.Round((pedido.MontoPagado / pedido.MontoTotal) * 100m, 2);
                    if (porcentaje <= 0)
                    {
                        porcentaje = null;
                    }
                }

                var fechaBase = pedido.FechaDevolucion
                    ?? pedido.FechaDevolucionEsperada
                    ?? pedido.FechaEntrega
                    ?? pedido.FechaCreacion;

                var pagoPrevio = new PagoRegistrado(
                    pedido.MontoPagado,
                    porcentaje,
                    fechaBase,
                    esPrevio: true);
                _pagosRegistrados.Add(pagoPrevio);
            }

            _montoPagadoActual = Math.Round(_pagosRegistrados.Sum(p => p.Monto), 2);
            if (_pagosRegistrados.Count > 0)
            {
                lstPagos.SelectedItem = _pagosRegistrados.Last();
            }
        }

        private void btnAgregarDetalle_Click(object sender, EventArgs e)
        {
            var form = new PedidoMuestraDetalleForm(_productos, _estadosMuestra, null, _fechaPedido);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                var nuevo = form.DetalleResult;
                if (!nuevo.IdProducto.HasValue)
                {
                    return;
                }

                if (_detalles.Any(d => d.IdProducto == nuevo.IdProducto))
                {
                    MessageBox.Show("sampleOrder.detail.duplicate".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!nuevo.IdEstadoMuestra.HasValue && _estadoPendienteMuestraId.HasValue)
                {
                    nuevo.IdEstadoMuestra = _estadoPendienteMuestraId;
                    nuevo.EstadoMuestra = ESTADO_PENDIENTE_ENVIO;
                }

                RecalcularSubtotal(nuevo);
                _detalles.Add(nuevo);
                ActualizarResumen();

                if (!string.IsNullOrWhiteSpace(nuevo.NombreProducto))
                {
                    RegistrarAccion("PedidoMuestra.Detalle.Agregar", "sampleOrder.log.detail.add", nuevo.NombreProducto);
                }
            }
        }

        private void btnEditarDetalle_Click(object sender, EventArgs e)
        {
            var detalle = ObtenerDetalleSeleccionado();
            if (detalle == null)
            {
                return;
            }

            var form = new PedidoMuestraDetalleForm(_productos, _estadosMuestra, detalle, _fechaPedido);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                var actualizado = form.DetalleResult;
                if (!actualizado.IdProducto.HasValue)
                    return;

                if (_detalles.Any(d => d.IdProducto == actualizado.IdProducto && d.IdDetalleMuestra != actualizado.IdDetalleMuestra))
                {
                    MessageBox.Show("sampleOrder.detail.duplicate".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                detalle.IdProducto = actualizado.IdProducto;
                detalle.NombreProducto = actualizado.NombreProducto;
                detalle.PrecioUnitario = actualizado.PrecioUnitario;
                detalle.Subtotal = actualizado.Subtotal;
                detalle.IdEstadoMuestra = actualizado.IdEstadoMuestra;
                detalle.EstadoMuestra = actualizado.EstadoMuestra;
                detalle.FechaDevolucion = actualizado.FechaDevolucion;

                RecalcularSubtotal(detalle);
                dgvDetalles.Refresh();
                ActualizarResumen();

                if (!string.IsNullOrWhiteSpace(actualizado.NombreProducto))
                {
                    RegistrarAccion("PedidoMuestra.Detalle.Editar", "sampleOrder.log.detail.update", actualizado.NombreProducto);
                }
            }
        }

        private void btnEliminarDetalle_Click(object sender, EventArgs e)
        {
            var detalle = ObtenerDetalleSeleccionado();
            if (detalle == null)
                return;

            var nombreProducto = detalle.NombreProducto;
            var confirm = MessageBox.Show("sampleOrder.detail.delete.confirm".Traducir(), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            _detalles.Remove(detalle);
            ActualizarResumen();

            if (!string.IsNullOrWhiteSpace(nombreProducto))
            {
                RegistrarAccion("PedidoMuestra.Detalle.Eliminar", "sampleOrder.log.detail.delete", nombreProducto);
            }
        }

        private PedidoMuestraDetalleViewModel ObtenerDetalleSeleccionado()
        {
            if (dgvDetalles.CurrentRow?.DataBoundItem is PedidoMuestraDetalleViewModel detalle)
            {
                return detalle;
            }
            return null;
        }

        private List<PedidoMuestraDetalleViewModel> ObtenerDetallesSeleccionados()
        {
            var seleccionados = new HashSet<PedidoMuestraDetalleViewModel>();

            foreach (DataGridViewCell cell in dgvDetalles.SelectedCells)
            {
                if (cell?.OwningRow?.DataBoundItem is PedidoMuestraDetalleViewModel detalle)
                {
                    seleccionados.Add(detalle);
                }
            }

            if (dgvDetalles.CurrentRow?.DataBoundItem is PedidoMuestraDetalleViewModel actual)
            {
                seleccionados.Add(actual);
            }

            return seleccionados.ToList();
        }

        private void btnPedirFacturacion_Click(object sender, EventArgs e)
        {
            if (_detalles.Count == 0)
            {
                return;
            }

            var seleccionados = ObtenerDetallesSeleccionados();
            if (seleccionados.Count == 0)
            {
                MessageBox.Show("sampleOrder.detail.selectRequired".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var idFacturar = BuscarEstadoMuestraId(ESTADO_A_FACTURAR);
            if (!idFacturar.HasValue)
            {
                MessageBox.Show("sampleOrder.state.missing".Traducir(ESTADO_A_FACTURAR), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var modificados = new List<string>();
            foreach (var detalle in seleccionados)
            {
                if (EsEstadoDevuelto(detalle) || EsEstadoPagado(detalle))
                    continue;

                var estadoActual = detalle.EstadoMuestra ?? ObtenerNombreEstado(detalle.IdEstadoMuestra);
                if (string.Equals(estadoActual, ESTADO_A_FACTURAR, StringComparison.OrdinalIgnoreCase))
                    continue;

                detalle.IdEstadoMuestra = idFacturar;
                detalle.EstadoMuestra = ESTADO_A_FACTURAR;
                RecalcularSubtotal(detalle);

                var nombreProducto = string.IsNullOrWhiteSpace(detalle.NombreProducto) ? "N/A" : detalle.NombreProducto;
                modificados.Add(nombreProducto);
            }

            if (modificados.Count == 0)
            {
                MessageBox.Show("sampleOrder.request.billing.none".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvDetalles.Refresh();
            ActualizarResumen();

            var nombres = string.Join(", ", modificados);
            RegistrarAccion("PedidoMuestra.PedirFacturacion", "sampleOrder.log.requestBilling", nombres);
        }

        private void btnExtenderDias_Click(object sender, EventArgs e)
        {
            if (!TryExtenderDetalleSeleccionado((int)nudDiasExtension.Value))
            {
                return;
            }
        }

        private void btnAgregarPago_Click(object sender, EventArgs e)
        {
            if (!TrySeleccionarMontoPago(out var monto, out var porcentaje))
                return;

            var detalle = porcentaje.HasValue
                ? string.Format("sampleOrder.payment.confirm.percentInfo".Traducir(), porcentaje.Value.ToString("0.##"))
                : "sampleOrder.payment.confirm.manualInfo".Traducir();
            var mensaje = string.Format("sampleOrder.payment.confirm".Traducir(), monto.ToString("C2"), detalle);
            if (MessageBox.Show(mensaje, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var registro = new PagoRegistrado(monto, porcentaje);
            _pagosRegistrados.Add(registro);
            lstPagos.SelectedItem = registro;
            _montoPagadoActual = Math.Round(_montoPagadoActual + monto, 2);
            ActualizarResumen();
            RegistrarPagoAgregado(registro);
        }

        private void btnEliminarPago_Click(object sender, EventArgs e)
        {
            if (!(lstPagos.SelectedItem is PagoRegistrado pago))
            {
                MessageBox.Show("sampleOrder.payment.remove.prompt".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var mensaje = string.Format("sampleOrder.payment.cancel.confirm".Traducir(), pago.Monto.ToString("C2"));
            if (MessageBox.Show(mensaje, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _pagosRegistrados.Remove(pago);
            _montoPagadoActual = Math.Max(0, Math.Round(_montoPagadoActual - pago.Monto, 2));
            ActualizarResumen();
            RegistrarPagoEliminado(pago);
        }

        private void ActualizarResumen()
        {
            var total = Math.Round(_detalles.Sum(d => d.Subtotal), 2);
            lblTotalValor.Text = total.ToString("N2");
            lblPagadoValor.Text = _montoPagadoActual.ToString("N2");
            var saldo = Math.Max(0, total - _montoPagadoActual);
            lblSaldoValor.Text = saldo.ToString("N2");

            var detallesActualizados = false;
            if (saldo <= 0)
            {
                detallesActualizados = MarcarDetallesComoPagados();
            }

            if (detallesActualizados)
            {
                dgvDetalles.Refresh();
            }

            ActualizarEstadoPedidoDesdeDetalles();
        }

        private bool TrySeleccionarMontoPago(out decimal monto, out decimal? porcentaje)
        {
            monto = 0m;
            porcentaje = null;

            var total = Math.Round(_detalles.Sum(d => d.Subtotal), 2);
            var saldoPendiente = Math.Max(0, total - _montoPagadoActual);
            if (saldoPendiente <= 0)
            {
                MessageBox.Show("sampleOrder.payment.nonePending".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            using (var dialog = new Form())
            {
                dialog.Text = "sampleOrder.payment.percent.title".Traducir();
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
                    Text = string.Format("sampleOrder.payment.percent.remaining".Traducir(), saldoPendiente.ToString("C2"))
                };
                table.Controls.Add(lblSaldo, 0, 0);

                var lblHint = new Label
                {
                    AutoSize = true,
                    Margin = new Padding(0, 6, 0, 6),
                    Text = "sampleOrder.payment.input.hint".Traducir()
                };
                table.Controls.Add(lblHint, 0, 1);

                var panelMonto = new FlowLayoutPanel
                {
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    WrapContents = false
                };
                var lblMonto = new Label { AutoSize = true, Text = "sampleOrder.payment.amount.label".Traducir(), Margin = new Padding(0, 6, 6, 0) };
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
                var lblPorcentaje = new Label { AutoSize = true, Text = "sampleOrder.payment.percent.label".Traducir(), Margin = new Padding(0, 6, 6, 0) };
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
                    Text = "sampleOrder.payment.quickOptions".Traducir(),
                    Margin = new Padding(0, 6, 6, 0)
                };
                panelOpciones.Controls.Add(lblOpciones);
                foreach (var porcentajeRapido in new[] { 30m, 50m, 100m })
                {
                    var btnOpcion = new Button
                    {
                        AutoSize = true,
                        Text = string.Format("sampleOrder.payment.quickOption".Traducir(), porcentajeRapido.ToString("0")),
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
                    Text = string.Format("sampleOrder.payment.preview".Traducir(), "--")
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
                    Text = "sampleOrder.payment.amount.confirmButton".Traducir()
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
                    lblResultado.Text = string.Format("sampleOrder.payment.preview".Traducir(), texto);
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
                            MessageBox.Show("sampleOrder.payment.input.invalid".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        montoIngresado = Math.Round(montoParseado, 2);
                    }

                    if (!string.IsNullOrWhiteSpace(txtPorcentaje.Text))
                    {
                        if (!TryParseDecimalFlexible(txtPorcentaje.Text, out var porcentajeParseado))
                        {
                            MessageBox.Show("sampleOrder.payment.input.invalid".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        porcentajeIngresado = Math.Round(porcentajeParseado, 2);
                    }

                    if (!montoIngresado.HasValue && !porcentajeIngresado.HasValue)
                    {
                        MessageBox.Show("sampleOrder.payment.input.required".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (porcentajeIngresado.HasValue)
                    {
                        if (porcentajeIngresado.Value <= 0 || porcentajeIngresado.Value > 100)
                        {
                            MessageBox.Show("sampleOrder.payment.input.percentRange".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var montoCalculado = Math.Round(saldoPendiente * (porcentajeIngresado.Value / 100m), 2);
                        if (!montoIngresado.HasValue)
                        {
                            montoIngresado = montoCalculado;
                        }
                        else if (Math.Abs(montoIngresado.Value - montoCalculado) > 0.01m)
                        {
                            MessageBox.Show(string.Format("sampleOrder.payment.input.mismatch".Traducir(), montoCalculado.ToString("C2")), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    if (!montoIngresado.HasValue || montoIngresado.Value <= 0)
                    {
                        MessageBox.Show("sampleOrder.payment.input.required".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (montoIngresado.Value > saldoPendiente)
                    {
                        MessageBox.Show(string.Format("sampleOrder.payment.input.exceed".Traducir(), saldoPendiente.ToString("C2")), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                RegistrarMovimientoPago("PedidoMuestra.Pago.Agregar", "sampleOrder.log.payment.add.percent", porcentaje, monto, fecha);
            }
            else
            {
                RegistrarMovimientoPago("PedidoMuestra.Pago.Agregar", "sampleOrder.log.payment.add.manual", monto, fecha);
            }
        }

        private void RegistrarPagoEliminado(PagoRegistrado pago)
        {
            if (pago == null)
                return;

            var monto = pago.Monto.ToString("C2");
            var fecha = pago.Fecha.ToString("g");

            if (pago.Porcentaje.HasValue)
            {
                var porcentaje = pago.Porcentaje.Value.ToString("0.##");
                RegistrarMovimientoPago("PedidoMuestra.Pago.Eliminar", "sampleOrder.log.payment.remove.percent", porcentaje, monto, fecha);
            }
            else
            {
                RegistrarMovimientoPago("PedidoMuestra.Pago.Eliminar", "sampleOrder.log.payment.remove.manual", monto, fecha);
            }
        }

        private void RegistrarMovimientoPago(string accion, string claveMensaje, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "PedidosMuestra");
            }
            catch
            {
                // Evitar que fallos de bitácora interrumpan el flujo de pagos
            }

            _logService.LogInfo(mensaje, "PedidosMuestra", SessionContext.NombreUsuario);
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

        private bool MarcarDetallesComoPagados()
        {
            var idPagado = BuscarEstadoMuestraId(ESTADO_PAGADO);
            if (!idPagado.HasValue)
                return false;

            var modificados = new List<string>();

            foreach (var detalle in _detalles)
            {
                if (detalle == null)
                    continue;

                var estadoActual = detalle.EstadoMuestra ?? ObtenerNombreEstado(detalle.IdEstadoMuestra);
                if (string.Equals(estadoActual, ESTADO_PAGADO, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!string.Equals(estadoActual, ESTADO_A_FACTURAR, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(estadoActual, ESTADO_FACTURADO, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                detalle.IdEstadoMuestra = idPagado;
                detalle.EstadoMuestra = ESTADO_PAGADO;
                RecalcularSubtotal(detalle);

                var nombreProducto = string.IsNullOrWhiteSpace(detalle.NombreProducto) ? "N/A" : detalle.NombreProducto;
                modificados.Add(nombreProducto);
            }

            if (modificados.Count == 0)
                return false;

            var nombres = string.Join(", ", modificados);
            RegistrarAccion("PedidoMuestra.Detalle.MarcarPagado", "sampleOrder.log.detail.paid", nombres);
            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarPedido();
        }

        private void GuardarPedido()
        {
            if (cmbCliente.SelectedItem == null)
            {
                MessageBox.Show("sampleOrder.client.required".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_detalles.Count == 0)
            {
                MessageBox.Show("sampleOrder.detail.required".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!(cmbCliente.SelectedValue is Guid idCliente) || idCliente == Guid.Empty)
                {
                    MessageBox.Show("sampleOrder.client.required".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ActualizarEstadoPedidoDesdeDetalles();

                var fechaCreacionUtc = _fechaPedido.Kind == DateTimeKind.Local
                    ? _fechaPedido.ToUniversalTime()
                    : _fechaPedido;

                var pedido = new PedidoMuestra
                {
                    IdPedidoMuestra = _pedidoOriginal?.IdPedidoMuestra ?? Guid.Empty,
                    IdCliente = idCliente,
                    NumeroPedidoMuestra = _numeroPedido,
                    FechaCreacion = fechaCreacionUtc,
                    FechaDevolucionEsperada = ObtenerFechaDevolucionEsperadaGeneral(),
                    DireccionEntrega = txtDireccion.Text?.Trim(),
                    PersonaContacto = txtContacto.Text?.Trim(),
                    EmailContacto = txtEmail.Text?.Trim(),
                    TelefonoContacto = txtTelefono.Text?.Trim(),
                    Observaciones = txtObservaciones.Text?.Trim(),
                    IdEstadoPedidoMuestra = _estadoPedidoActual,
                    MontoPagado = _montoPagadoActual
                };

                if (_pagosRegistrados.Count > 0)
                {
                    pedido.Pagos = _pagosRegistrados
                        .Select(p => new PedidoMuestraPago
                        {
                            IdPedidoMuestraPago = p.IdPago,
                            IdPedidoMuestra = pedido.IdPedidoMuestra,
                            Monto = p.Monto,
                            Porcentaje = p.Porcentaje,
                            FechaRegistro = ArgentinaDateTimeHelper.ToUtc(p.Fecha)
                        })
                        .ToList();
                }
                else
                {
                    pedido.Pagos = new List<PedidoMuestraPago>();
                }

                if (_adjuntos != null && _adjuntos.Count > 0)
                {
                    pedido.Adjuntos = _adjuntos
                        .Select(vm => MapearAdjuntoDominio(vm, pedido.IdPedidoMuestra))
                        .Where(a => a != null)
                        .ToList();
                }
                else
                {
                    pedido.Adjuntos = new List<ArchivoAdjunto>();
                }

                foreach (var detalle in _detalles)
                {
                    if (!detalle.IdProducto.HasValue)
                        continue;

                    pedido.Detalles.Add(new DetalleMuestra
                    {
                        IdDetalleMuestra = detalle.IdDetalleMuestra,
                        IdProducto = detalle.IdProducto.Value,
                        Cantidad = 1,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Subtotal = detalle.Subtotal,
                        IdEstadoMuestra = detalle.IdEstadoMuestra,
                        FechaDevolucion = detalle.FechaDevolucion
                    });
                }

                ResultadoOperacion resultado;
                if (_pedidoOriginal == null)
                {
                    resultado = _pedidoMuestraService.CrearPedidoMuestra(pedido);
                }
                else
                {
                    resultado = _pedidoMuestraService.ActualizarPedidoMuestra(pedido);
                }

                if (resultado.EsValido)
                {
                    var clave = _pedidoOriginal == null
                        ? "sampleOrder.log.created"
                        : "sampleOrder.log.updated";

                    var accion = _pedidoOriginal == null ? "PedidoMuestra.Alta" : "PedidoMuestra.Edicion";
                    RegistrarAccion(accion, clave);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error guardando pedido de muestra / Error saving sample order", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sampleOrder.save.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dgvDetalles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEditarDetalle_Click(sender, EventArgs.Empty);
            }
        }

        private void dgvDetalles_SelectionChanged(object sender, EventArgs e)
        {
            var haySeleccion = ObtenerDetalleSeleccionado() != null;
            btnEditarDetalle.Enabled = haySeleccion;
            btnEliminarDetalle.Enabled = haySeleccion;
            btnExtenderDias.Enabled = haySeleccion;
        }

        private void DgvDetalles_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvDetalles.ClearSelection();
                dgvDetalles.Rows[e.RowIndex].Selected = true;
                dgvDetalles.CurrentCell = dgvDetalles.Rows[e.RowIndex].Cells[Math.Max(e.ColumnIndex, 0)];
            }
        }

        private void ExtenderDetalleDesdeContexto()
        {
            var dias = SolicitarDiasExtension();
            if (dias.HasValue)
            {
                TryExtenderDetalleSeleccionado(dias.Value);
            }
        }

        private bool TryExtenderDetalleSeleccionado(int dias)
        {
            var detalle = ObtenerDetalleSeleccionado();
            if (detalle == null)
            {
                MessageBox.Show("sampleOrder.detail.selectRequired".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (EsEstadoDevuelto(detalle))
            {
                MessageBox.Show("sampleOrder.extend.onlyPending".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            var baseFecha = detalle.FechaDevolucion ?? DateTime.Today;
            detalle.FechaDevolucion = baseFecha.AddDays(dias);
            dgvDetalles.Refresh();
            ActualizarResumen();

            var nombreProducto = string.IsNullOrWhiteSpace(detalle.NombreProducto) ? "N/A" : detalle.NombreProducto;
            RegistrarAccion("PedidoMuestra.Detalle.Extender", "sampleOrder.log.detail.extend", nombreProducto, dias);

            return true;
        }

        private int? SolicitarDiasExtension()
        {
            using (var dialog = new Form())
            {
                dialog.Text = "sampleOrder.extend.days".Traducir();
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.ClientSize = new Size(260, 110);
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ShowInTaskbar = false;

                var lblMensaje = new Label
                {
                    AutoSize = true,
                    Text = "sampleOrder.extend.prompt".Traducir(),
                    Location = new Point(12, 12)
                };

                var nudDias = new NumericUpDown
                {
                    Minimum = 1,
                    Maximum = 60,
                    Value = nudDiasExtension.Value,
                    Location = new Point(12, 40),
                    Size = new Size(80, 20)
                };

                var btnAceptar = new Button
                {
                    Text = "form.accept".Traducir(),
                    DialogResult = DialogResult.OK,
                    Location = new Point(110, 70),
                    Size = new Size(60, 25)
                };

                var btnCancelar = new Button
                {
                    Text = "form.cancel".Traducir(),
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(180, 70),
                    Size = new Size(60, 25)
                };

                dialog.Controls.Add(lblMensaje);
                dialog.Controls.Add(nudDias);
                dialog.Controls.Add(btnAceptar);
                dialog.Controls.Add(btnCancelar);

                dialog.AcceptButton = btnAceptar;
                dialog.CancelButton = btnCancelar;

                return dialog.ShowDialog(this) == DialogResult.OK
                    ? (int?)nudDias.Value
                    : null;
            }
        }

        private void CambiarEstadoDetalle(EstadoMuestra estado)
        {
            if (estado == null)
                return;

            var detalle = ObtenerDetalleSeleccionado();
            if (detalle == null)
            {
                MessageBox.Show("sampleOrder.detail.selectRequired".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            detalle.IdEstadoMuestra = estado.IdEstadoMuestra;
            detalle.EstadoMuestra = estado.NombreEstadoMuestra;

            if (string.Equals(detalle.EstadoMuestra, ESTADO_DEVUELTO, StringComparison.OrdinalIgnoreCase) && !detalle.FechaDevolucion.HasValue)
            {
                detalle.FechaDevolucion = DateTime.Today;
            }

            RecalcularSubtotal(detalle);
            dgvDetalles.Refresh();
            ActualizarResumen();

            var nombreProducto = string.IsNullOrWhiteSpace(detalle.NombreProducto) ? "N/A" : detalle.NombreProducto;
            RegistrarAccion("PedidoMuestra.Detalle.CambiarEstado", "sampleOrder.log.detail.stateChange", nombreProducto, estado.NombreEstadoMuestra);
        }

        private bool EsEstadoDevuelto(PedidoMuestraDetalleViewModel detalle)
        {
            if (detalle == null)
                return false;

            var estado = detalle.EstadoMuestra ?? ObtenerNombreEstado(detalle.IdEstadoMuestra);
            return string.Equals(estado, ESTADO_DEVUELTO, StringComparison.OrdinalIgnoreCase);
        }

        private bool EsEstadoPagado(PedidoMuestraDetalleViewModel detalle)
        {
            if (detalle == null)
                return false;

            var estado = detalle.EstadoMuestra ?? ObtenerNombreEstado(detalle.IdEstadoMuestra);
            return string.Equals(estado, ESTADO_PAGADO, StringComparison.OrdinalIgnoreCase);
        }

        private Guid? BuscarEstadoMuestraId(string nombre)
        {
            var estado = _estadosMuestra.FirstOrDefault(e => string.Equals(e.NombreEstadoMuestra, nombre, StringComparison.OrdinalIgnoreCase));
            return estado?.IdEstadoMuestra;
        }

        private DateTime? ObtenerFechaDevolucionEsperadaGeneral()
        {
            var fechas = _detalles
                .Where(d => d.FechaDevolucion.HasValue)
                .Select(d => d.FechaDevolucion.Value.Date)
                .OrderBy(f => f)
                .ToList();

            return fechas.Count > 0 ? fechas.First() : (DateTime?)null;
        }

        private void RecalcularSubtotal(PedidoMuestraDetalleViewModel detalle)
        {
            if (detalle == null)
                return;

            detalle.Subtotal = EsDetalleFacturable(detalle)
                ? detalle.PrecioUnitario
                : 0m;
        }

        private bool EsDetalleFacturable(PedidoMuestraDetalleViewModel detalle)
        {
            if (detalle == null)
                return false;

            var estado = detalle.EstadoMuestra ?? ObtenerNombreEstado(detalle.IdEstadoMuestra);
            return string.Equals(estado, ESTADO_A_FACTURAR, StringComparison.OrdinalIgnoreCase)
                || string.Equals(estado, ESTADO_FACTURADO, StringComparison.OrdinalIgnoreCase)
                || string.Equals(estado, ESTADO_PAGADO, StringComparison.OrdinalIgnoreCase);
        }

        private string ObtenerNombreEstado(Guid? idEstado)
        {
            if (!idEstado.HasValue || idEstado.Value == Guid.Empty)
                return null;

            return _estadosMuestra.FirstOrDefault(e => e.IdEstadoMuestra == idEstado.Value)?.NombreEstadoMuestra;
        }

        private void RegistrarAccion(string accion, string claveMensaje, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);

            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "PedidosMuestra");
            }
            catch
            {
                // No interrumpir el flujo si falla el registro en bitácora
            }

            _logService.LogInfo(mensaje, "PedidosMuestra", SessionContext.NombreUsuario);
        }

        private Dictionary<string, (string Es, string En)> CrearDiccionarioMensajes()
        {
            return new Dictionary<string, (string Es, string En)>
            {
                ["sampleOrder.log.detail.add"] = ("Se agregó la muestra {0}.", "Sample {0} was added."),
                ["sampleOrder.log.detail.update"] = ("Se actualizó la muestra {0}.", "Sample {0} was updated."),
                ["sampleOrder.log.detail.delete"] = ("Se eliminó la muestra {0}.", "Sample {0} was removed."),
                ["sampleOrder.log.requestBilling"] = ("Se solicitó facturación para la muestra {0}.", "Billing was requested for sample {0}."),
                ["sampleOrder.log.payment.add.percent"] = ("Se registró un pago del {0}% por {1} el {2}.", "A payment of {0}% for {1} on {2} was recorded."),
                ["sampleOrder.log.payment.add.manual"] = ("Se registró un pago manual por {0} el {1}.", "A manual payment of {0} on {1} was recorded."),
                ["sampleOrder.log.payment.remove.percent"] = ("Se eliminó el pago del {0}% por {1} registrado el {2}.", "The payment of {0}% for {1} recorded on {2} was removed."),
                ["sampleOrder.log.payment.remove.manual"] = ("Se eliminó el pago manual por {0} registrado el {1}.", "The manual payment of {0} recorded on {1} was removed."),
                ["sampleOrder.log.detail.extend"] = ("Se extendió la devolución de {0} en {1} día(s).", "Return deadline for {0} extended by {1} day(s)."),
                ["sampleOrder.log.detail.stateChange"] = ("Se cambió el estado de {0} a {1}.", "State for {0} changed to {1}."),
                ["sampleOrder.log.detail.paid"] = ("Se marcaron como pagadas las muestras: {0}.", "Samples marked as paid: {0}."),
                ["sampleOrder.log.created"] = ("Se creó el pedido de muestra.", "Sample order was created."),
                ["sampleOrder.log.updated"] = ("Se actualizó el pedido de muestra.", "Sample order was updated."),
                ["sampleOrder.log.orderStateChange"] = ("El pedido de muestra cambió su estado a {0}.", "Sample order state changed to {0}."),
                ["sampleOrder.log.detail.overdueDetected"] = ("Se detectaron devoluciones vencidas al abrir el pedido.", "Overdue returns were detected when opening the sample order."),
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

        private void VerificarVencimiento()
        {
            if (_detalles.Count == 0)
                return;

            var hayPendientes = _detalles.Any(d => !EsEstadoDevuelto(d));
            if (!hayPendientes)
                return;

            var hayVencidas = _detalles
                .Where(d => !EsEstadoDevuelto(d) && d.FechaDevolucion.HasValue)
                .Any(d => d.FechaDevolucion.Value.Date < DateTime.Today);

            if (hayVencidas)
            {
                RegistrarAccion("PedidoMuestra.Detalle.Vencidos", "sampleOrder.log.detail.overdueDetected");
            }
        }
    }
}