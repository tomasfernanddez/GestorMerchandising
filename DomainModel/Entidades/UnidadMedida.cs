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
    /// <summary>
    /// Define las unidades de medida utilizadas para cuantificar los productos.
    /// </summary>
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
