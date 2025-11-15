using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Principales;
using DomainModel;

namespace DAL.Implementations.Principales
{
    public class EfPedidoMuestraRepository : EfRepository<PedidoMuestra>, IPedidoMuestraRepository
    {
        /// <summary>
        /// Inicializa el repositorio de pedidos de muestra con el contexto de datos proporcionado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfPedidoMuestraRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Construye la consulta base incluyendo las relaciones necesarias para un pedido de muestra.
        /// </summary>
        /// <returns>Consulta reutilizable para operaciones del repositorio.</returns>
        private IQueryable<PedidoMuestra> QueryBase()
        {
            return _dbSet
                .Include(p => p.Cliente)
                .Include(p => p.EstadoPedidoMuestra)
                .Include(p => p.Detalles.Select(d => d.Producto))
                .Include(p => p.Detalles.Select(d => d.EstadoMuestra))
                .Include(p => p.Adjuntos)
                .Include(p => p.Pagos);
        }

        /// <summary>
        /// Obtiene todos los pedidos de muestra ordenados por fecha de creación.
        /// </summary>
        /// <returns>Colección completa de pedidos de muestra.</returns>
        public override IEnumerable<PedidoMuestra> GetAll()
        {
            return QueryBase()
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona todos los pedidos de muestra ordenados por fecha de creación.
        /// </summary>
        /// <returns>Colección completa de pedidos de muestra.</returns>
        public override async Task<IEnumerable<PedidoMuestra>> GetAllAsync()
        {
            return await QueryBase()
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Recupera los pedidos de muestra asociados a un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Pedidos de muestra correspondientes al cliente.</returns>
        public IEnumerable<PedidoMuestra> GetMuestrasPorCliente(Guid idCliente)
        {
            return QueryBase()
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        /// <summary>
        /// Recupera de forma asíncrona los pedidos de muestra asociados a un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Pedidos de muestra correspondientes al cliente.</returns>
        public async Task<IEnumerable<PedidoMuestra>> GetMuestrasPorClienteAsync(Guid idCliente)
        {
            return await QueryBase()
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los pedidos de muestra que se encuentran en un estado determinado.
        /// </summary>
        /// <param name="idEstado">Identificador del estado del pedido de muestra.</param>
        /// <returns>Pedidos de muestra que coinciden con el estado.</returns>
        public IEnumerable<PedidoMuestra> GetMuestrasPorEstado(Guid idEstado)
        {
            return QueryBase()
                .Where(p => p.IdEstadoPedidoMuestra == idEstado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos de muestra que se encuentran en un estado determinado.
        /// </summary>
        /// <param name="idEstado">Identificador del estado del pedido de muestra.</param>
        /// <returns>Pedidos de muestra que coinciden con el estado.</returns>
        public async Task<IEnumerable<PedidoMuestra>> GetMuestrasPorEstadoAsync(Guid idEstado)
        {
            return await QueryBase()
                .Where(p => p.IdEstadoPedidoMuestra == idEstado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Recupera los pedidos de muestra cuya fecha esperada de devolución ya venció.
        /// </summary>
        /// <returns>Pedidos de muestra vencidos.</returns>
        public IEnumerable<PedidoMuestra> GetMuestrasVencidas()
        {
            var hoy = DateTime.UtcNow.Date;
            return QueryBase()
                .Where(p => p.FechaDevolucionEsperada.HasValue && DbFunctions.TruncateTime(p.FechaDevolucionEsperada) < hoy)
                .OrderBy(p => p.FechaDevolucionEsperada)
                .ToList();
        }

        /// <summary>
        /// Recupera de forma asíncrona los pedidos de muestra cuya fecha esperada de devolución ya venció.
        /// </summary>
        /// <returns>Pedidos de muestra vencidos.</returns>
        public async Task<IEnumerable<PedidoMuestra>> GetMuestrasVencidasAsync()
        {
            var hoy = DateTime.UtcNow.Date;
            return await QueryBase()
                .Where(p => p.FechaDevolucionEsperada.HasValue && DbFunctions.TruncateTime(p.FechaDevolucionEsperada) < hoy)
                .OrderBy(p => p.FechaDevolucionEsperada)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un pedido de muestra con todas sus relaciones cargadas.
        /// </summary>
        /// <param name="idPedidoMuestra">Identificador del pedido de muestra.</param>
        /// <returns>Pedido de muestra encontrado o null.</returns>
        public PedidoMuestra GetMuestraConDetalles(Guid idPedidoMuestra)
        {
            return QueryBase().FirstOrDefault(p => p.IdPedidoMuestra == idPedidoMuestra);
        }

        /// <summary>
        /// Obtiene de forma asíncrona un pedido de muestra con todas sus relaciones cargadas.
        /// </summary>
        /// <param name="idPedidoMuestra">Identificador del pedido de muestra.</param>
        /// <returns>Pedido de muestra encontrado o null.</returns>
        public async Task<PedidoMuestra> GetMuestraConDetallesAsync(Guid idPedidoMuestra)
        {
            return await QueryBase().FirstOrDefaultAsync(p => p.IdPedidoMuestra == idPedidoMuestra);
        }
    }
}