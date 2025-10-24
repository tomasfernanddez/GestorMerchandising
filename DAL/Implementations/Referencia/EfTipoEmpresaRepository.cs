using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;

namespace DAL.Implementations.Referencia
{
    public class EfTipoEmpresaRepository : EfRepository<TipoEmpresa>, ITipoEmpresaRepository
    {
        public EfTipoEmpresaRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        public IEnumerable<TipoEmpresa> GetTiposOrdenados()
        {
            return _dbSet
                .OrderBy(te => te.TipoEmpresaNombre)
                .ToList();
        }

        public async Task<IEnumerable<TipoEmpresa>> GetTiposOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(te => te.TipoEmpresaNombre)
                .ToListAsync();
        }
    }
}