using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Interfaces.Base;
using DomainModel.Entidades;

namespace DAL.Interfaces.Referencia
{
    public interface IEstadoPedidoMuestraRepository : IRepository<EstadoPedidoMuestra>
    {
        IEnumerable<EstadoPedidoMuestra> GetEstadosOrdenados();
        Task<IEnumerable<EstadoPedidoMuestra>> GetEstadosOrdenadosAsync();
    }
}