using System;
using System.Collections.Generic;
using BLL.Helpers;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Interfaces
{
    public class PedidoMuestraFiltro
    {
        public Guid? IdCliente { get; set; }
        public Guid? IdEstadoPedido { get; set; }
        public bool? Facturado { get; set; }
        public bool? ConSaldoPendiente { get; set; }
        public string TextoBusqueda { get; set; }
        public string NumeroPedido { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public bool IncluirDetalles { get; set; }
    }

    public interface IPedidoMuestraService
    {
        /// <summary>
        /// Obtiene pedidos muestra.
        /// </summary>
        IEnumerable<PedidoMuestra> ObtenerPedidosMuestra(PedidoMuestraFiltro filtro = null);
        /// <summary>
        /// Obtiene pedido muestra.
        /// </summary>
        PedidoMuestra ObtenerPedidoMuestra(Guid idPedidoMuestra, bool incluirDetalles = true);
        /// <summary>
        /// Crea pedido muestra.
        /// </summary>
        ResultadoOperacion CrearPedidoMuestra(PedidoMuestra pedido);
        /// <summary>
        /// Actualiza pedido muestra.
        /// </summary>
        ResultadoOperacion ActualizarPedidoMuestra(PedidoMuestra pedido);
        /// <summary>
        /// Cancela pedido muestra.
        /// </summary>
        ResultadoOperacion CancelarPedidoMuestra(Guid idPedidoMuestra, string usuario, string comentario = null);
        /// <summary>
        /// Obtiene estados pedido.
        /// </summary>
        IEnumerable<EstadoPedidoMuestra> ObtenerEstadosPedido();
        /// <summary>
        /// Obtiene estados muestra.
        /// </summary>
        IEnumerable<EstadoMuestra> ObtenerEstadosMuestra();
    }
}