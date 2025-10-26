using System;

namespace UI.Helpers
{
    internal static class ArgentinaDateTimeHelper
    {
        private static readonly TimeZoneInfo ArgentinaTimeZone = LoadArgentinaTimeZone();

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

        public static DateTime Now()
        {
            return ToArgentina(DateTime.UtcNow);
        }

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

        public static DateTime? ToArgentina(DateTime? value)
        {
            return value.HasValue ? ToArgentina(value.Value) : (DateTime?)null;
        }
    }
}