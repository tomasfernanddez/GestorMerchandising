using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BLL.Interfaces
{
    public interface IEncriptacionService
    {
        /// <summary>
        /// Genera un hash seguro a partir de una contraseña en texto plano.
        /// </summary>
        string GenerarHashPassword(string password);

        /// <summary>
        /// Verifica si una contraseña en texto plano coincide con el hash almacenado.
        /// </summary>
        bool VerificarPassword(string password, string hash);
    }
}
