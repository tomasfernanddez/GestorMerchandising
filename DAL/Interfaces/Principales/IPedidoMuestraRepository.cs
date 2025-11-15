using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    /// <summary>
    /// Define operaciones de acceso a datos especializadas para pedidos de muestra.
    /// </summary>
    public interface IPedidoMuestraRepository : IRepository<PedidoMuestra>
    {
        /// <summary>
        /// Obtiene los pedidos de muestra asociados a un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Colección de pedidos de muestra.</returns>
        IEnumerable<PedidoMuestra> GetMuestrasPorCliente(Guid idCliente);

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos de muestra asociados a un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Colección de pedidos de muestra.</returns>
        Task<IEnumerable<PedidoMuestra>> GetMuestrasPorClienteAsync(Guid idCliente);

        /// <summary>
        /// Obtiene los pedidos de muestra que se encuentran en un estado determinado.
        /// </summary>
        /// <param name="idEstado">Identificador del estado del pedido de muestra.</param>
        /// <returns>Colección de pedidos de muestra filtrados.</returns>
        IEnumerable<PedidoMuestra> GetMuestrasPorEstado(Guid idEstado);

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos de muestra que se encuentran en un estado determinado.
        /// </summary>
        /// <param name="idEstado">Identificador del estado del pedido de muestra.</param>
        /// <returns>Colección de pedidos de muestra filtrados.</returns>
        Task<IEnumerable<PedidoMuestra>> GetMuestrasPorEstadoAsync(Guid idEstado);

        /// <summary>
        /// Obtiene los pedidos de muestra cuya fecha esperada de devolución ya venció.
        /// </summary>
        /// <returns>Pedidos de muestra vencidos.</returns>
        IEnumerable<PedidoMuestra> GetMuestrasVencidas();

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos de muestra cuya fecha esperada de devolución ya venció.
        /// </summary>
        /// <returns>Pedidos de muestra vencidos.</returns>
        Task<IEnumerable<PedidoMuestra>> GetMuestrasVencidasAsync();

        /// <summary>
        /// Obtiene un pedido de muestra con todas sus relaciones cargadas.
        /// </summary>
        /// <param name="idPedidoMuestra">Identificador del pedido de muestra.</param>
        /// <returns>Pedido de muestra encontrado o null.</returns>
        PedidoMuestra GetMuestraConDetalles(Guid idPedidoMuestra);

        /// <summary>
        /// Obtiene de forma asíncrona un pedido de muestra con todas sus relaciones cargadas.
        /// </summary>
        /// <param name="idPedidoMuestra">Identificador del pedido de muestra.</param>
        /// <returns>Pedido de muestra encontrado o null.</returns>
        Task<PedidoMuestra> GetMuestraConDetallesAsync(Guid idPedidoMuestra);
    }
}
