using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    public interface IFacturaCabeceraRepository : IRepository<FacturaCabecera>
    {
        // Métodos específicos para FacturaCabecera
        IEnumerable<FacturaCabecera> GetFacturasPorCliente(Guid idCliente);
        Task<IEnumerable<FacturaCabecera>> GetFacturasPorClienteAsync(Guid idCliente);
        IEnumerable<FacturaCabecera> GetFacturasPorFecha(DateTime fechaDesde, DateTime fechaHasta);
        Task<IEnumerable<FacturaCabecera>> GetFacturasPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta);
        FacturaCabecera GetFacturaConDetalles(Guid idFactura);
        Task<FacturaCabecera> GetFacturaConDetallesAsync(Guid idFactura);
        int GetSiguienteNumeroFactura(int puntoVenta);
        Task<int> GetSiguienteNumeroFacturaAsync(int puntoVenta);
        decimal GetTotalFacturadoPorPeriodo(DateTime fechaDesde, DateTime fechaHasta);
        Task<decimal> GetTotalFacturadoPorPeriodoAsync(DateTime fechaDesde, DateTime fechaHasta);
    }
}
