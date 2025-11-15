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

        /// <summary>
        /// Inicializa una nueva instancia del servicio de productos usando la unidad de trabajo proporcionada.
        /// </summary>
        /// <param name="unitOfWork">Unidad de trabajo que provee acceso a los repositorios.</param>
        public ProductoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Busca productos cuyo nombre coincida con el término indicado.
        /// </summary>
        /// <param name="termino">Texto utilizado como criterio de búsqueda.</param>
        /// <returns>Colección de productos encontrados.</returns>
        public IEnumerable<Producto> Buscar(string termino)
        {
            return _unitOfWork.Productos.BuscarPorNombre(termino);
        }

        /// <summary>
        /// Busca asincrónicamente productos cuyo nombre coincida con el término indicado.
        /// </summary>
        /// <param name="termino">Texto utilizado como criterio de búsqueda.</param>
        /// <returns>Tarea que produce la colección de productos encontrados.</returns>
        public async Task<IEnumerable<Producto>> BuscarAsync(string termino)
        {
            return await _unitOfWork.Productos.BuscarPorNombreAsync(termino);
        }

        /// <summary>
        /// Obtiene la lista completa de productos registrados.
        /// </summary>
        /// <returns>Colección con todos los productos.</returns>
        public IEnumerable<Producto> ObtenerTodos()
        {
            return _unitOfWork.Productos.GetAll();
        }

        /// <summary>
        /// Obtiene de manera asincrónica la lista completa de productos registrados.
        /// </summary>
        /// <returns>Tarea que produce la colección de productos.</returns>
        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _unitOfWork.Productos.GetAllAsync();
        }

        /// <summary>
        /// Busca productos limitando los resultados para funcionalidades de autocompletado.
        /// </summary>
        /// <param name="termino">Texto a buscar.</param>
        /// <param name="maxResultados">Cantidad máxima de resultados.</param>
        /// <returns>Colección con los productos coincidentes.</returns>
        public IEnumerable<Producto> BuscarParaAutocomplete(string termino, int maxResultados = 10)
        {
            return _unitOfWork.Productos.BuscarParaAutocomplete(termino, maxResultados);
        }

        /// <summary>
        /// Busca asincrónicamente productos limitando los resultados para autocompletado.
        /// </summary>
        /// <param name="termino">Texto a buscar.</param>
        /// <param name="maxResultados">Cantidad máxima de resultados.</param>
        /// <returns>Tarea que produce los productos coincidentes.</returns>
        public async Task<IEnumerable<Producto>> BuscarParaAutocompleteAsync(string termino, int maxResultados = 10)
        {
            return await _unitOfWork.Productos.BuscarParaAutocompleteAsync(termino, maxResultados);
        }

        /// <summary>
        /// Obtiene un producto a partir de su identificador.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <returns>Producto encontrado o null si no existe.</returns>
        public Producto ObtenerPorId(Guid idProducto)
        {
            if (idProducto == Guid.Empty)
                return null;

            return _unitOfWork.Productos.GetById(idProducto);
        }

        /// <summary>
        /// Obtiene asincrónicamente un producto por su identificador.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <returns>Tarea que produce el producto encontrado.</returns>
        public async Task<Producto> ObtenerPorIdAsync(Guid idProducto)
        {
            if (idProducto == Guid.Empty)
                return null;

            return await _unitOfWork.Productos.GetByIdAsync(idProducto);
        }

        /// <summary>
        /// Obtiene un producto cuyo nombre coincida exactamente con el indicado.
        /// </summary>
        /// <param name="nombreProducto">Nombre exacto a buscar.</param>
        /// <returns>Producto encontrado o null si no existe.</returns>
        public Producto ObtenerPorNombreExacto(string nombreProducto)
        {
            return _unitOfWork.Productos.ObtenerPorNombreExacto(nombreProducto);
        }

        /// <summary>
        /// Obtiene asincrónicamente un producto cuyo nombre coincida exactamente con el indicado.
        /// </summary>
        /// <param name="nombreProducto">Nombre exacto a buscar.</param>
        /// <returns>Tarea que produce el producto encontrado.</returns>
        public async Task<Producto> ObtenerPorNombreExactoAsync(string nombreProducto)
        {
            return await _unitOfWork.Productos.ObtenerPorNombreExactoAsync(nombreProducto);
        }

        /// <summary>
        /// Crea un producto manualmente tras validar sus datos.
        /// </summary>
        /// <param name="producto">Producto a registrar.</param>
        /// <returns>Resultado del proceso de creación.</returns>
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

        /// <summary>
        /// Crea asincrónicamente un producto manual validando la información ingresada.
        /// </summary>
        /// <param name="producto">Producto a registrar.</param>
        /// <returns>Tarea que produce el resultado de la operación.</returns>
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

        /// <summary>
        /// Actualiza la información de un producto existente realizando las validaciones correspondientes.
        /// </summary>
        /// <param name="producto">Producto con los datos actualizados.</param>
        /// <returns>Resultado detallado de la actualización.</returns>
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

        /// <summary>
        /// Actualiza asincrónicamente la información de un producto existente.
        /// </summary>
        /// <param name="producto">Producto con los datos actualizados.</param>
        /// <returns>Tarea que produce el resultado de la actualización.</returns>
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

        /// <summary>
        /// Crea o reactiva un producto a partir de la carga de un pedido.
        /// </summary>
        /// <param name="nombreProducto">Nombre del producto solicitado.</param>
        /// <param name="idCategoria">Identificador de la categoría.</param>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        /// <returns>Tarea que produce el producto creado o actualizado.</returns>
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

        /// <summary>
        /// Registra un nuevo uso del producto indicado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto utilizado.</param>
        public void RegistrarUso(Guid idProducto)
        {
            if (idProducto == Guid.Empty)
                return;

            _unitOfWork.Productos.RegistrarUso(idProducto);
            _unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Registra asincrónicamente un nuevo uso del producto indicado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto utilizado.</param>
        public async Task RegistrarUsoAsync(Guid idProducto)
        {
            if (idProducto == Guid.Empty)
                return;

            await _unitOfWork.Productos.RegistrarUsoAsync(idProducto);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Cambia el estado de activación del producto indicado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <param name="activo">Nuevo estado a aplicar.</param>
        /// <returns>Resultado de la operación.</returns>
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

        /// <summary>
        /// Cambia asincrónicamente el estado de activación del producto indicado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <param name="activo">Nuevo estado a aplicar.</param>
        /// <returns>Tarea que produce el resultado de la operación.</returns>
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

        /// <summary>
        /// Obtiene las categorías de productos que se encuentran activas.
        /// </summary>
        /// <returns>Colección de categorías activas.</returns>
        public IEnumerable<CategoriaProducto> ObtenerCategoriasActivas()
        {
            return _unitOfWork.CategoriasProducto
                .GetCategoriasOrdenadas()
                .Where(c => c.Activo)
                .ToList();
        }

        /// <summary>
        /// Obtiene asincrónicamente las categorías de productos activas.
        /// </summary>
        /// <returns>Tarea que produce la colección de categorías activas.</returns>
        public async Task<IEnumerable<CategoriaProducto>> ObtenerCategoriasActivasAsync()
        {
            var categorias = await _unitOfWork.CategoriasProducto.GetCategoriasOrdenadasAsync();
            return categorias.Where(c => c.Activo).ToList();
        }

        /// <summary>
        /// Limpia y normaliza los datos del producto previo a su validación.
        /// </summary>
        /// <param name="producto">Producto a normalizar.</param>
        private static void NormalizarProducto(Producto producto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            if (!string.IsNullOrWhiteSpace(producto.NombreProducto))
                producto.NombreProducto = producto.NombreProducto.Trim();
        }

        /// <summary>
        /// Valida producto.
        /// </summary>
        /// <param name="producto">Producto a validar.</param>
        /// <returns>Resultado de la validación.</returns>
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