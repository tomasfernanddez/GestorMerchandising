using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using Services.BLL.Interfaces;
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

        private BindingList<PedidoMuestraRow> _rows;
        private List<Cliente> _clientes;
        private List<EstadoPedidoMuestra> _estadosPedido;
        private List<EstadoMuestra> _estadosMuestra;
        private bool _mostroAlertaVencidos;

        private const string ESTADO_A_FACTURAR = "A Facturar";

        private sealed class PedidoMuestraRow
        {
            public Guid IdPedidoMuestra { get; set; }
            public string Cliente { get; set; }
            public string Estado { get; set; }
            public DateTime? FechaEntrega { get; set; }
            public DateTime? FechaDevolucionEsperada { get; set; }
            public bool Facturado { get; set; }
            public decimal SaldoPendiente { get; set; }
            public string Contacto { get; set; }
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

            InitializeComponent();
        }

        private void PedidosMuestraForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarDatosReferencia();
            ConfigurarGrid();
            CargarPedidos();
        }

        private void ApplyTexts()
        {
            Text = "sampleOrder.list.title".Traducir();
            tsbNuevo.Text = "sampleOrder.list.new".Traducir();
            tsbEditar.Text = "sampleOrder.list.edit".Traducir();
            tsbActualizar.Text = "form.refresh".Traducir();
            tsbPedirFacturacion.Text = "sampleOrder.request.billing".Traducir();
            tsbExtenderDias.Text = "sampleOrder.extend.due".Traducir();

            lblClienteFiltro.Text = "sampleOrder.client".Traducir();
            lblEstadoFiltro.Text = "sampleOrder.state".Traducir();
            chkFacturadoFiltro.Text = "sampleOrder.invoiced.only".Traducir();
            chkSaldoFiltro.Text = "sampleOrder.balance.only".Traducir();
            btnFiltrar.Text = "form.filter".Traducir();
            lblDiasExtension.Text = "sampleOrder.extend.days".Traducir();

            if (dgvPedidos.Columns.Count > 0)
            {
                dgvPedidos.Columns[nameof(PedidoMuestraRow.Cliente)].HeaderText = "sampleOrder.client".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.Estado)].HeaderText = "sampleOrder.state".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.FechaEntrega)].HeaderText = "sampleOrder.delivery.date".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.FechaDevolucionEsperada)].HeaderText = "sampleOrder.return.expected".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.Facturado)].HeaderText = "sampleOrder.invoiced".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.SaldoPendiente)].HeaderText = "sampleOrder.summary.balance".Traducir();
                dgvPedidos.Columns[nameof(PedidoMuestraRow.Contacto)].HeaderText = "sampleOrder.contact.name".Traducir();
            }
        }

        private void CargarDatosReferencia()
        {
            try
            {
                _clientes = _clienteService.ObtenerClientesActivos().OrderBy(c => c.RazonSocial).ToList();
                _estadosPedido = _pedidoMuestraService.ObtenerEstadosPedido().OrderBy(e => e.NombreEstadoPedidoMuestra).ToList();
                _estadosMuestra = _pedidoMuestraService.ObtenerEstadosMuestra().ToList();

                cmbClienteFiltro.DisplayMember = nameof(Cliente.RazonSocial);
                cmbClienteFiltro.ValueMember = nameof(Cliente.IdCliente);
                cmbClienteFiltro.DataSource = _clientes;
                cmbClienteFiltro.SelectedIndex = -1;

                cmbEstadoFiltro.DisplayMember = nameof(EstadoPedidoMuestra.NombreEstadoPedidoMuestra);
                cmbEstadoFiltro.ValueMember = nameof(EstadoPedidoMuestra.IdEstadoPedidoMuestra);
                cmbEstadoFiltro.DataSource = _estadosPedido;
                cmbEstadoFiltro.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cargando datos de pedidos de muestra / Error loading sample orders data", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sampleOrder.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrid()
        {
            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.Columns.Clear();

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.Cliente),
                Name = nameof(PedidoMuestraRow.Cliente),
                HeaderText = "sampleOrder.client".Traducir(),
                Width = 220
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.Estado),
                Name = nameof(PedidoMuestraRow.Estado),
                HeaderText = "sampleOrder.state".Traducir(),
                Width = 120
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.FechaEntrega),
                Name = nameof(PedidoMuestraRow.FechaEntrega),
                HeaderText = "sampleOrder.delivery.date".Traducir(),
                DefaultCellStyle = { Format = "d" },
                Width = 110
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.FechaDevolucionEsperada),
                Name = nameof(PedidoMuestraRow.FechaDevolucionEsperada),
                HeaderText = "sampleOrder.return.expected".Traducir(),
                DefaultCellStyle = { Format = "d" },
                Width = 130
            });

            dgvPedidos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.Facturado),
                Name = nameof(PedidoMuestraRow.Facturado),
                HeaderText = "sampleOrder.invoiced".Traducir(),
                Width = 80
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.SaldoPendiente),
                Name = nameof(PedidoMuestraRow.SaldoPendiente),
                HeaderText = "sampleOrder.summary.balance".Traducir(),
                DefaultCellStyle = { Format = "C2" },
                Width = 120
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoMuestraRow.Contacto),
                Name = nameof(PedidoMuestraRow.Contacto),
                HeaderText = "sampleOrder.contact.name".Traducir(),
                Width = 160
            });

            _rows = new BindingList<PedidoMuestraRow>();
            dgvPedidos.DataSource = _rows;
        }

        private void CargarPedidos()
        {
            try
            {
                var filtro = new PedidoMuestraFiltro
                {
                    IdCliente = cmbClienteFiltro.SelectedItem is Cliente cliente ? cliente.IdCliente : (Guid?)null,
                    IdEstadoPedido = cmbEstadoFiltro.SelectedItem is EstadoPedidoMuestra estado ? estado.IdEstadoPedidoMuestra : (Guid?)null,
                    Facturado = chkFacturadoFiltro.Checked ? true : (bool?)null,
                    ConSaldoPendiente = chkSaldoFiltro.Checked ? true : (bool?)null,
                    IncluirDetalles = false
                };

                var pedidos = _pedidoMuestraService.ObtenerPedidosMuestra(filtro);

                _rows.Clear();
                foreach (var pedido in pedidos)
                {
                    _rows.Add(new PedidoMuestraRow
                    {
                        IdPedidoMuestra = pedido.IdPedidoMuestra,
                        Cliente = pedido.Cliente?.RazonSocial ?? string.Empty,
                        Estado = pedido.EstadoPedidoMuestra?.NombreEstadoPedidoMuestra,
                        FechaEntrega = pedido.FechaEntrega,
                        FechaDevolucionEsperada = pedido.FechaDevolucionEsperada,
                        Facturado = pedido.Facturado,
                        SaldoPendiente = pedido.SaldoPendiente,
                        Contacto = pedido.PersonaContacto
                    });
                }

                ResaltarVencidos();
                MostrarAlertaVencidos();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error listando pedidos de muestra / Error listing sample orders", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sampleOrder.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResaltarVencidos()
        {
            foreach (DataGridViewRow row in dgvPedidos.Rows)
            {
                if (row.DataBoundItem is PedidoMuestraRow pedido)
                {
                    var vencido = pedido.FechaDevolucionEsperada.HasValue && pedido.FechaDevolucionEsperada.Value.Date < DateTime.Today && !pedido.Facturado;
                    row.DefaultCellStyle.BackColor = vencido ? System.Drawing.Color.MistyRose : System.Drawing.Color.White;
                }
            }
        }

        private void MostrarAlertaVencidos()
        {
            if (_mostroAlertaVencidos)
                return;

            var hayVencidos = _rows.Any(r => r.FechaDevolucionEsperada.HasValue && r.FechaDevolucionEsperada.Value.Date < DateTime.Today && !r.Facturado);
            if (hayVencidos)
            {
                _mostroAlertaVencidos = true;
                MessageBox.Show("sampleOrder.return.overdue".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            _mostroAlertaVencidos = false;
            CargarPedidos();
        }

        private void AbrirEditor(Guid? idPedido = null)
        {
            try
            {
                using (var form = new PedidoMuestraForm(_pedidoMuestraService, _clienteService, _productoService, _bitacoraService, _logService, idPedido))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        _mostroAlertaVencidos = false;
                        CargarPedidos();
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error abriendo pedido de muestra / Error opening sample order", ex, "PedidosMuestra", SessionContext.NombreUsuario);
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

            try
            {
                var pedido = _pedidoMuestraService.ObtenerPedidoMuestra(idPedido.Value, incluirDetalles: true);
                if (pedido == null)
                    return;

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

                pedido.MontoPagado = pedido.MontoPagado; // asegurar no cambia
                var resultado = _pedidoMuestraService.ActualizarPedidoMuestra(pedido);
                if (resultado.EsValido)
                {
                    RegistrarAccion("PedidoMuestra.PedirFacturacion", "sampleOrder.log.requestBilling".Traducir());
                    _mostroAlertaVencidos = false;
                    CargarPedidos();
                }
                else
                {
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error solicitando facturación de muestra / Error requesting sample billing", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sampleOrder.save.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbExtenderDias_Click(object sender, EventArgs e)
        {
            var idPedido = ObtenerPedidoSeleccionado();
            if (!idPedido.HasValue)
                return;

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

                var dias = (int)nudDiasExtension.Value;
                pedido.FechaDevolucionEsperada = (pedido.FechaDevolucionEsperada ?? DateTime.Today).AddDays(dias);
                var resultado = _pedidoMuestraService.ActualizarPedidoMuestra(pedido);
                if (resultado.EsValido)
                {
                    RegistrarAccion("PedidoMuestra.ExtenderVencimiento", string.Format("sampleOrder.log.extend".Traducir(), dias));
                    _mostroAlertaVencidos = false;
                    CargarPedidos();
                }
                else
                {
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error extendiendo vencimiento de muestras / Error extending sample due date", ex, "PedidosMuestra", SessionContext.NombreUsuario);
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

        private void RegistrarAccion(string accion, string mensaje)
        {
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "PedidosMuestra");
                _logService.LogInfo(mensaje, "PedidosMuestra", SessionContext.NombreUsuario);
            }
            catch
            {
                // ignorar errores de registro
            }
        }
    }
}