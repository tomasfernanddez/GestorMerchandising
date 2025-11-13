using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DomainModel.Entidades;

namespace UI.Helpers
{
    public static class PedidoMuestraEstadoHelper
    {
        public static Guid? CalcularEstadoPedido(IEnumerable<string> estadosDetalle, IEnumerable<EstadoPedidoMuestra> estadosPedido)
        {
            var catalogo = estadosPedido?.ToList() ?? new List<EstadoPedidoMuestra>();
            if (catalogo.Count == 0)
                return null;

            var lista = estadosDetalle?
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(Normalizar)
                .ToList() ?? new List<string>();

            if (lista.Count == 0)
                return BuscarEstadoPorNombre(catalogo, "Pendiente de Envio") ?? catalogo.First().IdEstadoPedidoMuestra;

            var cancelado = Normalizar("Cancelado");
            if (lista.All(s => s == cancelado))
                return BuscarEstadoPorNombre(catalogo, "Cancelado");

            var pendientePago = Normalizar("Pendiente de Pago");
            if (lista.Any(s => s == pendientePago))
                return BuscarEstadoPorNombre(catalogo, "Pendiente de Pago");

            var devuelto = Normalizar("Devuelto");
            var pagado = Normalizar("Pagado");
            if (lista.All(s => s == devuelto || s == pagado))
                return BuscarEstadoPorNombre(catalogo, "Finalizado");

            var enCliente = Normalizar("En Cliente");
            if (lista.All(s => s == enCliente))
                return BuscarEstadoPorNombre(catalogo, "En Cliente");

            if (lista.All(s => s == enCliente || s == devuelto || s == pagado) && lista.Any(s => s == enCliente))
                return BuscarEstadoPorNombre(catalogo, "En Cliente") ?? BuscarEstadoPorNombre(catalogo, "Pendiente de Envio");

            var pendienteEnvio = Normalizar("Pendiente de Envio");
            if (lista.Any(s => s == pendienteEnvio))
                return BuscarEstadoPorNombre(catalogo, "Pendiente de Envio");

            return BuscarEstadoPorNombre(catalogo, "Pendiente de Envio") ?? catalogo.First().IdEstadoPedidoMuestra;
        }

        private static Guid? BuscarEstadoPorNombre(IEnumerable<EstadoPedidoMuestra> estados, string nombre)
        {
            var buscado = Normalizar(nombre ?? string.Empty);
            foreach (var estado in estados)
            {
                if (estado == null)
                    continue;

                var actual = Normalizar(estado.NombreEstadoPedidoMuestra ?? string.Empty);
                if (actual == buscado)
                    return estado.IdEstadoPedidoMuestra;
            }

            return null;
        }

        private static string Normalizar(string texto)
        {
            var formD = texto.Normalize(NormalizationForm.FormD);
            var chars = formD.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            var limpio = new string(chars.ToArray());
            return limpio.Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }
    }
}