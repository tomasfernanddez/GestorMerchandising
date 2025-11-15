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
        /// <summary>
        /// Inicializa el repositorio de tipos de empresa con el contexto proporcionado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfTipoEmpresaRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene los tipos de empresa ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de tipos de empresa.</returns>
        public IEnumerable<TipoEmpresa> GetTiposOrdenados()
        {
            return _dbSet
                .OrderBy(te => te.TipoEmpresaNombre)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los tipos de empresa ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de tipos de empresa.</returns>
        public async Task<IEnumerable<TipoEmpresa>> GetTiposOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(te => te.TipoEmpresaNombre)
                .ToListAsync();
        }
    }
}