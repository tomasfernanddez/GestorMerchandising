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
        }

        private void LogsBitacoraForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            WireUp();
            CargarDatos();
        }

        private void ApplyTexts()
        {
            Text = "Logs del Sistema y Bitácora";
            tabLogs.Text = "Logs de Sistema";
            tabBitacora.Text = "Bitácora de Usuarios";
            btnActualizar.Text = "Actualizar";
            btnLimpiarLogs.Text = "Limpiar Logs Antiguos";
            btnExportar.Text = "Exportar";
            btnCerrar.Text = "Cerrar";

            // Filtros
            lblFiltroFecha.Text = "Desde:";
            lblFiltroUsuario.Text = "Usuario:";
            lblFiltroModulo.Text = "Módulo:";
            chkSoloErrores.Text = "Solo errores";
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
                MessageBox.Show($"Error cargando datos: {ex.Message}", Text,
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
                        Usuario = b.Usuario?.NombreUsuario ?? "Sistema",
                        Accion = b.Accion,
                        Modulo = b.Modulo ?? "",
                        Descripcion = b.Descripcion ?? "",
                        Exitoso = b.Exitoso ? "Sí" : "No",
                        DireccionIP = b.DireccionIP ?? "",
                        MensajeError = b.MensajeError ?? ""
                    }).ToList();

                dgvBitacora.DataSource = bitacoras;
                FormatearGridBitacora();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando bitácora: {ex.Message}", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatearGridBitacora()
        {
            if (dgvBitacora.Columns.Count == 0) return;

            dgvBitacora.Columns["Fecha"].HeaderText = "Fecha";
            dgvBitacora.Columns["Fecha"].Width = 130;
            dgvBitacora.Columns["Usuario"].HeaderText = "Usuario";
            dgvBitacora.Columns["Usuario"].Width = 100;
            dgvBitacora.Columns["Accion"].HeaderText = "Acción";
            dgvBitacora.Columns["Accion"].Width = 120;
            dgvBitacora.Columns["Modulo"].HeaderText = "Módulo";
            dgvBitacora.Columns["Modulo"].Width = 80;
            dgvBitacora.Columns["Descripcion"].HeaderText = "Descripción";
            dgvBitacora.Columns["Descripcion"].Width = 200;
            dgvBitacora.Columns["Exitoso"].HeaderText = "Exitoso";
            dgvBitacora.Columns["Exitoso"].Width = 60;
            dgvBitacora.Columns["DireccionIP"].HeaderText = "IP";
            dgvBitacora.Columns["DireccionIP"].Width = 100;
            dgvBitacora.Columns["MensajeError"].HeaderText = "Error";
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
                cboFiltroModulo.Items.Add("Todos");
                foreach (var modulo in modulos)
                {
                    cboFiltroModulo.Items.Add(modulo);
                }
                cboFiltroModulo.SelectedIndex = 0;

                // Fecha por defecto: últimos 7 días
                dtpFiltroFecha.Value = DateTime.Now.AddDays(-7);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando filtros: {ex.Message}", Text,
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
                if (!string.IsNullOrEmpty(modulo) && modulo != "Todos")
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
                        Usuario = b.Usuario?.NombreUsuario ?? "Sistema",
                        Accion = b.Accion,
                        Modulo = b.Modulo ?? "",
                        Descripcion = b.Descripcion ?? "",
                        Exitoso = b.Exitoso ? "Sí" : "No",
                        DireccionIP = b.DireccionIP ?? "",
                        MensajeError = b.MensajeError ?? ""
                    }).ToList();

                dgvBitacora.DataSource = resultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtrando bitácora: {ex.Message}", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarLogsAntiguos()
        {
            try
            {
                var result = MessageBox.Show(
                    "¿Desea eliminar los logs de más de 30 días?\n\nEsta acción no se puede deshacer.",
                    "Confirmar limpieza",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _logService.LimpiarLogsAntiguos(30);
                    MessageBox.Show("Logs antiguos eliminados correctamente.", Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error limpiando logs: {ex.Message}", Text,
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
                        MessageBox.Show($"Datos exportados correctamente a:\n{sfd.FileName}", Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exportando datos: {ex.Message}", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}