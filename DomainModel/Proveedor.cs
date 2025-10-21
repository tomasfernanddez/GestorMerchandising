using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("Proveedor")]
    public class Proveedor
    {
        [Key]
        public Guid IdProveedor { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string RazonSocial { get; set; }

        [Required]
        [StringLength(15)]
        public string CUIT { get; set; }

        [StringLength(50)]
        public string CondicionIva { get; set; }

        [StringLength(150)]
        public string Domicilio { get; set; }

        [StringLength(100)]
        public string Localidad { get; set; }

        public bool Activo { get; set; } = true;

        public Guid? IdTipoProveedor { get; set; }

        // Navegación
        public virtual TipoProveedor TipoProveedor { get; set; }
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
