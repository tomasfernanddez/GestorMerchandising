using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("EstadoMuestra")]
    public class EstadoMuestra
    {
        [Key]
        public Guid IdEstadoMuestra { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombreEstadoMuestra { get; set; }

        // Navegación
        public virtual ICollection<DetalleMuestra> DetalleMuestras { get; set; } = new List<DetalleMuestra>();
    }
}
