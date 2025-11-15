using DAL.ScriptsSQL;

namespace BLL.Infrastructure
{
    /// <summary>
    /// Centraliza la inicialización y resolución de cadenas de conexión para la capa de datos.
    /// </summary>
    public static class DatabaseBootstrapper
    {
        /// <summary>
        /// Inicializa las bases de datos de negocio y seguridad, devolviendo las cadenas de conexión listas para usar.
        /// </summary>
        public static void Initialize(out string connectionStringNegocio, out string connectionStringSeguridad)
        {
            DatabaseInitializer.Initialize();
            connectionStringNegocio = DatabaseInitializer.GetConnectionString();
            connectionStringSeguridad = DatabaseInitializer.GetConnectionStringSeguridad();
        }
    }
}