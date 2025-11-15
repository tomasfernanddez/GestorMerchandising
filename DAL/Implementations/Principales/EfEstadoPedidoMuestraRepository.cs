using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;

namespace DAL.Implementations.Referencia
{
    public class EfEstadoPedidoMuestraRepository : EfRepository<EstadoPedidoMuestra>, IEstadoPedidoMuestraRepository
    {
        /// <summary>
        /// Inicializa el repositorio de estados de pedido de muestra con el contexto de datos proporcionado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfEstadoPedidoMuestraRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene los estados de pedido de muestra ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de pedido de muestra.</returns>
        public IEnumerable<EstadoPedidoMuestra> GetEstadosOrdenados()
        {
            return _dbSet
                .OrderBy(e => e.NombreEstadoPedidoMuestra)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los estados de pedido de muestra ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de pedido de muestra.</returns>
        public async Task<IEnumerable<EstadoPedidoMuestra>> GetEstadosOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(e => e.NombreEstadoPedidoMuestra)
                .ToListAsync();
        }
    }
}