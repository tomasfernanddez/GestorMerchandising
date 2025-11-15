using BLL.Interfaces;
using DAL.Interfaces.Base;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CondicionIvaService : ICondicionIvaService
    {
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Inicializa una nueva instancia de CondicionIvaService.
        /// </summary>
        public CondicionIvaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Obtiene por id.
        /// </summary>
        public CondicionIva ObtenerPorId(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return _unitOfWork.CondicionesIva.GetById(id);
        }
        /// <summary>
        /// Obtiene todas.
        /// </summary>
        public IEnumerable<CondicionIva> ObtenerTodas()
        {
            return _unitOfWork.CondicionesIva.ObtenerTodas() ?? Enumerable.Empty<CondicionIva>();
        }

        /// <summary>
        /// Obtiene asincrónicamente todas.
        /// </summary>
        public async Task<IEnumerable<CondicionIva>> ObtenerTodasAsync()
        {
            var resultado = await _unitOfWork.CondicionesIva.ObtenerTodasAsync();
            return resultado ?? Enumerable.Empty<CondicionIva>();
        }
    }
}