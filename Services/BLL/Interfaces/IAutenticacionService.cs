using Services.BLL.Helpers;
using System;
using System.Threading.Tasks;

namespace Services.BLL.Interfaces
{
    public interface IAutenticacionService
    {
        ResultadoAutenticacion Login(string nombreUsuario, string password, string direccionIP = null);
        Task<ResultadoAutenticacion> LoginAsync(string nombreUsuario, string password, string direccionIP = null);
        ResultadoOperacion Logout(Guid idUsuario, string direccionIP = null);
        ResultadoOperacion CambiarPassword(Guid idUsuario, string passwordActual, string passwordNuevo);
    }
}
