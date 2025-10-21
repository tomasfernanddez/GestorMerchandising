using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAutenticacionService
    {
        ResultadoAutenticacion Login(string nombreUsuario, string password, string direccionIP = null);
        Task<ResultadoAutenticacion> LoginAsync(string nombreUsuario, string password, string direccionIP = null);
        ResultadoOperacion Logout(Guid idUsuario, string direccionIP = null);
        ResultadoOperacion CambiarPassword(Guid idUsuario, string passwordActual, string passwordNuevo);
    }
}
