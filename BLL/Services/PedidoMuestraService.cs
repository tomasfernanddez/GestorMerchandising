using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Interfaces.Base;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Services
{
    public class PedidoMuestraService : IPedidoMuestraService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Dictionary<string, Guid> _estadoMuestraCache = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Guid, string> _estadoMuestraNombreCache = new Dictionary<Guid, string>();

        public PedidoMuestraService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<PedidoMuestra> ObtenerPedidosMuestra(PedidoMuestraFiltro filtro = null)
        {
            if (filtro == null)
            {
                filtro = new PedidoMuestraFiltro();
            }

            IEnumerable<PedidoMuestra> pedidos = _unitOfWork.PedidosMuestra.GetAll();

            if (!string.IsNullOrWhiteSpace(filtro.TextoBusqueda))
            {
                var texto = filtro.TextoBusqueda.Trim();
                pedidos = pedidos.Where(p =>
                    (!string.IsNullOrEmpty(p.Cliente?.RazonSocial) && p.Cliente.RazonSocial.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(p.PersonaContacto) && p.PersonaContacto.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(p.EmailContacto) && p.EmailContacto.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(p.Observaciones) && p.Observaciones.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    p.Detalles.Any(d => !string.IsNullOrEmpty(d.Producto?.NombreProducto) && d.Producto.NombreProducto.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            if (filtro.IdCliente.HasValue)
            {
                pedidos = pedidos.Where(p => p.IdCliente == filtro.IdCliente.Value);
            }

            if (filtro.IdEstadoPedido.HasValue)
            {
                pedidos = pedidos.Where(p => p.IdEstadoPedidoMuestra == filtro.IdEstadoPedido.Value);
            }

            if (filtro.Facturado.HasValue)
            {
                pedidos = pedidos.Where(p => p.Facturado == filtro.Facturado.Value);
            }

            if (filtro.ConSaldoPendiente.HasValue)
            {
                pedidos = pedidos.Where(p => filtro.ConSaldoPendiente.Value ? p.SaldoPendiente > 0 : p.SaldoPendiente <= 0);
            }

            if (filtro.FechaDesde.HasValue)
            {
                var desde = filtro.FechaDesde.Value.Date;
                pedidos = pedidos.Where(p => p.FechaCreacion.Date >= desde);
            }

            if (filtro.FechaHasta.HasValue)
            {
                var hasta = filtro.FechaHasta.Value.Date;
                pedidos = pedidos.Where(p => p.FechaCreacion.Date <= hasta);
            }

            pedidos = pedidos.OrderByDescending(p => p.FechaCreacion);

            if (filtro.IncluirDetalles)
            {
                pedidos = pedidos
                    .Select(p => _unitOfWork.PedidosMuestra.GetMuestraConDetalles(p.IdPedidoMuestra))
                    .Where(p => p != null)
                    .ToList();
            }

            return pedidos.ToList();
        }

        public PedidoMuestra ObtenerPedidoMuestra(Guid idPedidoMuestra, bool incluirDetalles = true)
        {
            if (idPedidoMuestra == Guid.Empty)
                return null;

            return incluirDetalles
                ? _unitOfWork.PedidosMuestra.GetMuestraConDetalles(idPedidoMuestra)
                : _unitOfWork.PedidosMuestra.GetById(idPedidoMuestra);
        }

        public ResultadoOperacion CrearPedidoMuestra(PedidoMuestra pedido)
        {
            if (pedido == null)
                return ResultadoOperacion.Error("Pedido de muestra inválido");

            try
            {
                PrepararPedido(pedido, true);

                _unitOfWork.PedidosMuestra.Add(pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Pedido de muestra creado", pedido.IdPedidoMuestra);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el pedido de muestra: {ObtenerMensajeProfundo(ex)}");
            }
        }

        public ResultadoOperacion ActualizarPedidoMuestra(PedidoMuestra pedido)
        {
            if (pedido == null || pedido.IdPedidoMuestra == Guid.Empty)
                return ResultadoOperacion.Error("Pedido de muestra inválido");

            try
            {
                var existente = _unitOfWork.PedidosMuestra.GetMuestraConDetalles(pedido.IdPedidoMuestra);
                if (existente == null)
                    return ResultadoOperacion.Error("El pedido de muestra no existe");

                existente.IdCliente = pedido.IdCliente;
                existente.FechaEntrega = pedido.FechaEntrega;
                existente.FechaDevolucionEsperada = pedido.FechaDevolucionEsperada;
                existente.FechaDevolucion = pedido.FechaDevolucion;
                existente.DireccionEntrega = pedido.DireccionEntrega?.Trim();
                existente.PersonaContacto = pedido.PersonaContacto?.Trim();
                existente.EmailContacto = pedido.EmailContacto?.Trim();
                existente.TelefonoContacto = pedido.TelefonoContacto?.Trim();
                existente.Observaciones = pedido.Observaciones?.Trim();
                existente.Facturado = pedido.Facturado;
                existente.RutaFacturaPdf = pedido.RutaFacturaPdf;
                existente.IdEstadoPedidoMuestra = pedido.IdEstadoPedidoMuestra;

                SincronizarDetalles(existente, pedido);

                existente.MontoPagado = pedido.MontoPagado;

                PrepararPedido(existente, false);

                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Pedido de muestra actualizado", existente.IdPedidoMuestra);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el pedido de muestra: {ObtenerMensajeProfundo(ex)}");
            }
        }

        public ResultadoOperacion CancelarPedidoMuestra(Guid idPedidoMuestra, string usuario, string comentario = null)
        {
            if (idPedidoMuestra == Guid.Empty)
                return ResultadoOperacion.Error("Datos inválidos para cancelar el pedido de muestra");

            try
            {
                var pedido = _unitOfWork.PedidosMuestra.GetMuestraConDetalles(idPedidoMuestra);
                if (pedido == null)
                    return ResultadoOperacion.Error("Pedido de muestra inexistente");

                var estados = _unitOfWork.EstadosPedidoMuestra?.GetEstadosOrdenados();
                var estadoCancelado = estados?.FirstOrDefault(e =>
                    string.Equals(e.NombreEstadoPedidoMuestra, "Cancelado", StringComparison.OrdinalIgnoreCase));
                if (estadoCancelado == null)
                    return ResultadoOperacion.Error("Estado 'Cancelado' no disponible");

                if (pedido.IdEstadoPedidoMuestra.HasValue && pedido.IdEstadoPedidoMuestra.Value == estadoCancelado.IdEstadoPedidoMuestra)
                    return ResultadoOperacion.Error("El pedido de muestra ya se encuentra cancelado");

                pedido.IdEstadoPedidoMuestra = estadoCancelado.IdEstadoPedidoMuestra;
                pedido.Facturado = false;

                if (pedido.Detalles != null)
                {
                    foreach (var detalle in pedido.Detalles)
                    {
                        detalle.Subtotal = 0m;
                        if (detalle.IdEstadoMuestra.HasValue)
                            continue;

                        var estadoPendiente = ObtenerEstadoMuestraPorNombre("Pendiente de Envío");
                        if (estadoPendiente.HasValue)
                        {
                            detalle.IdEstadoMuestra = estadoPendiente.Value;
                        }
                    }
                }

                pedido.MontoTotal = Math.Round(pedido.Detalles?.Sum(d => d.Subtotal) ?? 0m, 2);
                pedido.MontoPagado = pedido.MontoTotal;
                pedido.SaldoPendiente = 0m;

                _unitOfWork.PedidosMuestra.Update(pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Pedido de muestra cancelado", pedido.IdPedidoMuestra);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al cancelar el pedido de muestra: {ObtenerMensajeProfundo(ex)}");
            }
        }

        public IEnumerable<EstadoPedidoMuestra> ObtenerEstadosPedido()
        {
            return _unitOfWork.EstadosPedidoMuestra.GetEstadosOrdenados();
        }

        public IEnumerable<EstadoMuestra> ObtenerEstadosMuestra()
        {
            return _unitOfWork.EstadosMuestra.GetEstadosOrdenados();
        }

        private void PrepararPedido(PedidoMuestra pedido, bool esNuevo)
        {
            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido));

            if (esNuevo && pedido.IdPedidoMuestra == Guid.Empty)
            {
                pedido.IdPedidoMuestra = Guid.NewGuid();
            }

            if (pedido.Detalles == null)
            {
                pedido.Detalles = new List<DetalleMuestra>();
            }

            var detallesValidos = new List<DetalleMuestra>();
            foreach (var detalle in pedido.Detalles)
            {
                if (detalle.IdDetalleMuestra == Guid.Empty)
                {
                    detalle.IdDetalleMuestra = Guid.NewGuid();
                }

                detalle.IdPedidoMuestra = pedido.IdPedidoMuestra;
                if (detalle.Cantidad <= 0)
                {
                    detalle.Cantidad = 1;
                }

                detalle.Subtotal = CalcularSubtotal(detalle);

                if (!detalle.IdEstadoMuestra.HasValue)
                {
                    detalle.IdEstadoMuestra = ObtenerEstadoMuestraPorNombre("Pendiente de Envío");
                }

                detallesValidos.Add(detalle);
            }

            pedido.Detalles = detallesValidos;

            if (!pedido.FechaDevolucionEsperada.HasValue)
            {
                var baseDate = pedido.FechaEntrega ?? pedido.FechaCreacion;
                if (baseDate == default)
                {
                    baseDate = DateTime.UtcNow;
                }
                pedido.FechaDevolucionEsperada = baseDate.AddDays(7);
            }

            pedido.MontoTotal = Math.Round(pedido.Detalles.Sum(d => d.Subtotal), 2);
            pedido.MontoPagado = Math.Max(0, Math.Round(pedido.MontoPagado, 2));
            if (pedido.MontoPagado > pedido.MontoTotal)
            {
                pedido.MontoPagado = pedido.MontoTotal;
            }

            pedido.SaldoPendiente = Math.Max(0, Math.Round(pedido.MontoTotal - pedido.MontoPagado, 2));
        }

        private void SincronizarDetalles(PedidoMuestra existente, PedidoMuestra pedido)
        {
            var ctx = ObtenerContexto();
            var nuevos = pedido.Detalles?.ToList() ?? new List<DetalleMuestra>();
            var actuales = existente.Detalles?.ToList() ?? new List<DetalleMuestra>();
            var procesados = new HashSet<Guid>();

            foreach (var actual in actuales)
            {
                var entrante = nuevos.FirstOrDefault(d => d.IdDetalleMuestra != Guid.Empty && d.IdDetalleMuestra == actual.IdDetalleMuestra);
                if (entrante == null)
                {
                    if (ctx != null)
                    {
                        ctx.Set<DetalleMuestra>().Remove(actual);
                    }

                    existente.Detalles.Remove(actual);
                    continue;
                }

                actual.IdProducto = entrante.IdProducto;
                actual.Cantidad = entrante.Cantidad <= 0 ? 1 : entrante.Cantidad;
                actual.PrecioUnitario = entrante.PrecioUnitario;
                actual.Subtotal = CalcularSubtotal(actual);
                actual.IdEstadoMuestra = entrante.IdEstadoMuestra ?? ObtenerEstadoMuestraPorNombre("Pendiente de Envío");
                actual.FechaDevolucion = entrante.FechaDevolucion;

                procesados.Add(actual.IdDetalleMuestra);
            }

            foreach (var entrante in nuevos)
            {
                if (entrante.IdDetalleMuestra != Guid.Empty && procesados.Contains(entrante.IdDetalleMuestra))
                    continue;

                entrante.IdDetalleMuestra = entrante.IdDetalleMuestra == Guid.Empty ? Guid.NewGuid() : entrante.IdDetalleMuestra;
                entrante.IdPedidoMuestra = existente.IdPedidoMuestra;
                if (entrante.Cantidad <= 0)
                    entrante.Cantidad = 1;
                entrante.Subtotal = CalcularSubtotal(entrante);
                entrante.IdEstadoMuestra = entrante.IdEstadoMuestra ?? ObtenerEstadoMuestraPorNombre("Pendiente de Envío");

                existente.Detalles.Add(entrante);
            }
        }

        private decimal CalcularSubtotal(DetalleMuestra detalle)
        {
            if (detalle == null)
                return 0m;

            var monto = Math.Round(Math.Max(1, detalle.Cantidad) * detalle.PrecioUnitario, 2);
            var estado = detalle.EstadoMuestra?.NombreEstadoMuestra ?? ObtenerNombreEstado(detalle.IdEstadoMuestra);

            if (string.Equals(estado, "Facturar", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(estado, "Facturado", StringComparison.OrdinalIgnoreCase))
            {
                return monto;
            }

            return 0m;
        }

        private string ObtenerNombreEstado(Guid? idEstado)
        {
            if (!idEstado.HasValue || idEstado.Value == Guid.Empty)
                return string.Empty;

            if (_estadoMuestraNombreCache.TryGetValue(idEstado.Value, out var nombre))
                return nombre;

            var estados = _unitOfWork.EstadosMuestra.GetEstadosOrdenados();
            var estado = estados.FirstOrDefault(e => e.IdEstadoMuestra == idEstado.Value);
            if (estado != null)
            {
                _estadoMuestraNombreCache[idEstado.Value] = estado.NombreEstadoMuestra;
                if (!_estadoMuestraCache.ContainsKey(estado.NombreEstadoMuestra))
                {
                    _estadoMuestraCache[estado.NombreEstadoMuestra] = estado.IdEstadoMuestra;
                }
                return estado.NombreEstadoMuestra;
            }

            return string.Empty;
        }

        private Guid? ObtenerEstadoMuestraPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return null;

            if (_estadoMuestraCache.TryGetValue(nombre, out var id))
                return id;

            var estados = _unitOfWork.EstadosMuestra.GetEstadosOrdenados();
            var estado = estados.FirstOrDefault(e => string.Equals(e.NombreEstadoMuestra, nombre, StringComparison.OrdinalIgnoreCase));
            if (estado != null)
            {
                _estadoMuestraCache[nombre] = estado.IdEstadoMuestra;
                return estado.IdEstadoMuestra;
            }

            return null;
        }

        private DbContext ObtenerContexto()
        {
            return (_unitOfWork as IHasDbContext)?.Context;
        }

        private static string ObtenerMensajeProfundo(Exception ex)
        {
            if (ex == null)
                return string.Empty;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex.Message;
        }
    }
}