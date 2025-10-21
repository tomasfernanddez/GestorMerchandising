using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("TecnicaPersonalizacion")]
    public class TecnicaPersonalizacion
    {
        [Key]
        public Guid IdTecnicaPersonalizacion { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombreTecnicaPersonalizacion { get; set; }

        // Navegación
        public virtual ICollection<LogosPedido> LogosPedido { get; set; } = new List<LogosPedido>();
    }
}
