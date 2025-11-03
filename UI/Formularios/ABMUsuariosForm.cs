using BLL.Interfaces;
using Services.BLL.Interfaces;
using Services.DomainModel.Entities;
using DomainModel;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UI.Localization;


namespace UI.Formularios
{
    public partial class ABMUsuariosForm : Form
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPerfilService _perfilService;
        private readonly IBitacoraService _bitacora;
        private readonly ILogService _logService;
        private bool _puedeGestionar;
        private enum FiltroUsuarios { Activos, Bloqueados, Inactivos }
        private FiltroUsuarios _filtroActual = FiltroUsuarios.Activos;

        public ABMUsuariosForm()
        {
            InitializeComponent();
        }

        public ABMUsuariosForm(IUsuarioService usuarioService, IPerfilService perfilService,
            IBitacoraService bitacora, ILogService logService)
        {
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
            _bitacora = bitacora ?? throw new ArgumentNullException(nameof(bitacora));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));

            InitializeComponent();
            this.Load += ABMUsuariosForm_Load;
            Localization.LanguageChanged += OnLanguageChanged;
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            ApplyTexts();
            ActualizarColumnas();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Localization.LanguageChanged -= OnLanguageChanged;
            }
            base.Dispose(disposing);
        }

        private sealed class UsuarioGridRow
        {
            public Guid IdUsuario { get; set; }
            public string NombreUsuario { get; set; }
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Perfil { get; set; }
            public bool Activo { get; set; }
            public bool Bloqueado { get; set; }
            public DateTime FechaCreacion { get; set; }
            public DateTime? FechaUltimoAcceso { get; set; }
            public int IntentosLoginFallidos { get; set; }
        }

        private void ABMUsuariosForm_Load(object sender, EventArgs e)
        {
            // Solo administradores pueden gestionar usuarios
            _puedeGestionar = string.Equals(SessionContext.NombrePerfil, "Administrador",
                StringComparison.OrdinalIgnoreCase);

            if (!_puedeGestionar)
            {
                MessageBox.Show("abm.usuarios.error.load".Traducir(), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            EnsureColumns();
            ApplyTexts();
            WireUp();
            CargarUsuarios();
        }

        private void EnsureColumns()
        {
            if (dgvUsuarios.Columns.Count > 0) return;

            dgvUsuarios.AutoGenerateColumns = false;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.AllowUserToResizeColumns = true;

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UsuarioGridRow.IdUsuario),
                Name = "IdUsuario",
                Visible = false
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UsuarioGridRow.NombreUsuario),
                Name = "NombreUsuario",
                HeaderText = "abm.usuarios.column.username".Traducir(),
                FillWeight = 120,
                MinimumWidth = 100
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UsuarioGridRow.NombreCompleto),
                Name = "NombreCompleto",
                HeaderText = "abm.usuarios.column.fullname".Traducir(),
                FillWeight = 180,
                MinimumWidth = 150
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UsuarioGridRow.Email),
                Name = "Email",
                HeaderText = "abm.usuarios.column.email".Traducir(),
                FillWeight = 180,
                MinimumWidth = 150
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UsuarioGridRow.Perfil),
                Name = "Perfil",
                HeaderText = "abm.usuarios.column.profile".Traducir(),
                FillWeight = 140,
                MinimumWidth = 120
            });

            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(UsuarioGridRow.Activo),
                Name = "Activo",
                HeaderText = "abm.usuarios.column.active".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                MinimumWidth = 60
            });

            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(UsuarioGridRow.Bloqueado),
                Name = "Bloqueado",
                HeaderText = "abm.usuarios.column.blocked".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                MinimumWidth = 80
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UsuarioGridRow.FechaCreacion),
                Name = "FechaCreacion",
                HeaderText = "abm.usuarios.column.created".Traducir(),
                FillWeight = 110,
                MinimumWidth = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UsuarioGridRow.FechaUltimoAcceso),
                Name = "FechaUltimoAcceso",
                HeaderText = "abm.usuarios.column.lastAccess".Traducir(),
                FillWeight = 110,
                MinimumWidth = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });
        }

        private void ActualizarColumnas()
        {
            if (dgvUsuarios.Columns.Count == 0) return;

            dgvUsuarios.Columns["NombreUsuario"].HeaderText = "abm.usuarios.column.username".Traducir();
            dgvUsuarios.Columns["NombreCompleto"].HeaderText = "abm.usuarios.column.fullname".Traducir();
            dgvUsuarios.Columns["Email"].HeaderText = "abm.usuarios.column.email".Traducir();
            dgvUsuarios.Columns["Perfil"].HeaderText = "abm.usuarios.column.profile".Traducir();
            dgvUsuarios.Columns["Activo"].HeaderText = "abm.usuarios.column.active".Traducir();
            dgvUsuarios.Columns["Bloqueado"].HeaderText = "abm.usuarios.column.blocked".Traducir();
            dgvUsuarios.Columns["FechaCreacion"].HeaderText = "abm.usuarios.column.created".Traducir();
            dgvUsuarios.Columns["FechaUltimoAcceso"].HeaderText = "abm.usuarios.column.lastAccess".Traducir();
        }

        private void ApplyTexts()
        {
            Text = "abm.usuarios.title".Traducir();
            tsbNuevo.Text = "abm.usuarios.nuevo".Traducir();
            tsbEditar.Text = "abm.usuarios.editar".Traducir();
            tsbCambiarPassword.Text = "abm.usuarios.cambiar_pass".Traducir();
            tsbBloquear.Text = "abm.usuarios.bloquear".Traducir();
            tsbActivar.Text = "abm.usuarios.activar".Traducir();
            tsbActualizar.Text = "abm.usuarios.actualizar".Traducir();
            tsbMostrarBloqueados.Text = "abm.usuarios.mostrar_bloqueados".Traducir();
            tsbMostrarInactivos.Text = "abm.usuarios.mostrar_inactivos".Traducir();
            btnCerrar.Text = "form.cerrar".Traducir();
        }

        private void WireUp()
        {
            tsbNuevo.Click += (s, e) => NuevoUsuario();
            tsbEditar.Click += (s, e) => EditarSeleccionado();
            tsbCambiarPassword.Click += (s, e) => CambiarPasswordSeleccionado();
            tsbBloquear.Click += (s, e) => BloquearDesbloquearSeleccionado();
            tsbActivar.Click += (s, e) => ActivarDesactivarSeleccionado();
            tsbActualizar.Click += (s, e) => CargarUsuarios();
            tsbMostrarBloqueados.Click += (s, e) => CambiarFiltro(FiltroUsuarios.Bloqueados);
            tsbMostrarInactivos.Click += (s, e) => CambiarFiltro(FiltroUsuarios.Inactivos);
            btnCerrar.Click += (s, e) => Close();
            dgvUsuarios.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };
        }

        private void CambiarFiltro(FiltroUsuarios nuevoFiltro)
        {
            if (_filtroActual == nuevoFiltro)
            {
                // Si ya estoy en ese filtro, volver a Activos
                _filtroActual = FiltroUsuarios.Activos;
            }
            else
            {
                _filtroActual = nuevoFiltro;
            }

            ActualizarTextoBotonFiltro();
            CargarUsuarios();
        }

        private void ActualizarTextoBotonFiltro()
        {
            // Actualizar texto de los botones según el filtro actual
            switch (_filtroActual)
            {
                case FiltroUsuarios.Activos:
                    tsbMostrarBloqueados.Text = "abm.usuarios.mostrar_bloqueados".Traducir();
                    tsbMostrarInactivos.Text = "abm.usuarios.mostrar_inactivos".Traducir();
                    tsbMostrarBloqueados.BackColor = Color.Transparent;
                    tsbMostrarInactivos.BackColor = Color.Transparent;
                    break;

                case FiltroUsuarios.Bloqueados:
                    tsbMostrarBloqueados.Text = "abm.usuarios.mostrar_activos".Traducir();
                    tsbMostrarInactivos.Text = "abm.usuarios.mostrar_inactivos".Traducir();
                    tsbMostrarBloqueados.BackColor = Color.LightBlue; // Resaltar botón activo
                    tsbMostrarInactivos.BackColor = Color.Transparent;
                    break;

                case FiltroUsuarios.Inactivos:
                    tsbMostrarBloqueados.Text = "abm.usuarios.mostrar_bloqueados".Traducir();
                    tsbMostrarInactivos.Text = "abm.usuarios.mostrar_activos".Traducir();
                    tsbMostrarBloqueados.BackColor = Color.Transparent;
                    tsbMostrarInactivos.BackColor = Color.LightBlue; // Resaltar botón activo
                    break;
            }
        }
        private void CargarUsuarios()
        {
            try
            {
                // Obtener TODOS los usuarios (incluyendo inactivos)
                var todosUsuarios = _usuarioService.ObtenerTodosLosUsuarios().ToList();
                IEnumerable<Usuario> usuarios;

                switch (_filtroActual)
                {
                    case FiltroUsuarios.Bloqueados:
                        // Solo usuarios bloqueados (pueden estar activos o inactivos)
                        usuarios = todosUsuarios.Where(u => u.Bloqueado);
                        break;

                    case FiltroUsuarios.Inactivos:
                        // Solo usuarios inactivos (no bloqueados)
                        usuarios = todosUsuarios.Where(u => !u.Activo);
                        break;

                    default: // Activos
                             // Usuarios activos y no bloqueados
                        usuarios = todosUsuarios.Where(u => u.Activo && !u.Bloqueado);
                        break;
                }

                var lista = usuarios.ToList();

                var rows = lista.Select(u => new UsuarioGridRow
                {
                    IdUsuario = u.IdUsuario,
                    NombreUsuario = u.NombreUsuario,
                    NombreCompleto = u.NombreCompleto ?? "",
                    Email = u.Email ?? "",
                    Perfil = u.Perfil?.NombrePerfil ?? "",
                    Activo = u.Activo,
                    Bloqueado = u.Bloqueado,
                    FechaCreacion = u.FechaCreacion,
                    FechaUltimoAcceso = u.FechaUltimoAcceso,
                    IntentosLoginFallidos = u.IntentosLoginFallidos
                }).OrderBy(u => u.NombreUsuario).ToList();

                bsUsuarios.DataSource = rows;

                // Colorear filas según estado
                dgvUsuarios.CellFormatting -= DgvUsuarios_CellFormatting; // Remover handler previo
                dgvUsuarios.CellFormatting += DgvUsuarios_CellFormatting; // Agregar handler

                var filtroTexto = _filtroActual == FiltroUsuarios.Activos ? "activos" :
                                  _filtroActual == FiltroUsuarios.Bloqueados ? "bloqueados" : "inactivos";

                _logService.LogInfo($"Cargados {lista.Count} usuarios {filtroTexto} (total en DB: {todosUsuarios.Count})",
                    "Usuarios", SessionContext.NombreUsuario);
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cargando usuarios", ex, "Usuarios", SessionContext.NombreUsuario);
                MessageBox.Show("abm.usuarios.error.load".Traducir(), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvUsuarios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                var row = dgvUsuarios.Rows[e.RowIndex];
                var bloqueado = (bool)row.Cells["Bloqueado"].Value;
                var activo = (bool)row.Cells["Activo"].Value;

                if (bloqueado)
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                else if (!activo)
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                else
                    row.DefaultCellStyle.BackColor = Color.White; // Por si se cambia de filtro
            }
            catch
            {
                // Ignorar errores de formateo
            }
        }

        private UsuarioGridRow GetSeleccionado()
        {
            return bsUsuarios.Current as UsuarioGridRow;
        }

        private void NuevoUsuario()
        {
            try
            {
                using (var f = new UsuarioForm(_usuarioService, _perfilService, null))
                {
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Crear",
                            "action.crear".Traducir(), "Usuarios", true);
                        CargarUsuarios();
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error abriendo formulario de nuevo usuario", ex, "Usuarios", SessionContext.NombreUsuario);
                MessageBox.Show("abm.usuarios.error.save".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditarSeleccionado()
        {
            try
            {
                var row = GetSeleccionado();
                if (row == null) return;

                var usuario = _usuarioService.ObtenerPorId(row.IdUsuario);
                if (usuario == null) return;

                using (var f = new UsuarioForm(_usuarioService, _perfilService, usuario))
                {
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Editar",
                            $"{"action.editar".Traducir()}: {usuario.NombreUsuario}", "Usuarios", true);
                        CargarUsuarios();
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error editando usuario", ex, "Usuarios", SessionContext.NombreUsuario);
                MessageBox.Show("abm.usuarios.error.save".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CambiarPasswordSeleccionado()
        {
            try
            {
                var row = GetSeleccionado();
                if (row == null) return;

                var resultado = Microsoft.VisualBasic.Interaction.InputBox(
                    "usuario.password_new".Traducir(), "abm.usuarios.cambiar_pass".Traducir(), "");

                if (string.IsNullOrEmpty(resultado)) return;

                var res = _usuarioService.CambiarPassword(row.IdUsuario, resultado);

                if (res.EsValido)
                {
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Cambiar Contraseña",
                        $"{"abm.usuarios.log.passwordChanged".Traducir(row.NombreUsuario)}", "Usuarios", true);
                    MessageBox.Show("abm.usuarios.log.passwordChanged".Traducir(row.NombreUsuario), Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Cambiar Contraseña",
                        res.Mensaje, "Usuarios", false);
                    MessageBox.Show(res.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error cambiando contraseña", ex, "Usuarios", SessionContext.NombreUsuario);
                MessageBox.Show("abm.usuarios.error.save".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BloquearDesbloquearSeleccionado()
        {
            try
            {
                var row = GetSeleccionado();
                if (row == null) return;

                var accion = row.Bloqueado ? "desbloquear" : "bloquear";
                var accionPasado = row.Bloqueado ? "desbloqueado" : "bloqueado";
                var msgKey = row.Bloqueado ? "msg.confirm.desbloquear" : "msg.confirm.bloquear";

                if (MessageBox.Show(msgKey.Traducir(row.NombreUsuario),
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                var res = row.Bloqueado ?
                    _usuarioService.DesbloquearUsuario(row.IdUsuario) :
                    _usuarioService.BloquearUsuario(row.IdUsuario);

                if (res.EsValido)
                {
                    var logKey = row.Bloqueado ? "abm.usuarios.log.unblocked" : "abm.usuarios.log.blocked";
                    var accionKey = row.Bloqueado ? "Desbloquear" : "Bloquear";
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario,
                        accionKey,
                        logKey.Traducir(row.NombreUsuario), "Usuarios", true);
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show(res.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error bloqueando/desbloqueando usuario", ex, "Usuarios", SessionContext.NombreUsuario);
                MessageBox.Show("abm.usuarios.error.save".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActivarDesactivarSeleccionado()
        {
            try
            {
                var row = GetSeleccionado();
                if (row == null) return;

                if (row.IdUsuario == SessionContext.IdUsuario && row.Activo)
                {
                    MessageBox.Show("abm.usuarios.error.save".Traducir(), Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var msgKey = row.Activo ? "msg.confirm.desactivar" : "msg.confirm.activar";

                if (MessageBox.Show(msgKey.Traducir(row.NombreUsuario),
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                var res = row.Activo ?
                    _usuarioService.DesactivarUsuario(row.IdUsuario) :
                    _usuarioService.ActivarUsuario(row.IdUsuario);

                if (res.EsValido)
                {
                    var logKey = row.Activo ? "abm.usuarios.log.deactivated" : "abm.usuarios.log.activated";
                    var accionKey = row.Activo ? "Desactivar" : "Activar";
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario,
                        accionKey,
                        logKey.Traducir(row.NombreUsuario), "Usuarios", true);
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show(res.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error activando/desactivando usuario", ex, "Usuarios", SessionContext.NombreUsuario);
                MessageBox.Show("abm.usuarios.error.save".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
