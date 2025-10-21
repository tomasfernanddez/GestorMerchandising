using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("Sesion")]
    public class Sesion
    {
        [Key]
        public Guid IdSesion { get; set; } = Guid.NewGuid();

        public Guid IdUsuario { get; set; }

        public DateTime FechaInicio { get; set; } = DateTime.Now;

        public DateTime? FechaFin { get; set; }

        public bool Activa { get; set; } = true;

        [StringLength(100)]
        public string DireccionIP { get; set; }

        [StringLength(200)]
        public string UserAgent { get; set; }

        // Navegación
        public virtual Usuario Usuario { get; set; }
    }
}
