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
    public class EfPedidoDetalleRepository : EfRepository<PedidoDetalle>, IPedidoDetalleRepository
    {
        public EfPedidoDetalleRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene todos los detalles de un pedido
        /// </summary>
        public IEnumerable<PedidoDetalle> GetDetallesPorPedido(Guid idPedido)
        {
            return _dbSet.Where(pd => pd.IdPedido == idPedido)
                        .Include(pd => pd.Producto)
                        .Include(pd => pd.Producto.Categoria)
                        .Include(pd => pd.Producto.Proveedor)
                        .Include(pd => pd.EstadoProducto)
                        .Include(pd => pd.LogosPedido.Select(l => l.TecnicaPersonalizacion))
                        .Include(pd => pd.LogosPedido.Select(l => l.UbicacionLogo))
                        .Include(pd => pd.LogosPedido.Select(l => l.ProveedorPersonalizacion))
                        .ToList();
        }

        /// <summary>
        /// Obtiene todos los detalles de un pedido (async)
        /// </summary>
        public async Task<IEnumerable<PedidoDetalle>> GetDetallesPorPedidoAsync(Guid idPedido)
        {
            return await _dbSet.Where(pd => pd.IdPedido == idPedido)
                              .Include(pd => pd.Producto)
                              .Include(pd => pd.Producto.Categoria)
                              .Include(pd => pd.Producto.Proveedor)
                              .Include(pd => pd.EstadoProducto)
                              .Include(pd => pd.LogosPedido.Select(l => l.TecnicaPersonalizacion))
                              .Include(pd => pd.LogosPedido.Select(l => l.UbicacionLogo))
                              .Include(pd => pd.LogosPedido.Select(l => l.ProveedorPersonalizacion))
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene todos los detalles que contienen un producto específico
        /// </summary>
        public IEnumerable<PedidoDetalle> GetDetallesPorProducto(Guid idProducto)
        {
            return _dbSet.Where(pd => pd.IdProducto == idProducto)
                        .Include(pd => pd.Pedido)
                        .Include(pd => pd.Pedido.Cliente)
                        .Include(pd => pd.Producto)
                        .Include(pd => pd.EstadoProducto)
                        .OrderByDescending(pd => pd.Pedido.Fecha)
                        .ToList();
        }

        /// <summary>
        /// Obtiene todos los detalles que contienen un producto específico (async)
        /// </summary>
        public async Task<IEnumerable<PedidoDetalle>> GetDetallesPorProductoAsync(Guid idProducto)
        {
            return await _dbSet.Where(pd => pd.IdProducto == idProducto)
                              .Include(pd => pd.Pedido)
                              .Include(pd => pd.Pedido.Cliente)
                              .Include(pd => pd.Producto)
                              .Include(pd => pd.EstadoProducto)
                              .OrderByDescending(pd => pd.Pedido.Fecha)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene detalles por estado de producto
        /// </summary>
        public IEnumerable<PedidoDetalle> GetDetallesPorEstado(Guid idEstadoProducto)
        {
            return _dbSet.Where(pd => pd.IdEstadoProducto == idEstadoProducto)
                        .Include(pd => pd.Pedido)
                        .Include(pd => pd.Pedido.Cliente)
                        .Include(pd => pd.Producto)
                        .Include(pd => pd.EstadoProducto)
                        .OrderByDescending(pd => pd.Pedido.Fecha)
                        .ToList();
        }

        /// <summary>
        /// Obtiene detalles por estado de producto (async)
        /// </summary>
        public async Task<IEnumerable<PedidoDetalle>> GetDetallesPorEstadoAsync(Guid idEstadoProducto)
        {
            return await _dbSet.Where(pd => pd.IdEstadoProducto == idEstadoProducto)
                              .Include(pd => pd.Pedido)
                              .Include(pd => pd.Pedido.Cliente)
                              .Include(pd => pd.Producto)
                              .Include(pd => pd.EstadoProducto)
                              .OrderByDescending(pd => pd.Pedido.Fecha)
                              .ToListAsync();
        }

        /// <summary>
        /// Calcula el total de un pedido sumando los subtotales de sus detalles
        /// </summary>
        public decimal GetTotalPedido(Guid idPedido)
        {
            return _dbSet.Where(pd => pd.IdPedido == idPedido)
                        .Sum(pd => (decimal?)pd.Subtotal) ?? 0m;
        }

        /// <summary>
        /// Calcula el total de un pedido sumando los subtotales de sus detalles (async)
        /// </summary>
        public async Task<decimal> GetTotalPedidoAsync(Guid idPedido)
        {
            return await _dbSet.Where(pd => pd.IdPedido == idPedido)
                              .SumAsync(pd => (decimal?)pd.Subtotal) ?? 0m;
        }
    }
}
