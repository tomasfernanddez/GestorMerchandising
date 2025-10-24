using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Interfaces.Base;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Producto> Buscar(string termino)
        {
            return _unitOfWork.Productos.BuscarPorNombre(termino);
        }

        public async Task<IEnumerable<Producto>> BuscarAsync(string termino)
        {
            return await _unitOfWork.Productos.BuscarPorNombreAsync(termino);
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            return _unitOfWork.Productos.GetAll();
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _unitOfWork.Productos.GetAllAsync();
        }

        public IEnumerable<Producto> BuscarParaAutocomplete(string termino, int maxResultados = 10)
        {
            return _unitOfWork.Productos.BuscarParaAutocomplete(termino, maxResultados);
        }

        public async Task<IEnumerable<Producto>> BuscarParaAutocompleteAsync(string termino, int maxResultados = 10)
        {
            return await _unitOfWork.Productos.BuscarParaAutocompleteAsync(termino, maxResultados);
        }

        public Producto ObtenerPorId(Guid idProducto)
        {
            if (idProducto == Guid.Empty)
                return null;

            return _unitOfWork.Productos.GetById(idProducto);
        }

        public async Task<Producto> ObtenerPorIdAsync(Guid idProducto)
        {
            if (idProducto == Guid.Empty)
                return null;

            return await _unitOfWork.Productos.GetByIdAsync(idProducto);
        }

        public Producto ObtenerPorNombreExacto(string nombreProducto)
        {
            return _unitOfWork.Productos.ObtenerPorNombreExacto(nombreProducto);
        }

        public async Task<Producto> ObtenerPorNombreExactoAsync(string nombreProducto)
        {
            return await _unitOfWork.Productos.ObtenerPorNombreExactoAsync(nombreProducto);
        }

        public ResultadoOperacion CrearProductoManual(Producto producto)
        {
            try
            {
                NormalizarProducto(producto);
                var validacion = ValidarProducto(producto);
                if (!validacion.EsValido)
                    return validacion;

                if (_unitOfWork.Productos.ExisteNombre(producto.NombreProducto))
                {
                    return ResultadoOperacion.Error($"Ya existe un producto con el nombre '{producto.NombreProducto}'");
                }

                producto.IdProducto = Guid.NewGuid();
                producto.FechaCreacion = DateTime.UtcNow;
                producto.Activo = true;
                producto.FechaUltimoUso = DateTime.UtcNow;
                producto.VecesUsado = 0;

                _unitOfWork.Productos.Add(producto);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Producto creado correctamente", producto.IdProducto);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el producto: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> CrearProductoManualAsync(Producto producto)
        {
            try
            {
                NormalizarProducto(producto);
                var validacion = ValidarProducto(producto);
                if (!validacion.EsValido)
                    return validacion;

                if (await _unitOfWork.Productos.ExisteNombreAsync(producto.NombreProducto))
                {
                    return ResultadoOperacion.Error($"Ya existe un producto con el nombre '{producto.NombreProducto}'");
                }

                producto.IdProducto = Guid.NewGuid();
                producto.FechaCreacion = DateTime.UtcNow;
                producto.Activo = true;
                producto.FechaUltimoUso = DateTime.UtcNow;
                producto.VecesUsado = 0;

                _unitOfWork.Productos.Add(producto);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Producto creado correctamente", producto.IdProducto);
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

                var existente = _unitOfWork.Productos.GetById(producto.IdProducto);
                if (existente == null)
                    return ResultadoOperacion.Error("El producto no existe");

                var duplicado = _unitOfWork.Productos.ObtenerPorNombreExacto(producto.NombreProducto);
                if (duplicado != null && duplicado.IdProducto != producto.IdProducto)
                    return ResultadoOperacion.Error("Ya existe otro producto con ese nombre");

                existente.NombreProducto = producto.NombreProducto;
                existente.IdCategoria = producto.IdCategoria;
                existente.IdProveedor = producto.IdProveedor;
                existente.Activo = producto.Activo;
                _unitOfWork.Productos.Update(existente);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Producto actualizado correctamente");
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

                var existente = await _unitOfWork.Productos.GetByIdAsync(producto.IdProducto);
                if (existente == null)
                    return ResultadoOperacion.Error("El producto no existe");

                var duplicado = await _unitOfWork.Productos.ObtenerPorNombreExactoAsync(producto.NombreProducto);
                if (duplicado != null && duplicado.IdProducto != producto.IdProducto)
                    return ResultadoOperacion.Error("Ya existe otro producto con ese nombre");

                existente.NombreProducto = producto.NombreProducto;
                existente.IdCategoria = producto.IdCategoria;
                existente.IdProveedor = producto.IdProveedor;
                existente.Activo = producto.Activo;
                _unitOfWork.Productos.Update(existente);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Producto actualizado correctamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el producto: {ex.Message}");
            }
        }

        public async Task<Producto> CrearProductoDesdePedidoAsync(string nombreProducto, Guid idCategoria, Guid idProveedor)
        {
            if (string.IsNullOrWhiteSpace(nombreProducto))
                throw new ArgumentException("El nombre del producto es obligatorio", nameof(nombreProducto));
            if (idCategoria == Guid.Empty)
                throw new ArgumentException("La categoría es obligatoria", nameof(idCategoria));
            if (idProveedor == Guid.Empty)
                throw new ArgumentException("El proveedor es obligatorio", nameof(idProveedor));

            var normalizado = nombreProducto.Trim();
            var existente = await _unitOfWork.Productos.ObtenerPorNombreExactoAsync(normalizado);
            if (existente != null)
            {
                existente.IdCategoria = idCategoria;
                existente.IdProveedor = idProveedor;
                existente.Activo = true;
                existente.FechaUltimoUso = DateTime.UtcNow;
                existente.VecesUsado += 1;
                _unitOfWork.Productos.Update(existente);
                await _unitOfWork.SaveChangesAsync();
                return existente;
            }

            var producto = new Producto
            {
                IdProducto = Guid.NewGuid(),
                NombreProducto = normalizado,
                IdCategoria = idCategoria,
                IdProveedor = idProveedor,
                Activo = true,
                FechaCreacion = DateTime.UtcNow,
                FechaUltimoUso = DateTime.UtcNow,
                VecesUsado = 1
            };

            _unitOfWork.Productos.Add(producto);
            await _unitOfWork.SaveChangesAsync();

            return producto;
        }

        public void RegistrarUso(Guid idProducto)
        {
            if (idProducto == Guid.Empty)
                return;

            _unitOfWork.Productos.RegistrarUso(idProducto);
            _unitOfWork.SaveChanges();
        }

        public async Task RegistrarUsoAsync(Guid idProducto)
        {
            if (idProducto == Guid.Empty)
                return;

            await _unitOfWork.Productos.RegistrarUsoAsync(idProducto);
            await _unitOfWork.SaveChangesAsync();
        }

        public ResultadoOperacion CambiarEstado(Guid idProducto, bool activo)
        {
            try
            {
                if (idProducto == Guid.Empty)
                    return ResultadoOperacion.Error("El identificador del producto es inválido");

                var producto = _unitOfWork.Productos.GetById(idProducto);
                if (producto == null)
                    return ResultadoOperacion.Error("El producto no existe");

                producto.Activo = activo;
                _unitOfWork.Productos.Update(producto);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Estado actualizado correctamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el estado del producto: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> CambiarEstadoAsync(Guid idProducto, bool activo)
        {
            try
            {
                if (idProducto == Guid.Empty)
                    return ResultadoOperacion.Error("El identificador del producto es inválido");

                var producto = await _unitOfWork.Productos.GetByIdAsync(idProducto);
                if (producto == null)
                    return ResultadoOperacion.Error("El producto no existe");

                producto.Activo = activo;
                _unitOfWork.Productos.Update(producto);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Estado actualizado correctamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el estado del producto: {ex.Message}");
            }
        }

        public IEnumerable<CategoriaProducto> ObtenerCategoriasActivas()
        {
            return _unitOfWork.CategoriasProducto
                .GetCategoriasOrdenadas()
                .Where(c => c.Activo)
                .ToList();
        }

        public async Task<IEnumerable<CategoriaProducto>> ObtenerCategoriasActivasAsync()
        {
            var categorias = await _unitOfWork.CategoriasProducto.GetCategoriasOrdenadasAsync();
            return categorias.Where(c => c.Activo).ToList();
        }

        private static void NormalizarProducto(Producto producto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            if (!string.IsNullOrWhiteSpace(producto.NombreProducto))
                producto.NombreProducto = producto.NombreProducto.Trim();
        }

        private static ResultadoOperacion ValidarProducto(Producto producto)
        {
            if (producto == null)
                return ResultadoOperacion.Error("El producto es requerido");

            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
                return ResultadoOperacion.Error("El nombre del producto es obligatorio");

            if (producto.NombreProducto.Length > 150)
                return ResultadoOperacion.Error("El nombre del producto no puede superar los 150 caracteres");

            if (!producto.IdCategoria.HasValue || producto.IdCategoria == Guid.Empty)
                return ResultadoOperacion.Error("Debe seleccionar una categoría");

            if (!producto.IdProveedor.HasValue || producto.IdProveedor == Guid.Empty)
                return ResultadoOperacion.Error("Debe seleccionar un proveedor");

            return ResultadoOperacion.Exitoso("Validación correcta");
        }
    }
}