using Services.BLL.Helpers;
using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.BLL.Interfaces
{
    public interface IUsuarioService
    {
        /* Métodos CRUD básicos */
        /// <summary>
        /// Crea un nuevo usuario junto con su contraseña inicial.
        /// </summary>
        ResultadoOperacion CrearUsuario(Usuario usuario, string password);

        /// <summary>
        /// Crea un nuevo usuario de manera asíncrona junto con su contraseña inicial.
        /// </summary>
        Task<ResultadoOperacion> CrearUsuarioAsync(Usuario usuario, string password);

        /// <summary>
        /// Actualiza los datos de un usuario existente.
        /// </summary>
        ResultadoOperacion ActualizarUsuario(Usuario usuario);

        /// <summary>
        /// Elimina lógicamente un usuario identificado por su ID.
        /// </summary>
        ResultadoOperacion EliminarUsuario(Guid idUsuario);

        /* Consultas */
        /// <summary>
        /// Obtiene un usuario por su identificador.
        /// </summary>
        Usuario ObtenerPorId(Guid idUsuario);

        /// <summary>
        /// Obtiene un usuario e incluye la información de su perfil asociado.
        /// </summary>
        Usuario ObtenerPorIdConPerfil(Guid idUsuario);

        /// <summary>
        /// Busca un usuario por su nombre de inicio de sesión.
        /// </summary>
        Usuario ObtenerPorNombre(string nombreUsuario);

        /// <summary>
        /// Devuelve la lista de usuarios que se encuentran activos.
        /// </summary>
        IEnumerable<Usuario> ObtenerUsuariosActivos();

        /// <summary>
        /// Obtiene los usuarios asociados a un perfil específico.
        /// </summary>
        IEnumerable<Usuario> ObtenerPorPerfil(Guid idPerfil);

        /* Gestión de estado */
        /// <summary>
        /// Activa un usuario previamente desactivado.
        /// </summary>
        ResultadoOperacion ActivarUsuario(Guid idUsuario);

        /// <summary>
        /// Desactiva un usuario para impedir su acceso.
        /// </summary>
        ResultadoOperacion DesactivarUsuario(Guid idUsuario);

        /// <summary>
        /// Bloquea un usuario para impedir temporalmente su acceso.
        /// </summary>
        ResultadoOperacion BloquearUsuario(Guid idUsuario);

        /// <summary>
        /// Quita el bloqueo de un usuario.
        /// </summary>
        ResultadoOperacion DesbloquearUsuario(Guid idUsuario);

        /// <summary>
        /// Cambia la contraseña de un usuario.
        /// </summary>
        ResultadoOperacion CambiarPassword(Guid idUsuario, string passwordNuevo);

        /// <summary>
        /// Obtiene todos los usuarios registrados.
        /// </summary>
        IEnumerable<Usuario> ObtenerTodosLosUsuarios();

        /// <summary>
        /// Obtiene de manera asíncrona todos los usuarios registrados.
        /// </summary>
        Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync();

        /// <summary>
        /// Actualiza el idioma preferido de un usuario.
        /// </summary>
        ResultadoOperacion CambiarIdioma(Guid idUsuario, string idioma);
    }
}