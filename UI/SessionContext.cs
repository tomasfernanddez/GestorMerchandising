using System;
using Services.DomainModel.Entities;

namespace UI
{
    public static class SessionContext
    {
        public static Guid IdUsuario { get; private set; }
        public static string NombreUsuario { get; private set; }
        public static string NombreCompleto { get; private set; }
        public static Guid IdPerfil { get; private set; }
        public static string NombrePerfil { get; private set; }
        public static string IdiomaPreferido { get; private set; }

        public static void Set(Usuario u)
        {
            if (u == null) return;
            IdUsuario = u.IdUsuario;
            NombreUsuario = u.NombreUsuario;
            NombreCompleto = u.NombreCompleto;
            IdPerfil = u.IdPerfil;
            NombrePerfil = u.Perfil != null ? u.Perfil.NombrePerfil : null;
            IdiomaPreferido = string.IsNullOrWhiteSpace(u.IdiomaPreferido) ? null : u.IdiomaPreferido;
        }

        public static void Clear()
        {
            IdUsuario = Guid.Empty;
            IdPerfil = Guid.Empty;
            NombreUsuario = NombreCompleto = NombrePerfil = null;
            IdiomaPreferido = null;
        }

        public static bool IsAuthenticated => IdUsuario != Guid.Empty;

        public static void ActualizarIdioma(string idioma)
        {
            IdiomaPreferido = string.IsNullOrWhiteSpace(idioma) ? null : idioma;
        }
    }
}