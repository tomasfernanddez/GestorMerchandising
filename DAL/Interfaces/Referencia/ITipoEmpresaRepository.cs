using DAL.Interfaces.Base;
using DomainModel.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Referencia
{
    /// <summary>
    /// Define operaciones de acceso a datos para tipos de empresa.
    /// </summary>
    public interface ITipoEmpresaRepository : IRepository<TipoEmpresa>
    {
        /// <summary>
        /// Obtiene los tipos de empresa ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de tipos de empresa.</returns>
        IEnumerable<TipoEmpresa> GetTiposOrdenados();

        /// <summary>
        /// Obtiene de forma asíncrona los tipos de empresa ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de tipos de empresa.</returns>
        Task<IEnumerable<TipoEmpresa>> GetTiposOrdenadosAsync();
    }
}