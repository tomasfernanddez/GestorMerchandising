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
        private static string _culturaPorDefecto;

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cultura inicial por defecto
            _culturaPorDefecto = ConfigurationManager.AppSettings["UICulture"];
            if (string.IsNullOrWhiteSpace(_culturaPorDefecto))
                _culturaPorDefecto = "es-AR";

            // Configuración de servicios
            var cs = ConfigurationManager.ConnectionStrings["GestorMerchandisingDB"]?.ConnectionString;
            ServiceFactory.ConfigurarConnectionString(cs);

            var logSvc = ServicesFactory.CrearLogService();
            logSvc.LogInfo("=== Aplicación iniciada ===", "Sistema", null);

            // Loop de autenticación
            while (true)
            {
                // Restaurar idioma por defecto antes del login
                AplicarCulturaPorDefecto(logSvc);

                IAutenticacionService auth = ServicesFactory.CrearAutenticacionService();

                using (var loginForm = new LoginForm(auth))
                {
                    var dialogResult = loginForm.ShowDialog();
                    if (dialogResult != DialogResult.OK)
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

                    // Cargar idioma del usuario o aplicar fallback si falla
                    CargarIdiomaUsuario(logSvc);

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

        private static void CargarIdiomaUsuario(ILogService logSvc)
        {
            if (!SessionContext.IsAuthenticated)
            {
                AplicarCulturaPorDefecto(logSvc);
                return;
            }

            string idioma = SessionContext.IdiomaPreferido;

            try
            {
                if (string.IsNullOrWhiteSpace(idioma))
                {
                    var usuarioSvc = ServicesFactory.CrearUsuarioService();
                    var usuario = usuarioSvc.ObtenerPorId(SessionContext.IdUsuario);

                    if (usuario == null)
                    {
                        logSvc.LogWarning("No se encontró el usuario autenticado para cargar el idioma. Se utilizará el idioma por defecto.",
                            "Sistema", SessionContext.NombreUsuario);
                        AplicarCulturaPorDefecto(logSvc);
                        return;
                    }

                    idioma = usuario.IdiomaPreferido;
                    SessionContext.ActualizarIdioma(idioma);
                }

                if (string.IsNullOrWhiteSpace(idioma))
                {
                    logSvc.LogInfo($"Usuario {SessionContext.NombreUsuario} sin idioma preferido. Se utilizará la cultura por defecto {_culturaPorDefecto}.",
                        "Sistema", SessionContext.NombreUsuario);
                    AplicarCulturaPorDefecto(logSvc);
                    return;
                }

                if (TryAplicarCultura(idioma, logSvc, out var culturaAplicada))
                {
                    SessionContext.ActualizarIdioma(culturaAplicada);
                    logSvc.LogInfo($"Idioma cargado: {culturaAplicada} para usuario {SessionContext.NombreUsuario}",
                        "Sistema", SessionContext.NombreUsuario);
                    return;
                }

                logSvc.LogWarning($"No se pudo aplicar el idioma preferido {idioma} del usuario {SessionContext.NombreUsuario}. Se utilizará la cultura por defecto {_culturaPorDefecto}.",
                    "Sistema", SessionContext.NombreUsuario);
            }
            catch (Exception ex)
            {
                logSvc.LogWarning($"Error cargando idioma del usuario: {ex.Message}. Se utilizará la cultura por defecto.",
                    "Sistema", SessionContext.NombreUsuario);
            }

            AplicarCulturaPorDefecto(logSvc);
        }

        private static void AplicarCulturaPorDefecto(ILogService logSvc)
        {
            if (TryAplicarCultura(_culturaPorDefecto, logSvc, out var culturaAplicada))
            {
                SessionContext.ActualizarIdioma(culturaAplicada);
                return;
            }

            try
            {
                var culturaInvariante = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = culturaInvariante;
                Thread.CurrentThread.CurrentCulture = culturaInvariante;
                CultureInfo.DefaultThreadCurrentUICulture = culturaInvariante;
                CultureInfo.DefaultThreadCurrentCulture = culturaInvariante;
                Localization.Localization.Load("es-AR");
                SessionContext.ActualizarIdioma(null);
            }
            catch (Exception ex)
            {
                logSvc.LogError("Error aplicando cultura por defecto", ex, "Sistema", SessionContext.NombreUsuario);
            }
        }

        private static bool TryAplicarCultura(string cultureName, ILogService logSvc, out string culturaAplicada)
        {
            culturaAplicada = null;

            var culturaObjetivo = string.IsNullOrWhiteSpace(cultureName) ? _culturaPorDefecto : cultureName;

            try
            {
                var ci = new CultureInfo(culturaObjetivo);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = ci;
                CultureInfo.DefaultThreadCurrentUICulture = ci;
                CultureInfo.DefaultThreadCurrentCulture = ci;
                Localization.Localization.Load(ci.Name);
                culturaAplicada = ci.Name;
                return true;
            }
            catch (Exception ex) when (ex is CultureNotFoundException || ex is ArgumentException)
            {
                logSvc.LogWarning($"No se pudo aplicar la cultura '{culturaObjetivo}': {ex.Message}", "Sistema", SessionContext.NombreUsuario);
                return false;
            }
        }
    }
}
