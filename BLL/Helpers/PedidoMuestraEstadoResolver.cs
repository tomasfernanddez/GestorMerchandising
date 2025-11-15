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
        /// <summary>
        /// Calcula estado.
        /// </summary>
        public static EstadoCalculado CalcularEstado(IEnumerable<DetalleMuestra> detalles, IEnumerable<EstadoPedidoMuestra> estadosPedido)
        {
            var nombres = detalles?
                .Select(d => d?.EstadoMuestra?.NombreEstadoMuestra ?? string.Empty)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList() ?? new List<string>();

            return CalcularEstado(nombres, estadosPedido);
        }
        /// <summary>
        /// Calcula estado.
        /// </summary>

        public static EstadoCalculado CalcularEstado(IEnumerable<string> estadosDetalle, IEnumerable<EstadoPedidoMuestra> estadosPedido)
        {
            var lista = NormalizarEstados(estadosDetalle);
            var catalogo = estadosPedido?.ToList() ?? new List<EstadoPedidoMuestra>();

            if (lista.Count == 0)
            {
                return CrearResultado(BuscarEstadoPorNombre(catalogo, "Pendiente de Envio") ?? catalogo.FirstOrDefault());
            }

            var estadoCancelado = Normalizar("Cancelado");
            if (lista.All(s => string.Equals(s, estadoCancelado, StringComparison.Ordinal)))
            {
                return CrearResultado(BuscarEstadoPorNombre(catalogo, "Cancelado"));
            }

            var estadoPendientePago = Normalizar("Pendiente de Pago");
            if (lista.Any(s => string.Equals(s, estadoPendientePago, StringComparison.Ordinal)))
            {
                return CrearResultado(BuscarEstadoPorNombre(catalogo, "Pendiente de Pago"));
            }

            var estadoDevuelto = Normalizar("Devuelto");
            var estadoPagado = Normalizar("Pagado");
            if (lista.All(s => string.Equals(s, estadoDevuelto, StringComparison.Ordinal) || string.Equals(s, estadoPagado, StringComparison.Ordinal)))
            {
                return CrearResultado(BuscarEstadoPorNombre(catalogo, "Finalizado"));
            }

            var estadoEnCliente = Normalizar("En Cliente");
            if (lista.All(s => string.Equals(s, estadoEnCliente, StringComparison.Ordinal)))
            {
                return CrearResultado(BuscarEstadoPorNombre(catalogo, "En Cliente"));
            }

            if (lista.All(s => s == estadoEnCliente || s == estadoDevuelto || s == estadoPagado) && lista.Any(s => s == estadoEnCliente))
            {
                return CrearResultado(BuscarEstadoPorNombre(catalogo, "En Cliente") ?? BuscarEstadoPorNombre(catalogo, "Pendiente de Envio"));
            }

            var estadoPendienteEnvio = Normalizar("Pendiente de Envio");
            if (lista.Any(s => string.Equals(s, estadoPendienteEnvio, StringComparison.Ordinal)))
            {
                return CrearResultado(BuscarEstadoPorNombre(catalogo, "Pendiente de Envio"));
            }

            return CrearResultado(BuscarEstadoPorNombre(catalogo, "Pendiente de Envio") ?? catalogo.FirstOrDefault());
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
        /// Busca estado por nombre.
        /// </summary>

        private static EstadoPedidoMuestra BuscarEstadoPorNombre(IEnumerable<EstadoPedidoMuestra> estados, string nombreBuscado)
        {
            if (estados == null)
                return null;

            var normalizado = Normalizar(nombreBuscado ?? string.Empty);
            foreach (var estado in estados)
            {
                if (estado == null)
                    continue;

                var nombreEstado = estado.NombreEstadoPedidoMuestra ?? string.Empty;
                if (string.Equals(Normalizar(nombreEstado), normalizado, StringComparison.Ordinal))
                    return estado;
            }

            return null;
        }
        /// <summary>
        /// Crea resultado.
        /// </summary>

        private static EstadoCalculado CrearResultado(EstadoPedidoMuestra estado)
        {
            return new EstadoCalculado(estado?.IdEstadoPedidoMuestra, estado?.NombreEstadoPedidoMuestra);
        }
    }
}