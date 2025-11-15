using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ScriptsSQL
{
    public static class DatabaseInitializer
    {
        private static readonly string[] PossibleServers =
        {
            @"(localdb)\MSSQLLocalDB",
            @"localhost\SQLEXPRESS",
            @"localhost"
        };

        private static string _dbNegocio = "GestorMerchandisingNegocio";
        private static string _dbSeguridad = "GestorMerchandisingSeguridad";

        public static void Initialize()
        {
            InitializeDatabase("GestorMerchandisingNegocioDB", _dbNegocio, "GestorMerchandisingNegocio.sql");
            InitializeDatabase("GestorMerchandisingSeguridadDB", _dbSeguridad, "GestorMerchandisingSeguridad.sql");
        }

        private static void InitializeDatabase(string connectionName, string dbName, string scriptFile)
        {
            string baseConnection = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            string workingServer = GetWorkingServer(baseConnection);

            var builder = new SqlConnectionStringBuilder(baseConnection)
            {
                DataSource = workingServer,
                InitialCatalog = "master"
            };

            string masterConn = builder.ConnectionString;

            if (!DatabaseExists(masterConn, dbName))
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScriptsSQL", scriptFile);
                if (!File.Exists(path))
                    throw new FileNotFoundException($"No se encontró el script {scriptFile}", path);

                string script = File.ReadAllText(path);
                ExecuteSqlScript(masterConn, script, dbName);
                Console.WriteLine($"✔ Base de datos '{dbName}' creada correctamente.");
            }
            else
            {
                Console.WriteLine($"ℹ Base de datos '{dbName}' ya existe, se omite creación.");
            }
        }

        private static bool DatabaseExists(string masterConn, string dbName)
        {
            using (var conn = new SqlConnection(masterConn))
            {
                conn.Open();
                using (var cmd = new SqlCommand($"SELECT db_id('{dbName}')", conn))
                {
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value && result != null;
                }
            }
        }

        private static string GetWorkingServer(string baseConnection)
        {
            foreach (var server in PossibleServers)
            {
                try
                {
                    var builder = new SqlConnectionStringBuilder(baseConnection)
                    {
                        DataSource = server,
                        InitialCatalog = "master"
                    };

                    using (var conn = new SqlConnection(builder.ConnectionString))
                    {
                        conn.Open();
                        Console.WriteLine($"✔ Conectado a servidor: {server}");
                        return server;
                    }
                }
                catch
                {
                    Console.WriteLine($"✘ No se pudo conectar a {server}, probando siguiente...");
                }
            }

            throw new Exception("❌ No se encontró ninguna instancia válida de SQL Server.");
        }

        private static void ExecuteSqlScript(string masterConn, string script, string dbName)
        {
            using (var conn = new SqlConnection(masterConn))
            {
                conn.Open();

                var lines = script.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                StringBuilder sb = new StringBuilder();

                foreach (var line in lines)
                {
                    if (line.Trim().Equals("GO", StringComparison.OrdinalIgnoreCase))
                    {
                        ExecuteBlock(conn, sb, dbName);
                        sb.Clear();
                    }
                    else sb.AppendLine(line);
                }

                if (sb.Length > 0)
                    ExecuteBlock(conn, sb, dbName);
            }
        }

        private static void ExecuteBlock(SqlConnection conn, StringBuilder sb, string dbName)
        {
            if (sb.Length == 0) return;

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public static string GetConnectionString()
        {
            string baseConnection = ConfigurationManager.ConnectionStrings["GestorMerchandisingNegocioDB"].ConnectionString;
            string workingServer = GetWorkingServer(baseConnection);
            var builder = new SqlConnectionStringBuilder(baseConnection)
            {
                DataSource = workingServer,
                InitialCatalog = _dbNegocio
            };
            return builder.ConnectionString;
        }

        public static string GetConnectionStringSeguridad()
        {
            string baseConnection = ConfigurationManager.ConnectionStrings["GestorMerchandisingSeguridadDB"].ConnectionString;
            string workingServer = GetWorkingServer(baseConnection);
            var builder = new SqlConnectionStringBuilder(baseConnection)
            {
                DataSource = workingServer,
                InitialCatalog = _dbSeguridad
            };
            return builder.ConnectionString;
        }
    }
}