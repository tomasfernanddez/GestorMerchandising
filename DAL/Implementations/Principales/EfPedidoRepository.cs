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
    public class EfPedidoRepository : EfRepository<Pedido>, IPedidoRepository
    {
        public EfPedidoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene pedidos por cliente
        /// </summary>
        public IEnumerable<Pedido> GetPedidosPorCliente(Guid idCliente)
        {
            return _dbSet.Where(p => p.IdCliente == idCliente)
                        .Include(p => p.Cliente)
                        .Include(p => p.EstadoPedido)
                        .Include(p => p.TipoPago)
                        .Include(p => p.Detalles)
                        .OrderByDescending(p => p.Fecha)
                        .ToList();
        }

        /// <summary>
        /// Obtiene pedidos por cliente (async)
        /// </summary>
        public async Task<IEnumerable<Pedido>> GetPedidosPorClienteAsync(Guid idCliente)
        {
            return await _dbSet.Where(p => p.IdCliente == idCliente)
                              .Include(p => p.Cliente)
                              .Include(p => p.EstadoPedido)
                              .Include(p => p.TipoPago)
                              .Include(p => p.Detalles)
                              .OrderByDescending(p => p.Fecha)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene pedidos por estado
        /// </summary>
        public IEnumerable<Pedido> GetPedidosPorEstado(Guid idEstado)
        {
            return _dbSet.Where(p => p.IdEstadoPedido == idEstado)
                        .Include(p => p.Cliente)
                        .Include(p => p.EstadoPedido)
                        .Include(p => p.TipoPago)
                        .OrderByDescending(p => p.Fecha)
                        .ToList();
        }

        /// <summary>
        /// Obtiene pedidos por estado (async)
        /// </summary>
        public async Task<IEnumerable<Pedido>> GetPedidosPorEstadoAsync(Guid idEstado)
        {
            return await _dbSet.Where(p => p.IdEstadoPedido == idEstado)
                              .Include(p => p.Cliente)
                              .Include(p => p.EstadoPedido)
                              .Include(p => p.TipoPago)
                              .OrderByDescending(p => p.Fecha)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene pedidos por rango de fechas
        /// </summary>
        public IEnumerable<Pedido> GetPedidosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _dbSet.Where(p => p.Fecha >= fechaDesde && p.Fecha <= fechaHasta)
                        .Include(p => p.Cliente)
                        .Include(p => p.EstadoPedido)
                        .Include(p => p.TipoPago)
                        .OrderByDescending(p => p.Fecha)
                        .ToList();
        }

        /// <summary>
        /// Obtiene pedidos por rango de fechas (async)
        /// </summary>
        public async Task<IEnumerable<Pedido>> GetPedidosPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            return await _dbSet.Where(p => p.Fecha >= fechaDesde && p.Fecha <= fechaHasta)
                              .Include(p => p.Cliente)
                              .Include(p => p.EstadoPedido)
                              .Include(p => p.TipoPago)
                              .OrderByDescending(p => p.Fecha)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene pedidos que tienen fecha límite definida
        /// </summary>
        public IEnumerable<Pedido> GetPedidosConFechaLimite()
        {
            return _dbSet.Where(p => p.TieneFechaLimite && p.FechaLimite.HasValue)
                        .Include(p => p.Cliente)
                        .Include(p => p.EstadoPedido)
                        .OrderBy(p => p.FechaLimite)
                        .ToList();
        }

        /// <summary>
        /// Obtiene pedidos que tienen fecha límite definida (async)
        /// </summary>
        public async Task<IEnumerable<Pedido>> GetPedidosConFechaLimiteAsync()
        {
            return await _dbSet.Where(p => p.TieneFechaLimite && p.FechaLimite.HasValue)
                              .Include(p => p.Cliente)
                              .Include(p => p.EstadoPedido)
                              .OrderBy(p => p.FechaLimite)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene pedidos cuya fecha límite ha vencido
        /// </summary>
        public IEnumerable<Pedido> GetPedidosVencidos()
        {
            var hoy = DateTime.Today;
            return _dbSet.Where(p => p.TieneFechaLimite
                                    && p.FechaLimite.HasValue
                                    && p.FechaLimite.Value < hoy)
                        .Include(p => p.Cliente)
                        .Include(p => p.EstadoPedido)
                        .OrderBy(p => p.FechaLimite)
                        .ToList();
        }

        /// <summary>
        /// Obtiene pedidos cuya fecha límite ha vencido (async)
        /// </summary>
        public async Task<IEnumerable<Pedido>> GetPedidosVencidosAsync()
        {
            var hoy = DateTime.Today;
            return await _dbSet.Where(p => p.TieneFechaLimite
                                          && p.FechaLimite.HasValue
                                          && p.FechaLimite.Value < hoy)
                              .Include(p => p.Cliente)
                              .Include(p => p.EstadoPedido)
                              .OrderBy(p => p.FechaLimite)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene un pedido completo con todos sus detalles, logos, etc.
        /// </summary>
        public Pedido GetPedidoConDetalles(Guid idPedido)
        {
            return _dbSet.Where(p => p.IdPedido == idPedido)
                        .Include(p => p.Cliente)
                        .Include(p => p.EstadoPedido)
                        .Include(p => p.TipoPago)
                        .Include(p => p.Detalles.Select(d => d.Producto))
                        .Include(p => p.Detalles.Select(d => d.Producto.Categoria))
                        .Include(p => p.Detalles.Select(d => d.Producto.Proveedor))
                        .Include(p => p.Detalles.Select(d => d.EstadoProducto))
                        .Include(p => p.Detalles.Select(d => d.LogosPedido.Select(l => l.TecnicaPersonalizacion)))
                        .Include(p => p.Detalles.Select(d => d.LogosPedido.Select(l => l.UbicacionLogo)))
                        .Include(p => p.Detalles.Select(d => d.LogosPedido.Select(l => l.ProveedorPersonalizacion)))
                        .FirstOrDefault();
        }

        /// <summary>
        /// Obtiene un pedido completo con todos sus detalles, logos, etc. (async)
        /// </summary>
        public async Task<Pedido> GetPedidoConDetallesAsync(Guid idPedido)
        {
            return await _dbSet.Where(p => p.IdPedido == idPedido)
                              .Include(p => p.Cliente)
                              .Include(p => p.EstadoPedido)
                              .Include(p => p.TipoPago)
                              .Include(p => p.Detalles.Select(d => d.Producto))
                              .Include(p => p.Detalles.Select(d => d.Producto.Categoria))
                              .Include(p => p.Detalles.Select(d => d.Producto.Proveedor))
                              .Include(p => p.Detalles.Select(d => d.EstadoProducto))
                              .Include(p => p.Detalles.Select(d => d.LogosPedido.Select(l => l.TecnicaPersonalizacion)))
                              .Include(p => p.Detalles.Select(d => d.LogosPedido.Select(l => l.UbicacionLogo)))
                              .Include(p => p.Detalles.Select(d => d.LogosPedido.Select(l => l.ProveedorPersonalizacion)))
                              .FirstOrDefaultAsync();
        }
    }
}
