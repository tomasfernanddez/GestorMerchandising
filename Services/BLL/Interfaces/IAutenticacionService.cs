using Services.BLL.Helpers;
using System;
using System.Threading.Tasks;

namespace Services.BLL.Interfaces
{
    public interface IAutenticacionService
    {
        /// <summary>
        /// Autentica a un usuario utilizando su contraseña.
        /// </summary>
        ResultadoAutenticacion Login(string nombreUsuario, string password, string direccionIP = null);

        /// <summary>
        /// Autentica a un usuario de forma asíncrona utilizando su contraseña.
        /// </summary>
        Task<ResultadoAutenticacion> LoginAsync(string nombreUsuario, string password, string direccionIP = null);

        /// <summary>
        /// Finaliza la sesión del usuario indicado.
        /// </summary>
        ResultadoOperacion Logout(Guid idUsuario, string direccionIP = null);

        /// <summary>
        /// Cambia la contraseña de un usuario validando previamente la actual.
        /// </summary>
        ResultadoOperacion CambiarPassword(Guid idUsuario, string passwordActual, string passwordNuevo);
    }
}
