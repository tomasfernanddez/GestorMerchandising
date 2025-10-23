using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Principales;
using DomainModel;

namespace DAL.Implementations.Principales
{
    public class EfPedidoDetalleRepository : EfRepository<PedidoDetalle>, IPedidoDetalleRepository
    {
        public EfPedidoDetalleRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        private IQueryable<PedidoDetalle> QueryBase()
        {
            return _dbSet
                .Include(d => d.Producto)
                .Include(d => d.EstadoProducto)
                .Include(d => d.ProveedorPersonalizacion)
                .Include(d => d.LogosPedido.Select(l => l.TecnicaPersonalizacion))
                .Include(d => d.LogosPedido.Select(l => l.UbicacionLogo))
                .Include(d => d.LogosPedido.Select(l => l.Proveedor));
        }

        public override IEnumerable<PedidoDetalle> GetAll()
        {
            return QueryBase().ToList();
        }

        public override async Task<IEnumerable<PedidoDetalle>> GetAllAsync()
        {
            return await QueryBase().ToListAsync();
        }

        public IEnumerable<PedidoDetalle> GetDetallesPorPedido(Guid idPedido)
        {
            return QueryBase()
                .Where(d => d.IdPedido == idPedido)
                .OrderBy(d => d.Producto.NombreProducto)
                .ToList();
        }

        public async Task<IEnumerable<PedidoDetalle>> GetDetallesPorPedidoAsync(Guid idPedido)
        {
            return await QueryBase()
                .Where(d => d.IdPedido == idPedido)
                .OrderBy(d => d.Producto.NombreProducto)
                .ToListAsync();
        }

        public IEnumerable<PedidoDetalle> GetDetallesPorProducto(Guid idProducto)
        {
            return QueryBase()
                .Where(d => d.IdProducto == idProducto)
                .OrderByDescending(d => d.Pedido.FechaCreacion)
                .ToList();
        }

        public async Task<IEnumerable<PedidoDetalle>> GetDetallesPorProductoAsync(Guid idProducto)
        {
            return await QueryBase()
                .Where(d => d.IdProducto == idProducto)
                .OrderByDescending(d => d.Pedido.FechaCreacion)
                .ToListAsync();
        }

        public IEnumerable<PedidoDetalle> GetDetallesPorEstado(Guid idEstadoProducto)
        {
            return QueryBase()
                .Where(d => d.IdEstadoProducto == idEstadoProducto)
                .OrderBy(d => d.Producto.NombreProducto)
                .ToList();
        }

        public async Task<IEnumerable<PedidoDetalle>> GetDetallesPorEstadoAsync(Guid idEstadoProducto)
        {
            return await QueryBase()
                .Where(d => d.IdEstadoProducto == idEstadoProducto)
                .OrderBy(d => d.Producto.NombreProducto)
                .ToListAsync();
        }

        public decimal GetTotalPedido(Guid idPedido)
        {
            return QueryBase()
                .Where(d => d.IdPedido == idPedido)
                .Select(d => d.Cantidad * d.PrecioUnitario + d.LogosPedido.Sum(l => l.CostoPersonalizacion * (l.Cantidad <= 0 ? 1 : l.Cantidad)))
                .DefaultIfEmpty(0m)
                .Sum();
        }

        public async Task<decimal> GetTotalPedidoAsync(Guid idPedido)
        {
            var detalles = await QueryBase()
                .Where(d => d.IdPedido == idPedido)
                .ToListAsync();

            return detalles
                .Select(d => d.Cantidad * d.PrecioUnitario + d.LogosPedido.Sum(l => l.CostoPersonalizacion * (l.Cantidad <= 0 ? 1 : l.Cantidad)))
                .DefaultIfEmpty(0m)
                .Sum();
        }
    }
}