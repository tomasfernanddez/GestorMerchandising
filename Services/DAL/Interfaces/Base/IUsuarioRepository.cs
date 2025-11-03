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
        Usuario GetUsuarioPorNombre(string nombreUsuario);
        Task<Usuario> GetUsuarioPorNombreAsync(string nombreUsuario);

        IEnumerable<Usuario> GetUsuariosActivos();
        Task<IEnumerable<Usuario>> GetUsuariosActivosAsync();

        IEnumerable<Usuario> GetUsuariosPorPerfil(Guid idPerfil);
        Task<IEnumerable<Usuario>> GetUsuariosPorPerfilAsync(Guid idPerfil);

        Usuario ObtenerPorIdConPerfil(Guid idUsuario);

        bool ExisteNombreUsuario(string nombreUsuario);
        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);

        void BloquearUsuario(Guid idUsuario);
        void DesbloquearUsuario(Guid idUsuario);

        void RegistrarIntentoFallido(Guid idUsuario);
        void RegistrarAccesoExitoso(Guid idUsuario);

        void ActivarUsuario(Guid idUsuario);
        void DesactivarUsuario(Guid idUsuario);

        IEnumerable<Usuario> GetTodosLosUsuarios();
        Task<IEnumerable<Usuario>> GetTodosLosUsuariosAsync();
        new void Add(Usuario usuario);
    }
}
