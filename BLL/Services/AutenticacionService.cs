using BLL.Helpers;
using BLL.Interfaces;
using DAL.Interfaces.Base;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEncriptacionService _encriptacionService;

        public AutenticacionService(IUnitOfWork unitOfWork, IEncriptacionService encriptacionService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _encriptacionService = encriptacionService ?? throw new ArgumentNullException(nameof(encriptacionService));
        }

        public ResultadoAutenticacion Login(string nombreUsuario, string password, string direccionIP = null)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(nombreUsuario))
                    return ResultadoAutenticacion.Error("El nombre de usuario es obligatorio");

                if (string.IsNullOrWhiteSpace(password))
                    return ResultadoAutenticacion.Error("La contraseña es obligatoria");

                // Buscar usuario
                var usuario = _unitOfWork.Usuarios.GetUsuarioPorNombre(nombreUsuario);
                if (usuario == null)
                {
                    _unitOfWork.Bitacoras.RegistrarAccion(
                        Guid.Empty,
                        "Login Fallido",
                        $"Intento de login con usuario inexistente: {nombreUsuario}",
                        "Seguridad",
                        false,
                        "Usuario no encontrado",
                        direccionIP
                    );
                    _unitOfWork.SaveChanges();
                    return ResultadoAutenticacion.Error("Usuario o contraseña incorrectos");
                }

                // Estado del usuario
                if (usuario.Bloqueado)
                {
                    _unitOfWork.Bitacoras.RegistrarAccion(
                        usuario.IdUsuario,
                        "Login Bloqueado",
                        $"Intento de login con usuario bloqueado: {nombreUsuario}",
                        "Seguridad",
                        false,
                        "Usuario bloqueado",
                        direccionIP
                    );
                    _unitOfWork.SaveChanges();
                    return ResultadoAutenticacion.Error("Usuario bloqueado. Contacte al administrador.");
                }

                if (!usuario.Activo)
                {
                    _unitOfWork.Bitacoras.RegistrarAccion(
                        usuario.IdUsuario,
                        "Login Inactivo",
                        $"Intento de login con usuario inactivo: {nombreUsuario}",
                        "Seguridad",
                        false,
                        "Usuario inactivo",
                        direccionIP
                    );
                    _unitOfWork.SaveChanges();
                    return ResultadoAutenticacion.Error("Usuario inactivo. Contacte al administrador.");
                }

                // --- Verificación de contraseña ---
                // 1) Intento "oficial" con el servicio actual
                bool ok = _encriptacionService.VerificarPassword(password, usuario.PasswordHash);

                // 2) Fallback de compatibilidad (seed antiguo):
                //    a) SHA-256 HEX (64 chars, ej.: 8c6976e5... para "admin")
                //    b) (opcional) SHA-256 Base64 (44 chars)
                if (!ok)
                {
                    var shaHex = Sha256Hex(password);
                    if (string.Equals(usuario.PasswordHash, shaHex, StringComparison.OrdinalIgnoreCase))
                    {
                        ok = true;
                        // Auto-upgrade al formato oficial
                        usuario.PasswordHash = _encriptacionService.GenerarHashPassword(password);
                        _unitOfWork.Usuarios.Update(usuario);
                        _unitOfWork.Bitacoras.RegistrarAccion(
                            usuario.IdUsuario,
                            "Upgrade PasswordHash",
                            $"Hash migrado a formato oficial (fallback HEX)",
                            "Seguridad",
                            true,
                            null,
                            direccionIP
                        );
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        var shaB64 = Sha256Base64(password);
                        if (string.Equals(usuario.PasswordHash, shaB64, StringComparison.Ordinal))
                        {
                            ok = true;
                            // Auto-upgrade al formato oficial
                            usuario.PasswordHash = _encriptacionService.GenerarHashPassword(password);
                            _unitOfWork.Usuarios.Update(usuario);
                            _unitOfWork.Bitacoras.RegistrarAccion(
                                usuario.IdUsuario,
                                "Upgrade PasswordHash",
                                $"Hash migrado a formato oficial (fallback Base64)",
                                "Seguridad",
                                true,
                                null,
                                direccionIP
                            );
                            _unitOfWork.SaveChanges();
                        }
                    }
                }

                if (!ok)
                {
                    _unitOfWork.Usuarios.RegistrarIntentoFallido(usuario.IdUsuario);
                    _unitOfWork.Bitacoras.RegistrarAccion(
                        usuario.IdUsuario,
                        "Login Fallido",
                        $"Contraseña incorrecta para usuario: {nombreUsuario}",
                        "Seguridad",
                        false,
                        "Contraseña incorrecta",
                        direccionIP
                    );
                    _unitOfWork.SaveChanges();
                    return ResultadoAutenticacion.Error("Usuario o contraseña incorrectos");
                }

                // Login exitoso
                _unitOfWork.Usuarios.RegistrarAccesoExitoso(usuario.IdUsuario);
                _unitOfWork.Bitacoras.RegistrarAccion(
                    usuario.IdUsuario,
                    "Login Exitoso",
                    $"Usuario {nombreUsuario} ingresó al sistema",
                    "Seguridad",
                    true,
                    null,
                    direccionIP
                );
                _unitOfWork.SaveChanges();

                return ResultadoAutenticacion.Exitoso(usuario, "Login exitoso");
            }
            catch (Exception ex)
            {
                return ResultadoAutenticacion.Error($"Error durante la autenticación: {ex.Message}");
            }
        }

        public async Task<ResultadoAutenticacion> LoginAsync(string nombreUsuario, string password, string direccionIP = null)
        {
            // Implementación asincrónica simple; si tu DAL soporta async, podés migrar las llamadas internas.
            return await Task.FromResult(Login(nombreUsuario, password, direccionIP));
        }

        public ResultadoOperacion Logout(Guid idUsuario, string direccionIP = null)
        {
            try
            {
                var usuario = _unitOfWork.Usuarios.GetById(idUsuario);
                if (usuario == null)
                    return ResultadoOperacion.Error("Usuario no encontrado");

                _unitOfWork.Bitacoras.RegistrarAccion(
                    idUsuario,
                    "Logout",
                    $"Usuario {usuario.NombreUsuario} cerró sesión",
                    "Seguridad",
                    true,
                    null,
                    direccionIP
                );
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Sesión cerrada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error cerrando sesión: {ex.Message}");
            }
        }

        public ResultadoOperacion CambiarPassword(Guid idUsuario, string passwordActual, string passwordNuevo)
        {
            try
            {
                var usuario = _unitOfWork.Usuarios.GetById(idUsuario);
                if (usuario == null)
                    return ResultadoOperacion.Error("Usuario no encontrado");

                if (!_encriptacionService.VerificarPassword(passwordActual, usuario.PasswordHash))
                    return ResultadoOperacion.Error("La contraseña actual es incorrecta");

                if (string.IsNullOrWhiteSpace(passwordNuevo))
                    return ResultadoOperacion.Error("La nueva contraseña no puede estar vacía");

                if (passwordNuevo.Length < 4)
                    return ResultadoOperacion.Error("La nueva contraseña debe tener al menos 4 caracteres");

                usuario.PasswordHash = _encriptacionService.GenerarHashPassword(passwordNuevo);
                _unitOfWork.Usuarios.Update(usuario);

                _unitOfWork.Bitacoras.RegistrarAccion(
                    idUsuario,
                    "Cambio Password",
                    $"Usuario {usuario.NombreUsuario} cambió su contraseña",
                    "Seguridad",
                    true,
                    null,
                    null
                );
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Contraseña cambiada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error cambiando contraseña: {ex.Message}");
            }
        }

        // ===== Helpers privados para compatibilidad de hash =====
        private static string Sha256Hex(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private static string Sha256Base64(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}