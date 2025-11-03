using DAL.Implementations.Base;
using DAL.Interfaces.Principales;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Implementations.Principales
{
    public class EfFacturaCabeceraRepository : EfRepository<FacturaCabecera>, IFacturaCabeceraRepository
    {
        public EfFacturaCabeceraRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        public IEnumerable<FacturaCabecera> GetFacturasPorCliente(Guid idCliente)
        {
            return _dbSet
                .Where(f => f.IdCliente == idCliente)
                .Include(f => f.Cliente)
                .Include(f => f.Emisor)
                .Include(f => f.Detalles.Select(d => d.Producto))
                .OrderByDescending(f => f.FacturaFechaEmision)
                .ToList();
        }

        public async Task<IEnumerable<FacturaCabecera>> GetFacturasPorClienteAsync(Guid idCliente)
        {
            return await _dbSet
                .Where(f => f.IdCliente == idCliente)
                .Include(f => f.Cliente)
                .Include(f => f.Emisor)
                .Include(f => f.Detalles.Select(d => d.Producto))
                .OrderByDescending(f => f.FacturaFechaEmision)
                .ToListAsync();
        }

        public IEnumerable<FacturaCabecera> GetFacturasPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _dbSet
                .Where(f => f.FacturaFechaEmision >= fechaDesde && f.FacturaFechaEmision <= fechaHasta)
                .Include(f => f.Cliente)
                .Include(f => f.Emisor)
                .Include(f => f.Detalles.Select(d => d.Producto))
                .OrderByDescending(f => f.FacturaFechaEmision)
                .ToList();
        }

        public async Task<IEnumerable<FacturaCabecera>> GetFacturasPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            return await _dbSet
                .Where(f => f.FacturaFechaEmision >= fechaDesde && f.FacturaFechaEmision <= fechaHasta)
                .Include(f => f.Cliente)
                .Include(f => f.Emisor)
                .Include(f => f.Detalles.Select(d => d.Producto))
                .OrderByDescending(f => f.FacturaFechaEmision)
                .ToListAsync();
        }

        public FacturaCabecera GetFacturaConDetalles(Guid idFactura)
        {
            return _dbSet
                .Where(f => f.IdFactura == idFactura)
                .Include(f => f.Cliente)
                .Include(f => f.Emisor)
                .Include(f => f.Detalles.Select(d => d.Producto))
                .FirstOrDefault();
        }

        public async Task<FacturaCabecera> GetFacturaConDetallesAsync(Guid idFactura)
        {
            return await _dbSet
                .Where(f => f.IdFactura == idFactura)
                .Include(f => f.Cliente)
                .Include(f => f.Emisor)
                .Include(f => f.Detalles.Select(d => d.Producto))
                .FirstOrDefaultAsync();
        }

        public int GetSiguienteNumeroFactura(int puntoVenta)
        {
            var maxActual = _dbSet
                .Where(f => f.FacturaPuntoVenta == puntoVenta)
                .Select(f => (int?)f.FacturaNumeroComprobante)
                .DefaultIfEmpty(0)
                .Max();

            return (maxActual ?? 0) + 1;
        }

        public async Task<int> GetSiguienteNumeroFacturaAsync(int puntoVenta)
        {
            var maxActual = await _dbSet
                .Where(f => f.FacturaPuntoVenta == puntoVenta)
                .Select(f => (int?)f.FacturaNumeroComprobante)
                .DefaultIfEmpty(0)
                .MaxAsync();

            return (maxActual ?? 0) + 1;
        }

        public decimal GetTotalFacturadoPorPeriodo(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _dbSet
                .Where(f => f.FacturaFechaEmision >= fechaDesde && f.FacturaFechaEmision <= fechaHasta)
                .Sum(f => (decimal?)f.MontoTotal) ?? 0m;
        }

        public async Task<decimal> GetTotalFacturadoPorPeriodoAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            var total = await _dbSet
                .Where(f => f.FacturaFechaEmision >= fechaDesde && f.FacturaFechaEmision <= fechaHasta)
                .SumAsync(f => (decimal?)f.MontoTotal);

            return total ?? 0m;
        }
    }
}