using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Valida el formato y dígito verificador del CUIT argentino
        /// </summary>
        public static (bool esValido, string mensaje) ValidarCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return (false, "El CUIT no puede estar vacío");

            // Limpiar el CUIT (remover guiones y espacios)
            string cuitLimpio = Regex.Replace(cuit, @"[^\d]", "");

            // Verificar longitud
            if (cuitLimpio.Length != 11)
                return (false, "El CUIT debe tener exactamente 11 dígitos");

            // Verificar que solo contenga números
            if (!cuitLimpio.All(char.IsDigit))
                return (false, "El CUIT solo puede contener números");

            // Verificar prefijo válido (tipos de documento)
            var prefijo = cuitLimpio.Substring(0, 2);
            var prefijosValidos = new[] { "20", "23", "24", "27", "30", "33", "34" };

            if (!prefijosValidos.Contains(prefijo))
                return (false, "El prefijo del CUIT no es válido (debe ser 20, 23, 24, 27, 30, 33 o 34)");

            // Calcular dígito verificador
            var multiplicadores = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            var suma = 0;

            for (int i = 0; i < 10; i++)
            {
                suma += int.Parse(cuitLimpio[i].ToString()) * multiplicadores[i];
            }

            var resto = suma % 11;
            var digitoCalculado = resto < 2 ? resto : 11 - resto;
            var digitoOriginal = int.Parse(cuitLimpio[10].ToString());

            if (digitoCalculado != digitoOriginal)
                return (false, "El dígito verificador del CUIT es incorrecto");

            return (true, "CUIT válido");
        }

        /// <summary>
        /// Valida formato de email
        /// </summary>
        public static (bool esValido, string mensaje) ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (true, ""); // Email es opcional

            if (email.Length > 100)
                return (false, "El email no puede superar los 100 caracteres");

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    return (false, "El formato del email no es válido");

                // Verificar que tenga @ y punto
                if (!email.Contains("@") || !email.Contains("."))
                    return (false, "El email debe contener @ y al menos un punto");

                // Verificar que no tenga espacios
                if (email.Contains(" "))
                    return (false, "El email no puede contener espacios");

                return (true, "Email válido");
            }
            catch
            {
                return (false, "El formato del email no es válido");
            }
        }

        /// <summary>
        /// Valida longitud de texto
        /// </summary>
        public static (bool esValido, string mensaje) ValidarLongitudTexto(string texto, int longitudMaxima, string nombreCampo, bool esObligatorio = true)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                if (esObligatorio)
                    return (false, $"{nombreCampo} es obligatorio");
                return (true, "");
            }

            if (texto.Length > longitudMaxima)
                return (false, $"{nombreCampo} no puede superar los {longitudMaxima} caracteres");

            return (true, $"{nombreCampo} válido");
        }

        /// <summary>
        /// Valida que un texto tenga una longitud mínima y máxima
        /// </summary>
        public static (bool esValido, string mensaje) ValidarLongitudTexto(string texto, int longitudMinima, int longitudMaxima, string nombreCampo, bool esObligatorio = true)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                if (esObligatorio)
                    return (false, $"{nombreCampo} es obligatorio");
                return (true, "");
            }

            if (texto.Length < longitudMinima)
                return (false, $"{nombreCampo} debe tener al menos {longitudMinima} caracteres");

            if (texto.Length > longitudMaxima)
                return (false, $"{nombreCampo} no puede superar los {longitudMaxima} caracteres");

            return (true, $"{nombreCampo} válido");
        }

        /// <summary>
        /// Valida que un número esté dentro de un rango
        /// </summary>
        public static (bool esValido, string mensaje) ValidarRangoNumerico(decimal valor, decimal minimo, decimal maximo, string nombreCampo)
        {
            if (valor < minimo)
                return (false, $"{nombreCampo} no puede ser menor a {minimo}");

            if (valor > maximo)
                return (false, $"{nombreCampo} no puede ser mayor a {maximo}");

            return (true, $"{nombreCampo} válido");
        }

        /// <summary>
        /// Valida que un entero esté dentro de un rango
        /// </summary>
        public static (bool esValido, string mensaje) ValidarRangoNumerico(int valor, int minimo, int maximo, string nombreCampo)
        {
            if (valor < minimo)
                return (false, $"{nombreCampo} no puede ser menor a {minimo}");

            if (valor > maximo)
                return (false, $"{nombreCampo} no puede ser mayor a {maximo}");

            return (true, $"{nombreCampo} válido");
        }

        /// <summary>
        /// Valida formato de teléfono argentino
        /// </summary>
        public static (bool esValido, string mensaje) ValidarTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return (true, ""); // Teléfono es opcional

            // Limpiar el teléfono (remover espacios, guiones, paréntesis)
            string telefonoLimpio = Regex.Replace(telefono, @"[^\d]", "");

            // Verificar longitud (mínimo 7, máximo 15 dígitos)
            if (telefonoLimpio.Length < 7 || telefonoLimpio.Length > 15)
                return (false, "El teléfono debe tener entre 7 y 15 dígitos");

            // Verificar que solo contenga números
            if (!telefonoLimpio.All(char.IsDigit))
                return (false, "El teléfono solo puede contener números");

            return (true, "Teléfono válido");
        }

        /// <summary>
        /// Valida que una fecha esté dentro de un rango válido
        /// </summary>
        public static (bool esValido, string mensaje) ValidarFecha(DateTime fecha, DateTime? fechaMinima = null, DateTime? fechaMaxima = null, string nombreCampo = "Fecha")
        {
            var fechaMin = fechaMinima ?? new DateTime(1900, 1, 1);
            var fechaMax = fechaMaxima ?? DateTime.Now.AddYears(10);

            if (fecha < fechaMin)
                return (false, $"{nombreCampo} no puede ser anterior a {fechaMin:dd/MM/yyyy}");

            if (fecha > fechaMax)
                return (false, $"{nombreCampo} no puede ser posterior a {fechaMax:dd/MM/yyyy}");

            return (true, $"{nombreCampo} válida");
        }

        /// <summary>
        /// Valida que una contraseña cumpla con los requisitos mínimos de seguridad
        /// </summary>
        public static (bool esValido, string mensaje) ValidarPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "La contraseña es obligatoria");

            var longitudMinima = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PasswordMinLength"] ?? "4");

            if (password.Length < longitudMinima)
                return (false, $"La contraseña debe tener al menos {longitudMinima} caracteres");

            // Para versiones futuras se pueden agregar más validaciones:
            // - Al menos una mayúscula
            // - Al menos un número  
            // - Al menos un carácter especial
            // Por ahora mantenemos simplicidad según el MVP

            return (true, "Contraseña válida");
        }

        /// <summary>
        /// Valida que un Guid no esté vacío
        /// </summary>
        public static (bool esValido, string mensaje) ValidarGuid(Guid guid, string nombreCampo)
        {
            if (guid == Guid.Empty)
                return (false, $"Debe seleccionar un {nombreCampo}");

            return (true, $"{nombreCampo} válido");
        }

        /// <summary>
        /// Formatea un CUIT para mostrarlo con guiones
        /// </summary>
        public static string FormatearCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return cuit;

            var cuitLimpio = Regex.Replace(cuit, @"[^\d]", "");

            if (cuitLimpio.Length != 11)
                return cuit; // Si no es válido, devolver como está

            return $"{cuitLimpio.Substring(0, 2)}-{cuitLimpio.Substring(2, 8)}-{cuitLimpio.Substring(10, 1)}";
        }

        /// <summary>
        /// Valida múltiples campos y retorna todos los errores encontrados
        /// </summary>
        public static (bool esValido, string mensaje) ValidarMultiples(params (bool esValido, string mensaje)[] validaciones)
        {
            var errores = validaciones
                .Where(v => !v.esValido)
                .Select(v => v.mensaje)
                .ToList();

            if (errores.Any())
            {
                return (false, string.Join("\n", errores));
            }

            return (true, "Validación exitosa");
        }
    }
}
