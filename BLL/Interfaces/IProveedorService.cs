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
        Proveedor ObtenerProveedorPorId(Guid idProveedor);
        Task<Proveedor> ObtenerProveedorPorIdAsync(Guid idProveedor);
        IEnumerable<Proveedor> ObtenerProveedoresActivos();
        Task<IEnumerable<Proveedor>> ObtenerProveedoresActivosAsync();
        IEnumerable<Proveedor> BuscarProveedores(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo);
        Task<IEnumerable<Proveedor>> BuscarProveedoresAsync(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo);
        Proveedor ObtenerProveedorPorCUIT(string cuit);
        Task<Proveedor> ObtenerProveedorPorCUITAsync(string cuit);
        ResultadoOperacion CrearProveedor(Proveedor proveedor, IEnumerable<Guid> tecnicasPersonalizacion);
        Task<ResultadoOperacion> CrearProveedorAsync(Proveedor proveedor, IEnumerable<Guid> tecnicasPersonalizacion);
        ResultadoOperacion ActualizarProveedor(Proveedor proveedor, IEnumerable<Guid> tecnicasPersonalizacion);
        ResultadoOperacion DesactivarProveedor(Guid idProveedor);
        ResultadoOperacion ActivarProveedor(Guid idProveedor);
        IEnumerable<TipoProveedor> ObtenerTiposProveedor();
        Task<IEnumerable<TipoProveedor>> ObtenerTiposProveedorAsync();
        IEnumerable<TecnicaPersonalizacion> ObtenerTecnicasPersonalizacion();
        Task<IEnumerable<TecnicaPersonalizacion>> ObtenerTecnicasPersonalizacionAsync();
    }
}