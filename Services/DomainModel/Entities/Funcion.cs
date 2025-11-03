using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.DomainModel.Entities
{
    [Table("Funcion")]
    public class Funcion
    {
        [Key]
        public Guid IdFuncion { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Codigo { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public virtual ICollection<Perfil> Perfiles { get; set; } = new List<Perfil>();
    }
}