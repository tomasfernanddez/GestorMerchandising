using Services.DomainModel.Entities;
using Services.DAL.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DAL.Interfaces.Base
{
    public interface IBitacoraRepository : IRepository<Bitacora>
    {
        // Métodos específicos para Bitácora
        /// <summary>
        /// Obtiene los registros de bitácora asociados a un usuario.
        /// </summary>
        IEnumerable<Bitacora> GetBitacorasPorUsuario(Guid idUsuario);

        /// <summary>
        /// Obtiene de forma asíncrona los registros de bitácora asociados a un usuario.
        /// </summary>
        Task<IEnumerable<Bitacora>> GetBitacorasPorUsuarioAsync(Guid idUsuario);

        /// <summary>
        /// Obtiene los registros de bitácora dentro del rango de fechas indicado.
        /// </summary>
        IEnumerable<Bitacora> GetBitacorasPorFecha(DateTime fechaDesde, DateTime fechaHasta);

        /// <summary>
        /// Obtiene de forma asíncrona los registros de bitácora dentro del rango de fechas indicado.
        /// </summary>
        Task<IEnumerable<Bitacora>> GetBitacorasPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta);

        /// <summary>
        /// Obtiene los registros de bitácora asociados a un módulo específico.
        /// </summary>
        IEnumerable<Bitacora> GetBitacorasPorModulo(string modulo);

        /// <summary>
        /// Obtiene de forma asíncrona los registros de bitácora asociados a un módulo específico.
        /// </summary>
        Task<IEnumerable<Bitacora>> GetBitacorasPorModuloAsync(string modulo);

        /// <summary>
        /// Obtiene los registros de bitácora marcados como error.
        /// </summary>
        IEnumerable<Bitacora> GetBitacorasConError();

        /// <summary>
        /// Obtiene de forma asíncrona los registros de bitácora marcados como error.
        /// </summary>
        Task<IEnumerable<Bitacora>> GetBitacorasConErrorAsync();

        /// <summary>
        /// Registra un evento en la bitácora.
        /// </summary>
        void RegistrarAccion(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null);
    }
}