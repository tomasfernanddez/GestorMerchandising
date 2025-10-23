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
        [StringLength(100, MinimumLength = 3)]
        public string RazonSocial { get; set; }

        [Required]
        [StringLength(15)]
        public string CUIT { get; set; }

        [Required]
        public Guid IdCondicionIva { get; set; }

        [StringLength(150)]
        public string Domicilio { get; set; }

        [StringLength(20)]
        public string CodigoPostal { get; set; }

        [StringLength(100)]
        public string Localidad { get; set; }

        [Required]
        [StringLength(50)]
        public string CondicionesPago { get; set; }

        [StringLength(500)]
        public string Observaciones { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaAlta { get; set; } = DateTime.Now;

        public Guid? IdTipoProveedor { get; set; }

        public Guid? IdPais { get; set; }
        public Guid? IdProvincia { get; set; }
        public Guid? IdLocalidad { get; set; }

        // Navegación
        public virtual TipoProveedor TipoProveedor { get; set; }
        public virtual CondicionIva CondicionIva { get; set; }
        public virtual Pais Pais { get; set; }
        public virtual Provincia Provincia { get; set; }
        public virtual Localidad LocalidadRef { get; set; }

        public virtual ICollection<TecnicaPersonalizacion> TecnicasPersonalizacion { get; set; } = new List<TecnicaPersonalizacion>();

        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}