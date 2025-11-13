using BLL.Interfaces;
using BLL.Reportes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Services;
using Services.BLL.Interfaces;
using UI.Helpers;
using UI.Localization;

namespace UI.Formularios
{
    public partial class ReportesForm : Form
    {
        private readonly IReporteService _reporteService;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogService _logService;
        private readonly Dictionary<string, (string Es, string En)> _diccionarioMensajes;
        private bool _suspendEvents;

        public ReportesForm(IReporteService reporteService, IBitacoraService bitacoraService, ILogService logService)
        {
            _reporteService = reporteService ?? throw new ArgumentNullException(nameof(reporteService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _diccionarioMensajes = CrearDiccionarioMensajes();
            InitializeComponent();
        }

        private void ReportesForm_Load(object sender, EventArgs e)
        {
            _suspendEvents = true;
            try
            {
                ApplyTexts();
                InicializarControles();
                InicializarGrillas();
                CargarVentasPeriodo();
                CargarCategorias();
                CargarFacturacion();
                CargarPedidosCliente();
                CargarMejoresClientes();
                CargarClientesSaldo();
            }
            finally
            {
                _suspendEvents = false;
                ActualizarBotonesExport();
            }
        }

        private void ApplyTexts()
        {
            Text = "report.title".Traducir();
            btnExportExcel.Text = "report.actions.exportExcel".Traducir();
            btnExportPdf.Text = "report.actions.exportPdf".Traducir();
            tabVentasPeriodo.Text = "report.tab.sales.period".Traducir();
            tabCategorias.Text = "report.tab.sales.categories".Traducir();
            tabFacturacion.Text = "report.tab.billing".Traducir();
            tabPedidosCliente.Text = "report.tab.orders.client".Traducir();
            tabMejoresClientes.Text = "report.tab.sales.bestClients".Traducir();
            tabClientesSaldo.Text = "report.tab.clients.balance".Traducir();

            lblVentasTipo.Text = "report.filters.periodType".Traducir();
            lblVentasDesde.Text = "report.filters.desde".Traducir();
            lblVentasHasta.Text = "report.filters.hasta".Traducir();
            lblVentasMes.Text = "report.filters.mes".Traducir();
            lblVentasAnio.Text = "report.filters.anio".Traducir();

            lblCategoriasPeriodo.Text = "report.filters.periodType".Traducir();
            lblCategoriasMes.Text = "report.filters.mes".Traducir();
            lblCategoriasAnio.Text = "report.filters.anio".Traducir();

            lblFacturacionPeriodo.Text = "report.filters.periodType".Traducir();
            lblFacturacionMes.Text = "report.filters.mes".Traducir();
            lblFacturacionAnio.Text = "report.filters.anio".Traducir();

            lblPedidosClientePeriodo.Text = "report.filters.periodType".Traducir();
            lblPedidosClienteMes.Text = "report.filters.mes".Traducir();
            lblPedidosClienteAnio.Text = "report.filters.anio".Traducir();
            chkPedidosClienteSaldo.Text = "report.filters.onlyBalance".Traducir();

            lblMejoresClientesPeriodo.Text = "report.filters.periodType".Traducir();
            lblMejoresClientesMes.Text = "report.filters.mes".Traducir();
            lblMejoresClientesAnio.Text = "report.filters.anio".Traducir();

            lblClientesSaldoInfo.Text = "report.filters.noFilters".Traducir();

            ActualizarOpcionesCombos();
        }
        private void InicializarControles()
        {
            // Filtro "desde" debe tomar desde un mes atrás por defecto
            dtpVentasDesde.Value = DateTime.Today.AddMonths(-1);
            dtpVentasHasta.Value = DateTime.Today;
            dtpVentasMes.Format = DateTimePickerFormat.Custom;
            dtpVentasMes.CustomFormat = "MM-yyyy";
            dtpVentasMes.ShowUpDown = true;
            nudVentasAnio.Minimum = 2000;
            nudVentasAnio.Maximum = 2100;
            nudVentasAnio.Value = DateTime.Today.Year;

            dtpCategoriasMes.Format = DateTimePickerFormat.Custom;
            dtpCategoriasMes.CustomFormat = "MM-yyyy";
            dtpCategoriasMes.ShowUpDown = true;
            nudCategoriasAnio.Minimum = 2000;
            nudCategoriasAnio.Maximum = 2100;
            nudCategoriasAnio.Value = DateTime.Today.Year;

            dtpFacturacionMes.Format = DateTimePickerFormat.Custom;
            dtpFacturacionMes.CustomFormat = "MM-yyyy";
            dtpFacturacionMes.ShowUpDown = true;
            nudFacturacionAnio.Minimum = 2000;
            nudFacturacionAnio.Maximum = 2100;
            nudFacturacionAnio.Value = DateTime.Today.Year;

            dtpPedidosClienteMes.Format = DateTimePickerFormat.Custom;
            dtpPedidosClienteMes.CustomFormat = "MM-yyyy";
            dtpPedidosClienteMes.ShowUpDown = true;
            nudPedidosClienteAnio.Minimum = 2000;
            nudPedidosClienteAnio.Maximum = 2100;
            nudPedidosClienteAnio.Value = DateTime.Today.Year;

            dtpMejoresClientesMes.Format = DateTimePickerFormat.Custom;
            dtpMejoresClientesMes.CustomFormat = "MM-yyyy";
            dtpMejoresClientesMes.ShowUpDown = true;
            nudMejoresClientesAnio.Minimum = 2000;
            nudMejoresClientesAnio.Maximum = 2100;
            nudMejoresClientesAnio.Value = DateTime.Today.Year;

            ActualizarVisibilidadVentas();
            ActualizarVisibilidadCategorias();
            ActualizarVisibilidadFacturacion();
            ActualizarVisibilidadPedidosCliente();
            ActualizarVisibilidadMejoresClientes();
        }
        private void InicializarGrillas()
        {
            ConfigurarGrilla(dgvVentasPeriodo,
                CrearColumna("Periodo", "Periodo"),
                CrearColumnaNumerica("Total", "Total"));

            ConfigurarGrilla(dgvCategorias,
                CrearColumna("Categoría", "Categoria"),
                CrearColumnaNumerica("Cantidad", "Cantidad", "N0"),
                CrearColumnaNumerica("Total", "Total"));

            ConfigurarGrilla(dgvFacturacion,
                CrearColumna("Periodo", "Periodo"),
                CrearColumnaNumerica("Total", "TotalFacturado"));

            ConfigurarGrilla(dgvPedidosCliente,
                CrearColumna("Cliente", "Cliente"),
                CrearColumnaNumerica("Pedidos", "CantidadPedidos", "N0"),
                CrearColumnaNumerica("Total", "TotalFacturado"),
                CrearColumnaNumerica("Saldo", "SaldoPendiente"));

            ConfigurarGrilla(dgvMejoresClientes,
                CrearColumna("Cliente", "Cliente"),
                CrearColumnaNumerica("Pedidos", "CantidadPedidos", "N0"),
                CrearColumnaNumerica("Total", "TotalFacturado"));

            ConfigurarGrilla(dgvClientesSaldo,
                CrearColumna("Cliente", "Cliente"),
                CrearColumna("Pedidos con saldo", "PedidosConSaldo"),
                CrearColumnaNumerica("Saldo", "SaldoPendiente"));
        }
        private void ActualizarOpcionesCombos()
        {
            ActualizarComboVentas();
            ActualizarCombo(cmbCategoriasPeriodo, new[] { ReportePeriodoTipo.Todos, ReportePeriodoTipo.Mensual, ReportePeriodoTipo.Anual });
            ActualizarCombo(cmbFacturacionPeriodo, new[] { ReportePeriodoTipo.Mensual, ReportePeriodoTipo.Anual, ReportePeriodoTipo.Todos });
            ActualizarCombo(cmbPedidosClientePeriodo, new[] { ReportePeriodoTipo.Todos, ReportePeriodoTipo.Mensual, ReportePeriodoTipo.Anual });
            ActualizarCombo(cmbMejoresClientesPeriodo, new[] { ReportePeriodoTipo.Todos, ReportePeriodoTipo.Mensual, ReportePeriodoTipo.Anual });
        }

        private void ActualizarComboVentas()
        {
            var seleccionado = cmbVentasTipo.SelectedValue is ReporteVentasPeriodoTipo actual ? actual : ReporteVentasPeriodoTipo.Anual;
            var items = Enum.GetValues(typeof(ReporteVentasPeriodoTipo))
                .Cast<ReporteVentasPeriodoTipo>()
                .Select(v => new ComboItem<ReporteVentasPeriodoTipo>(ObtenerClaveVentas(v), v))
                .ToList();
            cmbVentasTipo.DisplayMember = nameof(ComboItem<ReporteVentasPeriodoTipo>.Texto);
            cmbVentasTipo.ValueMember = nameof(ComboItem<ReporteVentasPeriodoTipo>.Valor);
            cmbVentasTipo.DataSource = items;
            if (items.Any(i => Equals(i.Valor, seleccionado)))
            {
                cmbVentasTipo.SelectedValue = seleccionado;
            }
        }

        private void ActualizarCombo(ComboBox combo, IEnumerable<ReportePeriodoTipo> valores)
        {
            var seleccionado = combo.SelectedValue is ReportePeriodoTipo actual ? actual : valores.First();
            var items = valores
                .Select(v => new ComboItem<ReportePeriodoTipo>(ObtenerClavePeriodo(v), v))
                .ToList();
            combo.DisplayMember = nameof(ComboItem<ReportePeriodoTipo>.Texto);
            combo.ValueMember = nameof(ComboItem<ReportePeriodoTipo>.Valor);
            combo.DataSource = items;
            if (items.Any(i => Equals(i.Valor, seleccionado)))
            {
                combo.SelectedValue = seleccionado;
            }
        }
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            var grid = ObtenerGrillaActual();
            if (grid == null || grid.Rows.Count == 0)
                return;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "report.export.selectExcel".Traducir();
                dialog.FileName = $"{ObtenerNombreArchivoActual()}_{DateTime.Now:yyyyMMddHHmm}.xlsx";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        var titulo = ObtenerTituloActual();
                        ReportExportHelper.ExportToExcel(dialog.FileName, titulo, grid);
                        RegistrarAccion("Reportes.ExportExcel", "report.log.export.excel.success", titulo, dialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        var titulo = ObtenerTituloActual();
                        var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                        RegistrarError("Reportes.ExportExcel", "report.log.export.excel.error", ex, titulo, friendly);
                        MessageBox.Show("report.error.export".Traducir(friendly), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            var grid = ObtenerGrillaActual();
            if (grid == null || grid.Rows.Count == 0)
                return;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "report.export.selectPdf".Traducir();
                dialog.FileName = $"{ObtenerNombreArchivoActual()}_{DateTime.Now:yyyyMMddHHmm}.pdf";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        var titulo = ObtenerTituloActual();
                        ReportExportHelper.ExportToPdf(dialog.FileName, titulo, grid);
                        RegistrarAccion("Reportes.ExportPdf", "report.log.export.pdf.success", titulo, dialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        var titulo = ObtenerTituloActual();
                        var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                        RegistrarError("Reportes.ExportPdf", "report.log.export.pdf.error", ex, titulo, friendly);
                        MessageBox.Show("report.error.export".Traducir(friendly), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tabReportes_SelectedIndexChanged(object sender, EventArgs e) => ActualizarBotonesExport();

        private void cmbVentasTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarVisibilidadVentas();
            if (_suspendEvents)
                return;

            CargarVentasPeriodo();
        }

        private void cmbCategoriasPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarVisibilidadCategorias();
            if (_suspendEvents)
                return;

            CargarCategorias();
        }

        private void cmbFacturacionPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarVisibilidadFacturacion();
            if (_suspendEvents)
                return;

            CargarFacturacion();
        }

        private void cmbPedidosClientePeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarVisibilidadPedidosCliente();
            if (_suspendEvents)
                return;

            CargarPedidosCliente();
        }

        private void cmbMejoresClientesPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarVisibilidadMejoresClientes();
            if (_suspendEvents)
                return;

            CargarMejoresClientes();
        }

        private void dtpVentasDesde_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarVentasPeriodo();
        }

        private void dtpVentasHasta_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarVentasPeriodo();
        }

        private void dtpVentasMes_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarVentasPeriodo();
        }

        private void nudVentasAnio_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarVentasPeriodo();
        }

        private void dtpCategoriasMes_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarCategorias();
        }

        private void nudCategoriasAnio_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarCategorias();
        }

        private void dtpFacturacionMes_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarFacturacion();
        }

        private void nudFacturacionAnio_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarFacturacion();
        }

        private void dtpPedidosClienteMes_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarPedidosCliente();
        }

        private void nudPedidosClienteAnio_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarPedidosCliente();
        }

        private void chkPedidosClienteSaldo_CheckedChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarPedidosCliente();
        }

        private void dtpMejoresClientesMes_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarMejoresClientes();
        }

        private void nudMejoresClientesAnio_ValueChanged(object sender, EventArgs e)
        {
            if (_suspendEvents)
                return;

            CargarMejoresClientes();
        }

        private void CargarVentasPeriodo()
        {
            try
            {
                var filtro = new VentasPeriodoFiltro
                {
                    Tipo = cmbVentasTipo.SelectedValue is ReporteVentasPeriodoTipo tipo ? tipo : ReporteVentasPeriodoTipo.Anual
                };

                if (filtro.Tipo == ReporteVentasPeriodoTipo.Rango)
                {
                    filtro.Desde = dtpVentasDesde.Value.Date;
                    filtro.Hasta = dtpVentasHasta.Value.Date;
                }
                else if (filtro.Tipo == ReporteVentasPeriodoTipo.Mensual)
                {
                    filtro.Anio = (int)nudVentasAnio.Value;
                    filtro.Mes = dtpVentasMes.Value.Month;
                }
                else
                {
                    filtro.Anio = (int)nudVentasAnio.Value;
                }

                var datos = _reporteService.ObtenerVentasPorPeriodo(filtro) ?? new List<VentaPeriodoDetalle>();
                dgvVentasPeriodo.DataSource = datos;
                RegistrarAccion("Reportes.VentasPeriodo", "report.log.sales.success", datos.Count);
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("Reportes.VentasPeriodo", "report.log.sales.error", ex, friendly);
                MessageBox.Show("report.error.load".Traducir(friendly), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ActualizarBotonesExport();
            }
        }

        private void CargarCategorias()
        {
            try
            {
                var filtro = CrearFiltroPeriodo(cmbCategoriasPeriodo, dtpCategoriasMes, nudCategoriasAnio);
                var datos = _reporteService.ObtenerCategoriasMasVendidas(filtro) ?? new List<VentaCategoriaResumen>();
                dgvCategorias.DataSource = datos;
                RegistrarAccion("Reportes.Categorias", "report.log.categories.success", datos.Count);
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("Reportes.Categorias", "report.log.categories.error", ex, friendly);
                MessageBox.Show("report.error.load".Traducir(friendly), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ActualizarBotonesExport();
            }
        }

        private void CargarFacturacion()
        {
            try
            {
                var filtro = CrearFiltroPeriodo(cmbFacturacionPeriodo, dtpFacturacionMes, nudFacturacionAnio);
                var datos = _reporteService.ObtenerFacturacionPorPeriodo(filtro) ?? new List<FacturacionPeriodoResumen>();
                dgvFacturacion.DataSource = datos;
                RegistrarAccion("Reportes.Facturacion", "report.log.billing.success", datos.Count);
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("Reportes.Facturacion", "report.log.billing.error", ex, friendly);
                MessageBox.Show("report.error.load".Traducir(friendly), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ActualizarBotonesExport();
            }
        }

        private void CargarPedidosCliente()
        {
            try
            {
                var filtro = new PedidosClienteFiltro
                {
                    Tipo = cmbPedidosClientePeriodo.SelectedValue is ReportePeriodoTipo tipo ? tipo : ReportePeriodoTipo.Todos,
                    SoloSaldoPendiente = chkPedidosClienteSaldo.Checked
                };

                if (filtro.Tipo == ReportePeriodoTipo.Mensual)
                {
                    filtro.Anio = (int)nudPedidosClienteAnio.Value;
                    filtro.Mes = dtpPedidosClienteMes.Value.Month;
                }
                else if (filtro.Tipo == ReportePeriodoTipo.Anual)
                {
                    filtro.Anio = (int)nudPedidosClienteAnio.Value;
                }

                var datos = _reporteService.ObtenerPedidosPorCliente(filtro) ?? new List<PedidoClienteResumen>();
                dgvPedidosCliente.DataSource = datos;
                RegistrarAccion("Reportes.PedidosCliente", "report.log.orders.success", datos.Count);
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("Reportes.PedidosCliente", "report.log.orders.error", ex, friendly);
                MessageBox.Show("report.error.load".Traducir(friendly), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ActualizarBotonesExport();
            }
        }

        private void CargarMejoresClientes()
        {
            try
            {
                var filtro = CrearFiltroPeriodo(cmbMejoresClientesPeriodo, dtpMejoresClientesMes, nudMejoresClientesAnio);
                var datos = _reporteService.ObtenerMejoresClientes(filtro) ?? new List<ClienteRankingResumen>();
                dgvMejoresClientes.DataSource = datos;
                RegistrarAccion("Reportes.MejoresClientes", "report.log.bestClients.success", datos.Count);
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("Reportes.MejoresClientes", "report.log.bestClients.error", ex, friendly);
                MessageBox.Show("report.error.load".Traducir(friendly), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ActualizarBotonesExport();
            }
        }

        private void CargarClientesSaldo()
        {
            try
            {
                var datos = _reporteService.ObtenerClientesConSaldo() ?? new List<CuentaPorCobrarResumen>();
                dgvClientesSaldo.DataSource = datos;
                RegistrarAccion("Reportes.ClientesSaldo", "report.log.clientsBalance.success", datos.Count);
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarError("Reportes.ClientesSaldo", "report.log.clientsBalance.error", ex, friendly);
                MessageBox.Show("report.error.load".Traducir(friendly), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ActualizarBotonesExport();
            }
        }

        private ReportePeriodoFiltro CrearFiltroPeriodo(ComboBox combo, DateTimePicker mesPicker, NumericUpDown anioPicker)
        {
            var tipo = combo.SelectedValue is ReportePeriodoTipo valor ? valor : ReportePeriodoTipo.Todos;
            var filtro = new ReportePeriodoFiltro { Tipo = tipo };

            if (tipo == ReportePeriodoTipo.Mensual)
            {
                filtro.Anio = (int)anioPicker.Value;
                filtro.Mes = mesPicker.Value.Month;
            }
            else if (tipo == ReportePeriodoTipo.Anual)
            {
                filtro.Anio = (int)anioPicker.Value;
            }
            else
            {
                filtro.Anio = null;
                filtro.Mes = null;
            }

            return filtro;
        }
        private void ActualizarVisibilidadVentas()
        {
            var tipo = cmbVentasTipo.SelectedValue is ReporteVentasPeriodoTipo valor ? valor : ReporteVentasPeriodoTipo.Anual;
            var esRango = tipo == ReporteVentasPeriodoTipo.Rango;
            var esMensual = tipo == ReporteVentasPeriodoTipo.Mensual;

            lblVentasDesde.Visible = esRango;
            dtpVentasDesde.Visible = esRango;
            lblVentasHasta.Visible = esRango;
            dtpVentasHasta.Visible = esRango;
            lblVentasMes.Visible = esMensual;
            dtpVentasMes.Visible = esMensual;
            var mostrarAnio = esMensual || tipo == ReporteVentasPeriodoTipo.Anual;
            lblVentasAnio.Visible = mostrarAnio;
            nudVentasAnio.Visible = mostrarAnio;
        }

        private void ActualizarVisibilidadCategorias()
        {
            ActualizarVisibilidadPeriodo(cmbCategoriasPeriodo, lblCategoriasMes, dtpCategoriasMes, lblCategoriasAnio, nudCategoriasAnio);
        }

        private void ActualizarVisibilidadFacturacion()
        {
            ActualizarVisibilidadPeriodo(cmbFacturacionPeriodo, lblFacturacionMes, dtpFacturacionMes, lblFacturacionAnio, nudFacturacionAnio);
        }

        private void ActualizarVisibilidadPedidosCliente()
        {
            ActualizarVisibilidadPeriodo(cmbPedidosClientePeriodo, lblPedidosClienteMes, dtpPedidosClienteMes, lblPedidosClienteAnio, nudPedidosClienteAnio);
        }

        private void ActualizarVisibilidadMejoresClientes()
        {
            ActualizarVisibilidadPeriodo(cmbMejoresClientesPeriodo, lblMejoresClientesMes, dtpMejoresClientesMes, lblMejoresClientesAnio, nudMejoresClientesAnio);
        }

        private void ActualizarVisibilidadPeriodo(ComboBox combo, Control lblMes, Control inputMes, Control lblAnio, Control inputAnio)
        {
            var tipo = combo.SelectedValue is ReportePeriodoTipo valor ? valor : ReportePeriodoTipo.Todos;
            var mostrarMes = tipo == ReportePeriodoTipo.Mensual;
            lblMes.Visible = mostrarMes;
            inputMes.Visible = mostrarMes;
            var mostrarAnio = tipo == ReportePeriodoTipo.Mensual || tipo == ReportePeriodoTipo.Anual;
            lblAnio.Visible = mostrarAnio;
            inputAnio.Visible = mostrarAnio;
        }

        private void ActualizarBotonesExport()
        {
            var grid = ObtenerGrillaActual();
            var tieneDatos = grid != null && grid.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
            btnExportExcel.Enabled = tieneDatos;
            btnExportPdf.Enabled = tieneDatos;
        }

        private DataGridView ObtenerGrillaActual()
        {
            if (tabReportes.SelectedTab == tabVentasPeriodo) return dgvVentasPeriodo;
            if (tabReportes.SelectedTab == tabCategorias) return dgvCategorias;
            if (tabReportes.SelectedTab == tabFacturacion) return dgvFacturacion;
            if (tabReportes.SelectedTab == tabPedidosCliente) return dgvPedidosCliente;
            if (tabReportes.SelectedTab == tabMejoresClientes) return dgvMejoresClientes;
            if (tabReportes.SelectedTab == tabClientesSaldo) return dgvClientesSaldo;
            return null;
        }

        private string ObtenerTituloActual()
        {
            if (tabReportes.SelectedTab == tabVentasPeriodo) return tabVentasPeriodo.Text;
            if (tabReportes.SelectedTab == tabCategorias) return tabCategorias.Text;
            if (tabReportes.SelectedTab == tabFacturacion) return tabFacturacion.Text;
            if (tabReportes.SelectedTab == tabPedidosCliente) return tabPedidosCliente.Text;
            if (tabReportes.SelectedTab == tabMejoresClientes) return tabMejoresClientes.Text;
            if (tabReportes.SelectedTab == tabClientesSaldo) return tabClientesSaldo.Text;
            return Text;
        }

        private string ObtenerNombreArchivoActual()
        {
            var titulo = ObtenerTituloActual();
            var caracteresInvalidos = Path.GetInvalidFileNameChars().Concat(new[] { ' ' }).ToArray();
            var partes = titulo.Split(caracteresInvalidos, StringSplitOptions.RemoveEmptyEntries);
            var normalizado = string.Join("_", partes);
            return normalizado.Length == 0 ? "reporte" : normalizado.ToLowerInvariant();
        }

        private static void ConfigurarGrilla(DataGridView grid, params DataGridViewColumn[] columnas)
        {
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.Columns.Clear();
            grid.Columns.AddRange(columnas);
        }

        private static DataGridViewTextBoxColumn CrearColumna(string encabezado, string propertyName)
        {
            return new DataGridViewTextBoxColumn
            {
                HeaderText = encabezado,
                DataPropertyName = propertyName,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            };
        }

        private static DataGridViewTextBoxColumn CrearColumnaNumerica(string encabezado, string propertyName, string formato = "C2")
        {
            var columna = CrearColumna(encabezado, propertyName);
            columna.DefaultCellStyle.Format = formato;
            columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            return columna;
        }

        private void RegistrarAccion(string accion, string claveMensaje, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);

            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Reportes");
            }
            catch
            {
                // Evitar que un fallo en bitácora interrumpa la operación de reportes
            }

            _logService.LogInfo(mensaje, "Reportes", SessionContext.NombreUsuario);
        }

        private void RegistrarError(string accion, string claveMensaje, Exception ex, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);

            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Reportes", false, ex?.Message);
            }
            catch
            {
                // Ignorar errores de bitácora
            }

            _logService.LogError(mensaje, ex, "Reportes", SessionContext.NombreUsuario);
        }

        private string ObtenerMensaje(string clave, params object[] args)
        {
            if (_diccionarioMensajes.TryGetValue(clave, out var textos))
            {
                var mensajeEs = args != null && args.Length > 0 ? string.Format(textos.Es, args) : textos.Es;
                var mensajeEn = args != null && args.Length > 0 ? string.Format(textos.En, args) : textos.En;
                return string.Concat(mensajeEs, " / ", mensajeEn);
            }

            return args != null && args.Length > 0 ? string.Format(clave, args) : clave;
        }

        private Dictionary<string, (string Es, string En)> CrearDiccionarioMensajes()
        {
            return new Dictionary<string, (string Es, string En)>
            {
                ["report.log.sales.success"] = ("Se actualizaron las ventas por período ({0} filas).", "Sales by period refreshed ({0} rows)."),
                ["report.log.sales.error"] = ("Error al actualizar las ventas por período: {0}.", "Error refreshing sales by period: {0}."),
                ["report.log.categories.success"] = ("Se actualizaron las ventas por categoría ({0} filas).", "Category sales refreshed ({0} rows)."),
                ["report.log.categories.error"] = ("Error al actualizar las ventas por categoría: {0}.", "Error refreshing category sales: {0}."),
                ["report.log.billing.success"] = ("Se actualizó la facturación por período ({0} filas).", "Billing by period refreshed ({0} rows)."),
                ["report.log.billing.error"] = ("Error al actualizar la facturación por período: {0}.", "Error refreshing billing by period: {0}."),
                ["report.log.orders.success"] = ("Se actualizaron los pedidos por cliente ({0} filas).", "Customer orders refreshed ({0} rows)."),
                ["report.log.orders.error"] = ("Error al actualizar los pedidos por cliente: {0}.", "Error refreshing customer orders: {0}."),
                ["report.log.bestClients.success"] = ("Se actualizaron los mejores clientes ({0} filas).", "Top clients refreshed ({0} rows)."),
                ["report.log.bestClients.error"] = ("Error al actualizar los mejores clientes: {0}.", "Error refreshing top clients: {0}."),
                ["report.log.clientsBalance.success"] = ("Se actualizó el listado de clientes con saldo ({0} filas).", "Clients with outstanding balance refreshed ({0} rows)."),
                ["report.log.clientsBalance.error"] = ("Error al actualizar los clientes con saldo: {0}.", "Error refreshing clients with balance: {0}."),
                ["report.log.export.excel.success"] = ("Se exportó el reporte \"{0}\" a Excel en \"{1}\".", "Report \"{0}\" exported to Excel at \"{1}\"."),
                ["report.log.export.excel.error"] = ("Error al exportar el reporte \"{0}\" a Excel: {1}.", "Error exporting report \"{0}\" to Excel: {1}."),
                ["report.log.export.pdf.success"] = ("Se exportó el reporte \"{0}\" a PDF en \"{1}\".", "Report \"{0}\" exported to PDF at \"{1}\"."),
                ["report.log.export.pdf.error"] = ("Error al exportar el reporte \"{0}\" a PDF: {1}.", "Error exporting report \"{0}\" to PDF: {1}."),
            };
        }

        private string ObtenerClaveVentas(ReporteVentasPeriodoTipo tipo)
        {
            switch (tipo)
            {
                case ReporteVentasPeriodoTipo.Rango:
                    return "report.filters.tipo.rango";
                case ReporteVentasPeriodoTipo.Mensual:
                    return "report.filters.tipo.mensual";
                default:
                    return "report.filters.tipo.anual";
            }
        }

        private string ObtenerClavePeriodo(ReportePeriodoTipo tipo)
        {
            switch (tipo)
            {
                case ReportePeriodoTipo.Todos:
                    return "report.filters.tipo.todos";
                case ReportePeriodoTipo.Mensual:
                    return "report.filters.tipo.mensual";
                default:
                    return "report.filters.tipo.anual";
            }
        }

        private sealed class ComboItem<T>
        {
            private readonly string _key;

            public ComboItem(string key, T value)
            {
                _key = key;
                Valor = value;
            }

            public string Texto => _key.Traducir();

            public T Valor { get; }
        }
    }
}