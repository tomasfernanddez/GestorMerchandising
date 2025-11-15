using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Interfaces.Base
{
    /// <summary>
    /// Define operaciones básicas de acceso a datos para un agregado de dominio.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad gestionada por el repositorio.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtiene una entidad por su identificador único.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>Entidad encontrada o null.</returns>
        T GetById(Guid id);

        /// <summary>
        /// Obtiene de forma asíncrona una entidad por su identificador único.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>Entidad encontrada o null.</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene todas las entidades gestionadas por el repositorio.
        /// </summary>
        /// <returns>Colección de entidades.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Obtiene de forma asíncrona todas las entidades gestionadas por el repositorio.
        /// </summary>
        /// <returns>Colección de entidades.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Recupera las entidades que cumplen con el criterio especificado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Colección filtrada de entidades.</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Recupera de forma asíncrona las entidades que cumplen con el criterio especificado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Colección filtrada de entidades.</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Obtiene la primera entidad que cumple con el criterio indicado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Entidad encontrada o null.</returns>
        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Obtiene de forma asíncrona la primera entidad que cumple con el criterio indicado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Entidad encontrada o null.</returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Agrega una nueva entidad al repositorio.
        /// </summary>
        /// <param name="entity">Entidad a agregar.</param>
        void Add(T entity);

        /// <summary>
        /// Agrega múltiples entidades al repositorio.
        /// </summary>
        /// <param name="entities">Colección de entidades a agregar.</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Actualiza la entidad especificada.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        void Update(T entity);

        /// <summary>
        /// Elimina la entidad especificada.
        /// </summary>
        /// <param name="entity">Entidad a eliminar.</param>
        void Remove(T entity);

        /// <summary>
        /// Elimina múltiples entidades del repositorio.
        /// </summary>
        /// <param name="entities">Colección de entidades a eliminar.</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Obtiene la cantidad total de entidades.
        /// </summary>
        /// <returns>Número total de entidades.</returns>
        int Count();

        /// <summary>
        /// Obtiene la cantidad de entidades que cumplen el criterio indicado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Número de entidades coincidentes.</returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Obtiene de forma asíncrona la cantidad total de entidades.
        /// </summary>
        /// <returns>Número total de entidades.</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Obtiene de forma asíncrona la cantidad de entidades que cumplen el criterio indicado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Número de entidades coincidentes.</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Verifica si existe una entidad con el identificador indicado.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>True si la entidad existe.</returns>
        bool Exists(Guid id);

        /// <summary>
        /// Verifica si existe alguna entidad que cumpla con el criterio indicado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>True si existe al menos una entidad que cumple el criterio.</returns>
        bool Exists(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Verifica de forma asíncrona si existe una entidad con el identificador indicado.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>True si la entidad existe.</returns>
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Verifica de forma asíncrona si existe alguna entidad que cumpla con el criterio indicado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>True si existe al menos una entidad que cumple el criterio.</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}