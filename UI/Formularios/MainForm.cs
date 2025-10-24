using BLL.Factories;
using Services.BLL.Factories;
using System;
using System.Drawing;
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
        private ToolStripMenuItem mnuPedidos;

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

        private Panel pnlContent;
        private Form _formularioActual;

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
            mnuPedidos.Click += (s, e) => AbrirPedidos();

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
                var logSvc = ServicesFactory.CrearLogService();
                var nombreUsuarioActual = SessionContext.NombreUsuario;

                logSvc.LogInfo($"Cerrando sesión de usuario: {nombreUsuarioActual}", "Seguridad", nombreUsuarioActual);

                // Registrar logout en bitácora
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
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
                var logSvc = ServicesFactory.CrearLogService();
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
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
                var geoSvc = ServiceFactory.CrearGeoService();
                var condicionIvaSvc = ServiceFactory.CrearCondicionIvaService();
                var logSvc = ServicesFactory.CrearLogService();

                logSvc.LogInfo("Abriendo formulario ABM Clientes", "Clientes", SessionContext.NombreUsuario);

                var f = new ABMClientesForm(clienteSvc, bitacoraSvc, geoSvc, condicionIvaSvc);
                f.FormClosed += (s, e) =>
                {
                    logSvc.LogInfo("Cerrado formulario ABM Clientes", "Clientes", SessionContext.NombreUsuario);
                };

                MostrarFormulario(f);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error abriendo ABM Clientes", ex, "Clientes", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirProveedores()
        {
            try
            {
                var proveedorSvc = ServiceFactory.CrearProveedorService();
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
                var geoSvc = ServiceFactory.CrearGeoService();
                var condicionIvaSvc = ServiceFactory.CrearCondicionIvaService();
                var logSvc = ServicesFactory.CrearLogService();

                logSvc.LogInfo("Abriendo formulario ABM Proveedores / Opening suppliers form", "Proveedores", SessionContext.NombreUsuario);

                var f = new ABMProveedoresForm(proveedorSvc, bitacoraSvc, geoSvc, condicionIvaSvc);
                f.FormClosed += (s, e) =>
                {
                    logSvc.LogInfo("Cerrado formulario ABM Proveedores / Suppliers form closed", "Proveedores", SessionContext.NombreUsuario);
                };

                MostrarFormulario(f);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error abriendo ABM Proveedores / Error opening suppliers form", ex, "Proveedores", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirProductos()
        {
            try
            {
                var productoSvc = ServiceFactory.CrearProductoService();
                var categoriaSvc = ServiceFactory.CrearCategoriaProductoService();
                var proveedorSvc = ServiceFactory.CrearProveedorService();
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
                var logSvc = ServicesFactory.CrearLogService();

                var f = new ABMProductosForm(productoSvc, categoriaSvc, proveedorSvc, bitacoraSvc);
                f.FormClosed += (s, e) =>
                {
                    logSvc.LogInfo("Cerrado formulario ABM Productos / Products form closed", "Productos", SessionContext.NombreUsuario);
                };

                MostrarFormulario(f);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error abriendo ABM Productos / Error opening products form", ex, "Productos", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirPedidos()
        {
            try
            {
                var pedidoSvc = ServiceFactory.CrearPedidoService();
                var clienteSvc = ServiceFactory.CrearClienteService();
                var productoSvc = ServiceFactory.CrearProductoService();
                var categoriaSvc = ServiceFactory.CrearCategoriaProductoService();
                var proveedorSvc = ServiceFactory.CrearProveedorService();
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
                var logSvc = ServicesFactory.CrearLogService();

                logSvc.LogInfo("Abriendo gestor de pedidos / Opening orders module", "Pedidos", SessionContext.NombreUsuario);

                var f = new PedidosForm(pedidoSvc, clienteSvc, productoSvc, categoriaSvc, proveedorSvc, bitacoraSvc, logSvc);
                f.FormClosed += (s, e) =>
                {
                    logSvc.LogInfo("Cerrado gestor de pedidos / Orders module closed", "Pedidos", SessionContext.NombreUsuario);
                };

                MostrarFormulario(f);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error abriendo gestor de pedidos / Error opening orders module", ex, "Pedidos", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            mnuPedidos.Text = "menu.orders".Traducir();

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
                SessionContext.ActualizarIdioma(ci.Name);

                // Guardar preferencia del usuario en la BD
                if (SessionContext.IsAuthenticated)
                {
                    var usuarioSvc = ServicesFactory.CrearUsuarioService();
                    var resultado = usuarioSvc.CambiarIdioma(SessionContext.IdUsuario, cultureName);

                    if (resultado.EsValido)
                    {
                        var logSvc = ServicesFactory.CrearLogService();
                        logSvc.LogInfo($"Idioma guardado en BD: {cultureName} para usuario {SessionContext.NombreUsuario}",
                            "Sistema", SessionContext.NombreUsuario);

                        MessageBox.Show($"Idioma actualizado a {cultureName}.",
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
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error cambiando idioma", ex, "Sistema", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarFormulario(Form formulario)
        {
            if (formulario == null)
                return;

            if (_formularioActual != null)
            {
                _formularioActual.FormClosed -= FormularioActual_FormClosed;
                _formularioActual.Close();
                _formularioActual.Dispose();
                _formularioActual = null;
            }

            pnlContent?.Controls.Clear();

            _formularioActual = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.ShowIcon = false;
            formulario.ShowInTaskbar = false;
            formulario.FormClosed += FormularioActual_FormClosed;

            pnlContent?.Controls.Add(formulario);
            formulario.BringToFront();
            formulario.Show();
        }

        private void FormularioActual_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender == _formularioActual)
            {
                _formularioActual.FormClosed -= FormularioActual_FormClosed;
                _formularioActual = null;
                pnlContent?.Controls.Clear();
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
            this.mnuPedidos = new System.Windows.Forms.ToolStripMenuItem();

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
            this.pnlContent = new System.Windows.Forms.Panel();

            // MenuStrip
            this.menuStrip1.Items.AddRange(new ToolStripItem[] {
                this.mnuArchivo,
                this.mnuCatalogos,
                this.mnuSeguridad,
                this.mnuIdioma
            });
            this.menuStrip1.Dock = DockStyle.Top;
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
                this.mnuProductos,
                this.mnuPedidos
            });
            this.mnuCatalogos.Name = "mnuCatalogos";

            this.mnuClientes.Name = "mnuClientes";
            this.mnuProveedores.Name = "mnuProveedores";
            this.mnuProductos.Name = "mnuProductos";
            this.mnuPedidos.Name = "mnuPedidos";

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
            this.statusStrip1.Dock = DockStyle.Bottom;
            this.statusStrip1.Name = "statusStrip1";

            this.stsUsuario.Name = "stsUsuario";
            this.stsUsuario.Text = "Usuario: -";
            this.stsPerfil.Name = "stsPerfil";
            this.stsPerfil.Text = "Perfil: -";

            // pnlContent
            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 24);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(900, 404);
            this.pnlContent.BackColor = System.Drawing.SystemColors.Window;
            this.pnlContent.TabIndex = 2;

            // MainForm
            this.ClientSize = new System.Drawing.Size(900, 450);
            this.Controls.Add(this.pnlContent);
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
                var usuarioSvc = ServicesFactory.CrearUsuarioService();
                var perfilSvc = ServicesFactory.CrearPerfilService();
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
                var logSvc = ServicesFactory.CrearLogService();

                var f = new ABMUsuariosForm(usuarioSvc, perfilSvc, bitacoraSvc, logSvc);
                f.FormClosed += (s, e) =>
                {
                    logSvc.LogInfo("Cerrado formulario ABM Usuarios", "Seguridad", SessionContext.NombreUsuario);
                };

                MostrarFormulario(f);
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
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
                var logSvc = ServicesFactory.CrearLogService();

                var f = new LogsBitacoraForm(bitacoraSvc, logSvc);
                f.FormClosed += (s, e) =>
                {
                    logSvc.LogInfo("Cerrado formulario Logs y Bitácora", "Seguridad", SessionContext.NombreUsuario);
                };

                MostrarFormulario(f);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error abriendo logs: {ex.Message}", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}