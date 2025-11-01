using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DomainModel.Entidades;

namespace UI.Helpers
{
    public static class PedidoEstadoHelper
    {
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

        private static bool Contiene(string texto, string keyword)
        {
            var compare = CultureInfo.InvariantCulture.CompareInfo;
            return compare.IndexOf(texto ?? string.Empty, keyword, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }
    }
}