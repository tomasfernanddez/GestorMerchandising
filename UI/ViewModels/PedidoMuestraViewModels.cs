using System;

namespace UI.ViewModels
{
    public class PedidoMuestraDetalleViewModel
    {
        public Guid IdDetalleMuestra { get; set; }
        public Guid? IdProducto { get; set; }
        public string Producto { get; set; }
        public Guid? IdEstadoMuestra { get; set; }
        public string Estado { get; set; }
        public decimal? PrecioFacturacion { get; set; }
        public bool Facturado { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Comentario { get; set; }
    }
}