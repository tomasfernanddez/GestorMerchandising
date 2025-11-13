using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using BLL.Helpers;
using BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using Services;
using Services.BLL.Interfaces;
using UI.Helpers;
using UI.Localization;

namespace UI
{
    public partial class PedidosMuestraForm : Form
    {
        private readonly IPedidoMuestraService _pedidoMuestraService;
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogService _logService;
        private readonly Dictionary<string, (string Es, string En)> _diccionarioMensajes;

        private BindingList<PedidoMuestraRow> _rows;
        private List<EstadoPedidoMuestra> _estadosPedido;
        private List<EstadoMuestra> _estadosMuestra;
        private bool _suspendFilters;
        private const string ESTADO_A_FACTURAR = "Pendiente de Pago";

        private sealed class FiltroOpcion<T>
        {
            public FiltroOpcion(string texto, T valor)
            {
                Texto = texto;
                Valor = valor;
            }

            public string Texto { get; }
            public T Valor { get; }

            public override string ToString() => Texto;
        }

        private sealed class PedidoMuestraRow
        {
            public Guid IdPedidoMuestra { get; set; }
            public string Numero { get; set; }
            public string Cliente { get; set; }
            public string Estado { get; set; }
            public Guid? IdEstado { get; set; }
            public int CantidadProductos { get; set; }
            public DateTime FechaPedido { get; set; }
            public DateTime? FechaDevolucionEsperada { get; set; }
            public decimal SaldoPendiente { get; set; }
            public string Contacto { get; set; }
            public bool Vencido { get; set; }
        }

        public PedidosMuestraForm(
            IPedidoMuestraService pedidoMuestraService,
            IClienteService clienteService,
            IProductoService productoService,
            IBitacoraService bitacoraService,
            ILogService logService)
        {
            _pedidoMuestraService = pedidoMuestraService ?? throw new ArgumentNullException(nameof(pedidoMuestraService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _diccionarioMensajes = CrearDiccionarioMensajes();

            InitializeComponent();
        }

        private void PedidosMuestraForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarDatosReferencia();
            ConfigurarGrid();
            CargarPedidos();
            ConfigurarFiltros();
            WireEvents();
        }

        private void ApplyTexts()
        {
            Text = "sampleOrder.list.title".Traducir();
            tsbNuevo.Text = "sampleOrder.list.new".Traducir();
            tsbEditar.Text = "sampleOrder.list.edit".Traducir();
            tsbActualizar.Text = "form.refresh".Traducir();
            tsbCancelar.Text = "order.cancel.button".Traducir();
            tsbPedirFacturacion.Text = "sampleOrder.request.billing".Traducir();
            tsbExtenderDias.Text = "sampleOrder.extend.due".Traducir();
            tslBuscar.Text = "form.search".Traducir();
            btnBuscar.Text = "form.filter".Traducir();
            tslEstado.Text = "sampleOrder.state".Traducir();
            tslSaldo.Text = "sampleOrder.balance.filter".Traducir();

            if (dgvPedidos.Columns.Count > 0)
            {
                dgvPedidos.Columns[nameof(PedidoMuestraRow.Numero)].HeaderText = "sampleOrder.number".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.FechaPedido)].HeaderText = "sampleOrder.created.date".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.Cliente)].HeaderText = "sampleOrder.client".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.Estado)].HeaderText = "sampleOrder.state".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.CantidadProductos)].HeaderText = "sampleOrder.list.items".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.FechaDevolucionEsperada)].HeaderText = "sampleOrder.return.expected".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.SaldoPendiente)].HeaderText = "sampleOrder.summary.balance".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.Contacto)].HeaderText = "sampleOrder.contact.name".Traducir();
            }
        }

        private void CargarDatosReferencia()
        {
            try
            {
                _estadosPedido = _pedidoMuestraService.ObtenerEstadosPedido().OrderBy(e => e.NombreEstadoPedidoMuestra).ToList();
                _estadosMuestra = _pedidoMuestraService.ObtenerEstadosMuestra().ToList();
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("PedidoMuestra.Referencias", "sampleOrder.list.log.referenceError", ex, friendly);
                MessageBox.Show("sampleOrder.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrid()
        {
            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPedidos.Columns.Clear();

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.Numero),
                Name = nameof(PedidoMuestraRow.Numero),
                HeaderText = "sampleOrder.number".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 110,
                MinimumWidth = 90
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.FechaPedido),
                Name = nameof(PedidoMuestraRow.FechaPedido),
                HeaderText = "sampleOrder.created.date".Traducir(),
                DefaultCellStyle = { Format = "d" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 110,
                MinimumWidth = 90
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.Cliente),
                Name = nameof(PedidoMuestraRow.Cliente),
                HeaderText = "sampleOrder.client".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 220,
                MinimumWidth = 160
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.Estado),
                Name = nameof(PedidoMuestraRow.Estado),
                HeaderText = "sampleOrder.state".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 120,
                MinimumWidth = 110
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.CantidadProductos),
                Name = nameof(PedidoMuestraRow.CantidadProductos),
                HeaderText = "sampleOrder.list.items".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 90,
                MinimumWidth = 80
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.FechaDevolucionEsperada),
                Name = nameof(PedidoMuestraRow.FechaDevolucionEsperada),
                HeaderText = "sampleOrder.return.expected".Traducir(),
                DefaultCellStyle = { Format = "d" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 130,
                MinimumWidth = 120
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.SaldoPendiente),
                Name = nameof(PedidoMuestraRow.SaldoPendiente),
                HeaderText = "sampleOrder.summary.balance".Traducir(),
                DefaultCellStyle = { Format = "C2" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 120,
                MinimumWidth = 110
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.Contacto),
                Name = nameof(PedidoMuestraRow.Contacto),
                HeaderText = "sampleOrder.contact.name".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 160,
                MinimumWidth = 140
            });

            _rows = new BindingList<PedidoMuestraRow>();
            dgvPedidos.DataSource = _rows;
            dgvPedidos.SelectionChanged += (s, e) => ActualizarAcciones();
            dgvPedidos.DataBindingComplete += (s, e) => ResaltarVencidos();
            dgvPedidos.RowPrePaint += DgvPedidos_RowPrePaint;
        }

        private void ConfigurarFiltros()
        {
            _suspendFilters = true;
            try
            {
                var placeholder = "form.select.optional".Traducir();

                var estados = new List<EstadoPedidoMuestra>();
                estados.Add(new EstadoPedidoMuestra
                {
                    IdEstadoPedidoMuestra = Guid.Empty,
                    NombreEstadoPedidoMuestra = placeholder
                });
                if (_estadosPedido != null)
                {
                    estados.AddRange(_estadosPedido);
                }

                var comboEstados = cmbEstado.ComboBox;
                comboEstados.DisplayMember = nameof(EstadoPedidoMuestra.NombreEstadoPedidoMuestra);
                comboEstados.ValueMember = nameof(EstadoPedidoMuestra.IdEstadoPedidoMuestra);
                comboEstados.DataSource = estados;
                comboEstados.Format -= ComboEstados_Format;
                comboEstados.Format += ComboEstados_Format;
                comboEstados.SelectedValue = Guid.Empty;

                var opcionesSaldo = new List<FiltroOpcion<bool?>>
                {
                    new FiltroOpcion<bool?>(placeholder, null),
                    new FiltroOpcion<bool?>("sampleOrder.balance.with".Traducir(), true),
                    new FiltroOpcion<bool?>("sampleOrder.balance.without".Traducir(), false)
                };
                var comboSaldo = cmbSaldo.ComboBox;
                comboSaldo.DisplayMember = nameof(FiltroOpcion<bool?>.Texto);
                comboSaldo.ValueMember = nameof(FiltroOpcion<bool?>.Valor);
                comboSaldo.DataSource = opcionesSaldo;
                if (comboSaldo.Items.Count > 0)
                {
                    comboSaldo.SelectedIndex = 0;
                }
            }
            finally
            {
                _suspendFilters = false;
            }

            ActualizarAcciones();
        }

        private void ComboEstados_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is EstadoPedidoMuestra estado)
            {
                e.Value = LocalizationHelper.TranslateSampleOrderState(estado.NombreEstadoPedidoMuestra);
            }
        }

        private void WireEvents()
        {
            txtBuscar.TextChanged += (s, e) => { if (!_suspendFilters) AplicarFiltros(); };
            txtBuscar.KeyDown += TxtBuscar_KeyDown;
            cmbEstado.SelectedIndexChanged += Filtros_SelectedIndexChanged;
            cmbSaldo.SelectedIndexChanged += Filtros_SelectedIndexChanged;
            dgvPedidos.SelectionChanged += (s, e) => ActualizarAcciones();
        }

        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                AplicarFiltros();
            }
        }

        private void Filtros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suspendFilters)
                return;

            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            try
            {
                var filtro = new PedidoMuestraFiltro
                {
                    IdEstadoPedido = ObtenerGuidSeleccionado(cmbEstado),
                    ConSaldoPendiente = ObtenerValorSeleccionado(cmbSaldo),
                    TextoBusqueda = ObtenerTextoBusqueda(),
                    IncluirDetalles = true
                };

                var pedidos = _pedidoMuestraService.ObtenerPedidosMuestra(filtro);

                _rows.Clear();
                foreach (var pedido in pedidos)
                {
                    var fechaEsperada = CalcularFechaDevolucionEsperada(pedido);
                    var fechaPedido = ArgentinaDateTimeHelper.ToArgentina(pedido.FechaCreacion);
                    var fechaDevolucionCompleta = pedido.FechaDevolucion.HasValue
                        ? ArgentinaDateTimeHelper.ToArgentina(pedido.FechaDevolucion.Value).Date
                        : (DateTime?)null;
                    var vencido = !fechaDevolucionCompleta.HasValue
                        && fechaEsperada.HasValue
                        && fechaEsperada.Value.Date < DateTime.Today;

                    var estadosDetalle = pedido.Detalles?
                        .Select(d => d.EstadoMuestra?.NombreEstadoMuestra
                                     ?? _estadosMuestra?.FirstOrDefault(em => em.IdEstadoMuestra == d.IdEstadoMuestra)?.NombreEstadoMuestra
                                     ?? string.Empty)
                        .ToList() ?? new List<string>();

                    var calculado = PedidoMuestraEstadoResolver.CalcularEstado(estadosDetalle, _estadosPedido);
                    var estadoId = calculado?.IdEstado ?? pedido.IdEstadoPedidoMuestra;
                    var estadoNombre = calculado?.NombreEstado
                        ?? pedido.EstadoPedidoMuestra?.NombreEstadoPedidoMuestra
                        ?? _estadosPedido?.FirstOrDefault(e => e.IdEstadoPedidoMuestra == estadoId)?.NombreEstadoPedidoMuestra;

                    if (filtro.IdEstadoPedido.HasValue)
                    {
                        if (!estadoId.HasValue || estadoId.Value != filtro.IdEstadoPedido.Value)
                        {
                            continue;
                        }
                    }
                    else if (EsEstadoFinalizado(estadoNombre))
                    {
                        continue;
                    }

                    _rows.Add(new PedidoMuestraRow
                    {
                        IdPedidoMuestra = pedido.IdPedidoMuestra,
                        Numero = FormatearNumeroPedido(pedido.NumeroPedidoMuestra),
                        Cliente = FormatearNombreCliente(pedido.Cliente),
                        Estado = LocalizationHelper.TranslateSampleOrderState(estadoNombre),
                        IdEstado = estadoId,
                        CantidadProductos = pedido.Detalles?.Count ?? 0,
                        FechaPedido = fechaPedido,
                        FechaDevolucionEsperada = fechaEsperada,
                        SaldoPendiente = pedido.SaldoPendiente,
                        Contacto = pedido.PersonaContacto,
                        Vencido = vencido
                    });
                }

                ResaltarVencidos();
                ActualizarAcciones();
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("PedidoMuestra.Listado", "sampleOrder.list.log.error", ex, friendly);
                MessageBox.Show("sampleOrder.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResaltarVencidos()
        {
            foreach (DataGridViewRow row in dgvPedidos.Rows)
            {
                if (row.DataBoundItem is PedidoMuestraRow pedido)
                {
                    if (pedido.Vencido)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = Color.White;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = SystemColors.Window;
                        row.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                    }
                }
            }
        }

        private void DgvPedidos_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = dgvPedidos.Rows[e.RowIndex];
            if (row?.DataBoundItem is PedidoMuestraRow pedido)
            {
                if (pedido.Vencido)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = SystemColors.Window;
                    row.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private Guid? ObtenerGuidSeleccionado(ToolStripComboBox combo)
        {
            if (combo?.ComboBox?.SelectedValue is Guid guid && guid != Guid.Empty)
                return guid;

            return null;
        }

        private bool? ObtenerValorSeleccionado(ToolStripComboBox combo)
            => (combo?.SelectedItem as FiltroOpcion<bool?>)?.Valor;

        private string ObtenerTextoBusqueda()
        {
            var texto = txtBuscar.Text?.Trim();
            return string.IsNullOrWhiteSpace(texto) ? null : texto;
        }

        private string FormatearNumeroPedido(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                return string.Empty;

            var digits = new string(numero.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(digits))
                return numero.Trim();

            return digits.Length <= 6 ? digits.PadLeft(6, '0') : digits;
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

        private bool EsEstadoFinalizado(string nombreEstado)
        {
            if (string.IsNullOrWhiteSpace(nombreEstado))
                return false;

            var compare = CultureInfo.InvariantCulture.CompareInfo;
            return compare.IndexOf(nombreEstado, "final", CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }

        private DateTime? CalcularFechaDevolucionEsperada(PedidoMuestra pedido)
        {
            if (pedido == null)
                return null;

            var fechas = pedido.Detalles?
                .Where(d => d.FechaDevolucion.HasValue)
                .Select(d => d.FechaDevolucion.Value.Date)
                .OrderBy(f => f)
                .ToList();

            if (fechas != null && fechas.Count > 0)
                return fechas.First();

            return pedido.FechaDevolucionEsperada;
        }

        private void ActualizarAcciones()
        {
            var haySeleccion = dgvPedidos.CurrentRow?.DataBoundItem is PedidoMuestraRow;
            tsbCancelar.Enabled = haySeleccion;
            tsbEditar.Enabled = haySeleccion;
            tsbPedirFacturacion.Enabled = haySeleccion;
            tsbExtenderDias.Enabled = haySeleccion;
        }

        private int? PedirDiasExtension()
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
                    Value = 1,
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

        private bool EsDetalleDevuelto(DetalleMuestra detalle)
        {
            if (detalle == null)
                return false;

            var nombreEstado = detalle.EstadoMuestra?.NombreEstadoMuestra
                ?? _estadosMuestra?.FirstOrDefault(e => e.IdEstadoMuestra == detalle.IdEstadoMuestra)?.NombreEstadoMuestra;

            return string.Equals(nombreEstado, "Devuelto", StringComparison.OrdinalIgnoreCase);
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            AbrirEditor();
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            AbrirEditor(ObtenerPedidoSeleccionado());
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            CargarPedidos();
        }

        private void tsbCancelar_Click(object sender, EventArgs e)
        {
            var idPedido = ObtenerPedidoSeleccionado();
            if (!idPedido.HasValue)
                return;

            var row = dgvPedidos.CurrentRow?.DataBoundItem as PedidoMuestraRow;
            var numeroPedido = row?.Numero ?? idPedido.Value.ToString();

            var confirm = MessageBox.Show(
                "sampleOrder.cancel.confirm".Traducir(),
                Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                var comentario = ObtenerMensaje("sampleOrder.cancel.log.comment", SessionContext.NombreUsuario ?? "Sistema");
                var resultado = _pedidoMuestraService.CancelarPedidoMuestra(
                    idPedido.Value,
                    SessionContext.NombreUsuario,
                    comentario);

                if (resultado.EsValido)
                {
                    RegistrarAccion("PedidoMuestra.Cancelar", "sampleOrder.cancel.log.success", numeroPedido);
                    CargarPedidos();
                }
                else
                {
                    RegistrarFallo("PedidoMuestra.Cancelar", "sampleOrder.cancel.log.failure", resultado.Mensaje, numeroPedido, resultado.Mensaje);
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("PedidoMuestra.Cancelar", "sampleOrder.cancel.log.error", ex, numeroPedido, friendly);
                MessageBox.Show("sampleOrder.cancel.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void AbrirEditor(Guid? idPedido = null)
        {
            try
            {
                using (var form = new PedidoMuestraForm(_pedidoMuestraService, _clienteService, _productoService, _bitacoraService, _logService, idPedido))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        CargarPedidos();
                    }
                }
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("PedidoMuestra.Formulario", "sampleOrder.open.log.error", ex, friendly);
                MessageBox.Show("sampleOrder.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Guid? ObtenerPedidoSeleccionado()
        {
            if (dgvPedidos.CurrentRow?.DataBoundItem is PedidoMuestraRow row)
            {
                return row.IdPedidoMuestra;
            }
            return null;
        }

        private void tsbPedirFacturacion_Click(object sender, EventArgs e)
        {
            var idPedido = ObtenerPedidoSeleccionado();
            if (!idPedido.HasValue)
                return;

            var row = dgvPedidos.CurrentRow?.DataBoundItem as PedidoMuestraRow;
            var numeroPedido = row?.Numero ?? idPedido.Value.ToString();

            try
            {
                var pedido = _pedidoMuestraService.ObtenerPedidoMuestra(idPedido.Value, incluirDetalles: true);
                if (pedido == null)
                    return;

                if (string.IsNullOrWhiteSpace(numeroPedido))
                {
                    numeroPedido = FormatearNumeroPedido(pedido.NumeroPedidoMuestra);
                }

                var estadoFacturar = _estadosMuestra
                    .FirstOrDefault(em => string.Equals(em.NombreEstadoMuestra, ESTADO_A_FACTURAR, StringComparison.OrdinalIgnoreCase));
                if (estadoFacturar == null)
                {
                    MessageBox.Show("sampleOrder.state.missing".Traducir(ESTADO_A_FACTURAR), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var modifico = false;
                foreach (var detalle in pedido.Detalles)
                {
                    var estadoActual = _estadosMuestra.
                        FirstOrDefault(em => em.IdEstadoMuestra == detalle.IdEstadoMuestra)?.NombreEstadoMuestra;
                    if (!string.Equals(estadoActual, "Devuelto", StringComparison.OrdinalIgnoreCase))
                    {
                        detalle.IdEstadoMuestra = estadoFacturar.IdEstadoMuestra;
                        modifico = true;
                    }
                }

                if (!modifico)
                {
                    MessageBox.Show("sampleOrder.request.billing.none".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                pedido.FechaDevolucionEsperada = CalcularFechaDevolucionEsperada(pedido);
                var resultado = _pedidoMuestraService.ActualizarPedidoMuestra(pedido);
                if (resultado.EsValido)
                {
                    RegistrarAccion("PedidoMuestra.PedirFacturacion", "sampleOrder.billing.log.success", numeroPedido);
                    CargarPedidos();
                }
                else
                {
                    RegistrarFallo("PedidoMuestra.PedirFacturacion", "sampleOrder.billing.log.failure", resultado.Mensaje, numeroPedido, resultado.Mensaje);
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("PedidoMuestra.PedirFacturacion", "sampleOrder.billing.log.error", ex, numeroPedido, friendly);
                MessageBox.Show("sampleOrder.save.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbExtenderDias_Click(object sender, EventArgs e)
        {
            var idPedido = ObtenerPedidoSeleccionado();
            if (!idPedido.HasValue)
                return;

            var row = dgvPedidos.CurrentRow?.DataBoundItem as PedidoMuestraRow;
            var numeroPedido = row?.Numero ?? idPedido.Value.ToString();

            try
            {
                var pedido = _pedidoMuestraService.ObtenerPedidoMuestra(idPedido.Value, incluirDetalles: true);
                if (pedido == null)
                    return;

                if (pedido.Detalles.All(d =>
                        string.Equals(d.EstadoMuestra?.NombreEstadoMuestra ?? string.Empty, "Devuelto", StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("sampleOrder.extend.onlyPending".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var dias = PedirDiasExtension();
                if (!dias.HasValue)
                    return;

                foreach (var detalle in pedido.Detalles)
                {
                    if (EsDetalleDevuelto(detalle))
                        continue;

                    var baseFecha = detalle.FechaDevolucion ?? pedido.FechaDevolucionEsperada ?? pedido.FechaEntrega ?? DateTime.Today;
                    detalle.FechaDevolucion = baseFecha.AddDays(dias.Value);
                }

                pedido.FechaDevolucionEsperada = CalcularFechaDevolucionEsperada(pedido);
                var resultado = _pedidoMuestraService.ActualizarPedidoMuestra(pedido);
                if (resultado.EsValido)
                {
                    RegistrarAccion("PedidoMuestra.ExtenderVencimiento", "sampleOrder.extend.log.success", numeroPedido, dias.Value);
                    CargarPedidos();
                }
                else
                {
                    RegistrarFallo("PedidoMuestra.ExtenderVencimiento", "sampleOrder.extend.log.failure", resultado.Mensaje, numeroPedido, dias.Value, resultado.Mensaje);
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("PedidoMuestra.ExtenderVencimiento", "sampleOrder.extend.log.error", ex, numeroPedido, friendly);
                MessageBox.Show("sampleOrder.save.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPedidos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                AbrirEditor(ObtenerPedidoSeleccionado());
            }
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
                // Evitar que errores de bitácora interrumpan el flujo
            }

            _logService.LogInfo(mensaje, "PedidosMuestra", SessionContext.NombreUsuario);
        }

        private void RegistrarFallo(string accion, string claveMensaje, string detalle, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "PedidosMuestra", false, detalle);
            }
            catch
            {
                // Ignorar errores de bitácora
            }

            _logService.LogWarning(mensaje, "PedidosMuestra", SessionContext.NombreUsuario);
        }

        private void RegistrarError(string accion, string claveMensaje, Exception ex, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "PedidosMuestra", false, ex?.Message);
            }
            catch
            {
                // Ignorar errores de bitácora
            }

            _logService.LogError(mensaje, ex, "PedidosMuestra", SessionContext.NombreUsuario);
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

        private Dictionary<string, (string Es, string En)> CrearDiccionarioMensajes()
        {
            return new Dictionary<string, (string Es, string En)>
            {
                ["sampleOrder.list.log.referenceError"] = ("Error al cargar datos de pedidos de muestra: {0}.", "Error loading sample order data: {0}."),
                ["sampleOrder.list.log.error"] = ("Error al listar pedidos de muestra: {0}.", "Error listing sample orders: {0}."),
                ["sampleOrder.cancel.log.comment"] = ("Pedido de muestra cancelado por {0}.", "Sample order cancelled by {0}."),
                ["sampleOrder.cancel.log.success"] = ("Se canceló el pedido de muestra {0}.", "Sample order {0} was cancelled."),
                ["sampleOrder.cancel.log.failure"] = ("No se pudo cancelar el pedido de muestra {0}: {1}.", "Could not cancel sample order {0}: {1}."),
                ["sampleOrder.cancel.log.error"] = ("Error al cancelar el pedido de muestra {0}: {1}.", "Error cancelling sample order {0}: {1}."),
                ["sampleOrder.open.log.error"] = ("Error al abrir el pedido de muestra: {0}.", "Error opening sample order: {0}."),
                ["sampleOrder.billing.log.success"] = ("Se solicitó la facturación del pedido de muestra {0}.", "Billing requested for sample order {0}."),
                ["sampleOrder.billing.log.failure"] = ("No se pudo solicitar la facturación del pedido de muestra {0}: {1}.", "Could not request billing for sample order {0}: {1}."),
                ["sampleOrder.billing.log.error"] = ("Error al solicitar la facturación del pedido de muestra {0}: {1}.", "Error requesting billing for sample order {0}: {1}."),
                ["sampleOrder.extend.log.success"] = ("Se extendió la devolución del pedido de muestra {0} en {1} día(s).", "Sample order {0} return due date extended by {1} day(s)."),
                ["sampleOrder.extend.log.failure"] = ("No se pudo extender la devolución del pedido de muestra {0} en {1} día(s): {2}.", "Could not extend sample order {0} return by {1} day(s): {2}."),
                ["sampleOrder.extend.log.error"] = ("Error al extender la devolución del pedido de muestra {0}: {1}.", "Error extending sample order {0} return: {1}."),
            };
        }
    }
}