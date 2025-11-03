using BLL.Interfaces;
using BLL.Reportes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UI.Helpers;
using UI.Localization;

namespace UI.Formularios
{
    public partial class ReportesForm : Form
    {
        private readonly IReporteService _reporteService;
        public ReportesForm(IReporteService reporteService)
        {
            _reporteService = reporteService ?? throw new ArgumentNullException(nameof(reporteService));
            InitializeComponent();
        }

        private void ReportesForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            InicializarControles();
            InicializarGrillas();
            CargarVentasPeriodo();
            CargarCategorias();
            CargarFacturacion();
            CargarPedidosCliente();
            CargarPedidosProveedor();
            CargarMejoresClientes();
            CargarClientesSaldo();
            ActualizarBotonesExport();
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
            tabPedidosProveedor.Text = "report.tab.orders.provider".Traducir();
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

            lblPedidosProveedorPeriodo.Text = "report.filters.periodType".Traducir();
            lblPedidosProveedorMes.Text = "report.filters.mes".Traducir();
            lblPedidosProveedorAnio.Text = "report.filters.anio".Traducir();

            lblMejoresClientesPeriodo.Text = "report.filters.periodType".Traducir();
            lblMejoresClientesMes.Text = "report.filters.mes".Traducir();
            lblMejoresClientesAnio.Text = "report.filters.anio".Traducir();

            lblClientesSaldoInfo.Text = "report.filters.noFilters".Traducir();

            var refreshText = "report.actions.refresh".Traducir();
            btnVentasAplicar.Text = refreshText;
            btnCategoriasAplicar.Text = refreshText;
            btnFacturacionAplicar.Text = refreshText;
            btnPedidosClienteAplicar.Text = refreshText;
            btnPedidosProveedorAplicar.Text = refreshText;
            btnMejoresClientesAplicar.Text = refreshText;

            ActualizarOpcionesCombos();
        }
        private void InicializarControles()
        {
            dtpVentasDesde.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpVentasHasta.Value = DateTime.Today;
            dtpVentasMes.Format = DateTimePickerFormat.Custom;
            dtpVentasMes.CustomFormat = "yyyy-MM";
            dtpVentasMes.ShowUpDown = true;
            nudVentasAnio.Minimum = 2000;
            nudVentasAnio.Maximum = 2100;
            nudVentasAnio.Value = DateTime.Today.Year;

            dtpCategoriasMes.Format = DateTimePickerFormat.Custom;
            dtpCategoriasMes.CustomFormat = "yyyy-MM";
            dtpCategoriasMes.ShowUpDown = true;
            nudCategoriasAnio.Minimum = 2000;
            nudCategoriasAnio.Maximum = 2100;
            nudCategoriasAnio.Value = DateTime.Today.Year;

            dtpFacturacionMes.Format = DateTimePickerFormat.Custom;
            dtpFacturacionMes.CustomFormat = "yyyy-MM";
            dtpFacturacionMes.ShowUpDown = true;
            nudFacturacionAnio.Minimum = 2000;
            nudFacturacionAnio.Maximum = 2100;
            nudFacturacionAnio.Value = DateTime.Today.Year;

            dtpPedidosClienteMes.Format = DateTimePickerFormat.Custom;
            dtpPedidosClienteMes.CustomFormat = "yyyy-MM";
            dtpPedidosClienteMes.ShowUpDown = true;
            nudPedidosClienteAnio.Minimum = 2000;
            nudPedidosClienteAnio.Maximum = 2100;
            nudPedidosClienteAnio.Value = DateTime.Today.Year;

            dtpPedidosProveedorMes.Format = DateTimePickerFormat.Custom;
            dtpPedidosProveedorMes.CustomFormat = "yyyy-MM";
            dtpPedidosProveedorMes.ShowUpDown = true;
            nudPedidosProveedorAnio.Minimum = 2000;
            nudPedidosProveedorAnio.Maximum = 2100;
            nudPedidosProveedorAnio.Value = DateTime.Today.Year;

            dtpMejoresClientesMes.Format = DateTimePickerFormat.Custom;
            dtpMejoresClientesMes.CustomFormat = "yyyy-MM";
            dtpMejoresClientesMes.ShowUpDown = true;
            nudMejoresClientesAnio.Minimum = 2000;
            nudMejoresClientesAnio.Maximum = 2100;
            nudMejoresClientesAnio.Value = DateTime.Today.Year;

            ActualizarVisibilidadVentas();
            ActualizarVisibilidadCategorias();
            ActualizarVisibilidadFacturacion();
            ActualizarVisibilidadPedidosCliente();
            ActualizarVisibilidadPedidosProveedor();
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

            ConfigurarGrilla(dgvPedidosProveedor,
                CrearColumna("Proveedor", "Proveedor"),
                CrearColumnaNumerica("Pedidos", "CantidadPedidos", "N0"),
                CrearColumnaNumerica("Productos", "CantidadProductos", "N0"),
                CrearColumnaNumerica("Total", "Total"));

            ConfigurarGrilla(dgvMejoresClientes,
                CrearColumna("Cliente", "Cliente"),
                CrearColumnaNumerica("Pedidos", "CantidadPedidos", "N0"),
                CrearColumnaNumerica("Total", "TotalFacturado"));

            ConfigurarGrilla(dgvClientesSaldo,
                CrearColumna("Cliente", "Cliente"),
                CrearColumnaNumerica("Saldo", "SaldoPendiente"));
        }
        private void ActualizarOpcionesCombos()
        {
            ActualizarComboVentas();
            ActualizarCombo(cmbCategoriasPeriodo, new[] { ReportePeriodoTipo.Todos, ReportePeriodoTipo.Mensual, ReportePeriodoTipo.Anual });
            ActualizarCombo(cmbFacturacionPeriodo, new[] { ReportePeriodoTipo.Mensual, ReportePeriodoTipo.Anual, ReportePeriodoTipo.Todos });
            ActualizarCombo(cmbPedidosClientePeriodo, new[] { ReportePeriodoTipo.Todos, ReportePeriodoTipo.Mensual, ReportePeriodoTipo.Anual });
            ActualizarCombo(cmbPedidosProveedorPeriodo, new[] { ReportePeriodoTipo.Todos, ReportePeriodoTipo.Mensual, ReportePeriodoTipo.Anual });
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
                    ReportExportHelper.ExportToExcel(dialog.FileName, ObtenerTituloActual(), grid);
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
                    ReportExportHelper.ExportToPdf(dialog.FileName, ObtenerTituloActual(), grid);
                }
            }
        }

        private void btnVentasAplicar_Click(object sender, EventArgs e) => CargarVentasPeriodo();

        private void btnCategoriasAplicar_Click(object sender, EventArgs e) => CargarCategorias();

        private void btnFacturacionAplicar_Click(object sender, EventArgs e) => CargarFacturacion();

        private void btnPedidosClienteAplicar_Click(object sender, EventArgs e) => CargarPedidosCliente();

        private void btnPedidosProveedorAplicar_Click(object sender, EventArgs e) => CargarPedidosProveedor();

        private void btnMejoresClientesAplicar_Click(object sender, EventArgs e) => CargarMejoresClientes();

        private void tabReportes_SelectedIndexChanged(object sender, EventArgs e) => ActualizarBotonesExport();

        private void cmbVentasTipo_SelectedIndexChanged(object sender, EventArgs e) => ActualizarVisibilidadVentas();

        private void cmbCategoriasPeriodo_SelectedIndexChanged(object sender, EventArgs e) => ActualizarVisibilidadCategorias();

        private void cmbFacturacionPeriodo_SelectedIndexChanged(object sender, EventArgs e) => ActualizarVisibilidadFacturacion();

        private void cmbPedidosClientePeriodo_SelectedIndexChanged(object sender, EventArgs e) => ActualizarVisibilidadPedidosCliente();

        private void cmbPedidosProveedorPeriodo_SelectedIndexChanged(object sender, EventArgs e) => ActualizarVisibilidadPedidosProveedor();

        private void cmbMejoresClientesPeriodo_SelectedIndexChanged(object sender, EventArgs e) => ActualizarVisibilidadMejoresClientes();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("report.error.load".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("report.error.load".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("report.error.load".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("report.error.load".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ActualizarBotonesExport();
            }
        }

        private void CargarPedidosProveedor()
        {
            try
            {
                var filtro = CrearFiltroPeriodo(cmbPedidosProveedorPeriodo, dtpPedidosProveedorMes, nudPedidosProveedorAnio);
                var datos = _reporteService.ObtenerPedidosPorProveedor(filtro) ?? new List<PedidoProveedorResumen>();
                dgvPedidosProveedor.DataSource = datos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("report.error.load".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("report.error.load".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("report.error.load".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            lblVentasDesde.Visible = dtpVentasDesde.Visible = esRango;
            lblVentasHasta.Visible = dtpVentasHasta.Visible = esRango;
            lblVentasMes.Visible = dtpVentasMes.Visible = esMensual;
            lblVentasAnio.Visible = nudVentasAnio.Visible = esMensual || tipo == ReporteVentasPeriodoTipo.Anual;
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

        private void ActualizarVisibilidadPedidosProveedor()
        {
            ActualizarVisibilidadPeriodo(cmbPedidosProveedorPeriodo, lblPedidosProveedorMes, dtpPedidosProveedorMes, lblPedidosProveedorAnio, nudPedidosProveedorAnio);
        }

        private void ActualizarVisibilidadMejoresClientes()
        {
            ActualizarVisibilidadPeriodo(cmbMejoresClientesPeriodo, lblMejoresClientesMes, dtpMejoresClientesMes, lblMejoresClientesAnio, nudMejoresClientesAnio);
        }

        private void ActualizarVisibilidadPeriodo(ComboBox combo, Control lblMes, Control inputMes, Control lblAnio, Control inputAnio)
        {
            var tipo = combo.SelectedValue is ReportePeriodoTipo valor ? valor : ReportePeriodoTipo.Todos;
            lblMes.Visible = inputMes.Visible = tipo == ReportePeriodoTipo.Mensual;
            lblAnio.Visible = inputAnio.Visible = tipo == ReportePeriodoTipo.Mensual || tipo == ReportePeriodoTipo.Anual;
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
            if (tabReportes.SelectedTab == tabPedidosProveedor) return dgvPedidosProveedor;
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
            if (tabReportes.SelectedTab == tabPedidosProveedor) return tabPedidosProveedor.Text;
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