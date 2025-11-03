using BLL.Interfaces;
using BLL.Reportes;
using DAL.Interfaces.Base;
using DomainModel;
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

        public IList<VentaPeriodoDetalle> ObtenerVentasPorPeriodo(VentasPeriodoFiltro filtro)
        {
            var pedidos = ObtenerPedidos();
            var datos = FiltrarPedidosPorVentas(pedidos, filtro);

            switch (filtro?.Tipo ?? ReporteVentasPeriodoTipo.Anual)
            {
                case ReporteVentasPeriodoTipo.Rango:
                    return datos
                        .GroupBy(p => p.FechaCreacion.Date)
                        .OrderBy(g => g.Key)
                        .Select(g => new VentaPeriodoDetalle
                        {
                            Periodo = g.Key.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture),
                            Total = g.Sum(p => p.TotalConIva)
                        })
                        .ToList();

                case ReporteVentasPeriodoTipo.Mensual:
                    return datos
                        .GroupBy(p => p.FechaCreacion.Date)
                        .OrderBy(g => g.Key)
                        .Select(g => new VentaPeriodoDetalle
                        {
                            Periodo = g.Key.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture),
                            Total = g.Sum(p => p.TotalConIva)
                        })
                        .ToList();

                default:
                    return datos
                        .GroupBy(p => new { p.FechaCreacion.Year, p.FechaCreacion.Month })
                        .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                        .Select(g => new VentaPeriodoDetalle
                        {
                            Periodo = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy MMM", CultureInfo.CurrentCulture),
                            Total = g.Sum(p => p.TotalConIva)
                        })
                        .ToList();
            }
        }

        public IList<VentaCategoriaResumen> ObtenerCategoriasMasVendidas(ReportePeriodoFiltro filtro)
        {
            var pedidos = FiltrarPedidosPorPeriodo(ObtenerPedidos(), filtro);

            return pedidos
                .SelectMany(p => p.Detalles ?? Enumerable.Empty<PedidoDetalle>(), (pedido, detalle) => new
                {
                    pedido,
                    detalle,
                    categoriaId = detalle.Producto?.Categoria?.IdCategoria,
                    categoriaNombre = detalle.Producto?.Categoria?.NombreCategoria ?? "Sin categoría"
                })
                .GroupBy(x => new { x.categoriaId, x.categoriaNombre })
                .Select(g => new VentaCategoriaResumen
                {
                    IdCategoria = g.Key.categoriaId,
                    Categoria = g.Key.categoriaNombre,
                    Total = g.Sum(x => x.detalle.Cantidad * x.detalle.PrecioUnitario),
                    Cantidad = g.Sum(x => x.detalle.Cantidad)
                })
                .OrderByDescending(r => r.Total)
                .ThenBy(r => r.Categoria)
                .ToList();
        }

        public IList<FacturacionPeriodoResumen> ObtenerFacturacionPorPeriodo(ReportePeriodoFiltro filtro)
        {
            var facturas = ObtenerFacturas();
            var tipo = filtro?.Tipo ?? ReportePeriodoTipo.Mensual;

            if (tipo == ReportePeriodoTipo.Mensual)
            {
                var anio = filtro?.Anio ?? DateTime.Today.Year;
                var mes = filtro?.Mes ?? DateTime.Today.Month;
                var desde = new DateTime(anio, mes, 1);
                var hasta = desde.AddMonths(1);

                return facturas
                    .Where(f => f.FacturaFechaEmision >= desde && f.FacturaFechaEmision < hasta)
                    .GroupBy(f => f.FacturaFechaEmision.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => new FacturacionPeriodoResumen
                    {
                        Periodo = g.Key.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture),
                        TotalFacturado = g.Sum(f => f.MontoTotal)
                    })
                    .ToList();
            }

            if (tipo == ReportePeriodoTipo.Anual)
            {
                var anio = filtro?.Anio ?? DateTime.Today.Year;
                return facturas
                    .Where(f => f.FacturaFechaEmision.Year == anio)
                    .GroupBy(f => new { f.FacturaFechaEmision.Year, f.FacturaFechaEmision.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new FacturacionPeriodoResumen
                    {
                        Periodo = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy MMM", CultureInfo.CurrentCulture),
                        TotalFacturado = g.Sum(f => f.MontoTotal)
                    })
                    .ToList();
            }

            return facturas
                .GroupBy(f => f.FacturaFechaEmision.Year)
                .OrderBy(g => g.Key)
                .Select(g => new FacturacionPeriodoResumen
                {
                    Periodo = g.Key.ToString(CultureInfo.CurrentCulture),
                    TotalFacturado = g.Sum(f => f.MontoTotal)
                })
                .ToList();
        }

        public IList<PedidoClienteResumen> ObtenerPedidosPorCliente(PedidosClienteFiltro filtro)
        {
            var pedidos = FiltrarPedidosPorPeriodo(ObtenerPedidos(), filtro);

            if (filtro?.SoloSaldoPendiente ?? false)
            {
                pedidos = pedidos.Where(p => p.SaldoPendiente > 0);
            }

            return pedidos
                .GroupBy(p => new { p.IdCliente, Nombre = p.Cliente?.RazonSocial ?? "Sin cliente" })
                .Select(g => new PedidoClienteResumen
                {
                    IdCliente = g.Key.IdCliente,
                    Cliente = g.Key.Nombre,
                    CantidadPedidos = g.Count(),
                    TotalFacturado = g.Sum(p => p.TotalConIva),
                    SaldoPendiente = g.Sum(p => p.SaldoPendiente)
                })
                .OrderByDescending(r => r.TotalFacturado)
                .ThenBy(r => r.Cliente)
                .ToList();
        }

        public IList<PedidoProveedorResumen> ObtenerPedidosPorProveedor(ReportePeriodoFiltro filtro)
        {
            var pedidos = FiltrarPedidosPorPeriodo(ObtenerPedidos(), filtro);

            return pedidos
                .SelectMany(p => p.Detalles ?? Enumerable.Empty<PedidoDetalle>(), (pedido, detalle) => new
                {
                    pedido,
                    detalle,
                    proveedorId = detalle.IdProveedorPersonalizacion,
                    proveedorNombre = detalle.ProveedorPersonalizacion?.RazonSocial ?? "Sin proveedor"
                })
                .Where(x => x.proveedorId.HasValue)
                .GroupBy(x => new { x.proveedorId, x.proveedorNombre })
                .Select(g => new PedidoProveedorResumen
                {
                    IdProveedor = g.Key.proveedorId,
                    Proveedor = g.Key.proveedorNombre,
                    CantidadPedidos = g.Select(x => x.pedido.IdPedido).Distinct().Count(),
                    CantidadProductos = g.Sum(x => x.detalle.Cantidad),
                    Total = g.Sum(x => x.detalle.Cantidad * x.detalle.PrecioUnitario)
                })
                .OrderByDescending(r => r.Total)
                .ThenBy(r => r.Proveedor)
                .ToList();
        }

        public IList<ClienteRankingResumen> ObtenerMejoresClientes(ReportePeriodoFiltro filtro)
        {
            var pedidos = FiltrarPedidosPorPeriodo(ObtenerPedidos(), filtro);

            return pedidos
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
                .ToList();
        }

        public IList<CuentaPorCobrarResumen> ObtenerClientesConSaldo()
        {
            var pedidos = ObtenerPedidos();

            return pedidos
                .Where(p => p.SaldoPendiente > 0)
                .GroupBy(p => new { p.IdCliente, Nombre = p.Cliente?.RazonSocial ?? "Sin cliente" })
                .Select(g => new CuentaPorCobrarResumen
                {
                    IdCliente = g.Key.IdCliente,
                    Cliente = g.Key.Nombre,
                    SaldoPendiente = g.Sum(p => p.SaldoPendiente)
                })
                .OrderByDescending(r => r.SaldoPendiente)
                .ThenBy(r => r.Cliente)
                .ToList();
        }

        private List<Pedido> ObtenerPedidos()
        {
            return _unitOfWork.Pedidos.GetAll()?.ToList() ?? new List<Pedido>();
        }

        private List<FacturaCabecera> ObtenerFacturas()
        {
            return _unitOfWork.FacturasCabecera.GetAll()?.ToList() ?? new List<FacturaCabecera>();
        }

        private static IEnumerable<Pedido> FiltrarPedidosPorPeriodo(IEnumerable<Pedido> pedidos, ReportePeriodoFiltro filtro)
        {
            if (pedidos == null)
            {
                return Enumerable.Empty<Pedido>();
            }

            if (filtro == null || filtro.Tipo == ReportePeriodoTipo.Todos)
            {
                return pedidos;
            }

            if (filtro.Tipo == ReportePeriodoTipo.Mensual)
            {
                var anio = filtro.Anio ?? DateTime.Today.Year;
                var mes = filtro.Mes ?? DateTime.Today.Month;
                return pedidos.Where(p => p.FechaCreacion.Year == anio && p.FechaCreacion.Month == mes);
            }

            if (filtro.Tipo == ReportePeriodoTipo.Anual)
            {
                var anio = filtro.Anio ?? DateTime.Today.Year;
                return pedidos.Where(p => p.FechaCreacion.Year == anio);
            }

            return pedidos;
        }

        private static IEnumerable<Pedido> FiltrarPedidosPorVentas(IEnumerable<Pedido> pedidos, VentasPeriodoFiltro filtro)
        {
            if (pedidos == null)
            {
                return Enumerable.Empty<Pedido>();
            }

            if (filtro == null)
            {
                return pedidos;
            }

            switch (filtro.Tipo)
            {
                case ReporteVentasPeriodoTipo.Rango:
                    var desde = (filtro.Desde ?? DateTime.Today.AddMonths(-1)).Date;
                    var hasta = (filtro.Hasta ?? DateTime.Today).Date.AddDays(1);
                    return pedidos.Where(p => p.FechaCreacion >= desde && p.FechaCreacion < hasta);

                case ReporteVentasPeriodoTipo.Mensual:
                    var anioMes = filtro.Anio ?? DateTime.Today.Year;
                    var mes = filtro.Mes ?? DateTime.Today.Month;
                    return pedidos.Where(p => p.FechaCreacion.Year == anioMes && p.FechaCreacion.Month == mes);

                default:
                    var anio = filtro.Anio ?? DateTime.Today.Year;
                    return pedidos.Where(p => p.FechaCreacion.Year == anio);
            }
        }
    }
}