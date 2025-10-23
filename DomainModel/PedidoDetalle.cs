using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("PedidoDetalle")]
    public class PedidoDetalle
    {
        [Key]
        public Guid IdDetallePedido { get; set; } = Guid.NewGuid();

        public Guid IdPedido { get; set; }
        public Guid IdProducto { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Subtotal { get; private set; }

        [NotMapped]
        public decimal SubtotalCalculado => Cantidad * PrecioUnitario;

        public bool FichaAplicacion { get; set; }

        public Guid? IdEstadoProducto { get; set; }

        public DateTime? FechaLimiteProduccion { get; set; }

        [StringLength(250)]
        public string Notas { get; set; }

        public Guid? IdProveedorPersonalizacion { get; set; }

        // Navegación
        public virtual Pedido Pedido { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual EstadoProducto EstadoProducto { get; set; }
        public virtual Proveedor ProveedorPersonalizacion { get; set; }
        public virtual ICollection<LogosPedido> LogosPedido { get; set; } = new List<LogosPedido>();
    }
}
