using DAL.Interfaces.Base;
using DomainModel.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Referencia
{
    /// <summary>
    /// Define operaciones de acceso a datos para tipos de proveedor.
    /// </summary>
    public interface ITipoProveedorRepository : IRepository<TipoProveedor>
    {
        /// <summary>
        /// Obtiene los tipos de proveedor ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de tipos de proveedor.</returns>
        IEnumerable<TipoProveedor> GetTiposOrdenados();

        /// <summary>
        /// Obtiene de forma asíncrona los tipos de proveedor ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de tipos de proveedor.</returns>
        Task<IEnumerable<TipoProveedor>> GetTiposOrdenadosAsync();
    }
}
