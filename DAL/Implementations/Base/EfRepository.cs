using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces.Base;

namespace DAL.Implementations.Base
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly GestorMerchandisingContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Inicializa el repositorio con el contexto de datos especificado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework para acceder a la base de datos.</param>
        public EfRepository(GestorMerchandisingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        // ============================================================================
        // OPERACIONES DE CONSULTA
        // ============================================================================

        /// <summary>
        /// Obtiene una entidad por su identificador único.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>Entidad encontrada o null si no existe.</returns>
        public virtual T GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Obtiene de forma asíncrona una entidad por su identificador único.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>Entidad encontrada o null si no existe.</returns>
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Obtiene todas las entidades del conjunto.
        /// </summary>
        /// <returns>Colección con todas las entidades.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona todas las entidades del conjunto.
        /// </summary>
        /// <returns>Colección con todas las entidades.</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Obtiene las entidades que cumplen con el criterio especificado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Colección de entidades filtradas.</returns>
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona las entidades que cumplen con el criterio especificado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Colección de entidades filtradas.</returns>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Obtiene la primera entidad que cumple con el criterio o el valor por defecto si no existe.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Entidad encontrada o null.</returns>
        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Obtiene de forma asíncrona la primera entidad que cumple con el criterio o el valor por defecto si no existe.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Entidad encontrada o null.</returns>
        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        // ============================================================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================================================

        /// <summary>
        /// Agrega una nueva entidad al contexto de seguimiento.
        /// </summary>
        /// <param name="entity">Entidad a agregar.</param>
        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);
        }

        /// <summary>
        /// Agrega varias entidades al contexto de seguimiento.
        /// </summary>
        /// <param name="entities">Colección de entidades a agregar.</param>
        public virtual void AddRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.AddRange(entities);
        }

        /// <summary>
        /// Marca una entidad como modificada para actualizarla en la base de datos.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Elimina una entidad del conjunto.
        /// </summary>
        /// <param name="entity">Entidad a eliminar.</param>
        public virtual void Remove(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Elimina varias entidades del conjunto.
        /// </summary>
        /// <param name="entities">Colección de entidades a eliminar.</param>
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.RemoveRange(entities);
        }

        // ============================================================================
        // OPERACIONES DE CONTEO
        // ============================================================================

        /// <summary>
        /// Obtiene la cantidad total de entidades del conjunto.
        /// </summary>
        /// <returns>Número total de entidades.</returns>
        public virtual int Count()
        {
            return _dbSet.Count();
        }

        /// <summary>
        /// Obtiene la cantidad de entidades que cumplen el criterio especificado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Número de entidades que cumplen el criterio.</returns>
        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        /// <summary>
        /// Obtiene de forma asíncrona la cantidad total de entidades del conjunto.
        /// </summary>
        /// <returns>Número total de entidades.</returns>
        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        /// <summary>
        /// Obtiene de forma asíncrona la cantidad de entidades que cumplen el criterio especificado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>Número de entidades que cumplen el criterio.</returns>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        // ============================================================================
        // VERIFICACIÓN DE EXISTENCIA
        // ============================================================================

        /// <summary>
        /// Verifica si existe una entidad con el identificador indicado.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>True si la entidad existe.</returns>
        public virtual bool Exists(Guid id)
        {
            return _dbSet.Find(id) != null;
        }

        /// <summary>
        /// Verifica si existe alguna entidad que cumpla con el criterio indicado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>True si existe una entidad que cumple el criterio.</returns>
        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        /// <summary>
        /// Verifica de forma asíncrona si existe una entidad con el identificador indicado.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>True si la entidad existe.</returns>
        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        /// <summary>
        /// Verifica de forma asíncrona si existe alguna entidad que cumpla con el criterio indicado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <returns>True si existe una entidad que cumple el criterio.</returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        // ============================================================================
        // MÉTODOS AUXILIARES PROTEGIDOS
        // ============================================================================

        /// <summary>
        /// Incluye propiedades de navegación en la consulta.
        /// </summary>
        /// <param name="includeProperties">Expresiones de las propiedades de navegación a incluir.</param>
        /// <returns>Consulta con las propiedades especificadas incluidas.</returns>
        protected virtual IQueryable<T> IncludeProperties(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        /// <summary>
        /// Incluye propiedades de navegación utilizando su nombre textual.
        /// </summary>
        /// <param name="includeProperties">Nombres de las propiedades de navegación a incluir.</param>
        /// <returns>Consulta con las propiedades especificadas incluidas.</returns>
        protected virtual IQueryable<T> IncludeProperties(params string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        /// <summary>
        /// Aplica la lógica de paginación a la consulta recibida.
        /// </summary>
        /// <param name="query">Consulta base sobre la que se paginará.</param>
        /// <param name="pageNumber">Número de página solicitado.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>Consulta con paginación aplicada.</returns>
        protected virtual IQueryable<T> GetPagedQuery(IQueryable<T> query, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        // ============================================================================
        // MÉTODOS ADICIONALES ÚTILES
        // ============================================================================

        /// <summary>
        /// Obtiene una página de entidades del conjunto.
        /// </summary>
        /// <param name="pageNumber">Número de página solicitado.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>Colección paginada de entidades.</returns>
        public virtual IEnumerable<T> GetPaged(int pageNumber, int pageSize)
        {
            return GetPagedQuery(_dbSet, pageNumber, pageSize).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona una página de entidades del conjunto.
        /// </summary>
        /// <param name="pageNumber">Número de página solicitado.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>Colección paginada de entidades.</returns>
        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await GetPagedQuery(_dbSet, pageNumber, pageSize).ToListAsync();
        }

        /// <summary>
        /// Obtiene una página de entidades que cumplen el criterio indicado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <param name="pageNumber">Número de página solicitado.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>Colección paginada de entidades filtradas.</returns>
        public virtual IEnumerable<T> GetPaged(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            var query = _dbSet.Where(predicate);
            return GetPagedQuery(query, pageNumber, pageSize).ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona una página de entidades que cumplen el criterio indicado.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <param name="pageNumber">Número de página solicitado.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>Colección paginada de entidades filtradas.</returns>
        public virtual async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            var query = _dbSet.Where(predicate);
            return await GetPagedQuery(query, pageNumber, pageSize).ToListAsync();
        }

        /// <summary>
        /// Vuelve a cargar los datos de la entidad desde la base de datos.
        /// </summary>
        /// <param name="entity">Entidad a recargar.</param>
        public virtual void Reload(T entity)
        {
            _context.Entry(entity).Reload();
        }

        /// <summary>
        /// Vuelve a cargar de forma asíncrona los datos de la entidad desde la base de datos.
        /// </summary>
        /// <param name="entity">Entidad a recargar.</param>
        public virtual async Task ReloadAsync(T entity)
        {
            await _context.Entry(entity).ReloadAsync();
        }

        /// <summary>
        /// Desacopla la entidad del contexto actual.
        /// </summary>
        /// <param name="entity">Entidad a desacoplar.</param>
        public virtual void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
