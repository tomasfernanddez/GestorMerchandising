using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConexionLogin
    {
        private readonly string _connectionString;

        public ConexionLogin()
        {
            _connectionString = DAL.ScriptsSQL.DatabaseInitializer.GetConnectionStringSeguridad();
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}