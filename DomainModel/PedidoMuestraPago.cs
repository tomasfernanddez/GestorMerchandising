using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("PedidoMuestraPago")]
    public class PedidoMuestraPago
    {
        [Key]
        public Guid IdPedidoMuestraPago { get; set; } = Guid.NewGuid();

        [Required]
        [Index]
        public Guid IdPedidoMuestra { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Monto { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? Porcentaje { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(IdPedidoMuestra))]
        public virtual PedidoMuestra PedidoMuestra { get; set; }
    }
}