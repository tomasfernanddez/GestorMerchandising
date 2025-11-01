using BLL.Reportes;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using UI.Helpers;
using UI.Localization;

namespace UI.Formularios
{
    public partial class ReportesForm : Form
    {
        private readonly IReporteService _reporteService;
        private ReporteGeneralResult _datos;

        public ReportesForm(IReporteService reporteService)
        {
            _reporteService = reporteService ?? throw new ArgumentNullException(nameof(reporteService));
            InitializeComponent();
        }

        private void ReportesForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            ConfigurarGrillas();
            InicializarControles();
            CargarDatos();
        }

        private void ApplyTexts()
        {
            Text = "report.title".Traducir();
            grpFiltros.Text = "report.filters.title".Traducir();
            lblDiasLimite.Text = "report.filters.diasLimite".Traducir();
            lblMesComparativo.Text = "report.filters.mesComparativo".Traducir();
            lblAnioComparativo.Text = "report.filters.anioComparativo".Traducir();
            lblDesde.Text = "report.filters.desde".Traducir();
            lblHasta.Text = "report.filters.hasta".Traducir();

            btnActualizar.Text = "report.actions.refresh".Traducir();
            btnExportExcel.Text = "report.actions.exportExcel".Traducir();
            btnExportPdf.Text = "report.actions.exportPdf".Traducir();
            btnExportRaw.Text = "report.actions.exportRaw".Traducir();

            tabPrincipal.TabPages[0].Text = "report.tab.operativos".Traducir();
            tabPrincipal.TabPages[1].Text = "report.tab.ventas".Traducir();
            tabPrincipal.TabPages[2].Text = "report.tab.financieros".Traducir();

            tabOperativos.TabPages[0].Text = "report.tab.operativos.estado".Traducir();
            tabOperativos.TabPages[1].Text = "report.tab.operativos.vencimientos".Traducir();
            tabOperativos.TabPages[2].Text = "report.tab.operativos.demorados".Traducir();
            tabOperativos.TabPages[3].Text = "report.tab.operativos.muestras".Traducir();
            tabOperativos.TabPages[4].Text = "report.tab.operativos.saldo".Traducir();
            tabOperativos.TabPages[5].Text = "report.tab.operativos.produccion".Traducir();

            tabVentas.TabPages[0].Text = "report.tab.ventas.mensuales".Traducir();
            tabVentas.TabPages[1].Text = "report.tab.ventas.trimestrales".Traducir();
            tabVentas.TabPages[2].Text = "report.tab.ventas.anuales".Traducir();
            tabVentas.TabPages[3].Text = "report.tab.ventas.comparativas".Traducir();
            tabVentas.TabPages[4].Text = "report.tab.ventas.ranking".Traducir();
            tabVentas.TabPages[5].Text = "report.tab.ventas.categorias".Traducir();

            tabFinancieros.TabPages[0].Text = "report.tab.financieros.facturacion".Traducir();
            tabFinancieros.TabPages[1].Text = "report.tab.financieros.cobrar".Traducir();
            tabFinancieros.TabPages[2].Text = "report.tab.financieros.pagos".Traducir();
            tabFinancieros.TabPages[3].Text = "report.tab.financieros.proyeccion".Traducir();
        }

        private void ConfigurarGrillas()
        {
            ConfigurarGrilla(dgvOperativosEstado, new[]
            {
                CrearColumna("Estado", nameof(PedidoEstadoResumen.Estado)),
                CrearColumna("Cantidad", nameof(PedidoEstadoResumen.Cantidad), "N0"),
                CrearColumna("Total", nameof(PedidoEstadoResumen.Total), "N2"),
                CrearColumna("Saldo", nameof(PedidoEstadoResumen.SaldoPendiente), "N2"),
            });

            ConfigurarGrilla(dgvOperativosLimite, new[]
            {
                CrearColumna("Número", nameof(PedidoLimiteResumen.NumeroPedido)),
                CrearColumna("Cliente", nameof(PedidoLimiteResumen.Cliente)),
                CrearColumna("Fecha", nameof(PedidoLimiteResumen.FechaLimite), formato:"d"),
                CrearColumna("Días", nameof(PedidoLimiteResumen.DiasRestantes), "N0"),
                CrearColumna("Estado", nameof(PedidoLimiteResumen.Estado)),
            });

            ConfigurarGrilla(dgvOperativosDemoras, new[]
            {
                CrearColumna("Número", nameof(PedidoDemoradoResumen.NumeroPedido)),
                CrearColumna("Cliente", nameof(PedidoDemoradoResumen.Cliente)),
                CrearColumna("Fecha", nameof(PedidoDemoradoResumen.FechaLimite), formato:"d"),
                CrearColumna("Días atraso", nameof(PedidoDemoradoResumen.DiasAtraso), "N0"),
                CrearColumna("Estado", nameof(PedidoDemoradoResumen.Estado)),
            });

            ConfigurarGrilla(dgvOperativosMuestras, new[]
            {
                CrearColumna("Número", nameof(MuestraVencidaResumen.NumeroPedido)),
                CrearColumna("Cliente", nameof(MuestraVencidaResumen.Cliente)),
                CrearColumna("Fecha esperada", nameof(MuestraVencidaResumen.FechaEsperadaDevolucion), formato:"d"),
                CrearColumna("Días atraso", nameof(MuestraVencidaResumen.DiasAtraso), "N0"),
                CrearColumna("Saldo", nameof(MuestraVencidaResumen.SaldoPendiente), "N2"),
            });

            ConfigurarGrilla(dgvOperativosSaldo, new[]
            {
                CrearColumna("Número", nameof(PedidoSaldoPendienteResumen.NumeroPedido)),
                CrearColumna("Cliente", nameof(PedidoSaldoPendienteResumen.Cliente)),
                CrearColumna("Total", nameof(PedidoSaldoPendienteResumen.Total), "N2"),
                CrearColumna("Pagado", nameof(PedidoSaldoPendienteResumen.MontoPagado), "N2"),
                CrearColumna("Saldo", nameof(PedidoSaldoPendienteResumen.SaldoPendiente), "N2"),
            });

            ConfigurarGrilla(dgvOperativosProduccion, new[]
            {
                CrearColumna("Número", nameof(ProduccionEstadoResumen.NumeroPedido)),
                CrearColumna("Cliente", nameof(ProduccionEstadoResumen.Cliente)),
                CrearColumna("Fecha", nameof(ProduccionEstadoResumen.FechaProduccion), formato:"g"),
                CrearColumna("Estado", nameof(ProduccionEstadoResumen.Estado)),
            });

            ConfigurarGrilla(dgvVentasMensuales, new[]
            {
                CrearColumna("Periodo", nameof(VentaPeriodoResumen.Periodo)),
                CrearColumna("Total", nameof(VentaPeriodoResumen.Total), "N2"),
            });

            ConfigurarGrilla(dgvVentasTrimestrales, new[]
            {
                CrearColumna("Periodo", nameof(VentaPeriodoResumen.Periodo)),
                CrearColumna("Total", nameof(VentaPeriodoResumen.Total), "N2"),
            });

            ConfigurarGrilla(dgvVentasAnuales, new[]
            {
                CrearColumna("Periodo", nameof(VentaPeriodoResumen.Periodo)),
                CrearColumna("Total", nameof(VentaPeriodoResumen.Total), "N2"),
            });

            ConfigurarGrilla(dgvVentasComparativas, new[]
            {
                CrearColumna("Tipo", nameof(ComparativaRow.Tipo)),
                CrearColumna("Periodo actual", nameof(ComparativaRow.PeriodoActual)),
                CrearColumna("Monto actual", nameof(ComparativaRow.MontoActual), "N2"),
                CrearColumna("Periodo comparado", nameof(ComparativaRow.PeriodoComparado)),
                CrearColumna("Monto comparado", nameof(ComparativaRow.MontoComparado), "N2"),
                CrearColumna("Diferencia", nameof(ComparativaRow.Diferencia), "N2"),
            });

            ConfigurarGrilla(dgvVentasRanking, new[]
            {
                CrearColumna("Cliente", nameof(ClienteRankingResumen.Cliente)),
                CrearColumna("Total", nameof(ClienteRankingResumen.TotalFacturado), "N2"),
                CrearColumna("Pedidos", nameof(ClienteRankingResumen.CantidadPedidos), "N0"),
            });

            ConfigurarGrilla(dgvVentasCategorias, new[]
            {
                CrearColumna("Categoría", nameof(VentaCategoriaResumen.Categoria)),
                CrearColumna("Total", nameof(VentaCategoriaResumen.Total), "N2"),
            });

            ConfigurarGrilla(dgvFinanzasFacturacion, new[]
            {
                CrearColumna("Periodo", nameof(FacturacionPeriodoResumen.Periodo)),
                CrearColumna("Total", nameof(FacturacionPeriodoResumen.TotalFacturado), "N2"),
            });

            ConfigurarGrilla(dgvFinanzasCuentas, new[]
            {
                CrearColumna("Cliente", nameof(CuentaPorCobrarResumen.Cliente)),
                CrearColumna("Saldo", nameof(CuentaPorCobrarResumen.SaldoPendiente), "N2"),
            });

            ConfigurarGrilla(dgvFinanzasPagos, new[]
            {
                CrearColumna("Número", nameof(PagoRecibidoResumen.NumeroPedido)),
                CrearColumna("Cliente", nameof(PagoRecibidoResumen.Cliente)),
                CrearColumna("Fecha", nameof(PagoRecibidoResumen.Fecha), formato:"g"),
                CrearColumna("Monto", nameof(PagoRecibidoResumen.Monto), "N2"),
            });

            ConfigurarGrilla(dgvFinanzasProyeccion, new[]
            {
                CrearColumna("Número", nameof(ProyeccionIngresoResumen.NumeroPedido)),
                CrearColumna("Cliente", nameof(ProyeccionIngresoResumen.Cliente)),
                CrearColumna("Fecha", nameof(ProyeccionIngresoResumen.FechaEsperada), formato:"d"),
                CrearColumna("Monto", nameof(ProyeccionIngresoResumen.MontoProyectado), "N2"),
            });
        }

        private void InicializarControles()
        {
            nudDiasLimite.Minimum = 0;
            nudDiasLimite.Maximum = 365;
            nudDiasLimite.Value = 7;

            dtpMesComparativo.Format = DateTimePickerFormat.Custom;
            dtpMesComparativo.CustomFormat = "yyyy-MM";
            dtpMesComparativo.ShowUpDown = true;
            dtpMesComparativo.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);

            nudAnioComparativo.Minimum = 2000;
            nudAnioComparativo.Maximum = 2100;
            nudAnioComparativo.Value = Math.Max(2000, DateTime.Today.Year - 1);

            dtpDesde.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpHasta.Value = DateTime.Today;
        }

        private void CargarDatos()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var parametros = new ReporteParametros
                {
                    DiasLimite = (int)nudDiasLimite.Value,
                    MesComparativo = new DateTime(dtpMesComparativo.Value.Year, dtpMesComparativo.Value.Month, 1),
                    AnioComparativo = (int)nudAnioComparativo.Value,
                    PeriodoDesde = dtpDesde.Value.Date,
                    PeriodoHasta = dtpHasta.Value.Date
                };

                _datos = _reporteService.GenerarReportes(parametros);
                ActualizarVista();
            }
            catch (Exception ex)
            {
                MessageBox.Show("report.error.load".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ActualizarVista()
        {
            if (_datos == null)
            {
                dgvOperativosEstado.DataSource = null;
                dgvOperativosLimite.DataSource = null;
                dgvOperativosDemoras.DataSource = null;
                dgvOperativosMuestras.DataSource = null;
                dgvOperativosSaldo.DataSource = null;
                dgvOperativosProduccion.DataSource = null;
                dgvVentasMensuales.DataSource = null;
                dgvVentasTrimestrales.DataSource = null;
                dgvVentasAnuales.DataSource = null;
                dgvVentasComparativas.DataSource = null;
                dgvVentasRanking.DataSource = null;
                dgvVentasCategorias.DataSource = null;
                dgvFinanzasFacturacion.DataSource = null;
                dgvFinanzasCuentas.DataSource = null;
                dgvFinanzasPagos.DataSource = null;
                dgvFinanzasProyeccion.DataSource = null;
                btnExportExcel.Enabled = btnExportPdf.Enabled = btnExportRaw.Enabled = false;
                return;
            }

            dgvOperativosEstado.DataSource = _datos.Operativos.PedidosPorEstado.ToList();
            dgvOperativosLimite.DataSource = _datos.Operativos.PedidosConFechaLimite.ToList();
            dgvOperativosDemoras.DataSource = _datos.Operativos.PedidosDemorados.ToList();
            dgvOperativosMuestras.DataSource = _datos.Operativos.MuestrasVencidas.ToList();
            dgvOperativosSaldo.DataSource = _datos.Operativos.PedidosConSaldoPendiente.ToList();
            dgvOperativosProduccion.DataSource = _datos.Operativos.ProduccionEnCurso.ToList();

            dgvVentasMensuales.DataSource = _datos.Ventas.VentasMensuales.ToList();
            dgvVentasTrimestrales.DataSource = _datos.Ventas.VentasTrimestrales.ToList();
            dgvVentasAnuales.DataSource = _datos.Ventas.VentasAnuales.ToList();
            dgvVentasRanking.DataSource = _datos.Ventas.RankingClientes.ToList();
            dgvVentasCategorias.DataSource = _datos.Ventas.VentasPorCategoria.ToList();

            var comparativas = new List<ComparativaRow>();
            if (_datos.Ventas.ComparativaMensual != null)
            {
                var cmp = _datos.Ventas.ComparativaMensual;
                comparativas.Add(new ComparativaRow
                {
                    Tipo = "report.tab.ventas.mensuales".Traducir(),
                    PeriodoActual = cmp.PeriodoActual,
                    MontoActual = cmp.MontoActual,
                    PeriodoComparado = cmp.PeriodoComparado,
                    MontoComparado = cmp.MontoComparado,
                    Diferencia = cmp.Diferencia
                });
            }
            if (_datos.Ventas.ComparativaAnual != null)
            {
                var cmp = _datos.Ventas.ComparativaAnual;
                comparativas.Add(new ComparativaRow
                {
                    Tipo = "report.tab.ventas.anuales".Traducir(),
                    PeriodoActual = cmp.PeriodoActual,
                    MontoActual = cmp.MontoActual,
                    PeriodoComparado = cmp.PeriodoComparado,
                    MontoComparado = cmp.MontoComparado,
                    Diferencia = cmp.Diferencia
                });
            }
            dgvVentasComparativas.DataSource = comparativas;

            dgvFinanzasFacturacion.DataSource = _datos.Financieros.FacturacionPorPeriodo.ToList();
            dgvFinanzasCuentas.DataSource = _datos.Financieros.CuentasPorCobrar.ToList();
            dgvFinanzasPagos.DataSource = _datos.Financieros.PagosRecibidos.ToList();
            dgvFinanzasProyeccion.DataSource = _datos.Financieros.ProyeccionIngresos.ToList();

            btnExportExcel.Enabled = btnExportPdf.Enabled = btnExportRaw.Enabled = true;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (_datos == null)
                return;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "report.export.selectExcel".Traducir();
                dialog.FileName = $"reportes_{DateTime.Now:yyyyMMddHHmm}.xlsx";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        ReportExportHelper.ExportToExcel(dialog.FileName, _datos);
                        MessageBox.Show("report.export.success".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("report.export.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (_datos == null)
                return;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "report.export.selectPdf".Traducir();
                dialog.FileName = $"reportes_{DateTime.Now:yyyyMMddHHmm}.pdf";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        ReportExportHelper.ExportToPdf(dialog.FileName, _datos);
                        MessageBox.Show("report.export.success".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("report.export.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnExportRaw_Click(object sender, EventArgs e)
        {
            if (_datos == null)
                return;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "report.export.selectRaw".Traducir();
                dialog.FileName = $"reportes_{DateTime.Now:yyyyMMddHHmm}.zip";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        ReportExportHelper.ExportRawData(dialog.FileName, _datos);
                        MessageBox.Show("report.export.success".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("report.export.error".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private static void ConfigurarGrilla(DataGridView grid, IEnumerable<DataGridViewColumn> columnas)
        {
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.MultiSelect = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.Columns.Clear();
            grid.Columns.AddRange(columnas.ToArray());
        }

        private static DataGridViewTextBoxColumn CrearColumna(string encabezado, string propertyName, string formato = null)
        {
            var columna = new DataGridViewTextBoxColumn
            {
                HeaderText = encabezado,
                DataPropertyName = propertyName,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            };

            if (!string.IsNullOrEmpty(formato))
            {
                columna.DefaultCellStyle.Format = formato;
            }

            return columna;
        }

        private sealed class ComparativaRow
        {
            public string Tipo { get; set; }
            public string PeriodoActual { get; set; }
            public decimal MontoActual { get; set; }
            public string PeriodoComparado { get; set; }
            public decimal MontoComparado { get; set; }
            public decimal Diferencia { get; set; }
        }
    }
}