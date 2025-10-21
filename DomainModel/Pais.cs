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
    public class Pais
    {
        [Key]
        public Guid IdPais { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        public virtual ICollection<Provincia> Provincias { get; set; }
    }
}
