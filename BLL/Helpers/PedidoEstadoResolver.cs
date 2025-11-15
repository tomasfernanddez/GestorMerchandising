using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DomainModel.Entidades;

namespace BLL.Helpers
{
    /// <summary>
    /// Calcula estado.
    /// </summary>
    public static class PedidoEstadoResolver
    {
        public static EstadoCalculado CalcularEstado(IEnumerable<string> estadosDetalle, IEnumerable<EstadoPedido> estadosPedido)
        {
            var listaEstados = NormalizarEstados(estadosDetalle);
            var catalogo = estadosPedido?.ToList() ?? new List<EstadoPedido>();

            if (listaEstados.Count == 0)
            {
                return CrearResultado(BuscarEstado(catalogo, "produ"));
            }

            if (listaEstados.All(s => Contiene(s, "cancel")))
            {
                return CrearResultado(BuscarEstado(catalogo, "cancel"));
            }

            if (listaEstados.Any(s => Contiene(s, "repro")))
            {
                return CrearResultado(BuscarEstado(catalogo, "repro"));
            }

            if (listaEstados.All(s => Contiene(s, "entreg")))
            {
                var finalizado = BuscarEstado(catalogo, "final");
                if (finalizado != null)
                    return CrearResultado(finalizado);

                finalizado = BuscarEstado(catalogo, "entreg");
                if (finalizado != null)
                    return CrearResultado(finalizado);

                finalizado = BuscarEstado(catalogo, "cerr");
                if (finalizado != null)
                    return CrearResultado(finalizado);
            }

            var produccion = BuscarEstado(catalogo, "produ");
            if (produccion != null)
                return CrearResultado(produccion);

            return CrearResultado(catalogo.FirstOrDefault());
        }

        /// <summary>
        /// Normaliza estados.
        /// </summary>
        private static List<string> NormalizarEstados(IEnumerable<string> estados)
        {
            return estados?
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(Normalizar)
                .ToList()
                ?? new List<string>();
        }

        /// <summary>
        /// Normaliza.
        /// </summary>
        private static string Normalizar(string texto)
        {
            var formD = texto.Normalize(NormalizationForm.FormD);
            var chars = formD.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            var limpio = new string(chars.ToArray());
            return limpio.Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }

        /// <summary>
        /// Determina si contiene.
        /// </summary>
        private static bool Contiene(string texto, string keyword)
        {
            var compare = CultureInfo.InvariantCulture.CompareInfo;
            return compare.IndexOf(texto ?? string.Empty, keyword, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }

        /// <summary>
        /// Busca estado.
        /// </summary>
        private static EstadoPedido BuscarEstado(IEnumerable<EstadoPedido> estados, string keyword)
        {
            if (estados == null)
                return null;

            var compare = CultureInfo.InvariantCulture.CompareInfo;
            foreach (var estado in estados)
            {
                if (estado == null)
                    continue;

                var nombre = estado.NombreEstadoPedido ?? string.Empty;
                if (compare.IndexOf(nombre, keyword, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0)
                {
                    return estado;
                }
            }

            return null;
        }

        /// <summary>
        /// Crea resultado.
        /// </summary>
        private static EstadoCalculado CrearResultado(EstadoPedido estado)
        {
            return new EstadoCalculado(estado?.IdEstadoPedido, estado?.NombreEstadoPedido);
        }
    }
}