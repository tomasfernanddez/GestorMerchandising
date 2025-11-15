using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;

namespace Services.BLL.Interfaces
{
    public interface IBitacoraService
    {
        /// <summary>
        /// Registra una acción realizada por un usuario en la bitácora del sistema.
        /// </summary>
        void RegistrarAccion(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null);

        /// <summary>
        /// Obtiene las últimas acciones registradas para un usuario.
        /// </summary>
        IEnumerable<Bitacora> ObtenerUltimasAcciones(Guid idUsuario, int cantidad = 50);

        /// <summary>
        /// Lista las acciones registradas dentro de un rango de fechas.
        /// </summary>
        IEnumerable<Bitacora> ObtenerPorFecha(DateTime fechaDesde, DateTime fechaHasta);

        /// <summary>
        /// Obtiene únicamente los registros de la bitácora marcados como error.
        /// </summary>
        IEnumerable<Bitacora> ObtenerErrores();
    }
}
