using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entidades
{
    [Table("TipoEmpresa")]
    public class TipoEmpresa
    {
        [Key]
        public Guid IdTipoEmpresa { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        [Column("TipoEmpresa")]
        public string TipoEmpresaNombre { get; set; }

        // Navegación
        public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    }
}
