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
        /// <summary>
        /// Obtiene cliente por id.
        /// </summary>
        Cliente ObtenerClientePorId(Guid idCliente);
        /// <summary>
        /// Obtiene asincrónicamente cliente por id.
        /// </summary>
        Task<Cliente> ObtenerClientePorIdAsync(Guid idCliente);
        /// <summary>
        /// Obtiene clientes activos.
        /// </summary>
        IEnumerable<Cliente> ObtenerClientesActivos();
        /// <summary>
        /// /// Obtiene clientes filtrados por estado de actividad.
        /// </summary>
        /// <param name="activo">Estado deseado: true activos, false inactivos, null todos.</param>
        IEnumerable<Cliente> ObtenerClientesPorEstado(bool? activo = null);
        /// <summary>
        /// Obtiene asincrónicamente clientes activos.
        /// </summary>
        Task<IEnumerable<Cliente>> ObtenerClientesActivosAsync();
        /// <summary>
        /// Obtiene cliente por cuit.
        /// </summary>
        Cliente ObtenerClientePorCUIT(string cuit);
        /// <summary>
        /// Obtiene asincrónicamente cliente por cuit.
        /// </summary>
        Task<Cliente> ObtenerClientePorCUITAsync(string cuit);
        /// <summary>
        /// Busca clientes por razon social.
        /// </summary>
        IEnumerable<Cliente> BuscarClientesPorRazonSocial(string razonSocial);
        /// <summary>
        /// Busca asincrónicamente clientes por razon social.
        /// </summary>
        Task<IEnumerable<Cliente>> BuscarClientesPorRazonSocialAsync(string razonSocial);
        /* Operaciones de modificación */
        /// <summary>
        /// Crea cliente.
        /// </summary>
        ResultadoOperacion CrearCliente(Cliente cliente);
        /// <summary>
        /// Crea asincrónicamente cliente.
        /// </summary>
        Task<ResultadoOperacion> CrearClienteAsync(Cliente cliente);
        /// <summary>
        /// Actualiza cliente.
        /// </summary>
        ResultadoOperacion ActualizarCliente(Cliente cliente);
        /// <summary>
        /// Desactiva cliente.
        /// </summary>
        ResultadoOperacion DesactivarCliente(Guid idCliente);
        /// <summary>
        /// Activa cliente.
        /// </summary>
        ResultadoOperacion ActivarCliente(Guid idCliente);

        /* Métodos auxiliares */
        /// <summary>
        /// Obtiene tipos empresa.
        /// </summary>
        IEnumerable<TipoEmpresa> ObtenerTiposEmpresa();
        /// <summary>
        /// Obtiene estadisticas clientes.
        /// </summary>
        object ObtenerEstadisticasClientes();
    }
}