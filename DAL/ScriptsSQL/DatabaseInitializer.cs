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
    /// <summary>
    /// Orquesta la creación automática de las bases de datos de negocio y seguridad, resolviendo sus cadenas de conexión.
    /// </summary>
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

        /// <summary>
        /// Crea ambas bases de datos (negocio y seguridad) si no existen todavía.
        /// </summary>
        public static void Initialize()
        {
            InitializeDatabase("GestorMerchandisingNegocioDB", _dbNegocio, "GestorMerchandisingNegocio.sql");
            InitializeDatabase("GestorMerchandisingSeguridadDB", _dbSeguridad, "GestorMerchandisingSeguridad.sql");
        }

        /// <summary>
        /// Ejecuta el script de inicialización para una base de datos determinada, usando la cadena de configuración indicada.
        /// </summary>
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
                string path = ResolveScriptPath(scriptFile);
                string script = File.ReadAllText(path);
                ExecuteSqlScript(masterConn, script, dbName);
                Console.WriteLine($"✔ Base de datos '{dbName}' creada correctamente.");
            }
            else
            {
                Console.WriteLine($"ℹ Base de datos '{dbName}' ya existe, se omite creación.");
            }
        }

        /// <summary>
        /// Resuelve la ruta física del script SQL buscando en distintos puntos conocidos del proyecto/binario.
        /// </summary>
        /// <param name="scriptFile">Nombre del archivo de script.</param>
        /// <returns>Ruta absoluta existente del archivo solicitado.</returns>
        private static string ResolveScriptPath(string scriptFile)
        {
            foreach (var root in CandidateRoots().Distinct(StringComparer.OrdinalIgnoreCase))
            {
                foreach (var candidate in EnumerateCandidates(root, scriptFile))
                {
                    if (File.Exists(candidate))
                        return candidate;
                }
            }

            throw new FileNotFoundException($"No se encontró el script {scriptFile}");
        }

        private static IEnumerable<string> CandidateRoots()
        {
            var roots = new List<string>();

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrWhiteSpace(baseDirectory))
                roots.Add(baseDirectory);

            var executingAssemblyDirectory = Path.GetDirectoryName(typeof(DatabaseInitializer).Assembly.Location);
            if (!string.IsNullOrWhiteSpace(executingAssemblyDirectory))
                roots.Add(executingAssemblyDirectory);

            return roots;
        }

        /// <summary>
        /// Genera posibles rutas candidatas a partir de un directorio base ascendiendo en la jerarquía.
        /// </summary>
        private static IEnumerable<string> EnumerateCandidates(string root, string scriptFile)
        {
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var current = new DirectoryInfo(root);

            while (current != null && visited.Add(current.FullName))
            {
                yield return Path.Combine(current.FullName, "ScriptsSQL", scriptFile);
                yield return Path.Combine(current.FullName, scriptFile);
                current = current.Parent;
            }
        }

        /// <summary>
        /// Verifica si la base de datos objetivo ya existe en el servidor de SQL Server.
        /// </summary>
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

        /// <summary>
        /// Determina un servidor disponible para conectarse probando contra múltiples instancias conocidas.
        /// </summary>
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

        /// <summary>
        /// Ejecuta un script SQL completo, respetando los delimitadores GO para separar bloques.
        /// </summary>
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

        /// <summary>
        /// Ejecuta un bloque individual del script SQL.
        /// </summary>
        private static void ExecuteBlock(SqlConnection conn, StringBuilder sb, string dbName)
        {
            if (sb.Length == 0) return;

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Obtiene la cadena de conexión final para la base de datos de negocio, apuntando al servidor válido detectado.
        /// </summary>
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

        /// <summary>
        /// Obtiene la cadena de conexión final para la base de datos de seguridad, apuntando al servidor válido detectado.
        /// </summary>
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