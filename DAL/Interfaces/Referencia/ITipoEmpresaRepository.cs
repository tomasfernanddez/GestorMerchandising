using DAL.Interfaces.Base;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Referencia
{
    public interface ITipoEmpresaRepository : IRepository<TipoEmpresa>
    {
        IEnumerable<TipoEmpresa> GetTiposOrdenados();
        Task<IEnumerable<TipoEmpresa>> GetTiposOrdenadosAsync();
    }
}
