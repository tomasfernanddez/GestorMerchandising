using System;
using System.Collections.Generic;

namespace BLL.Reportes
{
    public class PedidoEstadoResumen
    {
        public string Estado { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoPendiente { get; set; }
    }

    public class PedidoLimiteResumen
    {
        public Guid IdPedido { get; set; }
        public string NumeroPedido { get; set; }
        public string Cliente { get; set; }
        public DateTime? FechaLimite { get; set; }
        public int DiasRestantes { get; set; }
        public string Estado { get; set; }
    }

    public class PedidoDemoradoResumen
    {
        public Guid IdPedido { get; set; }
        public string NumeroPedido { get; set; }
        public string Cliente { get; set; }
        public DateTime? FechaLimite { get; set; }
        public int DiasAtraso { get; set; }
        public string Estado { get; set; }
    }

    public class PedidoSaldoPendienteResumen
    {
        public Guid IdPedido { get; set; }
        public string NumeroPedido { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }
    }

    public class ProduccionEstadoResumen
    {
        public Guid IdPedido { get; set; }
        public string NumeroPedido { get; set; }
        public string Cliente { get; set; }
        public DateTime? FechaProduccion { get; set; }
        public string Estado { get; set; }
    }

    public class MuestraVencidaResumen
    {
        public Guid IdPedidoMuestra { get; set; }
        public string NumeroPedido { get; set; }
        public string Cliente { get; set; }
        public DateTime? FechaEsperadaDevolucion { get; set; }
        public int DiasAtraso { get; set; }
        public decimal SaldoPendiente { get; set; }
    }

    public class VentaPeriodoResumen
    {
        public string Periodo { get; set; }
        public decimal Total { get; set; }
    }

    public class ComparativaVentasResumen
    {
        public string PeriodoActual { get; set; }
        public string PeriodoComparado { get; set; }
        public decimal MontoActual { get; set; }
        public decimal MontoComparado { get; set; }
        public decimal Diferencia => MontoActual - MontoComparado;
        public decimal VariacionPorcentual => MontoComparado == 0 ? (MontoActual == 0 ? 0 : 100) : (MontoActual - MontoComparado) / MontoComparado * 100;
    }

    public class ClienteRankingResumen
    {
        public Guid IdCliente { get; set; }
        public string Cliente { get; set; }
        public decimal TotalFacturado { get; set; }
        public int CantidadPedidos { get; set; }
    }

    public class VentaCategoriaResumen
    {
        public Guid? IdCategoria { get; set; }
        public string Categoria { get; set; }
        public decimal Total { get; set; }
    }

    public class FacturacionPeriodoResumen
    {
        public string Periodo { get; set; }
        public decimal TotalFacturado { get; set; }
    }

    public class CuentaPorCobrarResumen
    {
        public Guid IdCliente { get; set; }
        public string Cliente { get; set; }
        public decimal SaldoPendiente { get; set; }
    }

    public class PagoRecibidoResumen
    {
        public DateTime Fecha { get; set; }
        public string NumeroPedido { get; set; }
        public string Cliente { get; set; }
        public decimal Monto { get; set; }
    }

    public class ProyeccionIngresoResumen
    {
        public Guid IdPedido { get; set; }
        public string NumeroPedido { get; set; }
        public string Cliente { get; set; }
        public DateTime? FechaEsperada { get; set; }
        public decimal MontoProyectado { get; set; }
    }

    public class ReporteOperativoData
    {
        public List<PedidoEstadoResumen> PedidosPorEstado { get; set; } = new List<PedidoEstadoResumen>();
        public List<PedidoLimiteResumen> PedidosConFechaLimite { get; set; } = new List<PedidoLimiteResumen>();
        public List<PedidoDemoradoResumen> PedidosDemorados { get; set; } = new List<PedidoDemoradoResumen>();
        public List<MuestraVencidaResumen> MuestrasVencidas { get; set; } = new List<MuestraVencidaResumen>();
        public List<PedidoSaldoPendienteResumen> PedidosConSaldoPendiente { get; set; } = new List<PedidoSaldoPendienteResumen>();
        public List<ProduccionEstadoResumen> ProduccionEnCurso { get; set; } = new List<ProduccionEstadoResumen>();
    }

    public class ReporteVentasData
    {
        public List<VentaPeriodoResumen> VentasMensuales { get; set; } = new List<VentaPeriodoResumen>();
        public List<VentaPeriodoResumen> VentasTrimestrales { get; set; } = new List<VentaPeriodoResumen>();
        public List<VentaPeriodoResumen> VentasAnuales { get; set; } = new List<VentaPeriodoResumen>();
        public ComparativaVentasResumen ComparativaMensual { get; set; }
        public ComparativaVentasResumen ComparativaAnual { get; set; }
        public List<ClienteRankingResumen> RankingClientes { get; set; } = new List<ClienteRankingResumen>();
        public List<VentaCategoriaResumen> VentasPorCategoria { get; set; } = new List<VentaCategoriaResumen>();
    }

    public class ReporteFinancieroData
    {
        public List<FacturacionPeriodoResumen> FacturacionPorPeriodo { get; set; } = new List<FacturacionPeriodoResumen>();
        public List<CuentaPorCobrarResumen> CuentasPorCobrar { get; set; } = new List<CuentaPorCobrarResumen>();
        public List<PagoRecibidoResumen> PagosRecibidos { get; set; } = new List<PagoRecibidoResumen>();
        public List<ProyeccionIngresoResumen> ProyeccionIngresos { get; set; } = new List<ProyeccionIngresoResumen>();
    }

    public class ReporteGeneralResult
    {
        public ReporteOperativoData Operativos { get; set; } = new ReporteOperativoData();
        public ReporteVentasData Ventas { get; set; } = new ReporteVentasData();
        public ReporteFinancieroData Financieros { get; set; } = new ReporteFinancieroData();
    }

    public class ReporteParametros
    {
        public int DiasLimite { get; set; } = 7;
        public DateTime? MesComparativo { get; set; }
            = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
        public int? AnioComparativo { get; set; } = DateTime.Today.Year - 1;
        public DateTime? PeriodoDesde { get; set; }
            = new DateTime(DateTime.Today.Year, 1, 1);
        public DateTime? PeriodoHasta { get; set; } = DateTime.Today;
    }
}