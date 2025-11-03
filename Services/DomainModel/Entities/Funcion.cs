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

        public string ObtenerIdentificador()
        {
            return Codigo ?? string.Empty;
        }

        public bool EstaActivo()
        {
            return Activo;
        }

        public IEnumerable<IPermisoComponent> ObtenerHijos()
        {
            return Enumerable.Empty<IPermisoComponent>();
        }

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