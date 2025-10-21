using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainModel.Entities
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public Guid IdUsuario { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string PasswordHash { get; set; }

        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaUltimoAcceso { get; set; }

        public int IntentosLoginFallidos { get; set; } = 0;

        public bool Bloqueado { get; set; } = false;

        public DateTime? FechaBloqueo { get; set; }

        // Foreign Key
        public Guid IdPerfil { get; set; }

        [StringLength(10)]
        public string IdiomaPreferido { get; set; }

        // Navegación
        public virtual Perfil Perfil { get; set; }
        public virtual ICollection<Bitacora> BitacoraRegistros { get; set; } = new List<Bitacora>();
    }
}
