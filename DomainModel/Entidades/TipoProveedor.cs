using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("TipoProveedor")]
    public class TipoProveedor
    {
        [Key]
        public Guid IdTipoProveedor { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string TipoProveedorNombre { get; set; }

        // Navegación
        public virtual ICollection<Proveedor> Proveedores { get; set; } = new List<Proveedor>();
    }
}
