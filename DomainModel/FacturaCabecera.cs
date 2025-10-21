using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("FacturaCabecera")]
    public class FacturaCabecera
    {
        [Key]
        public Guid IdFactura { get; set; } = Guid.NewGuid();

        public Guid IdCliente { get; set; }
        public Guid IdEmisor { get; set; }

        public DateTime FacturaFechaEmision { get; set; } = DateTime.Now;

        public int FacturaPuntoVenta { get; set; }
        public int FacturaNumeroComprobante { get; set; }

        [StringLength(10)]
        public string FacturaTipo { get; set; }

        [StringLength(10)]
        public string Moneda { get; set; }

        [Column(TypeName = "decimal")]
        public decimal MontoSubtotal { get; set; }

        [Column(TypeName = "decimal")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal MontoIVA { get; private set; }

        [Column(TypeName = "decimal")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal MontoTotal { get; private set; }

        // Navegación
        public virtual Cliente Cliente { get; set; }
        public virtual EmisorFactura Emisor { get; set; }
        public virtual ICollection<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
    }
}
