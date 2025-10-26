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
                .Include(pm => pm.Cliente)
                .Include(pm => pm.EstadoPedidoMuestra)
                .Include(pm => pm.Detalles.Select(d => d.Producto))
                .Include(pm => pm.Detalles.Select(d => d.EstadoMuestra));
        }

        public override IEnumerable<PedidoMuestra> GetAll()
        {
            return QueryBase()
                .OrderByDescending(pm => pm.FechaCreacion)
                .ToList();
        }

        public override async Task<IEnumerable<PedidoMuestra>> GetAllAsync()
        {
            return await QueryBase()
                .OrderByDescending(pm => pm.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<PedidoMuestra> GetMuestrasPorCliente(Guid idCliente)
        {
            return QueryBase()
                .Where(pm => pm.IdCliente == idCliente)
                .OrderByDescending(pm => pm.FechaCreacion)
                .ToList();
        }

        public async Task<IEnumerable<PedidoMuestra>> GetMuestrasPorClienteAsync(Guid idCliente)
        {
            return await QueryBase()
                .Where(pm => pm.IdCliente == idCliente)
                .OrderByDescending(pm => pm.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<PedidoMuestra> GetMuestrasPorEstado(Guid idEstado)
        {
            return QueryBase()
                .Where(pm => pm.IdEstadoPedidoMuestra == idEstado)
                .OrderByDescending(pm => pm.FechaCreacion)
                .ToList();
        }

        public async Task<IEnumerable<PedidoMuestra>> GetMuestrasPorEstadoAsync(Guid idEstado)
        {
            return await QueryBase()
                .Where(pm => pm.IdEstadoPedidoMuestra == idEstado)
                .OrderByDescending(pm => pm.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<PedidoMuestra> GetMuestrasVencidas()
        {
            var hoy = DateTime.UtcNow.Date;
            return QueryBase()
                .Where(pm => pm.FechaDevolucionEsperada.HasValue && DbFunctions.TruncateTime(pm.FechaDevolucionEsperada) < hoy && !pm.Facturado)
                .OrderBy(pm => pm.FechaDevolucionEsperada)
                .ToList();
        }

        public async Task<IEnumerable<PedidoMuestra>> GetMuestrasVencidasAsync()
        {
            var hoy = DateTime.UtcNow.Date;
            return await QueryBase()
                .Where(pm => pm.FechaDevolucionEsperada.HasValue && DbFunctions.TruncateTime(pm.FechaDevolucionEsperada) < hoy && !pm.Facturado)
                .OrderBy(pm => pm.FechaDevolucionEsperada)
                .ToListAsync();
        }

        public PedidoMuestra GetMuestraConDetalles(Guid idPedidoMuestra)
        {
            return QueryBase().FirstOrDefault(pm => pm.IdPedidoMuestra == idPedidoMuestra);
        }

        public async Task<PedidoMuestra> GetMuestraConDetallesAsync(Guid idPedidoMuestra)
        {
            return await QueryBase().FirstOrDefaultAsync(pm => pm.IdPedidoMuestra == idPedidoMuestra);
        }
    }
}