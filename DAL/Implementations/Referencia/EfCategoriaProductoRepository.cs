using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;

namespace DAL.Implementations.Referencia
{
    public class EfCategoriaProductoRepository : EfRepository<CategoriaProducto>, ICategoriaProductoRepository
    {
        public EfCategoriaProductoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        public IEnumerable<CategoriaProducto> GetCategoriasOrdenadas()
        {
            return _dbSet
                .OrderByDescending(c => c.Activo)
                .ThenBy(c => c.Orden)
                .ThenBy(c => c.NombreCategoria)
                .ToList();
        }

        public async Task<IEnumerable<CategoriaProducto>> GetCategoriasOrdenadasAsync()
        {
            return await _dbSet
                .OrderByDescending(c => c.Activo)
                .ThenBy(c => c.Orden)
                .ThenBy(c => c.NombreCategoria)
                .ToListAsync();
        }
    }
}