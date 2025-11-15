using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.DAL.Interfaces.Base
{
    public interface IRepository<T> where T : class
    {
        // Operaciones básicas
        /// <summary>
        /// Obtiene una entidad por su identificador.
        /// </summary>
        T GetById(Guid id);

        /// <summary>
        /// Obtiene de forma asíncrona una entidad por su identificador.
        /// </summary>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Recupera todas las entidades del repositorio.
        /// </summary>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Recupera de manera asíncrona todas las entidades del repositorio.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Obtiene las entidades que cumplen la condición especificada.
        /// </summary>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Obtiene de forma asíncrona las entidades que cumplen la condición especificada.
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Obtiene la primera entidad que cumple la condición o null si no existe.
        /// </summary>
        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Obtiene de forma asíncrona la primera entidad que cumple la condición o null.
        /// </summary>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        // Operaciones de modificación
        /// <summary>
        /// Agrega una entidad al repositorio.
        /// </summary>
        void Add(T entity);

        /// <summary>
        /// Agrega un conjunto de entidades al repositorio.
        /// </summary>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Marca una entidad para actualización.
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Elimina una entidad del repositorio.
        /// </summary>
        void Remove(T entity);

        /// <summary>
        /// Elimina un conjunto de entidades del repositorio.
        /// </summary>
        void RemoveRange(IEnumerable<T> entities);

        // Operaciones de conteo
        /// <summary>
        /// Obtiene la cantidad total de entidades.
        /// </summary>
        int Count();

        /// <summary>
        /// Obtiene la cantidad de entidades que cumplen el filtro indicado.
        /// </summary>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Obtiene de forma asíncrona la cantidad total de entidades.
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// Obtiene de forma asíncrona la cantidad de entidades que cumplen el filtro.
        /// </summary>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        // Verificación de existencia
        /// <summary>
        /// Verifica si existe una entidad con el identificador especificado.
        /// </summary>
        bool Exists(Guid id);

        /// <summary>
        /// Verifica si existe alguna entidad que cumpla el filtro indicado.
        /// </summary>
        bool Exists(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Verifica de forma asíncrona si existe una entidad con el identificador especificado.
        /// </summary>
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Verifica de forma asíncrona si existe alguna entidad que cumpla el filtro indicado.
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}