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
    public class EfProductoRepository : EfRepository<Producto>, IProductoRepository
    {
        public EfProductoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        private IQueryable<Producto> QueryBase()
        {
            return _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Include(p => p.UnidadMedida);
        }

        public override IEnumerable<Producto> GetAll()
        {
            return QueryBase().ToList();
        }

        public override async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await QueryBase().ToListAsync();
        }

        public IEnumerable<Producto> GetProductosPorCategoria(Guid idCategoria)
        {
            return QueryBase()
                .Where(p => p.Activo && p.IdCategoria == idCategoria)
                .OrderBy(p => p.NombreProducto)
                .ToList();
        }

        public async Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(Guid idCategoria)
        {
            return await QueryBase()
                .Where(p => p.Activo && p.IdCategoria == idCategoria)
                .OrderBy(p => p.NombreProducto)
                .ToListAsync();
        }

        public IEnumerable<Producto> GetProductosPorProveedor(Guid idProveedor)
        {
            return QueryBase()
                .Where(p => p.Activo && p.IdProveedor == idProveedor)
                .OrderBy(p => p.NombreProducto)
                .ToList();
        }

        public async Task<IEnumerable<Producto>> GetProductosPorProveedorAsync(Guid idProveedor)
        {
            return await QueryBase()
                .Where(p => p.Activo && p.IdProveedor == idProveedor)
                .OrderBy(p => p.NombreProducto)
                .ToListAsync();
        }

        public IEnumerable<Producto> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<Producto>();

            var termino = Normalizar(nombre);
            return QueryBase()
                .Where(p => p.Activo && p.NombreProducto.ToLower().Contains(termino))
                .OrderBy(p => p.NombreProducto)
                .ToList();
        }

        public async Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<Producto>();

            var termino = Normalizar(nombre);
            return await QueryBase()
                .Where(p => p.Activo && p.NombreProducto.ToLower().Contains(termino))
                .OrderBy(p => p.NombreProducto)
                .ToListAsync();
        }

        public bool ExisteNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var normalizado = Normalizar(nombre);
            return _dbSet.Any(p => p.NombreProducto.ToLower() == normalizado);
        }

        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var normalizado = Normalizar(nombre);
            return await _dbSet.AnyAsync(p => p.NombreProducto.ToLower() == normalizado);
        }

        public Producto ObtenerPorNombreExacto(string nombreNormalizado)
        {
            if (string.IsNullOrWhiteSpace(nombreNormalizado))
                return null;

            var normalizado = Normalizar(nombreNormalizado);
            return QueryBase().FirstOrDefault(p => p.NombreProducto.ToLower() == normalizado);
        }

        public async Task<Producto> ObtenerPorNombreExactoAsync(string nombreNormalizado)
        {
            if (string.IsNullOrWhiteSpace(nombreNormalizado))
                return null;

            var normalizado = Normalizar(nombreNormalizado);
            return await QueryBase().FirstOrDefaultAsync(p => p.NombreProducto.ToLower() == normalizado);
        }

        public IEnumerable<Producto> BuscarParaAutocomplete(string termino, int maxResultados = 10)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return new List<Producto>();

            var texto = Normalizar(termino);
            return QueryBase()
                .Where(p => p.Activo && p.NombreProducto.ToLower().Contains(texto))
                .OrderByDescending(p => p.FechaUltimoUso.HasValue)
                .ThenByDescending(p => p.FechaUltimoUso)
                .ThenBy(p => p.NombreProducto)
                .Take(maxResultados)
                .ToList();
        }

        public async Task<IEnumerable<Producto>> BuscarParaAutocompleteAsync(string termino, int maxResultados = 10)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return new List<Producto>();

            var texto = Normalizar(termino);
            return await QueryBase()
                .Where(p => p.Activo && p.NombreProducto.ToLower().Contains(texto))
                .OrderByDescending(p => p.FechaUltimoUso.HasValue)
                .ThenByDescending(p => p.FechaUltimoUso)
                .ThenBy(p => p.NombreProducto)
                .Take(maxResultados)
                .ToListAsync();
        }

        public void RegistrarUso(Guid idProducto)
        {
            var producto = _dbSet.FirstOrDefault(p => p.IdProducto == idProducto);
            if (producto == null)
                return;

            producto.FechaUltimoUso = DateTime.UtcNow;
            producto.VecesUsado += 1;
            Update(producto);
        }

        public async Task RegistrarUsoAsync(Guid idProducto)
        {
            var producto = await _dbSet.FirstOrDefaultAsync(p => p.IdProducto == idProducto);
            if (producto == null)
                return;

            producto.FechaUltimoUso = DateTime.UtcNow;
            producto.VecesUsado += 1;
            Update(producto);
        }

        private static string Normalizar(string texto)
        {
            return texto.Trim().ToLower();
        }
    }
}