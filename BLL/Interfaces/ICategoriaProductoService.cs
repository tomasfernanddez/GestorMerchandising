using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using DomainModel.Entidades;

namespace BLL.Interfaces
{
    public interface ICategoriaProductoService
    {
        /// <summary>
        /// Obtiene todas.
        /// </summary>
        IEnumerable<CategoriaProducto> ObtenerTodas();
        /// <summary>
        /// Obtiene asincrónicamente todas.
        /// </summary>
        Task<IEnumerable<CategoriaProducto>> ObtenerTodasAsync();
        /// <summary>
        /// Crea.
        /// </summary>
        ResultadoOperacion Crear(CategoriaProducto categoria);
        /// <summary>
        /// Crea asincrónicamente.
        /// </summary>
        Task<ResultadoOperacion> CrearAsync(CategoriaProducto categoria);
        /// <summary>
        /// Actualiza.
        /// </summary>
        ResultadoOperacion Actualizar(CategoriaProducto categoria);
        /// <summary>
        /// Actualiza asincrónicamente.
        /// </summary>
        Task<ResultadoOperacion> ActualizarAsync(CategoriaProducto categoria);
        /// <summary>
        /// Cambia estado.
        /// </summary>
        ResultadoOperacion CambiarEstado(Guid idCategoria, bool activo);
        /// <summary>
        /// Cambia asincrónicamente estado.
        /// </summary>
        Task<ResultadoOperacion> CambiarEstadoAsync(Guid idCategoria, bool activo);
        /// <summary>
        /// Reordena.
        /// </summary>
        ResultadoOperacion Reordenar(IReadOnlyList<Guid> nuevoOrden);
        /// <summary>
        /// Reordena asincrónicamente.
        /// </summary>
        Task<ResultadoOperacion> ReordenarAsync(IReadOnlyList<Guid> nuevoOrden);
    }
}