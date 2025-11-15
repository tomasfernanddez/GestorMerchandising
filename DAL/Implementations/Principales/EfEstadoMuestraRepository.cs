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
        /// <summary>
        /// Inicializa el repositorio de estados de muestra con el contexto de datos proporcionado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfEstadoMuestraRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene los estados de muestra ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de muestra.</returns>
        public IEnumerable<EstadoMuestra> GetEstadosOrdenados()
        {
            return _dbSet
                .OrderBy(e => e.NombreEstadoMuestra)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los estados de muestra ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de muestra.</returns>
        public async Task<IEnumerable<EstadoMuestra>> GetEstadosOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(e => e.NombreEstadoMuestra)
                .ToListAsync();
        }
    }
}