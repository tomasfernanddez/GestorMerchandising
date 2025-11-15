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
    [Table("Cliente")]
    /// <summary>
    /// Define los datos principales de un cliente, sus relaciones comerciales y vínculos geográficos.
    /// </summary>
    public class Cliente
    {
        [Key]
        public Guid IdCliente { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string RazonSocial { get; set; }

        [StringLength(100)]
        public string Alias { get; set; }

        [Required]
        [StringLength(15)]
        public string CUIT { get; set; }

        [Required]
        public Guid IdCondicionIva { get; set; }

        [StringLength(150)]
        public string Domicilio { get; set; }

        [StringLength(100)]
        public string Localidad { get; set; }

        public bool Activo { get; set; } = true;

        public Guid? IdTipoEmpresa { get; set; }

        // === NUEVO: claves foráneas GUID a tablas geo ===
        public Guid? IdPais { get; set; }
        public Guid? IdProvincia { get; set; }
        public Guid? IdLocalidad { get; set; }

        // Navegación
        public virtual TipoEmpresa TipoEmpresa { get; set; }
        public virtual CondicionIva CondicionIva { get; set; }

        // === NUEVO: navegación geo opcional ===
        public virtual Pais Pais { get; set; }
        public virtual Provincia Provincia { get; set; }
        public virtual Localidad LocalidadRef { get; set; } // para no chocar con string Localidad

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public virtual ICollection<PedidoMuestra> PedidosMuestra { get; set; } = new List<PedidoMuestra>();
        public virtual ICollection<FacturaCabecera> Facturas { get; set; } = new List<FacturaCabecera>();
    }
}
