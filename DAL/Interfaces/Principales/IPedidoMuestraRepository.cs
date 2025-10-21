using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    public interface IPedidoMuestraRepository : IRepository<PedidoMuestra>
    {
        // Métodos específicos para PedidoMuestra
        IEnumerable<PedidoMuestra> GetMuestrasPorCliente(Guid idCliente);
        Task<IEnumerable<PedidoMuestra>> GetMuestrasPorClienteAsync(Guid idCliente);
        IEnumerable<PedidoMuestra> GetMuestrasPorEstado(Guid idEstado);
        Task<IEnumerable<PedidoMuestra>> GetMuestrasPorEstadoAsync(Guid idEstado);
        IEnumerable<PedidoMuestra> GetMuestrasVencidas();
        Task<IEnumerable<PedidoMuestra>> GetMuestrasVencidasAsync();
        PedidoMuestra GetMuestraConDetalles(Guid idPedidoMuestra);
        Task<PedidoMuestra> GetMuestraConDetallesAsync(Guid idPedidoMuestra);
    }
}
