using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BLL.Interfaces;
using BLL.Helpers;
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
        private readonly Guid? _pedidoId;

        private PedidoMuestra _pedidoOriginal;
        private List<Cliente> _clientes;
        private List<Producto> _productos;
        private List<EstadoMuestra> _estadosMuestra;
        private List<EstadoPedidoMuestra> _estadosPedido;
        private BindingList<PedidoMuestraDetalleViewModel> _detalles;
        private readonly BindingList<PagoRegistrado> _pagosRegistrados = new BindingList<PagoRegistrado>();
        private decimal _montoPagadoBase;
        private decimal _montoPagadoActual;

        private const string ESTADO_DEVUELTO = "Devuelto";
        private const string ESTADO_A_FACTURAR = "A Facturar";
        private const string ESTADO_PENDIENTE_ENVIO = "Pendiente de Envío";

        private sealed class PagoRegistrado
        {
            public PagoRegistrado(decimal monto)
            {
                Id = Guid.NewGuid();
                Monto = Math.Round(monto, 2);
                Fecha = DateTime.Now;
            }

            public Guid Id { get; }
            public decimal Monto { get; }
            public DateTime Fecha { get; }

            public override string ToString()
            {
                return string.Format("sampleOrder.payment.entry".Traducir(), Fecha.ToString("g"), Monto.ToString("C2"));
            }
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

            InitializeComponent();

            lstPagos.DataSource = _pagosRegistrados;
        }

        private void PedidoMuestraForm_Load(object sender, EventArgs e)
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
                InicializarNuevoPedido();
            }

            ActualizarResumen();
            VerificarVencimiento();
        }

        private void ApplyTexts()
        {
            Text = _pedidoId.HasValue ? "sampleOrder.edit.title".Traducir() : "sampleOrder.new.title".Traducir();
            grpGeneral.Text = "sampleOrder.group.general".Traducir();
            grpDetalles.Text = "sampleOrder.group.details".Traducir();
            grpPagos.Text = "sampleOrder.group.payments".Traducir();

            lblCliente.Text = "sampleOrder.client".Traducir();
            lblContacto.Text = "sampleOrder.contact.name".Traducir();
            lblEmail.Text = "sampleOrder.contact.email".Traducir();
            lblTelefono.Text = "sampleOrder.contact.phone".Traducir();
            lblDireccion.Text = "sampleOrder.contact.address".Traducir();
            lblFechaEntrega.Text = "sampleOrder.delivery.date".Traducir();
            lblFechaDevolucion.Text = "sampleOrder.return.expected".Traducir();
            lblEstadoPedido.Text = "sampleOrder.state".Traducir();
            lblObservaciones.Text = "sampleOrder.notes".Traducir();
            chkFacturado.Text = "sampleOrder.invoiced".Traducir();
            btnSeleccionarFactura.Text = "sampleOrder.invoice.select".Traducir();

            btnAgregarDetalle.Text = "sampleOrder.detail.add".Traducir();
            btnEditarDetalle.Text = "sampleOrder.detail.edit".Traducir();
            btnEliminarDetalle.Text = "sampleOrder.detail.delete".Traducir();
            btnPedirFacturacion.Text = "sampleOrder.request.billing".Traducir();
            btnExtenderDias.Text = "sampleOrder.extend.due".Traducir();

            lblTotal.Text = "sampleOrder.summary.total".Traducir();
            lblPagado.Text = "sampleOrder.summary.paid".Traducir();
            lblSaldo.Text = "sampleOrder.summary.balance".Traducir();
            lblPagoNuevo.Text = "sampleOrder.payment.amount".Traducir();
            btnAgregarPago.Text = "sampleOrder.payment.add".Traducir();
            btnEliminarPago.Text = "sampleOrder.payment.remove".Traducir();
            lblExtenderDias.Text = "sampleOrder.extend.days".Traducir();

            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();

            if (dgvDetalles.Columns.Count > 0)
            {
                ActualizarEncabezadosGrid();
            }
        }

        private void CargarDatosReferencia()
        {
            try
            {
                _clientes = _clienteService.ObtenerClientesActivos().OrderBy(c => c.RazonSocial).ToList();
                _productos = _productoService.ObtenerTodos().OrderBy(p => p.NombreProducto).ToList();
                _estadosMuestra = _pedidoMuestraService.ObtenerEstadosMuestra().OrderBy(e => e.NombreEstadoMuestra).ToList();
                _estadosPedido = _pedidoMuestraService.ObtenerEstadosPedido().OrderBy(e => e.NombreEstadoPedidoMuestra).ToList();

                cmbCliente.DisplayMember = nameof(Cliente.RazonSocial);
                cmbCliente.ValueMember = nameof(Cliente.IdCliente);
                cmbCliente.DataSource = _clientes;

                cmbEstadoPedido.DisplayMember = nameof(EstadoPedidoMuestra.NombreEstadoPedidoMuestra);
                cmbEstadoPedido.ValueMember = nameof(EstadoPedidoMuestra.IdEstadoPedidoMuestra);
                cmbEstadoPedido.DataSource = _estadosPedido;
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
        }

        private void ActualizarEncabezadosGrid()
        {
            dgvDetalles.Columns[nameof(PedidoMuestraDetalleViewModel.NombreProducto)].HeaderText = "sampleOrder.detail.product".Traducir();
            dgvDetalles.Columns[nameof(PedidoMuestraDetalleViewModel.EstadoMuestra)].HeaderText = "sampleOrder.detail.state".Traducir();
            dgvDetalles.Columns[nameof(PedidoMuestraDetalleViewModel.PrecioUnitario)].HeaderText = "sampleOrder.detail.price".Traducir();
            dgvDetalles.Columns[nameof(PedidoMuestraDetalleViewModel.Subtotal)].HeaderText = "sampleOrder.detail.subtotal".Traducir();
            dgvDetalles.Columns[nameof(PedidoMuestraDetalleViewModel.FechaDevolucion)].HeaderText = "sampleOrder.detail.returnDate".Traducir();
        }

        private void InicializarNuevoPedido()
        {
            _detalles.Clear();
            _pagosRegistrados.Clear();
            _montoPagadoBase = 0;
            _montoPagadoActual = 0;

            dtpFechaDevolucionEsperada.Value = DateTime.Today.AddDays(7);
            dtpFechaEntrega.Checked = false;
            txtContacto.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtObservaciones.Text = string.Empty;
            txtFactura.Text = string.Empty;
            chkFacturado.Checked = false;
            nudDiasExtension.Value = 1;
        }

        private void CargarPedidoExistente(Guid idPedido)
        {
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

                cmbCliente.SelectedValue = _pedidoOriginal.IdCliente;
                if (_pedidoOriginal.FechaEntrega.HasValue)
                {
                    dtpFechaEntrega.Checked = true;
                    dtpFechaEntrega.Value = _pedidoOriginal.FechaEntrega.Value;
                }
                else
                {
                    dtpFechaEntrega.Checked = false;
                }

                if (_pedidoOriginal.FechaDevolucionEsperada.HasValue)
                {
                    dtpFechaDevolucionEsperada.Value = _pedidoOriginal.FechaDevolucionEsperada.Value;
                }

                if (_pedidoOriginal.IdEstadoPedidoMuestra.HasValue)
                {
                    cmbEstadoPedido.SelectedValue = _pedidoOriginal.IdEstadoPedidoMuestra.Value;
                }

                txtContacto.Text = _pedidoOriginal.PersonaContacto;
                txtEmail.Text = _pedidoOriginal.EmailContacto;
                txtTelefono.Text = _pedidoOriginal.TelefonoContacto;
                txtDireccion.Text = _pedidoOriginal.DireccionEntrega;
                txtObservaciones.Text = _pedidoOriginal.Observaciones;
                chkFacturado.Checked = _pedidoOriginal.Facturado;
                txtFactura.Text = _pedidoOriginal.RutaFacturaPdf;

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

                _montoPagadoBase = _pedidoOriginal.MontoPagado;
                _montoPagadoActual = _pedidoOriginal.MontoPagado;
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cargando pedido de muestra / Error loading sample order", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sampleOrder.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSeleccionarFactura_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "sampleOrder.invoice.select".Traducir();
                ofd.Filter = "PDF|*.pdf|Todos los archivos|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtFactura.Text = ofd.FileName;
                }
            }
        }

        private void btnAgregarDetalle_Click(object sender, EventArgs e)
        {
            var form = new PedidoMuestraDetalleForm(_productos, _estadosMuestra);
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

                nuevo.Subtotal = nuevo.PrecioUnitario;
                _detalles.Add(nuevo);
                ActualizarResumen();

                if (!string.IsNullOrWhiteSpace(nuevo.NombreProducto))
                {
                    RegistrarAccion(
                        "PedidoMuestra.Detalle.Agregar",
                        string.Format("sampleOrder.log.detail.add".Traducir(), nuevo.NombreProducto));
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

            var form = new PedidoMuestraDetalleForm(_productos, _estadosMuestra, detalle);
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

                dgvDetalles.Refresh();
                ActualizarResumen();

                if (!string.IsNullOrWhiteSpace(actualizado.NombreProducto))
                {
                    RegistrarAccion(
                        "PedidoMuestra.Detalle.Editar",
                        string.Format("sampleOrder.log.detail.update".Traducir(), actualizado.NombreProducto));
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
                RegistrarAccion(
                    "PedidoMuestra.Detalle.Eliminar",
                    string.Format("sampleOrder.log.detail.delete".Traducir(), nombreProducto));
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

        private void btnPedirFacturacion_Click(object sender, EventArgs e)
        {
            if (_detalles.Count == 0)
            {
                return;
            }

            var idFacturar = BuscarEstadoMuestraId(ESTADO_A_FACTURAR);
            if (!idFacturar.HasValue)
            {
                MessageBox.Show("sampleOrder.state.missing".Traducir(ESTADO_A_FACTURAR), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var detalle in _detalles)
            {
                if (!EsEstadoDevuelto(detalle))
                {
                    detalle.IdEstadoMuestra = idFacturar;
                    detalle.EstadoMuestra = ESTADO_A_FACTURAR;
                }
            }

            dgvDetalles.Refresh();
            ActualizarResumen();
            RegistrarAccion("PedidoMuestra.PedirFacturacion", "sampleOrder.log.requestBilling".Traducir());
        }

        private void btnExtenderDias_Click(object sender, EventArgs e)
        {
            if (_detalles.All(EsEstadoDevuelto))
            {
                MessageBox.Show("sampleOrder.extend.onlyPending".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dias = (int)nudDiasExtension.Value;
            dtpFechaDevolucionEsperada.Value = dtpFechaDevolucionEsperada.Value.AddDays(dias);
            RegistrarAccion("PedidoMuestra.ExtenderVencimiento", string.Format("sampleOrder.log.extend".Traducir(), dias));
        }

        private void btnAgregarPago_Click(object sender, EventArgs e)
        {
            if (nudPago.Value <= 0)
                return;

            var pago = new PagoRegistrado(nudPago.Value);
            _pagosRegistrados.Add(pago);
            _montoPagadoActual = Math.Round(_montoPagadoBase + _pagosRegistrados.Sum(p => p.Monto), 2);
            ActualizarResumen();
            RegistrarAccion("PedidoMuestra.Pago.Agregar", string.Format("sampleOrder.log.payment.add".Traducir(), pago.Monto.ToString("C2")));
        }

        private void btnEliminarPago_Click(object sender, EventArgs e)
        {
            if (lstPagos.SelectedItem is PagoRegistrado pago)
            {
                _pagosRegistrados.Remove(pago);
                _montoPagadoActual = Math.Round(_montoPagadoBase + _pagosRegistrados.Sum(p => p.Monto), 2);
                ActualizarResumen();
                RegistrarAccion("PedidoMuestra.Pago.Eliminar", string.Format("sampleOrder.log.payment.remove".Traducir(), pago.Monto.ToString("C2")));
            }
        }

        private void ActualizarResumen()
        {
            var total = Math.Round(_detalles.Sum(d => d.Subtotal), 2);
            lblTotalValor.Text = total.ToString("N2");
            lblPagadoValor.Text = _montoPagadoActual.ToString("N2");
            var saldo = Math.Max(0, total - _montoPagadoActual);
            lblSaldoValor.Text = saldo.ToString("N2");
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
                var pedido = new PedidoMuestra
                {
                    IdPedidoMuestra = _pedidoOriginal?.IdPedidoMuestra ?? Guid.Empty,
                    IdCliente = (Guid)cmbCliente.SelectedValue,
                    FechaEntrega = dtpFechaEntrega.Checked ? dtpFechaEntrega.Value : (DateTime?)null,
                    FechaDevolucionEsperada = dtpFechaDevolucionEsperada.Value,
                    DireccionEntrega = txtDireccion.Text?.Trim(),
                    PersonaContacto = txtContacto.Text?.Trim(),
                    EmailContacto = txtEmail.Text?.Trim(),
                    TelefonoContacto = txtTelefono.Text?.Trim(),
                    Observaciones = txtObservaciones.Text?.Trim(),
                    Facturado = chkFacturado.Checked,
                    RutaFacturaPdf = txtFactura.Text,
                    IdEstadoPedidoMuestra = cmbEstadoPedido.SelectedItem is EstadoPedidoMuestra estado ? estado.IdEstadoPedidoMuestra : (Guid?)null,
                    MontoPagado = _montoPagadoActual
                };

                if (!string.IsNullOrWhiteSpace(pedido.RutaFacturaPdf) && !File.Exists(pedido.RutaFacturaPdf))
                {
                    var confirm = MessageBox.Show("sampleOrder.invoice.notfound".Traducir(), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm != DialogResult.Yes)
                    {
                        return;
                    }
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
                    var mensaje = _pedidoOriginal == null
                        ? "sampleOrder.log.created".Traducir()
                        : "sampleOrder.log.updated".Traducir();

                    RegistrarAccion(_pedidoOriginal == null ? "PedidoMuestra.Alta" : "PedidoMuestra.Edicion", mensaje);
                    _logService.LogInfo(mensaje, "PedidosMuestra", SessionContext.NombreUsuario);

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

        private bool EsEstadoDevuelto(PedidoMuestraDetalleViewModel detalle)
        {
            if (detalle == null)
                return false;

            return string.Equals(detalle.EstadoMuestra, ESTADO_DEVUELTO, StringComparison.OrdinalIgnoreCase);
        }

        private Guid? BuscarEstadoMuestraId(string nombre)
        {
            var estado = _estadosMuestra.FirstOrDefault(e => string.Equals(e.NombreEstadoMuestra, nombre, StringComparison.OrdinalIgnoreCase));
            return estado?.IdEstadoMuestra;
        }

        private void RegistrarAccion(string accion, string mensaje)
        {
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "PedidosMuestra");
                _logService.LogInfo(mensaje, "PedidosMuestra", SessionContext.NombreUsuario);
            }
            catch
            {
                // No interrumpir el flujo si falla el registro
            }
        }

        private void VerificarVencimiento()
        {
            if (_detalles.Count == 0)
                return;

            if (_detalles.Any(d => !EsEstadoDevuelto(d)))
            {
                var vencida = dtpFechaDevolucionEsperada.Value.Date < DateTime.Today;
                if (vencida)
                {
                    MessageBox.Show("sampleOrder.return.overdue".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}