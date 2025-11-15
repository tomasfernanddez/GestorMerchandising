using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("EstadoProducto")]
    /// <summary>
    /// Especifica los estados de producción asignables a los productos incluidos en un pedido.
    /// </summary>
    public class EstadoProducto
    {
        [Key]
        public Guid IdEstadoProducto { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombreEstadoProducto { get; set; }

        // Navegación
        public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
    }
}
