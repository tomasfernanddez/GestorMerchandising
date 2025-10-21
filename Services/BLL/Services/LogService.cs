using Services.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Services.BLL.Services
{
    public class LogService : ILogService
    {
        private readonly string _logPath;
        private readonly bool _logToFile;
        private static readonly object _lockObject = new object();

        public LogService()
        {
            _logPath = ConfigurationManager.AppSettings["LogPath"] ?? "logs\\";
            _logToFile = bool.Parse(ConfigurationManager.AppSettings["LogToFile"] ?? "true");

            // Crear directorio si no existe
            if (_logToFile && !Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
        }

        public void LogError(string mensaje, Exception ex = null, string modulo = null, string usuario = null)
        {
            var logEntry = CrearEntradaLog("ERROR", mensaje, ex, modulo, usuario);
            EscribirLog(logEntry);
        }

        public void LogInfo(string mensaje, string modulo = null, string usuario = null)
        {
            var logEntry = CrearEntradaLog("INFO", mensaje, null, modulo, usuario);
            EscribirLog(logEntry);

            // También escribir en Debug para desarrollo
            System.Diagnostics.Debug.WriteLine(logEntry);
        }

        public void LogWarning(string mensaje, string modulo = null, string usuario = null)
        {
            var logEntry = CrearEntradaLog("WARNING", mensaje, null, modulo, usuario);
            EscribirLog(logEntry);
        }

        public string[] ObtenerUltimosLogs(int cantidad = 100)
        {
            try
            {
                var archivoHoy = ObtenerNombreArchivoLog(DateTime.Now);
                var rutaCompleta = Path.Combine(_logPath, archivoHoy);

                if (!File.Exists(rutaCompleta))
                    return new string[0];

                var todasLineas = File.ReadAllLines(rutaCompleta);

                // Devolver las últimas 'cantidad' líneas
                if (todasLineas.Length <= cantidad)
                    return todasLineas;

                var resultado = new string[cantidad];
                Array.Copy(todasLineas, todasLineas.Length - cantidad, resultado, 0, cantidad);
                return resultado;
            }
            catch (Exception ex)
            {
                return new[] { $"Error leyendo logs: {ex.Message}" };
            }
        }

        public void LimpiarLogsAntiguos(int diasAMantener = 30)
        {
            try
            {
                if (!Directory.Exists(_logPath)) return;

                var fechaLimite = DateTime.Now.AddDays(-diasAMantener);
                var archivos = Directory.GetFiles(_logPath, "*.log");

                foreach (var archivo in archivos)
                {
                    var fechaArchivo = File.GetCreationTime(archivo);
                    if (fechaArchivo < fechaLimite)
                    {
                        File.Delete(archivo);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log del error de limpieza (sin recursión)
                System.Diagnostics.Debug.WriteLine($"Error limpiando logs antiguos: {ex.Message}");
            }
        }

        private string CrearEntradaLog(string nivel, string mensaje, Exception ex, string modulo, string usuario)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var moduloStr = string.IsNullOrEmpty(modulo) ? "" : $"[{modulo}]";
            var usuarioStr = string.IsNullOrEmpty(usuario) ? "" : $"[{usuario}]";

            var entrada = $"{timestamp} {nivel} {moduloStr}{usuarioStr} {mensaje}";

            if (ex != null)
            {
                entrada += $"\nExcepción: {ex.GetType().Name}: {ex.Message}";
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    entrada += $"\nStackTrace: {ex.StackTrace}";
                }

                // Incluir inner exceptions
                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    entrada += $"\nInner Exception: {innerEx.GetType().Name}: {innerEx.Message}";
                    innerEx = innerEx.InnerException;
                }
            }

            return entrada;
        }

        private void EscribirLog(string entrada)
        {
            if (!_logToFile) return;

            try
            {
                lock (_lockObject)
                {
                    var nombreArchivo = ObtenerNombreArchivoLog(DateTime.Now);
                    var rutaCompleta = Path.Combine(_logPath, nombreArchivo);

                    File.AppendAllText(rutaCompleta, entrada + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // Fallback a System.Diagnostics si falla el archivo
                System.Diagnostics.Debug.WriteLine($"Error escribiendo log: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Entrada original: {entrada}");
            }
        }

        private string ObtenerNombreArchivoLog(DateTime fecha)
        {
            return $"gestor_merchandising_{fecha:yyyyMMdd}.log";
        }
    }
}
