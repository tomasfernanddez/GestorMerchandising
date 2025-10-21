using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("UnidadMedida")]
    public class UnidadMedida
    {
        [Key]
        public Guid IdUnidadMedida { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombreUnidadMedida { get; set; }

        // Navegación
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
