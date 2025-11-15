using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainModel.Entities
{
    /// <summary>
    /// Representa una sesión activa o histórica de un usuario en el sistema.
    /// </summary>
    [Table("Sesion")]
    public class Sesion
    {
        /// <summary>
        /// Identificador único de la sesión.
        /// </summary>
        [Key]
        public Guid IdSesion { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Identificador del usuario al que pertenece la sesión.
        /// </summary>
        public Guid IdUsuario { get; set; }

        /// <summary>
        /// Fecha y hora en la que inició la sesión.
        /// </summary>
        public DateTime FechaInicio { get; set; } = DateTime.Now;

        /// <summary>
        /// Fecha y hora en la que finalizó la sesión, si aplica.
        /// </summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Indica si la sesión continúa activa.
        /// </summary>
        public bool Activa { get; set; } = true;

        /// <summary>
        /// Dirección IP desde la que se autenticó el usuario.
        /// </summary>
        [StringLength(100)]
        public string DireccionIP { get; set; }

        /// <summary>
        /// Información del agente de usuario (navegador o aplicación cliente).
        /// </summary>
        [StringLength(200)]
        public string UserAgent { get; set; }

        /// <summary>
        /// Usuario asociado a la sesión.
        /// </summary>
        public virtual Usuario Usuario { get; set; }
    }
}
