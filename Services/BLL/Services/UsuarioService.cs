using Services.BLL.Helpers;
using Services.BLL.Interfaces;
using Services.DAL.Interfaces.Base;
using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.BLL.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEncriptacionService _encriptacionService;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de usuarios.
        /// </summary>
        public UsuarioService(IUnitOfWork unitOfWork, IEncriptacionService encriptacionService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _encriptacionService = encriptacionService ?? throw new ArgumentNullException(nameof(encriptacionService));
        }

        /// <summary>
        /// Crea un usuario y lo persiste con la contraseña indicada.
        /// </summary>
        public ResultadoOperacion CrearUsuario(Usuario usuario, string password)
        {
            try
            {
                var validacion = ValidarUsuario(usuario, password, esCreacion: true);
                if (!validacion.EsValido)
                    return validacion;

                // Verificar que no existe el nombre de usuario
                if (_unitOfWork.Usuarios.ExisteNombreUsuario(usuario.NombreUsuario))
                {
                    return ResultadoOperacion.Error($"Ya existe un usuario con el nombre '{usuario.NombreUsuario}'");
                }

                // Verificar que existe el perfil
                var perfil = _unitOfWork.Perfiles.GetById(usuario.IdPerfil);
                if (perfil == null || !perfil.Activo)
                {
                    return ResultadoOperacion.Error("El perfil seleccionado no existe o está inactivo");
                }

                // Configurar usuario
                usuario.IdUsuario = Guid.NewGuid();
                usuario.PasswordHash = _encriptacionService.GenerarHashPassword(password);
                usuario.FechaCreacion = DateTime.Now;
                usuario.Activo = true;
                usuario.Bloqueado = false;
                usuario.IntentosLoginFallidos = 0;

                // Asignar idioma por defecto si no está especificado
                if (string.IsNullOrWhiteSpace(usuario.IdiomaPreferido))
                {
                    usuario.IdiomaPreferido = "es-AR"; // Idioma por defecto
                }

                _unitOfWork.Usuarios.Add(usuario);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Usuario creado exitosamente", usuario.IdUsuario);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Crea un usuario de forma asíncrona reutilizando la lógica sincrónica.
        /// </summary>
        public async Task<ResultadoOperacion> CrearUsuarioAsync(Usuario usuario, string password)
        {
            // Implementación similar a la sincrónica
            return await Task.FromResult(CrearUsuario(usuario, password));
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente.
        /// </summary>
        public ResultadoOperacion ActualizarUsuario(Usuario usuario)
        {
            try
            {
                var validacion = ValidarUsuario(usuario, null, esCreacion: false);
                if (!validacion.EsValido)
                    return validacion;

                var usuarioExistente = _unitOfWork.Usuarios.GetById(usuario.IdUsuario);
                if (usuarioExistente == null)
                {
                    return ResultadoOperacion.Error("El usuario no existe");
                }

                // Verificar nombre de usuario único (excluyendo el actual)
                var usuarioConMismoNombre = _unitOfWork.Usuarios.GetUsuarioPorNombre(usuario.NombreUsuario);
                if (usuarioConMismoNombre != null && usuarioConMismoNombre.IdUsuario != usuario.IdUsuario)
                {
                    return ResultadoOperacion.Error($"Ya existe otro usuario con el nombre '{usuario.NombreUsuario}'");
                }

                // Verificar que existe el perfil
                var perfil = _unitOfWork.Perfiles.GetById(usuario.IdPerfil);
                if (perfil == null || !perfil.Activo)
                {
                    return ResultadoOperacion.Error("El perfil seleccionado no existe o está inactivo");
                }

                // Actualizar campos permitidos (no la contraseña)
                usuarioExistente.NombreUsuario = usuario.NombreUsuario;
                usuarioExistente.NombreCompleto = usuario.NombreCompleto;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.IdPerfil = usuario.IdPerfil;
                usuarioExistente.Activo = usuario.Activo;

                // Preservar idioma preferido si viene especificado
                if (!string.IsNullOrWhiteSpace(usuario.IdiomaPreferido))
                {
                    usuarioExistente.IdiomaPreferido = usuario.IdiomaPreferido;
                }

                _unitOfWork.Usuarios.Update(usuarioExistente);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Usuario actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Desactiva lógicamente a un usuario.
        /// </summary>
        public ResultadoOperacion EliminarUsuario(Guid idUsuario)
        {
            try
            {
                if (idUsuario == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del usuario no puede estar vacío");

                var usuario = _unitOfWork.Usuarios.GetById(idUsuario);
                if (usuario == null)
                    return ResultadoOperacion.Error("El usuario no existe");

                // No eliminar físicamente, solo desactivar
                usuario.Activo = false;
                _unitOfWork.Usuarios.Update(usuario);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Usuario desactivado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al eliminar el usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un usuario por su identificador único.
        /// </summary>
        public Usuario ObtenerPorId(Guid idUsuario)
        {
            if (idUsuario == Guid.Empty)
                throw new ArgumentException("El ID del usuario no puede estar vacío", nameof(idUsuario));

            return _unitOfWork.Usuarios.GetById(idUsuario);
        }

        /// <summary>
        /// Obtiene un usuario e incluye su perfil asociado.
        /// </summary>
        public Usuario ObtenerPorIdConPerfil(Guid idUsuario)
        {
            if (idUsuario == Guid.Empty)
                throw new ArgumentException("El ID del usuario no puede estar vacío", nameof(idUsuario));

            return _unitOfWork.Usuarios.ObtenerPorIdConPerfil(idUsuario);
        }

        /// <summary>
        /// Busca un usuario por su nombre de inicio de sesión.
        /// </summary>
        public Usuario ObtenerPorNombre(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(nombreUsuario));

            return _unitOfWork.Usuarios.GetUsuarioPorNombre(nombreUsuario);
        }

        /// <summary>
        /// Devuelve la lista de usuarios activos.
        /// </summary>
        public IEnumerable<Usuario> ObtenerUsuariosActivos()
        {
            return _unitOfWork.Usuarios.GetUsuariosActivos();
        }

        /// <summary>
        /// Recupera los usuarios asociados a un perfil específico.
        /// </summary>
        public IEnumerable<Usuario> ObtenerPorPerfil(Guid idPerfil)
        {
            return _unitOfWork.Usuarios.GetUsuariosPorPerfil(idPerfil);
        }

        /// <summary>
        /// Activa a un usuario que se encontraba deshabilitado.
        /// </summary>
        public ResultadoOperacion ActivarUsuario(Guid idUsuario)
        {
            try
            {
                if (idUsuario == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del usuario no puede estar vacío");

                var usuario = _unitOfWork.Usuarios.GetById(idUsuario);
                if (usuario == null)
                    return ResultadoOperacion.Error("El usuario no existe");

                if (usuario.Activo)
                    return ResultadoOperacion.Error("El usuario ya está activo");

                _unitOfWork.Usuarios.ActivarUsuario(idUsuario);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Usuario activado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al activar el usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Desactiva a un usuario para impedir su acceso.
        /// </summary>
        public ResultadoOperacion DesactivarUsuario(Guid idUsuario)
        {
            try
            {
                if (idUsuario == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del usuario no puede estar vacío");

                var usuario = _unitOfWork.Usuarios.GetById(idUsuario);
                if (usuario == null)
                    return ResultadoOperacion.Error("El usuario no existe");

                if (!usuario.Activo)
                    return ResultadoOperacion.Error("El usuario ya está inactivo");

                _unitOfWork.Usuarios.DesactivarUsuario(idUsuario);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Usuario desactivado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al desactivar el usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Bloquea temporalmente el acceso de un usuario.
        /// </summary>
        public ResultadoOperacion BloquearUsuario(Guid idUsuario)
        {
            try
            {
                if (idUsuario == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del usuario no puede estar vacío");

                var usuario = _unitOfWork.Usuarios.GetById(idUsuario);
                if (usuario == null)
                    return ResultadoOperacion.Error("El usuario no existe");

                if (usuario.Bloqueado)
                    return ResultadoOperacion.Error("El usuario ya está bloqueado");

                _unitOfWork.Usuarios.BloquearUsuario(idUsuario);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Usuario bloqueado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al bloquear el usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Quita el bloqueo aplicado a un usuario.
        /// </summary>
        public ResultadoOperacion DesbloquearUsuario(Guid idUsuario)
        {
            try
            {
                if (idUsuario == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del usuario no puede estar vacío");

                var usuario = _unitOfWork.Usuarios.GetById(idUsuario);
                if (usuario == null)
                    return ResultadoOperacion.Error("El usuario no existe");

                if (!usuario.Bloqueado)
                    return ResultadoOperacion.Error("El usuario no está bloqueado");

                _unitOfWork.Usuarios.DesbloquearUsuario(idUsuario);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Usuario desbloqueado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al desbloquear el usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario.
        /// </summary>
        public ResultadoOperacion CambiarPassword(Guid idUsuario, string passwordNuevo)
        {
            try
            {
                if (idUsuario == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del usuario no puede estar vacío");

                if (string.IsNullOrWhiteSpace(passwordNuevo))
                    return ResultadoOperacion.Error("La nueva contraseña no puede estar vacía");

                if (passwordNuevo.Length < 4)
                    return ResultadoOperacion.Error("La nueva contraseña debe tener al menos 4 caracteres");

                var usuario = _unitOfWork.Usuarios.GetById(idUsuario);
                if (usuario == null)
                    return ResultadoOperacion.Error("El usuario no existe");

                usuario.PasswordHash = _encriptacionService.GenerarHashPassword(passwordNuevo);
                _unitOfWork.Usuarios.Update(usuario);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Contraseña cambiada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al cambiar la contraseña: {ex.Message}");
            }
        }

        /// <summary>
        /// Ejecuta las validaciones de consistencia para un usuario.
        /// </summary>
        private ResultadoOperacion ValidarUsuario(Usuario usuario, string password, bool esCreacion)
        {
            if (usuario == null)
                return ResultadoOperacion.Error("El usuario no puede ser nulo");

            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                return ResultadoOperacion.Error("El nombre de usuario es obligatorio");

            if (usuario.NombreUsuario.Length < 3 || usuario.NombreUsuario.Length > 50)
                return ResultadoOperacion.Error("El nombre de usuario debe tener entre 3 y 50 caracteres");

            if (!string.IsNullOrWhiteSpace(usuario.NombreCompleto) && usuario.NombreCompleto.Length > 100)
                return ResultadoOperacion.Error("El nombre completo no puede superar los 100 caracteres");

            if (!string.IsNullOrWhiteSpace(usuario.Email))
            {
                if (usuario.Email.Length > 100)
                    return ResultadoOperacion.Error("El email no puede superar los 100 caracteres");

                if (!EsEmailValido(usuario.Email))
                    return ResultadoOperacion.Error("El formato del email no es válido");
            }

            if (esCreacion)
            {
                if (string.IsNullOrWhiteSpace(password))
                    return ResultadoOperacion.Error("La contraseña es obligatoria");

                if (password.Length < 4)
                    return ResultadoOperacion.Error("La contraseña debe tener al menos 4 caracteres");
            }

            if (usuario.IdPerfil == Guid.Empty)
                return ResultadoOperacion.Error("Debe seleccionar un perfil");

            return ResultadoOperacion.Exitoso("Validación exitosa");
        }

        /// <summary>
        /// Valida el formato de una dirección de correo.
        /// </summary>
        private bool EsEmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios sin filtrar por estado.
        /// </summary>
        public IEnumerable<Usuario> ObtenerTodosLosUsuarios()
        {
            return _unitOfWork.Usuarios.GetTodosLosUsuarios();
        }

        /// <summary>
        /// Obtiene de manera asíncrona todos los usuarios sin filtrar por estado.
        /// </summary>
        public async Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync()
        {
            return await _unitOfWork.Usuarios.GetTodosLosUsuariosAsync();
        }

        /// <summary>
        /// Actualiza el idioma preferido configurado para un usuario.
        /// </summary>
        public ResultadoOperacion CambiarIdioma(Guid idUsuario, string idioma)
        {
            try
            {
                if (idUsuario == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del usuario no puede estar vacío");

                if (string.IsNullOrWhiteSpace(idioma))
                    return ResultadoOperacion.Error("Debe especificar un idioma");

                var idiomasValidos = new[] { "es-AR", "en-US" };
                if (!idiomasValidos.Contains(idioma))
                    return ResultadoOperacion.Error("Idioma no válido");

                var usuario = _unitOfWork.Usuarios.GetById(idUsuario);
                if (usuario == null)
                    return ResultadoOperacion.Error("Usuario no encontrado");

                usuario.IdiomaPreferido = idioma;
                _unitOfWork.Usuarios.Update(usuario);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Idioma actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error cambiando idioma: {ex.Message}");
            }
        }
    }
}
