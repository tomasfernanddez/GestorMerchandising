using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DomainModel.Entidades;

namespace UI.Helpers
{
    public static class PedidoMuestraEstadoHelper
    {
        public static Guid? CalcularEstadoPedido(IEnumerable<string> estadosDetalle, IEnumerable<EstadoPedidoMuestra> estadosPedido)
        {
            if (estadosPedido == null)
                return null;

            var listaEstados = estadosDetalle?
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToList() ?? new List<string>();

            if (listaEstados.Count == 0)
            {
                return BuscarEstado(estadosPedido, "solic")
                       ?? BuscarEstado(estadosPedido, "prep")
                       ?? estadosPedido.FirstOrDefault()?.IdEstadoPedidoMuestra;
            }

            if (listaEstados.All(s => Contiene(s, "devuel") || Contiene(s, "perd")))
            {
                return BuscarEstado(estadosPedido, "cerr")
                       ?? BuscarEstado(estadosPedido, "entreg")
                       ?? estadosPedido.LastOrDefault()?.IdEstadoPedidoMuestra;
            }

            if (listaEstados.Any(s => Contiene(s, "evalu")))
            {
                return BuscarEstado(estadosPedido, "entreg")
                       ?? BuscarEstado(estadosPedido, "cerr")
                       ?? BuscarEstado(estadosPedido, "despach");
            }

            if (listaEstados.Any(s => Contiene(s, "transit") || Contiene(s, "tránsit")))
            {
                return BuscarEstado(estadosPedido, "despach")
                       ?? BuscarEstado(estadosPedido, "prepar");
            }

            if (listaEstados.Any(s => Contiene(s, "pend")))
            {
                return BuscarEstado(estadosPedido, "prepar")
                       ?? BuscarEstado(estadosPedido, "solic");
            }

            return BuscarEstado(estadosPedido, "prepar")
                   ?? BuscarEstado(estadosPedido, "despach")
                   ?? estadosPedido.FirstOrDefault()?.IdEstadoPedidoMuestra;
        }

        private static Guid? BuscarEstado(IEnumerable<EstadoPedidoMuestra> estados, string keyword)
        {
            if (estados == null)
                return null;

            var compare = CultureInfo.InvariantCulture.CompareInfo;
            foreach (var estado in estados)
            {
                if (estado == null)
                    continue;

                var nombre = estado.NombreEstadoPedidoMuestra ?? string.Empty;
                if (compare.IndexOf(nombre, keyword, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0)
                {
                    return estado.IdEstadoPedidoMuestra;
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