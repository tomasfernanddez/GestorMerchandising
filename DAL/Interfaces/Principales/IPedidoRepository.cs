using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    /// <summary>
    /// Define operaciones de acceso a datos especializadas para pedidos comerciales.
    /// </summary>
    public interface IPedidoRepository : IRepository<Pedido>
    {
        /// <summary>
        /// Obtiene los pedidos asociados a un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Colección de pedidos del cliente.</returns>
        IEnumerable<Pedido> GetPedidosPorCliente(Guid idCliente);

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos asociados a un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Colección de pedidos del cliente.</returns>
        Task<IEnumerable<Pedido>> GetPedidosPorClienteAsync(Guid idCliente);

        /// <summary>
        /// Obtiene los pedidos que se encuentran en un estado determinado.
        /// </summary>
        /// <param name="idEstado">Identificador del estado del pedido.</param>
        /// <returns>Colección de pedidos filtrados.</returns>
        IEnumerable<Pedido> GetPedidosPorEstado(Guid idEstado);

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos que se encuentran en un estado determinado.
        /// </summary>
        /// <param name="idEstado">Identificador del estado del pedido.</param>
        /// <returns>Colección de pedidos filtrados.</returns>
        Task<IEnumerable<Pedido>> GetPedidosPorEstadoAsync(Guid idEstado);

        /// <summary>
        /// Obtiene los pedidos creados dentro de un rango de fechas.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del rango.</param>
        /// <param name="fechaHasta">Fecha final del rango.</param>
        /// <returns>Colección de pedidos en el periodo.</returns>
        IEnumerable<Pedido> GetPedidosPorFecha(DateTime fechaDesde, DateTime fechaHasta);

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos creados dentro de un rango de fechas.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del rango.</param>
        /// <param name="fechaHasta">Fecha final del rango.</param>
        /// <returns>Colección de pedidos en el periodo.</returns>
        Task<IEnumerable<Pedido>> GetPedidosPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta);

        /// <summary>
        /// Obtiene los pedidos que poseen fecha límite de entrega configurada.
        /// </summary>
        /// <returns>Pedidos con fecha límite vigente.</returns>
        IEnumerable<Pedido> GetPedidosConFechaLimite();

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos que poseen fecha límite de entrega configurada.
        /// </summary>
        /// <returns>Pedidos con fecha límite vigente.</returns>
        Task<IEnumerable<Pedido>> GetPedidosConFechaLimiteAsync();

        /// <summary>
        /// Obtiene los pedidos cuya fecha límite ya venció.
        /// </summary>
        /// <returns>Pedidos vencidos.</returns>
        IEnumerable<Pedido> GetPedidosVencidos();

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos cuya fecha límite ya venció.
        /// </summary>
        /// <returns>Pedidos vencidos.</returns>
        Task<IEnumerable<Pedido>> GetPedidosVencidosAsync();

        /// <summary>
        /// Obtiene un pedido con todas sus relaciones de navegación cargadas.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Pedido encontrado o null.</returns>
        Pedido GetPedidoConDetalles(Guid idPedido);

        /// <summary>
        /// Obtiene de forma asíncrona un pedido con todas sus relaciones de navegación cargadas.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Pedido encontrado o null.</returns>
        Task<Pedido> GetPedidoConDetallesAsync(Guid idPedido);
    }
}