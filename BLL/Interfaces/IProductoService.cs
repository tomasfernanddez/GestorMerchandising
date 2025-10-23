using BLL.Helpers;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProductoService
    {
        /* Operaciones básicas */
        Producto ObtenerProductoPorId(Guid idProducto);
        Task<Producto> ObtenerProductoPorIdAsync(Guid idProducto);
        IEnumerable<Producto> ObtenerTodosLosProductos();
        Task<IEnumerable<Producto>> ObtenerTodosLosProductosAsync();

        /* Búsquedas */
        IEnumerable<Producto> BuscarPorNombre(string nombre);
        Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre);
        IEnumerable<Producto> ObtenerPorCategoria(Guid idCategoria);
        Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(Guid idCategoria);
        IEnumerable<Producto> ObtenerPorProveedor(Guid idProveedor);
        Task<IEnumerable<Producto>> ObtenerPorProveedorAsync(Guid idProveedor);

        /* Operaciones de modificación */
        ResultadoOperacion CrearProducto(Producto producto);
        Task<ResultadoOperacion> CrearProductoAsync(Producto producto);
        ResultadoOperacion ActualizarProducto(Producto producto);
        Task<ResultadoOperacion> ActualizarProductoAsync(Producto producto);

        /* Creación automática desde pedido */
        ResultadoOperacion CrearOObtenerProducto(string nombreProducto, Guid idCategoria, Guid idProveedor);
        Task<ResultadoOperacion> CrearOObtenerProductoAsync(string nombreProducto, Guid idCategoria, Guid idProveedor);

        /* Métodos auxiliares */
        IEnumerable<CategoriaProducto> ObtenerCategorias();
        Task<IEnumerable<CategoriaProducto>> ObtenerCategoriasAsync();
        bool ExisteProducto(string nombre);
        Task<bool> ExisteProductoAsync(string nombre);
    }
}
