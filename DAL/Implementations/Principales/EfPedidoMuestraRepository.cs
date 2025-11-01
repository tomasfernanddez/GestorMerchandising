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
        public EfPedidoMuestraRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        private IQueryable<PedidoMuestra> QueryBase()
        {
            return _dbSet
                .Include(p => p.Cliente)
                .Include(p => p.EstadoPedidoMuestra)
                .Include(p => p.Detalles.Select(d => d.Producto))
                .Include(p => p.Detalles.Select(d => d.EstadoMuestra));
        }

        public override IEnumerable<PedidoMuestra> GetAll()
        {
            return QueryBase()
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        public override async Task<IEnumerable<PedidoMuestra>> GetAllAsync()
        {
            return await QueryBase()
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<PedidoMuestra> GetMuestrasPorCliente(Guid idCliente)
        {
            return QueryBase()
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        public async Task<IEnumerable<PedidoMuestra>> GetMuestrasPorClienteAsync(Guid idCliente)
        {
            return await QueryBase()
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<PedidoMuestra> GetMuestrasPorEstado(Guid idEstado)
        {
            return QueryBase()
                .Where(p => p.IdEstadoPedidoMuestra == idEstado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();
        }

        public async Task<IEnumerable<PedidoMuestra>> GetMuestrasPorEstadoAsync(Guid idEstado)
        {
            return await QueryBase()
                .Where(p => p.IdEstadoPedidoMuestra == idEstado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<PedidoMuestra> GetMuestrasVencidas()
        {
            var hoy = DateTime.UtcNow.Date;
            return QueryBase()
                .Where(p => p.FechaDevolucionEsperada.HasValue && DbFunctions.TruncateTime(p.FechaDevolucionEsperada) < hoy)
                .OrderBy(p => p.FechaDevolucionEsperada)
                .ToList();
        }

        public async Task<IEnumerable<PedidoMuestra>> GetMuestrasVencidasAsync()
        {
            var hoy = DateTime.UtcNow.Date;
            return await QueryBase()
                .Where(p => p.FechaDevolucionEsperada.HasValue && DbFunctions.TruncateTime(p.FechaDevolucionEsperada) < hoy)
                .OrderBy(p => p.FechaDevolucionEsperada)
                .ToListAsync();
        }

        public PedidoMuestra GetMuestraConDetalles(Guid idPedidoMuestra)
        {
            return QueryBase().FirstOrDefault(p => p.IdPedidoMuestra == idPedidoMuestra);
        }

        public async Task<PedidoMuestra> GetMuestraConDetallesAsync(Guid idPedidoMuestra)
        {
            return await QueryBase().FirstOrDefaultAsync(p => p.IdPedidoMuestra == idPedidoMuestra);
        }
    }
}