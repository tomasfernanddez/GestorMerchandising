using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using DomainModel;
using DomainModel.Entidades;

namespace UI.Helpers
{
    public static class NotaPrestamoPdfHelper
    {
        public static void Generar(PedidoMuestra pedido, IEnumerable<DetalleMuestra> detalles, string rutaArchivo)
        {
            if (pedido == null) throw new ArgumentNullException(nameof(pedido));
            if (string.IsNullOrWhiteSpace(rutaArchivo)) throw new ArgumentNullException(nameof(rutaArchivo));

            var listaDetalles = detalles?.ToList() ?? new List<DetalleMuestra>();

            var lineas = new List<string>
            {
                $"Pedido de muestra / Sample loan: {pedido.NumeroCorrelativo}",
                $"Cliente / Customer: {pedido.Cliente?.RazonSocial ?? "-"}",
                $"Contacto / Contact: {pedido.PersonaContacto ?? "-"}",
                $"Email: {pedido.EmailContacto ?? "-"} | Tel.: {pedido.TelefonoContacto ?? "-"}",
                $"Entrega / Delivered: {FormatearFecha(pedido.FechaEntrega)}",
                $"Devolución esperada / Expected return: {FormatearFecha(pedido.FechaDevolucionEsperada)}",
                $"Observaciones / Notes: {(string.IsNullOrWhiteSpace(pedido.Observaciones) ? "-" : pedido.Observaciones)}",
                " ",
                "Detalle / Items"
            };

            int index = 1;
            foreach (var detalle in listaDetalles)
            {
                var producto = detalle.Producto?.NombreProducto ?? "(sin nombre / unnamed)";
                var estado = detalle.EstadoMuestra?.NombreEstadoMuestra ?? "-";
                var fechaDev = FormatearFecha(detalle.FechaDevolucion);
                var precio = detalle.PrecioFacturacion.HasValue ? detalle.PrecioFacturacion.Value.ToString("C2", CultureInfo.CurrentCulture) : "-";
                lineas.Add($"{index}. {producto} - Estado/Status: {estado} - Devolución/Return: {fechaDev} - Precio/Price: {precio}");
                index++;
            }

            var contenido = ConstruirContenido(lineas);
            var pdf = ConstruirPdf(contenido);
            File.WriteAllBytes(rutaArchivo, pdf);
        }

        private static string ConstruirContenido(List<string> lineas)
        {
            var sb = new StringBuilder();
            sb.AppendLine("BT");
            sb.AppendLine("/F1 12 Tf");
            sb.AppendLine("50 780 Td");

            bool primera = true;
            foreach (var linea in lineas)
            {
                if (!primera)
                {
                    sb.AppendLine("0 -16 Td");
                }
                sb.AppendLine($"({Escapar(linea)}) Tj");
                primera = false;
            }

            sb.AppendLine("ET");
            return sb.ToString();
        }

        private static byte[] ConstruirPdf(string contenido)
        {
            var contenidoBytes = Encoding.ASCII.GetBytes(contenido);
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms, Encoding.ASCII, 1024, leaveOpen: true))
            {
                writer.NewLine = "\n";
                writer.Write("%PDF-1.4\n");
                writer.Flush();

                var offsets = new List<long>();

                void EscribirObjeto(string objeto)
                {
                    offsets.Add(ms.Position);
                    writer.Write(objeto);
                    writer.Write("\n");
                    writer.Flush();
                }

                EscribirObjeto("1 0 obj << /Type /Catalog /Pages 2 0 R >> endobj");
                EscribirObjeto("2 0 obj << /Type /Pages /Kids [3 0 R] /Count 1 >> endobj");
                EscribirObjeto("3 0 obj << /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Contents 4 0 R /Resources << /Font << /F1 5 0 R >> >> >> endobj");
                EscribirObjeto($"4 0 obj << /Length {contenidoBytes.Length} >> stream\n{contenido}\nendstream endobj");
                EscribirObjeto("5 0 obj << /Type /Font /Subtype /Type1 /BaseFont /Helvetica >> endobj");

                var inicioXref = ms.Position;
                writer.Write("xref\n");
                writer.Write($"0 {offsets.Count + 1}\n");
                writer.Write("0000000000 65535 f \n");
                foreach (var offset in offsets)
                {
                    writer.Write($"{offset:0000000000} 00000 n \n");
                }
                writer.Write("trailer << /Size {0} /Root 1 0 R >>\n", offsets.Count + 1);
                writer.Write("startxref\n");
                writer.Write($"{inicioXref}\n");
                writer.Write("%%EOF\n");
                writer.Flush();

                return ms.ToArray();
            }
        }

        private static string Escapar(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;
            return texto.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
        }

        private static string FormatearFecha(DateTime? fecha)
        {
            return fecha.HasValue ? fecha.Value.ToString("d", CultureInfo.CurrentCulture) : "-";
        }
    }
}