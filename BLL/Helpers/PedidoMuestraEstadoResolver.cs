using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Helpers
{
    public static class PedidoMuestraEstadoResolver
    {
        public static EstadoCalculado CalcularEstado(IEnumerable<DetalleMuestra> detalles, IEnumerable<EstadoPedidoMuestra> estadosPedido)
        {
            var nombres = detalles?
                .Select(d => d.EstadoMuestra?.NombreEstadoMuestra ?? string.Empty)
                .ToList() ?? new List<string>();

            return CalcularEstado(nombres, estadosPedido);
        }

        public static EstadoCalculado CalcularEstado(IEnumerable<string> estadosDetalle, IEnumerable<EstadoPedidoMuestra> estadosPedido)
        {
            var lista = NormalizarEstados(estadosDetalle);
            var catalogo = estadosPedido?.ToList() ?? new List<EstadoPedidoMuestra>();

            if (lista.Count == 0)
            {
                return CrearResultado(BuscarEstadoPendiente(catalogo));
            }

            if (lista.All(s => Contiene(s, "cancel")))
            {
                return CrearResultado(BuscarEstado(catalogo, "cancel"));
            }

            if (lista.All(s => Contiene(s, "devuel") || Contiene(s, "perd")))
            {
                var finalizado = BuscarEstado(catalogo, "final");
                if (finalizado != null)
                    return CrearResultado(finalizado);

                finalizado = BuscarEstado(catalogo, "cerr");
                if (finalizado != null)
                    return CrearResultado(finalizado);
            }

            if (lista.Any(s => Contiene(s, "factur")))
            {
                var facturar = BuscarEstado(catalogo, "factur");
                if (facturar != null)
                    return CrearResultado(facturar);
            }

            if (lista.All(s => Contiene(s, "client")))
            {
                var enCliente = BuscarEstado(catalogo, "client");
                if (enCliente != null)
                    return CrearResultado(enCliente);
            }

            if (lista.Any(s => Contiene(s, "pend")))
            {
                var pendiente = BuscarEstadoPendiente(catalogo);
                if (pendiente != null)
                    return CrearResultado(pendiente);
            }

            return CrearResultado(BuscarEstadoPendiente(catalogo));
        }

        private static List<string> NormalizarEstados(IEnumerable<string> estados)
        {
            return estados?
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(Normalizar)
                .ToList()
                ?? new List<string>();
        }

        private static string Normalizar(string texto)
        {
            var formD = texto.Normalize(NormalizationForm.FormD);
            var chars = formD.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            var limpio = new string(chars.ToArray());
            return limpio.Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }

        private static bool Contiene(string texto, string keyword)
        {
            var compare = CultureInfo.InvariantCulture.CompareInfo;
            return compare.IndexOf(texto ?? string.Empty, keyword, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }

        private static EstadoPedidoMuestra BuscarEstado(IEnumerable<EstadoPedidoMuestra> estados, string keyword)
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
                    return estado;
                }
            }

            return null;
        }

        private static EstadoPedidoMuestra BuscarEstadoPendiente(IEnumerable<EstadoPedidoMuestra> estados)
        {
            var pendiente = BuscarEstado(estados, "pend");
            if (pendiente != null)
                return pendiente;

            pendiente = BuscarEstado(estados, "solic");
            if (pendiente != null)
                return pendiente;

            return estados?.FirstOrDefault();
        }

        private static EstadoCalculado CrearResultado(EstadoPedidoMuestra estado)
        {
            return new EstadoCalculado(estado?.IdEstadoPedidoMuestra, estado?.NombreEstadoPedidoMuestra);
        }
    }
}