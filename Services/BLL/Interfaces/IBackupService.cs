using System.Collections.Generic;
using Services.BLL.Models;

namespace Services.BLL.Interfaces
{
    public interface IBackupService
    {
        /// <summary>
        /// Genera un backup de la base de datos en la ruta indicada.
        /// </summary>
        /// <param name="rutaDestino">Ruta completa del archivo .bak. Si es null se usa el directorio por defecto.</param>
        /// <returns>Ruta completa del backup generado.</returns>
        string RealizarBackup(string rutaDestino = null);

        /// <summary>
        /// Restaura la base de datos a partir del archivo indicado.
        /// </summary>
        /// <param name="rutaArchivo">Ruta completa del archivo .bak a restaurar.</param>
        void RestaurarBackup(string rutaArchivo);

        /// <summary>
        /// Devuelve la lista de backups disponibles en el directorio configurado.
        /// </summary>
        IEnumerable<BackupFileInfo> ListarBackups();

        /// <summary>
        /// Directorio raíz donde se almacenan los backups.
        /// </summary>
        string ObtenerDirectorioBackups();

        /// <summary>
        /// Nombre sugerido para un nuevo archivo de backup.
        /// </summary>
        string GenerarNombreSugerido();
    }
}