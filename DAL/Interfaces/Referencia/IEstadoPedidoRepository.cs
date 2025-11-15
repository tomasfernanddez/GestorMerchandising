using DAL.Interfaces.Base;
using DomainModel.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Referencia
{
    /// <summary>
    /// Define operaciones de acceso a datos para estados de pedido.
    /// </summary>
    public interface IEstadoPedidoRepository : IRepository<EstadoPedido>
    {
        /// <summary>
        /// Obtiene los estados de pedido ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de pedido.</returns>
        IEnumerable<EstadoPedido> GetEstadosOrdenados();

        /// <summary>
        /// Obtiene de forma asíncrona los estados de pedido ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de pedido.</returns>
        Task<IEnumerable<EstadoPedido>> GetEstadosOrdenadosAsync();
    }
}