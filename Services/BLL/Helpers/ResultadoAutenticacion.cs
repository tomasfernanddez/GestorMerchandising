using Services.DomainModel.Entities;
using System;

namespace Services.BLL.Helpers
{
    public class ResultadoAutenticacion
    {
        public bool EsValido { get; set; }
        public string Mensaje { get; set; }
        public Usuario Usuario { get; set; }
        public Guid? IdSesion { get; set; }

        /// <summary>
        /// Crea un resultado exitoso incluyendo el usuario autenticado.
        /// </summary>
        public static ResultadoAutenticacion Exitoso(Usuario usuario, string mensaje = "Autenticación exitosa", Guid? idSesion = null)
        {
            return new ResultadoAutenticacion
            {
                EsValido = true,
                Mensaje = mensaje,
                Usuario = usuario,
                IdSesion = idSesion
            };
        }

        /// <summary>
        /// Crea un resultado de autenticación fallida con el mensaje indicado.
        /// </summary>
        public static ResultadoAutenticacion Error(string mensaje)
        {
            return new ResultadoAutenticacion
            {
                EsValido = false,
                Mensaje = mensaje
            };
        }
    }
}
