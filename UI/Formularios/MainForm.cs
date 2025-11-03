using BLL.Factories;
using Services;
using Services.BLL.Factories;
using Services.BLL.Services;
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
        private MenuStrip menuStrip1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel stsUsuario;
        private ToolStripStatusLabel stsPerfil;
        private Panel pnlContent;
        private Form _formularioActual;
        private readonly MenuService _menuService;

        public MainForm()
        {
            InitializeComponent();
            _menuService = new MenuService();
            RefrescarInterfaz();
        }

        private void RefrescarInterfaz()
        {
            Text = "main.title".Traducir();
            ConstruirMenuDinamico();
            UpdateStatus();
        }

        private void ConstruirMenuDinamico()
        {
            if (menuStrip1 == null)
            {
                return;
            }

            menuStrip1.Items.Clear();

            var menuCompleto = _menuService.ObtenerEstructuraMenu();
            var menuFiltrado = _menuService.FiltrarMenuPorPermisos(menuCompleto);

            foreach (var itemConfig in menuFiltrado)
            {
                var menuItem = CrearMenuItem(itemConfig);
                if (menuItem != null)
                {
                    menuStrip1.Items.Add(menuItem);
                }
            }
        }

        private ToolStripMenuItem CrearMenuItem(MenuItemConfig config)
        {
            if (config == null)
            {
                return null;
            }

            var textoTraducido = (config.Texto ?? string.Empty).Traducir();
            var menuItem = new ToolStripMenuItem(textoTraducido)
            {
                Tag = config
            };

            if (config.SubItems != null && config.SubItems.Count > 0)
            {
                foreach (var subItemConfig in config.SubItems)
                {
                    var subItem = CrearMenuItem(subItemConfig);
                    if (subItem != null)
                    {
                        menuItem.DropDownItems.Add(subItem);
                    }
                }
            }
            else
            {
                var handler = ObtenerManejadorEvento(config.Id);
                if (handler != null)
                {
                    menuItem.Click += handler;
                }
            }

            return menuItem;
        }

        private EventHandler ObtenerManejadorEvento(string identificador)
        {
            switch (identificador)
            {
                case "menu.file.logout":
                    return (s, e) => CerrarSesion();
                case "menu.file.exit":
                    return (s, e) => SalirAplicacion();
                case "menu.catalogs.clients":
                    return (s, e) => AbrirClientes();
                case "menu.catalogs.providers":
                    return (s, e) => AbrirProveedores();
                case "menu.catalogs.products":
                    return (s, e) => AbrirProductos();
                case "menu.orders.new":
                    return (s, e) => AbrirNuevoPedido();
                case "menu.orders.list":
                    return (s, e) => AbrirPedidos();
                case "menu.orders.samples.new":
                    return (s, e) => AbrirNuevoPedidoMuestra();
                case "menu.orders.samples":
                    return (s, e) => AbrirPedidosMuestra();
                case "menu.security.users":
                    return (s, e) => AbrirGestionUsuarios();
                case "menu.security.profiles":
                    return (s, e) => AbrirGestionPerfiles();
                case "menu.security.bitacora":
                    return (s, e) => AbrirLogsBitacora();
                case "menu.reports":
                    return (s, e) => AbrirReportes();
                case "menu.language.es":
                    return (s, e) => CambiarIdioma("es-AR");
                case "menu.language.en":
                    return (s, e) => CambiarIdioma("en-US");
                default:
                    return null;
            }
        }

        private void SalirAplicacion()
        {
            Close();
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
                SessionContext.CerrarSesion();

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

        private void AbrirNuevoPedido()
        {
            var esAdmin = SessionContext.EsAdministrador;
            if (!esAdmin && !SessionContext.TieneFuncion("PEDIDOS_VENTAS"))
            {
                MessageBox.Show("No tiene permisos para crear pedidos.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var pedidoSvc = ServiceFactory.CrearPedidoService();
                var clienteSvc = ServiceFactory.CrearClienteService();
                var productoSvc = ServiceFactory.CrearProductoService();
                var categoriaSvc = ServiceFactory.CrearCategoriaProductoService();
                var proveedorSvc = ServiceFactory.CrearProveedorService();
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
                var logSvc = ServicesFactory.CrearLogService();

                logSvc.LogInfo("Iniciando creación de nuevo pedido", "Pedidos", SessionContext.NombreUsuario);

                var formulario = new PedidoForm(pedidoSvc, clienteSvc, productoSvc, categoriaSvc, proveedorSvc, bitacoraSvc, logSvc);
                formulario.FormClosed += (s, e) =>
                {
                    logSvc.LogInfo("Formulario de nuevo pedido cerrado", "Pedidos", SessionContext.NombreUsuario);
                };

                MostrarFormulario(formulario);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error creando un nuevo pedido", ex, "Pedidos", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirNuevoPedidoMuestra()
        {
            var esAdmin = SessionContext.EsAdministrador;
            if (!esAdmin && !SessionContext.TieneFuncion("PEDIDOS_MUESTRAS"))
            {
                MessageBox.Show("No tiene permisos para crear pedidos de muestra.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var pedidoMuestraSvc = ServiceFactory.CrearPedidoMuestraService();
                var clienteSvc = ServiceFactory.CrearClienteService();
                var productoSvc = ServiceFactory.CrearProductoService();
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
                var logSvc = ServicesFactory.CrearLogService();

                logSvc.LogInfo("Iniciando creación de nuevo pedido de muestra / Starting new sample order creation", "PedidosMuestra", SessionContext.NombreUsuario);

                var formulario = new PedidoMuestraForm(pedidoMuestraSvc, clienteSvc, productoSvc, bitacoraSvc, logSvc);
                formulario.FormClosed += (s, e) =>
                {
                    logSvc.LogInfo("Formulario de nuevo pedido de muestra cerrado / New sample order form closed", "PedidosMuestra", SessionContext.NombreUsuario);
                };

                MostrarFormulario(formulario);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error creando un nuevo pedido de muestra / Error creating new sample order", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirPedidosMuestra()
        {
            try
            {
                var pedidoMuestraSvc = ServiceFactory.CrearPedidoMuestraService();
                var clienteSvc = ServiceFactory.CrearClienteService();
                var productoSvc = ServiceFactory.CrearProductoService();
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
                var logSvc = ServicesFactory.CrearLogService();

                logSvc.LogInfo("Abriendo pedidos de muestra / Opening sample orders", "PedidosMuestra", SessionContext.NombreUsuario);

                var f = new PedidosMuestraForm(pedidoMuestraSvc, clienteSvc, productoSvc, bitacoraSvc, logSvc);
                MostrarFormulario(f);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error abriendo pedidos de muestra / Error opening sample orders", ex, "PedidosMuestra", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirReportes()
        {
            var esAdmin = SessionContext.EsAdministrador;
            var tienePermiso = esAdmin
                || SessionContext.TieneFuncion("REPORTES_OPERATIVOS")
                || SessionContext.TieneFuncion("REPORTES_VENTAS")
                || SessionContext.TieneFuncion("REPORTES_FINANCIEROS");

            if (!tienePermiso)
            {
                MessageBox.Show("report.permission.denied".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var reporteSvc = ServiceFactory.CrearReporteService();
                var logSvc = ServicesFactory.CrearLogService();

                logSvc.LogInfo("Abriendo módulo de reportes", "Reportes", SessionContext.NombreUsuario);

                var form = new ReportesForm(reporteSvc);
                form.FormClosed += (s, e) =>
                {
                    logSvc.LogInfo("Cerrado módulo de reportes", "Reportes", SessionContext.NombreUsuario);
                };

                MostrarFormulario(form);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error abriendo módulo de reportes", ex, "Reportes", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                RefrescarInterfaz();
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
            this.menuStrip1 = new MenuStrip();
            this.statusStrip1 = new StatusStrip();
            this.stsUsuario = new ToolStripStatusLabel();
            this.stsPerfil = new ToolStripStatusLabel();
            this.pnlContent = new Panel();

            // menuStrip1
            this.menuStrip1.Dock = DockStyle.Top;
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(900, 24);

            // statusStrip1
            this.statusStrip1.Dock = DockStyle.Bottom;
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.stsUsuario, this.stsPerfil });
            this.statusStrip1.Location = new Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(900, 22);

            // stsUsuario
            this.stsUsuario.Name = "stsUsuario";
            this.stsUsuario.Text = "Usuario: -";

            // stsPerfil
            this.stsPerfil.Name = "stsPerfil";
            this.stsPerfil.Text = "Perfil: -";

            // pnlContent
            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.Location = new Point(0, 24);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new Size(900, 404);
            this.pnlContent.BackColor = SystemColors.Window;

            // MainForm
            this.ClientSize = new Size(900, 450);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "SIGPM";
        }

        private void AbrirGestionPerfiles()
        {
            var esAdmin = SessionContext.EsAdministrador;
            var puedePerfiles = esAdmin || SessionContext.TieneFuncion("SEG_PERFILES");

            if (!puedePerfiles)
            {
                MessageBox.Show("profile.permission.denied".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var perfilSvc = ServicesFactory.CrearPerfilService();
                var bitacoraSvc = ServicesFactory.CrearBitacoraService();
                var logSvc = ServicesFactory.CrearLogService();

                var formulario = new ABMPerfilesForm(perfilSvc, bitacoraSvc, logSvc);
                formulario.FormClosed += (s, e) =>
                {
                    logSvc.LogInfo("Cerrado formulario ABM Perfiles", "Seguridad", SessionContext.NombreUsuario);
                };

                MostrarFormulario(formulario);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error abriendo gestión de perfiles", ex, "Seguridad", SessionContext.NombreUsuario);
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirGestionUsuarios()
        {
            // Solo administradores pueden acceder
            var esAdmin = SessionContext.EsAdministrador;
            var puedeUsuarios = esAdmin || SessionContext.TieneFuncion("SEG_USUARIOS");

            if (!puedeUsuarios)
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
            var esAdmin = SessionContext.EsAdministrador;

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