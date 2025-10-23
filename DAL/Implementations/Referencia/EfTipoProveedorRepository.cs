using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;

namespace DAL.Implementations.Referencia
{
    public class EfTipoProveedorRepository : EfRepository<TipoProveedor>, ITipoProveedorRepository
    {
        public EfTipoProveedorRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        public IEnumerable<TipoProveedor> GetTiposOrdenados()
        {
            return _dbSet
                .OrderBy(tp => tp.TipoProveedorNombre)
                .ToList();
        }

        public async Task<IEnumerable<TipoProveedor>> GetTiposOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(tp => tp.TipoProveedorNombre)
                .ToListAsync();
        }
    }
}