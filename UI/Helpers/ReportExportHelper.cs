using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UI.Localization;

namespace UI.Helpers
{
    /// <summary>
    /// Contiene utilidades para exportar información tabular a diferentes formatos de reporte.
    /// </summary>
    public static class ReportExportHelper
    {
        /// <summary>
        /// Genera un archivo XLSX con la información visible de la grilla.
        /// </summary>
        /// <param name="path">Ruta de destino del archivo.</param>
        /// <param name="titulo">Título del reporte.</param>
        /// <param name="grid">Grilla cuyos datos se exportarán.</param>
        public static void ExportToExcel(string path, string titulo, DataGridView grid)
        {
            if (grid == null)
                throw new ArgumentNullException(nameof(grid));

            var filas = ConstruirFilasDesdeGrilla(grid);
            CrearXlsx(path, titulo ?? "Reporte", filas);
        }

        /// <summary>
        /// Genera un archivo PDF simple con los datos visibles de la grilla.
        /// </summary>
        /// <param name="path">Ruta destino del archivo.</param>
        /// <param name="titulo">Título del reporte.</param>
        /// <param name="grid">Grilla de donde se extraen los datos.</param>
        public static void ExportToPdf(string path, string titulo, DataGridView grid)
        {
            if (grid == null)
                throw new ArgumentNullException(nameof(grid));

            var filas = ConstruirFilasDesdeGrilla(grid);
            var lineas = new List<string>
            {
                 $"{titulo ?? "Reporte"} - {DateTime.Now:G}",
                string.Empty
            };

            foreach (var fila in filas.Skip(1))
            {
                lineas.Add(string.Join(" | ", fila));
            }

            CrearPdf(path, lineas);
        }

        /// <summary>
        /// Construye una representación en filas de los datos de la grilla.
        /// </summary>
        /// <param name="grid">Grilla de origen.</param>
        /// <returns>Lista de filas con los valores visibles en la interfaz.</returns>
        private static IList<string[]> ConstruirFilasDesdeGrilla(DataGridView grid)
        {
            var columnas = grid.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            var filas = new List<string[]> { columnas.Select(c => c.HeaderText ?? string.Empty).ToArray() };

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                var valores = columnas
                    .Select(c => FormatearValor(row.Cells[c.Index]?.Value))
                    .ToArray();

                filas.Add(valores);
            }

            if (filas.Count == 1)
            {
                var sinDatos = new string[columnas.Count];
                if (sinDatos.Length > 0)
                {
                    sinDatos[0] = "report.export.noData".Traducir();
                }
                filas.Add(sinDatos);
            }

            return filas;
        }

        /// <summary>
        /// Formatea un valor proveniente de la grilla usando la cultura actual.
        /// </summary>
        /// <param name="valor">Valor a formatear.</param>
        /// <returns>Cadena representando el valor.</returns>
        private static string FormatearValor(object valor)
        {
            if (valor == null || valor == DBNull.Value)
            {
                return string.Empty;
            }

            switch (valor)
            {
                case DateTime fecha:
                    return fecha.ToString("g", CultureInfo.CurrentCulture);
                case IFormattable formattable:
                    return formattable.ToString(null, CultureInfo.CurrentCulture);
                default:
                    return Convert.ToString(valor, CultureInfo.CurrentCulture) ?? string.Empty;
            }
        }

        /// <summary>
        /// Crea un archivo XLSX usando las filas proporcionadas.
        /// </summary>
        /// <param name="path">Ruta destino.</param>
        /// <param name="sheetName">Nombre de la hoja.</param>
        /// <param name="filas">Filas que se exportarán.</param>
        private static void CrearXlsx(string path, string sheetName, IList<string[]> filas)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                AgregarEntrada(archive, "[Content_Types].xml", ObtenerContentTypes());
                AgregarEntrada(archive, "_rels/.rels", ObtenerRelsPrincipal());
                AgregarEntrada(archive, "xl/_rels/workbook.xml.rels", ObtenerWorkbookRels());
                AgregarEntrada(archive, "xl/workbook.xml", ObtenerWorkbookXml(sheetName));
                AgregarEntrada(archive, "xl/styles.xml", ObtenerStylesXml());
                AgregarEntrada(archive, "xl/worksheets/sheet1.xml", ObtenerSheetXml(filas));
            }
        }

        /// <summary>
        /// Agrega una entrada al archivo comprimido que conforma el XLSX.
        /// </summary>
        /// <param name="archive">Archivo ZIP en construcción.</param>
        /// <param name="nombre">Nombre de la entrada.</param>
        /// <param name="contenido">Contenido XML a escribir.</param>
        private static void AgregarEntrada(ZipArchive archive, string nombre, string contenido)
        {
            var entry = archive.CreateEntry(nombre, CompressionLevel.Optimal);
            using (var writer = new StreamWriter(entry.Open(), Encoding.UTF8))
            {
                writer.Write(contenido);
            }
        }

        /// <summary>
        /// Devuelve el manifiesto de tipos de contenido requerido por un paquete XLSX.
        /// </summary>
        /// <returns>XML con la definición de tipos de contenido.</returns>
        private static string ObtenerContentTypes()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
                   "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
                   "<Default Extension=\"xml\" ContentType=\"application/xml\"/>" +
                   "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
                   "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
                   "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>" +
                   "</Types>";
        }

        /// <summary>
        /// Obtiene el archivo de relaciones principal del paquete XLSX.
        /// </summary>
        /// <returns>Contenido XML de la relación principal.</returns>
        private static string ObtenerRelsPrincipal()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                   "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
                   "</Relationships>";
        }

        /// <summary>
        /// Obtiene la definición de relaciones específicas del libro de Excel.
        /// </summary>
        /// <returns>XML con las relaciones del workbook.</returns>
        private static string ObtenerWorkbookRels()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                   "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
                   "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>" +
                   "</Relationships>";
        }

        /// <summary>
        /// Genera el XML principal del libro de Excel.
        /// </summary>
        /// <param name="sheetName">Nombre que se asignará a la hoja.</param>
        /// <returns>XML del workbook.</returns>
        private static string ObtenerWorkbookXml(string sheetName)
        {
            var nombre = EscaparXml(sheetName ?? "Sheet1");
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                   "<sheets><sheet name=\"" + nombre + "\" sheetId=\"1\" r:id=\"rId1\"/></sheets></workbook>";
        }

        /// <summary>
        /// Obtiene la definición de estilos mínima para el archivo XLSX.
        /// </summary>
        /// <returns>XML con la configuración de estilos.</returns>
        private static string ObtenerStylesXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" +
                   "<fonts count=\"1\"><font><sz val=\"11\"/><name val=\"Calibri\"/></font></fonts>" +
                   "<fills count=\"1\"><fill><patternFill patternType=\"none\"/></fill></fills>" +
                   "<borders count=\"1\"><border><left/><right/><top/><bottom/><diagonal/></border></borders>" +
                   "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>" +
                   "<cellXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/></cellXfs>" +
                   "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>" +
                   "</styleSheet>";
        }

        /// <summary>
        /// Construye el XML de la hoja con las filas provistas.
        /// </summary>
        /// <param name="filas">Filas que se escribirán en la hoja.</param>
        /// <returns>XML con la representación de la hoja.</returns>
        private static string ObtenerSheetXml(IList<string[]> filas)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><sheetData>");

            for (var i = 0; i < filas.Count; i++)
            {
                var fila = filas[i] ?? Array.Empty<string>();
                sb.Append("<row r=\"" + (i + 1) + "\">");

                for (var c = 0; c < fila.Length; c++)
                {
                    var valor = fila[c] ?? string.Empty;
                    var celda = ObtenerColumna(c) + (i + 1);
                    sb.Append("<c r=\"" + celda + "\" t=\"inlineStr\"><is><t>");
                    sb.Append(EscaparXml(valor));
                    sb.Append("</t></is></c>");
                }

                sb.Append("</row>");
            }

            sb.Append("</sheetData></worksheet>");
            return sb.ToString();
        }

        /// <summary>
        /// Convierte un índice numérico en la referencia de columna de Excel.
        /// </summary>
        /// <param name="index">Índice basado en cero.</param>
        /// <returns>Cadena que representa la columna (A, B, ...).</returns>
        private static string ObtenerColumna(int index)
        {
            var sb = new StringBuilder();
            var i = index;

            do
            {
                var remainder = i % 26;
                sb.Insert(0, (char)('A' + remainder));
                i = (i / 26) - 1;
            } while (i >= 0);

            return sb.ToString();
        }

        /// <summary>
        /// Escapa caracteres especiales para su inclusión dentro de XML.
        /// </summary>
        /// <param name="valor">Texto a escapar.</param>
        /// <returns>Cadena segura para insertar en XML.</returns>
        private static string EscaparXml(string valor)
        {
            return (valor ?? string.Empty)
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }

        /// <summary>
        /// Construye un PDF básico con las líneas indicadas.
        /// </summary>
        /// <param name="path">Ruta donde se guardará el PDF.</param>
        /// <param name="lineas">Líneas de texto que se incluirán en el documento.</param>
        private static void CrearPdf(string path, IEnumerable<string> lineas)
        {
            var contenido = ConstruirContenidoPdf(lineas ?? Enumerable.Empty<string>());

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.ASCII, 1024, leaveOpen: true))
            {
                writer.WriteLine("%PDF-1.4");
                writer.Flush();

                var offsets = new List<long> { 0 };

                offsets.Add(stream.Position);
                writer.WriteLine("1 0 obj << /Type /Catalog /Pages 2 0 R >> endobj");
                writer.Flush();

                offsets.Add(stream.Position);
                writer.WriteLine("2 0 obj << /Type /Pages /Kids [3 0 R] /Count 1 >> endobj");
                writer.Flush();

                offsets.Add(stream.Position);
                writer.WriteLine("3 0 obj << /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >> endobj");
                writer.Flush();

                offsets.Add(stream.Position);
                writer.WriteLine("4 0 obj << /Type /Font /Subtype /Type1 /BaseFont /Helvetica >> endobj");
                writer.Flush();

                offsets.Add(stream.Position);
                var contenidoBytes = Encoding.ASCII.GetBytes(contenido);
                writer.WriteLine($"5 0 obj << /Length {contenidoBytes.Length} >> stream");
                writer.Flush();
                writer.BaseStream.Write(contenidoBytes, 0, contenidoBytes.Length);
                writer.WriteLine();
                writer.WriteLine("endstream");
                writer.WriteLine("endobj");
                writer.Flush();

                var xrefPosition = stream.Position;
                writer.WriteLine("xref");
                writer.WriteLine("0 6");
                writer.WriteLine("0000000000 65535 f ");
                for (var i = 1; i <= 5; i++)
                {
                    writer.WriteLine($"{offsets[i]:D10} 00000 n ");
                }

                writer.WriteLine("trailer << /Size 6 /Root 1 0 R >>");
                writer.WriteLine("startxref");
                writer.WriteLine(xrefPosition.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("%%EOF");
                writer.Flush();

                File.WriteAllBytes(path, stream.ToArray());
            }
        }

        /// <summary>
        /// Genera las instrucciones de dibujo para incluir texto dentro del PDF.
        /// </summary>
        /// <param name="lineas">Líneas que se deben renderizar.</param>
        /// <returns>Cadena con instrucciones PDF.</returns>
        private static string ConstruirContenidoPdf(IEnumerable<string> lineas)
        {
            var sb = new StringBuilder();
            sb.AppendLine("BT");
            sb.AppendLine("/F1 10 Tf");

            var y = 780;
            foreach (var linea in lineas)
            {
                if (y < 60)
                {
                    y = 780;
                }

                sb.AppendFormat(CultureInfo.InvariantCulture, "1 0 0 1 50 {0} Tm\n", y);
                sb.AppendFormat("({0}) Tj\n", EscaparPdf(linea ?? string.Empty));
                y -= 14;
            }

            sb.Append("ET");
            return sb.ToString();
        }

        /// <summary>
        /// Escapa caracteres especiales según el estándar PDF.
        /// </summary>
        /// <param name="texto">Texto a escapar.</param>
        /// <returns>Texto preparado para usarse en el contenido PDF.</returns>
        private static string EscaparPdf(string texto)
        {
            return (texto ?? string.Empty)
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }
    }
}