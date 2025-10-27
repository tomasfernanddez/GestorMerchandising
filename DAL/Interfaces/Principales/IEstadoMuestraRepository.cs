using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Interfaces.Base;
using DomainModel.Entidades;

namespace DAL.Interfaces.Referencia
{
    public interface IEstadoMuestraRepository : IRepository<EstadoMuestra>
    {
        IEnumerable<EstadoMuestra> GetEstadosOrdenados();
        Task<IEnumerable<EstadoMuestra>> GetEstadosOrdenadosAsync();
    }
}