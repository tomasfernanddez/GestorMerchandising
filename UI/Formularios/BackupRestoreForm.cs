using Services;
using Services.BLL.Interfaces;
using Services.BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UI.Localization;

namespace UI.Formularios
{
    public partial class BackupRestoreForm : Form
    {
        private readonly IBackupService _backupService;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogService _logService;
        private readonly BindingList<BackupItemViewModel> _backups = new BindingList<BackupItemViewModel>();
        private readonly Dictionary<string, (string Es, string En)> _diccionarioMensajes;

        private sealed class BackupItemViewModel
        {
            public string Nombre { get; set; }
            public string RutaCompleta { get; set; }
            public DateTime Fecha { get; set; }
            public long TamanoBytes { get; set; }
            public string TamanoLegible { get; set; }
        }

        public BackupRestoreForm(
            IBackupService backupService,
            IBitacoraService bitacoraService,
            ILogService logService)
        {
            _backupService = backupService ?? throw new ArgumentNullException(nameof(backupService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));

            _diccionarioMensajes = CrearDiccionarioMensajes();

            InitializeComponent();

            Load += BackupRestoreForm_Load;
        }

        private void BackupRestoreForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            ConfigurarGrid();
            WireUpEvents();
            CargarBackups();
        }

        private void ApplyTexts()
        {
            Text = "backup.title".Traducir();
            lblDescripcion.Text = "backup.description".Traducir();
            lblDirectorio.Text = string.Format("backup.directory".Traducir(), _backupService.ObtenerDirectorioBackups());
            lnkAbrirCarpeta.Text = "backup.openFolder".Traducir();
            lblAyudaRestore.Text = "backup.restore.hint".Traducir();
            btnRefrescar.Text = "form.refresh".Traducir();
            btnGenerarBackup.Text = "backup.button.create".Traducir();
            btnRestaurar.Text = "backup.button.restore".Traducir();
            btnCerrar.Text = "form.close".Traducir();

            ActualizarCabecerasGrid();
            ActualizarEstadoBackups();
        }

        private void ConfigurarGrid()
        {
            dgvBackups.AutoGenerateColumns = false;
            dgvBackups.Columns.Clear();
            dgvBackups.AllowUserToAddRows = false;
            dgvBackups.AllowUserToDeleteRows = false;
            dgvBackups.MultiSelect = false;
            dgvBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBackups.ReadOnly = true;

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BackupItemViewModel.Nombre),
                Name = nameof(BackupItemViewModel.Nombre),
                HeaderText = "backup.grid.name".Traducir(),
                FillWeight = 220,
                MinimumWidth = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BackupItemViewModel.Fecha),
                Name = nameof(BackupItemViewModel.Fecha),
                HeaderText = "backup.grid.date".Traducir(),
                DefaultCellStyle = { Format = "g" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BackupItemViewModel.TamanoLegible),
                Name = nameof(BackupItemViewModel.TamanoLegible),
                HeaderText = "backup.grid.size".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgvBackups.DataSource = _backups;
        }

        private void WireUpEvents()
        {
            btnRefrescar.Click += (s, e) => CargarBackups();
            btnGenerarBackup.Click += BtnGenerarBackup_Click;
            btnRestaurar.Click += BtnRestaurar_Click;
            btnCerrar.Click += (s, e) => Close();
            lnkAbrirCarpeta.LinkClicked += LnkAbrirCarpeta_LinkClicked;
            dgvBackups.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    BtnRestaurar_Click(s, EventArgs.Empty);
                }
            };
        }

        private void BtnGenerarBackup_Click(object sender, EventArgs e)
        {
            using (var dialogo = new SaveFileDialog())
            {
                dialogo.Filter = "backup.select.filter".Traducir();
                dialogo.InitialDirectory = _backupService.ObtenerDirectorioBackups();
                dialogo.FileName = _backupService.GenerarNombreSugerido();

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var nombreDestino = Path.GetFileName(dialogo.FileName);

                EjecutarOperacion(() =>
                {
                    var rutaGenerada = _backupService.RealizarBackup(dialogo.FileName);
                    var nombreArchivo = Path.GetFileName(rutaGenerada);
                    RegistrarInfo("Backup.Crear", "backup.log.create.success", nombreArchivo);
                    MessageBox.Show(string.Format("backup.success".Traducir(), nombreArchivo), Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarBackups();
                },
                ex => RegistrarError("Backup.Crear", "backup.log.create.error", ex, nombreDestino ?? dialogo.FileName),
                ex => MessageBox.Show(string.Format("backup.error".Traducir(), ex.Message), Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error));
            }
        }

        private void BtnRestaurar_Click(object sender, EventArgs e)
        {
            var seleccionado = ObtenerSeleccion();
            string ruta = seleccionado?.RutaCompleta;
            string nombre = seleccionado?.Nombre;

            if (string.IsNullOrWhiteSpace(ruta))
            {
                using (var dialogo = new OpenFileDialog())
                {
                    dialogo.Filter = "backup.select.filter".Traducir();
                    dialogo.InitialDirectory = _backupService.ObtenerDirectorioBackups();

                    if (dialogo.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    ruta = dialogo.FileName;
                    nombre = Path.GetFileName(ruta);
                }
            }

            if (string.IsNullOrWhiteSpace(ruta) || string.IsNullOrWhiteSpace(nombre))
            {
                return;
            }

            var confirm = MessageBox.Show(string.Format("backup.confirm.restore".Traducir(), nombre), Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            var nombreFinal = nombre;

            EjecutarOperacion(() =>
            {
                _backupService.RestaurarBackup(ruta);
                RegistrarInfo("Backup.Restaurar", "backup.log.restore.success", nombreFinal);
                MessageBox.Show("backup.restore.success".Traducir(), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            },
            ex => RegistrarError("Backup.Restaurar", "backup.log.restore.error", ex, nombreFinal ?? ruta),
            ex => MessageBox.Show(string.Format("backup.restore.error".Traducir(), ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error));
        }

        private void LnkAbrirCarpeta_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var carpeta = _backupService.ObtenerDirectorioBackups();
                if (Directory.Exists(carpeta))
                {
                    Process.Start("explorer.exe", carpeta);
                }
                else
                {
                    MessageBox.Show("backup.folder.notFound".Traducir(), Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("backup.folder.error".Traducir(), ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarBackups()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _backups.Clear();

                var elementos = _backupService.ListarBackups() ?? Enumerable.Empty<BackupFileInfo>();
                foreach (var backup in elementos)
                {
                    _backups.Add(new BackupItemViewModel
                    {
                        Nombre = backup.Nombre,
                        RutaCompleta = backup.RutaCompleta,
                        Fecha = backup.FechaCreacion,
                        TamanoBytes = backup.TamanoBytes,
                        TamanoLegible = backup.TamanoLegible()
                    });
                }

                ActualizarEstadoBackups();
            }
            catch (Exception ex)
            {
                RegistrarError("Backup.Listar", "backup.log.list.error", ex, ex.Message);
                MessageBox.Show(string.Format("backup.list.error".Traducir(), ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ActualizarCabecerasGrid()
        {
            if (dgvBackups.Columns.Count == 0)
            {
                return;
            }

            dgvBackups.Columns[nameof(BackupItemViewModel.Nombre)].HeaderText = "backup.grid.name".Traducir();
            dgvBackups.Columns[nameof(BackupItemViewModel.Fecha)].HeaderText = "backup.grid.date".Traducir();
            dgvBackups.Columns[nameof(BackupItemViewModel.TamanoLegible)].HeaderText = "backup.grid.size".Traducir();
        }

        private void ActualizarEstadoBackups()
        {
            if (_backups.Count == 0)
            {
                lblUltimoBackup.Text = "backup.noFiles".Traducir();
                return;
            }

            var masReciente = _backups.OrderByDescending(b => b.Fecha).First();
            lblUltimoBackup.Text = string.Format("backup.lastRun".Traducir(), masReciente.Fecha.ToString("g"));
        }

        private BackupItemViewModel ObtenerSeleccion()
        {
            if (dgvBackups.CurrentRow?.DataBoundItem is BackupItemViewModel seleccionado)
            {
                return seleccionado;
            }

            return null;
        }

        private void EjecutarOperacion(Action accion, Action<Exception> registrarError, Action<Exception> manejarError)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                accion();
            }
            catch (Exception ex)
            {
                registrarError?.Invoke(ex);
                manejarError?.Invoke(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void RegistrarInfo(string accion, string claveMensaje, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Seguridad");
            }
            catch
            {
                // Ignorar errores de bitácora para no interrumpir la experiencia de usuario
            }

            _logService.LogInfo(mensaje, "Seguridad", SessionContext.NombreUsuario);
        }

        private void RegistrarError(string accion, string claveMensaje, Exception ex, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Seguridad", false,
                    ex?.Message);
            }
            catch
            {
                // Ignorar errores de bitácora
            }

            _logService.LogError(mensaje, ex, "Seguridad", SessionContext.NombreUsuario);
        }

        private Dictionary<string, (string Es, string En)> CrearDiccionarioMensajes()
        {
            return new Dictionary<string, (string Es, string En)>
            {
                ["backup.log.create.success"] = ("Se generó el backup {0} correctamente.", "Backup {0} generated successfully."),
                ["backup.log.create.error"] = ("Error al generar el backup {0}.", "Error generating backup {0}."),
                ["backup.log.restore.success"] = ("Se restauró la base de datos desde {0}.", "Database restored from {0}."),
                ["backup.log.restore.error"] = ("Error al restaurar la base de datos desde {0}.", "Error restoring database from {0}."),
                ["backup.log.list.error"] = ("Error al listar backups: {0}.", "Error listing backups: {0}."),
            };
        }

        private string ObtenerMensaje(string clave, params object[] args)
        {
            if (_diccionarioMensajes.TryGetValue(clave, out var textos))
            {
                var mensajeEs = args != null && args.Length > 0 ? string.Format(textos.Es, args) : textos.Es;
                var mensajeEn = args != null && args.Length > 0 ? string.Format(textos.En, args) : textos.En;
                return string.Concat(mensajeEs, " / ", mensajeEn);
            }

            if (args != null && args.Length > 0)
            {
                return string.Format(clave, args);
            }

            return clave;
        }
    }
}