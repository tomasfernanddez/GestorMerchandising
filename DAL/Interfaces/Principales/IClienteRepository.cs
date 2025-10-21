using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        // Métodos específicos para Cliente
        IEnumerable<Cliente> GetClientesActivos();
        Task<IEnumerable<Cliente>> GetClientesActivosAsync();
        IEnumerable<Cliente> GetClientesPorTipoEmpresa(Guid idTipoEmpresa);
        Task<IEnumerable<Cliente>> GetClientesPorTipoEmpresaAsync(Guid idTipoEmpresa);
        Cliente GetClientePorCUIT(string cuit);
        Task<Cliente> GetClientePorCUITAsync(string cuit);
        IEnumerable<Cliente> BuscarPorRazonSocial(string razonSocial);
        Task<IEnumerable<Cliente>> BuscarPorRazonSocialAsync(string razonSocial);
        bool ExisteCUIT(string cuit);
        Task<bool> ExisteCUITAsync(string cuit);
        void DesactivarCliente(Guid idCliente);
        void ActivarCliente(Guid idCliente);
    }
}
