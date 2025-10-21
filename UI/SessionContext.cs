using System;
using DomainModel;
using DomainModel.Entidades;

namespace UI
{
    public static class SessionContext
    {
        public static Guid IdUsuario { get; private set; }
        public static string NombreUsuario { get; private set; }
        public static string NombreCompleto { get; private set; }
        public static Guid IdPerfil { get; private set; }
        public static string NombrePerfil { get; private set; }

        public static void Set(Usuario u)
        {
            if (u == null) return;
            IdUsuario = u.IdUsuario;
            NombreUsuario = u.NombreUsuario;
            NombreCompleto = u.NombreCompleto;
            IdPerfil = u.IdPerfil;
            NombrePerfil = u.Perfil != null ? u.Perfil.NombrePerfil : null;
        }

        public static void Clear()
        {
            IdUsuario = Guid.Empty;
            IdPerfil = Guid.Empty;
            NombreUsuario = NombreCompleto = NombrePerfil = null;
        }

        public static bool IsAuthenticated => IdUsuario != Guid.Empty;
    }
}