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
    [Table("LogosPedido")]
    public class LogosPedido
    {
        [Key]
        public Guid IdLogoPedido { get; set; } = Guid.NewGuid();

        public Guid IdDetallePedido { get; set; }

        public Guid? IdTecnicaPersonalizacion { get; set; }
        public Guid? IdUbicacionLogo { get; set; }

        // Navegación
        public virtual PedidoDetalle DetallePedido { get; set; }
        public virtual TecnicaPersonalizacion TecnicaPersonalizacion { get; set; }
        public virtual UbicacionLogo UbicacionLogo { get; set; }
    }
}
