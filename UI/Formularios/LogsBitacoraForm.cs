using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Services.BLL.Interfaces;
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
            Localization.LanguageChanged += OnLanguageChanged;
        }

        private void LogsBitacoraForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            WireUp();
            CargarDatos();
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            ApplyTexts();
            FormatearGridBitacora();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Localization.LanguageChanged -= OnLanguageChanged;
            }
            base.Dispose(disposing);
        }

        private void ApplyTexts()
        {
            Text = "bitacora.title".Traducir();
            tabLogs.Text = "bitacora.tab.logs".Traducir();
            tabBitacora.Text = "bitacora.tab.audit".Traducir();
            btnActualizar.Text = "bitacora.button.refresh".Traducir();
            btnLimpiarLogs.Text = "bitacora.button.clean".Traducir();
            btnExportar.Text = "bitacora.button.export".Traducir();
            btnCerrar.Text = "form.cerrar".Traducir();

            // Filtros
            lblFiltroFecha.Text = "bitacora.filter.dateFrom".Traducir();
            lblFiltroUsuario.Text = "bitacora.filter.user".Traducir();
            lblFiltroModulo.Text = "bitacora.filter.module".Traducir();
            chkSoloErrores.Text = "bitacora.filter.onlyErrors".Traducir();
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
                MessageBox.Show("bitacora.error.load".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string TraducirModulo(string modulo)
        {
            if (string.IsNullOrEmpty(modulo)) return "";
            var key = $"module.{modulo.ToLower().Replace(" ", "_")}";
            var traduccion = Localization.T(key);
            return traduccion == key ? modulo : traduccion;
        }

        private string TraducirAccion(string accion)
        {
            if (string.IsNullOrEmpty(accion)) return "";
            var key = $"action.{accion.ToLower().Replace(" ", "_")}";
            var traduccion = Localization.T(key);
            return traduccion == key ? accion : traduccion;
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
                        Usuario = b.Usuario?.NombreUsuario ?? "Sistema",
                        Accion = TraducirAccion(b.Accion),
                        Modulo = TraducirModulo(b.Modulo ?? ""),
                        Descripcion = b.Descripcion ?? "",
                        Exitoso = b.Exitoso ? "form.filter.yes".Traducir() : "form.filter.no".Traducir(),
                        DireccionIP = b.DireccionIP ?? "",
                        MensajeError = b.MensajeError ?? ""
                    }).ToList();

                dgvBitacora.DataSource = bitacoras;
                FormatearGridBitacora();
            }
            catch (Exception ex)
            {
                MessageBox.Show("bitacora.error.load".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatearGridBitacora()
        {
            if (dgvBitacora.Columns.Count == 0) return;

            dgvBitacora.Columns["Fecha"].HeaderText = "bitacora.column.date".Traducir();
            dgvBitacora.Columns["Fecha"].Width = 130;
            dgvBitacora.Columns["Usuario"].HeaderText = "bitacora.column.user".Traducir();
            dgvBitacora.Columns["Usuario"].Width = 100;
            dgvBitacora.Columns["Accion"].HeaderText = "bitacora.column.action".Traducir();
            dgvBitacora.Columns["Accion"].Width = 120;
            dgvBitacora.Columns["Modulo"].HeaderText = "bitacora.column.module".Traducir();
            dgvBitacora.Columns["Modulo"].Width = 80;
            dgvBitacora.Columns["Descripcion"].HeaderText = "bitacora.column.description".Traducir();
            dgvBitacora.Columns["Descripcion"].Width = 200;
            dgvBitacora.Columns["Exitoso"].HeaderText = "bitacora.column.success".Traducir();
            dgvBitacora.Columns["Exitoso"].Width = 60;
            dgvBitacora.Columns["DireccionIP"].HeaderText = "bitacora.column.ip".Traducir();
            dgvBitacora.Columns["DireccionIP"].Width = 100;
            dgvBitacora.Columns["MensajeError"].HeaderText = "bitacora.column.error".Traducir();
            dgvBitacora.Columns["MensajeError"].Width = 200;

            // Colorear filas con errores
            dgvBitacora.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var exitoso = dgvBitacora.Rows[e.RowIndex].Cells["Exitoso"].Value?.ToString();
                if (exitoso == "No")
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
                    .ToList();

                cboFiltroModulo.Items.Clear();
                cboFiltroModulo.Items.Add("bitacora.filter.all".Traducir());
                foreach (var modulo in modulos)
                {
                    cboFiltroModulo.Items.Add(TraducirModulo(modulo));
                }
                cboFiltroModulo.SelectedIndex = 0;

                // Fecha por defecto: últimos 7 días
                dtpFiltroFecha.Value = DateTime.Now.AddDays(-7);
            }
            catch (Exception ex)
            {
                MessageBox.Show("bitacora.error.load".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarBitacora()
        {
            try
            {
                var fechaDesde = dtpFiltroFecha.Value.Date;
                var usuario = txtFiltroUsuario.Text.Trim();
                var modulo = cboFiltroModulo.SelectedItem?.ToString();
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
                var moduloAll = "bitacora.filter.all".Traducir();
                if (!string.IsNullOrEmpty(modulo) && modulo != moduloAll)
                {
                    bitacoras = bitacoras.Where(b => TraducirModulo(b.Modulo) == modulo);
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
                        Usuario = b.Usuario?.NombreUsuario ?? "Sistema",
                        Accion = TraducirAccion(b.Accion),
                        Modulo = TraducirModulo(b.Modulo ?? ""),
                        Descripcion = b.Descripcion ?? "",
                        Exitoso = b.Exitoso ? "form.filter.yes".Traducir() : "form.filter.no".Traducir(),
                        DireccionIP = b.DireccionIP ?? "",
                        MensajeError = b.MensajeError ?? ""
                    }).ToList();

                dgvBitacora.DataSource = resultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show("bitacora.error.load".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarLogsAntiguos()
        {
            try
            {
                var result = MessageBox.Show(
                    "bitacora.clean.confirm".Traducir(),
                    Text,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _logService.LimpiarLogsAntiguos(30);
                    MessageBox.Show("bitacora.clean.success".Traducir(), Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("bitacora.clean.error".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarDatos()
        {
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Archivos de texto (*.txt)|*.txt";
                    sfd.FileName = $"logs_bitacora_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        var contenido = $"LOGS DEL SISTEMA - {DateTime.Now}\n";
                        contenido += new string('=', 50) + "\n\n";
                        contenido += string.Join("\n", _logService.ObtenerUltimosLogs(1000));
                        contenido += "\n\n" + new string('=', 50) + "\n";
                        contenido += "BITÁCORA DE USUARIOS\n";
                        contenido += new string('=', 50) + "\n\n";

                        // Agregar bitácora
                        var bitacoras = _bitacoraService.ObtenerPorFecha(DateTime.Now.AddDays(-30), DateTime.Now);
                        foreach (var b in bitacoras.OrderByDescending(x => x.Fecha))
                        {
                            contenido += $"{b.Fecha:yyyy-MM-dd HH:mm:ss} | {b.Usuario?.NombreUsuario ?? "Sistema"} | ";
                            contenido += $"{b.Accion} | {b.Modulo} | {b.Descripcion}";
                            if (!b.Exitoso && !string.IsNullOrEmpty(b.MensajeError))
                                contenido += $" | ERROR: {b.MensajeError}";
                            contenido += "\n";
                        }

                        System.IO.File.WriteAllText(sfd.FileName, contenido);
                        MessageBox.Show("bitacora.export.success".Traducir(), Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("bitacora.export.error".Traducir(ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}