using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DAL.Interfaces.Base
{
    public interface IHasDbContext
    {
        /// <summary>
        /// Contexto de Entity Framework asociado a la instancia.
        /// </summary>
        DbContext Context { get; }
    }
}
