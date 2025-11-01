using System;

namespace UI.ViewModels
{
    public class PedidoMuestraDetalleViewModel
    {
        public Guid IdDetalleMuestra { get; set; }
        public Guid? IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; } = 1;
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public Guid? IdEstadoMuestra { get; set; }
        public string EstadoMuestra { get; set; }
        public DateTime? FechaDevolucion { get; set; }
    }
}