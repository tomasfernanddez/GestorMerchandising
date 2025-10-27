using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;

namespace DAL.Implementations.Referencia
{
    public class EfEstadoMuestraRepository : EfRepository<EstadoMuestra>, IEstadoMuestraRepository
    {
        public EfEstadoMuestraRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        public IEnumerable<EstadoMuestra> GetEstadosOrdenados()
        {
            return _dbSet
                .OrderBy(e => e.NombreEstadoMuestra)
                .ToList();
        }

        public async Task<IEnumerable<EstadoMuestra>> GetEstadosOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(e => e.NombreEstadoMuestra)
                .ToListAsync();
        }
    }
}