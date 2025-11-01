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
        IEnumerable<PedidoMuestra> ObtenerPedidosMuestra(PedidoMuestraFiltro filtro = null);
        PedidoMuestra ObtenerPedidoMuestra(Guid idPedidoMuestra, bool incluirDetalles = true);
        ResultadoOperacion CrearPedidoMuestra(PedidoMuestra pedido);
        ResultadoOperacion ActualizarPedidoMuestra(PedidoMuestra pedido);
        ResultadoOperacion CancelarPedidoMuestra(Guid idPedidoMuestra, string usuario, string comentario = null);
        IEnumerable<EstadoPedidoMuestra> ObtenerEstadosPedido();
        IEnumerable<EstadoMuestra> ObtenerEstadosMuestra();
    }
}