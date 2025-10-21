using DAL.Implementations.Base;
using DAL.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Extensions
{
    public static class UowEfExtensions
    {
        /// <summary>
        /// Devuelve el DbSet<T> asociado al DbContext que está detrás del UnitOfWork.
        /// Requiere que el UoW implemente IHasDbContext.
        /// </summary>
        public static DbSet<T> Set<T>(this IUnitOfWork uow) where T : class
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));

            if (!(uow is IHasDbContext hasCtx) || hasCtx.Context == null)
                throw new InvalidOperationException(
                    "El IUnitOfWork no expone un DbContext. Asegúrate de que tu UoW implemente IHasDbContext y tenga la propiedad Context.");

            return hasCtx.Context.Set<T>();
        }
    }
}
