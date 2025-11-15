using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;

namespace DAL.Implementations.Referencia
{
    public class EfTipoProveedorRepository : EfRepository<TipoProveedor>, ITipoProveedorRepository
    {
        /// <summary>
        /// Inicializa el repositorio de tipos de proveedor con el contexto proporcionado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfTipoProveedorRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene los tipos de proveedor ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de tipos de proveedor.</returns>
        public IEnumerable<TipoProveedor> GetTiposOrdenados()
        {
            return _dbSet
                .OrderBy(tp => tp.TipoProveedorNombre)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los tipos de proveedor ordenados alfabéticamente.
        /// </summary>
        /// <returns>Colección de tipos de proveedor.</returns>
        public async Task<IEnumerable<TipoProveedor>> GetTiposOrdenadosAsync()
        {
            return await _dbSet
                .OrderBy(tp => tp.TipoProveedorNombre)
                .ToListAsync();
        }
    }
}