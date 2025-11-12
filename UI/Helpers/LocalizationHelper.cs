using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Services.DomainModel.Entities;
using UI.Localization;

namespace UI.Helpers
{
    public static class LocalizationHelper
    {
        public static string TranslateProfileName(string nombrePerfil)
            => TranslateFlexible("profile.name", nombrePerfil);

        public static string TranslateProfileDescription(string descripcion)
            => TranslateFlexible("profile.description", descripcion);

        public static string TranslateFunctionName(Funcion funcion)
        {
            if (funcion == null)
                return string.Empty;

            // Prefer código para generar la clave de traducción
            var codeKey = string.IsNullOrWhiteSpace(funcion.Codigo)
                ? null
                : $"function.code.{Normalize(funcion.Codigo)}";

            if (!string.IsNullOrWhiteSpace(codeKey))
            {
                var translated = codeKey.Traducir();
                if (!string.Equals(translated, codeKey, StringComparison.OrdinalIgnoreCase))
                    return translated;
            }

            return TranslateFlexible("function.name", funcion.Nombre);
        }

        public static string TranslateModule(string modulo)
            => TranslateFlexible("log.module", modulo);

        public static string TranslateAction(string accion)
        {
            if (string.IsNullOrWhiteSpace(accion))
                return string.Empty;

            accion = accion.Trim();

            // Intento directo: la acción ya puede ser una clave completa
            var direct = accion.Traducir();
            if (!string.Equals(direct, accion, StringComparison.OrdinalIgnoreCase))
                return direct;

            var normalized = Normalize(accion.Replace('.', '_'));
            var prefixed = $"log.action.{normalized}";
            var translated = prefixed.Traducir();
            if (!string.Equals(translated, prefixed, StringComparison.OrdinalIgnoreCase))
                return translated;

            var segments = accion.Split(new[] { '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length == 0)
                return accion;

            var translatedSegments = segments
                .Select(s => new
                {
                    Original = s,
                    Translated = TranslateFlexible("log.segment", s)
                })
                .ToArray();

            // Si todos los segmentos vuelven sin traducción, retornar la acción original
            if (translatedSegments.All(pair => string.Equals(pair.Original, pair.Translated, StringComparison.OrdinalIgnoreCase)))
            {
                return accion;
            }

            return string.Join(" ", translatedSegments.Select(p => p.Translated));
        }

        public static string TranslateDescription(string descripcion)
            => TranslateFlexible("log.description", descripcion);

        public static string TranslateError(string mensajeError)
            => TranslateFlexible("log.error", mensajeError);

        public static string TranslateBoolean(bool value)
            => value ? "common.yes".Traducir() : "common.no".Traducir();

        public static string TranslateOrderState(string estado)
            => TranslateFlexible("order.state", estado);

        public static string TranslateSampleOrderState(string estado)
            => TranslateFlexible("sampleOrder.state", estado);

        private static string TranslateFlexible(string prefix, string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
                return string.Empty;

            var trimmed = rawValue.Trim();

            // Intento directo con el valor recibido como clave
            var direct = trimmed.Traducir();
            if (!string.Equals(direct, trimmed, StringComparison.OrdinalIgnoreCase))
                return direct;

            // Intento con prefijo + valor normalizado
            var normalized = Normalize(trimmed);
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                var prefixed = $"{prefix}.{normalized}";
                var translated = prefixed.Traducir();
                if (!string.Equals(translated, prefixed, StringComparison.OrdinalIgnoreCase))
                    return translated;
            }

            return trimmed;
        }

        private static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var builder = new StringBuilder();
            var normalized = value
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormD);

            foreach (var ch in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (category == UnicodeCategory.NonSpacingMark)
                    continue;

                if (char.IsLetterOrDigit(ch))
                {
                    builder.Append(ch);
                }
                else if (char.IsWhiteSpace(ch) || ch == '.' || ch == '-' || ch == '_' || ch == '/')
                {
                    builder.Append('.');
                }
            }

            var result = builder.ToString();
            while (result.Contains(".."))
            {
                result = result.Replace("..", ".");
            }

            return result.Trim('.');
        }
    }
}