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

        [StringLength(20)]
        public string NumeroCorrelativo { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaEntrega { get; set; }

        public DateTime? FechaDevolucionEsperada { get; set; }

        public DateTime? FechaDevolucionReal { get; set; }

        [StringLength(150)]
        public string DireccionEntrega { get; set; }

        [StringLength(100)]
        public string PersonaContacto { get; set; }

        [StringLength(100)]
        public string EmailContacto { get; set; }

        [StringLength(30)]
        public string TelefonoContacto { get; set; }

        [StringLength(300)]
        public string Observaciones { get; set; }

        public int DiasProrroga { get; set; }

        public bool Facturado { get; set; }

        public decimal TotalFacturado { get; set; }

        public Guid? IdEstadoPedidoMuestra { get; set; }

        // Navegación
        public virtual Cliente Cliente { get; set; }
        public virtual EstadoPedidoMuestra EstadoPedidoMuestra { get; set; }
        public virtual ICollection<DetalleMuestra> Detalles { get; set; } = new List<DetalleMuestra>();
    }
}
