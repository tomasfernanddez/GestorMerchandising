using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Base
{
    public interface IHasDbContext
    {
        DbContext Context { get; }
    }
}
