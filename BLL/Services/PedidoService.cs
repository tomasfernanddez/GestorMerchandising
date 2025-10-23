using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Helpers;
using BLL.Interfaces;
using DAL;
using DAL.Interfaces.Base;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private const decimal IVA_DEFAULT = 0.21m;

        public PedidoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Pedido> ObtenerPedidos(PedidoFiltro filtro = null)
        {
            filtro ??= new PedidoFiltro();
            IEnumerable<Pedido> pedidos = _unitOfWork.Pedidos.GetAll();

            if (!string.IsNullOrWhiteSpace(filtro.NumeroPedido))
            {
                var termino = filtro.NumeroPedido.Trim().ToLower();
                pedidos = pedidos.Where(p => p.NumeroPedido != null && p.NumeroPedido.ToLower().Contains(termino));
            }

            if (filtro.IdCliente.HasValue)
            {
                pedidos = pedidos.Where(p => p.IdCliente == filtro.IdCliente.Value);
            }

            if (filtro.IdEstado.HasValue)
            {
                pedidos = pedidos.Where(p => p.IdEstadoPedido == filtro.IdEstado.Value);
            }

            if (filtro.Facturado.HasValue)
            {
                pedidos = pedidos.Where(p => p.Facturado == filtro.Facturado.Value);
            }

            if (filtro.ConSaldoPendiente.HasValue)
            {
                pedidos = pedidos.Where(p =>
                    filtro.ConSaldoPendiente.Value ? p.SaldoPendiente > 0 : p.SaldoPendiente <= 0);
            }

            if (filtro.FechaDesde.HasValue)
            {
                pedidos = pedidos.Where(p => p.FechaCreacion.Date >= filtro.FechaDesde.Value.Date);
            }

            if (filtro.FechaHasta.HasValue)
            {
                pedidos = pedidos.Where(p => p.FechaCreacion.Date <= filtro.FechaHasta.Value.Date);
            }

            pedidos = pedidos.OrderByDescending(p => p.FechaCreacion);

            if (filtro.IncluirDetalles)
            {
                // Aseguramos que los pedidos tengan la última información desde la base
                pedidos = pedidos
                    .Select(p => _unitOfWork.Pedidos.GetPedidoConDetalles(p.IdPedido))
                    .Where(p => p != null)
                    .ToList();
            }

            return pedidos.ToList();
        }

        public Pedido ObtenerPedido(Guid idPedido, bool incluirDetalles = true)
        {
            if (idPedido == Guid.Empty)
                return null;

            return incluirDetalles
                ? _unitOfWork.Pedidos.GetPedidoConDetalles(idPedido)
                : _unitOfWork.Pedidos.GetById(idPedido);
        }

        public ResultadoOperacion CrearPedido(Pedido pedido)
        {
            if (pedido == null)
                return ResultadoOperacion.Error("Pedido inválido");

            try
            {
                PrepararPedido(pedido, esNuevo: true);

                _unitOfWork.Pedidos.Add(pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Pedido creado correctamente", pedido.IdPedido);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el pedido: {ex.Message}");
            }
        }

        public ResultadoOperacion ActualizarPedido(Pedido pedido)
        {
            if (pedido == null || pedido.IdPedido == Guid.Empty)
                return ResultadoOperacion.Error("Pedido inválido");

            try
            {
                var existente = _unitOfWork.Pedidos.GetPedidoConDetalles(pedido.IdPedido);
                if (existente == null)
                    return ResultadoOperacion.Error("El pedido no existe");

                // Sincronizar datos básicos
                existente.IdCliente = pedido.IdCliente;
                existente.NumeroPedido = string.IsNullOrWhiteSpace(pedido.NumeroPedido)
                    ? existente.NumeroPedido
                    : pedido.NumeroPedido.Trim();
                existente.FechaLimiteEntrega = pedido.FechaLimiteEntrega;
                existente.Observaciones = pedido.Observaciones;
                existente.Cliente_OC = pedido.Cliente_OC;
                existente.Cliente_PersonaNombre = pedido.Cliente_PersonaNombre;
                existente.Cliente_PersonaEmail = pedido.Cliente_PersonaEmail;
                existente.Cliente_PersonaTelefono = pedido.Cliente_PersonaTelefono;
                existente.Cliente_DireccionEntrega = pedido.Cliente_DireccionEntrega;
                existente.IdTipoPago = pedido.IdTipoPago;
                existente.IdEstadoPedido = pedido.IdEstadoPedido;
                existente.NumeroRemito = pedido.NumeroRemito;
                existente.Facturado = pedido.Facturado;
                existente.RutaFacturaPdf = pedido.RutaFacturaPdf;
                existente.MontoPagado = pedido.MontoPagado;

                // Reemplazar notas
                existente.Notas.Clear();
                foreach (var nota in pedido.Notas ?? new List<PedidoNota>())
                {
                    nota.IdNota = nota.IdNota == Guid.Empty ? Guid.NewGuid() : nota.IdNota;
                    nota.IdPedido = existente.IdPedido;
                    existente.Notas.Add(nota);
                }

                // Reemplazar historial de estados si llega información nueva
                if (pedido.HistorialEstados != null && pedido.HistorialEstados.Any())
                {
                    existente.HistorialEstados.Clear();
                    foreach (var hist in pedido.HistorialEstados)
                    {
                        hist.IdHistorial = hist.IdHistorial == Guid.Empty ? Guid.NewGuid() : hist.IdHistorial;
                        hist.IdPedido = existente.IdPedido;
                        if (hist.FechaCambio == default)
                            hist.FechaCambio = DateTime.UtcNow;
                        existente.HistorialEstados.Add(hist);
                    }
                }

                // Sincronizar detalles: eliminar los faltantes y actualizar/crear
                SincronizarDetalles(existente, pedido.Detalles ?? new List<PedidoDetalle>());

                RecalcularTotales(existente);
                _unitOfWork.Pedidos.Update(existente);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Pedido actualizado correctamente", existente.IdPedido);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el pedido: {ex.Message}");
            }
        }

        public ResultadoOperacion CambiarEstado(Guid idPedido, Guid idEstado, string comentario, string usuario)
        {
            if (idPedido == Guid.Empty || idEstado == Guid.Empty)
                return ResultadoOperacion.Error("Datos inválidos para el cambio de estado");

            try
            {
                var pedido = _unitOfWork.Pedidos.GetPedidoConDetalles(idPedido);
                if (pedido == null)
                    return ResultadoOperacion.Error("Pedido inexistente");

                pedido.IdEstadoPedido = idEstado;

                var historial = new PedidoEstadoHistorial
                {
                    IdPedido = pedido.IdPedido,
                    IdEstadoPedido = idEstado,
                    Comentario = comentario,
                    Usuario = usuario,
                    FechaCambio = DateTime.UtcNow
                };

                pedido.HistorialEstados.Add(historial);

                _unitOfWork.Pedidos.Update(pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Estado del pedido actualizado", pedido.IdPedido);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al cambiar el estado del pedido: {ex.Message}");
            }
        }

        public ResultadoOperacion RegistrarNota(Guid idPedido, string nota, string usuario)
        {
            if (idPedido == Guid.Empty || string.IsNullOrWhiteSpace(nota))
                return ResultadoOperacion.Error("Datos inválidos para registrar la nota");

            try
            {
                var pedido = _unitOfWork.Pedidos.GetPedidoConDetalles(idPedido);
                if (pedido == null)
                    return ResultadoOperacion.Error("Pedido inexistente");

                pedido.Notas.Add(new PedidoNota
                {
                    IdPedido = pedido.IdPedido,
                    Nota = nota.Trim(),
                    Usuario = usuario,
                    Fecha = DateTime.UtcNow
                });

                _unitOfWork.Pedidos.Update(pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Nota registrada correctamente", pedido.IdPedido);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al registrar la nota: {ex.Message}");
            }
        }

        public string GenerarProximoNumeroPedido()
        {
            var pedidos = _unitOfWork.Pedidos.GetAll();
            var max = pedidos
                .Select(p => ParseNumeroPedido(p.NumeroPedido))
                .DefaultIfEmpty(0)
                .Max();

            return $"PED-{(max + 1).ToString("D4")}";
        }

        public IEnumerable<EstadoPedido> ObtenerEstadosPedido()
        {
            return _unitOfWork.EstadosPedido.GetEstadosOrdenados();
        }

        public IEnumerable<EstadoProducto> ObtenerEstadosProducto()
        {
            return _unitOfWork.EstadosProducto.GetEstadosOrdenados();
        }

        public IEnumerable<TipoPago> ObtenerTiposPago()
        {
            var ctx = ObtenerContexto();
            return ctx?.TiposPago
                .OrderBy(t => t.NombreTipoPago)
                .ToList() ?? new List<TipoPago>();
        }

        public IEnumerable<TecnicaPersonalizacion> ObtenerTecnicasPersonalizacion()
        {
            var ctx = ObtenerContexto();
            return ctx?.TecnicasPersonalizacion
                .OrderBy(t => t.NombreTecnicaPersonalizacion)
                .ToList() ?? new List<TecnicaPersonalizacion>();
        }

        public IEnumerable<UbicacionLogo> ObtenerUbicacionesLogo()
        {
            var ctx = ObtenerContexto();
            return ctx?.UbicacionesLogo
                .OrderBy(u => u.NombreUbicacionLogo)
                .ToList() ?? new List<UbicacionLogo>();
        }

        private void PrepararPedido(Pedido pedido, bool esNuevo)
        {
            if (esNuevo)
            {
                pedido.IdPedido = pedido.IdPedido == Guid.Empty ? Guid.NewGuid() : pedido.IdPedido;
                pedido.FechaCreacion = DateTime.UtcNow;
            }

            if (string.IsNullOrWhiteSpace(pedido.NumeroPedido))
            {
                pedido.NumeroPedido = GenerarProximoNumeroPedido();
            }
            else
            {
                pedido.NumeroPedido = pedido.NumeroPedido.Trim();
            }

            if (pedido.Detalles == null)
            {
                pedido.Detalles = new List<PedidoDetalle>();
            }

            foreach (var detalle in pedido.Detalles)
            {
                PrepararDetalle(pedido, detalle);
            }

            if (pedido.HistorialEstados == null)
            {
                pedido.HistorialEstados = new List<PedidoEstadoHistorial>();
            }

            if (!pedido.HistorialEstados.Any() && pedido.IdEstadoPedido.HasValue)
            {
                pedido.HistorialEstados.Add(new PedidoEstadoHistorial
                {
                    IdPedido = pedido.IdPedido,
                    IdEstadoPedido = pedido.IdEstadoPedido.Value,
                    FechaCambio = DateTime.UtcNow,
                    Usuario = "Sistema"
                });
            }

            RecalcularTotales(pedido);
        }

        private void PrepararDetalle(Pedido pedido, PedidoDetalle detalle)
        {
            if (detalle == null)
                return;

            detalle.IdDetallePedido = detalle.IdDetallePedido == Guid.Empty ? Guid.NewGuid() : detalle.IdDetallePedido;
            detalle.IdPedido = pedido.IdPedido;
            detalle.PrecioUnitario = Math.Round(detalle.PrecioUnitario, 2);
            detalle.Notas = detalle.Notas?.Trim();

            var producto = ResolverProducto(detalle.Producto, detalle);
            detalle.IdProducto = producto.IdProducto;
            detalle.Producto = producto;

            if (detalle.LogosPedido == null)
            {
                detalle.LogosPedido = new List<LogosPedido>();
            }

            foreach (var logo in detalle.LogosPedido)
            {
                logo.IdLogoPedido = logo.IdLogoPedido == Guid.Empty ? Guid.NewGuid() : logo.IdLogoPedido;
                logo.IdDetallePedido = detalle.IdDetallePedido;
                if (logo.Cantidad <= 0)
                    logo.Cantidad = 1;
            }
        }

        private Producto ResolverProducto(Producto productoEntrada, PedidoDetalle detalle)
        {
            if (productoEntrada == null && detalle?.Producto == null)
                throw new InvalidOperationException("Debe especificarse un producto para el detalle del pedido");

            var nombre = productoEntrada?.NombreProducto ?? detalle?.Producto?.NombreProducto;
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre del producto es obligatorio");

            var existente = _unitOfWork.Productos.ObtenerPorNombreExacto(nombre);
            if (existente != null)
            {
                _unitOfWork.Productos.RegistrarUso(existente.IdProducto);
                return existente;
            }

            var categoria = productoEntrada?.IdCategoria ?? detalle?.Producto?.IdCategoria;
            var proveedor = productoEntrada?.IdProveedor ?? detalle?.Producto?.IdProveedor;
            if (!categoria.HasValue)
                throw new InvalidOperationException("Debe seleccionar una categoría para el nuevo producto");
            if (!proveedor.HasValue)
                throw new InvalidOperationException("Debe seleccionar un proveedor para el nuevo producto");

            var nuevo = new Producto
            {
                IdProducto = Guid.NewGuid(),
                NombreProducto = nombre.Trim(),
                IdCategoria = categoria,
                IdProveedor = proveedor,
                Activo = true,
                FechaCreacion = DateTime.UtcNow,
                FechaUltimoUso = DateTime.UtcNow,
                VecesUsado = 1
            };

            _unitOfWork.Productos.Add(nuevo);
            return nuevo;
        }

        private void SincronizarDetalles(Pedido destino, IEnumerable<PedidoDetalle> nuevosDetalles)
        {
            var nuevos = nuevosDetalles.ToList();
            var existentes = destino.Detalles.ToList();

            // Eliminar detalles que ya no existen
            foreach (var detalleExistente in existentes)
            {
                if (!nuevos.Any(d => d.IdDetallePedido == detalleExistente.IdDetallePedido))
                {
                    destino.Detalles.Remove(detalleExistente);
                }
            }

            foreach (var detalleNuevo in nuevos)
            {
                var existente = destino.Detalles.FirstOrDefault(d => d.IdDetallePedido == detalleNuevo.IdDetallePedido);
                if (existente == null)
                {
                    PrepararDetalle(destino, detalleNuevo);
                    destino.Detalles.Add(detalleNuevo);
                }
                else
                {
                    existente.Cantidad = detalleNuevo.Cantidad;
                    existente.PrecioUnitario = Math.Round(detalleNuevo.PrecioUnitario, 2);
                    existente.FichaAplicacion = detalleNuevo.FichaAplicacion;
                    existente.IdEstadoProducto = detalleNuevo.IdEstadoProducto;
                    existente.FechaLimiteProduccion = detalleNuevo.FechaLimiteProduccion;
                    existente.Notas = detalleNuevo.Notas?.Trim();
                    existente.IdProveedorPersonalizacion = detalleNuevo.IdProveedorPersonalizacion;

                    var productoActualizado = ResolverProducto(detalleNuevo.Producto ?? existente.Producto, detalleNuevo);
                    existente.IdProducto = productoActualizado.IdProducto;
                    existente.Producto = productoActualizado;

                    // Reemplazar logos
                    existente.LogosPedido.Clear();
                    foreach (var logo in detalleNuevo.LogosPedido ?? new List<LogosPedido>())
                    {
                        logo.IdLogoPedido = logo.IdLogoPedido == Guid.Empty ? Guid.NewGuid() : logo.IdLogoPedido;
                        logo.IdDetallePedido = existente.IdDetallePedido;
                        if (logo.Cantidad <= 0)
                            logo.Cantidad = 1;
                        existente.LogosPedido.Add(logo);
                    }
                }
            }
        }

        private void RecalcularTotales(Pedido pedido)
        {
            var totalProductos = pedido.Detalles
                .Select(d => d.Cantidad * d.PrecioUnitario)
                .DefaultIfEmpty(0m)
                .Sum();

            var totalLogos = pedido.Detalles
                .SelectMany(d => d.LogosPedido ?? new List<LogosPedido>())
                .Select(l => l.CostoPersonalizacion * (l.Cantidad <= 0 ? 1 : l.Cantidad))
                .DefaultIfEmpty(0m)
                .Sum();

            pedido.TotalSinIva = Math.Round(totalProductos + totalLogos, 2);
            pedido.MontoIva = Math.Round(pedido.TotalSinIva * IVA_DEFAULT, 2);
            pedido.TotalConIva = Math.Round(pedido.TotalSinIva + pedido.MontoIva, 2);
            pedido.SaldoPendiente = Math.Max(0, Math.Round(pedido.TotalConIva - pedido.MontoPagado, 2));
        }

        private static int ParseNumeroPedido(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                return 0;

            var partes = numero.Split('-');
            if (partes.Length != 2)
                return 0;

            return int.TryParse(partes[1], out var valor) ? valor : 0;
        }

        private GestorMerchandisingContext ObtenerContexto()
        {
            if (_unitOfWork is IHasDbContext hasDb)
            {
                return hasDb.Context as GestorMerchandisingContext;
            }

            return null;
        }
    }
}