using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("Pais")]
    /// <summary>
    /// Representa un país utilizado como referencia geográfica para clientes, proveedores y provincias.
    /// </summary>
    public class Pais
    {
        [Key]
        public Guid IdPais { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        public virtual ICollection<Provincia> Provincias { get; set; }
    }
}
