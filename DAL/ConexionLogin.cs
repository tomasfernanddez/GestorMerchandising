using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// Gestiona las conexiones SQL hacia la base de datos de seguridad utilizada en el módulo de login.
    /// </summary>
    public class ConexionLogin
    {
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa la conexión resolviendo dinámicamente la cadena correspondiente a la base de seguridad.
        /// </summary>
        public ConexionLogin()
        {
            _connectionString = DAL.ScriptsSQL.DatabaseInitializer.GetConnectionStringSeguridad();
        }

        /// <summary>
        /// Crea una nueva conexión SQL lista para usarse en operaciones de autenticación.
        /// </summary>
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}