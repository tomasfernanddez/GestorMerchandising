using DAL.Interfaces.Base;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Referencia
{
    /// <summary>
    /// Define operaciones de acceso a datos para condiciones de IVA.
    /// </summary>
    public interface ICondicionIvaRepository : IRepository<CondicionIva>
    {
        /// <summary>
        /// Obtiene todas las condiciones de IVA ordenadas por nombre.
        /// </summary>
        /// <returns>Colección de condiciones de IVA.</returns>
        IEnumerable<CondicionIva> ObtenerTodas();

        /// <summary>
        /// Obtiene de forma asíncrona todas las condiciones de IVA ordenadas por nombre.
        /// </summary>
        /// <returns>Colección de condiciones de IVA.</returns>
        Task<IEnumerable<CondicionIva>> ObtenerTodasAsync();

        /// <summary>
        /// Verifica si existe una condición de IVA con el identificador indicado.
        /// </summary>
        /// <param name="idCondicionIva">Identificador de la condición de IVA.</param>
        /// <returns>True si existe.</returns>
        bool Existe(Guid idCondicionIva);

        /// <summary>
        /// Verifica de forma asíncrona si existe una condición de IVA con el identificador indicado.
        /// </summary>
        /// <param name="idCondicionIva">Identificador de la condición de IVA.</param>
        /// <returns>True si existe.</returns>
        Task<bool> ExisteAsync(Guid idCondicionIva);

        /// <summary>
        /// Obtiene una condición de IVA por su nombre normalizado.
        /// </summary>
        /// <param name="nombre">Nombre de la condición de IVA.</param>
        /// <returns>Condición encontrada o null.</returns>
        CondicionIva ObtenerPorNombre(string nombre);
    }
}