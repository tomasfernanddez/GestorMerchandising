using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Localization
{
    public static class Localization
    {
        private static readonly object _lock = new object();
        private static Dictionary<string, string> _strings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static string _loadedCulture = null;

        /// <summary>
        /// Carga el archivo idioma.<culture>.txt (UTF-8) desde la carpeta Idiomas del ejecutable.
        /// Formato: clave=valor (comentarios con # o //).
        /// </summary>
        public static void Load(string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName)) cultureName = "es-AR";

            lock (_lock)
            {
                if (string.Equals(_loadedCulture, cultureName, StringComparison.OrdinalIgnoreCase))
                    return;

                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var dir = Path.Combine(baseDir, "Idiomas");

                // intentos de resolución: cultura completa, cultura neutral, es-AR, es
                var candidates = new[]
                {
                    Path.Combine(dir, $"idioma.{cultureName}.txt"),
                    Path.Combine(dir, $"idioma.{new CultureInfo(cultureName).TwoLetterISOLanguageName}.txt"),
                    Path.Combine(dir, "idioma.es-AR.txt"),
                    Path.Combine(dir, "idioma.es.txt")
                };

                var file = "";
                foreach (var c in candidates)
                    if (File.Exists(c)) { file = c; break; }

                var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                if (!string.IsNullOrEmpty(file))
                {
                    foreach (var raw in File.ReadAllLines(file))
                    {
                        var line = raw.Trim();
                        if (line.Length == 0) continue;
                        if (line.StartsWith("#") || line.StartsWith("//")) continue;

                        var idx = line.IndexOf('=');
                        if (idx <= 0) continue;

                        var key = line.Substring(0, idx).Trim();
                        var value = line.Substring(idx + 1).Trim();

                        if (key.Length == 0) continue;
                        dict[key] = value;
                    }
                }

                _strings = dict;
                _loadedCulture = cultureName;
            }
        }

        /// <summary>
        /// Devuelve la traducción de la clave; si no existe, devuelve la clave tal cual.
        /// Permite string.Format con parámetros.
        /// </summary>
        public static string T(string key, params object[] args)
        {
            if (string.IsNullOrEmpty(key)) return key ?? "";
            string value;
            lock (_lock)
            {
                if (!_strings.TryGetValue(key, out value))
                    value = key; // fallback: no rompe, muestra la clave/literal
            }
            if (args != null && args.Length > 0)
            {
                try { value = string.Format(value, args); }
                catch { /* ignora errores de formato */ }
            }
            return value;
        }
    }
}
