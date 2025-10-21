using BLL.Interfaces;
using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class BitacoraService : IBitacoraService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BitacoraService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void RegistrarAccion(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null)
        {
            try
            {
                _unitOfWork.RegistrarAccion(idUsuario, accion, descripcion, modulo, exitoso, mensajeError, direccionIP);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error registrando en bitácora: {ex.Message}");
            }
        }

        public IEnumerable<Bitacora> ObtenerUltimasAcciones(Guid idUsuario, int cantidad = 50)
        {
            return _unitOfWork.Bitacoras.GetBitacorasPorUsuario(idUsuario).Take(cantidad);
        }

        public IEnumerable<Bitacora> ObtenerPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _unitOfWork.Bitacoras.GetBitacorasPorFecha(fechaDesde, fechaHasta);
        }

        public IEnumerable<Bitacora> ObtenerErrores()
        {
            return _unitOfWork.Bitacoras.GetBitacorasConError();
        }
    }
}
