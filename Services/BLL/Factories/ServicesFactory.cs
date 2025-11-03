using Services.BLL.Interfaces;
using Services.BLL.Services;
using Services.DAL.Ef.Base;
using Services.DAL.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecIUnitOfWork = Services.DAL.Interfaces.Base.IUnitOfWork;
using SecEfUnitOfWork = Services.DAL.Ef.Base.EfUnitOfWork;
using SecDbContext = Services.DAL.Ef.Base.ServicesContext;


namespace Services.BLL.Factories
{
    public static class ServicesFactory
    {
        // =====================================================================
        // CONFIGURACIÓN CENTRALIZADA
        // =====================================================================
        private static string _connectionString;
        private static bool _datosBaseInicializados;
        private static readonly object _initLock = new object();

        /// <summary>
        /// Configura el connection string a usar en toda la aplicación.
        /// Se llama típicamente desde el inicio de la UI.
        /// </summary>
        public static void ConfigurarConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Obtiene el connection string con fallback automático.
        /// Prioridad: 1) Configurado por código, 2) app.config, 3) nombre por defecto
        /// </summary>
        private static string ObtenerConnectionString()
        {
            // 1. Si fue configurado explícitamente
            if (!string.IsNullOrWhiteSpace(_connectionString))
                return _connectionString;

            // 2. Intentar leer del app.config/web.config
            var cs = ConfigurationManager.ConnectionStrings["GestorMerchandisingDB"]?.ConnectionString;
            if (!string.IsNullOrWhiteSpace(cs))
                return cs;

            // 3. Nombre por defecto (busca en app.config)
            return "GestorMerchandisingDB";
        }

        // =====================================================================
        // INFRAESTRUCTURA PRIVADA
        // =====================================================================

        private static SecDbContext CrearContextoSeguridad()
        {
            var cs = ObtenerConnectionString();
            return new SecDbContext(cs);
        }
        private static SecIUnitOfWork CrearUnitOfWorkSeguridad()
        {
            var ctx = CrearContextoSeguridad();
            var uow = new SecEfUnitOfWork(ctx);
            EnsureDatosBase(uow);
            return uow;
        }

        private static void EnsureDatosBase(SecIUnitOfWork uow)
        {
            if (_datosBaseInicializados)
                return;

            lock (_initLock)
            {
                if (_datosBaseInicializados)
                    return;

                uow.InicializarSistema();
                _datosBaseInicializados = true;
            }
        }

        // =====================================================================
        // SERVICIOS DE ARQUITECTURA BASE
        // =====================================================================

        /// <summary>
        /// Crea el servicio de encriptación (sin dependencias).
        /// </summary>
        public static IEncriptacionService CrearEncriptacionService()
        {
            return new EncriptacionService();
        }

        /// <summary>
        /// Crea el servicio de autenticación.
        /// </summary>
        public static IAutenticacionService CrearAutenticacionService()
        {
            var uow = CrearUnitOfWorkSeguridad();
            var enc = CrearEncriptacionService();
            return new AutenticacionService(uow, enc);
        }

        /// <summary>
        /// Crea el servicio de bitácora.
        /// </summary>
        public static IBitacoraService CrearBitacoraService()
        {
            var uow = CrearUnitOfWorkSeguridad();
            return new BitacoraService(uow);
        }

        public sealed class ServiceScope : IDisposable
        {
            public SecDbContext Context { get; }
            public SecIUnitOfWork Uow { get; }
            public IAutenticacionService Autenticacion { get; }
            public IBitacoraService Bitacora { get; }
            public ILogService Logs { get; }
            public IUsuarioService Usuarios { get; }
            public IPerfilService Perfiles { get; }

            internal ServiceScope(SecDbContext ctx)
            {
                Context = ctx;
                Uow = new SecEfUnitOfWork(ctx);
                Autenticacion = new AutenticacionService(Uow, new EncriptacionService());
                Bitacora = new BitacoraService(Uow);
                Logs = new LogService();
                Usuarios = new UsuarioService(Uow, new EncriptacionService());
                Perfiles = new PerfilService(Uow);
            }

            public void Dispose()
            {
                (Uow as IDisposable)?.Dispose(); // si UoW dispone el Context, mejor
                Context?.Dispose();
            }
        }

        public static ServiceScope BeginScope()
        {
            return new ServiceScope(CrearContextoSeguridad());
        }

        // =====================================================================
        // INFORMACIÓN DE CONFIGURACIÓN
        // =====================================================================

        /// <summary>
        /// Obtiene información sobre el connection string configurado.
        /// </summary>
        public static string ObtenerInformacionConfiguracion()
        {
            var cs = ObtenerConnectionString();
            var preview = cs.Length > 80 ? cs.Substring(0, 80) + "..." : cs;
            return $"ConnectionString configurado: {preview}";
        }

        /// <summary>
        /// Verifica que exista una configuración válida.
        /// </summary>
        public static bool VerificarConfiguracion()
        {
            var cs = ObtenerConnectionString();
            return !string.IsNullOrWhiteSpace(cs);
        }

        internal static string GetConnectionStringInterno()
        {
            // reutiliza tu ObtenerConnectionString() privado
            var metodoPrivado = typeof(ServicesFactory)
                .GetMethod("ObtenerConnectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            return (string)metodoPrivado.Invoke(null, null);
        }

        public static ILogService CrearLogService()
        {
            return new LogService();
        }

        public static IUsuarioService CrearUsuarioService()
        {
            var uow = CrearUnitOfWorkSeguridad();
            var enc = CrearEncriptacionService();
            return new UsuarioService(uow, enc);
        }

        // También agregar IPerfilService si no existe:
        /// <summary>
        /// Crea el servicio de perfiles.
        /// </summary>
        public static IPerfilService CrearPerfilService()
        {
            var uow = CrearUnitOfWorkSeguridad();
            return new PerfilService(uow);
        }
    }
}
