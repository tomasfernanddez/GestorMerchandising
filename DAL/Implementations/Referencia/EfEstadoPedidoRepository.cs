using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;

namespace DAL.Implementations.Referencia
{
    public class EfEstadoPedidoRepository : EfRepository<EstadoPedido>, IEstadoPedidoRepository
    {
        public EfEstadoPedidoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        public IEnumerable<EstadoPedido> GetEstadosOrdenados()
        {
            return _dbSet
                .OrderBy(e => e.NombreEstadoPedido)
                .ToList();
        }

        public async Task<IEnumerable<EstadoPedido>> GetEstadosOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(e => e.NombreEstadoPedido)
                .ToListAsync();
        }
    }
}