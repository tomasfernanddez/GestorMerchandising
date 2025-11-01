using BLL.Reportes;

namespace BLL.Interfaces
{
    public interface IReporteService
    {
        ReporteGeneralResult GenerarReportes(ReporteParametros parametros);
    }
}