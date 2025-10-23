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
    public class EfProductoRepository : EfRepository<Producto>, IProductoRepository
    {
        public EfProductoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        public IEnumerable<Producto> GetProductosPorCategoria(Guid idCategoria)
        {
            return _dbSet.Where(p => p.IdCategoria == idCategoria)
                        .Include(p => p.Categoria)
                        .Include(p => p.Proveedor)
                        .OrderBy(p => p.NombreProducto)
                        .ToList();
        }

        /// <summary>
        /// Obtiene productos por categoría (async)
        /// </summary>
        public async Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(Guid idCategoria)
        {
            return await _dbSet.Where(p => p.IdCategoria == idCategoria)
                              .Include(p => p.Categoria)
                              .Include(p => p.Proveedor)
                              .OrderBy(p => p.NombreProducto)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos por proveedor
        /// </summary>
        public IEnumerable<Producto> GetProductosPorProveedor(Guid idProveedor)
        {
            return _dbSet.Where(p => p.IdProveedor == idProveedor)
                        .Include(p => p.Categoria)
                        .Include(p => p.Proveedor)
                        .OrderBy(p => p.NombreProducto)
                        .ToList();
        }

        /// <summary>
        /// Obtiene productos por proveedor (async)
        /// </summary>
        public async Task<IEnumerable<Producto>> GetProductosPorProveedorAsync(Guid idProveedor)
        {
            return await _dbSet.Where(p => p.IdProveedor == idProveedor)
                              .Include(p => p.Categoria)
                              .Include(p => p.Proveedor)
                              .OrderBy(p => p.NombreProducto)
                              .ToListAsync();
        }

        /// <summary>
        /// Busca productos por nombre (búsqueda parcial, case-insensitive)
        /// </summary>
        public IEnumerable<Producto> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<Producto>();

            var termino = nombre.Trim().ToLower();

            return _dbSet.Where(p => p.NombreProducto.ToLower().Contains(termino))
                        .Include(p => p.Categoria)
                        .Include(p => p.Proveedor)
                        .OrderBy(p => p.NombreProducto)
                        .ToList();
        }

        /// <summary>
        /// Busca productos por nombre (async)
        /// </summary>
        public async Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<Producto>();

            var termino = nombre.Trim().ToLower();

            return await _dbSet.Where(p => p.NombreProducto.ToLower().Contains(termino))
                              .Include(p => p.Categoria)
                              .Include(p => p.Proveedor)
                              .OrderBy(p => p.NombreProducto)
                              .ToListAsync();
        }

        /// <summary>
        /// Verifica si existe un producto con el nombre especificado
        /// </summary>
        public bool ExisteNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var nombreLimpio = nombre.Trim();
            return _dbSet.Any(p => p.NombreProducto.Equals(nombreLimpio, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica si existe un producto con el nombre especificado (async)
        /// </summary>
        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var nombreLimpio = nombre.Trim();
            return await _dbSet.AnyAsync(p => p.NombreProducto.Equals(nombreLimpio, StringComparison.OrdinalIgnoreCase));
        }
    }
}
