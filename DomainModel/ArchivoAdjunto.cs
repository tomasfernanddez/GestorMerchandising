using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("ArchivoAdjunto")]
    public class ArchivoAdjunto
    {
        [Key]
        public Guid IdArchivoAdjunto { get; set; } = Guid.NewGuid();

        public Guid? IdPedido { get; set; }

        public Guid? IdPedidoMuestra { get; set; }

        [Required]
        [StringLength(200)]
        public string NombreArchivo { get; set; }

        [Required]
        [StringLength(10)]
        public string Extension { get; set; }

        [Required]
        [StringLength(100)]
        public string TipoContenido { get; set; }

        public long TamanoBytes { get; set; }

        public DateTime FechaSubida { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid IdUsuario { get; set; }

        [StringLength(100)]
        public string NombreUsuario { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public byte[] Contenido { get; set; }

        // Navegación
        public virtual Pedido Pedido { get; set; }
        public virtual PedidoMuestra PedidoMuestra { get; set; }
    }
}