using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Interfaces.Base;
using DomainModel.Entidades;

namespace BLL.Services
{
    public class CategoriaProductoService : ICategoriaProductoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaProductoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<CategoriaProducto> ObtenerTodas()
        {
            return _unitOfWork.CategoriasProducto.GetCategoriasOrdenadas();
        }

        public async Task<IEnumerable<CategoriaProducto>> ObtenerTodasAsync()
        {
            return await _unitOfWork.CategoriasProducto.GetCategoriasOrdenadasAsync();
        }

        public ResultadoOperacion Crear(CategoriaProducto categoria)
        {
            try
            {
                NormalizarCategoria(categoria);
                var validacion = ValidarCategoria(categoria);
                if (!validacion.EsValido)
                    return validacion;

                if (ExisteNombre(categoria.NombreCategoria))
                    return ResultadoOperacion.Error("Ya existe una categoría con ese nombre");

                categoria.IdCategoria = Guid.NewGuid();
                categoria.Activo = true;
                categoria.FechaCreacion = DateTime.UtcNow;
                categoria.Orden = ObtenerSiguienteOrden();

                _unitOfWork.CategoriasProducto.Add(categoria);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Categoría creada correctamente", categoria.IdCategoria);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear la categoría: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> CrearAsync(CategoriaProducto categoria)
        {
            try
            {
                NormalizarCategoria(categoria);
                var validacion = ValidarCategoria(categoria);
                if (!validacion.EsValido)
                    return validacion;

                var existentes = await _unitOfWork.CategoriasProducto.GetAllAsync();
                if (existentes.Any(c => string.Equals(c.NombreCategoria, categoria.NombreCategoria, StringComparison.OrdinalIgnoreCase)))
                    return ResultadoOperacion.Error("Ya existe una categoría con ese nombre");

                categoria.IdCategoria = Guid.NewGuid();
                categoria.Activo = true;
                categoria.FechaCreacion = DateTime.UtcNow;
                categoria.Orden = ObtenerSiguienteOrden(existentes);

                _unitOfWork.CategoriasProducto.Add(categoria);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Categoría creada correctamente", categoria.IdCategoria);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear la categoría: {ex.Message}");
            }
        }

        public ResultadoOperacion Actualizar(CategoriaProducto categoria)
        {
            try
            {
                NormalizarCategoria(categoria);
                var validacion = ValidarCategoria(categoria);
                if (!validacion.EsValido)
                    return validacion;

                var existente = _unitOfWork.CategoriasProducto.GetById(categoria.IdCategoria);
                if (existente == null)
                    return ResultadoOperacion.Error("La categoría no existe");

                if (ExisteNombre(categoria.NombreCategoria, categoria.IdCategoria))
                    return ResultadoOperacion.Error("Ya existe otra categoría con ese nombre");

                existente.NombreCategoria = categoria.NombreCategoria;
                existente.Activo = categoria.Activo;
                _unitOfWork.CategoriasProducto.Update(existente);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Categoría actualizada correctamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar la categoría: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> ActualizarAsync(CategoriaProducto categoria)
        {
            try
            {
                NormalizarCategoria(categoria);
                var validacion = ValidarCategoria(categoria);
                if (!validacion.EsValido)
                    return validacion;

                var existente = await _unitOfWork.CategoriasProducto.GetByIdAsync(categoria.IdCategoria);
                if (existente == null)
                    return ResultadoOperacion.Error("La categoría no existe");

                var existentes = await _unitOfWork.CategoriasProducto.GetAllAsync();
                if (existentes.Any(c => c.IdCategoria != categoria.IdCategoria && string.Equals(c.NombreCategoria, categoria.NombreCategoria, StringComparison.OrdinalIgnoreCase)))
                    return ResultadoOperacion.Error("Ya existe otra categoría con ese nombre");

                existente.NombreCategoria = categoria.NombreCategoria;
                existente.Activo = categoria.Activo;
                _unitOfWork.CategoriasProducto.Update(existente);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Categoría actualizada correctamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar la categoría: {ex.Message}");
            }
        }

        public ResultadoOperacion CambiarEstado(Guid idCategoria, bool activo)
        {
            try
            {
                var categoria = _unitOfWork.CategoriasProducto.GetById(idCategoria);
                if (categoria == null)
                    return ResultadoOperacion.Error("La categoría no existe");

                categoria.Activo = activo;
                _unitOfWork.CategoriasProducto.Update(categoria);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Estado actualizado correctamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el estado de la categoría: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> CambiarEstadoAsync(Guid idCategoria, bool activo)
        {
            try
            {
                var categoria = await _unitOfWork.CategoriasProducto.GetByIdAsync(idCategoria);
                if (categoria == null)
                    return ResultadoOperacion.Error("La categoría no existe");

                categoria.Activo = activo;
                _unitOfWork.CategoriasProducto.Update(categoria);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Estado actualizado correctamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el estado de la categoría: {ex.Message}");
            }
        }

        public ResultadoOperacion Reordenar(IReadOnlyList<Guid> nuevoOrden)
        {
            if (nuevoOrden == null)
                return ResultadoOperacion.Error("El orden es obligatorio");

            var categorias = _unitOfWork.CategoriasProducto.GetAll().ToList();
            for (int i = 0; i < nuevoOrden.Count; i++)
            {
                var categoria = categorias.FirstOrDefault(c => c.IdCategoria == nuevoOrden[i]);
                if (categoria == null)
                    continue;

                categoria.Orden = i + 1;
                _unitOfWork.CategoriasProducto.Update(categoria);
            }

            _unitOfWork.SaveChanges();
            return ResultadoOperacion.Exitoso("Orden actualizado correctamente");
        }

        public async Task<ResultadoOperacion> ReordenarAsync(IReadOnlyList<Guid> nuevoOrden)
        {
            if (nuevoOrden == null)
                return ResultadoOperacion.Error("El orden es obligatorio");

            var categorias = (await _unitOfWork.CategoriasProducto.GetAllAsync()).ToList();
            for (int i = 0; i < nuevoOrden.Count; i++)
            {
                var categoria = categorias.FirstOrDefault(c => c.IdCategoria == nuevoOrden[i]);
                if (categoria == null)
                    continue;

                categoria.Orden = i + 1;
                _unitOfWork.CategoriasProducto.Update(categoria);
            }

            await _unitOfWork.SaveChangesAsync();
            return ResultadoOperacion.Exitoso("Orden actualizado correctamente");
        }

        private bool ExisteNombre(string nombre, Guid? idExcluida = null)
        {
            var categorias = _unitOfWork.CategoriasProducto.GetAll();
            return categorias.Any(c => (!idExcluida.HasValue || c.IdCategoria != idExcluida.Value) &&
                string.Equals(c.NombreCategoria, nombre, StringComparison.OrdinalIgnoreCase));
        }

        private static void NormalizarCategoria(CategoriaProducto categoria)
        {
            if (categoria == null)
                throw new ArgumentNullException(nameof(categoria));

            if (!string.IsNullOrWhiteSpace(categoria.NombreCategoria))
                categoria.NombreCategoria = categoria.NombreCategoria.Trim();
        }

        private static ResultadoOperacion ValidarCategoria(CategoriaProducto categoria)
        {
            if (categoria == null)
                return ResultadoOperacion.Error("La categoría es requerida");

            if (string.IsNullOrWhiteSpace(categoria.NombreCategoria))
                return ResultadoOperacion.Error("El nombre de la categoría es obligatorio");

            if (categoria.NombreCategoria.Length > 100)
                return ResultadoOperacion.Error("El nombre de la categoría no puede superar los 100 caracteres");

            return ResultadoOperacion.Exitoso("Validación correcta");
        }

        private int ObtenerSiguienteOrden(IEnumerable<CategoriaProducto> categorias = null)
        {
            categorias ??= _unitOfWork.CategoriasProducto.GetAll();
            if (!categorias.Any())
                return 1;

            return categorias.Max(c => c.Orden) + 1;
        }
    }
}