using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("PedidoNota")]
    /// <summary>
    /// Guarda las anotaciones internas realizadas sobre un pedido, incluyendo autor y fecha de registro.
    /// </summary>
    public class PedidoNota
    {
        [Key]
        public Guid IdNota { get; set; } = Guid.NewGuid();

        public Guid IdPedido { get; set; }

        [StringLength(500)]
        public string Nota { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string Usuario { get; set; }

        public virtual Pedido Pedido { get; set; }
    }
}