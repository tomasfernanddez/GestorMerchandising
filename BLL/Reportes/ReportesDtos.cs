using System;
namespace BLL.Reportes
{
    public enum ReporteVentasPeriodoTipo
    {
        Rango,
        Mensual,
        Anual
    }

    public enum ReportePeriodoTipo
    {
        Todos,
        Mensual,
        Anual
    }

    public class VentasPeriodoFiltro
    {
        public ReporteVentasPeriodoTipo Tipo { get; set; } = ReporteVentasPeriodoTipo.Anual;

        public DateTime? Desde { get; set; }
            = new DateTime(DateTime.Today.Year, 1, 1);

        public DateTime? Hasta { get; set; }
            = DateTime.Today;

        public int? Anio { get; set; } = DateTime.Today.Year;

        public int? Mes { get; set; } = DateTime.Today.Month;
    }

    public class ReportePeriodoFiltro
    {
        public ReportePeriodoTipo Tipo { get; set; } = ReportePeriodoTipo.Todos;

        public int? Anio { get; set; } = DateTime.Today.Year;

        public int? Mes { get; set; } = DateTime.Today.Month;
    }

    public class PedidosClienteFiltro : ReportePeriodoFiltro
    {
        public bool SoloSaldoPendiente { get; set; }
            = false;
    }

    public class VentaPeriodoDetalle
    {
        public string Periodo { get; set; }
        public decimal Total { get; set; }
    }

    public class VentaCategoriaResumen
    {
        public Guid? IdCategoria { get; set; }
        public string Categoria { get; set; }
        public decimal Total { get; set; }
        public int Cantidad { get; set; }
    }

    public class FacturacionPeriodoResumen
    {
        public string Periodo { get; set; }
        public decimal TotalFacturado { get; set; }
    }

    public class ClienteRankingResumen
    {
        public Guid? IdCliente { get; set; }
        public string Cliente { get; set; }
        public decimal TotalFacturado { get; set; }
        public int CantidadPedidos { get; set; }
    }

    public class PedidoClienteResumen
    {
        public Guid? IdCliente { get; set; }
        public string Cliente { get; set; }
        public int CantidadPedidos { get; set; }
        public decimal TotalFacturado { get; set; }
        public decimal SaldoPendiente { get; set; }
    }

    public class PedidoProveedorResumen
    {
        public Guid? IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public int CantidadPedidos { get; set; }
        public int CantidadProductos { get; set; }
        public decimal Total { get; set; }
    }

    public class CuentaPorCobrarResumen
    {
        public Guid? IdCliente { get; set; }
        public string Cliente { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string PedidosConSaldo { get; set; }
    }
}