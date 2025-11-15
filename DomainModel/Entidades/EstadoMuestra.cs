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
    /// <summary>
    /// Define los posibles estados de seguimiento para las muestras enviadas a los clientes.
    /// </summary>
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
