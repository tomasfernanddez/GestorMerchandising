using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BLL.Helpers;
using BLL.Interfaces;
using DAL;
using DAL.Interfaces.Base;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Services
{
    public class PedidoMuestraService : IPedidoMuestraService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PedidoMuestraService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<PedidoMuestra> ObtenerPedidos(PedidoMuestraFiltro filtro = null)
        {
            filtro ??= new PedidoMuestraFiltro();

            IEnumerable<PedidoMuestra> query = _unitOfWork.PedidosMuestra.GetAll();

            if (!string.IsNullOrWhiteSpace(filtro.Numero))
            {
                var numero = filtro.Numero.Trim();
                query = query.Where(pm =>
                    (!string.IsNullOrWhiteSpace(pm.NumeroCorrelativo) && pm.NumeroCorrelativo.IndexOf(numero, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (pm.Cliente?.RazonSocial ?? string.Empty).IndexOf(numero, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (filtro.IdCliente.HasValue)
            {
                query = query.Where(pm => pm.IdCliente == filtro.IdCliente.Value);
            }

            if (filtro.IdEstado.HasValue)
            {
                query = query.Where(pm => pm.IdEstadoPedidoMuestra == filtro.IdEstado.Value);
            }

            if (filtro.Facturado.HasValue)
            {
                query = query.Where(pm => pm.Facturado == filtro.Facturado.Value);
            }

            if (filtro.SoloVencidos)
            {
                var hoy = DateTime.UtcNow.Date;
                query = query.Where(pm => pm.FechaDevolucionEsperada.HasValue && pm.FechaDevolucionEsperada.Value.Date < hoy && !pm.Facturado);
            }

            query = query.OrderByDescending(pm => pm.FechaCreacion);

            if (filtro.IncluirDetalles)
            {
                return query.Select(pm => _unitOfWork.PedidosMuestra.GetMuestraConDetalles(pm.IdPedidoMuestra)).Where(pm => pm != null).ToList();
            }

            return query.ToList();
        }

        public PedidoMuestra ObtenerPedido(Guid idPedido, bool incluirDetalles = true)
        {
            if (idPedido == Guid.Empty)
                return null;

            return incluirDetalles
                ? _unitOfWork.PedidosMuestra.GetMuestraConDetalles(idPedido)
                : _unitOfWork.PedidosMuestra.GetById(idPedido);
        }

        public ResultadoOperacion CrearPedido(PedidoMuestra pedido)
        {
            if (pedido == null)
                return ResultadoOperacion.Error("Pedido inválido");

            try
            {
                NormalizarPedido(pedido);

                _unitOfWork.PedidosMuestra.Add(pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Pedido de muestra creado correctamente", pedido.IdPedidoMuestra);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el pedido de muestra: {ex.Message}");
            }
        }

        public ResultadoOperacion ActualizarPedido(PedidoMuestra pedido)
        {
            if (pedido == null || pedido.IdPedidoMuestra == Guid.Empty)
                return ResultadoOperacion.Error("Pedido inválido");

            try
            {
                var existente = _unitOfWork.PedidosMuestra.GetMuestraConDetalles(pedido.IdPedidoMuestra);
                if (existente == null)
                    return ResultadoOperacion.Error("El pedido de muestra no existe");

                SincronizarPedido(existente, pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Pedido de muestra actualizado", existente.IdPedidoMuestra);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el pedido de muestra: {ex.Message}");
            }
        }

        public ResultadoOperacion RegistrarDevolucion(Guid idDetalle, Guid? idEstado, DateTime? fechaDevolucion, decimal? precioFacturacion, string comentario, bool marcarFacturado)
        {
            if (idDetalle == Guid.Empty)
                return ResultadoOperacion.Error("Detalle inválido");

            try
            {
                var pedido = BuscarPedidoPorDetalle(idDetalle);
                if (pedido == null)
                    return ResultadoOperacion.Error("El detalle no existe en ningún pedido");

                var detalle = pedido.Detalles.FirstOrDefault(d => d.IdDetalleMuestra == idDetalle);
                if (detalle == null)
                    return ResultadoOperacion.Error("No se encontró el detalle indicado");

                detalle.IdEstadoMuestra = idEstado;
                detalle.FechaDevolucion = fechaDevolucion ?? DateTime.UtcNow;
                detalle.ComentarioDevolucion = comentario;
                detalle.PrecioFacturacion = precioFacturacion;
                detalle.Facturado = marcarFacturado;
                detalle.FechaFacturacion = marcarFacturado ? DateTime.UtcNow : (DateTime?)null;

                if (pedido.Detalles.All(d => d.FechaDevolucion.HasValue))
                {
                    pedido.FechaDevolucionReal = pedido.Detalles.Max(d => d.FechaDevolucion);
                }

                RecalcularTotales(pedido);

                _unitOfWork.SaveChanges();
                return ResultadoOperacion.Exitoso("Devolución registrada", pedido.IdPedidoMuestra);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error registrando devolución: {ex.Message}");
            }
        }

        public ResultadoOperacion FacturarPendientes(Guid idPedido, IDictionary<Guid, decimal> preciosPorDetalle, bool generarPedidoFacturacion)
        {
            if (idPedido == Guid.Empty)
                return ResultadoOperacion.Error("Pedido inválido");

            try
            {
                var pedido = _unitOfWork.PedidosMuestra.GetMuestraConDetalles(idPedido);
                if (pedido == null)
                    return ResultadoOperacion.Error("El pedido de muestra no existe");

                foreach (var detalle in pedido.Detalles)
                {
                    if (!preciosPorDetalle.TryGetValue(detalle.IdDetalleMuestra, out var precio))
                        continue;

                    detalle.PrecioFacturacion = precio;
                    detalle.Facturado = true;
                    detalle.FechaFacturacion = DateTime.UtcNow;
                }

                pedido.Facturado = pedido.Detalles.All(d => d.Facturado);
                RecalcularTotales(pedido);

                _unitOfWork.SaveChanges();

                var mensaje = generarPedidoFacturacion
                    ? "Facturación registrada y pedido de facturación generado"
                    : "Facturación registrada";

                return ResultadoOperacion.Exitoso(mensaje, pedido.IdPedidoMuestra);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al facturar muestras: {ex.Message}");
            }
        }

        public string GenerarSiguienteNumero()
        {
            var pedidos = _unitOfWork.PedidosMuestra.GetAll();
            var max = pedidos
                .Select(pm => ParseNumero(pm.NumeroCorrelativo))
                .DefaultIfEmpty(0)
                .Max();

            return (max + 1).ToString("0000", CultureInfo.InvariantCulture);
        }

        public void ActualizarDiasProrroga(Guid idPedido, int diasAdicionales)
        {
            if (idPedido == Guid.Empty)
                throw new ArgumentException("El identificador es requerido", nameof(idPedido));

            var pedido = _unitOfWork.PedidosMuestra.GetById(idPedido);
            if (pedido == null)
                throw new InvalidOperationException("El pedido no existe");

            pedido.DiasProrroga += diasAdicionales;
            if (pedido.FechaDevolucionEsperada.HasValue)
            {
                pedido.FechaDevolucionEsperada = pedido.FechaDevolucionEsperada.Value.AddDays(diasAdicionales);
            }
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<EstadoPedidoMuestra> ObtenerEstadosPedido()
        {
            return ObtenerContexto().EstadosPedidoMuestra
                .OrderBy(e => e.NombreEstadoPedidoMuestra)
                .ToList();
        }

        public IEnumerable<EstadoMuestra> ObtenerEstadosMuestra()
        {
            return ObtenerContexto().EstadosMuestra
                .OrderBy(e => e.NombreEstadoMuestra)
                .ToList();
        }

        private GestorMerchandisingContext ObtenerContexto()
        {
            if (_unitOfWork is IHasDbContext hasDb && hasDb.Context is GestorMerchandisingContext ctx)
            {
                return ctx;
            }

            throw new InvalidOperationException("El UnitOfWork no expone el contexto requerido");
        }

        private void NormalizarPedido(PedidoMuestra pedido)
        {
            pedido.NumeroCorrelativo = string.IsNullOrWhiteSpace(pedido.NumeroCorrelativo)
                ? GenerarSiguienteNumero()
                : pedido.NumeroCorrelativo.Trim();

            if (!pedido.FechaDevolucionEsperada.HasValue && pedido.FechaEntrega.HasValue)
            {
                pedido.FechaDevolucionEsperada = pedido.FechaEntrega.Value.AddDays(30);
            }

            pedido.Observaciones = pedido.Observaciones?.Trim();
            pedido.PersonaContacto = pedido.PersonaContacto?.Trim();
            pedido.EmailContacto = pedido.EmailContacto?.Trim();
            pedido.TelefonoContacto = pedido.TelefonoContacto?.Trim();
            pedido.DireccionEntrega = pedido.DireccionEntrega?.Trim();

            if (pedido.Detalles != null)
            {
                var productos = new HashSet<Guid>();
                foreach (var detalle in pedido.Detalles)
                {
                    if (!productos.Add(detalle.IdProducto))
                    {
                        throw new InvalidOperationException("No se puede repetir el producto en el pedido de muestras");
                    }

                    detalle.ComentarioDevolucion = detalle.ComentarioDevolucion?.Trim();
                }
            }

            RecalcularTotales(pedido);
        }

        private void SincronizarPedido(PedidoMuestra destino, PedidoMuestra origen)
        {
            destino.IdCliente = origen.IdCliente;
            destino.NumeroCorrelativo = string.IsNullOrWhiteSpace(origen.NumeroCorrelativo)
                ? destino.NumeroCorrelativo
                : origen.NumeroCorrelativo.Trim();
            destino.FechaEntrega = origen.FechaEntrega;
            destino.FechaDevolucionEsperada = origen.FechaDevolucionEsperada;
            destino.FechaDevolucionReal = origen.FechaDevolucionReal;
            destino.DireccionEntrega = origen.DireccionEntrega?.Trim();
            destino.PersonaContacto = origen.PersonaContacto?.Trim();
            destino.EmailContacto = origen.EmailContacto?.Trim();
            destino.TelefonoContacto = origen.TelefonoContacto?.Trim();
            destino.Observaciones = origen.Observaciones?.Trim();
            destino.IdEstadoPedidoMuestra = origen.IdEstadoPedidoMuestra;
            destino.DiasProrroga = origen.DiasProrroga;

            SincronizarDetalles(destino, origen);
            RecalcularTotales(destino);
        }

        private void SincronizarDetalles(PedidoMuestra destino, PedidoMuestra origen)
        {
            var detallesDestino = destino.Detalles.ToDictionary(d => d.IdDetalleMuestra);
            var productosUsados = new HashSet<Guid>();

            foreach (var detalle in origen.Detalles)
            {
                if (!productosUsados.Add(detalle.IdProducto))
                {
                    throw new InvalidOperationException("No se puede repetir el producto en el pedido de muestras");
                }

                if (detalle.IdDetalleMuestra == Guid.Empty || !detallesDestino.ContainsKey(detalle.IdDetalleMuestra))
                {
                    detalle.IdDetalleMuestra = detalle.IdDetalleMuestra == Guid.Empty ? Guid.NewGuid() : detalle.IdDetalleMuestra;
                    destino.Detalles.Add(detalle);
                    continue;
                }

                var existente = detallesDestino[detalle.IdDetalleMuestra];
                existente.IdProducto = detalle.IdProducto;
                existente.IdEstadoMuestra = detalle.IdEstadoMuestra;
                existente.FechaDevolucion = detalle.FechaDevolucion;
                existente.ComentarioDevolucion = detalle.ComentarioDevolucion?.Trim();
                existente.PrecioFacturacion = detalle.PrecioFacturacion;
                existente.Facturado = detalle.Facturado;
                existente.FechaFacturacion = detalle.FechaFacturacion;
            }

            var idsOrigen = new HashSet<Guid>(origen.Detalles.Select(d => d.IdDetalleMuestra));
            var eliminar = destino.Detalles.Where(d => !idsOrigen.Contains(d.IdDetalleMuestra)).ToList();
            foreach (var detalle in eliminar)
            {
                destino.Detalles.Remove(detalle);
            }
        }

        private void RecalcularTotales(PedidoMuestra pedido)
        {
            if (pedido.Detalles == null)
            {
                pedido.TotalFacturado = 0m;
                pedido.Facturado = false;
                return;
            }

            pedido.TotalFacturado = pedido.Detalles.Where(d => d.PrecioFacturacion.HasValue).Sum(d => d.PrecioFacturacion.Value);
            pedido.Facturado = pedido.Detalles.All(d => d.Facturado);
        }

        private PedidoMuestra BuscarPedidoPorDetalle(Guid idDetalle)
        {
            return _unitOfWork.PedidosMuestra.GetAll()
                .Select(pm => _unitOfWork.PedidosMuestra.GetMuestraConDetalles(pm.IdPedidoMuestra))
                .FirstOrDefault(pm => pm.Detalles.Any(d => d.IdDetalleMuestra == idDetalle));
        }

        private static int ParseNumero(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                return 0;

            return int.TryParse(numero, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
                ? value
                : 0;
        }
    }
}