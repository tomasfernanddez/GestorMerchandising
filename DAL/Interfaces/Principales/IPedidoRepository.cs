using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        // Métodos específicos para Pedido
        IEnumerable<Pedido> GetPedidosPorCliente(Guid idCliente);
        Task<IEnumerable<Pedido>> GetPedidosPorClienteAsync(Guid idCliente);
        IEnumerable<Pedido> GetPedidosPorEstado(Guid idEstado);
        Task<IEnumerable<Pedido>> GetPedidosPorEstadoAsync(Guid idEstado);
        IEnumerable<Pedido> GetPedidosPorFecha(DateTime fechaDesde, DateTime fechaHasta);
        Task<IEnumerable<Pedido>> GetPedidosPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta);
        IEnumerable<Pedido> GetPedidosConFechaLimite();
        Task<IEnumerable<Pedido>> GetPedidosConFechaLimiteAsync();
        IEnumerable<Pedido> GetPedidosVencidos();
        Task<IEnumerable<Pedido>> GetPedidosVencidosAsync();
        Pedido GetPedidoConDetalles(Guid idPedido);
        Task<Pedido> GetPedidoConDetallesAsync(Guid idPedido);
    }
}
