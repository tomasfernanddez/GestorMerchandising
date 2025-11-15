using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    /// <summary>
    /// Define operaciones de acceso a datos especializadas para clientes.
    /// </summary>
    public interface IClienteRepository : IRepository<Cliente>
    {
        /// <summary>
        /// Obtiene los clientes activos ordenados por razón social.
        /// </summary>
        /// <returns>Colección de clientes activos.</returns>
        IEnumerable<Cliente> GetClientesActivos();

        /// <summary>
        /// Obtiene clientes filtrados por su estado de actividad.
        /// </summary>
        /// <param name="activo">Estado deseado: true activos, false inactivos, null todos.</param>
        /// <returns>Colección de clientes filtrados.</returns>
        IEnumerable<Cliente> GetClientesPorEstado(bool? activo);

        /// <summary>
        /// Obtiene de forma asíncrona los clientes activos ordenados por razón social.
        /// </summary>
        /// <returns>Colección de clientes activos.</returns>
        Task<IEnumerable<Cliente>> GetClientesActivosAsync();

        /// <summary>
        /// Obtiene los clientes activos de un tipo de empresa específico.
        /// </summary>
        /// <param name="idTipoEmpresa">Identificador del tipo de empresa.</param>
        /// <returns>Colección de clientes filtrados.</returns>
        IEnumerable<Cliente> GetClientesPorTipoEmpresa(Guid idTipoEmpresa);

        /// <summary>
        /// Obtiene de forma asíncrona los clientes activos de un tipo de empresa específico.
        /// </summary>
        /// <param name="idTipoEmpresa">Identificador del tipo de empresa.</param>
        /// <returns>Colección de clientes filtrados.</returns>
        Task<IEnumerable<Cliente>> GetClientesPorTipoEmpresaAsync(Guid idTipoEmpresa);

        /// <summary>
        /// Recupera un cliente por su CUIT.
        /// </summary>
        /// <param name="cuit">Número de CUIT del cliente.</param>
        /// <returns>Cliente encontrado o null.</returns>
        Cliente GetClientePorCUIT(string cuit);

        /// <summary>
        /// Recupera de forma asíncrona un cliente por su CUIT.
        /// </summary>
        /// <param name="cuit">Número de CUIT del cliente.</param>
        /// <returns>Cliente encontrado o null.</returns>
        Task<Cliente> GetClientePorCUITAsync(string cuit);

        /// <summary>
        /// Busca clientes por coincidencias parciales en la razón social o alias.
        /// </summary>
        /// <param name="razonSocial">Texto a buscar.</param>
        /// <returns>Colección de clientes coincidentes.</returns>
        IEnumerable<Cliente> BuscarPorRazonSocial(string razonSocial);

        /// <summary>
        /// Busca de forma asíncrona clientes por coincidencias parciales en la razón social o alias.
        /// </summary>
        /// <param name="razonSocial">Texto a buscar.</param>
        /// <returns>Colección de clientes coincidentes.</returns>
        Task<IEnumerable<Cliente>> BuscarPorRazonSocialAsync(string razonSocial);

        /// <summary>
        /// Verifica si existe un cliente con el CUIT indicado.
        /// </summary>
        /// <param name="cuit">Número de CUIT a verificar.</param>
        /// <returns>True si el CUIT ya está registrado.</returns>
        bool ExisteCUIT(string cuit);

        /// <summary>
        /// Verifica de forma asíncrona si existe un cliente con el CUIT indicado.
        /// </summary>
        /// <param name="cuit">Número de CUIT a verificar.</param>
        /// <returns>True si el CUIT ya está registrado.</returns>
        Task<bool> ExisteCUITAsync(string cuit);

        /// <summary>
        /// Desactiva un cliente marcándolo como inactivo.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        void DesactivarCliente(Guid idCliente);

        /// <summary>
        /// Activa un cliente marcándolo como disponible nuevamente.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        void ActivarCliente(Guid idCliente);
    }
}