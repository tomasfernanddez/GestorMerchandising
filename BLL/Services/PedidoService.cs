using BLL.Helpers;
using BLL.Interfaces;
using DAL.Interfaces.Base;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PedidoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // ============================================================================
        // CONSULTAS BÁSICAS
        // ============================================================================

        public Pedido ObtenerPedidoPorId(Guid idPedido)
        {
            if (idPedido == Guid.Empty)
                throw new ArgumentException("El ID del pedido no puede estar vacío", nameof(idPedido));

            return _unitOfWork.Pedidos.GetById(idPedido);
        }

        public async Task<Pedido> ObtenerPedidoPorIdAsync(Guid idPedido)
        {
            if (idPedido == Guid.Empty)
                throw new ArgumentException("El ID del pedido no puede estar vacío", nameof(idPedido));

            return await _unitOfWork.Pedidos.GetByIdAsync(idPedido);
        }

        public Pedido ObtenerPedidoCompleto(Guid idPedido)
        {
            if (idPedido == Guid.Empty)
                throw new ArgumentException("El ID del pedido no puede estar vacío", nameof(idPedido));

            return _unitOfWork.Pedidos.GetPedidoConDetalles(idPedido);
        }

        public async Task<Pedido> ObtenerPedidoCompletoAsync(Guid idPedido)
        {
            if (idPedido == Guid.Empty)
                throw new ArgumentException("El ID del pedido no puede estar vacío", nameof(idPedido));

            return await _unitOfWork.Pedidos.GetPedidoConDetallesAsync(idPedido);
        }

        public IEnumerable<Pedido> ObtenerTodosLosPedidos()
        {
            return _unitOfWork.Pedidos.GetAll();
        }

        public async Task<IEnumerable<Pedido>> ObtenerTodosLosPedidosAsync()
        {
            return await Task.Run(() => _unitOfWork.Pedidos.GetAll());
        }

        // ============================================================================
        // BÚSQUEDAS Y FILTROS
        // ============================================================================

        public IEnumerable<Pedido> ObtenerPedidosPorCliente(Guid idCliente)
        {
            if (idCliente == Guid.Empty)
                return new List<Pedido>();

            return _unitOfWork.Pedidos.GetPedidosPorCliente(idCliente);
        }

        public async Task<IEnumerable<Pedido>> ObtenerPedidosPorClienteAsync(Guid idCliente)
        {
            if (idCliente == Guid.Empty)
                return new List<Pedido>();

            return await _unitOfWork.Pedidos.GetPedidosPorClienteAsync(idCliente);
        }

        public IEnumerable<Pedido> ObtenerPedidosPorEstado(Guid idEstado)
        {
            if (idEstado == Guid.Empty)
                return new List<Pedido>();

            return _unitOfWork.Pedidos.GetPedidosPorEstado(idEstado);
        }

        public async Task<IEnumerable<Pedido>> ObtenerPedidosPorEstadoAsync(Guid idEstado)
        {
            if (idEstado == Guid.Empty)
                return new List<Pedido>();

            return await _unitOfWork.Pedidos.GetPedidosPorEstadoAsync(idEstado);
        }

        public IEnumerable<Pedido> ObtenerPedidosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _unitOfWork.Pedidos.GetPedidosPorFecha(fechaDesde, fechaHasta);
        }

        public async Task<IEnumerable<Pedido>> ObtenerPedidosPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            return await _unitOfWork.Pedidos.GetPedidosPorFechaAsync(fechaDesde, fechaHasta);
        }

        public IEnumerable<Pedido> ObtenerPedidosConFechaLimite()
        {
            return _unitOfWork.Pedidos.GetPedidosConFechaLimite();
        }

        public async Task<IEnumerable<Pedido>> ObtenerPedidosConFechaLimiteAsync()
        {
            return await _unitOfWork.Pedidos.GetPedidosConFechaLimiteAsync();
        }

        public IEnumerable<Pedido> ObtenerPedidosVencidos()
        {
            return _unitOfWork.Pedidos.GetPedidosVencidos();
        }

        public async Task<IEnumerable<Pedido>> ObtenerPedidosVencidosAsync()
        {
            return await _unitOfWork.Pedidos.GetPedidosVencidosAsync();
        }

        public Pedido ObtenerPedidoPorNumero(string numeroPedido)
        {
            if (string.IsNullOrWhiteSpace(numeroPedido))
                return null;

            return _unitOfWork.Pedidos.Find(p => p.NumeroPedido == numeroPedido.Trim()).FirstOrDefault();
        }

        public async Task<Pedido> ObtenerPedidoPorNumeroAsync(string numeroPedido)
        {
            if (string.IsNullOrWhiteSpace(numeroPedido))
                return null;

            return await Task.Run(() => _unitOfWork.Pedidos.Find(p => p.NumeroPedido == numeroPedido.Trim()).FirstOrDefault());
        }

        // ============================================================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================================================

        public ResultadoOperacion CrearPedido(Pedido pedido)
        {
            try
            {
                NormalizarPedido(pedido);
                var validacion = ValidarPedido(pedido);
                if (!validacion.EsValido)
                    return validacion;

                // Generar número de pedido correlativo
                pedido.NumeroPedido = GenerarNumeroPedido();
                pedido.IdPedido = Guid.NewGuid();
                pedido.Fecha = DateTime.Now;

                _unitOfWork.Pedidos.Add(pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso($"Pedido {pedido.NumeroPedido} creado exitosamente", pedido.IdPedido);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el pedido: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> CrearPedidoAsync(Pedido pedido)
        {
            try
            {
                NormalizarPedido(pedido);
                var validacion = ValidarPedido(pedido);
                if (!validacion.EsValido)
                    return validacion;

                // Generar número de pedido correlativo
                pedido.NumeroPedido = await GenerarNumeroPedidoAsync();
                pedido.IdPedido = Guid.NewGuid();
                pedido.Fecha = DateTime.Now;

                _unitOfWork.Pedidos.Add(pedido);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso($"Pedido {pedido.NumeroPedido} creado exitosamente", pedido.IdPedido);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el pedido: {ex.Message}");
            }
        }

        public ResultadoOperacion ActualizarPedido(Pedido pedido)
        {
            try
            {
                NormalizarPedido(pedido);
                var validacion = ValidarPedido(pedido);
                if (!validacion.EsValido)
                    return validacion;

                var pedidoExistente = _unitOfWork.Pedidos.GetById(pedido.IdPedido);
                if (pedidoExistente == null)
                    return ResultadoOperacion.Error("El pedido no existe");

                _unitOfWork.Pedidos.Update(pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso($"Pedido {pedido.NumeroPedido} actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el pedido: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> ActualizarPedidoAsync(Pedido pedido)
        {
            try
            {
                NormalizarPedido(pedido);
                var validacion = ValidarPedido(pedido);
                if (!validacion.EsValido)
                    return validacion;

                var pedidoExistente = await _unitOfWork.Pedidos.GetByIdAsync(pedido.IdPedido);
                if (pedidoExistente == null)
                    return ResultadoOperacion.Error("El pedido no existe");

                _unitOfWork.Pedidos.Update(pedido);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso($"Pedido {pedido.NumeroPedido} actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el pedido: {ex.Message}");
            }
        }

        public ResultadoOperacion EliminarPedido(Guid idPedido)
        {
            try
            {
                var pedido = _unitOfWork.Pedidos.GetById(idPedido);
                if (pedido == null)
                    return ResultadoOperacion.Error("El pedido no existe");

                _unitOfWork.Pedidos.Remove(pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso($"Pedido {pedido.NumeroPedido} eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al eliminar el pedido: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> EliminarPedidoAsync(Guid idPedido)
        {
            try
            {
                var pedido = await _unitOfWork.Pedidos.GetByIdAsync(idPedido);
                if (pedido == null)
                    return ResultadoOperacion.Error("El pedido no existe");

                _unitOfWork.Pedidos.Remove(pedido);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso($"Pedido {pedido.NumeroPedido} eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al eliminar el pedido: {ex.Message}");
            }
        }

        // ============================================================================
        // OPERACIONES SOBRE DETALLES
        // ============================================================================

        public ResultadoOperacion AgregarDetalle(Guid idPedido, PedidoDetalle detalle)
        {
            try
            {
                detalle.IdPedido = idPedido;
                detalle.IdDetallePedido = Guid.NewGuid();

                var validacion = ValidarDetalle(detalle);
                if (!validacion.EsValido)
                    return validacion;

                _unitOfWork.PedidoDetalles.Add(detalle);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Detalle agregado exitosamente", detalle.IdDetallePedido);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al agregar el detalle: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> AgregarDetalleAsync(Guid idPedido, PedidoDetalle detalle)
        {
            try
            {
                detalle.IdPedido = idPedido;
                detalle.IdDetallePedido = Guid.NewGuid();

                var validacion = ValidarDetalle(detalle);
                if (!validacion.EsValido)
                    return validacion;

                _unitOfWork.PedidoDetalles.Add(detalle);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Detalle agregado exitosamente", detalle.IdDetallePedido);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al agregar el detalle: {ex.Message}");
            }
        }

        public ResultadoOperacion ActualizarDetalle(PedidoDetalle detalle)
        {
            try
            {
                var validacion = ValidarDetalle(detalle);
                if (!validacion.EsValido)
                    return validacion;

                _unitOfWork.PedidoDetalles.Update(detalle);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Detalle actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el detalle: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> ActualizarDetalleAsync(PedidoDetalle detalle)
        {
            try
            {
                var validacion = ValidarDetalle(detalle);
                if (!validacion.EsValido)
                    return validacion;

                _unitOfWork.PedidoDetalles.Update(detalle);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Detalle actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el detalle: {ex.Message}");
            }
        }

        public ResultadoOperacion EliminarDetalle(Guid idDetalle)
        {
            try
            {
                var detalle = _unitOfWork.PedidoDetalles.GetById(idDetalle);
                if (detalle == null)
                    return ResultadoOperacion.Error("El detalle no existe");

                _unitOfWork.PedidoDetalles.Remove(detalle);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Detalle eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al eliminar el detalle: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> EliminarDetalleAsync(Guid idDetalle)
        {
            try
            {
                var detalle = await _unitOfWork.PedidoDetalles.GetByIdAsync(idDetalle);
                if (detalle == null)
                    return ResultadoOperacion.Error("El detalle no existe");

                _unitOfWork.PedidoDetalles.Remove(detalle);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Detalle eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al eliminar el detalle: {ex.Message}");
            }
        }

        // ============================================================================
        // FACTURACIÓN
        // ============================================================================

        public ResultadoOperacion MarcarComoFacturado(Guid idPedido, string rutaPDF)
        {
            try
            {
                var pedido = _unitOfWork.Pedidos.GetById(idPedido);
                if (pedido == null)
                    return ResultadoOperacion.Error("El pedido no existe");

                pedido.Facturado = true;
                pedido.RutaFacturaPDF = rutaPDF;

                _unitOfWork.Pedidos.Update(pedido);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso($"Pedido {pedido.NumeroPedido} marcado como facturado");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al marcar como facturado: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> MarcarComoFacturadoAsync(Guid idPedido, string rutaPDF)
        {
            try
            {
                var pedido = await _unitOfWork.Pedidos.GetByIdAsync(idPedido);
                if (pedido == null)
                    return ResultadoOperacion.Error("El pedido no existe");

                pedido.Facturado = true;
                pedido.RutaFacturaPDF = rutaPDF;

                _unitOfWork.Pedidos.Update(pedido);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso($"Pedido {pedido.NumeroPedido} marcado como facturado");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al marcar como facturado: {ex.Message}");
            }
        }

        // ============================================================================
        // CÁLCULOS
        // ============================================================================

        public decimal CalcularTotalPedido(Guid idPedido)
        {
            return _unitOfWork.PedidoDetalles.GetTotalPedido(idPedido);
        }

        public async Task<decimal> CalcularTotalPedidoAsync(Guid idPedido)
        {
            return await _unitOfWork.PedidoDetalles.GetTotalPedidoAsync(idPedido);
        }

        public decimal CalcularTotalConIVA(Guid idPedido)
        {
            var total = CalcularTotalPedido(idPedido);
            // IVA del 21%
            return total * 1.21m;
        }

        public async Task<decimal> CalcularTotalConIVAAsync(Guid idPedido)
        {
            var total = await CalcularTotalPedidoAsync(idPedido);
            // IVA del 21%
            return total * 1.21m;
        }

        // ============================================================================
        // MÉTODOS AUXILIARES
        // ============================================================================

        public string GenerarNumeroPedido()
        {
            try
            {
                // Obtener todos los pedidos y buscar el último número
                var ultimoPedido = _unitOfWork.Pedidos.GetAll()
                    .OrderByDescending(p => p.Fecha)
                    .FirstOrDefault();

                int nuevoNumero = 1;

                if (ultimoPedido != null && !string.IsNullOrEmpty(ultimoPedido.NumeroPedido))
                {
                    // Extraer el número del formato PED-0001
                    var partes = ultimoPedido.NumeroPedido.Split('-');
                    if (partes.Length == 2 && int.TryParse(partes[1], out int numeroActual))
                    {
                        nuevoNumero = numeroActual + 1;
                    }
                }

                return $"PED-{nuevoNumero:D4}";
            }
            catch
            {
                // Si hay algún error, generar un número basado en timestamp
                return $"PED-{DateTime.Now:yyyyMMddHHmmss}";
            }
        }

        public async Task<string> GenerarNumeroPedidoAsync()
        {
            return await Task.Run(() => GenerarNumeroPedido());
        }

        public IEnumerable<EstadoPedido> ObtenerEstadosPedido()
        {
            return _unitOfWork.EstadosPedido.GetAll();
        }

        public async Task<IEnumerable<EstadoPedido>> ObtenerEstadosPedidoAsync()
        {
            return await Task.Run(() => _unitOfWork.EstadosPedido.GetAll());
        }

        public IEnumerable<EstadoProducto> ObtenerEstadosProducto()
        {
            return _unitOfWork.EstadosProducto.GetAll();
        }

        public async Task<IEnumerable<EstadoProducto>> ObtenerEstadosProductoAsync()
        {
            return await Task.Run(() => _unitOfWork.EstadosProducto.GetAll());
        }

        public IEnumerable<TipoPago> ObtenerTiposPago()
        {
            // Nota: TipoPago debe estar en el UnitOfWork
            // Por ahora retorno vacío hasta que se implemente
            return new List<TipoPago>();
        }

        public async Task<IEnumerable<TipoPago>> ObtenerTiposPagoAsync()
        {
            return await Task.Run(() => ObtenerTiposPago());
        }

        // ============================================================================
        // MÉTODOS PRIVADOS DE VALIDACIÓN Y NORMALIZACIÓN
        // ============================================================================

        private void NormalizarPedido(Pedido pedido)
        {
            if (pedido == null) return;
            pedido.Cliente_DireccionEntrega = pedido.Cliente_DireccionEntrega?.Trim();
            pedido.Cliente_OC = pedido.Cliente_OC?.Trim();
            pedido.Cliente_PersonaNombre = pedido.Cliente_PersonaNombre?.Trim();
            pedido.Cliente_PersonaEmail = pedido.Cliente_PersonaEmail?.Trim();
            pedido.Cliente_PersonaTelefono = pedido.Cliente_PersonaTelefono?.Trim();
            pedido.NumeroRemito = pedido.NumeroRemito?.Trim();
            pedido.Observaciones = pedido.Observaciones?.Trim();
        }

        private ResultadoOperacion ValidarPedido(Pedido pedido)
        {
            if (pedido == null)
                return ResultadoOperacion.Error("El pedido no puede ser nulo");

            if (pedido.IdCliente == Guid.Empty)
                return ResultadoOperacion.Error("El cliente es obligatorio");

            // Validar que el cliente existe
            var cliente = _unitOfWork.Clientes.GetById(pedido.IdCliente);
            if (cliente == null)
                return ResultadoOperacion.Error("El cliente seleccionado no existe");

            return ResultadoOperacion.Exitoso("Validación exitosa");
        }

        private ResultadoOperacion ValidarDetalle(PedidoDetalle detalle)
        {
            if (detalle == null)
                return ResultadoOperacion.Error("El detalle no puede ser nulo");

            if (detalle.IdPedido == Guid.Empty)
                return ResultadoOperacion.Error("El pedido es obligatorio");

            if (detalle.IdProducto == Guid.Empty)
                return ResultadoOperacion.Error("El producto es obligatorio");

            if (detalle.Cantidad <= 0)
                return ResultadoOperacion.Error("La cantidad debe ser mayor a cero");

            if (detalle.PrecioUnitario < 0)
                return ResultadoOperacion.Error("El precio unitario no puede ser negativo");

            return ResultadoOperacion.Exitoso("Validación exitosa");
        }
    }
}
