using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    /// <summary>
    /// Define operaciones de acceso a datos especializadas para facturas.
    /// </summary>
    public interface IFacturaCabeceraRepository : IRepository<FacturaCabecera>
    {
        /// <summary>
        /// Obtiene las facturas emitidas para un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Colección de facturas del cliente.</returns>
        IEnumerable<FacturaCabecera> GetFacturasPorCliente(Guid idCliente);

        /// <summary>
        /// Obtiene de forma asíncrona las facturas emitidas para un cliente específico.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Colección de facturas del cliente.</returns>
        Task<IEnumerable<FacturaCabecera>> GetFacturasPorClienteAsync(Guid idCliente);

        /// <summary>
        /// Obtiene las facturas emitidas dentro de un rango de fechas.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del rango.</param>
        /// <param name="fechaHasta">Fecha final del rango.</param>
        /// <returns>Facturas emitidas en el periodo.</returns>
        IEnumerable<FacturaCabecera> GetFacturasPorFecha(DateTime fechaDesde, DateTime fechaHasta);

        /// <summary>
        /// Obtiene de forma asíncrona las facturas emitidas dentro de un rango de fechas.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del rango.</param>
        /// <param name="fechaHasta">Fecha final del rango.</param>
        /// <returns>Facturas emitidas en el periodo.</returns>
        Task<IEnumerable<FacturaCabecera>> GetFacturasPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta);

        /// <summary>
        /// Obtiene una factura con todas sus relaciones cargadas.
        /// </summary>
        /// <param name="idFactura">Identificador de la factura.</param>
        /// <returns>Factura encontrada o null.</returns>
        FacturaCabecera GetFacturaConDetalles(Guid idFactura);

        /// <summary>
        /// Obtiene de forma asíncrona una factura con todas sus relaciones cargadas.
        /// </summary>
        /// <param name="idFactura">Identificador de la factura.</param>
        /// <returns>Factura encontrada o null.</returns>
        Task<FacturaCabecera> GetFacturaConDetallesAsync(Guid idFactura);

        /// <summary>
        /// Obtiene el próximo número de factura disponible para un punto de venta.
        /// </summary>
        /// <param name="puntoVenta">Identificador del punto de venta.</param>
        /// <returns>Número correlativo siguiente.</returns>
        int GetSiguienteNumeroFactura(int puntoVenta);

        /// <summary>
        /// Obtiene de forma asíncrona el próximo número de factura disponible para un punto de venta.
        /// </summary>
        /// <param name="puntoVenta">Identificador del punto de venta.</param>
        /// <returns>Número correlativo siguiente.</returns>
        Task<int> GetSiguienteNumeroFacturaAsync(int puntoVenta);

        /// <summary>
        /// Calcula el total facturado dentro de un periodo.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del periodo.</param>
        /// <param name="fechaHasta">Fecha final del periodo.</param>
        /// <returns>Monto total facturado.</returns>
        decimal GetTotalFacturadoPorPeriodo(DateTime fechaDesde, DateTime fechaHasta);

        /// <summary>
        /// Calcula de forma asíncrona el total facturado dentro de un periodo.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del periodo.</param>
        /// <param name="fechaHasta">Fecha final del periodo.</param>
        /// <returns>Monto total facturado.</returns>
        Task<decimal> GetTotalFacturadoPorPeriodoAsync(DateTime fechaDesde, DateTime fechaHasta);
    }
}