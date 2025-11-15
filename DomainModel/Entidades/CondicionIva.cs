using DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Entidades
{
    [Table("CondicionIva")]
    /// <summary>
    /// Define las condiciones fiscales frente al IVA aplicables a clientes y proveedores.
    /// </summary>
    public class CondicionIva
    {
        [Key]
        public Guid IdCondicionIva { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
        public virtual ICollection<Proveedor> Proveedores { get; set; } = new List<Proveedor>();
    }
}