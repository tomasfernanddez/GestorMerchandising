﻿using Services.BLL.Helpers;
using Services.BLL.Interfaces;
using Services.DAL.Interfaces.Base;
using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.BLL.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PerfilService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Perfil> ObtenerTodos()
        {
            try
            {
                return _unitOfWork.Perfiles.GetAllWithFunciones().OrderBy(p => p.NombrePerfil).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener perfiles: {ex.Message}", ex);
            }
        }

        public IEnumerable<Perfil> ObtenerPerfilesActivos()
        {
            try
            {
                return _unitOfWork.Perfiles.GetPerfilesActivos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener perfiles activos: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesActivosAsync()
        {
            try
            {
                return await _unitOfWork.Perfiles.GetPerfilesActivosAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener perfiles activos: {ex.Message}", ex);
            }
        }

        public Perfil ObtenerPorId(Guid idPerfil)
        {
            if (idPerfil == Guid.Empty)
                throw new ArgumentException("El ID del perfil no puede estar vacío", nameof(idPerfil));

            try
            {
                return _unitOfWork.Perfiles.GetByIdWithFunciones(idPerfil);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener perfil por ID: {ex.Message}", ex);
            }
        }

        public Perfil ObtenerPorNombre(string nombrePerfil)
        {
            if (string.IsNullOrWhiteSpace(nombrePerfil))
                throw new ArgumentException("El nombre del perfil no puede estar vacío", nameof(nombrePerfil));

            try
            {
                return _unitOfWork.Perfiles.GetPerfilPorNombre(nombrePerfil);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener perfil por nombre: {ex.Message}", ex);
            }
        }

        public IEnumerable<Funcion> ObtenerFuncionesDisponibles()
        {
            try
            {
                return _unitOfWork.Funciones
                    .GetAll()
                    .Where(f => f.Activo)
                    .OrderBy(f => f.Nombre)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las funciones disponibles: {ex.Message}", ex);
            }
        }

        public ResultadoOperacion CrearPerfil(Perfil perfil)
        {
            try
            {
                var validacion = ValidarPerfil(perfil, esNuevo: true);
                if (!validacion.EsValido)
                    return validacion;

                perfil.IdPerfil = Guid.NewGuid();
                perfil.NombrePerfil = perfil.NombrePerfil.Trim();
                perfil.Descripcion = string.IsNullOrWhiteSpace(perfil.Descripcion)
                    ? null
                    : perfil.Descripcion.Trim();
                perfil.Activo = true;

                SincronizarFunciones(perfil, perfil.Funciones?.Select(f => f.IdFuncion));

                _unitOfWork.Perfiles.Add(perfil);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Perfil creado exitosamente", perfil.IdPerfil);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el perfil: {ObtenerMensajeProfundo(ex)}");
            }
        }

        public ResultadoOperacion ActualizarPerfil(Perfil perfil)
        {
            try
            {
                var validacion = ValidarPerfil(perfil, esNuevo: false);
                if (!validacion.EsValido)
                    return validacion;

                var existente = _unitOfWork.Perfiles.GetByIdWithFunciones(perfil.IdPerfil);
                if (existente == null)
                    return ResultadoOperacion.Error("El perfil no existe");

                if (!perfil.Activo && TieneUsuariosActivos(perfil.IdPerfil))
                    return ResultadoOperacion.Error("No se puede desactivar el perfil porque tiene usuarios activos asociados");

                existente.NombrePerfil = perfil.NombrePerfil.Trim();
                existente.Descripcion = string.IsNullOrWhiteSpace(perfil.Descripcion)
                    ? null
                    : perfil.Descripcion.Trim();
                existente.Activo = perfil.Activo;

                SincronizarFunciones(existente, perfil.Funciones?.Select(f => f.IdFuncion));

                _unitOfWork.Perfiles.Update(existente);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Perfil actualizado exitosamente", existente.IdPerfil);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el perfil: {ObtenerMensajeProfundo(ex)}");
            }
        }

        public ResultadoOperacion ActivarPerfil(Guid idPerfil)
        {
            if (idPerfil == Guid.Empty)
                return ResultadoOperacion.Error("El ID del perfil no puede estar vacío");

            try
            {
                var perfil = _unitOfWork.Perfiles.GetById(idPerfil);
                if (perfil == null)
                    return ResultadoOperacion.Error("El perfil no existe");

                if (perfil.Activo)
                    return ResultadoOperacion.Exitoso("El perfil ya estaba activo", idPerfil);

                perfil.Activo = true;
                _unitOfWork.Perfiles.Update(perfil);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Perfil activado correctamente", idPerfil);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al activar el perfil: {ObtenerMensajeProfundo(ex)}");
            }
        }

        public ResultadoOperacion DesactivarPerfil(Guid idPerfil)
        {
            if (idPerfil == Guid.Empty)
                return ResultadoOperacion.Error("El ID del perfil no puede estar vacío");

            try
            {
                var perfil = _unitOfWork.Perfiles.GetById(idPerfil);
                if (perfil == null)
                    return ResultadoOperacion.Error("El perfil no existe");

                if (!perfil.Activo)
                    return ResultadoOperacion.Exitoso("El perfil ya estaba inactivo", idPerfil);

                if (TieneUsuariosActivos(idPerfil))
                    return ResultadoOperacion.Error("No se puede desactivar el perfil porque tiene usuarios activos asociados");

                perfil.Activo = false;
                _unitOfWork.Perfiles.Update(perfil);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Perfil desactivado correctamente", idPerfil);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al desactivar el perfil: {ObtenerMensajeProfundo(ex)}");
            }
        }

        private ResultadoOperacion ValidarPerfil(Perfil perfil, bool esNuevo)
        {
            if (perfil == null)
                return ResultadoOperacion.Error("El perfil es inválido");

            if (!esNuevo && perfil.IdPerfil == Guid.Empty)
                return ResultadoOperacion.Error("El ID del perfil no puede estar vacío");

            var nombre = perfil.NombrePerfil?.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                return ResultadoOperacion.Error("El nombre del perfil es obligatorio");

            if (nombre.Length > 50)
                return ResultadoOperacion.Error("El nombre del perfil no puede superar los 50 caracteres");

            var descripcion = perfil.Descripcion?.Trim();
            if (!string.IsNullOrEmpty(descripcion) && descripcion.Length > 200)
                return ResultadoOperacion.Error("La descripción no puede superar los 200 caracteres");

            var perfiles = _unitOfWork.Perfiles.GetAll();
            var duplicado = perfiles
                .FirstOrDefault(p => string.Equals(p.NombrePerfil, nombre, StringComparison.OrdinalIgnoreCase)
                                     && (!esNuevo && perfil.IdPerfil != Guid.Empty ? p.IdPerfil != perfil.IdPerfil : true));

            if (duplicado != null)
                return ResultadoOperacion.Error($"Ya existe un perfil con el nombre '{nombre}'");

            perfil.NombrePerfil = nombre;
            perfil.Descripcion = descripcion;

            return ResultadoOperacion.Exitoso("Perfil válido");
        }

        private bool TieneUsuariosActivos(Guid idPerfil)
        {
            var usuarios = _unitOfWork.Usuarios.GetUsuariosPorPerfil(idPerfil);
            return usuarios?.Any() ?? false;
        }

        private void SincronizarFunciones(Perfil perfilDestino, IEnumerable<Guid> funcionesSeleccionadas)
        {
            if (perfilDestino == null)
            {
                return;
            }

            if (perfilDestino.Funciones == null)
            {
                perfilDestino.Funciones = new List<Funcion>();
            }
            else
            {
                perfilDestino.Funciones.Clear();
            }

            if (funcionesSeleccionadas == null)
            {
                return;
            }

            var ids = funcionesSeleccionadas
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (!ids.Any())
            {
                return;
            }

            var funciones = _unitOfWork.Funciones.Find(f => ids.Contains(f.IdFuncion) && f.Activo);
            foreach (var funcion in funciones)
            {
                perfilDestino.Funciones.Add(funcion);
            }
        }

        private static string ObtenerMensajeProfundo(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex.Message;
        }
    }
}
