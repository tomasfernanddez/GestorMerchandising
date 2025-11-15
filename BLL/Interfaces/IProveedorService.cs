using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Interfaces
{
    public interface IProveedorService
    {
        /// <summary>
        /// Obtiene proveedor por id.
        /// </summary>
        Proveedor ObtenerProveedorPorId(Guid idProveedor);
        /// <summary>
        /// Obtiene asincrónicamente proveedor por id.
        /// </summary>
        Task<Proveedor> ObtenerProveedorPorIdAsync(Guid idProveedor);
        /// <summary>
        /// Obtiene proveedores activos.
        /// </summary>
        IEnumerable<Proveedor> ObtenerProveedoresActivos();
        /// <summary>
        /// Obtiene asincrónicamente proveedores activos.
        /// </summary>
        Task<IEnumerable<Proveedor>> ObtenerProveedoresActivosAsync();
        /// <summary>
        /// Busca proveedores.
        /// </summary>
        IEnumerable<Proveedor> BuscarProveedores(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo);
        /// <summary>
        /// Busca proveedores.
        /// </summary>
        IEnumerable<Proveedor> BuscarProveedores(string razonSocial, Guid? idTipoProveedor, bool? activo);
        /// <summary>
        /// Busca asincrónicamente proveedores.
        /// </summary>
        Task<IEnumerable<Proveedor>> BuscarProveedoresAsync(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo);
        /// <summary>
        /// Busca asincrónicamente proveedores.
        /// </summary>
        Task<IEnumerable<Proveedor>> BuscarProveedoresAsync(string razonSocial, Guid? idTipoProveedor, bool? activo);
        /// <summary>
        /// Obtiene proveedor por cuit.
        /// </summary>
        Proveedor ObtenerProveedorPorCUIT(string cuit);
        /// <summary>
        /// Obtiene asincrónicamente proveedor por cuit.
        /// </summary>
        Task<Proveedor> ObtenerProveedorPorCUITAsync(string cuit);
        /// <summary>
        /// Crea proveedor.
        /// </summary>
        ResultadoOperacion CrearProveedor(Proveedor proveedor, IEnumerable<Guid> tiposProveedor, IEnumerable<Guid> tecnicasPersonalizacion);
        /// <summary>
        /// Crea asincrónicamente proveedor.
        /// </summary>
        Task<ResultadoOperacion> CrearProveedorAsync(Proveedor proveedor, IEnumerable<Guid> tiposProveedor, IEnumerable<Guid> tecnicasPersonalizacion);
        /// <summary>
        /// Actualiza proveedor.
        /// </summary>
        ResultadoOperacion ActualizarProveedor(Proveedor proveedor, IEnumerable<Guid> tiposProveedor, IEnumerable<Guid> tecnicasPersonalizacion);
        /// <summary>
        /// Desactiva proveedor.
        /// </summary>
        ResultadoOperacion DesactivarProveedor(Guid idProveedor);
        /// <summary>
        /// Activa proveedor.
        /// </summary>
        ResultadoOperacion ActivarProveedor(Guid idProveedor);
        /// <summary>
        /// Obtiene tipos proveedor.
        /// </summary>
        IEnumerable<TipoProveedor> ObtenerTiposProveedor();
        /// <summary>
        /// Obtiene asincrónicamente tipos proveedor.
        /// </summary>
        Task<IEnumerable<TipoProveedor>> ObtenerTiposProveedorAsync();
        /// <summary>
        /// Obtiene tecnicas personalizacion.
        /// </summary>
        IEnumerable<TecnicaPersonalizacion> ObtenerTecnicasPersonalizacion();
        /// <summary>
        /// Obtiene asincrónicamente tecnicas personalizacion.
        /// </summary>
        Task<IEnumerable<TecnicaPersonalizacion>> ObtenerTecnicasPersonalizacionAsync();
    }
}