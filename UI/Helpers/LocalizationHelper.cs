using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Services.DomainModel.Entities;
using UI.Localization;

namespace UI.Helpers
{
    /// <summary>
    /// Provee funciones de ayuda para traducir textos de dominios específicos de la aplicación.
    /// </summary>
    public static class LocalizationHelper
    {
        /// <summary>
        /// Traduce el nombre de un perfil utilizando claves amigables.
        /// </summary>
        /// <param name="nombrePerfil">Nombre original del perfil.</param>
        /// <returns>Nombre traducido si existe, caso contrario el valor original.</returns>
        public static string TranslateProfileName(string nombrePerfil)
            => TranslateFlexible("profile.name", nombrePerfil);

        /// <summary>
        /// Traduce la descripción de un perfil.
        /// </summary>
        /// <param name="descripcion">Descripción original.</param>
        /// <returns>Descripción traducida o el valor original si no hay traducción.</returns>
        public static string TranslateProfileDescription(string descripcion)
            => TranslateFlexible("profile.description", descripcion);

        /// <summary>
        /// Traduce el nombre de una función teniendo en cuenta su código y descripción.
        /// </summary>
        /// <param name="funcion">Función cuya descripción debe traducirse.</param>
        /// <returns>Texto traducido asociado a la función.</returns>
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

        /// <summary>
        /// Traduce el nombre de un módulo para los reportes de auditoría.
        /// </summary>
        /// <param name="modulo">Nombre del módulo a traducir.</param>
        /// <returns>Nombre traducido del módulo.</returns>
        public static string TranslateModule(string modulo)
            => TranslateFlexible("log.module", modulo);

        /// <summary>
        /// Traduce la acción registrada en la bitácora.
        /// </summary>
        /// <param name="accion">Acción registrada.</param>
        /// <returns>Descripción traducida de la acción.</returns>
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

        /// <summary>
        /// Traduce la descripción almacenada en la bitácora.
        /// </summary>
        /// <param name="descripcion">Texto original.</param>
        /// <returns>Texto traducido si existe.</returns>
        public static string TranslateDescription(string descripcion)
            => TranslateFlexible("log.description", descripcion);

        /// <summary>
        /// Traduce el mensaje de error registrado.
        /// </summary>
        /// <param name="mensajeError">Mensaje original.</param>
        /// <returns>Mensaje traducido.</returns>
        public static string TranslateError(string mensajeError)
            => TranslateFlexible("log.error", mensajeError);

        /// <summary>
        /// Traduce un valor booleano a los textos localizados de "sí" o "no".
        /// </summary>
        /// <param name="value">Valor booleano a convertir.</param>
        /// <returns>Cadena traducida correspondiente.</returns>
        public static string TranslateBoolean(bool value)
            => value ? "common.yes".Traducir() : "common.no".Traducir();

        /// <summary>
        /// Traduce un estado de pedido comercial.
        /// </summary>
        /// <param name="estado">Estado sin traducir.</param>
        /// <returns>Texto traducido del estado.</returns>
        public static string TranslateOrderState(string estado)
            => TranslateFlexible("order.state", estado);

        /// <summary>
        /// Traduce un estado de pedido de muestra.
        /// </summary>
        /// <param name="estado">Estado original.</param>
        /// <returns>Cadena traducida para el estado de muestra.</returns>
        public static string TranslateSampleOrderState(string estado)
            => TranslateFlexible("sampleOrder.state", estado);

        /// <summary>
        /// Aplica diferentes estrategias de traducción para un valor determinado.
        /// </summary>
        /// <param name="prefix">Prefijo utilizado para construir la clave de traducción.</param>
        /// <param name="rawValue">Valor original que se intenta traducir.</param>
        /// <returns>Cadena traducida o el valor original si no se encontró traducción.</returns>
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

        /// <summary>
        /// Normaliza una cadena para generar claves de traducción consistentes.
        /// </summary>
        /// <param name="value">Texto a normalizar.</param>
        /// <returns>Cadena en minúsculas sin acentos ni caracteres especiales.</returns>
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