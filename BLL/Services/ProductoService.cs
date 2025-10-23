using BLL.Helpers;
using BLL.Interfaces;
using DAL.Interfaces.Base;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Producto ObtenerProductoPorId(Guid idProducto)
        {
            if (idProducto == Guid.Empty)
                throw new ArgumentException("El ID del producto no puede estar vacío", nameof(idProducto));

            return _unitOfWork.Productos.GetById(idProducto);
        }

        public async Task<Producto> ObtenerProductoPorIdAsync(Guid idProducto)
        {
            if (idProducto == Guid.Empty)
                throw new ArgumentException("El ID del producto no puede estar vacío", nameof(idProducto));

            return await _unitOfWork.Productos.GetByIdAsync(idProducto);
        }

        public IEnumerable<Producto> ObtenerTodosLosProductos()
        {
            return _unitOfWork.Productos.GetAll();
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosLosProductosAsync()
        {
            return await Task.Run(() => _unitOfWork.Productos.GetAll());
        }

        public IEnumerable<Producto> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<Producto>();

            return _unitOfWork.Productos.BuscarPorNombre(nombre);
        }

        public async Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<Producto>();

            return await _unitOfWork.Productos.BuscarPorNombreAsync(nombre);
        }

        public IEnumerable<Producto> ObtenerPorCategoria(Guid idCategoria)
        {
            if (idCategoria == Guid.Empty)
                return new List<Producto>();

            return _unitOfWork.Productos.GetProductosPorCategoria(idCategoria);
        }

        public async Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(Guid idCategoria)
        {
            if (idCategoria == Guid.Empty)
                return new List<Producto>();

            return await _unitOfWork.Productos.GetProductosPorCategoriaAsync(idCategoria);
        }

        public IEnumerable<Producto> ObtenerPorProveedor(Guid idProveedor)
        {
            if (idProveedor == Guid.Empty)
                return new List<Producto>();

            return _unitOfWork.Productos.GetProductosPorProveedor(idProveedor);
        }

        public async Task<IEnumerable<Producto>> ObtenerPorProveedorAsync(Guid idProveedor)
        {
            if (idProveedor == Guid.Empty)
                return new List<Producto>();

            return await _unitOfWork.Productos.GetProductosPorProveedorAsync(idProveedor);
        }

        public ResultadoOperacion CrearProducto(Producto producto)
        {
            try
            {
                NormalizarProducto(producto);
                var validacion = ValidarProducto(producto);
                if (!validacion.EsValido)
                    return validacion;

                // Verificar si ya existe un producto con el mismo nombre
                if (_unitOfWork.Productos.ExisteNombre(producto.NombreProducto))
                {
                    return ResultadoOperacion.Error($"Ya existe un producto con el nombre '{producto.NombreProducto}'");
                }

                producto.IdProducto = Guid.NewGuid();

                _unitOfWork.Productos.Add(producto);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Producto creado exitosamente", producto.IdProducto);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el producto: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> CrearProductoAsync(Producto producto)
        {
            try
            {
                NormalizarProducto(producto);
                var validacion = ValidarProducto(producto);
                if (!validacion.EsValido)
                    return validacion;

                // Verificar si ya existe un producto con el mismo nombre
                if (await _unitOfWork.Productos.ExisteNombreAsync(producto.NombreProducto))
                {
                    return ResultadoOperacion.Error($"Ya existe un producto con el nombre '{producto.NombreProducto}'");
                }

                producto.IdProducto = Guid.NewGuid();

                _unitOfWork.Productos.Add(producto);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Producto creado exitosamente", producto.IdProducto);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el producto: {ex.Message}");
            }
        }

        public ResultadoOperacion ActualizarProducto(Producto producto)
        {
            try
            {
                NormalizarProducto(producto);
                var validacion = ValidarProducto(producto);
                if (!validacion.EsValido)
                    return validacion;

                var productoExistente = _unitOfWork.Productos.GetById(producto.IdProducto);
                if (productoExistente == null)
                    return ResultadoOperacion.Error("El producto no existe");

                _unitOfWork.Productos.Update(producto);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Producto actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el producto: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> ActualizarProductoAsync(Producto producto)
        {
            try
            {
                NormalizarProducto(producto);
                var validacion = ValidarProducto(producto);
                if (!validacion.EsValido)
                    return validacion;

                var productoExistente = await _unitOfWork.Productos.GetByIdAsync(producto.IdProducto);
                if (productoExistente == null)
                    return ResultadoOperacion.Error("El producto no existe");

                _unitOfWork.Productos.Update(producto);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Producto actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el producto: {ex.Message}");
            }
        }

        /// <summary>
        /// Método especial: crea el producto automáticamente si no existe, o devuelve el existente.
        /// Este es el flujo principal para pedidos: el usuario escribe el nombre y se crea automáticamente.
        /// </summary>
        public ResultadoOperacion CrearOObtenerProducto(string nombreProducto, Guid idCategoria, Guid idProveedor)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreProducto))
                    return ResultadoOperacion.Error("El nombre del producto es obligatorio");

                nombreProducto = nombreProducto.Trim();

                // Buscar si ya existe
                var productosExistentes = _unitOfWork.Productos.BuscarPorNombre(nombreProducto);
                var productoExistente = productosExistentes.FirstOrDefault(p =>
                    p.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

                if (productoExistente != null)
                {
                    // Ya existe, devolver su ID
                    return ResultadoOperacion.Exitoso("Producto encontrado", productoExistente.IdProducto);
                }

                // No existe, crearlo automáticamente
                var nuevoProducto = new Producto
                {
                    IdProducto = Guid.NewGuid(),
                    NombreProducto = nombreProducto,
                    IdCategoria = idCategoria,
                    IdProveedor = idProveedor
                };

                var validacion = ValidarProducto(nuevoProducto);
                if (!validacion.EsValido)
                    return validacion;

                _unitOfWork.Productos.Add(nuevoProducto);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Producto creado automáticamente", nuevoProducto.IdProducto);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear/obtener el producto: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> CrearOObtenerProductoAsync(string nombreProducto, Guid idCategoria, Guid idProveedor)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreProducto))
                    return ResultadoOperacion.Error("El nombre del producto es obligatorio");

                nombreProducto = nombreProducto.Trim();

                // Buscar si ya existe
                var productosExistentes = await _unitOfWork.Productos.BuscarPorNombreAsync(nombreProducto);
                var productoExistente = productosExistentes.FirstOrDefault(p =>
                    p.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

                if (productoExistente != null)
                {
                    // Ya existe, devolver su ID
                    return ResultadoOperacion.Exitoso("Producto encontrado", productoExistente.IdProducto);
                }

                // No existe, crearlo automáticamente
                var nuevoProducto = new Producto
                {
                    IdProducto = Guid.NewGuid(),
                    NombreProducto = nombreProducto,
                    IdCategoria = idCategoria,
                    IdProveedor = idProveedor
                };

                var validacion = ValidarProducto(nuevoProducto);
                if (!validacion.EsValido)
                    return validacion;

                _unitOfWork.Productos.Add(nuevoProducto);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Producto creado automáticamente", nuevoProducto.IdProducto);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear/obtener el producto: {ex.Message}");
            }
        }

        public IEnumerable<CategoriaProducto> ObtenerCategorias()
        {
            return _unitOfWork.CategoriasProducto.GetCategoriasOrdenadas();
        }

        public async Task<IEnumerable<CategoriaProducto>> ObtenerCategoriasAsync()
        {
            return await _unitOfWork.CategoriasProducto.GetCategoriasOrdenadasAsync();
        }

        public bool ExisteProducto(string nombre)
        {
            return _unitOfWork.Productos.ExisteNombre(nombre);
        }

        public async Task<bool> ExisteProductoAsync(string nombre)
        {
            return await _unitOfWork.Productos.ExisteNombreAsync(nombre);
        }

        // ============================================================================
        // MÉTODOS PRIVADOS DE VALIDACIÓN Y NORMALIZACIÓN
        // ============================================================================

        private void NormalizarProducto(Producto producto)
        {
            if (producto == null) return;
            producto.NombreProducto = producto.NombreProducto?.Trim();
        }

        private ResultadoOperacion ValidarProducto(Producto producto)
        {
            if (producto == null)
                return ResultadoOperacion.Error("El producto no puede ser nulo");

            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
                return ResultadoOperacion.Error("El nombre del producto es obligatorio");

            if (producto.NombreProducto.Length > 150)
                return ResultadoOperacion.Error("El nombre del producto no puede superar los 150 caracteres");

            if (!producto.IdCategoria.HasValue || producto.IdCategoria.Value == Guid.Empty)
                return ResultadoOperacion.Error("La categoría del producto es obligatoria");

            if (!producto.IdProveedor.HasValue || producto.IdProveedor.Value == Guid.Empty)
                return ResultadoOperacion.Error("El proveedor del producto es obligatorio");

            return ResultadoOperacion.Exitoso("Validación exitosa");
        }
    }
}
