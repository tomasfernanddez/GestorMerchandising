using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Localization
{
    public static class LocalizationExtensions
    {
        /// <summary>
        /// Traduce una clave. Si la clave no existe, devuelve el mismo texto.
        /// Uso: "login.title".Traducir()
        /// </summary>
        public static string Traducir(this string key, params object[] args)
            => Localization.T(key, args);
    }
}
