using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("UbicacionLogo")]
    /// <summary>
    /// Indica las ubicaciones disponibles para aplicar logos en los productos.
    /// </summary>
    public class UbicacionLogo
    {
        [Key]
        public Guid IdUbicacionLogo { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombreUbicacionLogo { get; set; }

        // Navegación
        public virtual ICollection<LogosPedido> LogosPedido { get; set; } = new List<LogosPedido>();
    }
}
