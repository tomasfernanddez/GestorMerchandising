using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Interfaces.Base;
using DomainModel.Entidades;

namespace DAL.Interfaces.Referencia
{
    /// <summary>
    /// Define operaciones de acceso a datos para estados de muestra.
    /// </summary>
    public interface IEstadoMuestraRepository : IRepository<EstadoMuestra>
    {
        /// <summary>
        /// Obtiene los estados de muestra ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de muestra.</returns>
        IEnumerable<EstadoMuestra> GetEstadosOrdenados();

        /// <summary>
        /// Obtiene de forma asíncrona los estados de muestra ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de muestra.</returns>
        Task<IEnumerable<EstadoMuestra>> GetEstadosOrdenadosAsync();
    }
}