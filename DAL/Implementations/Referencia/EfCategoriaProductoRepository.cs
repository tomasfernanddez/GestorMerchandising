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
        /// <summary>
        /// Inicializa el repositorio de categorías de producto con el contexto indicado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfCategoriaProductoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene las categorías de producto ordenadas por estado, orden y nombre.
        /// </summary>
        /// <returns>Colección de categorías de producto.</returns>
        public IEnumerable<CategoriaProducto> GetCategoriasOrdenadas()
        {
            return _dbSet
                .OrderByDescending(c => c.Activo)
                .ThenBy(c => c.Orden)
                .ThenBy(c => c.NombreCategoria)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona las categorías de producto ordenadas por estado, orden y nombre.
        /// </summary>
        /// <returns>Colección de categorías de producto.</returns>
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