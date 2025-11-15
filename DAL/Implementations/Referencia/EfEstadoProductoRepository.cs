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
        /// <summary>
        /// Inicializa el repositorio de estados de producto con el contexto de datos proporcionado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfEstadoProductoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene los estados de producto ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de producto.</returns>
        public IEnumerable<EstadoProducto> GetEstadosOrdenados()
        {
            return _dbSet
                .OrderBy(e => e.NombreEstadoProducto)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los estados de producto ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de producto.</returns>
        public async Task<IEnumerable<EstadoProducto>> GetEstadosOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(e => e.NombreEstadoProducto)
                .ToListAsync();
        }
    }
}