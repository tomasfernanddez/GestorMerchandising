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

        public EfRepository(GestorMerchandisingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        // ============================================================================
        // OPERACIONES DE CONSULTA
        // ============================================================================

        public virtual T GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        // ============================================================================
        // OPERACIONES DE MODIFICACIÓN
        // ============================================================================

        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

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

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.RemoveRange(entities);
        }

        // ============================================================================
        // OPERACIONES DE CONTEO
        // ============================================================================

        public virtual int Count()
        {
            return _dbSet.Count();
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        // ============================================================================
        // VERIFICACIÓN DE EXISTENCIA
        // ============================================================================

        public virtual bool Exists(Guid id)
        {
            return _dbSet.Find(id) != null;
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        // ============================================================================
        // MÉTODOS AUXILIARES PROTEGIDOS
        // ============================================================================

        /// <summary>
        /// Incluye propiedades de navegación en la consulta
        /// </summary>
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
        /// Incluye propiedades de navegación usando strings (para propiedades anidadas)
        /// </summary>
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
        /// Obtiene una consulta con paginación
        /// </summary>
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
        /// Obtiene entidades con paginación
        /// </summary>
        public virtual IEnumerable<T> GetPaged(int pageNumber, int pageSize)
        {
            return GetPagedQuery(_dbSet, pageNumber, pageSize).ToList();
        }

        /// <summary>
        /// Obtiene entidades con paginación (async)
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await GetPagedQuery(_dbSet, pageNumber, pageSize).ToListAsync();
        }

        /// <summary>
        /// Obtiene entidades con filtro y paginación
        /// </summary>
        public virtual IEnumerable<T> GetPaged(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            var query = _dbSet.Where(predicate);
            return GetPagedQuery(query, pageNumber, pageSize).ToList();
        }

        /// <summary>
        /// Obtiene entidades con filtro y paginación (async)
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            var query = _dbSet.Where(predicate);
            return await GetPagedQuery(query, pageNumber, pageSize).ToListAsync();
        }

        /// <summary>
        /// Recarga una entidad desde la base de datos
        /// </summary>
        public virtual void Reload(T entity)
        {
            _context.Entry(entity).Reload();
        }

        /// <summary>
        /// Recarga una entidad desde la base de datos (async)
        /// </summary>
        public virtual async Task ReloadAsync(T entity)
        {
            await _context.Entry(entity).ReloadAsync();
        }

        /// <summary>
        /// Desacopla una entidad del contexto
        /// </summary>
        public virtual void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
