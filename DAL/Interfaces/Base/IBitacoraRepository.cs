using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Base
{
    public interface IBitacoraRepository : IRepository<Bitacora>
    {
        // Métodos específicos para Bitácora
        IEnumerable<Bitacora> GetBitacorasPorUsuario(Guid idUsuario);
        Task<IEnumerable<Bitacora>> GetBitacorasPorUsuarioAsync(Guid idUsuario);

        IEnumerable<Bitacora> GetBitacorasPorFecha(DateTime fechaDesde, DateTime fechaHasta);
        Task<IEnumerable<Bitacora>> GetBitacorasPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta);

        IEnumerable<Bitacora> GetBitacorasPorModulo(string modulo);
        Task<IEnumerable<Bitacora>> GetBitacorasPorModuloAsync(string modulo);

        IEnumerable<Bitacora> GetBitacorasConError();
        Task<IEnumerable<Bitacora>> GetBitacorasConErrorAsync();

        void RegistrarAccion(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null);
    }
}
