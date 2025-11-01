using BLL.Interfaces;
using BLL.Reportes;
using DAL.Interfaces.Base;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BLL.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReporteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public ReporteGeneralResult GenerarReportes(ReporteParametros parametros)
        {
            if (parametros == null)
                parametros = new ReporteParametros();

            var hoy = DateTime.Today;
            var pedidos = _unitOfWork.Pedidos.GetAll()?.ToList() ?? new List<Pedido>();
            var pedidosMuestra = _unitOfWork.PedidosMuestra.GetAll()?.ToList() ?? new List<PedidoMuestra>();
            var facturas = _unitOfWork.FacturasCabecera.GetAll()?.ToList() ?? new List<FacturaCabecera>();

            var resultado = new ReporteGeneralResult();

            ConstruirOperativos(resultado.Operativos, pedidos, pedidosMuestra, parametros.DiasLimite, hoy);
            ConstruirVentas(resultado.Ventas, pedidos, parametros, hoy);
            ConstruirFinancieros(resultado.Financieros, pedidos, pedidosMuestra, facturas, parametros, hoy);

            return resultado;
        }

        private void ConstruirOperativos(ReporteOperativoData destino, List<Pedido> pedidos, List<PedidoMuestra> pedidosMuestra, int diasLimite, DateTime hoy)
        {
            destino.PedidosPorEstado = pedidos
                .GroupBy(p => (p.EstadoPedido?.NombreEstadoPedido ?? "Sin estado").Trim())
                .Select(g => new PedidoEstadoResumen
                {
                    Estado = string.IsNullOrWhiteSpace(g.Key) ? "Sin estado" : g.Key,
                    Cantidad = g.Count(),
                    Total = g.Sum(p => p.TotalConIva),
                    SaldoPendiente = g.Sum(p => p.SaldoPendiente)
                })
                .OrderByDescending(r => r.Cantidad)
                .ToList();

            var fechaLimiteMax = hoy.AddDays(Math.Max(0, diasLimite));
            destino.PedidosConFechaLimite = pedidos
                .Where(p => p.FechaLimiteEntrega.HasValue)
                .Select(p => new PedidoLimiteResumen
                {
                    IdPedido = p.IdPedido,
                    NumeroPedido = p.NumeroPedido,
                    Cliente = p.Cliente?.RazonSocial ?? "Sin cliente",
                    FechaLimite = p.FechaLimiteEntrega,
                    DiasRestantes = p.FechaLimiteEntrega.HasValue ? (int)Math.Round((p.FechaLimiteEntrega.Value.Date - hoy).TotalDays) : 0,
                    Estado = p.EstadoPedido?.NombreEstadoPedido ?? "Sin estado"
                })
                .Where(r => r.FechaLimite.HasValue && r.FechaLimite.Value.Date >= hoy && r.FechaLimite.Value.Date <= fechaLimiteMax)
                .OrderBy(r => r.FechaLimite)
                .ToList();

            destino.PedidosDemorados = pedidos
                .Where(p => p.FechaLimiteEntrega.HasValue && p.FechaLimiteEntrega.Value.Date < hoy && p.FechaFinalizacion == null)
                .Select(p => new PedidoDemoradoResumen
                {
                    IdPedido = p.IdPedido,
                    NumeroPedido = p.NumeroPedido,
                    Cliente = p.Cliente?.RazonSocial ?? "Sin cliente",
                    FechaLimite = p.FechaLimiteEntrega,
                    DiasAtraso = (int)Math.Round((hoy - p.FechaLimiteEntrega.Value.Date).TotalDays),
                    Estado = p.EstadoPedido?.NombreEstadoPedido ?? "Sin estado"
                })
                .OrderByDescending(r => r.DiasAtraso)
                .ToList();

            destino.PedidosConSaldoPendiente = pedidos
                .Where(p => p.SaldoPendiente > 0)
                .Select(p => new PedidoSaldoPendienteResumen
                {
                    IdPedido = p.IdPedido,
                    NumeroPedido = p.NumeroPedido,
                    Cliente = p.Cliente?.RazonSocial ?? "Sin cliente",
                    Total = p.TotalConIva,
                    MontoPagado = p.MontoPagado,
                    SaldoPendiente = p.SaldoPendiente
                })
                .OrderByDescending(r => r.SaldoPendiente)
                .ToList();

            destino.ProduccionEnCurso = pedidos
                .Where(p => p.FechaProduccion.HasValue && !p.FechaFinalizacion.HasValue)
                .Select(p => new ProduccionEstadoResumen
                {
                    IdPedido = p.IdPedido,
                    NumeroPedido = p.NumeroPedido,
                    Cliente = p.Cliente?.RazonSocial ?? "Sin cliente",
                    FechaProduccion = p.FechaProduccion,
                    Estado = p.EstadoPedido?.NombreEstadoPedido ?? "Sin estado"
                })
                .OrderBy(p => p.FechaProduccion)
                .ToList();

            destino.MuestrasVencidas = pedidosMuestra
                .Where(pm => pm.FechaDevolucionEsperada.HasValue && pm.FechaDevolucionEsperada.Value.Date < hoy)
                .Where(pm => pm.Detalles != null && pm.Detalles.Any())
                .Select(pm => new
                {
                    Pedido = pm,
                    Pendientes = pm.Detalles.Any(d => !DetalleDevueltoOPagado(d))
                })
                .Where(x => x.Pendientes)
                .Select(x => new MuestraVencidaResumen
                {
                    IdPedidoMuestra = x.Pedido.IdPedidoMuestra,
                    NumeroPedido = x.Pedido.NumeroPedidoMuestra,
                    Cliente = x.Pedido.Cliente?.RazonSocial ?? "Sin cliente",
                    FechaEsperadaDevolucion = x.Pedido.FechaDevolucionEsperada,
                    DiasAtraso = (int)Math.Round((hoy - x.Pedido.FechaDevolucionEsperada.Value.Date).TotalDays),
                    SaldoPendiente = x.Pedido.SaldoPendiente
                })
                .OrderByDescending(r => r.DiasAtraso)
                .ToList();
        }

        private void ConstruirVentas(ReporteVentasData destino, List<Pedido> pedidos, ReporteParametros parametros, DateTime hoy)
        {
            var inicioMesActual = new DateTime(hoy.Year, hoy.Month, 1);
            var inicioMesComparativo = parametros.MesComparativo ?? inicioMesActual.AddMonths(-1);
            inicioMesComparativo = new DateTime(inicioMesComparativo.Year, inicioMesComparativo.Month, 1);

            var inicioAnioActual = new DateTime(hoy.Year, 1, 1);
            var anioComparativo = parametros.AnioComparativo ?? hoy.Year - 1;
            var inicioAnioComparativo = new DateTime(anioComparativo, 1, 1);

            var haceDoceMeses = inicioMesActual.AddMonths(-11);
            destino.VentasMensuales = pedidos
                .Where(p => p.FechaCreacion >= haceDoceMeses)
                .GroupBy(p => new { p.FechaCreacion.Year, p.FechaCreacion.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new VentaPeriodoResumen
                {
                    Periodo = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy MMM", CultureInfo.CurrentCulture),
                    Total = g.Sum(p => p.TotalConIva)
                })
                .ToList();

            destino.VentasTrimestrales = pedidos
                .Where(p => p.FechaCreacion >= inicioMesActual.AddMonths(-11))
                .GroupBy(p => new { p.FechaCreacion.Year, Trimestre = (p.FechaCreacion.Month - 1) / 3 + 1 })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Trimestre)
                .Select(g => new VentaPeriodoResumen
                {
                    Periodo = $"{g.Key.Year} T{g.Key.Trimestre}",
                    Total = g.Sum(p => p.TotalConIva)
                })
                .ToList();

            destino.VentasAnuales = pedidos
                .GroupBy(p => p.FechaCreacion.Year)
                .OrderBy(g => g.Key)
                .Select(g => new VentaPeriodoResumen
                {
                    Periodo = g.Key.ToString(),
                    Total = g.Sum(p => p.TotalConIva)
                })
                .ToList();

            var totalMesActual = pedidos
                .Where(p => p.FechaCreacion >= inicioMesActual && p.FechaCreacion < inicioMesActual.AddMonths(1))
                .Sum(p => p.TotalConIva);
            var totalMesComparativo = pedidos
                .Where(p => p.FechaCreacion >= inicioMesComparativo && p.FechaCreacion < inicioMesComparativo.AddMonths(1))
                .Sum(p => p.TotalConIva);
            destino.ComparativaMensual = new ComparativaVentasResumen
            {
                PeriodoActual = inicioMesActual.ToString("yyyy MMM", CultureInfo.CurrentCulture),
                PeriodoComparado = inicioMesComparativo.ToString("yyyy MMM", CultureInfo.CurrentCulture),
                MontoActual = totalMesActual,
                MontoComparado = totalMesComparativo
            };

            var totalAnioActual = pedidos
                .Where(p => p.FechaCreacion >= inicioAnioActual && p.FechaCreacion < inicioAnioActual.AddYears(1))
                .Sum(p => p.TotalConIva);
            var totalAnioComparativo = pedidos
                .Where(p => p.FechaCreacion >= inicioAnioComparativo && p.FechaCreacion < inicioAnioComparativo.AddYears(1))
                .Sum(p => p.TotalConIva);
            destino.ComparativaAnual = new ComparativaVentasResumen
            {
                PeriodoActual = inicioAnioActual.Year.ToString(),
                PeriodoComparado = inicioAnioComparativo.Year.ToString(),
                MontoActual = totalAnioActual,
                MontoComparado = totalAnioComparativo
            };

            destino.RankingClientes = pedidos
                .GroupBy(p => new { p.IdCliente, Nombre = p.Cliente?.RazonSocial ?? "Sin cliente" })
                .Select(g => new ClienteRankingResumen
                {
                    IdCliente = g.Key.IdCliente,
                    Cliente = g.Key.Nombre,
                    TotalFacturado = g.Sum(p => p.TotalConIva),
                    CantidadPedidos = g.Count()
                })
                .OrderByDescending(r => r.TotalFacturado)
                .ThenBy(r => r.Cliente)
                .Take(15)
                .ToList();

            destino.VentasPorCategoria = pedidos
                .SelectMany(p => p.Detalles.Select(d => new
                {
                    CategoriaId = d.Producto?.Categoria?.IdCategoria,
                    Categoria = d.Producto?.Categoria?.NombreCategoria ?? "Sin categoría",
                    Importe = (decimal)d.Cantidad * d.PrecioUnitario
                }))
                .GroupBy(x => new { x.CategoriaId, x.Categoria })
                .Select(g => new VentaCategoriaResumen
                {
                    IdCategoria = g.Key.CategoriaId,
                    Categoria = g.Key.Categoria,
                    Total = g.Sum(x => x.Importe)
                })
                .OrderByDescending(v => v.Total)
                .ToList();
        }

        private void ConstruirFinancieros(ReporteFinancieroData destino, List<Pedido> pedidos, List<PedidoMuestra> pedidosMuestra, List<FacturaCabecera> facturas, ReporteParametros parametros, DateTime hoy)
        {
            var desde = parametros.PeriodoDesde ?? new DateTime(hoy.Year, 1, 1);
            var hasta = (parametros.PeriodoHasta ?? hoy).Date.AddDays(1);

            destino.FacturacionPorPeriodo = facturas
                .Where(f => f.FacturaFechaEmision >= desde && f.FacturaFechaEmision < hasta)
                .GroupBy(f => new { f.FacturaFechaEmision.Year, f.FacturaFechaEmision.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new FacturacionPeriodoResumen
                {
                    Periodo = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy MMM", CultureInfo.CurrentCulture),
                    TotalFacturado = g.Sum(f => f.MontoTotal)
                })
                .ToList();

            destino.CuentasPorCobrar = pedidos
                .Where(p => p.SaldoPendiente > 0)
                .GroupBy(p => new { p.IdCliente, Nombre = p.Cliente?.RazonSocial ?? "Sin cliente" })
                .Select(g => new CuentaPorCobrarResumen
                {
                    IdCliente = g.Key.IdCliente,
                    Cliente = g.Key.Nombre,
                    SaldoPendiente = g.Sum(p => p.SaldoPendiente)
                })
                .OrderByDescending(c => c.SaldoPendiente)
                .ToList();

            if (pedidosMuestra != null && pedidosMuestra.Any(pm => pm.SaldoPendiente > 0))
            {
                var muestrasPorCobrar = pedidosMuestra
                    .Where(pm => pm.SaldoPendiente > 0)
                    .GroupBy(pm => new { pm.IdCliente, Nombre = pm.Cliente?.RazonSocial ?? "Sin cliente" })
                    .Select(g => new CuentaPorCobrarResumen
                    {
                        IdCliente = g.Key.IdCliente,
                        Cliente = g.Key.Nombre,
                        SaldoPendiente = g.Sum(pm => pm.SaldoPendiente)
                    });

                destino.CuentasPorCobrar = destino.CuentasPorCobrar
                    .Concat(muestrasPorCobrar)
                    .GroupBy(c => new { c.IdCliente, c.Cliente })
                    .Select(g => new CuentaPorCobrarResumen
                    {
                        IdCliente = g.Key.IdCliente,
                        Cliente = g.Key.Cliente,
                        SaldoPendiente = g.Sum(x => x.SaldoPendiente)
                    })
                    .OrderByDescending(c => c.SaldoPendiente)
                    .ToList();
            }

            destino.PagosRecibidos = pedidos
                .Where(p => p.MontoPagado > 0)
                .Select(p => new PagoRecibidoResumen
                {
                    Fecha = p.FechaConfirmacion ?? p.FechaEntrega ?? p.FechaCreacion,
                    NumeroPedido = p.NumeroPedido,
                    Cliente = p.Cliente?.RazonSocial ?? "Sin cliente",
                    Monto = p.MontoPagado
                })
                .Where(r => r.Fecha >= desde && r.Fecha < hasta)
                .OrderByDescending(r => r.Fecha)
                .ToList();

            destino.ProyeccionIngresos = pedidos
                .Where(p => p.FechaConfirmacion.HasValue && !p.Facturado && p.TotalConIva > p.MontoPagado)
                .Select(p => new ProyeccionIngresoResumen
                {
                    IdPedido = p.IdPedido,
                    NumeroPedido = p.NumeroPedido,
                    Cliente = p.Cliente?.RazonSocial ?? "Sin cliente",
                    FechaEsperada = p.FechaEntrega ?? p.FechaLimiteEntrega ?? p.FechaCreacion.AddDays(7),
                    MontoProyectado = p.TotalConIva - p.MontoPagado
                })
                .OrderBy(r => r.FechaEsperada)
                .ToList();
        }

        private static bool DetalleDevueltoOPagado(DetalleMuestra detalle)
        {
            if (detalle == null)
                return false;

            var nombre = detalle.EstadoMuestra?.NombreEstadoMuestra ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombre) && detalle.IdEstadoMuestra.HasValue)
            {
                nombre = detalle.EstadoMuestra?.NombreEstadoMuestra ?? string.Empty;
            }

            return Coincide(nombre, "devuel") || Coincide(nombre, "pag");
        }

        private static bool Coincide(string texto, params string[] claves)
        {
            if (claves == null || claves.Length == 0)
                return false;

            var valor = (texto ?? string.Empty).Trim();
            if (valor.Length == 0)
                return false;

            var compare = CultureInfo.InvariantCulture.CompareInfo;
            foreach (var clave in claves)
            {
                if (string.IsNullOrWhiteSpace(clave))
                    continue;

                if (compare.IndexOf(valor, clave, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0)
                    return true;
            }

            return false;
        }
    }
}