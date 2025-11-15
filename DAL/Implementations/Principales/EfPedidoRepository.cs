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
    public class EfPedidoRepository : EfRepository<Pedido>, IPedidoRepository
    {
        /// <summary>
        /// Inicializa el repositorio de pedidos con el contexto de datos recibido.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework de la aplicación.</param>
        public EfPedidoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Construye la consulta base incluyendo todas las relaciones necesarias para un pedido.
        /// </summary>
        /// <returns>Consulta enriquecida que puede reutilizarse en distintas operaciones.</returns>
        private IQueryable<Pedido> QueryBase()
        {
            return _dbSet
                .Include(p => p.Cliente)
                .Include(p => p.EstadoPedido)
                .Include(p => p.TipoPago)
                .Include(p => p.Detalles.Select(d => d.Producto))
                .Include(p => p.Detalles.Select(d => d.EstadoProducto))
                .Include(p => p.Detalles.Select(d => d.ProveedorPersonalizacion))
                .Include(p => p.Detalles.Select(d => d.LogosPedido.Select(l => l.TecnicaPersonalizacion)))
                .Include(p => p.Detalles.Select(d => d.LogosPedido.Select(l => l.UbicacionLogo)))
                .Include(p => p.Detalles.Select(d => d.LogosPedido.Select(l => l.Proveedor)))
                .Include(p => p.Pagos)
                .Include(p => p.HistorialEstados.Select(h => h.EstadoPedido))
                .Include(p => p.Notas)
                .Include(p => p.Adjuntos);
        }

        /// <summary>
        /// Obtiene todos los pedidos ordenados por fecha de creación.
        /// </summary>
        /// <returns>Colección completa de pedidos.</returns>
        public override IEnumerable<Pedido> GetAll()
        {
            return QueryBase()
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona todos los pedidos ordenados por fecha de creación.
        /// </summary>
        /// <returns>Colección completa de pedidos.</returns>
        public override async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await QueryBase()
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Recupera los pedidos pertenecientes a un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Pedidos asociados al cliente indicado.</returns>
        public IEnumerable<Pedido> GetPedidosPorCliente(Guid idCliente)
        {
            return QueryBase()
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        /// <summary>
        /// Recupera de forma asíncrona los pedidos pertenecientes a un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Pedidos asociados al cliente indicado.</returns>
        public async Task<IEnumerable<Pedido>> GetPedidosPorClienteAsync(Guid idCliente)
        {
            return await QueryBase()
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los pedidos que se encuentran en un estado determinado.
        /// </summary>
        /// <param name="idEstado">Identificador del estado.</param>
        /// <returns>Pedidos que coinciden con el estado.</returns>
        public IEnumerable<Pedido> GetPedidosPorEstado(Guid idEstado)
        {
            return QueryBase()
                .Where(p => p.IdEstadoPedido == idEstado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos que se encuentran en un estado determinado.
        /// </summary>
        /// <param name="idEstado">Identificador del estado.</param>
        /// <returns>Pedidos que coinciden con el estado.</returns>
        public async Task<IEnumerable<Pedido>> GetPedidosPorEstadoAsync(Guid idEstado)
        {
            return await QueryBase()
                .Where(p => p.IdEstadoPedido == idEstado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Recupera los pedidos generados entre dos fechas inclusive.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del rango.</param>
        /// <param name="fechaHasta">Fecha final del rango.</param>
        /// <returns>Pedidos creados dentro del periodo.</returns>
        public IEnumerable<Pedido> GetPedidosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            var hasta = fechaHasta.Date.AddDays(1);
            return QueryBase()
                .Where(p => p.FechaCreacion >= fechaDesde.Date && p.FechaCreacion < hasta)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        /// <summary>
        /// Recupera de forma asíncrona los pedidos generados entre dos fechas inclusive.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del rango.</param>
        /// <param name="fechaHasta">Fecha final del rango.</param>
        /// <returns>Pedidos creados dentro del periodo.</returns>
        public async Task<IEnumerable<Pedido>> GetPedidosPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            var hasta = fechaHasta.Date.AddDays(1);
            return await QueryBase()
                .Where(p => p.FechaCreacion >= fechaDesde.Date && p.FechaCreacion < hasta)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los pedidos que tienen fecha límite de entrega configurada.
        /// </summary>
        /// <returns>Pedidos ordenados por la fecha límite más próxima.</returns>
        public IEnumerable<Pedido> GetPedidosConFechaLimite()
        {
            return QueryBase()
                .Where(p => p.FechaLimiteEntrega.HasValue)
                .OrderBy(p => p.FechaLimiteEntrega)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los pedidos que tienen fecha límite de entrega configurada.
        /// </summary>
        /// <returns>Pedidos ordenados por la fecha límite más próxima.</returns>
        public async Task<IEnumerable<Pedido>> GetPedidosConFechaLimiteAsync()
        {
            return await QueryBase()
                .Where(p => p.FechaLimiteEntrega.HasValue)
                .OrderBy(p => p.FechaLimiteEntrega)
                .ToListAsync();
        }

        /// <summary>
        /// Recupera los pedidos cuya fecha límite ya venció.
        /// </summary>
        /// <returns>Pedidos vencidos ordenados por fecha límite.</returns>
        public IEnumerable<Pedido> GetPedidosVencidos()
        {
            var hoy = DateTime.UtcNow.Date;
            return QueryBase()
                .Where(p => p.FechaLimiteEntrega.HasValue && DbFunctions.TruncateTime(p.FechaLimiteEntrega) < hoy && p.IdEstadoPedido != null)
                .OrderBy(p => p.FechaLimiteEntrega)
                .ToList();
        }

        /// <summary>
        /// Recupera de forma asíncrona los pedidos cuya fecha límite ya venció.
        /// </summary>
        /// <returns>Pedidos vencidos ordenados por fecha límite.</returns>
        public async Task<IEnumerable<Pedido>> GetPedidosVencidosAsync()
        {
            var hoy = DateTime.UtcNow.Date;
            return await QueryBase()
                .Where(p => p.FechaLimiteEntrega.HasValue && DbFunctions.TruncateTime(p.FechaLimiteEntrega) < hoy && p.IdEstadoPedido != null)
                .OrderBy(p => p.FechaLimiteEntrega)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un pedido con todas sus relaciones detalladas.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Pedido encontrado o null.</returns>
        public Pedido GetPedidoConDetalles(Guid idPedido)
        {
            return QueryBase().FirstOrDefault(p => p.IdPedido == idPedido);
        }

        /// <summary>
        /// Obtiene de forma asíncrona un pedido con todas sus relaciones detalladas.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Pedido encontrado o null.</returns>
        public async Task<Pedido> GetPedidoConDetallesAsync(Guid idPedido)
        {
            return await QueryBase().FirstOrDefaultAsync(p => p.IdPedido == idPedido);
        }
    }
}