using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using Services.BLL.Interfaces;
using UI.Helpers;
using UI.Localization;
using UI.ViewModels;

namespace UI
{
    public partial class PedidosMuestraForm : Form
    {
        private readonly IPedidoMuestraService _pedidoMuestraService;
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogService _logService;

        private readonly Dictionary<Control, string> _diccionarioTextos = new();
        private BindingList<PedidoRow> _rows = new();
        private List<EstadoPedidoMuestra> _estadosPedido = new();
        private List<EstadoMuestra> _estadosMuestra = new();

        private sealed class PedidoRow
        {
            public Guid IdPedido { get; set; }
            public string Numero { get; set; }
            public string Cliente { get; set; }
            public string Estado { get; set; }
            public DateTime? FechaEntrega { get; set; }
            public DateTime? FechaDevolucionEsperada { get; set; }
            public int DiasProrroga { get; set; }
            public bool Facturado { get; set; }
            public decimal TotalFacturado { get; set; }
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

            Load += PedidosMuestraForm_Load;
            FormClosed += (s, e) => Localization.Localization.LanguageChanged -= Localization_LanguageChanged;
        }

        private void PedidosMuestraForm_Load(object sender, EventArgs e)
        {
            Localization.Localization.LanguageChanged += Localization_LanguageChanged;
            ConstruirDiccionario();
            ConfigurarGrid();
            CargarEstados();
            ApplyTexts();
            WireUp();
            BuscarPedidos();
        }

        private void Localization_LanguageChanged(object sender, EventArgs e)
        {
            ApplyTexts();
        }

        private void WireUp()
        {
            tsbNuevo.Click += (s, e) => NuevoPedido();
            tsbEditar.Click += (s, e) => EditarSeleccionado();
            tsbActualizar.Click += (s, e) => BuscarPedidos();
            tsbGenerarNota.Click += (s, e) => GenerarNotaPrestamo();
            tsbFacturar.Click += (s, e) => FacturarSeleccionado();
            btnBuscar.Click += (s, e) => BuscarPedidos();
            txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BuscarPedidos(); } };
            cboEstado.SelectedIndexChanged += (s, e) => BuscarPedidos();
            chkVencidos.CheckedChanged += (s, e) => BuscarPedidos();
            dgvPedidos.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };
        }

        private void ConstruirDiccionario()
        {
            _diccionarioTextos[tsbNuevo] = "sample.list.new";
            _diccionarioTextos[tsbEditar] = "sample.list.edit";
            _diccionarioTextos[tsbActualizar] = "form.refresh";
            _diccionarioTextos[tsbGenerarNota] = "sample.list.loanNote";
            _diccionarioTextos[tsbFacturar] = "sample.list.invoice";
            _diccionarioTextos[lblBuscar] = "form.search";
            _diccionarioTextos[lblEstado] = "sample.state";
            _diccionarioTextos[chkVencidos] = "sample.list.onlyExpired";
            _diccionarioTextos[btnBuscar] = "form.filter";
        }

        private void ApplyTexts()
        {
            Text = "sample.list.title".Traducir();
            foreach (var kvp in _diccionarioTextos)
            {
                kvp.Key.Text = kvp.Value.Traducir();
            }

            if (dgvPedidos.Columns.Count > 0)
            {
                dgvPedidos.Columns[nameof(PedidoRow.Numero)].HeaderText = "sample.number".Traducir();
                dgvPedidos.Columns[nameof(PedidoRow.Cliente)].HeaderText = "sample.client".Traducir();
                dgvPedidos.Columns[nameof(PedidoRow.Estado)].HeaderText = "sample.state".Traducir();
                dgvPedidos.Columns[nameof(PedidoRow.FechaEntrega)].HeaderText = "sample.deliveryDate".Traducir();
                dgvPedidos.Columns[nameof(PedidoRow.FechaDevolucionEsperada)].HeaderText = "sample.expectedReturn".Traducir();
                dgvPedidos.Columns[nameof(PedidoRow.DiasProrroga)].HeaderText = "sample.extraDays".Traducir();
                dgvPedidos.Columns[nameof(PedidoRow.Facturado)].HeaderText = "sample.invoiced".Traducir();
                dgvPedidos.Columns[nameof(PedidoRow.TotalFacturado)].HeaderText = "sample.invoicedAmount".Traducir();
            }

            InicializarEstadosCombo();
        }

        private void ConfigurarGrid()
        {
            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.Columns.Clear();

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.Numero),
                Name = nameof(PedidoRow.Numero),
                FillWeight = 110,
                MinimumWidth = 100
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.Cliente),
                Name = nameof(PedidoRow.Cliente),
                FillWeight = 220,
                MinimumWidth = 160
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.Estado),
                Name = nameof(PedidoRow.Estado),
                FillWeight = 120,
                MinimumWidth = 110
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.FechaEntrega),
                Name = nameof(PedidoRow.FechaEntrega),
                FillWeight = 110,
                MinimumWidth = 100,
                DefaultCellStyle = { Format = "d" }
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.FechaDevolucionEsperada),
                Name = nameof(PedidoRow.FechaDevolucionEsperada),
                FillWeight = 110,
                MinimumWidth = 110,
                DefaultCellStyle = { Format = "d" }
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.DiasProrroga),
                Name = nameof(PedidoRow.DiasProrroga),
                FillWeight = 80,
                MinimumWidth = 80
            });
            dgvPedidos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.Facturado),
                Name = nameof(PedidoRow.Facturado),
                FillWeight = 80,
                MinimumWidth = 70
            });
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoRow.TotalFacturado),
                Name = nameof(PedidoRow.TotalFacturado),
                FillWeight = 100,
                MinimumWidth = 90,
                DefaultCellStyle = { Format = "C2" }
            });

            dgvPedidos.DataSource = bsPedidos;
            bsPedidos.DataSource = _rows;
        }

        private void InicializarEstadosCombo()
        {
            var items = new List<KeyValuePair<Guid?, string>>
            {
                new KeyValuePair<Guid?, string>(null, "form.select.all".Traducir())
            };
            items.AddRange(_estadosPedido.Select(e => new KeyValuePair<Guid?, string>(e.IdEstadoPedidoMuestra, e.NombreEstadoPedidoMuestra)));

            cboEstado.DisplayMember = "Value";
            cboEstado.ValueMember = "Key";
            cboEstado.DataSource = items;
        }

        private void CargarEstados()
        {
            try
            {
                _estadosPedido = _pedidoMuestraService.ObtenerEstadosPedido()?.ToList() ?? new List<EstadoPedidoMuestra>();
                _estadosMuestra = _pedidoMuestraService.ObtenerEstadosMuestra()?.ToList() ?? new List<EstadoMuestra>();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cargando estados de muestra / Error loading sample states", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sample.loadStates.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarPedidos()
        {
            try
            {
                var filtro = new PedidoMuestraFiltro
                {
                    Numero = txtBuscar.Text,
                    SoloVencidos = chkVencidos.Checked,
                    IdEstado = cboEstado.SelectedValue is Guid estadoId ? estadoId : (Guid?)null
                };

                var pedidos = _pedidoMuestraService.ObtenerPedidos(filtro)?.ToList() ?? new List<PedidoMuestra>();
                _rows = new BindingList<PedidoRow>(pedidos.Select(MapearPedido).ToList());
                bsPedidos.DataSource = _rows;

                _logService.LogInfo($"Listado de pedidos de muestra actualizado ({_rows.Count}) / Sample loan orders refreshed ({_rows.Count})", "PedidosMuestra", SessionContext.NombreUsuario);
            }
            catch (Exception ex)
            {
                _logService.LogError("Error buscando pedidos de muestra / Error searching sample orders", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sample.search.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private PedidoRow MapearPedido(PedidoMuestra pedido)
        {
            return new PedidoRow
            {
                IdPedido = pedido.IdPedidoMuestra,
                Numero = pedido.NumeroCorrelativo,
                Cliente = pedido.Cliente?.RazonSocial ?? string.Empty,
                Estado = pedido.EstadoPedidoMuestra?.NombreEstadoPedidoMuestra ?? "",
                FechaEntrega = pedido.FechaEntrega,
                FechaDevolucionEsperada = pedido.FechaDevolucionEsperada,
                DiasProrroga = pedido.DiasProrroga,
                Facturado = pedido.Facturado,
                TotalFacturado = pedido.TotalFacturado
            };
        }

        private PedidoRow ObtenerSeleccionado()
        {
            return bsPedidos.Current as PedidoRow;
        }

        private void NuevoPedido()
        {
            try
            {
                using (var form = CrearDetalleForm(null))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        BuscarPedidos();
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error creando pedido de muestra / Error creating sample order", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sample.create.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditarSeleccionado()
        {
            var seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show("sample.select.warning".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var pedido = _pedidoMuestraService.ObtenerPedido(seleccionado.IdPedido);
                if (pedido == null)
                {
                    MessageBox.Show("sample.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var form = CrearDetalleForm(pedido))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        BuscarPedidos();
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error editando pedido de muestra / Error editing sample order", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sample.edit.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private PedidoMuestraForm CrearDetalleForm(PedidoMuestra pedido)
        {
            return new PedidoMuestraForm(
                _pedidoMuestraService,
                _clienteService,
                _productoService,
                _bitacoraService,
                _logService,
                pedido,
                _estadosMuestra);
        }

        private void GenerarNotaPrestamo()
        {
            var seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show("sample.select.warning".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var pedido = _pedidoMuestraService.ObtenerPedido(seleccionado.IdPedido);
                if (pedido == null)
                {
                    MessageBox.Show("sample.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "PDF|*.pdf";
                    dialog.FileName = $"NotaPrestamo_{pedido.NumeroCorrelativo}.pdf";
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        NotaPrestamoPdfHelper.Generar(pedido, pedido.Detalles, dialog.FileName);
                        var mensaje = $"Nota de préstamo generada en {dialog.FileName} / Loan note generated at {dialog.FileName}";
                        _logService.LogInfo(mensaje, "PedidosMuestra", SessionContext.NombreUsuario);
                        _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, "PedidoMuestra.NotaPrestamo", mensaje, "PedidosMuestra");
                        MessageBox.Show("sample.loanNote.success".Traducir(Path.GetFileName(dialog.FileName)), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error generando nota de préstamo / Error generating loan note", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sample.loanNote.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FacturarSeleccionado()
        {
            var seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show("sample.select.warning".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var pedido = _pedidoMuestraService.ObtenerPedido(seleccionado.IdPedido);
                if (pedido == null)
                {
                    MessageBox.Show("sample.load.error".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var detalles = pedido.Detalles.Where(EsDetalleNoDevuelto).ToList();
                if (!detalles.Any())
                {
                    MessageBox.Show("sample.invoice.none".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!SolicitarPrecios(detalles, out var precios, out var generarPedido))
                {
                    return;
                }

                var resultado = _pedidoMuestraService.FacturarPendientes(pedido.IdPedidoMuestra, precios, generarPedido);
                if (resultado.Exitoso)
                {
                    var mensaje = $"Muestras facturadas ({pedido.NumeroCorrelativo}) / Samples invoiced ({pedido.NumeroCorrelativo})";
                    _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, "PedidoMuestra.Facturar", mensaje, "PedidosMuestra");
                    _logService.LogInfo(mensaje, "PedidosMuestra", SessionContext.NombreUsuario);
                    MessageBox.Show("sample.invoice.success".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BuscarPedidos();
                }
                else
                {
                    MessageBox.Show("sample.invoice.error".Traducir(resultado.Mensaje), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error facturando muestras / Error invoicing samples", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show("sample.invoice.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool EsDetalleNoDevuelto(DetalleMuestra detalle)
        {
            var estado = _estadosMuestra.FirstOrDefault(e => e.IdEstadoMuestra == detalle.IdEstadoMuestra)?.NombreEstadoMuestra ?? string.Empty;
            return !detalle.Facturado && (estado.IndexOf("No Devuelto", StringComparison.OrdinalIgnoreCase) >= 0 || estado.IndexOf("Not Returned", StringComparison.OrdinalIgnoreCase) >= 0 || string.IsNullOrWhiteSpace(estado));
        }

        private bool SolicitarPrecios(List<DetalleMuestra> detalles, out Dictionary<Guid, decimal> precios, out bool generarPedido)
        {
            precios = new Dictionary<Guid, decimal>();
            generarPedido = false;

            using (var dialog = new Form())
            {
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.Width = 520;
                dialog.Height = 420;
                dialog.Text = "sample.invoice.dialog.title".Traducir();
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;

                var datos = detalles.Select(d => new FacturacionRow
                {
                    IdDetalle = d.IdDetalleMuestra,
                    Producto = d.Producto?.NombreProducto ?? string.Empty,
                    Precio = d.PrecioFacturacion ?? 0m
                }).ToList();

                var grid = new DataGridView
                {
                    Dock = DockStyle.Top,
                    Height = 300,
                    AutoGenerateColumns = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    DataSource = datos
                };
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(FacturacionRow.Producto),
                    Name = nameof(FacturacionRow.Producto),
                    ReadOnly = true,
                    FillWeight = 200
                });
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(FacturacionRow.Precio),
                    Name = nameof(FacturacionRow.Precio),
                    DefaultCellStyle = { Format = "C2" },
                    FillWeight = 100
                });

                var chkGenerarPedido = new CheckBox
                {
                    Text = "sample.invoice.createOrder".Traducir(),
                    Dock = DockStyle.Top,
                    Height = 30
                };

                var panelBotones = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    FlowDirection = FlowDirection.RightToLeft,
                    Height = 50
                };
                var btnAceptar = new Button { Text = "form.accept".Traducir(), DialogResult = DialogResult.OK, Width = 100 };
                var btnCancelar = new Button { Text = "form.cancel".Traducir(), DialogResult = DialogResult.Cancel, Width = 100 };
                panelBotones.Controls.Add(btnAceptar);
                panelBotones.Controls.Add(btnCancelar);

                dialog.Controls.Add(panelBotones);
                dialog.Controls.Add(chkGenerarPedido);
                dialog.Controls.Add(grid);
                dialog.AcceptButton = btnAceptar;
                dialog.CancelButton = btnCancelar;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return false;
                }

                foreach (var row in datos)
                {
                    if (row.Precio < 0)
                    {
                        MessageBox.Show("sample.invoice.negative".Traducir(), dialog.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    precios[row.IdDetalle] = row.Precio;
                }

                generarPedido = chkGenerarPedido.Checked;
                return true;
            }
        }

        private sealed class FacturacionRow
        {
            public Guid IdDetalle { get; set; }
            public string Producto { get; set; }
            public decimal Precio { get; set; }
        }
    }
}