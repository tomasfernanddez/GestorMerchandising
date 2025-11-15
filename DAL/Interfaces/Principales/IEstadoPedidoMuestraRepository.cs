using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Interfaces.Base;
using DomainModel.Entidades;

namespace DAL.Interfaces.Referencia
{
    /// <summary>
    /// Define operaciones de acceso a datos para estados de pedidos de muestra.
    /// </summary>
    public interface IEstadoPedidoMuestraRepository : IRepository<EstadoPedidoMuestra>
    {
        /// <summary>
        /// Obtiene los estados de pedido de muestra ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de pedido de muestra.</returns>
        IEnumerable<EstadoPedidoMuestra> GetEstadosOrdenados();

        /// <summary>
        /// Obtiene de forma asíncrona los estados de pedido de muestra ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de pedido de muestra.</returns>
        Task<IEnumerable<EstadoPedidoMuestra>> GetEstadosOrdenadosAsync();
    }
}