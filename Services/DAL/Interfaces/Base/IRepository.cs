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
        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        // Operaciones de modificación
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        // Operaciones de conteo
        int Count();
        int Count(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        // Verificación de existencia
        bool Exists(Guid id);
        bool Exists(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
