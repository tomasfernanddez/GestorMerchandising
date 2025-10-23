using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Entidades;

namespace BLL.Helpers
{
    public static class ProveedorCatalogoHelper
    {
        private static readonly string[] _condicionesPago =
        {
            "Contado",
            "30 días f/f",
            "60 días f/f",
            "90 días f/f"
        };

        public static IReadOnlyList<string> CondicionesPago => _condicionesPago;

        public static bool EsTipoPersonalizador(TipoProveedor tipoProveedor)
        {
            if (tipoProveedor == null)
                return false;

            return tipoProveedor.TipoProveedorNombre != null
                   && tipoProveedor.TipoProveedorNombre.IndexOf("personal", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool EsTipoPersonalizador(string tipoNombre)
        {
            if (string.IsNullOrWhiteSpace(tipoNombre))
                return false;

            return tipoNombre.IndexOf("personal", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool EsCondicionPagoValida(string condicion)
        {
            if (string.IsNullOrWhiteSpace(condicion))
                return false;

            return _condicionesPago.Any(cp => cp.Equals(condicion.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}