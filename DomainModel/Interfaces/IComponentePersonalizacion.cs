using System;
using System.Collections.Generic;

namespace DomainModel.Interfaces
{
    /// <summary>
    /// Patrón Composite - Componente base para personalizaciones
    /// Permite tratar de manera uniforme logos individuales y grupos de logos
    /// </summary>
    public interface IComponentePersonalizacion
    {
        /// <summary>
        /// Identificador único del componente
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Obtiene una descripción textual del componente de personalización
        /// </summary>
        /// <returns>Descripción del componente</returns>
        string ObtenerDescripcion();

        /// <summary>
        /// Calcula el costo total de la personalización
        /// En logos individuales, devuelve el costo de ese logo
        /// En grupos, suma recursivamente el costo de todos los componentes
        /// </summary>
        /// <returns>Costo total</returns>
        decimal CalcularCosto();

        /// <summary>
        /// Indica si este componente es un contenedor (composite) o una hoja (leaf)
        /// </summary>
        bool EsGrupo { get; }

        /// <summary>
        /// Obtiene todos los componentes hijos (solo para composites)
        /// Para hojas, devuelve una colección vacía
        /// </summary>
        IEnumerable<IComponentePersonalizacion> ObtenerComponentes();

        /// <summary>
        /// Agrega un componente hijo (solo para composites)
        /// Para hojas, lanza una excepción
        /// </summary>
        void Agregar(IComponentePersonalizacion componente);

        /// <summary>
        /// Elimina un componente hijo (solo para composites)
        /// Para hojas, lanza una excepción
        /// </summary>
        void Eliminar(IComponentePersonalizacion componente);

        /// <summary>
        /// Obtiene el ID del detalle de pedido asociado
        /// </summary>
        Guid IdDetallePedido { get; }
    }
}
