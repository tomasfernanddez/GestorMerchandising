using BLL.Interfaces;
using BLL.Services;
using DAL;
using DAL.Implementations.Base;
using DAL.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Factories
{
    public static class ServiceFactory
    {
        // =====================================================================
        // CONFIGURACIÓN CENTRALIZADA
        // =====================================================================
        private static string _connectionString;

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
        private static GestorMerchandisingContext CrearContexto()
        {
            var cs = ObtenerConnectionString();
            return new GestorMerchandisingContext(cs);
        }

        private static IUnitOfWork CrearUnitOfWork()
        {
            var context = CrearContexto();
            return new EfUnitOfWork(context);
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
            var uow = CrearUnitOfWork();
            var enc = CrearEncriptacionService();
            return new AutenticacionService(uow, enc);
        }

        /// <summary>
        /// Crea el servicio de bitácora.
        /// </summary>
        public static IBitacoraService CrearBitacoraService()
        {
            var uow = CrearUnitOfWork();
            return new BitacoraService(uow);
        }

        // =====================================================================
        // SERVICIOS PRINCIPALES DE NEGOCIO
        // =====================================================================

        /// <summary>
        /// Crea el servicio de clientes.
        /// </summary>
        public static IClienteService CrearClienteService()
        {
            var uow = CrearUnitOfWork();
            return new ClienteService(uow);
        }

        // TODO: Agregar cuando se implementen
        /*
        public static IProveedorService CrearProveedorService()
        {
            var uow = CrearUnitOfWork();
            return new ProveedorService(uow);
        }

        public static IProductoService CrearProductoService()
        {
            var uow = CrearUnitOfWork();
            return new ProductoService(uow);
        }

        public static IPedidoService CrearPedidoService()
        {
            var uow = CrearUnitOfWork();
            return new PedidoService(uow);
        }
        */
        public sealed class ServiceScope : IDisposable
        {
            public GestorMerchandisingContext Context { get; }
            public IUnitOfWork Uow { get; }
            public IClienteService Clientes { get; }
            public IAutenticacionService Autenticacion { get; }
            public IBitacoraService Bitacora { get; }
            public ILogService Logs { get; }
            public IUsuarioService Usuarios { get; }
            public IPerfilService Perfiles { get; }

            internal ServiceScope(GestorMerchandisingContext ctx)
            {
                Context = ctx;
                Uow = new EfUnitOfWork(ctx);
                Clientes = new ClienteService(Uow);
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
            return new ServiceScope(CrearContexto());
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
            var metodoPrivado = typeof(ServiceFactory)
                .GetMethod("ObtenerConnectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            return (string)metodoPrivado.Invoke(null, null);
        }

        public static IGeoService CrearGeoService()
        {
            // 1) crear el DbContext concreto (usa el TUYO: por nombre suele ser GestorMerchandisingContext)
            DbContext ctx = new GestorMerchandisingContext(); // <-- reemplazá por tu clase real de DbContext

            // 2) crear el UoW EF con ese contexto
            IUnitOfWork uow = new EfUnitOfWork(ctx);

            // 3) devolver el GeoService
            return new GeoService(uow);
        }

        public static ILogService CrearLogService()
        {
            return new LogService();
        }

        public static IUsuarioService CrearUsuarioService()
        {
            var uow = CrearUnitOfWork();
            var enc = CrearEncriptacionService();
            return new UsuarioService(uow, enc);
        }

        // También agregar IPerfilService si no existe:
        /// <summary>
        /// Crea el servicio de perfiles.
        /// </summary>
        public static IPerfilService CrearPerfilService()
        {
            var uow = CrearUnitOfWork();
            return new PerfilService(uow);
        }
    }
}
