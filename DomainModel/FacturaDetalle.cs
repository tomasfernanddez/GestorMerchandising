using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("FacturaDetalle")]
    /// <summary>
    /// Registra cada ítem facturado, enlazando productos con la cabecera y calculando importes parciales.
    /// </summary>
    public class FacturaDetalle
    {
        [Key]
        public Guid IdDetalleFactura { get; set; } = Guid.NewGuid();

        public Guid IdFactura { get; set; }
        public Guid IdProducto { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal")]
        public decimal PrecioUnidad { get; set; }

        [Column(TypeName = "decimal")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Subtotal { get; private set; }

        public decimal SubtotalIVA { get; }

        // Navegación
        public virtual FacturaCabecera Factura { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
