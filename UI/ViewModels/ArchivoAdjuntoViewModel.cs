using System;
using UI.Helpers;

namespace UI.ViewModels
{
    public class ArchivoAdjuntoViewModel
    {
        public Guid IdArchivoAdjunto { get; set; }
        public Guid? IdPedido { get; set; }
        public Guid? IdPedidoMuestra { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string TipoContenido { get; set; }
        public long TamanoBytes { get; set; }
        public DateTime FechaSubida { get; set; }
        public Guid IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Descripcion { get; set; }
        public byte[] Contenido { get; set; }

        public string TamanoLegible => FormatearTamano(TamanoBytes);

        public string FechaSubidaTexto
        {
            get
            {
                var fechaLocal = ArgentinaDateTimeHelper.ToArgentina(FechaSubida);
                return fechaLocal.ToString("g");
            }
        }

        private static string FormatearTamano(long bytes)
        {
            if (bytes <= 0)
                return "0 B";

            string[] unidades = { "B", "KB", "MB", "GB" };
            double tam = bytes;
            var indice = 0;
            while (tam >= 1024 && indice < unidades.Length - 1)
            {
                tam /= 1024;
                indice++;
            }

            return $"{tam:0.##} {unidades[indice]}";
        }
    }
}