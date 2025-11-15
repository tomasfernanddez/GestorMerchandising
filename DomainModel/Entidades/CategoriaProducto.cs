using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("CategoriaProducto")]
    /// <summary>
    /// Clasifica los productos por categoría para organizar el catálogo y facilitar su selección.
    /// </summary>
    public class CategoriaProducto
    {
        [Key]
        public Guid IdCategoria { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string NombreCategoria { get; set; }

        public bool Activo { get; set; } = true;

        public int Orden { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegación
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
