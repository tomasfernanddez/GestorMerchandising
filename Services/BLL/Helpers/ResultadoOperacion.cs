using System;

namespace Services.BLL.Helpers
{
    public class ResultadoOperacion
    {
        public bool EsValido { get; set; }
        public string Mensaje { get; set; }
        public Guid? IdGenerado { get; set; }

        /// <summary>
        /// Crea un resultado exitoso
        /// </summary>
        public static ResultadoOperacion Exitoso(string mensaje, Guid? idGenerado = null)
        {
            return new ResultadoOperacion
            {
                EsValido = true,
                Mensaje = mensaje,
                IdGenerado = idGenerado
            };
        }

        /// <summary>
        /// Crea un resultado de error
        /// </summary>
        public static ResultadoOperacion Error(string mensaje)
        {
            return new ResultadoOperacion
            {
                EsValido = false,
                Mensaje = mensaje
            };
        }
    }
}
