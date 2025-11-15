using System;

namespace UI.ViewModels
{
    /// <summary>
    /// Representa el detalle de un pedido de muestra para la interfaz.
    /// </summary>
    public class PedidoMuestraDetalleViewModel
    {
        /// <summary>
        /// Identificador del detalle de muestra.
        /// </summary>
        public Guid IdDetalleMuestra { get; set; }
        /// <summary>
        /// Identificador del producto asociado.
        /// </summary>
        public Guid? IdProducto { get; set; }
        /// <summary>
        /// Nombre del producto que se presta como muestra.
        /// </summary>
        public string NombreProducto { get; set; }
        /// <summary>
        /// Cantidad solicitada en la muestra.
        /// </summary>
        public int Cantidad { get; set; } = 1;
        /// <summary>
        /// Precio unitario referencial.
        /// </summary>
        public decimal PrecioUnitario { get; set; }
        /// <summary>
        /// Subtotal calculado para la muestra.
        /// </summary>
        public decimal Subtotal { get; set; }
        /// <summary>
        /// Identificador del estado de la muestra.
        /// </summary>
        public Guid? IdEstadoMuestra { get; set; }
        /// <summary>
        /// Estado descriptivo de la muestra.
        /// </summary>
        public string EstadoMuestra { get; set; }
        /// <summary>
        /// Fecha estimada de devolución.
        /// </summary>
        public DateTime? FechaDevolucion { get; set; }
    }
}