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
        /// <summary>
        /// Determina si el tipo personalizador.
        /// </summary>

        public static bool EsTipoPersonalizador(TipoProveedor tipoProveedor)
        {
            if (tipoProveedor == null)
                return false;

            return tipoProveedor.TipoProveedorNombre != null
                   && tipoProveedor.TipoProveedorNombre.IndexOf("personal", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        /// <summary>
        /// Determina si el tipo personalizador.
        /// </summary>

        public static bool EsTipoPersonalizador(string tipoNombre)
        {
            if (string.IsNullOrWhiteSpace(tipoNombre))
                return false;

            return tipoNombre.IndexOf("personal", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        /// <summary>
        /// Determina si el tipo producto.
        /// </summary>

        public static bool EsTipoProducto(TipoProveedor tipoProveedor)
        {
            if (tipoProveedor == null)
                return false;

            return EsTipoProducto(tipoProveedor.TipoProveedorNombre);
        }
        /// <summary>
        /// Determina si el tipo producto.
        /// </summary>

        public static bool EsTipoProducto(string tipoNombre)
        {
            if (string.IsNullOrWhiteSpace(tipoNombre))
                return false;

            return tipoNombre.IndexOf("producto", StringComparison.OrdinalIgnoreCase) >= 0
                   || tipoNombre.IndexOf("product", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        /// <summary>
        /// Determina si el condicion pago valida.
        /// </summary>

        public static bool EsCondicionPagoValida(string condicion)
        {
            if (string.IsNullOrWhiteSpace(condicion))
                return false;

            return _condicionesPago.Any(cp => cp.Equals(condicion.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}