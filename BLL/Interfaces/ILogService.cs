using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ILogService
    {
        void LogError(string mensaje, Exception ex = null, string modulo = null, string usuario = null);
        void LogInfo(string mensaje, string modulo = null, string usuario = null);
        void LogWarning(string mensaje, string modulo = null, string usuario = null);
        string[] ObtenerUltimosLogs(int cantidad = 100);
        void LimpiarLogsAntiguos(int diasAMantener = 30);
    }
}
