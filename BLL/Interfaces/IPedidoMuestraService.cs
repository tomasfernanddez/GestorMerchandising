using System;
using System.Collections.Generic;
using BLL.Helpers;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Interfaces
{
    public class PedidoMuestraFiltro
    {
        public string Numero { get; set; }
        public Guid? IdCliente { get; set; }
        public Guid? IdEstado { get; set; }
        public bool? Facturado { get; set; }
        public bool SoloVencidos { get; set; }
        public bool IncluirDetalles { get; set; }
    }

    public interface IPedidoMuestraService
    {
        IEnumerable<PedidoMuestra> ObtenerPedidos(PedidoMuestraFiltro filtro = null);
        PedidoMuestra ObtenerPedido(Guid idPedido, bool incluirDetalles = true);
        ResultadoOperacion CrearPedido(PedidoMuestra pedido);
        ResultadoOperacion ActualizarPedido(PedidoMuestra pedido);
        ResultadoOperacion RegistrarDevolucion(Guid idDetalle, Guid? idEstado, DateTime? fechaDevolucion, decimal? precioFacturacion, string comentario, bool marcarFacturado);
        ResultadoOperacion FacturarPendientes(Guid idPedido, IDictionary<Guid, decimal> preciosPorDetalle, bool generarPedidoFacturacion);
        string GenerarSiguienteNumero();
        void ActualizarDiasProrroga(Guid idPedido, int diasAdicionales);
        IEnumerable<EstadoPedidoMuestra> ObtenerEstadosPedido();
        IEnumerable<EstadoMuestra> ObtenerEstadosMuestra();
    }
}