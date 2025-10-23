using DomainModel.Entidades;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    /// <summary>
    /// Patrón Composite - Leaf (Hoja)
    /// Representa un logo individual de personalización en un pedido
    /// </summary>
    [Table("LogosPedido")]
    public class LogosPedido : IComponentePersonalizacion
    {
        [Key]
        public Guid IdLogoPedido { get; set; } = Guid.NewGuid();

        public Guid IdDetallePedido { get; set; }

        public Guid? IdTecnicaPersonalizacion { get; set; }
        public Guid? IdUbicacionLogo { get; set; }

        /// <summary>
        /// Proveedor que aplica la personalización (puede ser diferente al proveedor del producto)
        /// </summary>
        public Guid? IdProveedorPersonalizacion { get; set; }

        /// <summary>
        /// Costo adicional de este logo/personalización
        /// </summary>
        public decimal CostoPersonalizacion { get; set; } = 0;

        /// <summary>
        /// Descripción adicional del logo (ej: "Logo en 2 colores", "Medida 10x15cm")
        /// </summary>
        [StringLength(200)]
        public string Descripcion { get; set; }

        // Navegación
        public virtual PedidoDetalle DetallePedido { get; set; }
        public virtual TecnicaPersonalizacion TecnicaPersonalizacion { get; set; }
        public virtual UbicacionLogo UbicacionLogo { get; set; }
        public virtual Proveedor ProveedorPersonalizacion { get; set; }

        #region Implementación de IComponentePersonalizacion (Patrón Composite - Leaf)

        /// <summary>
        /// ID del componente (mapea a IdLogoPedido)
        /// </summary>
        [NotMapped]
        public Guid Id => IdLogoPedido;

        /// <summary>
        /// Un logo individual no es un grupo (es una hoja)
        /// </summary>
        [NotMapped]
        public bool EsGrupo => false;

        /// <summary>
        /// Obtiene la descripción del logo
        /// </summary>
        public string ObtenerDescripcion()
        {
            var partes = new List<string>();

            if (TecnicaPersonalizacion != null)
                partes.Add(TecnicaPersonalizacion.NombreTecnicaPersonalizacion);

            if (UbicacionLogo != null)
                partes.Add($"en {UbicacionLogo.NombreUbicacionLogo}");

            if (!string.IsNullOrWhiteSpace(Descripcion))
                partes.Add($"({Descripcion})");

            return partes.Any() ? string.Join(" ", partes) : "Personalización";
        }

        /// <summary>
        /// Calcula el costo de esta personalización individual
        /// </summary>
        public decimal CalcularCosto()
        {
            return CostoPersonalizacion;
        }

        /// <summary>
        /// Un logo individual no tiene componentes hijos
        /// </summary>
        public IEnumerable<IComponentePersonalizacion> ObtenerComponentes()
        {
            return Enumerable.Empty<IComponentePersonalizacion>();
        }

        /// <summary>
        /// No se puede agregar componentes a un logo individual (hoja)
        /// </summary>
        public void Agregar(IComponentePersonalizacion componente)
        {
            throw new InvalidOperationException(
                "No se pueden agregar componentes a un logo individual. Use GrupoPersonalizacion para agrupar logos.");
        }

        /// <summary>
        /// No se puede eliminar componentes de un logo individual (hoja)
        /// </summary>
        public void Eliminar(IComponentePersonalizacion componente)
        {
            throw new InvalidOperationException(
                "No se pueden eliminar componentes de un logo individual.");
        }

        #endregion
    }
}
