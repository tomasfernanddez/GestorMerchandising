using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;

namespace Services.BLL.Interfaces
{
    public interface IBitacoraService
    {
        void RegistrarAccion(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null);
        IEnumerable<Bitacora> ObtenerUltimasAcciones(Guid idUsuario, int cantidad = 50);
        IEnumerable<Bitacora> ObtenerPorFecha(DateTime fechaDesde, DateTime fechaHasta);
        IEnumerable<Bitacora> ObtenerErrores();
    }
}
