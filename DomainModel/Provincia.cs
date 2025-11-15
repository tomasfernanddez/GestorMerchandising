using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("Provincia")]
    /// <summary>
    /// Representa una provincia o estado perteneciente a un país y contiene sus localidades asociadas.
    /// </summary>
    public class Provincia
    {
        [Key]
        public Guid IdProvincia { get; set; } = Guid.NewGuid();

        [Required]
        public Guid IdPais { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [ForeignKey(nameof(IdPais))]
        public virtual Pais Pais { get; set; }

        public virtual ICollection<Localidad> Localidades { get; set; }
    }
}
