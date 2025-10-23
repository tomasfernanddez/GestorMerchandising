using BLL.Interfaces;
using BLL.Services;
using DAL;
using DAL.Interfaces.Base;
using System;
using System.Configuration;
using System.Data.Entity;
using BizIUnitOfWork = DAL.Interfaces.Base.IUnitOfWork;
using BizEfUnitOfWork = DAL.Implementations.Base.EfUnitOfWork;
using BizDbContext = DAL.GestorMerchandisingContext;

namespace BLL.Factories
{
    public static class ServiceFactory
    {

        // =====================================================================
        // CONFIGURACIÓN CENTRALIZADA
        // =====================================================================
        private static string _connectionString;
        private static bool _databaseActualizada;
        private static readonly object _upgradeLock = new object();

        /// <summary>
        /// Configura el connection string a usar en toda la aplicación.
        /// Se llama típicamente desde el inicio de la UI.
        /// </summary>
        public static void ConfigurarConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            EnsureDatabaseActualizada();
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

        private static BizDbContext CrearContextoNegocio()
        {
            EnsureDatabaseActualizada();
            var cs = ObtenerConnectionString();
            return new BizDbContext(cs);
        }
        private static BizIUnitOfWork CrearUnitOfWorkNegocio()
        {
            var ctx = CrearContextoNegocio();
            return new BizEfUnitOfWork(ctx);
        }

        public static IClienteService CrearClienteService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new ClienteService(uow);
        }

        public static IProveedorService CrearProveedorService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new ProveedorService(uow);
        }

        public static ICondicionIvaService CrearCondicionIvaService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new CondicionIvaService(uow);
        }

        // TODO: Agregar cuando se implementen
        /*
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

            internal ServiceScope(GestorMerchandisingContext ctx)
            {
                Context = ctx;
                Uow = new BizEfUnitOfWork(ctx);
                Clientes = new ClienteService(Uow);
            }

            public void Dispose()
            {
                (Uow as IDisposable)?.Dispose(); // si UoW dispone el Context, mejor
                Context?.Dispose();
            }
        }

        public static ServiceScope BeginScope()
        {
            return new ServiceScope(CrearContextoNegocio());
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

        private static void EnsureDatabaseActualizada()
        {
            if (_databaseActualizada)
                return;

            lock (_upgradeLock)
            {
                if (_databaseActualizada)
                    return;

                var cs = ObtenerConnectionString();
                if (string.IsNullOrWhiteSpace(cs))
                    return;

                var csSettings = ConfigurationManager.ConnectionStrings[cs];
                var connectionString = csSettings?.ConnectionString ?? cs;

                DatabaseUpgrade.EnsureUpToDate(connectionString);
                _databaseActualizada = true;
            }
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
            DbContext ctx = new BizDbContext(); // <-- reemplazá por tu clase real de DbContext

            // 2) crear el UoW EF con ese contexto
            IUnitOfWork uow = new BizEfUnitOfWork(ctx);

            // 3) devolver el GeoService
            return new GeoService(uow);
        }
    }
}
