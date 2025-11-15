using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("Localidad")]
    /// <summary>
    /// Representa una localidad geográfica asociada a una provincia para ubicar clientes y proveedores.
    /// </summary>
    public class Localidad
    {
        [Key]
        public Guid IdLocalidad { get; set; } = Guid.NewGuid();

        [Required]
        public Guid IdProvincia { get; set; }

        [Required, StringLength(120)]
        public string Nombre { get; set; }

        [ForeignKey(nameof(IdProvincia))]
        public virtual Provincia Provincia { get; set; }
    }
}
