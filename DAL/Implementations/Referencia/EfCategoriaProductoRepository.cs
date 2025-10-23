using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL.Implementations.Referencia
{
    public class EfCategoriaProductoRepository : EfRepository<CategoriaProducto>, ICategoriaProductoRepository
    {
        public EfCategoriaProductoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene todas las categorías ordenadas alfabéticamente
        /// </summary>
        public IEnumerable<CategoriaProducto> GetCategoriasOrdenadas()
        {
            return _dbSet.OrderBy(c => c.NombreCategoria)
                        .ToList();
        }

        /// <summary>
        /// Obtiene todas las categorías ordenadas alfabéticamente (async)
        /// </summary>
        public async Task<IEnumerable<CategoriaProducto>> GetCategoriasOrdenadasAsync()
        {
            return await _dbSet.OrderBy(c => c.NombreCategoria)
                              .ToListAsync();
        }
    }
}
