using System;
using System.Collections.Generic;
using System.Linq;
using Services.DomainModel.Entities;

namespace Services
{
    public static class SessionContext
    {
        private static readonly Guid PerfilAdministradorId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public static Guid IdUsuario { get; private set; }
        public static string NombreUsuario { get; private set; }
        public static string NombreCompleto { get; private set; }
        public static string Email { get; private set; }
        public static Guid IdPerfil { get; private set; }
        public static string NombrePerfil { get; private set; }
        public static bool EsAdministrador { get; private set; }
        public static string IdiomaPreferido { get; private set; }

        private static readonly HashSet<string> _funciones = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public static IReadOnlyCollection<string> Funciones => _funciones;

        public static bool IsAuthenticated => IdUsuario != Guid.Empty;

        public static void InicializarSesion(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            IdUsuario = usuario.IdUsuario;
            NombreUsuario = usuario.NombreUsuario;
            NombreCompleto = usuario.NombreCompleto;
            Email = usuario.Email;
            IdPerfil = usuario.IdPerfil;
            NombrePerfil = usuario.Perfil?.NombrePerfil?.Trim() ?? string.Empty;
            IdiomaPreferido = string.IsNullOrWhiteSpace(usuario.IdiomaPreferido) ? null : usuario.IdiomaPreferido.Trim();

            EsAdministrador = DeterminarSiEsAdministrador(usuario.Perfil);

            _funciones.Clear();

            if (usuario.Perfil?.Funciones != null)
            {
                foreach (var funcion in usuario.Perfil.Funciones)
                {
                    if (funcion?.Activo != true)
                    {
                        continue;
                    }

                    var codigoNormalizado = funcion.Codigo?.Trim();
                    if (!string.IsNullOrWhiteSpace(codigoNormalizado))
                    {
                        _funciones.Add(codigoNormalizado);
                    }
                }
            }
        }

        public static bool TieneFuncion(string codigoFuncion)
        {
            if (EsAdministrador)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(codigoFuncion))
            {
                return false;
            }

            return _funciones.Contains(codigoFuncion.Trim());
        }

        public static IEnumerable<string> ObtenerFunciones()
        {
            return _funciones.ToList();
        }

        public static void CerrarSesion()
        {
            IdUsuario = Guid.Empty;
            NombreUsuario = string.Empty;
            NombreCompleto = string.Empty;
            Email = string.Empty;
            IdPerfil = Guid.Empty;
            NombrePerfil = string.Empty;
            IdiomaPreferido = null;
            EsAdministrador = false;
            _funciones.Clear();
        }

        public static void ActualizarIdioma(string idioma)
        {
            IdiomaPreferido = string.IsNullOrWhiteSpace(idioma) ? null : idioma.Trim();
        }

        // Métodos de compatibilidad con la implementación previa -----------------------
        public static void Set(Usuario usuario) => InicializarSesion(usuario);

        public static void Clear() => CerrarSesion();

        private static bool DeterminarSiEsAdministrador(Perfil perfil)
        {
            if (perfil == null)
            {
                return false;
            }

            if (perfil.IdPerfil == PerfilAdministradorId)
            {
                return true;
            }

            if (perfil is object)
            {
                var nombreNormalizado = perfil.NombrePerfil?.Trim();
                if (!string.IsNullOrWhiteSpace(nombreNormalizado) &&
                    string.Equals(nombreNormalizado, "Administrador", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                var propiedadAdmin = perfil.GetType().GetProperty("EsAdministrador");
                if (propiedadAdmin != null && propiedadAdmin.PropertyType == typeof(bool))
                {
                    var valor = propiedadAdmin.GetValue(perfil);
                    if (valor is bool esAdmin && esAdmin)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}