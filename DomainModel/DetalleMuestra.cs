using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("DetalleMuestra")]
    public class DetalleMuestra
    {
        [Key]
        public Guid IdDetalleMuestra { get; set; } = Guid.NewGuid();

        public Guid IdPedidoMuestra { get; set; }
        public Guid IdProducto { get; set; }

        public Guid? IdEstadoMuestra { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        [StringLength(200)]
        public string ComentarioDevolucion { get; set; }

        public decimal? PrecioFacturacion { get; set; }

        public bool Facturado { get; set; }

        public DateTime? FechaFacturacion { get; set; }

        // Navegación
        public virtual PedidoMuestra PedidoMuestra { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual EstadoMuestra EstadoMuestra { get; set; }
    }
}
