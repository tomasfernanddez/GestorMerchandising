using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("TipoPago")]
    public class TipoPago
    {
        [Key]
        public Guid IdTipoPago { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombreTipoPago { get; set; }

        // Navegación
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
