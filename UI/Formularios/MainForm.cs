using BLL.Factories;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using UI.Formularios;
using UI.Localization;

namespace UI
{
    public partial class MainForm : Form
    {
        // Menú
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mnuArchivo;
        private ToolStripMenuItem mnuCerrarSesion;
        private ToolStripMenuItem mnuSalir;

        private ToolStripMenuItem mnuCatalogos;
        private ToolStripMenuItem mnuClientes;
        private ToolStripMenuItem mnuProveedores;
        private ToolStripMenuItem mnuProductos;

        private ToolStripMenuItem mnuSeguridad;
        private ToolStripMenuItem mnuUsuarios;
        private ToolStripMenuItem mnuPerfiles;
        private ToolStripMenuItem mnuLogsBitacora;

        private ToolStripMenuItem mnuIdioma;
        private ToolStripMenuItem mnuEspanol;
        private ToolStripMenuItem mnuIngles;

        // Status bar
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel stsUsuario;
        private ToolStripStatusLabel stsPerfil;

        public MainForm()
        {
            InitializeComponent();
            WireUp();
            ApplyTexts();
            ApplyPermissions();
            UpdateStatus();
        }

        private void WireUp()
        {
            // Eventos de Menú
            mnuSalir.Click += (s, e) => Close();
            mnuCerrarSesion.Click += (s, e) => CerrarSesion();

            mnuClientes.Click += (s, e) => AbrirClientes();
            mnuProveedores.Click += (s, e) => AbrirProveedores();
            mnuProductos.Click += (s, e) => AbrirProductos();

            mnuUsuarios.Click += (s, e) => AbrirGestionUsuarios();
            mnuPerfiles.Click += (s, e) => MessageBox.Show("Pendiente: ABM Perfiles.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            mnuLogsBitacora.Click += (s, e) => AbrirLogsBitacora();

            // Idioma
            mnuEspanol.Click += (s, e) => CambiarIdioma("es-AR");
            mnuIngles.Click += (s, e) => CambiarIdioma("en-US");
        }

        private void CerrarSesion()
        {
            try
            {
                var logSvc = ServiceFactory.CrearLogService();
                var nombreUsuarioActual = SessionContext.NombreUsuario;

                logSvc.LogInfo($"Cerrando sesión de usuario: {nombreUsuarioActual}", "Seguridad", nombreUsuarioActual);

                // Registrar logout en bitácora
                var bitacoraSvc = ServiceFactory.CrearBitacoraService();
                bitacoraSvc.RegistrarAccion(
                    SessionContext.IdUsuario,
                    "Logout",
                    $"Usuario {nombreUsuarioActual} cerró sesión manualmente",
                    "Seguridad",
                    true,
                    null,
                    "127.0.0.1"
                );

                // Limpiar sesión
                SessionContext.Clear();

                // Cerrar este formulario
                this.DialogResult = DialogResult.OK;
                this.Close();

                logSvc.LogInfo("Sesión cerrada exitosamente", "Seguridad", nombreUsuarioActual);
            }
            catch (Exception ex)
            {
                var logSvc = ServiceFactory.CrearLogService();
                logSvc.LogError("Error cerrando sesión", ex, "Seguridad", SessionContext.NombreUsuario);
                MessageBox.Show($"Error cerrando sesión: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirClientes()
        {
            try
            {
                var clienteSvc = ServiceFactory.CrearClienteService();
                var bitacoraSvc = ServiceFactory.CrearBitacoraService();
                var geoSvc = ServiceFactory.CrearGeoService();
                var logSvc = ServiceFactory.CrearLogService();

                logSvc.LogInfo("Abriendo formulario ABM Clientes", "Clientes", SessionContext.NombreUsuario);

                using (var f = new ABMClientesForm(clienteSvc, bitacoraSvc, geoSvc))
                    f.ShowDialog(this);

                logSvc.LogInfo("Cerrado formulario ABM Clientes", "Clientes", SessionContext.NombreUsuario);
            }
            catch (Exception ex)
            {
                var logSvc = ServiceFactory.CrearLogService();
                logSvc.LogError("Error abriendo ABM Clientes", ex, "Clientes", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirProveedores()
        {
            using (var f = new ABMProveedoresForm())
                f.ShowDialog(this);
        }

        private void AbrirProductos()
        {
            using (var f = new ABMProductosForm())
                f.ShowDialog(this);
        }

        private void ApplyTexts()
        {
            Text = "main.title".Traducir(); // SIGPM o lo que prefieras

            // Menú
            mnuArchivo.Text = "menu.file".Traducir();
            mnuCerrarSesion.Text = "menu.logout".Traducir();
            mnuSalir.Text = "menu.exit".Traducir();

            mnuCatalogos.Text = "menu.catalogs".Traducir();
            mnuClientes.Text = "menu.clients".Traducir();
            mnuProveedores.Text = "menu.suppliers".Traducir();
            mnuProductos.Text = "menu.products".Traducir();

            mnuSeguridad.Text = "menu.security".Traducir();
            mnuUsuarios.Text = "menu.users".Traducir();
            mnuPerfiles.Text = "menu.roles".Traducir();

            mnuIdioma.Text = "menu.language".Traducir();
            mnuEspanol.Text = "menu.lang.es".Traducir();
            mnuIngles.Text = "menu.lang.en".Traducir();
            mnuLogsBitacora.Text = "menu.logsbitacora".Traducir();

            // Status
            stsUsuario.Text = "status.user".Traducir(SessionContext.NombreUsuario ?? "-");
            stsPerfil.Text = "status.role".Traducir(SessionContext.NombrePerfil ?? "-");
        }

        private void ApplyPermissions()
        {
            // Mostrar Seguridad solo a Administrador (ajusta a tu lógica de perfiles)
            var esAdmin = string.Equals(SessionContext.NombrePerfil, "Administrador", StringComparison.OrdinalIgnoreCase);
            mnuSeguridad.Visible = esAdmin;
        }

        private void UpdateStatus()
        {
            stsUsuario.Text = "status.user".Traducir(SessionContext.NombreUsuario ?? "-");
            stsPerfil.Text = "status.role".Traducir(SessionContext.NombrePerfil ?? "-");
        }

        private void CambiarIdioma(string cultureName)
        {
            try
            {
                var ci = new CultureInfo(cultureName);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = ci;
                Localization.Localization.Load(ci.Name);

                // Guardar preferencia del usuario en la BD
                if (SessionContext.IsAuthenticated)
                {
                    var usuarioSvc = ServiceFactory.CrearUsuarioService();
                    var resultado = usuarioSvc.CambiarIdioma(SessionContext.IdUsuario, cultureName);

                    if (resultado.EsValido)
                    {
                        var logSvc = ServiceFactory.CrearLogService();
                        logSvc.LogInfo($"Idioma guardado en BD: {cultureName} para usuario {SessionContext.NombreUsuario}",
                            "Sistema", SessionContext.NombreUsuario);

                        MessageBox.Show($"Idioma actualizado a {cultureName}. Los cambios se aplicarán completamente al reiniciar sesión.",
                            "Idioma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error guardando idioma: {resultado.Mensaje}",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                ApplyTexts(); // Refrescar textos del formulario actual
            }
            catch (Exception ex)
            {
                var logSvc = ServiceFactory.CrearLogService();
                logSvc.LogError("Error cambiando idioma", ex, "Sistema", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // InitializeComponent: diseño simple con MenuStrip y StatusStrip
        // ============================================================
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuArchivo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCerrarSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSalir = new System.Windows.Forms.ToolStripMenuItem();

            this.mnuCatalogos = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClientes = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuProveedores = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuProductos = new System.Windows.Forms.ToolStripMenuItem();

            this.mnuSeguridad = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogsBitacora = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPerfiles = new System.Windows.Forms.ToolStripMenuItem();

            this.mnuIdioma = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEspanol = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIngles = new System.Windows.Forms.ToolStripMenuItem();

            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stsUsuario = new System.Windows.Forms.ToolStripStatusLabel();
            this.stsPerfil = new System.Windows.Forms.ToolStripStatusLabel();

            // MenuStrip
            this.menuStrip1.Items.AddRange(new ToolStripItem[] {
                this.mnuArchivo,
                this.mnuCatalogos,
                this.mnuSeguridad,
                this.mnuIdioma
            });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";

            // Archivo
            this.mnuArchivo.DropDownItems.AddRange(new ToolStripItem[] {
                this.mnuCerrarSesion,
                new ToolStripSeparator(),
                this.mnuSalir
            });
            this.mnuArchivo.Name = "mnuArchivo";

            this.mnuCerrarSesion.Name = "mnuCerrarSesion";
            this.mnuSalir.Name = "mnuSalir";

            // Catálogos
            this.mnuCatalogos.DropDownItems.AddRange(new ToolStripItem[] {
                this.mnuClientes,
                this.mnuProveedores,
                this.mnuProductos
            });
            this.mnuCatalogos.Name = "mnuCatalogos";

            this.mnuClientes.Name = "mnuClientes";
            this.mnuProveedores.Name = "mnuProveedores";
            this.mnuProductos.Name = "mnuProductos";

            // Seguridad
            this.mnuSeguridad.DropDownItems.AddRange(new ToolStripItem[] {
                this.mnuUsuarios,
                this.mnuPerfiles,
                new ToolStripSeparator(),  // NUEVO separador
                this.mnuLogsBitacora       // NUEVO menú
            });
            this.mnuSeguridad.Name = "mnuSeguridad";

            this.mnuLogsBitacora.Name = "mnuLogsBitacora";
            this.mnuLogsBitacora.Text = "Logs y Bitácora";
            this.mnuUsuarios.Name = "mnuUsuarios";
            this.mnuPerfiles.Name = "mnuPerfiles";

            // Idioma
            this.mnuIdioma.DropDownItems.AddRange(new ToolStripItem[] {
                this.mnuEspanol,
                this.mnuIngles
            });
            this.mnuIdioma.Name = "mnuIdioma";

            this.mnuEspanol.Name = "mnuEspanol";
            this.mnuIngles.Name = "mnuIngles";

            // StatusStrip
            this.statusStrip1.Items.AddRange(new ToolStripItem[] {
                this.stsUsuario,
                this.stsPerfil
            });
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";

            this.stsUsuario.Name = "stsUsuario";
            this.stsUsuario.Text = "Usuario: -";
            this.stsPerfil.Name = "stsPerfil";
            this.stsPerfil.Text = "Perfil: -";

            // MainForm
            this.ClientSize = new System.Drawing.Size(900, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "SIGPM";
        }
        private void AbrirGestionUsuarios()
        {
            // Solo administradores pueden acceder
            var esAdmin = string.Equals(SessionContext.NombrePerfil, "Administrador", StringComparison.OrdinalIgnoreCase);

            if (!esAdmin)
            {
                MessageBox.Show("No tiene permisos para gestionar usuarios.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var usuarioSvc = ServiceFactory.CrearUsuarioService();
                var perfilSvc = ServiceFactory.CrearPerfilService();
                var bitacoraSvc = ServiceFactory.CrearBitacoraService();
                var logSvc = ServiceFactory.CrearLogService();

                using (var f = new ABMUsuariosForm(usuarioSvc, perfilSvc, bitacoraSvc, logSvc))
                    f.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error abriendo gestión de usuarios: {ex.Message}", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // NUEVO MÉTODO - Abrir logs y bitácora
        private void AbrirLogsBitacora()
        {
            // Solo administradores pueden ver logs completos
            var esAdmin = string.Equals(SessionContext.NombrePerfil, "Administrador", StringComparison.OrdinalIgnoreCase);

            if (!esAdmin)
            {
                MessageBox.Show("No tiene permisos para ver los logs del sistema.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var bitacoraSvc = ServiceFactory.CrearBitacoraService();
                var logSvc = ServiceFactory.CrearLogService();

                using (var f = new LogsBitacoraForm(bitacoraSvc, logSvc))
                    f.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error abriendo logs: {ex.Message}", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}