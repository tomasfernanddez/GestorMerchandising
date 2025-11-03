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
    public static class ReportExportHelper
    {
        public static void ExportToExcel(string path, string titulo, DataGridView grid)
        {
            if (grid == null)
                throw new ArgumentNullException(nameof(grid));

            var filas = ConstruirFilasDesdeGrilla(grid);
            CrearXlsx(path, titulo ?? "Reporte", filas);
        }

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

        private static void AgregarEntrada(ZipArchive archive, string nombre, string contenido)
        {
            var entry = archive.CreateEntry(nombre, CompressionLevel.Optimal);
            using (var writer = new StreamWriter(entry.Open(), Encoding.UTF8))
            {
                writer.Write(contenido);
            }
        }

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

        private static string ObtenerRelsPrincipal()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                   "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
                   "</Relationships>";
        }

        private static string ObtenerWorkbookRels()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                   "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
                   "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>" +
                   "</Relationships>";
        }

        private static string ObtenerWorkbookXml(string sheetName)
        {
            var nombre = EscaparXml(sheetName ?? "Sheet1");
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                   "<sheets><sheet name=\"" + nombre + "\" sheetId=\"1\" r:id=\"rId1\"/></sheets></workbook>";
        }

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

        private static string EscaparXml(string valor)
        {
            return (valor ?? string.Empty)
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }

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

        private static string EscaparPdf(string texto)
        {
            return (texto ?? string.Empty)
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }
    }
}