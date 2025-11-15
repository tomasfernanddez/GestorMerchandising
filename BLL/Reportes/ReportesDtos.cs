using System;
namespace BLL.Reportes
{
    /// <summary>
    /// Define los rangos disponibles para agrupar la información de ventas en un reporte de periodo.
    /// </summary>
    public enum ReporteVentasPeriodoTipo
    {
        /// <summary>
        /// Agrupa los resultados dentro de un rango arbitrario entre dos fechas específicas.
        /// </summary>
        Rango,
        /// <summary>
        /// Agrupa la información considerando cada mes del año.
        /// </summary>
        Mensual,
        /// <summary>
        /// Consolida los datos en bloques anuales.
        /// </summary>
        Anual
    }

    /// <summary>
    /// Representa las opciones genéricas para agrupar datos dentro de un periodo determinado.
    /// </summary>
    public enum ReportePeriodoTipo
    {
        /// <summary>
        /// Incluye todos los registros sin agrupar por periodos específicos.
        /// </summary>
        Todos,
        /// <summary>
        /// Agrupa los resultados por mes.
        /// </summary>
        Mensual,
        /// <summary>
        /// Agrupa los resultados por año.
        /// </summary>
        Anual
    }

    /// <summary>
    /// Define los criterios utilizados para solicitar un reporte de ventas por periodo.
    /// </summary>
    public class VentasPeriodoFiltro
    {
        /// <summary>
        /// Tipo de agrupación que se aplicará al reporte.
        /// </summary>
        public ReporteVentasPeriodoTipo Tipo { get; set; } = ReporteVentasPeriodoTipo.Anual;

        /// <summary>
        /// Fecha inicial del periodo a analizar.
        /// </summary>
        public DateTime? Desde { get; set; }
            = new DateTime(DateTime.Today.Year, 1, 1);

        /// <summary>
        /// Fecha final del periodo a analizar.
        /// </summary>
        public DateTime? Hasta { get; set; }
            = DateTime.Today;

        /// <summary>
        /// Año utilizado cuando el reporte se agrupa por periodos mensuales o anuales.
        /// </summary>
        public int? Anio { get; set; } = DateTime.Today.Year;

        /// <summary>
        /// Mes utilizado cuando el reporte se agrupa por periodos mensuales.
        /// </summary>
        public int? Mes { get; set; } = DateTime.Today.Month;
    }

    /// <summary>
    /// Representa los filtros comunes aplicables a un reporte basado en periodos temporales.
    /// </summary>
    public class ReportePeriodoFiltro
    {
        /// <summary>
        /// Tipo de periodo elegido para agrupar la información.
        /// </summary>
        public ReportePeriodoTipo Tipo { get; set; } = ReportePeriodoTipo.Todos;

        /// <summary>
        /// Año a evaluar cuando aplica el filtro por periodo.
        /// </summary>
        public int? Anio { get; set; } = DateTime.Today.Year;

        /// <summary>
        /// Mes a evaluar cuando se requiere filtrar por meses específicos.
        /// </summary>
        public int? Mes { get; set; } = DateTime.Today.Month;
    }

    /// <summary>
    /// Filtro especializado para reportes de pedidos por cliente.
    /// </summary>
    public class PedidosClienteFiltro : ReportePeriodoFiltro
    {
        /// <summary>
        /// Indica si el reporte debe limitarse a pedidos con saldo pendiente.
        /// </summary>
        public bool SoloSaldoPendiente { get; set; }
            = false;
    }

    /// <summary>
    /// Representa el detalle agregado de ventas dentro de un periodo.
    /// </summary>
    public class VentaPeriodoDetalle
    {
        /// <summary>
        /// Texto descriptivo del periodo (mes, año o rango).
        /// </summary>
        public string Periodo { get; set; }
        /// <summary>
        /// Importe total vendido dentro del periodo indicado.
        /// </summary>
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Resumen de ventas agrupadas por categoría de producto.
    /// </summary>
    public class VentaCategoriaResumen
    {
        /// <summary>
        /// Identificador de la categoría asociada con la venta.
        /// </summary>
        public Guid? IdCategoria { get; set; }
        /// <summary>
        /// Nombre de la categoría vinculada.
        /// </summary>
        public string Categoria { get; set; }
        /// <summary>
        /// Total facturado para la categoría.
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Cantidad de pedidos o ventas registradas.
        /// </summary>
        public int Cantidad { get; set; }
    }

    /// <summary>
    /// Resumen de facturación consolidada por periodo.
    /// </summary>
    public class FacturacionPeriodoResumen
    {
        /// <summary>
        /// Periodo representado en el resumen.
        /// </summary>
        public string Periodo { get; set; }
        /// <summary>
        /// Total facturado dentro del periodo.
        /// </summary>
        public decimal TotalFacturado { get; set; }
    }

    /// <summary>
    /// Resumen del rendimiento de un cliente en un ranking de facturación.
    /// </summary>
    public class ClienteRankingResumen
    {
        /// <summary>
        /// Identificador del cliente asociado.
        /// </summary>
        public Guid? IdCliente { get; set; }
        /// <summary>
        /// Nombre o razón social del cliente.
        /// </summary>
        public string Cliente { get; set; }
        /// <summary>
        /// Total facturado por el cliente.
        /// </summary>
        public decimal TotalFacturado { get; set; }
        /// <summary>
        /// Cantidad de pedidos asociados al cliente.
        /// </summary>
        public int CantidadPedidos { get; set; }
    }

    /// <summary>
    /// Resumen de los pedidos emitidos por cliente incluyendo saldos.
    /// </summary>
    public class PedidoClienteResumen
    {
        /// <summary>
        /// Identificador del cliente al que pertenece la información.
        /// </summary>
        public Guid? IdCliente { get; set; }
        /// <summary>
        /// Nombre del cliente.
        /// </summary>
        public string Cliente { get; set; }
        /// <summary>
        /// Cantidad total de pedidos generados por el cliente.
        /// </summary>
        public int CantidadPedidos { get; set; }
        /// <summary>
        /// Monto facturado acumulado.
        /// </summary>
        public decimal TotalFacturado { get; set; }
        /// <summary>
        /// Saldo aún pendiente de cobro.
        /// </summary>
        public decimal SaldoPendiente { get; set; }
    }

    /// <summary>
    /// Resumen de pedidos procesados para un proveedor determinado.
    /// </summary>
    public class PedidoProveedorResumen
    {
        /// <summary>
        /// Identificador del proveedor vinculado al resumen.
        /// </summary>
        public Guid? IdProveedor { get; set; }
        /// <summary>
        /// Nombre del proveedor.
        /// </summary>
        public string Proveedor { get; set; }
        /// <summary>
        /// Cantidad de pedidos registrados para el proveedor.
        /// </summary>
        public int CantidadPedidos { get; set; }
        /// <summary>
        /// Número de productos incluidos en los pedidos del proveedor.
        /// </summary>
        public int CantidadProductos { get; set; }
        /// <summary>
        /// Importe total asociado al proveedor.
        /// </summary>
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Resumen de cuentas por cobrar asociadas a un cliente.
    /// </summary>
    public class CuentaPorCobrarResumen
    {
        /// <summary>
        /// Identificador del cliente que adeuda pagos.
        /// </summary>
        public Guid? IdCliente { get; set; }
        /// <summary>
        /// Nombre del cliente involucrado.
        /// </summary>
        public string Cliente { get; set; }
        /// <summary>
        /// Saldo total pendiente de cobro.
        /// </summary>
        public decimal SaldoPendiente { get; set; }
        /// <summary>
        /// Listado textual de pedidos con saldo pendiente.
        /// </summary>
        public string PedidosConSaldo { get; set; }
    }
}