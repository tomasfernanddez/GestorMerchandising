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
        /// <summary>
        /// Inicializa el repositorio de facturas con el contexto de datos indicado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfFacturaCabeceraRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene las facturas emitidas para un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Colección de facturas ordenadas por fecha de emisión.</returns>
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

        /// <summary>
        /// Obtiene de forma asíncrona las facturas emitidas para un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Colección de facturas ordenadas por fecha de emisión.</returns>
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

        /// <summary>
        /// Recupera las facturas emitidas dentro de un rango de fechas.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del rango.</param>
        /// <param name="fechaHasta">Fecha final del rango.</param>
        /// <returns>Facturas dentro del periodo especificado.</returns>
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

        /// <summary>
        /// Recupera de forma asíncrona las facturas emitidas dentro de un rango de fechas.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del rango.</param>
        /// <param name="fechaHasta">Fecha final del rango.</param>
        /// <returns>Facturas dentro del periodo especificado.</returns>
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

        /// <summary>
        /// Obtiene una factura incluyendo toda su información relacionada.
        /// </summary>
        /// <param name="idFactura">Identificador de la factura.</param>
        /// <returns>Factura con sus detalles cargados.</returns>
        public FacturaCabecera GetFacturaConDetalles(Guid idFactura)
        {
            return _dbSet
                .Where(f => f.IdFactura == idFactura)
                .Include(f => f.Cliente)
                .Include(f => f.Emisor)
                .Include(f => f.Detalles.Select(d => d.Producto))
                .FirstOrDefault();
        }

        /// <summary>
        /// Obtiene de forma asíncrona una factura incluyendo toda su información relacionada.
        /// </summary>
        /// <param name="idFactura">Identificador de la factura.</param>
        /// <returns>Factura con sus detalles cargados.</returns>
        public async Task<FacturaCabecera> GetFacturaConDetallesAsync(Guid idFactura)
        {
            return await _dbSet
                .Where(f => f.IdFactura == idFactura)
                .Include(f => f.Cliente)
                .Include(f => f.Emisor)
                .Include(f => f.Detalles.Select(d => d.Producto))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene el próximo número de factura para un punto de venta específico.
        /// </summary>
        /// <param name="puntoVenta">Identificador del punto de venta.</param>
        /// <returns>Próximo número correlativo de factura.</returns>
        public int GetSiguienteNumeroFactura(int puntoVenta)
        {
            var maxActual = _dbSet
                .Where(f => f.FacturaPuntoVenta == puntoVenta)
                .Select(f => (int?)f.FacturaNumeroComprobante)
                .DefaultIfEmpty(0)
                .Max();

            return (maxActual ?? 0) + 1;
        }

        /// <summary>
        /// Obtiene de forma asíncrona el próximo número de factura para un punto de venta específico.
        /// </summary>
        /// <param name="puntoVenta">Identificador del punto de venta.</param>
        /// <returns>Próximo número correlativo de factura.</returns>
        public async Task<int> GetSiguienteNumeroFacturaAsync(int puntoVenta)
        {
            var maxActual = await _dbSet
                .Where(f => f.FacturaPuntoVenta == puntoVenta)
                .Select(f => (int?)f.FacturaNumeroComprobante)
                .DefaultIfEmpty(0)
                .MaxAsync();

            return (maxActual ?? 0) + 1;
        }

        /// <summary>
        /// Calcula el monto total facturado dentro de un periodo determinado.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del periodo.</param>
        /// <param name="fechaHasta">Fecha final del periodo.</param>
        /// <returns>Monto total facturado en el periodo.</returns>
        public decimal GetTotalFacturadoPorPeriodo(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _dbSet
                .Where(f => f.FacturaFechaEmision >= fechaDesde && f.FacturaFechaEmision <= fechaHasta)
                .Sum(f => (decimal?)f.MontoTotal) ?? 0m;
        }

        /// <summary>
        /// Calcula de forma asíncrona el monto total facturado dentro de un periodo determinado.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del periodo.</param>
        /// <param name="fechaHasta">Fecha final del periodo.</param>
        /// <returns>Monto total facturado en el periodo.</returns>
        public async Task<decimal> GetTotalFacturadoPorPeriodoAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            var total = await _dbSet
                .Where(f => f.FacturaFechaEmision >= fechaDesde && f.FacturaFechaEmision <= fechaHasta)
                .SumAsync(f => (decimal?)f.MontoTotal);

            return total ?? 0m;
        }
    }
}