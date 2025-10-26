using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using BLL.Helpers;
using BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using Services.BLL.Interfaces;
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
        private readonly PedidoMuestra _pedidoOriginal;
        private readonly bool _esEdicion;
        private readonly List<EstadoMuestra> _estadosMuestra;
        private readonly Dictionary<Control, string> _diccionarioTextos = new Dictionary<Control, string>();

        private BindingList<PedidoMuestraDetalleViewModel> _detalles = new BindingList<PedidoMuestraDetalleViewModel>();
        private List<Cliente> _clientes = new List<Cliente>();
        private List<Producto> _productos = new List<Producto>();
        private List<EstadoPedidoMuestra> _estadosPedido = new List<EstadoPedidoMuestra>();

        public PedidoMuestraForm(
            IPedidoMuestraService pedidoMuestraService,
            IClienteService clienteService,
            IProductoService productoService,
            IBitacoraService bitacoraService,
            ILogService logService,
            PedidoMuestra pedido,
            List<EstadoMuestra> estadosMuestra)
        {
            _pedidoMuestraService = pedidoMuestraService ?? throw new ArgumentNullException(nameof(pedidoMuestraService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _pedidoOriginal = pedido;
            _esEdicion = pedido != null;
            _estadosMuestra = estadosMuestra ?? new List<EstadoMuestra>();

            InitializeComponent();

            Load += PedidoMuestraForm_Load;
            FormClosed += (s, e) => Localization.Localization.LanguageChanged -= Localization_LanguageChanged;
        }

        private void PedidoMuestraForm_Load(object sender, EventArgs e)
        {
            Localization.Localization.LanguageChanged += Localization_LanguageChanged;
            ConstruirDiccionario();
            CargarDatosReferencia();
            ConfigurarGrid();
            CargarPedido();
            ApplyTexts();
            WireUp();
        }

        private void Localization_LanguageChanged(object sender, EventArgs e)
        {
            ApplyTexts();
        }

        private void WireUp()
        {
            btnAgregar.Click += (s, e) => AgregarDetalle();
            btnEliminar.Click += (s, e) => EliminarDetalle();
            btnGuardar.Click += (s, e) => Guardar();
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            chkEntrega.CheckedChanged += (s, e) => dtpEntrega.Enabled = !chkEntrega.Checked;
            dgvDetalles.DataError += (s, e) => e.ThrowException = false;
        }

        private void ConstruirDiccionario()
        {
            _diccionarioTextos[lblNumero] = "sample.number";
            _diccionarioTextos[lblCliente] = "sample.client";
            _diccionarioTextos[lblContacto] = "sample.contact";
            _diccionarioTextos[lblEmail] = "sample.contact.email";
            _diccionarioTextos[lblTelefono] = "sample.contact.phone";
            _diccionarioTextos[lblEntrega] = "sample.deliveryDate";
            _diccionarioTextos[chkEntrega] = "sample.delivery.noDate";
            _diccionarioTextos[lblDevolucion] = "sample.expectedReturn";
            _diccionarioTextos[lblProrroga] = "sample.extraDays";
            _diccionarioTextos[lblObservaciones] = "sample.notes";
            _diccionarioTextos[btnAgregar] = "sample.detail.add";
            _diccionarioTextos[btnEliminar] = "sample.detail.remove";
            _diccionarioTextos[btnGuardar] = "form.save";
            _diccionarioTextos[btnCancelar] = "form.cancel";
            _diccionarioTextos[lblEstado] = "sample.state";
        }

        private void ApplyTexts()
        {
            Text = _esEdicion ? "sample.edit.title".Traducir() : "sample.new.title".Traducir();
            foreach (var kvp in _diccionarioTextos)
            {
                kvp.Key.Text = kvp.Value.Traducir();
            }

            if (dgvDetalles.Columns.Count > 0)
            {
                dgvDetalles.Columns["colProducto"].HeaderText = "sample.detail.product".Traducir();
                dgvDetalles.Columns["colEstado"].HeaderText = "sample.detail.state".Traducir();
                dgvDetalles.Columns["colFecha"].HeaderText = "sample.detail.returnDate".Traducir();
                dgvDetalles.Columns["colPrecio"].HeaderText = "sample.detail.price".Traducir();
                dgvDetalles.Columns["colFacturado"].HeaderText = "sample.detail.invoiced".Traducir();
                dgvDetalles.Columns["colComentario"].HeaderText = "sample.detail.comment".Traducir();
            }

            InicializarEstadosPedido();
            InicializarClientes();
        }

        private void CargarDatosReferencia()
        {
            try
            {
                _clientes = _clienteService.ObtenerClientesActivos()?.OrderBy(c => c.RazonSocial).ToList() ?? new List<Cliente>();
                _productos = _productoService.ObtenerTodos()?.OrderBy(p => p.NombreProducto).ToList() ?? new List<Producto>();
                _estadosPedido = _pedidoMuestraService.ObtenerEstadosPedido()?.ToList() ?? new List<EstadoPedidoMuestra>();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cargando datos de referencia de muestras / Error loading sample references", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sample.references.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InicializarClientes()
        {
            cboCliente.DisplayMember = nameof(Cliente.RazonSocial);
            cboCliente.ValueMember = nameof(Cliente.IdCliente);
            cboCliente.DataSource = _clientes;
        }

        private void InicializarEstadosPedido()
        {
            cboEstado.DisplayMember = nameof(EstadoPedidoMuestra.NombreEstadoPedidoMuestra);
            cboEstado.ValueMember = nameof(EstadoPedidoMuestra.IdEstadoPedidoMuestra);
            cboEstado.DataSource = _estadosPedido;
        }

        private void ConfigurarGrid()
        {
            dgvDetalles.AutoGenerateColumns = false;
            dgvDetalles.Columns.Clear();

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.IdDetalleMuestra),
                Name = "colId",
                Visible = false
            });

            dgvDetalles.Columns.Add(new DataGridViewComboBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.IdProducto),
                Name = "colProducto",
                DataSource = _productos,
                DisplayMember = nameof(Producto.NombreProducto),
                ValueMember = nameof(Producto.IdProducto),
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                FlatStyle = FlatStyle.Standard,
                FillWeight = 220,
                ValueType = typeof(Guid?),
                DefaultCellStyle = { NullValue = "" }
            });

            dgvDetalles.Columns.Add(new DataGridViewComboBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.IdEstadoMuestra),
                Name = "colEstado",
                DataSource = _estadosMuestra,
                DisplayMember = nameof(EstadoMuestra.NombreEstadoMuestra),
                ValueMember = nameof(EstadoMuestra.IdEstadoMuestra),
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                FlatStyle = FlatStyle.Standard,
                FillWeight = 160,
                ValueType = typeof(Guid?),
                DefaultCellStyle = { NullValue = "" }
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.FechaDevolucion),
                Name = "colFecha",
                DefaultCellStyle = { Format = "d" },
                FillWeight = 110
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.PrecioFacturacion),
                Name = "colPrecio",
                DefaultCellStyle = { Format = "C2" },
                FillWeight = 100
            });

            dgvDetalles.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.Facturado),
                Name = "colFacturado",
                FillWeight = 80
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraDetalleViewModel.Comentario),
                Name = "colComentario",
                FillWeight = 160
            });

            bsDetalles.DataSource = _detalles;
            dgvDetalles.DataSource = bsDetalles;
        }

        private void CargarPedido()
        {
            if (_esEdicion)
            {
                txtNumero.Text = _pedidoOriginal.NumeroCorrelativo;
                SeleccionarCliente(_pedidoOriginal.IdCliente);
                txtContacto.Text = _pedidoOriginal.PersonaContacto;
                txtEmail.Text = _pedidoOriginal.EmailContacto;
                txtTelefono.Text = _pedidoOriginal.TelefonoContacto;
                if (_pedidoOriginal.FechaEntrega.HasValue)
                {
                    dtpEntrega.Value = _pedidoOriginal.FechaEntrega.Value;
                    chkEntrega.Checked = false;
                }
                else
                {
                    chkEntrega.Checked = true;
                    dtpEntrega.Enabled = false;
                }
                if (_pedidoOriginal.FechaDevolucionEsperada.HasValue)
                {
                    dtpDevolucion.Value = _pedidoOriginal.FechaDevolucionEsperada.Value;
                }
                nudProrroga.Value = Math.Max(0, Math.Min(nudProrroga.Maximum, _pedidoOriginal.DiasProrroga));
                txtObservaciones.Text = _pedidoOriginal.Observaciones;
                SeleccionarEstadoPedido(_pedidoOriginal.IdEstadoPedidoMuestra);

                var detalles = _pedidoOriginal.Detalles?.Select(d => new PedidoMuestraDetalleViewModel
                {
                    IdDetalleMuestra = d.IdDetalleMuestra,
                    IdProducto = d.IdProducto,
                    IdEstadoMuestra = d.IdEstadoMuestra,
                    PrecioFacturacion = d.PrecioFacturacion,
                    Facturado = d.Facturado,
                    FechaDevolucion = d.FechaDevolucion,
                    Comentario = d.ComentarioDevolucion
                }).ToList() ?? new List<PedidoMuestraDetalleViewModel>();

                _detalles = new BindingList<PedidoMuestraDetalleViewModel>(detalles);
            }
            else
            {
                txtNumero.Text = _pedidoMuestraService.GenerarSiguienteNumero();
                if (_clientes.Any()) cboCliente.SelectedIndex = 0;
                dtpEntrega.Value = DateTime.Today;
                dtpDevolucion.Value = DateTime.Today.AddDays(30);
                _detalles = new BindingList<PedidoMuestraDetalleViewModel>();
            }

            bsDetalles.DataSource = _detalles;
            dgvDetalles.DataSource = bsDetalles;
        }

        private void SeleccionarCliente(Guid idCliente)
        {
            for (int i = 0; i < _clientes.Count; i++)
            {
                if (_clientes[i].IdCliente == idCliente)
                {
                    cboCliente.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SeleccionarEstadoPedido(Guid? idEstado)
        {
            if (!idEstado.HasValue) return;
            for (int i = 0; i < _estadosPedido.Count; i++)
            {
                if (_estadosPedido[i].IdEstadoPedidoMuestra == idEstado.Value)
                {
                    cboEstado.SelectedIndex = i;
                    break;
                }
            }
        }

        private void AgregarDetalle()
        {
            var nuevo = new PedidoMuestraDetalleViewModel
            {
                IdDetalleMuestra = Guid.NewGuid()
            };
            _detalles.Add(nuevo);
            bsDetalles.ResetBindings(false);
            if (dgvDetalles.Rows.Count > 0)
            {
                dgvDetalles.CurrentCell = dgvDetalles.Rows[dgvDetalles.Rows.Count - 1].Cells["colProducto"];
                dgvDetalles.BeginEdit(true);
            }
        }

        private void EliminarDetalle()
        {
            if (bsDetalles.Current is PedidoMuestraDetalleViewModel detalle)
            {
                _detalles.Remove(detalle);
            }
        }

        private void Guardar()
        {
            if (!Validar())
            {
                return;
            }

            try
            {
                var pedido = _esEdicion
                    ? new PedidoMuestra { IdPedidoMuestra = _pedidoOriginal.IdPedidoMuestra }
                    : new PedidoMuestra();

                pedido.NumeroCorrelativo = txtNumero.Text?.Trim();
                pedido.IdCliente = (Guid)cboCliente.SelectedValue;
                pedido.PersonaContacto = txtContacto.Text?.Trim();
                pedido.EmailContacto = txtEmail.Text?.Trim();
                pedido.TelefonoContacto = txtTelefono.Text?.Trim();
                pedido.FechaEntrega = chkEntrega.Checked ? (DateTime?)null : dtpEntrega.Value.Date;
                pedido.FechaDevolucionEsperada = dtpDevolucion.Value.Date;
                pedido.DiasProrroga = (int)nudProrroga.Value;
                pedido.Observaciones = txtObservaciones.Text?.Trim();
                pedido.IdEstadoPedidoMuestra = cboEstado.SelectedValue is Guid estadoId ? estadoId : (Guid?)null;

                pedido.Detalles = _detalles.Select(MapearDetalle).ToList();

                ResultadoOperacion resultado;
                if (_esEdicion)
                {
                    resultado = _pedidoMuestraService.ActualizarPedido(pedido);
                }
                else
                {
                    resultado = _pedidoMuestraService.CrearPedido(pedido);
                }

                if (resultado.EsValido)
                {
                    var mensaje = _esEdicion
                        ? $"Pedido de muestra actualizado ({pedido.NumeroCorrelativo}) / Sample loan order updated ({pedido.NumeroCorrelativo})"
                        : $"Pedido de muestra creado ({pedido.NumeroCorrelativo}) / Sample loan order created ({pedido.NumeroCorrelativo})";

                    _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, _esEdicion ? "PedidoMuestra.Editar" : "PedidoMuestra.Crear", mensaje, "PedidosMuestra");
                    _logService.LogInfo(mensaje, "PedidosMuestra", SessionContext.NombreUsuario);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("sample.save.error".Traducir(resultado.Mensaje), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error guardando pedido de muestra / Error saving sample order", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sample.save.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DetalleMuestra MapearDetalle(PedidoMuestraDetalleViewModel vm)
        {
            return new DetalleMuestra
            {
                IdDetalleMuestra = vm.IdDetalleMuestra == Guid.Empty ? Guid.NewGuid() : vm.IdDetalleMuestra,
                IdProducto = vm.IdProducto.Value,
                IdEstadoMuestra = vm.IdEstadoMuestra,
                FechaDevolucion = vm.FechaDevolucion,
                ComentarioDevolucion = vm.Comentario,
                PrecioFacturacion = vm.PrecioFacturacion,
                Facturado = vm.Facturado
            };
        }

        private bool Validar()
        {
            if (cboCliente.SelectedItem == null)
            {
                MessageBox.Show("sample.validation.client".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!_detalles.Any())
            {
                MessageBox.Show("sample.validation.details".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var usados = new HashSet<Guid>();
            foreach (var detalle in _detalles)
            {
                if (!detalle.IdProducto.HasValue || detalle.IdProducto.Value == Guid.Empty)
                {
                    MessageBox.Show("sample.validation.product".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!usados.Add(detalle.IdProducto.Value))
                {
                    MessageBox.Show("sample.validation.duplicateProduct".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (detalle.PrecioFacturacion.HasValue && detalle.PrecioFacturacion < 0)
                {
                    MessageBox.Show("sample.validation.price".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }
    }
}