using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Services.BLL.Interfaces;
using UI.Helpers;
using UI.Localization;

namespace UI.Formularios
{
    public partial class LogsBitacoraForm : Form
    {
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogService _logService;

        public LogsBitacoraForm()
        {
            InitializeComponent();
        }

        public LogsBitacoraForm(IBitacoraService bitacoraService, ILogService logService)
        {
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));

            InitializeComponent();
            this.Load += LogsBitacoraForm_Load;
        }

        private void LogsBitacoraForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            WireUp();
            CargarDatos();
        }

        private void ApplyTexts()
        {
            Text = "logsBitacora.title".Traducir();
            tabLogs.Text = "logsBitacora.tab.logs".Traducir();
            tabBitacora.Text = "logsBitacora.tab.audit".Traducir();
            btnActualizar.Text = "form.refresh".Traducir();
            btnLimpiarLogs.Text = "logsBitacora.clean".Traducir();
            btnExportar.Text = "logsBitacora.export".Traducir();
            btnCerrar.Text = "form.close".Traducir();

            lblFiltroFecha.Text = "logsBitacora.filter.date".Traducir();
            lblFiltroUsuario.Text = "logsBitacora.filter.user".Traducir();
            lblFiltroModulo.Text = "logsBitacora.filter.module".Traducir();
            chkSoloErrores.Text = "logsBitacora.filter.errors".Traducir();
        }

        private void WireUp()
        {
            btnActualizar.Click += (s, e) => CargarDatos();
            btnLimpiarLogs.Click += (s, e) => LimpiarLogsAntiguos();
            btnExportar.Click += (s, e) => ExportarDatos();
            btnCerrar.Click += (s, e) => Close();

            // Filtros
            dtpFiltroFecha.ValueChanged += (s, e) => FiltrarBitacora();
            txtFiltroUsuario.TextChanged += (s, e) => FiltrarBitacora();
            cboFiltroModulo.SelectedIndexChanged += (s, e) => FiltrarBitacora();
            chkSoloErrores.CheckedChanged += (s, e) => FiltrarBitacora();
        }

        private void CargarDatos()
        {
            try
            {
                // Cargar Logs
                var logs = _logService.ObtenerUltimosLogs(500);
                txtLogs.Lines = logs;
                if (logs.Length > 0)
                {
                    txtLogs.SelectionStart = txtLogs.Text.Length;
                    txtLogs.ScrollToCaret();
                }

                // Cargar Bitácora
                CargarBitacora();

                // Cargar combos de filtros
                CargarCombosFiltroBitacora();
            }
            catch (Exception ex)
            {
                MessageBox.Show("logsBitacora.error.load".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarBitacora()
        {
            try
            {
                var fechaDesde = DateTime.Now.AddDays(-30); // Últimos 30 días
                var bitacoras = _bitacoraService.ObtenerPorFecha(fechaDesde, DateTime.Now)
                    .OrderByDescending(b => b.Fecha)
                    .Take(1000)
                    .Select(b => new
                    {
                        Fecha = b.Fecha,
                        Usuario = b.Usuario?.NombreUsuario ?? "logsBitacora.system".Traducir(),
                        Accion = LocalizationHelper.TranslateAction(b.Accion),
                        Modulo = LocalizationHelper.TranslateModule(b.Modulo),
                        Descripcion = LocalizationHelper.TranslateDescription(b.Descripcion),
                        Exitoso = LocalizationHelper.TranslateBoolean(b.Exitoso),
                        DireccionIP = b.DireccionIP ?? string.Empty,
                        MensajeError = LocalizationHelper.TranslateError(b.MensajeError)
                    }).ToList();

                dgvBitacora.DataSource = bitacoras;
                FormatearGridBitacora();
            }
            catch (Exception ex)
            {
                MessageBox.Show("logsBitacora.error.audit".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatearGridBitacora()
        {
            if (dgvBitacora.Columns.Count == 0) return;

            dgvBitacora.Columns["Fecha"].HeaderText = "logsBitacora.column.date".Traducir();
            dgvBitacora.Columns["Fecha"].Width = 130;
            dgvBitacora.Columns["Usuario"].HeaderText = "logsBitacora.column.user".Traducir();
            dgvBitacora.Columns["Usuario"].Width = 100;
            dgvBitacora.Columns["Accion"].HeaderText = "logsBitacora.column.action".Traducir();
            dgvBitacora.Columns["Accion"].Width = 120;
            dgvBitacora.Columns["Modulo"].HeaderText = "logsBitacora.column.module".Traducir();
            dgvBitacora.Columns["Modulo"].Width = 80;
            dgvBitacora.Columns["Descripcion"].HeaderText = "logsBitacora.column.description".Traducir();
            dgvBitacora.Columns["Descripcion"].Width = 200;
            dgvBitacora.Columns["Exitoso"].HeaderText = "logsBitacora.column.success".Traducir();
            dgvBitacora.Columns["Exitoso"].Width = 60;
            dgvBitacora.Columns["DireccionIP"].HeaderText = "logsBitacora.column.ip".Traducir();
            dgvBitacora.Columns["DireccionIP"].Width = 100;
            dgvBitacora.Columns["MensajeError"].HeaderText = "logsBitacora.column.error".Traducir();
            dgvBitacora.Columns["MensajeError"].Width = 200;

            // Colorear filas con errores
            dgvBitacora.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var exitoso = dgvBitacora.Rows[e.RowIndex].Cells["Exitoso"].Value?.ToString();
                if (string.Equals(exitoso, "common.no".Traducir(), StringComparison.OrdinalIgnoreCase))
                {
                    dgvBitacora.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                }
            };
        }

        private void CargarCombosFiltroBitacora()
        {
            try
            {
                // Combo de módulos
                var modulos = _bitacoraService.ObtenerPorFecha(DateTime.Now.AddDays(-30), DateTime.Now)
                    .Select(b => b.Modulo)
                    .Where(m => !string.IsNullOrEmpty(m))
                    .Distinct()
                    .OrderBy(m => m)
                    .Select(m => new ModuloItem(m))
                    .ToList();

                cboFiltroModulo.Items.Clear();
                cboFiltroModulo.Items.Add(ModuloItem.Todos());
                foreach (var modulo in modulos)
                    cboFiltroModulo.Items.Add(modulo);
                cboFiltroModulo.SelectedIndex = 0;

                // Fecha por defecto: últimos 7 días
                dtpFiltroFecha.Value = DateTime.Now.AddDays(-7);
            }
            catch (Exception ex)
            {
                MessageBox.Show("logsBitacora.error.filters".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarBitacora()
        {
            try
            {
                var fechaDesde = dtpFiltroFecha.Value.Date;
                var usuario = txtFiltroUsuario.Text.Trim();
                var moduloSeleccionado = cboFiltroModulo.SelectedItem as ModuloItem;
                var modulo = moduloSeleccionado?.Value;
                var soloErrores = chkSoloErrores.Checked;

                var bitacoras = _bitacoraService.ObtenerPorFecha(fechaDesde, DateTime.Now)
                    .AsEnumerable();

                // Filtro por usuario
                if (!string.IsNullOrEmpty(usuario))
                {
                    bitacoras = bitacoras.Where(b =>
                        b.Usuario?.NombreUsuario?.ToLower().Contains(usuario.ToLower()) == true);
                }

                // Filtro por módulo
                if (!string.IsNullOrEmpty(modulo) && !ModuloItem.EsTodos(moduloSeleccionado))
                {
                    bitacoras = bitacoras.Where(b => b.Modulo == modulo);
                }

                // Filtro solo errores
                if (soloErrores)
                {
                    bitacoras = bitacoras.Where(b => !b.Exitoso);
                }

                var resultado = bitacoras
                    .OrderByDescending(b => b.Fecha)
                    .Take(1000)
                    .Select(b => new
                    {
                        Fecha = b.Fecha,
                        Usuario = b.Usuario?.NombreUsuario ?? "logsBitacora.system".Traducir(),
                        Accion = LocalizationHelper.TranslateAction(b.Accion),
                        Modulo = LocalizationHelper.TranslateModule(b.Modulo),
                        Descripcion = LocalizationHelper.TranslateDescription(b.Descripcion),
                        Exitoso = LocalizationHelper.TranslateBoolean(b.Exitoso),
                        DireccionIP = b.DireccionIP ?? string.Empty,
                        MensajeError = LocalizationHelper.TranslateError(b.MensajeError)
                    }).ToList();

                dgvBitacora.DataSource = resultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show("logsBitacora.error.filter".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarLogsAntiguos()
        {
            try
            {
                var result = MessageBox.Show(
                    "logsBitacora.clean.confirm".Traducir(),
                    "logsBitacora.clean.title".Traducir(),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _logService.LimpiarLogsAntiguos(30);
                    MessageBox.Show("logsBitacora.clean.success".Traducir(), Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("logsBitacora.error.clean".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarDatos()
        {
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "logsBitacora.export.filter".Traducir();
                    sfd.FileName = $"logs_bitacora_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        var contenido = $"{"logsBitacora.export.header.logs".Traducir()} - {DateTime.Now}\n";
                        contenido += new string('=', 50) + "\n\n";
                        contenido += string.Join("\n", _logService.ObtenerUltimosLogs(1000));
                        contenido += "\n\n" + new string('=', 50) + "\n";
                        contenido += "logsBitacora.export.header.audit".Traducir() + "\n";
                        contenido += new string('=', 50) + "\n\n";

                        // Agregar bitácora
                        var bitacoras = _bitacoraService.ObtenerPorFecha(DateTime.Now.AddDays(-30), DateTime.Now);
                        foreach (var b in bitacoras.OrderByDescending(x => x.Fecha))
                        {
                            contenido += $"{b.Fecha:yyyy-MM-dd HH:mm:ss} | {b.Usuario?.NombreUsuario ?? "logsBitacora.system".Traducir()} | ";
                            contenido += $"{LocalizationHelper.TranslateAction(b.Accion)} | {LocalizationHelper.TranslateModule(b.Modulo)} | {LocalizationHelper.TranslateDescription(b.Descripcion)}";
                            if (!b.Exitoso && !string.IsNullOrEmpty(b.MensajeError))
                                contenido += $" | {"logsBitacora.export.error".Traducir(LocalizationHelper.TranslateError(b.MensajeError))}";
                            contenido += "\n";
                        }

                        System.IO.File.WriteAllText(sfd.FileName, contenido);
                        MessageBox.Show("logsBitacora.export.success".Traducir(sfd.FileName), Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("logsBitacora.error.export".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private sealed class ModuloItem
        {
            private readonly string _display;

            private ModuloItem(string value, string display)
            {
                Value = value;
                _display = display;
            }

            public ModuloItem(string value)
            {
                Value = value;
                _display = LocalizationHelper.TranslateModule(value);
            }

            public string Value { get; }

            public override string ToString() => _display;

            public static ModuloItem Todos()
                => new ModuloItem(null, "logsBitacora.filter.module.all".Traducir());

            public static bool EsTodos(ModuloItem item)
                => item != null && string.IsNullOrEmpty(item.Value);
        }
    }
}