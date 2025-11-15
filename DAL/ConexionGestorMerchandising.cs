using System;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class ConexionGestorMerchandising
    {
        private readonly string _connectionString;

        public ConexionGestorMerchandising()
        {
            _connectionString = DAL.ScriptsSQL.DatabaseInitializer.GetConnectionString();
        }
        public SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        public bool ProbarConexion()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error de conexión: " + ex.Message);
                return false;
            }
        }
    }
}