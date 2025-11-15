using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DomainModel.Entidades;

namespace UI.Helpers
{
    /// <summary>
    /// Ofrece lógica para determinar estados de pedidos a partir del detalle de productos.
    /// </summary>
    public static class PedidoEstadoHelper
    {
        /// <summary>
        /// Calcula el estado del pedido en base a los estados de sus detalles.
        /// </summary>
        /// <param name="estadosDetalle">Colección de estados provenientes de los detalles.</param>
        /// <param name="estadosPedido">Catálogo de estados disponibles.</param>
        /// <returns>Identificador del estado sugerido para el pedido.</returns>
        public static Guid? CalcularEstadoPedido(IEnumerable<string> estadosDetalle, IEnumerable<EstadoPedido> estadosPedido)
        {
            if (estadosPedido == null)
                return null;

            var lista = estadosDetalle?
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToList() ?? new List<string>();

            if (lista.Count == 0)
            {
                return BuscarEstado(estadosPedido, "pend")
                       ?? BuscarEstado(estadosPedido, "nuevo")
                       ?? estadosPedido.FirstOrDefault()?.IdEstadoPedido;
            }

            if (lista.All(s => Contiene(s, "cancel")))
            {
                return BuscarEstado(estadosPedido, "cancel")
                       ?? estadosPedido.FirstOrDefault(e => Contiene(e?.NombreEstadoPedido, "cancel"))?.IdEstadoPedido;
            }

            if (lista.All(s => Contiene(s, "entreg") || Contiene(s, "final") || Contiene(s, "complet") || Contiene(s, "cerr")))
            {
                return BuscarEstado(estadosPedido, "entreg")
                       ?? BuscarEstado(estadosPedido, "final")
                       ?? BuscarEstado(estadosPedido, "complet")
                       ?? BuscarEstado(estadosPedido, "cerr");
            }

            if (lista.Any(s => Contiene(s, "produ")))
            {
                return BuscarEstado(estadosPedido, "produ");
            }

            if (lista.Any(s => Contiene(s, "env") || Contiene(s, "despach") || Contiene(s, "log")))
            {
                return BuscarEstado(estadosPedido, "env")
                       ?? BuscarEstado(estadosPedido, "despach");
            }

            return BuscarEstado(estadosPedido, "pend")
                   ?? estadosPedido.FirstOrDefault()?.IdEstadoPedido;
        }

        /// <summary>
        /// Busca un estado cuyo nombre contenga la palabra clave indicada.
        /// </summary>
        /// <param name="estados">Estados disponibles.</param>
        /// <param name="keyword">Palabra clave para comparar.</param>
        /// <returns>Identificador del estado encontrado o <c>null</c>.</returns>
        private static Guid? BuscarEstado(IEnumerable<EstadoPedido> estados, string keyword)
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
                    return estado.IdEstadoPedido;
                }
            }

            return null;
        }

        /// <summary>
        /// Determina si el texto contiene la palabra clave ignorando acentos y mayúsculas.
        /// </summary>
        /// <param name="texto">Texto que se analiza.</param>
        /// <param name="keyword">Palabra clave a buscar.</param>
        /// <returns>Verdadero cuando se encuentra la palabra clave.</returns>
        private static bool Contiene(string texto, string keyword)
        {
            var compare = CultureInfo.InvariantCulture.CompareInfo;
            return compare.IndexOf(texto ?? string.Empty, keyword, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }
    }
}