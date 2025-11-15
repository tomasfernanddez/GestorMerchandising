using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("DetalleMuestra")]
    /// <summary>
    /// Describe los productos asociados a una muestra, incluyendo cantidades, precios y estado de seguimiento.
    /// </summary>
    public class DetalleMuestra
    {
        [Key]
        public Guid IdDetalleMuestra { get; set; } = Guid.NewGuid();

        public Guid IdPedidoMuestra { get; set; }
        public Guid IdProducto { get; set; }

        public int Cantidad { get; set; } = 1;

        [Column(TypeName = "decimal")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Subtotal { get; set; }

        public Guid? IdEstadoMuestra { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        // Navegación
        public virtual PedidoMuestra PedidoMuestra { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual EstadoMuestra EstadoMuestra { get; set; }
    }
}