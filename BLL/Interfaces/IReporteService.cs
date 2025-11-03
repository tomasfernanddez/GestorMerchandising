using BLL.Reportes;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IReporteService
    {
        IList<VentaPeriodoDetalle> ObtenerVentasPorPeriodo(VentasPeriodoFiltro filtro);

        IList<VentaCategoriaResumen> ObtenerCategoriasMasVendidas(ReportePeriodoFiltro filtro);

        IList<FacturacionPeriodoResumen> ObtenerFacturacionPorPeriodo(ReportePeriodoFiltro filtro);

        IList<PedidoClienteResumen> ObtenerPedidosPorCliente(PedidosClienteFiltro filtro);

        IList<PedidoProveedorResumen> ObtenerPedidosPorProveedor(ReportePeriodoFiltro filtro);

        IList<ClienteRankingResumen> ObtenerMejoresClientes(ReportePeriodoFiltro filtro);

        IList<CuentaPorCobrarResumen> ObtenerClientesConSaldo();
    }
}