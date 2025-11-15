using BLL.Reportes;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IReporteService
    {
        /// <summary>
        /// Obtiene ventas por periodo.
        /// </summary>
        IList<VentaPeriodoDetalle> ObtenerVentasPorPeriodo(VentasPeriodoFiltro filtro);

        /// <summary>
        /// Obtiene categorias mas vendidas.
        /// </summary>
        IList<VentaCategoriaResumen> ObtenerCategoriasMasVendidas(ReportePeriodoFiltro filtro);

        /// <summary>
        /// Obtiene facturacion por periodo.
        /// </summary>
        IList<FacturacionPeriodoResumen> ObtenerFacturacionPorPeriodo(ReportePeriodoFiltro filtro);

        /// <summary>
        /// Obtiene pedidos por cliente.
        /// </summary>
        IList<PedidoClienteResumen> ObtenerPedidosPorCliente(PedidosClienteFiltro filtro);

        /// <summary>
        /// Obtiene pedidos por proveedor.
        /// </summary>
        IList<PedidoProveedorResumen> ObtenerPedidosPorProveedor(ReportePeriodoFiltro filtro);

        /// <summary>
        /// Obtiene mejores clientes.
        /// </summary>
        IList<ClienteRankingResumen> ObtenerMejoresClientes(ReportePeriodoFiltro filtro);

        /// <summary>
        /// Obtiene clientes con saldo.
        /// </summary>
        IList<CuentaPorCobrarResumen> ObtenerClientesConSaldo();
    }
}