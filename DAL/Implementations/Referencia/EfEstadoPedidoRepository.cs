using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;

namespace DAL.Implementations.Referencia
{
    public class EfEstadoPedidoRepository : EfRepository<EstadoPedido>, IEstadoPedidoRepository
    {
        /// <summary>
        /// Inicializa el repositorio de estados de pedido con el contexto de datos proporcionado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfEstadoPedidoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene los estados de pedido ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de pedido.</returns>
        public IEnumerable<EstadoPedido> GetEstadosOrdenados()
        {
            return _dbSet
                .OrderBy(e => e.NombreEstadoPedido)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los estados de pedido ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de estados de pedido.</returns>
        public async Task<IEnumerable<EstadoPedido>> GetEstadosOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(e => e.NombreEstadoPedido)
                .ToListAsync();
        }
    }
}