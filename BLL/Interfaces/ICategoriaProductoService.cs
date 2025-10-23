using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using DomainModel.Entidades;

namespace BLL.Interfaces
{
    public interface ICategoriaProductoService
    {
        IEnumerable<CategoriaProducto> ObtenerTodas();
        Task<IEnumerable<CategoriaProducto>> ObtenerTodasAsync();
        ResultadoOperacion Crear(CategoriaProducto categoria);
        Task<ResultadoOperacion> CrearAsync(CategoriaProducto categoria);
        ResultadoOperacion Actualizar(CategoriaProducto categoria);
        Task<ResultadoOperacion> ActualizarAsync(CategoriaProducto categoria);
        ResultadoOperacion CambiarEstado(Guid idCategoria, bool activo);
        Task<ResultadoOperacion> CambiarEstadoAsync(Guid idCategoria, bool activo);
        ResultadoOperacion Reordenar(IReadOnlyList<Guid> nuevoOrden);
        Task<ResultadoOperacion> ReordenarAsync(IReadOnlyList<Guid> nuevoOrden);
    }
}