using BLL.Helpers;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IClienteService
    {
        /* Operaciones básicas */
        Cliente ObtenerClientePorId(Guid idCliente);
        Task<Cliente> ObtenerClientePorIdAsync(Guid idCliente);
        IEnumerable<Cliente> ObtenerClientesActivos();
        Task<IEnumerable<Cliente>> ObtenerClientesActivosAsync();
        Cliente ObtenerClientePorCUIT(string cuit);
        Task<Cliente> ObtenerClientePorCUITAsync(string cuit);
        IEnumerable<Cliente> BuscarClientesPorRazonSocial(string razonSocial);
        Task<IEnumerable<Cliente>> BuscarClientesPorRazonSocialAsync(string razonSocial);
        /* Operaciones de modificación */
        ResultadoOperacion CrearCliente(Cliente cliente);
        Task<ResultadoOperacion> CrearClienteAsync(Cliente cliente);
        ResultadoOperacion ActualizarCliente(Cliente cliente);
        ResultadoOperacion DesactivarCliente(Guid idCliente);
        ResultadoOperacion ActivarCliente(Guid idCliente);

        /* Métodos auxiliares */
        IEnumerable<TipoEmpresa> ObtenerTiposEmpresa();
        object ObtenerEstadisticasClientes();
    }
}
