using DAL.Interfaces.Base;
using DomainModel.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Referencia
{
    /// <summary>
    /// Define operaciones de acceso a datos para categorías de producto.
    /// </summary>
    public interface ICategoriaProductoRepository : IRepository<CategoriaProducto>
    {
        /// <summary>
        /// Obtiene las categorías de producto ordenadas por estado y prioridad.
        /// </summary>
        /// <returns>Colección de categorías de producto.</returns>
        IEnumerable<CategoriaProducto> GetCategoriasOrdenadas();

        /// <summary>
        /// Obtiene de forma asíncrona las categorías de producto ordenadas por estado y prioridad.
        /// </summary>
        /// <returns>Colección de categorías de producto.</returns>
        Task<IEnumerable<CategoriaProducto>> GetCategoriasOrdenadasAsync();
    }
}