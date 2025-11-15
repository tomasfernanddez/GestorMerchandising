using DAL.Interfaces.Base;
using DomainModel.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Referencia
{
    /// <summary>
    /// Define operaciones de acceso a datos para estados de producto.
    /// </summary>
    public interface IEstadoProductoRepository : IRepository<EstadoProducto>
    {
        /// <summary>
        /// Obtiene los estados de producto ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de producto.</returns>
        IEnumerable<EstadoProducto> GetEstadosOrdenados();

        /// <summary>
        /// Obtiene de forma asíncrona los estados de producto ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de producto.</returns>
        Task<IEnumerable<EstadoProducto>> GetEstadosOrdenadosAsync();
    }
}