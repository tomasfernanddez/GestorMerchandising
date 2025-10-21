using BLL.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
    public class EncriptacionService : IEncriptacionService
    {
        // "Pepper" (clave de aplicación). Podés inyectarla por config si querés.
        private readonly string _encryptionKey;

        public EncriptacionService(string encryptionKey = null)
        {
            _encryptionKey = encryptionKey ?? "GestorMerchandising2024!Key";
        }

        /// <summary>
        /// Genera SHA-256(UTF-8) en HEX MAYÚSCULA usando pepper (password + _encryptionKey).
        /// Este es el formato "oficial" que vamos a guardar en DB.
        /// </summary>
        public string GenerarHashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));

            var salted = password + _encryptionKey;                // conserva tu lógica de sal/pepper
            var bytes = Sha256(Encoding.UTF8.GetBytes(salted));
            return ToHexUpper(bytes);                              // HEX MAYÚSCULA
        }

        /// <summary>
        /// Verifica primero contra el formato oficial (con pepper) y,
        /// si no coincide, intenta modo legacy sin pepper (SHA-256 puro),
        /// comparando en forma case-insensitive.
        /// </summary>
        public bool VerificarPassword(string password, string hashGuardado)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashGuardado))
                return false;

            // 1) Oficial: salted (pepper) + UTF-8 + HEX
            var oficial = GenerarHashPassword(password);
            if (string.Equals(oficial, hashGuardado, StringComparison.OrdinalIgnoreCase))
                return true;

            // 2) Legacy: sin pepper (útil para usuarios viejos insertados por SQL)
            var legacy = Sha256HexUpper(password);
            if (string.Equals(legacy, hashGuardado, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        // ================== Helpers ==================

        private static byte[] Sha256(byte[] data)
        {
            using (var sha = SHA256.Create())
                return sha.ComputeHash(data);
        }

        private static string ToHexUpper(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes) sb.Append(b.ToString("X2"));   // MAYÚSCULA
            return sb.ToString();
        }

        /// <summary>
        /// SHA-256(UTF-8) en HEX MAYÚSCULA sin pepper (compatibilidad).
        /// </summary>
        private static string Sha256HexUpper(string plain)
        {
            var bytes = Sha256(Encoding.UTF8.GetBytes(plain));
            return ToHexUpper(bytes);
        }
    }
}