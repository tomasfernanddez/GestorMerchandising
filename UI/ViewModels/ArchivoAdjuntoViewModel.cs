using System;
using UI.Helpers;

namespace UI.ViewModels
{
    /// <summary>
    /// Representa la información necesaria para mostrar un archivo adjunto en la interfaz.
    /// </summary>
    public class ArchivoAdjuntoViewModel
    {
        /// <summary>
        /// Identificador del archivo adjunto.
        /// </summary>
        public Guid IdArchivoAdjunto { get; set; }
        /// <summary>
        /// Identificador del pedido asociado cuando aplica.
        /// </summary>
        public Guid? IdPedido { get; set; }
        /// <summary>
        /// Identificador del pedido de muestra asociado cuando aplica.
        /// </summary>
        public Guid? IdPedidoMuestra { get; set; }
        /// <summary>
        /// Nombre visible del archivo.
        /// </summary>
        public string NombreArchivo { get; set; }
        /// <summary>
        /// Extensión del archivo.
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// Tipo MIME del contenido.
        /// </summary>
        public string TipoContenido { get; set; }
        /// <summary>
        /// Tamaño del archivo en bytes.
        /// </summary>
        public long TamanoBytes { get; set; }
        /// <summary>
        /// Fecha y hora en la que se subió el archivo.
        /// </summary>
        public DateTime FechaSubida { get; set; }
        /// <summary>
        /// Identificador del usuario que subió el archivo.
        /// </summary>
        public Guid IdUsuario { get; set; }
        /// <summary>
        /// Nombre del usuario que subió el archivo.
        /// </summary>
        public string NombreUsuario { get; set; }
        /// <summary>
        /// Descripción opcional del adjunto.
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Contenido binario del archivo.
        /// </summary>
        public byte[] Contenido { get; set; }

        /// <summary>
        /// Devuelve el tamaño en un formato legible.
        /// </summary>
        public string TamanoLegible => FormatearTamano(TamanoBytes);

        /// <summary>
        /// Fecha de subida convertida a zona horaria argentina en formato corto.
        /// </summary>
        public string FechaSubidaTexto
        {
            get
            {
                var fechaLocal = ArgentinaDateTimeHelper.ToArgentina(FechaSubida);
                return fechaLocal.ToString("g");
            }
        }

        /// <summary>
        /// Formatea un valor de bytes en unidades legibles.
        /// </summary>
        /// <param name="bytes">Cantidad de bytes.</param>
        /// <returns>Cadena con el tamaño legible.</returns>
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