using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainModel.Entities
{
    [Table("Perfil")]
    public class Perfil
    {
        [Key]
        public Guid IdPerfil { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombrePerfil { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        // Navegación
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public virtual ICollection<Funcion> Funciones { get; set; } = new List<Funcion>();
    }
}
