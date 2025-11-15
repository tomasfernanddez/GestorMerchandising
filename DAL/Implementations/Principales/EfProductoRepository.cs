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
        /// <summary>
        /// Crea una instancia del repositorio de productos utilizando el contexto indicado.
        /// </summary>
        /// <param name="context">Contexto de datos de Gestor Merchandising.</param>
        public EfProductoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Genera la consulta base con las relaciones necesarias para trabajar con productos.
        /// </summary>
        /// <returns>Consulta preparada con las inclusiones estándar.</returns>
        private IQueryable<Producto> QueryBase()
        {
            return _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Include(p => p.UnidadMedida);
        }

        /// <summary>
        /// Obtiene todos los productos incluyendo sus relaciones.
        /// </summary>
        /// <returns>Colección de productos.</returns>
        public override IEnumerable<Producto> GetAll()
        {
            return QueryBase().ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona todos los productos incluyendo sus relaciones.
        /// </summary>
        /// <returns>Colección de productos.</returns>
        public override async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await QueryBase().ToListAsync();
        }

        /// <summary>
        /// Recupera los productos activos de una categoría concreta.
        /// </summary>
        /// <param name="idCategoria">Identificador de la categoría.</param>
        /// <returns>Productos activos pertenecientes a la categoría.</returns>
        public IEnumerable<Producto> GetProductosPorCategoria(Guid idCategoria)
        {
            return QueryBase()
                .Where(p => p.Activo && p.IdCategoria == idCategoria)
                .OrderBy(p => p.NombreProducto)
                .ToList();
        }

        /// <summary>
        /// Recupera de forma asíncrona los productos activos de una categoría concreta.
        /// </summary>
        /// <param name="idCategoria">Identificador de la categoría.</param>
        /// <returns>Productos activos pertenecientes a la categoría.</returns>
        public async Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(Guid idCategoria)
        {
            return await QueryBase()
                .Where(p => p.Activo && p.IdCategoria == idCategoria)
                .OrderBy(p => p.NombreProducto)
                .ToListAsync();
        }

        /// <summary>
        /// Recupera los productos activos de un proveedor específico.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        /// <returns>Productos activos asociados al proveedor.</returns>
        public IEnumerable<Producto> GetProductosPorProveedor(Guid idProveedor)
        {
            return QueryBase()
                .Where(p => p.Activo && p.IdProveedor == idProveedor)
                .OrderBy(p => p.NombreProducto)
                .ToList();
        }

        /// <summary>
        /// Recupera de forma asíncrona los productos activos de un proveedor específico.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        /// <returns>Productos activos asociados al proveedor.</returns>
        public async Task<IEnumerable<Producto>> GetProductosPorProveedorAsync(Guid idProveedor)
        {
            return await QueryBase()
                .Where(p => p.Activo && p.IdProveedor == idProveedor)
                .OrderBy(p => p.NombreProducto)
                .ToListAsync();
        }

        /// <summary>
        /// Busca productos activos por nombre realizando coincidencias parciales.
        /// </summary>
        /// <param name="nombre">Texto a buscar.</param>
        /// <returns>Productos que contienen el texto indicado.</returns>
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

        /// <summary>
        /// Busca de forma asíncrona productos activos por nombre realizando coincidencias parciales.
        /// </summary>
        /// <param name="nombre">Texto a buscar.</param>
        /// <returns>Productos que contienen el texto indicado.</returns>
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

        /// <summary>
        /// Verifica si ya existe un producto con el nombre indicado.
        /// </summary>
        /// <param name="nombre">Nombre del producto a validar.</param>
        /// <returns>True si el nombre ya está registrado.</returns>
        public bool ExisteNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var normalizado = Normalizar(nombre);
            return _dbSet.Any(p => p.NombreProducto.ToLower() == normalizado);
        }

        /// <summary>
        /// Verifica de forma asíncrona si ya existe un producto con el nombre indicado.
        /// </summary>
        /// <param name="nombre">Nombre del producto a validar.</param>
        /// <returns>True si el nombre ya está registrado.</returns>
        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var normalizado = Normalizar(nombre);
            return await _dbSet.AnyAsync(p => p.NombreProducto.ToLower() == normalizado);
        }

        /// <summary>
        /// Obtiene un producto por su nombre normalizado.
        /// </summary>
        /// <param name="nombreNormalizado">Nombre del producto en minúsculas y sin espacios extra.</param>
        /// <returns>Producto que coincide exactamente con el nombre.</returns>
        public Producto ObtenerPorNombreExacto(string nombreNormalizado)
        {
            if (string.IsNullOrWhiteSpace(nombreNormalizado))
                return null;

            var normalizado = Normalizar(nombreNormalizado);
            return QueryBase().FirstOrDefault(p => p.NombreProducto.ToLower() == normalizado);
        }

        /// <summary>
        /// Obtiene de forma asíncrona un producto por su nombre normalizado.
        /// </summary>
        /// <param name="nombreNormalizado">Nombre del producto en minúsculas y sin espacios extra.</param>
        /// <returns>Producto que coincide exactamente con el nombre.</returns>
        public async Task<Producto> ObtenerPorNombreExactoAsync(string nombreNormalizado)
        {
            if (string.IsNullOrWhiteSpace(nombreNormalizado))
                return null;

            var normalizado = Normalizar(nombreNormalizado);
            return await QueryBase().FirstOrDefaultAsync(p => p.NombreProducto.ToLower() == normalizado);
        }

        /// <summary>
        /// Busca productos para autocompletado priorizando los más utilizados.
        /// </summary>
        /// <param name="termino">Texto ingresado por el usuario.</param>
        /// <param name="maxResultados">Cantidad máxima de resultados a retornar.</param>
        /// <returns>Listado paginado de productos para sugerencias.</returns>
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

        /// <summary>
        /// Busca de forma asíncrona productos para autocompletado priorizando los más utilizados.
        /// </summary>
        /// <param name="termino">Texto ingresado por el usuario.</param>
        /// <param name="maxResultados">Cantidad máxima de resultados a retornar.</param>
        /// <returns>Listado paginado de productos para sugerencias.</returns>
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

        /// <summary>
        /// Registra que un producto fue utilizado para priorizarlo en futuras búsquedas.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        public void RegistrarUso(Guid idProducto)
        {
            var producto = _dbSet.FirstOrDefault(p => p.IdProducto == idProducto);
            if (producto == null)
                return;

            producto.FechaUltimoUso = DateTime.UtcNow;
            producto.VecesUsado += 1;
            Update(producto);
        }

        /// <summary>
        /// Registra de forma asíncrona que un producto fue utilizado para priorizarlo en futuras búsquedas.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        public async Task RegistrarUsoAsync(Guid idProducto)
        {
            var producto = await _dbSet.FirstOrDefaultAsync(p => p.IdProducto == idProducto);
            if (producto == null)
                return;

            producto.FechaUltimoUso = DateTime.UtcNow;
            producto.VecesUsado += 1;
            Update(producto);
        }

        /// <summary>
        /// Normaliza un texto eliminando espacios adicionales y convirtiéndolo a minúsculas.
        /// </summary>
        /// <param name="texto">Texto a normalizar.</param>
        /// <returns>Texto normalizado.</returns>
        private static string Normalizar(string texto)
        {
            return texto.Trim().ToLower();
        }
    }
}