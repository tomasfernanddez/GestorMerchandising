using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Interfaces
{
    public interface IProductoService
    {
        /// <summary>
        /// Busca un producto.
        /// </summary>
        IEnumerable<Producto> Buscar(string termino);
        /// <summary>
        /// Busca asincrónicamente un producto.
        /// </summary>
        Task<IEnumerable<Producto>> BuscarAsync(string termino);
        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        IEnumerable<Producto> ObtenerTodos();
        /// <summary>
        /// Obtiene asincrónicamente todos los productos.
        /// </summary>
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        /// <summary>
        /// Busca para autocomplete.
        /// </summary>
        IEnumerable<Producto> BuscarParaAutocomplete(string termino, int maxResultados = 10);
        /// <summary>
        /// Busca asincrónicamente para autocomplete.
        /// </summary>
        Task<IEnumerable<Producto>> BuscarParaAutocompleteAsync(string termino, int maxResultados = 10);
        /// <summary>
        /// Obtiene producto por id.
        /// </summary>
        Producto ObtenerPorId(Guid idProducto);
        /// <summary>
        /// Obtiene producto asincrónicamente por id.
        /// </summary>
        Task<Producto> ObtenerPorIdAsync(Guid idProducto);
        /// <summary>
        /// Obtiene producto por nombre exacto.
        /// </summary>
        Producto ObtenerPorNombreExacto(string nombreProducto);
        /// <summary>
        /// Obtiene producto asincrónicamente por nombre exacto.
        /// </summary>
        Task<Producto> ObtenerPorNombreExactoAsync(string nombreProducto);
        /// <summary>
        /// Crea producto manual.
        /// </summary>
        ResultadoOperacion CrearProductoManual(Producto producto);
        /// <summary>
        /// Crea asincrónicamente producto manual.
        /// </summary>
        Task<ResultadoOperacion> CrearProductoManualAsync(Producto producto);
        /// <summary>
        /// Actualiza producto.
        /// </summary>
        ResultadoOperacion ActualizarProducto(Producto producto);
        /// <summary>
        /// Actualiza asincrónicamente producto.
        /// </summary>
        Task<ResultadoOperacion> ActualizarProductoAsync(Producto producto);
        /// <summary>
        /// Crea asincrónicamente producto desde pedido.
        /// </summary>
        Task<Producto> CrearProductoDesdePedidoAsync(string nombreProducto, Guid idCategoria, Guid idProveedor);
        /// <summary>
        /// Registra uso del producto.
        /// </summary>
        void RegistrarUso(Guid idProducto);
        /// <summary>
        /// Registra asincrónicamente uso del producto.
        /// </summary>
        Task RegistrarUsoAsync(Guid idProducto);
        /// <summary>
        /// Cambia estado del producto.
        /// </summary>
        ResultadoOperacion CambiarEstado(Guid idProducto, bool activo);
        /// <summary>
        /// Cambia asincrónicamente estado del producto.
        /// </summary>
        Task<ResultadoOperacion> CambiarEstadoAsync(Guid idProducto, bool activo);
        /// <summary>
        /// Obtiene categorias activas del producto.
        /// </summary>
        IEnumerable<CategoriaProducto> ObtenerCategoriasActivas();
        /// <summary>
        /// Obtiene asincrónicamente categorias activas del producto.
        /// </summary>
        Task<IEnumerable<CategoriaProducto>> ObtenerCategoriasActivasAsync();
    }
}