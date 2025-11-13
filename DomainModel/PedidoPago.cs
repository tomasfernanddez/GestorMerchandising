using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("PedidoPago")]
    public class PedidoPago
    {
        [Key]
        public Guid IdPedidoPago { get; set; } = Guid.NewGuid();

        [Index]
        [Required]
        public Guid IdPedido { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Monto { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? Porcentaje { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(IdPedido))]
        public virtual Pedido Pedido { get; set; }
    }
}