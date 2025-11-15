using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    /// <summary>
    /// Define operaciones de acceso a datos especializadas para proveedores.
    /// </summary>
    public interface IProveedorRepository : IRepository<Proveedor>
    {
        /// <summary>
        /// Obtiene los proveedores activos ordenados por razón social.
        /// </summary>
        /// <returns>Colección de proveedores activos.</returns>
        IEnumerable<Proveedor> GetProveedoresActivos();

        /// <summary>
        /// Obtiene de forma asíncrona los proveedores activos ordenados por razón social.
        /// </summary>
        /// <returns>Colección de proveedores activos.</returns>
        Task<IEnumerable<Proveedor>> GetProveedoresActivosAsync();

        /// <summary>
        /// Obtiene los proveedores activos asociados a un tipo de proveedor.
        /// </summary>
        /// <param name="idTipoProveedor">Identificador del tipo de proveedor.</param>
        /// <returns>Colección de proveedores filtrados.</returns>
        IEnumerable<Proveedor> GetProveedoresPorTipo(Guid idTipoProveedor);

        /// <summary>
        /// Obtiene de forma asíncrona los proveedores activos asociados a un tipo de proveedor.
        /// </summary>
        /// <param name="idTipoProveedor">Identificador del tipo de proveedor.</param>
        /// <returns>Colección de proveedores filtrados.</returns>
        Task<IEnumerable<Proveedor>> GetProveedoresPorTipoAsync(Guid idTipoProveedor);

        /// <summary>
        /// Recupera un proveedor por su CUIT.
        /// </summary>
        /// <param name="cuit">Número de CUIT del proveedor.</param>
        /// <returns>Proveedor encontrado o null.</returns>
        Proveedor GetProveedorPorCUIT(string cuit);

        /// <summary>
        /// Recupera de forma asíncrona un proveedor por su CUIT.
        /// </summary>
        /// <param name="cuit">Número de CUIT del proveedor.</param>
        /// <returns>Proveedor encontrado o null.</returns>
        Task<Proveedor> GetProveedorPorCUITAsync(string cuit);

        /// <summary>
        /// Busca proveedores por razón social o alias.
        /// </summary>
        /// <param name="razonSocial">Texto a buscar.</param>
        /// <returns>Colección de proveedores coincidentes.</returns>
        IEnumerable<Proveedor> BuscarPorRazonSocial(string razonSocial);

        /// <summary>
        /// Busca de forma asíncrona proveedores por razón social o alias.
        /// </summary>
        /// <param name="razonSocial">Texto a buscar.</param>
        /// <returns>Colección de proveedores coincidentes.</returns>
        Task<IEnumerable<Proveedor>> BuscarPorRazonSocialAsync(string razonSocial);

        /// <summary>
        /// Verifica si existe un proveedor con el CUIT indicado.
        /// </summary>
        /// <param name="cuit">Número de CUIT a verificar.</param>
        /// <returns>True si el CUIT ya está registrado.</returns>
        bool ExisteCUIT(string cuit);

        /// <summary>
        /// Verifica de forma asíncrona si existe un proveedor con el CUIT indicado.
        /// </summary>
        /// <param name="cuit">Número de CUIT a verificar.</param>
        /// <returns>True si el CUIT ya está registrado.</returns>
        Task<bool> ExisteCUITAsync(string cuit);

        /// <summary>
        /// Desactiva un proveedor marcándolo como inactivo.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        void DesactivarProveedor(Guid idProveedor);

        /// <summary>
        /// Activa un proveedor marcándolo como disponible.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        void ActivarProveedor(Guid idProveedor);

        /// <summary>
        /// Busca proveedores aplicando filtros combinados.
        /// </summary>
        /// <param name="razonSocial">Texto a buscar en razón social o alias.</param>
        /// <param name="cuit">Número de CUIT parcial o completo.</param>
        /// <param name="idTipoProveedor">Tipo de proveedor requerido.</param>
        /// <param name="activo">Estado deseado del proveedor.</param>
        /// <returns>Colección de proveedores que cumplen con los filtros.</returns>
        IEnumerable<Proveedor> Buscar(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo);

        /// <summary>
        /// Busca de forma asíncrona proveedores aplicando filtros combinados.
        /// </summary>
        /// <param name="razonSocial">Texto a buscar en razón social o alias.</param>
        /// <param name="cuit">Número de CUIT parcial o completo.</param>
        /// <param name="idTipoProveedor">Tipo de proveedor requerido.</param>
        /// <param name="activo">Estado deseado del proveedor.</param>
        /// <returns>Colección de proveedores que cumplen con los filtros.</returns>
        Task<IEnumerable<Proveedor>> BuscarAsync(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo);

        /// <summary>
        /// Obtiene un proveedor incluyendo toda su información relacionada.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        /// <returns>Proveedor con sus colecciones cargadas.</returns>
        Proveedor ObtenerConDetalles(Guid idProveedor);

        /// <summary>
        /// Obtiene de forma asíncrona un proveedor incluyendo toda su información relacionada.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        /// <returns>Proveedor con sus colecciones cargadas.</returns>
        Task<Proveedor> ObtenerConDetallesAsync(Guid idProveedor);
    }
}