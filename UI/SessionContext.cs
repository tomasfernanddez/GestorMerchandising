using System;
using System.Collections.Generic;
using System.Linq;
using Services.BLL.Factories;
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
        public static IReadOnlyCollection<string> Funciones => _funciones;

        private static readonly HashSet<string> _funciones = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public static void Set(Usuario u)
        {
            if (u == null) return;
            IdUsuario = u.IdUsuario;
            NombreUsuario = u.NombreUsuario;
            NombreCompleto = u.NombreCompleto;
            IdPerfil = u.IdPerfil;
            NombrePerfil = u.Perfil != null ? u.Perfil.NombrePerfil : null;
            IdiomaPreferido = string.IsNullOrWhiteSpace(u.IdiomaPreferido) ? null : u.IdiomaPreferido;

            _funciones.Clear();

            IEnumerable<Funcion> funcionesPerfil = u.Perfil?.Funciones;

            if ((funcionesPerfil == null || !funcionesPerfil.Any()) || string.IsNullOrWhiteSpace(NombrePerfil))
            {
                try
                {
                    var usuarioSvc = ServicesFactory.CrearUsuarioService();
                    var usuarioCompleto = usuarioSvc.ObtenerPorId(u.IdUsuario);

                    if (usuarioCompleto?.Perfil != null)
                    {
                        funcionesPerfil = usuarioCompleto.Perfil.Funciones;

                        if (string.IsNullOrWhiteSpace(NombrePerfil))
                        {
                            NombrePerfil = usuarioCompleto.Perfil.NombrePerfil;
                        }
                    }
                }
                catch
                {
                    funcionesPerfil = funcionesPerfil ?? Enumerable.Empty<Funcion>();
                }
            }

            if (funcionesPerfil != null)
            {
                foreach (var funcion in funcionesPerfil.Where(f => f != null && f.Activo && !string.IsNullOrWhiteSpace(f.Codigo)))
                {
                    _funciones.Add(funcion.Codigo);
                }
            }
        }

        public static void Clear()
        {
            IdUsuario = Guid.Empty;
            IdPerfil = Guid.Empty;
            NombreUsuario = NombreCompleto = NombrePerfil = null;
            IdiomaPreferido = null;
            _funciones.Clear();
        }

        public static bool IsAuthenticated => IdUsuario != Guid.Empty;

        public static void ActualizarIdioma(string idioma)
        {
            IdiomaPreferido = string.IsNullOrWhiteSpace(idioma) ? null : idioma;
        }

        public static bool TieneFuncion(string codigoFuncion)
        {
            if (string.IsNullOrWhiteSpace(codigoFuncion))
                return false;

            return _funciones.Contains(codigoFuncion);
        }
    }
}