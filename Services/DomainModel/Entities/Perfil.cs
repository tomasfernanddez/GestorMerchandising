using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Services.DomainModel.Interfaces;

namespace Services.DomainModel.Entities
{
    [Table("Perfil")]
    public class Perfil : IPermisoComponent
    {
        [Key]
        public Guid IdPerfil { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string NombrePerfil { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        // Navegación
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public virtual ICollection<Funcion> Funciones { get; set; } = new List<Funcion>();

        #region Implementación del Patrón Composite (Composite)

        /// <summary>
        /// Obtiene el identificador textual del perfil para el patrón Composite.
        /// </summary>
        public string ObtenerIdentificador()
        {
            return NombrePerfil ?? string.Empty;
        }

        /// <summary>
        /// Indica si el perfil se encuentra activo.
        /// </summary>
        public bool EstaActivo()
        {
            return Activo;
        }

        /// <summary>
        /// Devuelve las funciones asociadas al perfil como componentes hijos.
        /// </summary>
        public IEnumerable<IPermisoComponent> ObtenerHijos()
        {
            return Funciones?.Cast<IPermisoComponent>() ?? Enumerable.Empty<IPermisoComponent>();
        }

        /// <summary>
        /// Verifica si el perfil contiene una función específica.
        /// </summary>
        public bool ContieneFuncion(string codigoFuncion)
        {
            if (Funciones == null || Funciones.Count == 0)
            {
                return false;
            }

            return Funciones.Any(f => f.ContieneFuncion(codigoFuncion));
        }

        /// <summary>
        /// Obtiene todas las funciones asociadas al perfil, opcionalmente filtradas por estado.
        /// </summary>
        public IEnumerable<Funcion> ObtenerFunciones(bool soloActivas = true)
        {
            if (Funciones == null || Funciones.Count == 0)
            {
                return Enumerable.Empty<Funcion>();
            }

            return Funciones
                .SelectMany(f => f.ObtenerFunciones(soloActivas))
                .Where(f => !soloActivas || f.Activo)
                .ToList();
        }

        /// <summary>
        /// Agrega una función al perfil si aún no está asociada.
        /// </summary>
        public void AgregarFuncion(Funcion funcion)
        {
            if (funcion == null)
            {
                return;
            }

            if (Funciones == null)
            {
                Funciones = new List<Funcion>();
            }

            if (Funciones.Any(f => f.IdFuncion == funcion.IdFuncion))
            {
                return;
            }

            Funciones.Add(funcion);
        }

        /// <summary>
        /// Elimina una función del perfil según su identificador.
        /// </summary>
        public bool RemoverFuncion(Guid idFuncion)
        {
            if (Funciones == null || Funciones.Count == 0)
            {
                return false;
            }

            var existente = Funciones.FirstOrDefault(f => f.IdFuncion == idFuncion);
            if (existente == null)
            {
                return false;
            }

            return Funciones.Remove(existente);
        }

        /// <summary>
        /// Reemplaza la colección de funciones del perfil por la lista proporcionada.
        /// </summary>
        public void ReemplazarFunciones(IEnumerable<Funcion> nuevasFunciones)
        {
            if (Funciones == null)
            {
                Funciones = new List<Funcion>();
            }
            else
            {
                Funciones.Clear();
            }

            if (nuevasFunciones == null)
            {
                return;
            }

            foreach (var funcion in nuevasFunciones)
            {
                AgregarFuncion(funcion);
            }
        }

        #endregion
    }
}