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
        IEnumerable<Producto> Buscar(string termino);
        Task<IEnumerable<Producto>> BuscarAsync(string termino);
        IEnumerable<Producto> ObtenerTodos();
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        IEnumerable<Producto> BuscarParaAutocomplete(string termino, int maxResultados = 10);
        Task<IEnumerable<Producto>> BuscarParaAutocompleteAsync(string termino, int maxResultados = 10);
        Producto ObtenerPorId(Guid idProducto);
        Task<Producto> ObtenerPorIdAsync(Guid idProducto);
        Producto ObtenerPorNombreExacto(string nombreProducto);
        Task<Producto> ObtenerPorNombreExactoAsync(string nombreProducto);
        ResultadoOperacion CrearProductoManual(Producto producto);
        Task<ResultadoOperacion> CrearProductoManualAsync(Producto producto);
        ResultadoOperacion ActualizarProducto(Producto producto);
        Task<ResultadoOperacion> ActualizarProductoAsync(Producto producto);
        Task<Producto> CrearProductoDesdePedidoAsync(string nombreProducto, Guid idCategoria, Guid idProveedor);
        void RegistrarUso(Guid idProducto);
        Task RegistrarUsoAsync(Guid idProducto);
        ResultadoOperacion CambiarEstado(Guid idProducto, bool activo);
        Task<ResultadoOperacion> CambiarEstadoAsync(Guid idProducto, bool activo);
        IEnumerable<CategoriaProducto> ObtenerCategoriasActivas();
        Task<IEnumerable<CategoriaProducto>> ObtenerCategoriasActivasAsync();
    }
}