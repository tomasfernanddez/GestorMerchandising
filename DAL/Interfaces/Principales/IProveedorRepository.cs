using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    public interface IProveedorRepository : IRepository<Proveedor>
    {
        // Métodos específicos para Proveedor
        IEnumerable<Proveedor> GetProveedoresActivos();
        Task<IEnumerable<Proveedor>> GetProveedoresActivosAsync();
        IEnumerable<Proveedor> GetProveedoresPorTipo(Guid idTipoProveedor);
        Task<IEnumerable<Proveedor>> GetProveedoresPorTipoAsync(Guid idTipoProveedor);
        Proveedor GetProveedorPorCUIT(string cuit);
        Task<Proveedor> GetProveedorPorCUITAsync(string cuit);
        IEnumerable<Proveedor> BuscarPorRazonSocial(string razonSocial);
        Task<IEnumerable<Proveedor>> BuscarPorRazonSocialAsync(string razonSocial);
        bool ExisteCUIT(string cuit);
        Task<bool> ExisteCUITAsync(string cuit);
        void DesactivarProveedor(Guid idProveedor);
        void ActivarProveedor(Guid idProveedor);

        IEnumerable<Proveedor> Buscar(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo);
        Task<IEnumerable<Proveedor>> BuscarAsync(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo);
        Proveedor ObtenerConDetalles(Guid idProveedor);
        Task<Proveedor> ObtenerConDetallesAsync(Guid idProveedor);
    }
}