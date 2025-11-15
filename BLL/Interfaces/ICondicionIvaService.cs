using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICondicionIvaService
    {
        /// <summary>
        /// Obtiene todas.
        /// </summary>
        IEnumerable<CondicionIva> ObtenerTodas();
        /// <summary>
        /// Obtiene asincrónicamente todas.
        /// </summary>
        Task<IEnumerable<CondicionIva>> ObtenerTodasAsync();
        /// <summary>
        /// Obtiene por id.
        /// </summary>
        CondicionIva ObtenerPorId(Guid id);
    }
}