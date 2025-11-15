using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DAL.Interfaces.Base
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        // Métodos específicos para Usuario
        /// <summary>
        /// Busca un usuario por su nombre de inicio de sesión.
        /// </summary>
        Usuario GetUsuarioPorNombre(string nombreUsuario);

        /// <summary>
        /// Busca de forma asíncrona un usuario por su nombre de inicio de sesión.
        /// </summary>
        Task<Usuario> GetUsuarioPorNombreAsync(string nombreUsuario);

        /// <summary>
        /// Obtiene los usuarios que se encuentran activos y no bloqueados.
        /// </summary>
        IEnumerable<Usuario> GetUsuariosActivos();

        /// <summary>
        /// Obtiene de forma asíncrona los usuarios que se encuentran activos y no bloqueados.
        /// </summary>
        Task<IEnumerable<Usuario>> GetUsuariosActivosAsync();

        /// <summary>
        /// Obtiene los usuarios asociados a un perfil específico.
        /// </summary>
        IEnumerable<Usuario> GetUsuariosPorPerfil(Guid idPerfil);

        /// <summary>
        /// Obtiene de forma asíncrona los usuarios asociados a un perfil específico.
        /// </summary>
        Task<IEnumerable<Usuario>> GetUsuariosPorPerfilAsync(Guid idPerfil);

        /// <summary>
        /// Obtiene un usuario incluyendo la información de su perfil.
        /// </summary>
        Usuario ObtenerPorIdConPerfil(Guid idUsuario);

        /// <summary>
        /// Verifica si existe un usuario con el nombre indicado.
        /// </summary>
        bool ExisteNombreUsuario(string nombreUsuario);

        /// <summary>
        /// Verifica de forma asíncrona si existe un usuario con el nombre indicado.
        /// </summary>
        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);

        /// <summary>
        /// Bloquea un usuario debido a intentos fallidos u otra condición.
        /// </summary>
        void BloquearUsuario(Guid idUsuario);

        /// <summary>
        /// Quita el bloqueo aplicado al usuario.
        /// </summary>
        void DesbloquearUsuario(Guid idUsuario);

        /// <summary>
        /// Incrementa el contador de intentos de acceso fallidos.
        /// </summary>
        void RegistrarIntentoFallido(Guid idUsuario);

        /// <summary>
        /// Registra un acceso exitoso restableciendo los contadores.
        /// </summary>
        void RegistrarAccesoExitoso(Guid idUsuario);

        /// <summary>
        /// Activa un usuario deshabilitado.
        /// </summary>
        void ActivarUsuario(Guid idUsuario);

        /// <summary>
        /// Desactiva un usuario de manera lógica.
        /// </summary>
        void DesactivarUsuario(Guid idUsuario);

        /// <summary>
        /// Obtiene todos los usuarios registrados.
        /// </summary>
        IEnumerable<Usuario> GetTodosLosUsuarios();

        /// <summary>
        /// Obtiene de forma asíncrona todos los usuarios registrados.
        /// </summary>
        Task<IEnumerable<Usuario>> GetTodosLosUsuariosAsync();

        /// <summary>
        /// Agrega un nuevo usuario al repositorio.
        /// </summary>
        new void Add(Usuario usuario);
    }
}