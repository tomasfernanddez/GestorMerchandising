using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    /// <summary>
    /// Define operaciones de acceso a datos especializadas para productos.
    /// </summary>
    public interface IProductoRepository : IRepository<Producto>
    {
        /// <summary>
        /// Obtiene los productos activos pertenecientes a una categoría específica.
        /// </summary>
        /// <param name="idCategoria">Identificador de la categoría.</param>
        /// <returns>Colección de productos filtrados.</returns>
        IEnumerable<Producto> GetProductosPorCategoria(Guid idCategoria);

        /// <summary>
        /// Obtiene de forma asíncrona los productos activos pertenecientes a una categoría específica.
        /// </summary>
        /// <param name="idCategoria">Identificador de la categoría.</param>
        /// <returns>Colección de productos filtrados.</returns>
        Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(Guid idCategoria);

        /// <summary>
        /// Obtiene los productos activos asociados a un proveedor determinado.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        /// <returns>Colección de productos filtrados.</returns>
        IEnumerable<Producto> GetProductosPorProveedor(Guid idProveedor);

        /// <summary>
        /// Obtiene de forma asíncrona los productos activos asociados a un proveedor determinado.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        /// <returns>Colección de productos filtrados.</returns>
        Task<IEnumerable<Producto>> GetProductosPorProveedorAsync(Guid idProveedor);

        /// <summary>
        /// Busca productos por coincidencias parciales en el nombre.
        /// </summary>
        /// <param name="nombre">Texto a buscar.</param>
        /// <returns>Productos que coinciden con el texto.</returns>
        IEnumerable<Producto> BuscarPorNombre(string nombre);

        /// <summary>
        /// Busca de forma asíncrona productos por coincidencias parciales en el nombre.
        /// </summary>
        /// <param name="nombre">Texto a buscar.</param>
        /// <returns>Productos que coinciden con el texto.</returns>
        Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre);

        /// <summary>
        /// Determina si existe un producto con el nombre indicado.
        /// </summary>
        /// <param name="nombre">Nombre a verificar.</param>
        /// <returns>True si el nombre ya existe.</returns>
        bool ExisteNombre(string nombre);

        /// <summary>
        /// Determina de forma asíncrona si existe un producto con el nombre indicado.
        /// </summary>
        /// <param name="nombre">Nombre a verificar.</param>
        /// <returns>True si el nombre ya existe.</returns>
        Task<bool> ExisteNombreAsync(string nombre);

        /// <summary>
        /// Obtiene un producto por su nombre normalizado.
        /// </summary>
        /// <param name="nombreNormalizado">Nombre normalizado del producto.</param>
        /// <returns>Producto encontrado o null.</returns>
        Producto ObtenerPorNombreExacto(string nombreNormalizado);

        /// <summary>
        /// Obtiene de forma asíncrona un producto por su nombre normalizado.
        /// </summary>
        /// <param name="nombreNormalizado">Nombre normalizado del producto.</param>
        /// <returns>Producto encontrado o null.</returns>
        Task<Producto> ObtenerPorNombreExactoAsync(string nombreNormalizado);

        /// <summary>
        /// Busca productos para autocompletado priorizando los más usados.
        /// </summary>
        /// <param name="termino">Texto introducido por el usuario.</param>
        /// <param name="maxResultados">Cantidad máxima de resultados a devolver.</param>
        /// <returns>Listado de sugerencias de productos.</returns>
        IEnumerable<Producto> BuscarParaAutocomplete(string termino, int maxResultados = 10);

        /// <summary>
        /// Busca de forma asíncrona productos para autocompletado priorizando los más usados.
        /// </summary>
        /// <param name="termino">Texto introducido por el usuario.</param>
        /// <param name="maxResultados">Cantidad máxima de resultados a devolver.</param>
        /// <returns>Listado de sugerencias de productos.</returns>
        Task<IEnumerable<Producto>> BuscarParaAutocompleteAsync(string termino, int maxResultados = 10);

        /// <summary>
        /// Registra el uso de un producto para estadísticas y ordenamiento.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        void RegistrarUso(Guid idProducto);

        /// <summary>
        /// Registra de forma asíncrona el uso de un producto para estadísticas y ordenamiento.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        Task RegistrarUsoAsync(Guid idProducto);
    }
}