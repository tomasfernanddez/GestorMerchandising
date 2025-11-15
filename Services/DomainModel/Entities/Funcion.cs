using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Services.DomainModel.Interfaces;

namespace Services.DomainModel.Entities
{
    [Table("Funcion")]
    public class Funcion : IPermisoComponent
    {
        [Key]
        public Guid IdFuncion { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Codigo { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public virtual ICollection<Perfil> Perfiles { get; set; } = new List<Perfil>();

        #region Implementación del Patrón Composite (Leaf)

        /// <summary>
        /// Obtiene el identificador textual de la función.
        /// </summary>
        public string ObtenerIdentificador()
        {
            return Codigo ?? string.Empty;
        }

        /// <summary>
        /// Indica si la función se encuentra activa.
        /// </summary>
        public bool EstaActivo()
        {
            return Activo;
        }

        /// <summary>
        /// Devuelve los componentes hijos, vacíos en el caso de la hoja.
        /// </summary>
        public IEnumerable<IPermisoComponent> ObtenerHijos()
        {
            return Enumerable.Empty<IPermisoComponent>();
        }

        /// <summary>
        /// Determina si la función coincide con el código indicado.
        /// </summary>
        public bool ContieneFuncion(string codigoFuncion)
        {
            if (string.IsNullOrWhiteSpace(codigoFuncion))
            {
                return false;
            }

            var codigoNormalizado = Codigo?.Trim();
            if (string.IsNullOrWhiteSpace(codigoNormalizado))
            {
                return false;
            }

            return string.Equals(codigoNormalizado, codigoFuncion.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Devuelve la función actual si corresponde según el filtro de actividad.
        /// </summary>
        public IEnumerable<Funcion> ObtenerFunciones(bool soloActivas = true)
        {
            if (!soloActivas || Activo)
            {
                yield return this;
            }
        }

        #endregion
    }
}