using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Base
{
    /// <summary>
    /// Permite exponer el DbContext utilizado internamente por una implementación.
    /// </summary>
    public interface IHasDbContext
    {
        /// <summary>
        /// Contexto de Entity Framework asociado a la instancia.
        /// </summary>
        DbContext Context { get; }
    }
}
