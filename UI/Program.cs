using BLL.Factories;
using Services.BLL.Interfaces;
using Services.BLL.Factories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Localization;

namespace UI
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cultura inicial por defecto
            var cultureDefault = ConfigurationManager.AppSettings["UICulture"];
            if (string.IsNullOrWhiteSpace(cultureDefault)) cultureDefault = "es-AR";

            // Configuración de servicios
            var cs = ConfigurationManager.ConnectionStrings["GestorMerchandisingDB"]?.ConnectionString;
            ServiceFactory.ConfigurarConnectionString(cs);

            var logSvc = ServicesFactory.CrearLogService();
            logSvc.LogInfo("=== Aplicación iniciada ===", "Sistema", null);

            // Loop de autenticación
            while (true)
            {
                // Restaurar idioma por defecto antes del login
                var ci = new CultureInfo(cultureDefault);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = ci;
                Localization.Localization.Load(ci.Name);

                IAutenticacionService auth = ServicesFactory.CrearAutenticacionService();

                using (var loginForm = new LoginForm(auth))
                {
                    if (loginForm.ShowDialog() != DialogResult.OK)
                    {
                        // Usuario canceló el login - salir de la aplicación
                        break;
                    }

                    if (!SessionContext.IsAuthenticated)
                    {
                        // Login falló - volver a mostrar login
                        continue;
                    }

                    logSvc.LogInfo($"Login exitoso para usuario: {SessionContext.NombreUsuario}",
                        "Seguridad", SessionContext.NombreUsuario);

                    // Cargar idioma del usuario
                    CargarIdiomaUsuario();

                    using (var mainForm = new MainForm())
                    {
                        var resultado = mainForm.ShowDialog();

                        if (resultado == DialogResult.OK)
                        {
                            // Cerrar sesión - volver al login
                            logSvc.LogInfo($"Cerrando sesión de {SessionContext.NombreUsuario}",
                                "Seguridad", SessionContext.NombreUsuario);
                            SessionContext.Clear();
                            continue; // Volver al inicio del while (login)
                        }
                        else
                        {
                            // Usuario cerró la ventana principal - salir
                            break;
                        }
                    }
                }
            }

            logSvc.LogInfo("=== Aplicación finalizada ===", "Sistema", null);
        }

        private static void CargarIdiomaUsuario()
        {
            try
            {
                if (!SessionContext.IsAuthenticated)
                    return;

                var usuarioSvc = ServicesFactory.CrearUsuarioService();
                var usuario = usuarioSvc.ObtenerPorId(SessionContext.IdUsuario);

                if (usuario != null && !string.IsNullOrWhiteSpace(usuario.IdiomaPreferido))
                {
                    var ci = new CultureInfo(usuario.IdiomaPreferido);
                    Thread.CurrentThread.CurrentUICulture = ci;
                    Thread.CurrentThread.CurrentCulture = ci;
                    Localization.Localization.Load(ci.Name);

                    var logSvc = ServicesFactory.CrearLogService();
                    logSvc.LogInfo($"Idioma cargado: {usuario.IdiomaPreferido} para usuario {usuario.NombreUsuario}",
                        "Sistema", SessionContext.NombreUsuario);
                }
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error cargando idioma del usuario", ex, "Sistema", SessionContext.NombreUsuario);
            }
        }
    }
}
