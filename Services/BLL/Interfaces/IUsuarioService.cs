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
        ResultadoOperacion CrearUsuario(Usuario usuario, string password);
        Task<ResultadoOperacion> CrearUsuarioAsync(Usuario usuario, string password);
        ResultadoOperacion ActualizarUsuario(Usuario usuario);
        ResultadoOperacion EliminarUsuario(Guid idUsuario);

        /* Consultas */
        Usuario ObtenerPorId(Guid idUsuario);
        Usuario ObtenerPorIdConPerfil(Guid idUsuario);
        Usuario ObtenerPorNombre(string nombreUsuario);
        IEnumerable<Usuario> ObtenerUsuariosActivos();
        IEnumerable<Usuario> ObtenerPorPerfil(Guid idPerfil);

        /* Gestión de estado */
        ResultadoOperacion ActivarUsuario(Guid idUsuario);
        ResultadoOperacion DesactivarUsuario(Guid idUsuario);
        ResultadoOperacion BloquearUsuario(Guid idUsuario);
        ResultadoOperacion DesbloquearUsuario(Guid idUsuario);

        ResultadoOperacion CambiarPassword(Guid idUsuario, string passwordNuevo);

        IEnumerable<Usuario> ObtenerTodosLosUsuarios();
        Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync();
        ResultadoOperacion CambiarIdioma(Guid idUsuario, string idioma);
    }
}
