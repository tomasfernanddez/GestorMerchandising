using System.Collections.Generic;
using Services.DomainModel.Entities;

namespace Services.DomainModel.Interfaces
{
    /// <summary>
    /// Componente base del Patrón Composite para permisos dentro del módulo de usuarios y perfiles.
    /// Permite tratar de forma uniforme a funciones individuales y a agrupadores de funciones (perfiles).
    /// </summary>
    public interface IPermisoComponent
    {
        /// <summary>
        /// Obtiene un identificador de lectura humana para el componente.
        /// </summary>
        /// <returns>Identificador único dentro del contexto de permisos.</returns>
        string ObtenerIdentificador();

        /// <summary>
        /// Indica si el componente se encuentra activo dentro de la jerarquía de permisos.
        /// </summary>
        /// <returns><c>true</c> si el componente está activo.</returns>
        bool EstaActivo();

        /// <summary>
        /// Devuelve los subcomponentes directos que cuelgan de este nodo.
        /// Para componentes hoja devuelve una colección vacía.
        /// </summary>
        IEnumerable<IPermisoComponent> ObtenerHijos();

        /// <summary>
        /// Determina si el componente contiene una función con el código indicado.
        /// </summary>
        /// <param name="codigoFuncion">Código de la función a verificar.</param>
        /// <returns><c>true</c> si la función está presente.</returns>
        bool ContieneFuncion(string codigoFuncion);

        /// <summary>
        /// Obtiene todas las funciones de la rama correspondiente a este componente.
        /// </summary>
        /// <param name="soloActivas">Indica si se deben devolver únicamente funciones activas.</param>
        /// <returns>Colección plana de funciones.</returns>
        IEnumerable<Funcion> ObtenerFunciones(bool soloActivas = true);
    }
}