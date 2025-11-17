using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI.Helpers
{
    /// <summary>
    /// Provee un registro mínimo de arranque para diagnosticar errores antes de que se configure el log principal.
    /// </summary>
    internal static class StartupDiagnostics
    {
        private const string AppFolderName = "GestorMerchandising";
        private const string StartupLogFileName = "startup.log";

        private static readonly object _fileLock = new object();
        private static readonly string _logDirectory = ResolveLogDirectory();
        private static readonly string _logFilePath = Path.Combine(_logDirectory, StartupLogFileName);
        private static TextWriter _fileWriter;
        private static bool _initialized;

        public static string LogFilePath => _logFilePath;

        /// <summary>
        /// Redirecciona la salida de consola al archivo de log de arranque para capturar mensajes tempranos.
        /// </summary>
        public static void BeginSession()
        {
            if (_initialized)
                return;

            _initialized = true;

            try
            {
                Directory.CreateDirectory(_logDirectory);

                _fileWriter = TextWriter.Synchronized(new StreamWriter(new FileStream(_logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8)
                {
                    AutoFlush = true
                });

                var teeWriter = new TeeTextWriter(Console.Out, _fileWriter);
                Console.SetOut(TextWriter.Synchronized(teeWriter));
                Console.SetError(TextWriter.Synchronized(teeWriter));

                AppDomain.CurrentDomain.ProcessExit += (_, __) => Shutdown();
                AppDomain.CurrentDomain.DomainUnload += (_, __) => Shutdown();

                Log("=== Inicio del diagnóstico de arranque ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"StartupDiagnostics initialization failed: {ex}");
            }
        }

        /// <summary>
        /// Registra un mensaje en el archivo de arranque.
        /// </summary>
        public static void Log(string message, Exception exception = null)
        {
            if (string.IsNullOrWhiteSpace(message) && exception == null)
                return;

            try
            {
                var sb = new StringBuilder()
                    .Append('[')
                    .Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .Append("] ")
                    .AppendLine(message ?? string.Empty);

                if (exception != null)
                {
                    sb.AppendLine(exception.ToString());
                }

                lock (_fileLock)
                {
                    if (_fileWriter != null)
                    {
                        _fileWriter.Write(sb.ToString());
                        _fileWriter.Flush();
                    }
                    else
                    {
                        File.AppendAllText(_logFilePath, sb.ToString(), Encoding.UTF8);
                    }
                }
            }
            catch
            {
                // No interrumpir el flujo de inicio por errores de logging.
            }
        }

        /// <summary>
        /// Muestra un mensaje fatal al usuario e informa dónde encontrar el log.
        /// </summary>
        public static void ReportFatal(string context, Exception exception)
        {
            Log(context, exception);

            var sb = new StringBuilder()
                .AppendLine("No se pudo iniciar la aplicación.");

            if (!string.IsNullOrWhiteSpace(context))
            {
                sb.AppendLine(context);
            }

            if (exception != null)
            {
                sb.AppendLine(exception.Message);
            }

            sb.AppendLine()
              .AppendLine("Revisá el detalle en:")
              .AppendLine(_logFilePath);

            try
            {
                MessageBox.Show(sb.ToString(), "Error al iniciar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                // En caso de no poder mostrar UI (por ejemplo, en servicios).
            }
        }

        private static void Shutdown()
        {
            try
            {
                _fileWriter?.Flush();
                _fileWriter?.Dispose();
                _fileWriter = null;
            }
            catch
            {
                // Ignorar errores al cerrar.
            }
        }

        private static string ResolveLogDirectory()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            if (string.IsNullOrWhiteSpace(basePath))
            {
                basePath = AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory;
            }

            return Path.Combine(basePath, AppFolderName, "logs");
        }

        private sealed class TeeTextWriter : TextWriter
        {
            private readonly TextWriter[] _writers;

            public TeeTextWriter(params TextWriter[] writers)
            {
                _writers = writers?.Where(w => w != null).ToArray() ?? Array.Empty<TextWriter>();
            }

            public override Encoding Encoding => Encoding.UTF8;

            public override void Write(char value)
            {
                foreach (var writer in _writers)
                {
                    writer.Write(value);
                }
            }

            public override void Flush()
            {
                foreach (var writer in _writers)
                {
                    writer.Flush();
                }
            }
        }
    }
}