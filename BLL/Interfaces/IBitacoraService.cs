using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBitacoraService
    {
        void RegistrarAccion(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null);
        IEnumerable<Bitacora> ObtenerUltimasAcciones(Guid idUsuario, int cantidad = 50);
        IEnumerable<Bitacora> ObtenerPorFecha(DateTime fechaDesde, DateTime fechaHasta);
        IEnumerable<Bitacora> ObtenerErrores();
    }
}
