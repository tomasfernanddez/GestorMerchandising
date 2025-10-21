using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("Bitacora")]
    public class Bitacora
    {
        [Key]
        public Guid IdBitacora { get; set; } = Guid.NewGuid();

        public Guid IdUsuario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [StringLength(100)]
        public string Accion { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [StringLength(50)]
        public string Modulo { get; set; } // Cliente, Proveedor, Pedido, etc.

        [StringLength(100)]
        public string DireccionIP { get; set; }

        public bool Exitoso { get; set; } = true;

        [StringLength(500)]
        public string MensajeError { get; set; }

        // Navegación
        public virtual Usuario Usuario { get; set; }
    }
}
