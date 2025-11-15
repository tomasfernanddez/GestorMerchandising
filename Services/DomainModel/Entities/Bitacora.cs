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
    /// Representa un registro de bitácora con la información de auditoría de una acción del sistema.
    /// </summary>
    [Table("Bitacora")]
    public class Bitacora
    {
        /// <summary>
        /// Identificador único del registro de bitácora.
        /// </summary>
        [Key]
        public Guid IdBitacora { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Identificador del usuario que ejecutó la acción registrada.
        /// </summary>
        public Guid IdUsuario { get; set; }

        /// <summary>
        /// Fecha y hora en la que ocurrió el evento.
        /// </summary>
        public DateTime Fecha { get; set; } = DateTime.Now;

        /// <summary>
        /// Nombre de la acción ejecutada.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Accion { get; set; }

        /// <summary>
        /// Descripción adicional de la acción.
        /// </summary>
        [StringLength(500)]
        public string Descripcion { get; set; }

        /// <summary>
        /// Módulo funcional del sistema donde ocurrió la acción.
        /// </summary>
        [StringLength(50)]
        public string Modulo { get; set; } // Cliente, Proveedor, Pedido, etc.

        /// <summary>
        /// Dirección IP desde la que se realizó la acción.
        /// </summary>
        [StringLength(100)]
        public string DireccionIP { get; set; }

        /// <summary>
        /// Indica si la operación finalizó de manera exitosa.
        /// </summary>
        public bool Exitoso { get; set; } = true;

        /// <summary>
        /// Mensaje de error asociado cuando la operación falla.
        /// </summary>
        [StringLength(500)]
        public string MensajeError { get; set; }

        /// <summary>
        /// Usuario relacionado con el registro de bitácora.
        /// </summary>
        public virtual Usuario Usuario { get; set; }
    }
}
