using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;

namespace DAL.Implementations.Referencia
{
    public class EfEstadoProductoRepository : EfRepository<EstadoProducto>, IEstadoProductoRepository
    {
        public EfEstadoProductoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        public IEnumerable<EstadoProducto> GetEstadosOrdenados()
        {
            return _dbSet
                .OrderBy(e => e.NombreEstadoProducto)
                .ToList();
        }

        public async Task<IEnumerable<EstadoProducto>> GetEstadosOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(e => e.NombreEstadoProducto)
                .ToListAsync();
        }
    }
}