using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("EstadoPedido")]
    /// <summary>
    /// Lista los estados disponibles para controlar el ciclo de vida de los pedidos.
    /// </summary>
    public class EstadoPedido
    {
        [Key]
        public Guid IdEstadoPedido { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombreEstadoPedido { get; set; }

        // Navegación
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
