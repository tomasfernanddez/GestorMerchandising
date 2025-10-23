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
    [Table("Producto")]
    public class Producto
    {
        [Key]
        public Guid IdProducto { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(150)]
        public string NombreProducto { get; set; }

        public Guid? IdCategoria { get; set; }
        public Guid? IdUnidadMedida { get; set; }
        public Guid? IdProveedor { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaUltimoUso { get; set; }

        public int VecesUsado { get; set; }

        // Navegación
        public virtual CategoriaProducto Categoria { get; set; }
        public virtual UnidadMedida UnidadMedida { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
        public virtual ICollection<DetalleMuestra> DetalleMuestras { get; set; } = new List<DetalleMuestra>();
        public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();
    }
}
