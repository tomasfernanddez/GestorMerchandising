using System;

namespace Services.BLL.Models
{
    /// <summary>
    /// Describe un archivo de copia de seguridad generado por la aplicación.
    /// </summary>
    public class BackupFileInfo
    {
        /// <summary>
        /// Nombre del archivo de respaldo.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Ruta absoluta donde se almacena el archivo.
        /// </summary>
        public string RutaCompleta { get; set; }
        /// <summary>
        /// Fecha de creación del respaldo.
        /// </summary>
        public DateTime FechaCreacion { get; set; }
        /// <summary>
        /// Tamaño del archivo en bytes.
        /// </summary>
        public long TamanoBytes { get; set; }

        /// <summary>
        /// Obtiene una representación legible del tamaño del archivo.
        /// </summary>
        /// <returns>Cadena con el tamaño formateado en unidades legibles.</returns>

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