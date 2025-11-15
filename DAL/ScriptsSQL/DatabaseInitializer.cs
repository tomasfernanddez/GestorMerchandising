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
        private const string DefaultDbNegocio = "GestorMerchandisingNegocio";
        private const string DefaultDbSeguridad = "GestorMerchandisingSeguridad";

        private static readonly string[] PossibleServers =
        {
            @"(localdb)\MSSQLLocalDB",
            @"localhost\SQLEXPRESS",
            @"localhost"
        };

        private static string _dbNegocio = DefaultDbNegocio;
        private static string _dbSeguridad = DefaultDbSeguridad;

        /// <summary>
        /// Crea ambas bases de datos (negocio y seguridad) si no existen todavía.
        /// </summary>
        public static void Initialize()
        {
            EnsureDatabaseForConnection(null, "GestorMerchandisingNegocioDB", DefaultDbNegocio, ref _dbNegocio, "GestorMerchandisingNegocio.sql");
            EnsureDatabaseForConnection(null, "GestorMerchandisingSeguridadDB", DefaultDbSeguridad, ref _dbSeguridad, "GestorMerchandisingSeguridad.sql");
        }

        /// <summary>
        /// Garantiza la existencia de la base de datos de negocio utilizando la cadena de conexión indicada.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión final (puede ser nula para usar la configurada por nombre).</param>
        public static void EnsureNegocioDatabase(string connectionString)
        {
            EnsureDatabaseForConnection(connectionString, "GestorMerchandisingNegocioDB", DefaultDbNegocio, ref _dbNegocio, "GestorMerchandisingNegocio.sql");
        }

        /// <summary>
        /// Garantiza la existencia de la base de datos de seguridad utilizando la cadena de conexión indicada.
        /// </summary>
        public static void EnsureSeguridadDatabase(string connectionString)
        {
            EnsureDatabaseForConnection(connectionString, "GestorMerchandisingSeguridadDB", DefaultDbSeguridad, ref _dbSeguridad, "GestorMerchandisingSeguridad.sql");
        }

        /// <summary>
        /// Ejecuta el script de inicialización para una base de datos determinada, resolviendo servidor y nombre de base dinámicamente.        /// </summary>
        /// <summary>
        private static void EnsureDatabaseForConnection(string connectionString, string connectionName, string defaultDbName, ref string currentDbName, string scriptFile)
        {
            string baseConnection = connectionString;

            if (string.IsNullOrWhiteSpace(baseConnection))
            {
                baseConnection = ConfigurationManager.ConnectionStrings[connectionName]?.ConnectionString;
            }

            if (string.IsNullOrWhiteSpace(baseConnection))
                throw new InvalidOperationException($"No se encontró la cadena de conexión '{connectionName}'.");

            var builder = new SqlConnectionStringBuilder(baseConnection);

            if (!string.IsNullOrWhiteSpace(builder.InitialCatalog))
                currentDbName = builder.InitialCatalog;

            string workingServer = GetWorkingServer(baseConnection);

            var masterBuilder = new SqlConnectionStringBuilder(baseConnection)
            {
                DataSource = workingServer,
                InitialCatalog = "master"
            };

            string masterConn = masterBuilder.ConnectionString;

            if (!DatabaseExists(masterConn, currentDbName))
            {
                string path = ResolveScriptPath(scriptFile);
                string script = File.ReadAllText(path);
                script = AdaptScriptForDatabase(script, defaultDbName, currentDbName);
                ExecuteSqlScript(masterConn, script, currentDbName);
                Console.WriteLine($"✔ Base de datos '{currentDbName}' creada correctamente.");
            }
            else
            {
                Console.WriteLine($"ℹ Base de datos '{currentDbName}' ya existe, se omite creación.");
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
            var builder = new SqlConnectionStringBuilder(baseConnection);
            var candidates = new List<string>();

            if (!string.IsNullOrWhiteSpace(builder.DataSource))
                candidates.Add(builder.DataSource);

            candidates.AddRange(PossibleServers);

            foreach (var server in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                try
                {
                    builder.DataSource = server;
                    builder.InitialCatalog = "master";

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
        /// Adapta el script SQL para usar el nombre de base de datos objetivo en lugar del por defecto.
        /// <summary>
        private static string AdaptScriptForDatabase(string script, string defaultName, string targetName)
        {
            if (string.Equals(defaultName, targetName, StringComparison.OrdinalIgnoreCase))
                return script;

            return script
                .Replace($"[{defaultName}]", $"[{targetName}]")
                .Replace($"'{defaultName}'", $"'{targetName}'");
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