using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BLL.Interfaces
{
    public interface ILogService
    {
        /// <summary>
        /// Registra un mensaje de error opcionalmente asociado a un módulo y usuario.
        /// </summary>
        void LogError(string mensaje, Exception ex = null, string modulo = null, string usuario = null);

        /// <summary>
        /// Registra un mensaje informativo en el log del sistema.
        /// </summary>
        void LogInfo(string mensaje, string modulo = null, string usuario = null);

        /// <summary>
        /// Registra un mensaje de advertencia en el log del sistema.
        /// </summary>
        void LogWarning(string mensaje, string modulo = null, string usuario = null);

        /// <summary>
        /// Obtiene los últimos registros del log.
        /// </summary>
        string[] ObtenerUltimosLogs(int cantidad = 100);

        /// <summary>
        /// Elimina registros antiguos del log conservando los más recientes.
        /// </summary>
        void LimpiarLogsAntiguos(int diasAMantener = 30);
    }
}
