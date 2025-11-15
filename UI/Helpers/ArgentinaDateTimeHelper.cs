using System;

namespace UI.Helpers
{
    /// <summary>
    /// Provee utilidades para manejar conversiones de fechas al huso horario de Argentina.
    /// </summary>
    internal static class ArgentinaDateTimeHelper
    {
        /// <summary>
        /// Zona horaria utilizada para las conversiones hacia Argentina.
        /// </summary>
        private static readonly TimeZoneInfo ArgentinaTimeZone = LoadArgentinaTimeZone();

        /// <summary>
        /// Obtiene la zona horaria de Argentina desde el sistema operativo o crea una equivalente.
        /// </summary>
        /// <returns>Instancia de <see cref="TimeZoneInfo"/> configurada para Argentina.</returns>
        private static TimeZoneInfo LoadArgentinaTimeZone()
        {
            var ids = new[] { "Argentina Standard Time", "America/Argentina/Buenos_Aires", "America/Buenos_Aires" };
            foreach (var id in ids)
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(id);
                }
                catch (TimeZoneNotFoundException)
                {
                }
                catch (InvalidTimeZoneException)
                {
                }
            }

            return TimeZoneInfo.CreateCustomTimeZone("UTC-3", TimeSpan.FromHours(-3), "UTC-3", "UTC-3");
        }

        /// <summary>
        /// Obtiene la fecha y hora actual convertida al horario de Argentina.
        /// </summary>
        /// <returns>Fecha y hora local de Argentina.</returns>
        public static DateTime Now()
        {
            return ToArgentina(DateTime.UtcNow);
        }

        /// <summary>
        /// Convierte una fecha al horario de Argentina respetando su tipo original.
        /// </summary>
        /// <param name="value">Fecha que se desea transformar.</param>
        /// <returns>Fecha representada en la zona horaria argentina.</returns>
        public static DateTime ToArgentina(DateTime value)
        {
            DateTime utc;
            switch (value.Kind)
            {
                case DateTimeKind.Utc:
                    utc = value;
                    break;
                case DateTimeKind.Local:
                    return value;
                default:
                    utc = DateTime.SpecifyKind(value, DateTimeKind.Utc);
                    break;
            }

            var local = TimeZoneInfo.ConvertTimeFromUtc(utc, ArgentinaTimeZone);
            return DateTime.SpecifyKind(local, DateTimeKind.Local);
        }

        /// <summary>
        /// Convierte una fecha opcional al horario de Argentina.
        /// </summary>
        /// <param name="value">Fecha opcional que se desea transformar.</param>
        /// <returns>Fecha convertida o <c>null</c> si no había valor.</returns>
        public static DateTime? ToArgentina(DateTime? value)
        {
            return value.HasValue ? ToArgentina(value.Value) : (DateTime?)null;
        }

        /// <summary>
        /// Convierte una fecha desde Argentina hacia formato UTC.
        /// </summary>
        /// <param name="value">Fecha en horario local.</param>
        /// <returns>Fecha transformada a UTC.</returns>
        public static DateTime ToUtc(DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
                return value;

            var baseValue = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(baseValue, ArgentinaTimeZone);
        }

        /// <summary>
        /// Convierte una fecha opcional desde Argentina a UTC.
        /// </summary>
        /// <param name="value">Fecha opcional a transformar.</param>
        /// <returns>Fecha en UTC o <c>null</c> si no se recibió valor.</returns>
        public static DateTime? ToUtc(DateTime? value)
        {
            return value.HasValue ? ToUtc(value.Value) : (DateTime?)null;
        }
    }
}