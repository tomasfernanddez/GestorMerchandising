using System;

namespace UI.Helpers
{
    /// <summary>
    /// Provee utilidades de presentación para combinar nombres con alias.
    /// </summary>
    public static class DisplayNameHelper
    {
        /// <summary>
        /// Combina un nombre y su alias mostrando el alias entre paréntesis cuando está disponible.
        /// </summary>
        /// <param name="nombre">Nombre principal que se mostrará.</param>
        /// <param name="alias">Alias opcional asociado al nombre.</param>
        /// <returns>Nombre con alias entre paréntesis cuando corresponde, o solo el nombre si no hay alias.</returns>
        public static string FormatearNombreConAlias(string nombre, string alias)
        {
            var nombreLimpio = nombre?.Trim();
            var aliasLimpio = alias?.Trim();

            if (string.IsNullOrEmpty(nombreLimpio))
                return aliasLimpio ?? string.Empty;

            return string.IsNullOrEmpty(aliasLimpio)
                ? nombreLimpio
                : string.Format("{0} ({1})", nombreLimpio, aliasLimpio);
        }
    }
}