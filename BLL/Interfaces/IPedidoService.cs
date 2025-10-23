using BLL.Helpers;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IPedidoService
    {
        /* Operaciones básicas */
        Pedido ObtenerPedidoPorId(Guid idPedido);
        Task<Pedido> ObtenerPedidoPorIdAsync(Guid idPedido);
        Pedido ObtenerPedidoCompleto(Guid idPedido);
        Task<Pedido> ObtenerPedidoCompletoAsync(Guid idPedido);
        IEnumerable<Pedido> ObtenerTodosLosPedidos();
        Task<IEnumerable<Pedido>> ObtenerTodosLosPedidosAsync();

        /* Búsquedas y filtros */
        IEnumerable<Pedido> ObtenerPedidosPorCliente(Guid idCliente);
        Task<IEnumerable<Pedido>> ObtenerPedidosPorClienteAsync(Guid idCliente);
        IEnumerable<Pedido> ObtenerPedidosPorEstado(Guid idEstado);
        Task<IEnumerable<Pedido>> ObtenerPedidosPorEstadoAsync(Guid idEstado);
        IEnumerable<Pedido> ObtenerPedidosPorFecha(DateTime fechaDesde, DateTime fechaHasta);
        Task<IEnumerable<Pedido>> ObtenerPedidosPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta);
        IEnumerable<Pedido> ObtenerPedidosConFechaLimite();
        Task<IEnumerable<Pedido>> ObtenerPedidosConFechaLimiteAsync();
        IEnumerable<Pedido> ObtenerPedidosVencidos();
        Task<IEnumerable<Pedido>> ObtenerPedidosVencidosAsync();
        Pedido ObtenerPedidoPorNumero(string numeroPedido);
        Task<Pedido> ObtenerPedidoPorNumeroAsync(string numeroPedido);

        /* Operaciones de modificación */
        ResultadoOperacion CrearPedido(Pedido pedido);
        Task<ResultadoOperacion> CrearPedidoAsync(Pedido pedido);
        ResultadoOperacion ActualizarPedido(Pedido pedido);
        Task<ResultadoOperacion> ActualizarPedidoAsync(Pedido pedido);
        ResultadoOperacion EliminarPedido(Guid idPedido);
        Task<ResultadoOperacion> EliminarPedidoAsync(Guid idPedido);

        /* Operaciones sobre detalles */
        ResultadoOperacion AgregarDetalle(Guid idPedido, PedidoDetalle detalle);
        Task<ResultadoOperacion> AgregarDetalleAsync(Guid idPedido, PedidoDetalle detalle);
        ResultadoOperacion ActualizarDetalle(PedidoDetalle detalle);
        Task<ResultadoOperacion> ActualizarDetalleAsync(PedidoDetalle detalle);
        ResultadoOperacion EliminarDetalle(Guid idDetalle);
        Task<ResultadoOperacion> EliminarDetalleAsync(Guid idDetalle);

        /* Facturación */
        ResultadoOperacion MarcarComoFacturado(Guid idPedido, string rutaPDF);
        Task<ResultadoOperacion> MarcarComoFacturadoAsync(Guid idPedido, string rutaPDF);

        /* Cálculos */
        decimal CalcularTotalPedido(Guid idPedido);
        Task<decimal> CalcularTotalPedidoAsync(Guid idPedido);
        decimal CalcularTotalConIVA(Guid idPedido);
        Task<decimal> CalcularTotalConIVAAsync(Guid idPedido);

        /* Métodos auxiliares */
        string GenerarNumeroPedido();
        Task<string> GenerarNumeroPedidoAsync();
        IEnumerable<EstadoPedido> ObtenerEstadosPedido();
        Task<IEnumerable<EstadoPedido>> ObtenerEstadosPedidoAsync();
        IEnumerable<EstadoProducto> ObtenerEstadosProducto();
        Task<IEnumerable<EstadoProducto>> ObtenerEstadosProductoAsync();
        IEnumerable<TipoPago> ObtenerTiposPago();
        Task<IEnumerable<TipoPago>> ObtenerTiposPagoAsync();
    }
}
