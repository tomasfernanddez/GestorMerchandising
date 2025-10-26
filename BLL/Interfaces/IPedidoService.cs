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
        IEnumerable<Pedido> ObtenerPedidos(PedidoFiltro filtro = null);
        Pedido ObtenerPedido(Guid idPedido, bool incluirDetalles = true);
        ResultadoOperacion CrearPedido(Pedido pedido);
        ResultadoOperacion ActualizarPedido(Pedido pedido);
        ResultadoOperacion CambiarEstado(Guid idPedido, Guid idEstado, string comentario, string usuario);
        ResultadoOperacion CancelarPedido(Guid idPedido, string usuario, string comentario);
        ResultadoOperacion RegistrarNota(Guid idPedido, string nota, string usuario);
        string GenerarProximoNumeroPedido();
        IEnumerable<EstadoPedido> ObtenerEstadosPedido();
        IEnumerable<EstadoProducto> ObtenerEstadosProducto();
        IEnumerable<TipoPago> ObtenerTiposPago();
        IEnumerable<TecnicaPersonalizacion> ObtenerTecnicasPersonalizacion();
        IEnumerable<UbicacionLogo> ObtenerUbicacionesLogo();
    }
}