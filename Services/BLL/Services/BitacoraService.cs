using Services.BLL.Interfaces;
using Services.DAL.Interfaces.Base;
using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Services.BLL.Services
{
    public class BitacoraService : IBitacoraService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Inicializa el servicio de bitácora con la unidad de trabajo provista.
        /// </summary>
        public BitacoraService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Registra una acción realizada por un usuario.
        /// </summary>
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

        /// <summary>
        /// Obtiene las últimas acciones registradas para un usuario.
        /// </summary>
        public IEnumerable<Bitacora> ObtenerUltimasAcciones(Guid idUsuario, int cantidad = 50)
        {
            return _unitOfWork.Bitacoras.GetBitacorasPorUsuario(idUsuario).Take(cantidad);
        }

        /// <summary>
        /// Obtiene los registros de bitácora dentro de un rango de fechas.
        /// </summary>
        public IEnumerable<Bitacora> ObtenerPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _unitOfWork.Bitacoras.GetBitacorasPorFecha(fechaDesde, fechaHasta);
        }

        /// <summary>
        /// Recupera los registros de bitácora que contienen errores.
        /// </summary>
        public IEnumerable<Bitacora> ObtenerErrores()
        {
            return _unitOfWork.Bitacoras.GetBitacorasConError();
        }
    }
}
