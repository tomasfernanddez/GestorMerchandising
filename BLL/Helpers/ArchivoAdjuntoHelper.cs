using System;
using System.Collections.Generic;
using System.IO;
using DomainModel;

namespace BLL.Helpers
{
    public static class ArchivoAdjuntoHelper
    {
        public const long MaxFileSizeBytes = 2 * 1024 * 1024;

        private static readonly HashSet<string> ExtensionesPermitidas = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf",
            ".doc",
            ".docx",
            ".jpg",
            ".jpeg",
            ".png"
        };

        private static readonly Dictionary<string, string> TiposContenido = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = "application/pdf",
            [".doc"] = "application/msword",
            [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".png"] = "image/png"
        };

        public static void PrepararAdjuntosParaPedido(ICollection<ArchivoAdjunto> adjuntos, Guid idPedido)
        {
            ProcesarAdjuntos(adjuntos, idPedido, null);
        }

        public static void PrepararAdjuntosParaPedidoMuestra(ICollection<ArchivoAdjunto> adjuntos, Guid idPedidoMuestra)
        {
            ProcesarAdjuntos(adjuntos, null, idPedidoMuestra);
        }

        public static void ActualizarAdjuntoExistente(ArchivoAdjunto destino, ArchivoAdjunto origen)
        {
            if (destino == null)
                throw new ArgumentNullException(nameof(destino));
            if (origen == null)
                throw new ArgumentNullException(nameof(origen));

            var nombre = string.IsNullOrWhiteSpace(origen.NombreArchivo) ? destino.NombreArchivo : origen.NombreArchivo;
            destino.NombreArchivo = NormalizarNombre(nombre);

            var extension = string.IsNullOrWhiteSpace(origen.Extension) ? destino.Extension : origen.Extension;
            destino.Extension = NormalizarExtension(extension);

            destino.TipoContenido = NormalizarTipoContenido(string.IsNullOrWhiteSpace(origen.TipoContenido)
                ? destino.TipoContenido
                : origen.TipoContenido, destino.Extension);

            destino.Descripcion = NormalizarDescripcion(origen.Descripcion);
            destino.NombreUsuario = NormalizarNombreUsuario(string.IsNullOrWhiteSpace(origen.NombreUsuario)
                ? destino.NombreUsuario
                : origen.NombreUsuario);

            if (origen.IdUsuario != Guid.Empty)
            {
                destino.IdUsuario = origen.IdUsuario;
            }

            var contenido = origen.Contenido ?? destino.Contenido;
            if (contenido == null || contenido.Length == 0)
                throw new InvalidOperationException("El archivo adjunto no contiene datos.");

            ValidarContenido(contenido);
            destino.Contenido = contenido;
            destino.TamanoBytes = contenido.LongLength;

            destino.FechaSubida = origen.FechaSubida != default
                ? AsegurarUtc(origen.FechaSubida)
                : (destino.FechaSubida == default ? DateTime.UtcNow : AsegurarUtc(destino.FechaSubida));
        }

        private static void ProcesarAdjuntos(ICollection<ArchivoAdjunto> adjuntos, Guid? idPedido, Guid? idPedidoMuestra)
        {
            if (adjuntos == null || adjuntos.Count == 0)
                return;

            foreach (var adjunto in adjuntos)
            {
                if (adjunto == null)
                    continue;

                if (adjunto.IdArchivoAdjunto == Guid.Empty)
                {
                    adjunto.IdArchivoAdjunto = Guid.NewGuid();
                }

                if (idPedido.HasValue)
                {
                    adjunto.IdPedido = idPedido;
                    adjunto.IdPedidoMuestra = null;
                }
                else if (idPedidoMuestra.HasValue)
                {
                    adjunto.IdPedidoMuestra = idPedidoMuestra;
                    adjunto.IdPedido = null;
                }

                adjunto.NombreArchivo = NormalizarNombre(adjunto.NombreArchivo);
                adjunto.Extension = NormalizarExtension(string.IsNullOrWhiteSpace(adjunto.Extension)
                    ? Path.GetExtension(adjunto.NombreArchivo)
                    : adjunto.Extension);
                adjunto.TipoContenido = NormalizarTipoContenido(adjunto.TipoContenido, adjunto.Extension);
                adjunto.Descripcion = NormalizarDescripcion(adjunto.Descripcion);
                adjunto.NombreUsuario = NormalizarNombreUsuario(adjunto.NombreUsuario);
                adjunto.FechaSubida = AsegurarUtc(adjunto.FechaSubida);

                if (adjunto.IdUsuario == Guid.Empty)
                    throw new InvalidOperationException("El usuario que adjunta el archivo es obligatorio.");

                if (adjunto.Contenido == null || adjunto.Contenido.Length == 0)
                    throw new InvalidOperationException("El archivo adjunto no contiene datos.");

                ValidarContenido(adjunto.Contenido);
                adjunto.TamanoBytes = adjunto.Contenido.LongLength;
            }
        }

        private static string NormalizarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre del archivo adjunto es obligatorio.");

            var limpio = Path.GetFileName(nombre.Trim());
            if (string.IsNullOrWhiteSpace(limpio))
                throw new InvalidOperationException("El nombre del archivo adjunto es obligatorio.");

            return limpio;
        }

        private static string NormalizarExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                throw new InvalidOperationException("El archivo adjunto no tiene una extensión válida.");

            var valor = extension.Trim();
            if (!valor.StartsWith("."))
                valor = "." + valor;

            if (valor.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                valor = ".jpg";

            if (!ExtensionesPermitidas.Contains(valor))
                throw new InvalidOperationException($"El tipo de archivo '{valor}' no está permitido.");

            return valor.ToUpperInvariant();
        }

        private static string NormalizarTipoContenido(string tipo, string extension)
        {
            if (!string.IsNullOrWhiteSpace(tipo))
                return tipo.Trim();

            if (TiposContenido.TryGetValue(extension, out var conocido))
                return conocido;

            return "application/octet-stream";
        }

        private static string NormalizarDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                return null;

            return descripcion.Trim();
        }

        private static string NormalizarNombreUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return "Sistema";

            return nombreUsuario.Trim();
        }

        private static DateTime AsegurarUtc(DateTime fecha)
        {
            if (fecha == default)
                return DateTime.UtcNow;

            switch (fecha.Kind)
            {
                case DateTimeKind.Utc:
                    return fecha;
                case DateTimeKind.Local:
                    return fecha.ToUniversalTime();
                default:
                    return DateTime.SpecifyKind(fecha, DateTimeKind.Utc);
            }
        }

        private static void ValidarContenido(byte[] contenido)
        {
            if (contenido.LongLength > MaxFileSizeBytes)
                throw new InvalidOperationException($"El archivo adjunto supera el tamaño máximo permitido de {MaxFileSizeBytes / 1024 / 1024} MB.");
        }
    }
}