using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations.Base
{
    public class EfUsuarioRepository : EfRepository<Usuario>, IUsuarioRepository
    {
        public EfUsuarioRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario
        /// </summary>
        public Usuario GetUsuarioPorNombre(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return null;

            return _dbSet.Include(u => u.Perfil)
                        .FirstOrDefault(u => u.NombreUsuario == nombreUsuario.Trim());
        }

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario (async)
        /// </summary>
        public async Task<Usuario> GetUsuarioPorNombreAsync(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return null;

            return await _dbSet.Include(u => u.Perfil)
                              .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario.Trim());
        }

        /// <summary>
        /// Obtiene todos los usuarios activos
        /// </summary>
        public IEnumerable<Usuario> GetUsuariosActivos()
        {
            return _dbSet.Where(u => u.Activo == true && u.Bloqueado == false)
                        .Include(u => u.Perfil)
                        .OrderBy(u => u.NombreUsuario)
                        .ToList();
        }

        /// <summary>
        /// Obtiene todos los usuarios activos (async)
        /// </summary>
        public async Task<IEnumerable<Usuario>> GetUsuariosActivosAsync()
        {
            return await _dbSet.Where(u => u.Activo == true && u.Bloqueado == false)
                              .Include(u => u.Perfil)
                              .OrderBy(u => u.NombreUsuario)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene usuarios por perfil
        /// </summary>
        public IEnumerable<Usuario> GetUsuariosPorPerfil(Guid idPerfil)
        {
            return _dbSet.Where(u => u.IdPerfil == idPerfil && u.Activo == true)
                        .Include(u => u.Perfil)
                        .OrderBy(u => u.NombreUsuario)
                        .ToList();
        }

        /// <summary>
        /// Obtiene usuarios por perfil (async)
        /// </summary>
        public async Task<IEnumerable<Usuario>> GetUsuariosPorPerfilAsync(Guid idPerfil)
        {
            return await _dbSet.Where(u => u.IdPerfil == idPerfil && u.Activo == true)
                              .Include(u => u.Perfil)
                              .OrderBy(u => u.NombreUsuario)
                              .ToListAsync();
        }

        /// <summary>
        /// Verifica si existe un nombre de usuario
        /// </summary>
        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return false;

            return _dbSet.Any(u => u.NombreUsuario == nombreUsuario.Trim());
        }

        /// <summary>
        /// Verifica si existe un nombre de usuario (async)
        /// </summary>
        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return false;

            return await _dbSet.AnyAsync(u => u.NombreUsuario == nombreUsuario.Trim());
        }

        /// <summary>
        /// Bloquea un usuario por intentos fallidos
        /// </summary>
        public void BloquearUsuario(Guid idUsuario)
        {
            var usuario = GetById(idUsuario);
            if (usuario != null)
            {
                usuario.Bloqueado = true;
                usuario.FechaBloqueo = DateTime.Now;
                Update(usuario);
            }
        }

        /// <summary>
        /// Desbloquea un usuario
        /// </summary>
        public void DesbloquearUsuario(Guid idUsuario)
        {
            var usuario = GetById(idUsuario);
            if (usuario != null)
            {
                usuario.Bloqueado = false;
                usuario.FechaBloqueo = null;
                usuario.IntentosLoginFallidos = 0;
                Update(usuario);
            }
        }

        /// <summary>
        /// Registra un intento de login fallido
        /// </summary>
        public void RegistrarIntentoFallido(Guid idUsuario)
        {
            var usuario = GetById(idUsuario);
            if (usuario != null)
            {
                usuario.IntentosLoginFallidos++;

                // Bloquear después de 3 intentos fallidos (configurable)
                if (usuario.IntentosLoginFallidos >= 3)
                {
                    usuario.Bloqueado = true;
                    usuario.FechaBloqueo = DateTime.Now;
                }

                Update(usuario);
            }
        }

        /// <summary>
        /// Registra un acceso exitoso
        /// </summary>
        public void RegistrarAccesoExitoso(Guid idUsuario)
        {
            var usuario = GetById(idUsuario);
            if (usuario != null)
            {
                usuario.FechaUltimoAcceso = DateTime.Now;
                usuario.IntentosLoginFallidos = 0; // Resetear intentos fallidos
                Update(usuario);
            }
        }

        /// <summary>
        /// Activa un usuario
        /// </summary>
        public void ActivarUsuario(Guid idUsuario)
        {
            var usuario = GetById(idUsuario);
            if (usuario != null)
            {
                usuario.Activo = true;
                Update(usuario);
            }
        }

        /// <summary>
        /// Desactiva un usuario (eliminación lógica)
        /// </summary>
        public void DesactivarUsuario(Guid idUsuario)
        {
            var usuario = GetById(idUsuario);
            if (usuario != null)
            {
                usuario.Activo = false;
                Update(usuario);
            }
        }
        public IEnumerable<Usuario> GetTodosLosUsuarios()
        {
            return _dbSet.Include(u => u.Perfil)
                .OrderBy(u => u.NombreUsuario)
                .ToList();
        }

        public async Task<IEnumerable<Usuario>> GetTodosLosUsuariosAsync()
        {
            return await _dbSet.Include(u => u.Perfil)
                .OrderBy(u => u.NombreUsuario)
                .ToListAsync();
        }
    }
}
