using System;

namespace Services.BLL.Models
{
    public class BackupFileInfo
    {
        public string Nombre { get; set; }
        public string RutaCompleta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public long TamanoBytes { get; set; }

        public string TamanoLegible()
        {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;

            if (TamanoBytes >= GB)
            {
                return string.Format("{0:0.##} GB", (double)TamanoBytes / GB);
            }
            if (TamanoBytes >= MB)
            {
                return string.Format("{0:0.##} MB", (double)TamanoBytes / MB);
            }
            if (TamanoBytes >= KB)
            {
                return string.Format("{0:0.##} KB", (double)TamanoBytes / KB);
            }

            return string.Format("{0} B", TamanoBytes);
        }
    }
}