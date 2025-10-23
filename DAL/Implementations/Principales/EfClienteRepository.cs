using DAL.Implementations.Base;
using DAL.Interfaces.Principales;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL.Implementations.Principales
{
    public class EfClienteRepository : EfRepository<Cliente>, IClienteRepository
    {
        public EfClienteRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// Obtener todos los clientes activos
        public IEnumerable<Cliente> GetClientesActivos()
        {
            return _dbSet.Where(c => c.Activo == true)
                        .Include(c => c.TipoEmpresa)
                        .Include(c => c.CondicionIva)
                        .OrderBy(c => c.RazonSocial)
                        .ToList();
        }

        /// Obtiene todos los clientes activos (async)
        public async Task<IEnumerable<Cliente>> GetClientesActivosAsync()
        {
            return await _dbSet.Where(c => c.Activo == true)
                              .Include(c => c.TipoEmpresa)
                              .Include(c => c.CondicionIva)
                              .OrderBy(c => c.RazonSocial)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene clientes por tipo de empresa
        /// </summary>
        public IEnumerable<Cliente> GetClientesPorTipoEmpresa(Guid idTipoEmpresa)
        {
            return _dbSet.Where(c => c.IdTipoEmpresa == idTipoEmpresa && c.Activo == true)
                        .Include(c => c.TipoEmpresa)
                        .OrderBy(c => c.RazonSocial)
                        .ToList();
        }

        /// <summary>
        /// Obtiene clientes por tipo de empresa (async)
        /// </summary>
        public async Task<IEnumerable<Cliente>> GetClientesPorTipoEmpresaAsync(Guid idTipoEmpresa)
        {
            return await _dbSet.Where(c => c.IdTipoEmpresa == idTipoEmpresa && c.Activo == true)
                              .Include(c => c.TipoEmpresa)
                              .OrderBy(c => c.RazonSocial)
                              .ToListAsync();
        }

        /// <summary>
        /// Busca un cliente por CUIT
        /// </summary>
        public Cliente GetClientePorCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return null;

            return _dbSet.Include(c => c.TipoEmpresa)
                        .Include(c => c.CondicionIva)
                        .FirstOrDefault(c => c.CUIT == cuit.Trim());
        }

        /// <summary>
        /// Busca un cliente por CUIT (async)
        /// </summary>
        public async Task<Cliente> GetClientePorCUITAsync(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return null;

            return await _dbSet.Include(c => c.TipoEmpresa)
                            .Include(c => c.CondicionIva)
                            .FirstOrDefaultAsync(c => c.CUIT == cuit.Trim());
        }

        /// <summary>
        /// Busca clientes por razón social (búsqueda parcial)
        /// </summary>
        public IEnumerable<Cliente> BuscarPorRazonSocial(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return new List<Cliente>();

            var termino = razonSocial.Trim().ToLower();

            return _dbSet.Where(c => c.RazonSocial.ToLower().Contains(termino))
                        .Include(c => c.TipoEmpresa)
                        .Include(c => c.CondicionIva)
                        .OrderBy(c => c.RazonSocial)
                        .ToList();
        }

        /// <summary>
        /// Busca clientes por razón social (búsqueda parcial - async)
        /// </summary>
        public async Task<IEnumerable<Cliente>> BuscarPorRazonSocialAsync(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return new List<Cliente>();

            var termino = razonSocial.Trim().ToLower();

            return await _dbSet.Where(c => c.RazonSocial.ToLower().Contains(termino))
                              .Include(c => c.TipoEmpresa)
                              .Include(c => c.CondicionIva)
                              .OrderBy(c => c.RazonSocial)
                              .ToListAsync();
        }

        /// <summary>
        /// Verifica si existe un cliente con el CUIT especificado
        /// </summary>
        public bool ExisteCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            return _dbSet.Any(c => c.CUIT == cuit.Trim());
        }

        /// <summary>
        /// Verifica si existe un cliente con el CUIT especificado (async)
        /// </summary>
        public async Task<bool> ExisteCUITAsync(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            return await _dbSet.AnyAsync(c => c.CUIT == cuit.Trim());
        }

        /// <summary>
        /// Desactiva un cliente (no lo elimina físicamente)
        /// </summary>
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
        /// Activa un cliente
        /// </summary>
        public void ActivarCliente(Guid idCliente)
        {
            var cliente = GetById(idCliente);
            if (cliente != null)
            {
                cliente.Activo = true;
                Update(cliente);
            }
        }

        // ============================================================================
        // MÉTODOS ADICIONALES ÚTILES
        // ============================================================================

        /// <summary>
        /// Obtiene clientes con pedidos (incluye información relacionada)
        /// </summary>
        public IEnumerable<Cliente> GetClientesConPedidos()
        {
            return _dbSet.Where(c => c.Activo == true && c.Pedidos.Any())
                        .Include(c => c.TipoEmpresa)
                        .Include(c => c.Pedidos)
                        .OrderBy(c => c.RazonSocial)
                        .ToList();
        }

        /// <summary>
        /// Obtiene clientes con pedidos (async)
        /// </summary>
        public async Task<IEnumerable<Cliente>> GetClientesConPedidosAsync()
        {
            return await _dbSet.Where(c => c.Activo == true && c.Pedidos.Any())
                              .Include(c => c.TipoEmpresa)
                              .Include(c => c.Pedidos)
                              .OrderBy(c => c.RazonSocial)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene un cliente con toda su información relacionada
        /// </summary>
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
        /// Obtiene un cliente con toda su información relacionada (async)
        /// </summary>
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
        /// Busca clientes por múltiples criterios
        /// </summary>
        public IEnumerable<Cliente> BuscarClientes(string textoBusqueda, Guid? idTipoEmpresa = null, bool? activo = null)
        {
            var query = _dbSet.AsQueryable();

            // Filtro por texto (CUIT o Razón Social)
            if (!string.IsNullOrWhiteSpace(textoBusqueda))
            {
                var termino = textoBusqueda.Trim().ToLower();
                query = query.Where(c => c.RazonSocial.ToLower().Contains(termino) ||
                                        c.CUIT.Contains(termino));
            }

            // Filtro por tipo de empresa
            if (idTipoEmpresa.HasValue)
            {
                query = query.Where(c => c.IdTipoEmpresa == idTipoEmpresa.Value);
            }

            // Filtro por estado activo
            if (activo.HasValue)
            {
                query = query.Where(c => c.Activo == activo.Value);
            }

            return query.Include(c => c.TipoEmpresa)
                       .OrderBy(c => c.RazonSocial)
                       .ToList();
        }

        /// <summary>
        /// Obtiene estadísticas básicas de clientes
        /// </summary>
        public dynamic GetEstadisticasClientes()
        {
            var totalClientes = _dbSet.Count();
            var clientesActivos = _dbSet.Count(c => c.Activo == true);
            var clientesInactivos = totalClientes - clientesActivos;
            var clientesConPedidos = _dbSet.Count(c => c.Activo == true && c.Pedidos.Any());

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
