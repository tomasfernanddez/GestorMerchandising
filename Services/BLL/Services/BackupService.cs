using Services.BLL.Interfaces;
using Services.BLL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Services.BLL.Services
{
    public class BackupService : IBackupService
    {
        private readonly string _backupDirectory;
        private readonly string _connectionString;
        private readonly string _adminConnectionString;
        private readonly string _databaseName;
        private readonly string _serverBackupDirectory;

        public BackupService(string connectionString = null, string backupDirectory = null)
        {
            _connectionString = ResolverConnectionString(connectionString);
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("No se encontró la cadena de conexión de la base de datos.");
            }

            var builder = new SqlConnectionStringBuilder(_connectionString);
            if (string.IsNullOrWhiteSpace(builder.InitialCatalog))
            {
                throw new InvalidOperationException("La cadena de conexión no especifica la base de datos a respaldar.");
            }

            _databaseName = builder.InitialCatalog;

            var adminBuilder = new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = "master"
            };
            _adminConnectionString = adminBuilder.ConnectionString;

            _serverBackupDirectory = ObtenerDirectorioBackupServidor();
            _backupDirectory = PrepararDirectorioBackups(backupDirectory);
        }

        public string RealizarBackup(string rutaDestino = null)
        {
            var ruta = ObtenerRutaBackup(rutaDestino);

            using (var connection = new SqlConnection(_adminConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandTimeout = 0;
                command.CommandText = $"BACKUP DATABASE [{_databaseName}] TO DISK = N'{EscaparRuta(ruta)}' WITH INIT, FORMAT, NAME = N'{_databaseName} Full Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 5";
                command.ExecuteNonQuery();
            }

            return ruta;
        }

        public void RestaurarBackup(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                throw new ArgumentException("La ruta del backup no puede ser vacía.", nameof(rutaArchivo));
            }

            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException("No se encontró el archivo de backup especificado.", rutaArchivo);
            }

            using (var connection = new SqlConnection(_adminConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandTimeout = 0;

                command.CommandText = $"ALTER DATABASE [{_databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                command.ExecuteNonQuery();

                try
                {
                    command.CommandText = $"RESTORE DATABASE [{_databaseName}] FROM DISK = N'{EscaparRuta(rutaArchivo)}' WITH REPLACE, STATS = 5";
                    command.ExecuteNonQuery();
                }
                finally
                {
                    try
                    {
                        command.CommandText = $"ALTER DATABASE [{_databaseName}] SET MULTI_USER";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        // Si falla el cambio de modo, no detener el flujo; el administrador puede ajustarlo manualmente.
                    }
                }
            }
        }

        public IEnumerable<BackupFileInfo> ListarBackups()
        {
            if (!Directory.Exists(_backupDirectory))
            {
                yield break;
            }

            var archivos = Directory.GetFiles(_backupDirectory, "*.bak", SearchOption.TopDirectoryOnly)
                .Select(ruta => new FileInfo(ruta))
                .OrderByDescending(info => info.CreationTimeUtc)
                .ThenByDescending(info => info.LastWriteTimeUtc);

            foreach (var archivo in archivos)
            {
                yield return new BackupFileInfo
                {
                    Nombre = archivo.Name,
                    RutaCompleta = archivo.FullName,
                    FechaCreacion = archivo.LastWriteTime,
                    TamanoBytes = archivo.Length
                };
            }
        }

        public string ObtenerDirectorioBackups()
        {
            return _backupDirectory;
        }

        public string GenerarNombreSugerido()
        {
            return $"{_databaseName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
        }

        private string PrepararDirectorioBackups(string backupDirectory)
        {
            var rutaConfigurada = backupDirectory ?? ConfigurationManager.AppSettings["BackupPath"] ?? "backups\\";
            var rutaFinal = NormalizarRutaDirectorio(rutaConfigurada);
            AsegurarDirectorio(rutaFinal);
            return rutaFinal;
        }

        private string ObtenerRutaBackup(string rutaDestino)
        {
            if (string.IsNullOrWhiteSpace(rutaDestino))
            {
                rutaDestino = Path.Combine(_backupDirectory, GenerarNombreSugerido());
            }
            else if (!Path.IsPathRooted(rutaDestino))
            {
                rutaDestino = Path.Combine(_backupDirectory, rutaDestino);
            }

            var directorio = Path.GetDirectoryName(rutaDestino);
            if (!string.IsNullOrWhiteSpace(directorio) && !Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            return rutaDestino;
        }

        private string NormalizarRutaDirectorio(string rutaConfigurada)
        {
            if (Path.IsPathRooted(rutaConfigurada))
            {
                return rutaConfigurada;
            }

            var baseDir = !string.IsNullOrWhiteSpace(_serverBackupDirectory)
                ? _serverBackupDirectory
                : AppDomain.CurrentDomain.BaseDirectory;

            return Path.GetFullPath(Path.Combine(baseDir, rutaConfigurada));
        }

        private static void AsegurarDirectorio(string rutaFinal)
        {
            if (string.IsNullOrWhiteSpace(rutaFinal))
            {
                throw new InvalidOperationException("No se pudo determinar un directorio válido para almacenar los backups.");
            }

            if (Directory.Exists(rutaFinal))
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(rutaFinal);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new InvalidOperationException("La aplicación no tiene permisos para crear la carpeta de backups especificada. Configure un directorio accesible para el servicio de SQL Server.", ex);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException("No se pudo preparar la carpeta de backups especificada. Verifique la configuración del directorio de backups.", ex);
            }
        }

        private string ResolverConnectionString(string connectionStringOrName)
        {
            var cs = connectionStringOrName;

            if (string.IsNullOrWhiteSpace(cs))
            {
                cs = ConfigurationManager.ConnectionStrings["GestorMerchandisingDB"]?.ConnectionString;
            }
            else
            {
                var settings = ConfigurationManager.ConnectionStrings[cs];
                if (settings != null)
                {
                    cs = settings.ConnectionString;
                }
            }

            return cs;
        }

        private static string EscaparRuta(string ruta)
        {
            return ruta.Replace("'", "''");
        }

        private string ObtenerDirectorioBackupServidor()
        {
            try
            {
                using (var connection = new SqlConnection(_adminConnectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = "SELECT CAST(ISNULL(SERVERPROPERTY('InstanceDefaultBackupPath'), '') AS NVARCHAR(4000))";
                    var resultado = command.ExecuteScalar() as string;
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        return resultado;
                    }

                    command.CommandText = "DECLARE @ruta NVARCHAR(4000); EXEC master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\\Microsoft\\MSSQLServer\\MSSQLServer', N'BackupDirectory', @ruta OUTPUT; SELECT ISNULL(@ruta, '')";
                    resultado = command.ExecuteScalar() as string;
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        return resultado;
                    }
                }
            }
            catch (SqlException)
            {
                // Ignorar y usar las rutas configuradas.
            }
            catch (InvalidOperationException)
            {
                // Ignorar y usar las rutas configuradas.
            }

            return null;
        }
    }
}