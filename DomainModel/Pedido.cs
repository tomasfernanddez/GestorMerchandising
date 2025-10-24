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

        [Required]
        [StringLength(20)]
        public string NumeroPedido { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaConfirmacion { get; set; }

        public DateTime? FechaProduccion { get; set; }

        public DateTime? FechaFinalizacion { get; set; }

        public DateTime? FechaEnvio { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public DateTime? FechaLimiteEntrega { get; set; }

        [StringLength(500)]
        public string Observaciones { get; set; }

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

        public bool Facturado { get; set; }

        [StringLength(260)]
        public string RutaFacturaPdf { get; set; }

        public decimal TotalSinIva { get; set; }

        public decimal MontoIva { get; set; }

        public decimal TotalConIva { get; set; }

        public decimal MontoPagado { get; set; }

        public decimal SaldoPendiente { get; set; }

        // Navegación
        public virtual Cliente Cliente { get; set; }
        public virtual EstadoPedido EstadoPedido { get; set; }
        public virtual TipoPago TipoPago { get; set; }
        public virtual ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
        public virtual ICollection<PedidoEstadoHistorial> HistorialEstados { get; set; } = new List<PedidoEstadoHistorial>();
        public virtual ICollection<PedidoNota> Notas { get; set; } = new List<PedidoNota>();
    }
}