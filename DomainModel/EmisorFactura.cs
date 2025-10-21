using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("EmisorFactura")]
    public class EmisorFactura
    {
        [Key]
        public Guid IdEmisor { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string RazonSocial { get; set; }

        [Required]
        [StringLength(15)]
        public string EmisorCUIT { get; set; }

        [StringLength(50)]
        public string EmisorCondicionIVA { get; set; }

        [StringLength(150)]
        public string EmisorDomicilio { get; set; }

        [StringLength(100)]
        public string EmisorLocalidad { get; set; }

        // Navegación
        public virtual ICollection<FacturaCabecera> Facturas { get; set; } = new List<FacturaCabecera>();
    }
}
