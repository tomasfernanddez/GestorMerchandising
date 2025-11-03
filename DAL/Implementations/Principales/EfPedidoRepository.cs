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
        public EfPedidoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

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
                .Include(p => p.HistorialEstados.Select(h => h.EstadoPedido))
                .Include(p => p.Notas)
                .Include(p => p.Adjuntos);
        }

        public override IEnumerable<Pedido> GetAll()
        {
            return QueryBase()
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        public override async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await QueryBase()
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<Pedido> GetPedidosPorCliente(Guid idCliente)
        {
            return QueryBase()
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosPorClienteAsync(Guid idCliente)
        {
            return await QueryBase()
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<Pedido> GetPedidosPorEstado(Guid idEstado)
        {
            return QueryBase()
                .Where(p => p.IdEstadoPedido == idEstado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosPorEstadoAsync(Guid idEstado)
        {
            return await QueryBase()
                .Where(p => p.IdEstadoPedido == idEstado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<Pedido> GetPedidosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            var hasta = fechaHasta.Date.AddDays(1);
            return QueryBase()
                .Where(p => p.FechaCreacion >= fechaDesde.Date && p.FechaCreacion < hasta)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            var hasta = fechaHasta.Date.AddDays(1);
            return await QueryBase()
                .Where(p => p.FechaCreacion >= fechaDesde.Date && p.FechaCreacion < hasta)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<Pedido> GetPedidosConFechaLimite()
        {
            return QueryBase()
                .Where(p => p.FechaLimiteEntrega.HasValue)
                .OrderBy(p => p.FechaLimiteEntrega)
                .ToList();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosConFechaLimiteAsync()
        {
            return await QueryBase()
                .Where(p => p.FechaLimiteEntrega.HasValue)
                .OrderBy(p => p.FechaLimiteEntrega)
                .ToListAsync();
        }

        public IEnumerable<Pedido> GetPedidosVencidos()
        {
            var hoy = DateTime.UtcNow.Date;
            return QueryBase()
                .Where(p => p.FechaLimiteEntrega.HasValue && DbFunctions.TruncateTime(p.FechaLimiteEntrega) < hoy && p.IdEstadoPedido != null)
                .OrderBy(p => p.FechaLimiteEntrega)
                .ToList();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosVencidosAsync()
        {
            var hoy = DateTime.UtcNow.Date;
            return await QueryBase()
                .Where(p => p.FechaLimiteEntrega.HasValue && DbFunctions.TruncateTime(p.FechaLimiteEntrega) < hoy && p.IdEstadoPedido != null)
                .OrderBy(p => p.FechaLimiteEntrega)
                .ToListAsync();
        }

        public Pedido GetPedidoConDetalles(Guid idPedido)
        {
            return QueryBase().FirstOrDefault(p => p.IdPedido == idPedido);
        }

        public async Task<Pedido> GetPedidoConDetallesAsync(Guid idPedido)
        {
            return await QueryBase().FirstOrDefaultAsync(p => p.IdPedido == idPedido);
        }
    }
}