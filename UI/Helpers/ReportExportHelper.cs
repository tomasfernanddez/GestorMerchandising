using BLL.Reportes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace UI.Helpers
{
    public static class ReportExportHelper
    {
        public static void ExportToExcel(string path, ReporteGeneralResult data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var rows = new List<string[]> { new[] { "Reporte", "Descripción", "Valor 1", "Valor 2", "Valor 3", "Valor 4" } };

            rows.AddRange(data.Operativos.PedidosPorEstado.Select(item => new[]
            {
                "Operativos - Pedidos por estado",
                item.Estado,
                item.Cantidad.ToString(CultureInfo.InvariantCulture),
                item.Total.ToString("F2", CultureInfo.InvariantCulture),
                item.SaldoPendiente.ToString("F2", CultureInfo.InvariantCulture),
                string.Empty
            }));

            rows.AddRange(data.Operativos.PedidosConFechaLimite.Select(item => new[]
            {
                "Operativos - Próximos vencimientos",
                item.NumeroPedido,
                item.Cliente,
                FormatearFecha(item.FechaLimite),
                item.DiasRestantes.ToString(CultureInfo.InvariantCulture),
                item.Estado
            }));

            rows.AddRange(data.Operativos.PedidosDemorados.Select(item => new[]
            {
                "Operativos - Pedidos demorados",
                item.NumeroPedido,
                item.Cliente,
                FormatearFecha(item.FechaLimite),
                item.DiasAtraso.ToString(CultureInfo.InvariantCulture),
                item.Estado
            }));

            rows.AddRange(data.Operativos.MuestrasVencidas.Select(item => new[]
            {
                "Operativos - Muestras vencidas",
                item.NumeroPedido,
                item.Cliente,
                FormatearFecha(item.FechaEsperadaDevolucion),
                item.DiasAtraso.ToString(CultureInfo.InvariantCulture),
                item.SaldoPendiente.ToString("F2", CultureInfo.InvariantCulture)
            }));

            rows.AddRange(data.Operativos.PedidosConSaldoPendiente.Select(item => new[]
            {
                "Operativos - Pedidos con saldo",
                item.NumeroPedido,
                item.Cliente,
                item.Total.ToString("F2", CultureInfo.InvariantCulture),
                item.MontoPagado.ToString("F2", CultureInfo.InvariantCulture),
                item.SaldoPendiente.ToString("F2", CultureInfo.InvariantCulture)
            }));

            rows.AddRange(data.Operativos.ProduccionEnCurso.Select(item => new[]
            {
                "Operativos - Producción en curso",
                item.NumeroPedido,
                item.Cliente,
                FormatearFecha(item.FechaProduccion),
                string.Empty,
                item.Estado
            }));

            rows.AddRange(data.Ventas.VentasMensuales.Select(item => new[]
            {
                "Ventas - Mensuales",
                item.Periodo,
                item.Total.ToString("F2", CultureInfo.InvariantCulture),
                string.Empty,
                string.Empty,
                string.Empty
            }));

            rows.AddRange(data.Ventas.VentasTrimestrales.Select(item => new[]
            {
                "Ventas - Trimestrales",
                item.Periodo,
                item.Total.ToString("F2", CultureInfo.InvariantCulture),
                string.Empty,
                string.Empty,
                string.Empty
            }));

            rows.AddRange(data.Ventas.VentasAnuales.Select(item => new[]
            {
                "Ventas - Anuales",
                item.Periodo,
                item.Total.ToString("F2", CultureInfo.InvariantCulture),
                string.Empty,
                string.Empty,
                string.Empty
            }));

            if (data.Ventas.ComparativaMensual != null)
            {
                rows.Add(new[]
                {
                    "Ventas - Comparativa mensual",
                    data.Ventas.ComparativaMensual.PeriodoActual,
                    data.Ventas.ComparativaMensual.MontoActual.ToString("F2", CultureInfo.InvariantCulture),
                    data.Ventas.ComparativaMensual.PeriodoComparado,
                    data.Ventas.ComparativaMensual.MontoComparado.ToString("F2", CultureInfo.InvariantCulture),
                    data.Ventas.ComparativaMensual.Diferencia.ToString("F2", CultureInfo.InvariantCulture)
                });
            }

            if (data.Ventas.ComparativaAnual != null)
            {
                rows.Add(new[]
                {
                    "Ventas - Comparativa anual",
                    data.Ventas.ComparativaAnual.PeriodoActual,
                    data.Ventas.ComparativaAnual.MontoActual.ToString("F2", CultureInfo.InvariantCulture),
                    data.Ventas.ComparativaAnual.PeriodoComparado,
                    data.Ventas.ComparativaAnual.MontoComparado.ToString("F2", CultureInfo.InvariantCulture),
                    data.Ventas.ComparativaAnual.Diferencia.ToString("F2", CultureInfo.InvariantCulture)
                });
            }

            rows.AddRange(data.Ventas.RankingClientes.Select(item => new[]
            {
                "Ventas - Ranking clientes",
                item.Cliente,
                item.TotalFacturado.ToString("F2", CultureInfo.InvariantCulture),
                item.CantidadPedidos.ToString(CultureInfo.InvariantCulture),
                string.Empty,
                string.Empty
            }));

            rows.AddRange(data.Ventas.VentasPorCategoria.Select(item => new[]
            {
                "Ventas - Categorías",
                item.Categoria,
                item.Total.ToString("F2", CultureInfo.InvariantCulture),
                string.Empty,
                string.Empty,
                string.Empty
            }));

            rows.AddRange(data.Financieros.FacturacionPorPeriodo.Select(item => new[]
            {
                "Finanzas - Facturación",
                item.Periodo,
                item.TotalFacturado.ToString("F2", CultureInfo.InvariantCulture),
                string.Empty,
                string.Empty,
                string.Empty
            }));

            rows.AddRange(data.Financieros.CuentasPorCobrar.Select(item => new[]
            {
                "Finanzas - Cuentas por cobrar",
                item.Cliente,
                item.SaldoPendiente.ToString("F2", CultureInfo.InvariantCulture),
                string.Empty,
                string.Empty,
                string.Empty
            }));

            rows.AddRange(data.Financieros.PagosRecibidos.Select(item => new[]
            {
                "Finanzas - Pagos recibidos",
                item.NumeroPedido,
                item.Cliente,
                FormatearFecha(item.Fecha),
                item.Monto.ToString("F2", CultureInfo.InvariantCulture),
                string.Empty
            }));

            rows.AddRange(data.Financieros.ProyeccionIngresos.Select(item => new[]
            {
                "Finanzas - Proyección ingresos",
                item.NumeroPedido,
                item.Cliente,
                FormatearFecha(item.FechaEsperada),
                item.MontoProyectado.ToString("F2", CultureInfo.InvariantCulture),
                string.Empty
            }));

            CrearXlsx(path, "Reportes", rows);
        }

        public static void ExportToPdf(string path, ReporteGeneralResult data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var lineas = new List<string>
            {
                $"Reporte generado: {DateTime.Now:G}",
                string.Empty,
                "OPERATIVOS"
            };

            lineas.AddRange(data.Operativos.PedidosPorEstado.Select(p =>
                $"Estado {p.Estado}: {p.Cantidad} pedidos - Total {p.Total:C2} - Saldo {p.SaldoPendiente:C2}"));
            lineas.AddRange(data.Operativos.PedidosConFechaLimite.Select(p =>
                $"Vence {FormatearFecha(p.FechaLimite)} ({p.DiasRestantes} días) - {p.NumeroPedido} - {p.Cliente}"));
            lineas.AddRange(data.Operativos.PedidosDemorados.Select(p =>
                $"Atraso {p.DiasAtraso} días - {p.NumeroPedido} - {p.Cliente}"));
            lineas.AddRange(data.Operativos.MuestrasVencidas.Select(p =>
                $"Muestra {p.NumeroPedido} - {p.Cliente} - atraso {p.DiasAtraso} días"));
            lineas.AddRange(data.Operativos.PedidosConSaldoPendiente.Select(p =>
                $"Saldo {p.SaldoPendiente:C2} - {p.NumeroPedido} - {p.Cliente}"));
            lineas.AddRange(data.Operativos.ProduccionEnCurso.Select(p =>
                $"En producción desde {FormatearFecha(p.FechaProduccion)} - {p.NumeroPedido}"));

            lineas.Add(string.Empty);
            lineas.Add("VENTAS");
            lineas.AddRange(data.Ventas.VentasMensuales.Select(v =>
                $"Mensual {v.Periodo}: {v.Total:C2}"));
            lineas.AddRange(data.Ventas.VentasTrimestrales.Select(v =>
                $"Trimestral {v.Periodo}: {v.Total:C2}"));
            lineas.AddRange(data.Ventas.VentasAnuales.Select(v =>
                $"Anual {v.Periodo}: {v.Total:C2}"));

            if (data.Ventas.ComparativaMensual != null)
            {
                lineas.Add($"Comparativa mensual {data.Ventas.ComparativaMensual.PeriodoActual}: {data.Ventas.ComparativaMensual.MontoActual:C2} vs {data.Ventas.ComparativaMensual.PeriodoComparado}: {data.Ventas.ComparativaMensual.MontoComparado:C2}");
            }

            if (data.Ventas.ComparativaAnual != null)
            {
                lineas.Add($"Comparativa anual {data.Ventas.ComparativaAnual.PeriodoActual}: {data.Ventas.ComparativaAnual.MontoActual:C2} vs {data.Ventas.ComparativaAnual.PeriodoComparado}: {data.Ventas.ComparativaAnual.MontoComparado:C2}");
            }

            lineas.AddRange(data.Ventas.RankingClientes.Select(r =>
                $"Cliente {r.Cliente}: {r.TotalFacturado:C2} ({r.CantidadPedidos} pedidos)"));
            lineas.AddRange(data.Ventas.VentasPorCategoria.Select(r =>
                $"Categoría {r.Categoria}: {r.Total:C2}"));

            lineas.Add(string.Empty);
            lineas.Add("FINANZAS");
            lineas.AddRange(data.Financieros.FacturacionPorPeriodo.Select(f =>
                $"Facturación {f.Periodo}: {f.TotalFacturado:C2}"));
            lineas.AddRange(data.Financieros.CuentasPorCobrar.Select(c =>
                $"Por cobrar {c.Cliente}: {c.SaldoPendiente:C2}"));
            lineas.AddRange(data.Financieros.PagosRecibidos.Select(p =>
                $"Pago {FormatearFecha(p.Fecha)} {p.NumeroPedido}: {p.Monto:C2}"));
            lineas.AddRange(data.Financieros.ProyeccionIngresos.Select(p =>
                $"Proyección {p.NumeroPedido}: {p.MontoProyectado:C2} (est. {FormatearFecha(p.FechaEsperada)})"));

            CrearPdf(path, lineas);
        }

        public static void ExportRawData(string path, ReporteGeneralResult data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                EscribirCsv(archive, "operativos_pedidos_por_estado.csv",
                    new[] { "Estado", "Cantidad", "Total", "Saldo" },
                    data.Operativos.PedidosPorEstado.Select(p => new[]
                    {
                        p.Estado,
                        p.Cantidad.ToString(CultureInfo.InvariantCulture),
                        p.Total.ToString("F2", CultureInfo.InvariantCulture),
                        p.SaldoPendiente.ToString("F2", CultureInfo.InvariantCulture)
                    }));

                EscribirCsv(archive, "operativos_proximos_vencimientos.csv",
                    new[] { "Numero", "Cliente", "FechaLimite", "Dias", "Estado" },
                    data.Operativos.PedidosConFechaLimite.Select(p => new[]
                    {
                        p.NumeroPedido,
                        p.Cliente,
                        FormatearFecha(p.FechaLimite),
                        p.DiasRestantes.ToString(CultureInfo.InvariantCulture),
                        p.Estado
                    }));

                EscribirCsv(archive, "operativos_demoras.csv",
                    new[] { "Numero", "Cliente", "FechaLimite", "DiasAtraso", "Estado" },
                    data.Operativos.PedidosDemorados.Select(p => new[]
                    {
                        p.NumeroPedido,
                        p.Cliente,
                        FormatearFecha(p.FechaLimite),
                        p.DiasAtraso.ToString(CultureInfo.InvariantCulture),
                        p.Estado
                    }));

                EscribirCsv(archive, "operativos_muestras_vencidas.csv",
                    new[] { "Numero", "Cliente", "FechaEsperada", "DiasAtraso", "Saldo" },
                    data.Operativos.MuestrasVencidas.Select(p => new[]
                    {
                        p.NumeroPedido,
                        p.Cliente,
                        FormatearFecha(p.FechaEsperadaDevolucion),
                        p.DiasAtraso.ToString(CultureInfo.InvariantCulture),
                        p.SaldoPendiente.ToString("F2", CultureInfo.InvariantCulture)
                    }));

                EscribirCsv(archive, "operativos_saldo.csv",
                    new[] { "Numero", "Cliente", "Total", "Pagado", "Saldo" },
                    data.Operativos.PedidosConSaldoPendiente.Select(p => new[]
                    {
                        p.NumeroPedido,
                        p.Cliente,
                        p.Total.ToString("F2", CultureInfo.InvariantCulture),
                        p.MontoPagado.ToString("F2", CultureInfo.InvariantCulture),
                        p.SaldoPendiente.ToString("F2", CultureInfo.InvariantCulture)
                    }));

                EscribirCsv(archive, "operativos_produccion.csv",
                    new[] { "Numero", "Cliente", "FechaProduccion", "Estado" },
                    data.Operativos.ProduccionEnCurso.Select(p => new[]
                    {
                        p.NumeroPedido,
                        p.Cliente,
                        FormatearFecha(p.FechaProduccion),
                        p.Estado
                    }));

                EscribirCsv(archive, "ventas_mensuales.csv",
                    new[] { "Periodo", "Total" },
                    data.Ventas.VentasMensuales.Select(v => new[]
                    {
                        v.Periodo,
                        v.Total.ToString("F2", CultureInfo.InvariantCulture)
                    }));

                EscribirCsv(archive, "ventas_trimestrales.csv",
                    new[] { "Periodo", "Total" },
                    data.Ventas.VentasTrimestrales.Select(v => new[]
                    {
                        v.Periodo,
                        v.Total.ToString("F2", CultureInfo.InvariantCulture)
                    }));

                EscribirCsv(archive, "ventas_anuales.csv",
                    new[] { "Periodo", "Total" },
                    data.Ventas.VentasAnuales.Select(v => new[]
                    {
                        v.Periodo,
                        v.Total.ToString("F2", CultureInfo.InvariantCulture)
                    }));

                if (data.Ventas.ComparativaMensual != null)
                {
                    EscribirCsv(archive, "ventas_comparativa_mensual.csv",
                        new[] { "PeriodoActual", "MontoActual", "PeriodoComparado", "MontoComparado", "Diferencia" },
                        new[]
                        {
                            new[]
                            {
                                data.Ventas.ComparativaMensual.PeriodoActual,
                                data.Ventas.ComparativaMensual.MontoActual.ToString("F2", CultureInfo.InvariantCulture),
                                data.Ventas.ComparativaMensual.PeriodoComparado,
                                data.Ventas.ComparativaMensual.MontoComparado.ToString("F2", CultureInfo.InvariantCulture),
                                data.Ventas.ComparativaMensual.Diferencia.ToString("F2", CultureInfo.InvariantCulture)
                            }
                        });
                }

                if (data.Ventas.ComparativaAnual != null)
                {
                    EscribirCsv(archive, "ventas_comparativa_anual.csv",
                        new[] { "PeriodoActual", "MontoActual", "PeriodoComparado", "MontoComparado", "Diferencia" },
                        new[]
                        {
                            new[]
                            {
                                data.Ventas.ComparativaAnual.PeriodoActual,
                                data.Ventas.ComparativaAnual.MontoActual.ToString("F2", CultureInfo.InvariantCulture),
                                data.Ventas.ComparativaAnual.PeriodoComparado,
                                data.Ventas.ComparativaAnual.MontoComparado.ToString("F2", CultureInfo.InvariantCulture),
                                data.Ventas.ComparativaAnual.Diferencia.ToString("F2", CultureInfo.InvariantCulture)
                            }
                        });
                }

                EscribirCsv(archive, "ventas_ranking_clientes.csv",
                    new[] { "Cliente", "Total", "CantidadPedidos" },
                    data.Ventas.RankingClientes.Select(r => new[]
                    {
                        r.Cliente,
                        r.TotalFacturado.ToString("F2", CultureInfo.InvariantCulture),
                        r.CantidadPedidos.ToString(CultureInfo.InvariantCulture)
                    }));

                EscribirCsv(archive, "ventas_por_categoria.csv",
                    new[] { "Categoria", "Total" },
                    data.Ventas.VentasPorCategoria.Select(r => new[]
                    {
                        r.Categoria,
                        r.Total.ToString("F2", CultureInfo.InvariantCulture)
                    }));

                EscribirCsv(archive, "finanzas_facturacion.csv",
                    new[] { "Periodo", "Total" },
                    data.Financieros.FacturacionPorPeriodo.Select(f => new[]
                    {
                        f.Periodo,
                        f.TotalFacturado.ToString("F2", CultureInfo.InvariantCulture)
                    }));

                EscribirCsv(archive, "finanzas_cuentas_por_cobrar.csv",
                    new[] { "Cliente", "Saldo" },
                    data.Financieros.CuentasPorCobrar.Select(c => new[]
                    {
                        c.Cliente,
                        c.SaldoPendiente.ToString("F2", CultureInfo.InvariantCulture)
                    }));

                EscribirCsv(archive, "finanzas_pagos_recibidos.csv",
                    new[] { "Numero", "Cliente", "Fecha", "Monto" },
                    data.Financieros.PagosRecibidos.Select(p => new[]
                    {
                        p.NumeroPedido,
                        p.Cliente,
                        FormatearFecha(p.Fecha),
                        p.Monto.ToString("F2", CultureInfo.InvariantCulture)
                    }));

                EscribirCsv(archive, "finanzas_proyeccion.csv",
                    new[] { "Numero", "Cliente", "FechaEsperada", "Monto" },
                    data.Financieros.ProyeccionIngresos.Select(p => new[]
                    {
                        p.NumeroPedido,
                        p.Cliente,
                        FormatearFecha(p.FechaEsperada),
                        p.MontoProyectado.ToString("F2", CultureInfo.InvariantCulture)
                    }));
            }
        }

        private static void EscribirCsv(ZipArchive archive, string nombre, IEnumerable<string> cabecera, IEnumerable<IEnumerable<string>> filas)
        {
            var entry = archive.CreateEntry(nombre, CompressionLevel.Optimal);
            using (var writer = new StreamWriter(entry.Open(), Encoding.UTF8))
            {
                writer.WriteLine(string.Join(",", cabecera.Select(EscapeCsv)));
                foreach (var fila in filas)
                {
                    writer.WriteLine(string.Join(",", fila.Select(EscapeCsv)));
                }
            }
        }

        private static string EscapeCsv(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var needsQuotes = value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) >= 0;
            var escaped = value.Replace("\"", "\"\"");
            return needsQuotes ? $"\"{escaped}\"" : escaped;
        }

        private static string FormatearFecha(DateTime? fecha)
        {
            return fecha?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty;
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
            var nombre = string.IsNullOrWhiteSpace(sheetName) ? "Hoja1" : sheetName;
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                   $"<sheets><sheet name=\"{EscaparXml(nombre)}\" sheetId=\"1\" r:id=\"rId1\"/></sheets>" +
                   "</workbook>";
        }

        private static string ObtenerStylesXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" +
                   "<fonts count=\"1\"><font><sz val=\"11\"/><color theme=\"1\"/><name val=\"Calibri\"/></font></fonts>" +
                   "<fills count=\"1\"><fill><patternFill patternType=\"none\"/></fill></fills>" +
                   "<borders count=\"1\"><border/></borders>" +
                   "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>" +
                   "<cellXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/></cellXfs>" +
                   "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>" +
                   "</styleSheet>";
        }

        private static string ObtenerSheetXml(IList<string[]> filas)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
            sb.Append("<sheetData>");

            for (var i = 0; i < filas.Count; i++)
            {
                var fila = filas[i] ?? Array.Empty<string>();
                sb.AppendFormat(CultureInfo.InvariantCulture, "<row r=\"{0}\">", i + 1);

                for (var c = 0; c < fila.Length; c++)
                {
                    var valor = fila[c] ?? string.Empty;
                    sb.AppendFormat(CultureInfo.InvariantCulture,
                        "<c r=\"{0}{1}\" t=\"inlineStr\"><is><t>{2}</t></is></c>",
                        ObtenerColumna(c),
                        i + 1,
                        EscaparXml(valor));
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