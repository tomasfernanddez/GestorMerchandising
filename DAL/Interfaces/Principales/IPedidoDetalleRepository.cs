using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    public interface IPedidoDetalleRepository : IRepository<PedidoDetalle>
    {
        // Métodos específicos para PedidoDetalle
        IEnumerable<PedidoDetalle> GetDetallesPorPedido(Guid idPedido);
        Task<IEnumerable<PedidoDetalle>> GetDetallesPorPedidoAsync(Guid idPedido);
        IEnumerable<PedidoDetalle> GetDetallesPorProducto(Guid idProducto);
        Task<IEnumerable<PedidoDetalle>> GetDetallesPorProductoAsync(Guid idProducto);
        IEnumerable<PedidoDetalle> GetDetallesPorEstado(Guid idEstadoProducto);
        Task<IEnumerable<PedidoDetalle>> GetDetallesPorEstadoAsync(Guid idEstadoProducto);
        decimal GetTotalPedido(Guid idPedido);
        Task<decimal> GetTotalPedidoAsync(Guid idPedido);
    }
}
