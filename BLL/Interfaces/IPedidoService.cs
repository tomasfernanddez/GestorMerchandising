using System;
using System.Collections.Generic;
using BLL.Helpers;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Interfaces
{
    public class PedidoFiltro
    {
        public string NumeroPedido { get; set; }
        public Guid? IdCliente { get; set; }
        public Guid? IdEstado { get; set; }
        public bool? Facturado { get; set; }
        public bool? ConSaldoPendiente { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public bool IncluirDetalles { get; set; }
    }

    public interface IPedidoService
    {
        /// <summary>
        /// Obtiene pedidos.
        /// </summary>
        IEnumerable<Pedido> ObtenerPedidos(PedidoFiltro filtro = null);
        /// <summary>
        /// Obtiene pedido.
        /// </summary>
        Pedido ObtenerPedido(Guid idPedido, bool incluirDetalles = true);
        /// <summary>
        /// Crea pedido.
        /// </summary>
        ResultadoOperacion CrearPedido(Pedido pedido);
        /// <summary>
        /// Actualiza pedido.
        /// </summary>
        ResultadoOperacion ActualizarPedido(Pedido pedido);
        /// <summary>
        /// Cambia estado.
        /// </summary>
        ResultadoOperacion CambiarEstado(Guid idPedido, Guid idEstado, string comentario, string usuario);
        /// <summary>
        /// Cancela pedido.
        /// </summary>
        ResultadoOperacion CancelarPedido(Guid idPedido, string usuario, string comentario);
        /// <summary>
        /// Registra nota.
        /// </summary>
        ResultadoOperacion RegistrarNota(Guid idPedido, string nota, string usuario);
        /// <summary>
        /// Genera proximo numero pedido.
        /// </summary>
        string GenerarProximoNumeroPedido();
        /// <summary>
        /// Obtiene estados pedido.
        /// </summary>
        IEnumerable<EstadoPedido> ObtenerEstadosPedido();
        /// <summary>
        /// Obtiene estados producto.
        /// </summary>
        IEnumerable<EstadoProducto> ObtenerEstadosProducto();
        /// <summary>
        /// Obtiene tipos pago.
        /// </summary>
        IEnumerable<TipoPago> ObtenerTiposPago();
        /// <summary>
        /// Obtiene tecnicas personalizacion.
        /// </summary>
        IEnumerable<TecnicaPersonalizacion> ObtenerTecnicasPersonalizacion();
        /// <summary>
        /// Obtiene ubicaciones logo.
        /// </summary>
        IEnumerable<UbicacionLogo> ObtenerUbicacionesLogo();
    }
}