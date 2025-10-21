using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ISistemaInicializacionService
    {
        /* Métodos de inicialización */
        (bool exito, string mensaje) InicializarSistema();
        (bool esValido, string mensaje) VerificarSistemaAutenticacion();
        (bool exito, string mensaje) TestLoginAdmin();

        /* Métodos de verificación */
        (bool exito, string mensaje) TestConexionBasico();
        (bool exito, string mensaje) TestConexionBD();
        (bool esValido, string mensaje) VerificarConfiguracionDetallada();
        (bool exito, string mensaje) TestArquitecturaBase();

        /* Información del sistema */
        string ObtenerInformacionConfiguracion();
        bool VerificarConfiguracion();
    }
}
