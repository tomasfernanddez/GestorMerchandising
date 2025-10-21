using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("PedidoMuestra")]
    public class PedidoMuestra
    {
        [Key]
        public Guid IdPedidoMuestra { get; set; } = Guid.NewGuid();

        public Guid IdCliente { get; set; }

        public DateTime? FechaEntrega { get; set; }
        public DateTime? FechaDevolucion { get; set; }

        [StringLength(150)]
        public string DireccionEntrega { get; set; }

        [StringLength(100)]
        public string PersonaContacto { get; set; }

        [StringLength(100)]
        public string EmailContacto { get; set; }

        [StringLength(30)]
        public string TelefonoContacto { get; set; }

        public Guid? IdEstadoPedidoMuestra { get; set; }

        // Navegación
        public virtual Cliente Cliente { get; set; }
        public virtual EstadoPedidoMuestra EstadoPedidoMuestra { get; set; }
        public virtual ICollection<DetalleMuestra> Detalles { get; set; } = new List<DetalleMuestra>();
    }
}
