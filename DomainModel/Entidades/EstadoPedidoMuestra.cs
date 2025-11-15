using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("EstadoPedidoMuestra")]
    /// <summary>
    /// Define los estados posibles para el seguimiento de los pedidos de muestras.
    /// </summary>
    public class EstadoPedidoMuestra
    {
        [Key]
        public Guid IdEstadoPedidoMuestra { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombreEstadoPedidoMuestra { get; set; }

        // Navegación
        public virtual ICollection<PedidoMuestra> PedidosMuestra { get; set; } = new List<PedidoMuestra>();
    }
}
