using DomainModel.Entidades;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("PedidoEstadoHistorial")]
    /// <summary>
    /// Registra cada cambio de estado de un pedido con su fecha, comentario y usuario responsable.
    /// </summary>
    public class PedidoEstadoHistorial
    {
        [Key]
        public Guid IdHistorial { get; set; } = Guid.NewGuid();

        public Guid IdPedido { get; set; }

        public Guid IdEstadoPedido { get; set; }

        public DateTime FechaCambio { get; set; } = DateTime.UtcNow;

        [StringLength(250)]
        public string Comentario { get; set; }

        [StringLength(100)]
        public string Usuario { get; set; }

        public virtual Pedido Pedido { get; set; }

        public virtual EstadoPedido EstadoPedido { get; set; }
    }
}