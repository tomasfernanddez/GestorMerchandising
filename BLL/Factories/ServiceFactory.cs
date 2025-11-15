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
        private const string ConnectionStringName = "GestorMerchandisingNegocioDB";
        private static bool _databaseActualizada;
        private static readonly object _upgradeLock = new object();

        /// <summary>
        /// Configura el connection string a usar en toda la aplicación.
        /// Se llama típicamente desde el inicio de la UI.
        /// </summary>
        public static void ConfigurarConnectionString(string connectionString)
        {
            _connectionString = string.IsNullOrWhiteSpace(connectionString) ? null : connectionString;
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
            var cs = ConfigurationManager.ConnectionStrings[ConnectionStringName]?.ConnectionString;
            if (!string.IsNullOrWhiteSpace(cs))
                return cs;

            // 3. Nombre por defecto (busca en app.config)
            return ConnectionStringName;
        }

        // =====================================================================
        // INFRAESTRUCTURA PRIVADA
        // =====================================================================
        /// <summary>
        /// Crea contexto negocio.
        /// </summary>
        
        private static BizDbContext CrearContextoNegocio()
        {
            EnsureDatabaseActualizada();
            var cs = ObtenerConnectionString();
            return new BizDbContext(cs);
        }
        /// <summary>
        /// Crea unit of work negocio.
        /// </summary>
        private static BizIUnitOfWork CrearUnitOfWorkNegocio()
        {
            var ctx = CrearContextoNegocio();
            return new BizEfUnitOfWork(ctx);
        }
        /// <summary>
        /// Crea cliente service.
        /// </summary>

        public static IClienteService CrearClienteService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new ClienteService(uow);
        }
        /// <summary>
        /// Crea proveedor service.
        /// </summary>

        public static IProveedorService CrearProveedorService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new ProveedorService(uow);
        }
        /// <summary>
        /// Crea producto service.
        /// </summary>

        public static IProductoService CrearProductoService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new ProductoService(uow);
        }
        /// <summary>
        /// Crea categoria producto service.
        /// </summary>

        public static ICategoriaProductoService CrearCategoriaProductoService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new CategoriaProductoService(uow);
        }
        /// <summary>
        /// Crea pedido service.
        /// </summary>

        public static IPedidoService CrearPedidoService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new PedidoService(uow);
        }
        /// <summary>
        /// Crea pedido muestra service.
        /// </summary>

        public static IPedidoMuestraService CrearPedidoMuestraService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new PedidoMuestraService(uow);
        }
        /// <summary>
        /// Crea reporte service.
        /// </summary>

        public static IReporteService CrearReporteService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new ReporteService(uow);
        }
        /// <summary>
        /// Crea condicion iva service.
        /// </summary>
        public static ICondicionIvaService CrearCondicionIvaService()
        {
            var uow = CrearUnitOfWorkNegocio();
            return new CondicionIvaService(uow);
        }

        public sealed class ServiceScope : IDisposable
        {
            public GestorMerchandisingContext Context { get; }
            public IUnitOfWork Uow { get; }
            public IClienteService Clientes { get; }

            /// <summary>
            /// Inicializa una nueva instancia de ServiceScope.
            /// </summary>
            internal ServiceScope(GestorMerchandisingContext ctx)
            {
                Context = ctx;
                Uow = new BizEfUnitOfWork(ctx);
                Clientes = new ClienteService(Uow);
            }

            /// <summary>
            /// Libera los recursos administrados y no administrados utilizados por la instancia.
            /// </summary>
            public void Dispose()
            {
                (Uow as IDisposable)?.Dispose(); // si UoW dispone el Context, mejor
                Context?.Dispose();
            }
        }

        /// <summary>
        /// Inicia scope.
        /// </summary>
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

        /// <summary>
        /// Garantiza database actualizada.
        /// </summary>
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

        /// <summary>
        /// Obtiene connection string interno.
        /// </summary>
        internal static string GetConnectionStringInterno()
        {
            // reutiliza tu ObtenerConnectionString() privado
            var metodoPrivado = typeof(ServiceFactory)
                .GetMethod("ObtenerConnectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            return (string)metodoPrivado.Invoke(null, null);
        }

        /// <summary>
        /// Crea geo service.
        /// </summary>
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
