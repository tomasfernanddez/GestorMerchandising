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
    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        public Guid IdPedido { get; set; } = Guid.NewGuid();

        public Guid IdCliente { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public DateTime? FechaLimite { get; set; }

        public bool TieneFechaLimite { get; set; }

        [StringLength(150)]
        public string Cliente_DireccionEntrega { get; set; }

        [StringLength(50)]
        public string Cliente_OC { get; set; }

        [StringLength(100)]
        public string Cliente_PersonaNombre { get; set; }

        [StringLength(100)]
        public string Cliente_PersonaEmail { get; set; }

        [StringLength(30)]
        public string Cliente_PersonaTelefono { get; set; }

        [StringLength(50)]
        public string NumeroRemito { get; set; }

        public Guid? IdEstadoPedido { get; set; }
        public Guid? IdTipoPago { get; set; }

        // Navegación
        public virtual Cliente Cliente { get; set; }
        public virtual EstadoPedido EstadoPedido { get; set; }
        public virtual TipoPago TipoPago { get; set; }
        public virtual ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    }
}
