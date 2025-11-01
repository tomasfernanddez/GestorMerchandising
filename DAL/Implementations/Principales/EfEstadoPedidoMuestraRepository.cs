using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;

namespace DAL.Implementations.Referencia
{
    public class EfEstadoPedidoMuestraRepository : EfRepository<EstadoPedidoMuestra>, IEstadoPedidoMuestraRepository
    {
        public EfEstadoPedidoMuestraRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        public IEnumerable<EstadoPedidoMuestra> GetEstadosOrdenados()
        {
            return _dbSet
                .OrderBy(e => e.NombreEstadoPedidoMuestra)
                .ToList();
        }

        public async Task<IEnumerable<EstadoPedidoMuestra>> GetEstadosOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(e => e.NombreEstadoPedidoMuestra)
                .ToListAsync();
        }
    }
}