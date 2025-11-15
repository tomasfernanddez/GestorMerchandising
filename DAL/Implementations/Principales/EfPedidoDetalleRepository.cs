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
        /// <summary>
        /// Inicializa el repositorio de detalles de pedido con el contexto de datos indicado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework de la aplicación.</param>
        public EfPedidoDetalleRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Construye la consulta base incluyendo todas las relaciones del detalle.
        /// </summary>
        /// <returns>Consulta preparada para reutilizar en las operaciones públicas.</returns>
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

        /// <summary>
        /// Obtiene todos los detalles de pedidos.
        /// </summary>
        /// <returns>Colección completa de detalles.</returns>
        public override IEnumerable<PedidoDetalle> GetAll()
        {
            return QueryBase().ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona todos los detalles de pedidos.
        /// </summary>
        /// <returns>Colección completa de detalles.</returns>
        public override async Task<IEnumerable<PedidoDetalle>> GetAllAsync()
        {
            return await QueryBase().ToListAsync();
        }

        /// <summary>
        /// Recupera los detalles pertenecientes a un pedido específico.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Detalles asociados al pedido.</returns>
        public IEnumerable<PedidoDetalle> GetDetallesPorPedido(Guid idPedido)
        {
            return QueryBase()
                .Where(d => d.IdPedido == idPedido)
                .OrderBy(d => d.Producto.NombreProducto)
                .ToList();
        }

        /// <summary>
        /// Recupera de forma asíncrona los detalles pertenecientes a un pedido específico.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Detalles asociados al pedido.</returns>
        public async Task<IEnumerable<PedidoDetalle>> GetDetallesPorPedidoAsync(Guid idPedido)
        {
            return await QueryBase()
                .Where(d => d.IdPedido == idPedido)
                .OrderBy(d => d.Producto.NombreProducto)
                .ToListAsync();
        }

        /// <summary>
        /// Recupera los detalles que corresponden a un producto determinado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <returns>Detalles donde interviene el producto.</returns>
        public IEnumerable<PedidoDetalle> GetDetallesPorProducto(Guid idProducto)
        {
            return QueryBase()
                .Where(d => d.IdProducto == idProducto)
                .OrderByDescending(d => d.Pedido.FechaCreacion)
                .ToList();
        }

        /// <summary>
        /// Recupera de forma asíncrona los detalles que corresponden a un producto determinado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <returns>Detalles donde interviene el producto.</returns>
        public async Task<IEnumerable<PedidoDetalle>> GetDetallesPorProductoAsync(Guid idProducto)
        {
            return await QueryBase()
                .Where(d => d.IdProducto == idProducto)
                .OrderByDescending(d => d.Pedido.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los detalles que se encuentran en un estado de producción determinado.
        /// </summary>
        /// <param name="idEstadoProducto">Identificador del estado del producto.</param>
        /// <returns>Detalles que coinciden con el estado.</returns>
        public IEnumerable<PedidoDetalle> GetDetallesPorEstado(Guid idEstadoProducto)
        {
            return QueryBase()
                .Where(d => d.IdEstadoProducto == idEstadoProducto)
                .OrderBy(d => d.Producto.NombreProducto)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los detalles que se encuentran en un estado de producción determinado.
        /// </summary>
        /// <param name="idEstadoProducto">Identificador del estado del producto.</param>
        /// <returns>Detalles que coinciden con el estado.</returns>
        public async Task<IEnumerable<PedidoDetalle>> GetDetallesPorEstadoAsync(Guid idEstadoProducto)
        {
            return await QueryBase()
                .Where(d => d.IdEstadoProducto == idEstadoProducto)
                .OrderBy(d => d.Producto.NombreProducto)
                .ToListAsync();
        }

        /// <summary>
        /// Calcula el total facturable para un pedido sumando productos y personalizaciones.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Total monetario del pedido.</returns>
        public decimal GetTotalPedido(Guid idPedido)
        {
            return QueryBase()
                .Where(d => d.IdPedido == idPedido)
                .Select(d => d.Cantidad * d.PrecioUnitario + d.LogosPedido.Sum(l => l.CostoPersonalizacion * (l.Cantidad <= 0 ? 1 : l.Cantidad)))
                .DefaultIfEmpty(0m)
                .Sum();
        }

        /// <summary>
        /// Calcula de forma asíncrona el total facturable para un pedido sumando productos y personalizaciones.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Total monetario del pedido.</returns>
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