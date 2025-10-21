using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Fachada mínima para no romper llamadas existentes desde la UI.
    /// Delegamos todo en ApplicationServices (que usa ServiceFactory internamente).
    /// </summary>
    public static class SistemaInicializacionService
    {
        public static (bool exito, string mensaje) TestConexionBasico()
            => ApplicationServices.TestConexionBasico();

        public static (bool exito, string mensaje) TestConexionBD()
            => ApplicationServices.TestConexionBD();

        public static (bool exito, string mensaje) InicializarSistema()
            => ApplicationServices.InicializarSistema();

        public static (bool esValido, string mensaje) VerificarSistemaAutenticacion()
            => ApplicationServices.VerificarSistemaAutenticacion();

        public static (bool exito, string mensaje) TestLoginAdmin()
            => ApplicationServices.TestLoginAdmin();

        public static (bool esValido, string mensaje) VerificarConfiguracionDetallada()
            => ApplicationServices.VerificarConfiguracionDetallada();

        public static (bool exito, string mensaje) TestArquitecturaBase()
            => ApplicationServices.TestArquitecturaBase();

        public static string ObtenerInformacionConfiguracion()
            => ApplicationServices.ObtenerInformacionConfiguracion();
    }
}
