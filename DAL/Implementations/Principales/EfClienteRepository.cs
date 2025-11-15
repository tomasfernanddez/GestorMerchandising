using DAL.Implementations.Base;
using DAL.Interfaces.Principales;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL.Implementations.Principales
{
    public class EfClienteRepository : EfRepository<Cliente>, IClienteRepository
    {
        /// <summary>
        /// Inicializa el repositorio de clientes utilizando el contexto proporcionado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework de la aplicación.</param>
        public EfClienteRepository(GestorMerchandisingContext context) : base(context)
        {
        }


        /// <summary>
        /// Obtiene los clientes activos incluyendo su tipo de empresa y condición impositiva.
        /// </summary>
        /// <returns>Clientes activos ordenados por razón social.</returns>
        public IEnumerable<Cliente> GetClientesActivos()
        {
            return _dbSet.Where(c => c.Activo)
                        .Include(c => c.TipoEmpresa)
                        .Include(c => c.CondicionIva)
                        .OrderBy(c => c.RazonSocial)
                        .ToList();
        }

        /// <summary>
        /// Obtiene clientes filtrados por estado de actividad incluyendo datos relacionados.
        /// </summary>
        /// <param name="activo">Estado deseado: true activos, false inactivos, null todos.</param>
        /// <returns>Colección de clientes filtrados.</returns>
        public IEnumerable<Cliente> GetClientesPorEstado(bool? activo)
        {
            var query = _dbSet
                .Include(c => c.TipoEmpresa)
                .Include(c => c.CondicionIva)
                .AsQueryable();

            if (activo.HasValue)
                query = query.Where(c => c.Activo == activo.Value);

            return query
                .OrderBy(c => c.RazonSocial)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los clientes activos incluyendo su tipo de empresa y condición impositiva.
        /// </summary>
        /// <returns>Clientes activos ordenados por razón social.</returns>
        public async Task<IEnumerable<Cliente>> GetClientesActivosAsync()
        {
            return await _dbSet.Where(c => c.Activo)
                              .Include(c => c.TipoEmpresa)
                              .Include(c => c.CondicionIva)
                              .OrderBy(c => c.RazonSocial)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene los clientes activos de un tipo de empresa específico.
        /// </summary>
        /// <param name="idTipoEmpresa">Identificador del tipo de empresa.</param>
        /// <returns>Clientes asociados al tipo de empresa indicado.</returns>
        public IEnumerable<Cliente> GetClientesPorTipoEmpresa(Guid idTipoEmpresa)
        {
            return _dbSet.Where(c => c.IdTipoEmpresa == idTipoEmpresa && c.Activo)
                        .Include(c => c.TipoEmpresa)
                        .OrderBy(c => c.RazonSocial)
                        .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los clientes activos de un tipo de empresa específico.
        /// </summary>
        /// <param name="idTipoEmpresa">Identificador del tipo de empresa.</param>
        /// <returns>Clientes asociados al tipo de empresa indicado.</returns>
        public async Task<IEnumerable<Cliente>> GetClientesPorTipoEmpresaAsync(Guid idTipoEmpresa)
        {
            return await _dbSet.Where(c => c.IdTipoEmpresa == idTipoEmpresa && c.Activo)
                              .Include(c => c.TipoEmpresa)
                              .OrderBy(c => c.RazonSocial)
                              .ToListAsync();
        }

        /// <summary>
        /// Recupera un cliente por su CUIT.
        /// </summary>
        /// <param name="cuit">Número de CUIT del cliente.</param>
        /// <returns>Cliente encontrado o null.</returns>
        public Cliente GetClientePorCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return null;

            return _dbSet.Include(c => c.TipoEmpresa)
                        .Include(c => c.CondicionIva)
                        .FirstOrDefault(c => c.CUIT == cuit.Trim());
        }

        /// <summary>
        /// Recupera de forma asíncrona un cliente por su CUIT.
        /// </summary>
        /// <param name="cuit">Número de CUIT del cliente.</param>
        /// <returns>Cliente encontrado o null.</returns>
        public async Task<Cliente> GetClientePorCUITAsync(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return null;

            var limpio = cuit.Trim();
            return await _dbSet.Include(c => c.TipoEmpresa)
                            .Include(c => c.CondicionIva)
                            .FirstOrDefaultAsync(c => c.CUIT == limpio);
        }

        /// <summary>
        /// Busca clientes activos por razón social o alias.
        /// </summary>
        /// <param name="razonSocial">Texto a buscar.</param>
        /// <returns>Clientes coincidentes con el texto.</returns>
        public IEnumerable<Cliente> BuscarPorRazonSocial(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return new List<Cliente>();

            var termino = razonSocial.Trim().ToLower();

            return _dbSet.Where(c => c.RazonSocial.ToLower().Contains(termino)
                                   || (c.Alias != null && c.Alias.ToLower().Contains(termino)))
                        .Include(c => c.TipoEmpresa)
                        .Include(c => c.CondicionIva)
                        .OrderBy(c => c.RazonSocial)
                        .ToList();
        }

        /// <summary>
        /// Busca de forma asíncrona clientes activos por razón social o alias.
        /// </summary>
        /// <param name="razonSocial">Texto a buscar.</param>
        /// <returns>Clientes coincidentes con el texto.</returns>
        public async Task<IEnumerable<Cliente>> BuscarPorRazonSocialAsync(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return new List<Cliente>();

            var termino = razonSocial.Trim().ToLower();

            return await _dbSet.Where(c => c.RazonSocial.ToLower().Contains(termino)
                                         || (c.Alias != null && c.Alias.ToLower().Contains(termino)))
                              .Include(c => c.TipoEmpresa)
                              .Include(c => c.CondicionIva)
                              .OrderBy(c => c.RazonSocial)
                              .ToListAsync();
        }

        /// <summary>
        /// Verifica si existe un cliente con el CUIT especificado.
        /// </summary>
        /// /// <param name="cuit">Cuit a buscar.</param>
        /// <returns>Clientes coincidentes con el cuit.</returns>
        public bool ExisteCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            return _dbSet.Any(c => c.CUIT == cuit.Trim());
        }

        /// <summary>
        /// Verifica asincrónicamente si existe un cliente con el CUIT especificado.
        /// </summary>
        /// /// <param name="cuit">Cuit a buscar.</param>
        /// <returns>Clientes coincidentes con el cuit.</returns>
        public async Task<bool> ExisteCUITAsync(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            return await _dbSet.AnyAsync(c => c.CUIT == cuit.Trim());
        }

        /// <summary>
        /// Desactiva un cliente estableciendo su estado en inactivo.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        public void DesactivarCliente(Guid idCliente)
        {
            var cliente = GetById(idCliente);
            if (cliente != null)
            {
                cliente.Activo = false;
                Update(cliente);
            }
        }

        /// <summary>
        /// Activa un cliente estableciendo su estado en activo.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        public void ActivarCliente(Guid idCliente)
        {
            var cliente = GetById(idCliente);
            if (cliente != null)
            {
                cliente.Activo = true;
                Update(cliente);
            }
        }

        /// <summary>
        /// Obtiene los clientes activos que tienen pedidos asociados.
        /// </summary>
        /// <returns>Clientes con pedidos existentes.</returns>
        public IEnumerable<Cliente> GetClientesConPedidos()
        {
            return _dbSet.Where(c => c.Activo && c.Pedidos.Any())
                        .Include(c => c.TipoEmpresa)
                        .Include(c => c.Pedidos)
                        .OrderBy(c => c.RazonSocial)
                        .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los clientes activos que tienen pedidos asociados.
        /// </summary>
        /// <returns>Clientes con pedidos existentes.</returns>
        public async Task<IEnumerable<Cliente>> GetClientesConPedidosAsync()
        {
            return await _dbSet.Where(c => c.Activo && c.Pedidos.Any())
                              .Include(c => c.TipoEmpresa)
                              .Include(c => c.Pedidos)
                              .OrderBy(c => c.RazonSocial)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene un cliente con toda su información relacionada.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Cliente con todas las colecciones asociadas cargadas.</returns>
        public Cliente GetClienteCompleto(Guid idCliente)
        {
            return _dbSet.Where(c => c.IdCliente == idCliente)
                        .Include(c => c.TipoEmpresa)
                        .Include(c => c.Pedidos.Select(p => p.EstadoPedido))
                        .Include(c => c.PedidosMuestra.Select(pm => pm.EstadoPedidoMuestra))
                        .Include(c => c.Facturas)
                        .FirstOrDefault();
        }

        /// <summary>
        /// Obtiene de forma asíncrona un cliente con toda su información relacionada.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Cliente con todas las colecciones asociadas cargadas.</returns>
        public async Task<Cliente> GetClienteCompletoAsync(Guid idCliente)
        {
            return await _dbSet.Where(c => c.IdCliente == idCliente)
                              .Include(c => c.TipoEmpresa)
                              .Include(c => c.Pedidos.Select(p => p.EstadoPedido))
                              .Include(c => c.PedidosMuestra.Select(pm => pm.EstadoPedidoMuestra))
                              .Include(c => c.Facturas)
                              .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Busca clientes aplicando filtros generales opcionales.
        /// </summary>
        /// <param name="textoBusqueda">Texto libre para buscar en razón social o CUIT.</param>
        /// <param name="idTipoEmpresa">Tipo de empresa opcional para filtrar.</param>
        /// <param name="activo">Estado deseado del cliente.</param>
        /// <returns>Clientes que cumplen con los criterios indicados.</returns>
        public IEnumerable<Cliente> BuscarClientes(string textoBusqueda, Guid? idTipoEmpresa = null, bool? activo = null)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(textoBusqueda))
            {
                var termino = textoBusqueda.Trim().ToLower();
                query = query.Where(c => c.RazonSocial.ToLower().Contains(termino) ||
                                        c.CUIT.Contains(termino));
            }

            if (idTipoEmpresa.HasValue)
            {
                query = query.Where(c => c.IdTipoEmpresa == idTipoEmpresa.Value);
            }

            if (activo.HasValue)
            {
                query = query.Where(c => c.Activo == activo.Value);
            }

            return query.Include(c => c.TipoEmpresa)
                       .OrderBy(c => c.RazonSocial)
                       .ToList();
        }

        /// <summary>
        /// Calcula estadísticas simples sobre los clientes registrados.
        /// </summary>
        /// <returns>Objeto dinámico con datos agregados de clientes.</returns>
        public dynamic GetEstadisticasClientes()
        {
            var totalClientes = _dbSet.Count();
            var clientesActivos = _dbSet.Count(c => c.Activo);
            var clientesInactivos = totalClientes - clientesActivos;
            var clientesConPedidos = _dbSet.Count(c => c.Activo && c.Pedidos.Any());

            return new
            {
                TotalClientes = totalClientes,
                ClientesActivos = clientesActivos,
                ClientesInactivos = clientesInactivos,
                ClientesConPedidos = clientesConPedidos,
                PorcentajeActivos = totalClientes > 0 ? (clientesActivos * 100.0 / totalClientes) : 0
            };
        }
    }
}
