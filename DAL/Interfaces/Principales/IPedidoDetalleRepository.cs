using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    /// <summary>
    /// Define operaciones de acceso a datos especializadas para los detalles de pedidos.
    /// </summary>
    public interface IPedidoDetalleRepository : IRepository<PedidoDetalle>
    {
        /// <summary>
        /// Obtiene los detalles correspondientes a un pedido específico.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Colección de detalles del pedido.</returns>
        IEnumerable<PedidoDetalle> GetDetallesPorPedido(Guid idPedido);

        /// <summary>
        /// Obtiene de forma asíncrona los detalles correspondientes a un pedido específico.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Colección de detalles del pedido.</returns>
        Task<IEnumerable<PedidoDetalle>> GetDetallesPorPedidoAsync(Guid idPedido);

        /// <summary>
        /// Obtiene los detalles donde interviene un producto determinado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <returns>Colección de detalles filtrados.</returns>
        IEnumerable<PedidoDetalle> GetDetallesPorProducto(Guid idProducto);

        /// <summary>
        /// Obtiene de forma asíncrona los detalles donde interviene un producto determinado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <returns>Colección de detalles filtrados.</returns>
        Task<IEnumerable<PedidoDetalle>> GetDetallesPorProductoAsync(Guid idProducto);

        /// <summary>
        /// Obtiene los detalles que se encuentran en un estado de producción específico.
        /// </summary>
        /// <param name="idEstadoProducto">Identificador del estado del producto.</param>
        /// <returns>Colección de detalles filtrados.</returns>
        IEnumerable<PedidoDetalle> GetDetallesPorEstado(Guid idEstadoProducto);

        /// <summary>
        /// Obtiene de forma asíncrona los detalles que se encuentran en un estado de producción específico.
        /// </summary>
        /// <param name="idEstadoProducto">Identificador del estado del producto.</param>
        /// <returns>Colección de detalles filtrados.</returns>
        Task<IEnumerable<PedidoDetalle>> GetDetallesPorEstadoAsync(Guid idEstadoProducto);

        /// <summary>
        /// Calcula el total monetario correspondiente a un pedido.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Monto total del pedido.</returns>
        decimal GetTotalPedido(Guid idPedido);

        /// <summary>
        /// Calcula de forma asíncrona el total monetario correspondiente a un pedido.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Monto total del pedido.</returns>
        Task<decimal> GetTotalPedidoAsync(Guid idPedido);
    }
}