using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Services.DAL.Ef.Base;
using Services.DomainModel.Entities;

namespace Services.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ServicesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Services.DAL.Ef.Base.ServicesContext";
        }

        protected override void Seed(ServicesContext context)
        {
            context.InicializarDatos();

            var funcionesPorDefecto = ObtenerFuncionesPorDefecto();
            foreach (var funcion in funcionesPorDefecto)
            {
                context.Funciones.AddOrUpdate(f => f.IdFuncion, funcion);
            }

            context.SaveChanges();

            var perfiles = context.Perfiles.Include(p => p.Funciones).ToList();
            if (!perfiles.Any())
            {
                return;
            }

            var funciones = context.Funciones.ToDictionary(f => f.Codigo, StringComparer.OrdinalIgnoreCase);
            foreach (var asignacion in ObtenerAsignacionesFunciones())
            {
                var perfil = perfiles.FirstOrDefault(p => p.IdPerfil == asignacion.Key);
                if (perfil == null)
                {
                    continue;
                }

                foreach (var codigo in asignacion.Value)
                {
                    if (!funciones.TryGetValue(codigo, out var funcion))
                    {
                        continue;
                    }

                    if (perfil.Funciones.All(f => f.IdFuncion != funcion.IdFuncion))
                    {
                        perfil.Funciones.Add(funcion);
                    }
                }
            }

            context.SaveChanges();
        }

        private static IEnumerable<Funcion> ObtenerFuncionesPorDefecto()
        {
            yield return new Funcion
            {
                IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Codigo = "SEG_PERFILES",
                Nombre = "Administrar perfiles",
                Descripcion = "Acceso al módulo de perfiles",
                Activo = true
            };

            yield return new Funcion
            {
                IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                Codigo = "SEG_USUARIOS",
                Nombre = "Administrar usuarios",
                Descripcion = "Acceso al módulo de usuarios",
                Activo = true
            };

            yield return new Funcion
            {
                IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                Codigo = "CAT_CLIENTES",
                Nombre = "Gestión de clientes",
                Descripcion = "Acceso al mantenimiento de clientes",
                Activo = true
            };

            yield return new Funcion
            {
                IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                Codigo = "CAT_PROVEEDORES",
                Nombre = "Gestión de proveedores",
                Descripcion = "Acceso al mantenimiento de proveedores",
                Activo = true
            };

            yield return new Funcion
            {
                IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                Codigo = "CAT_PRODUCTOS",
                Nombre = "Gestión de productos",
                Descripcion = "Acceso al mantenimiento de productos",
                Activo = true
            };

            yield return new Funcion
            {
                IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000006"),
                Codigo = "PEDIDOS_VENTAS",
                Nombre = "Gestión de pedidos",
                Descripcion = "Acceso al módulo de pedidos",
                Activo = true
            };

            yield return new Funcion
            {
                IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000007"),
                Codigo = "PEDIDOS_MUESTRAS",
                Nombre = "Gestión de pedidos de muestra",
                Descripcion = "Acceso al módulo de pedidos de muestra",
                Activo = true
            };

            yield return new Funcion
            {
                IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000008"),
                Codigo = "REPORTES_OPERATIVOS",
                Nombre = "Reportes operativos",
                Descripcion = "Acceso a reportes operativos",
                Activo = true
            };

            yield return new Funcion
            {
                IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000009"),
                Codigo = "REPORTES_VENTAS",
                Nombre = "Reportes de ventas",
                Descripcion = "Acceso a reportes de ventas",
                Activo = true
            };

            yield return new Funcion
            {
                IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000010"),
                Codigo = "REPORTES_FINANCIEROS",
                Nombre = "Reportes financieros",
                Descripcion = "Acceso a reportes financieros",
                Activo = true
            };
        }

        private static IReadOnlyDictionary<Guid, string[]> ObtenerAsignacionesFunciones()
        {
            return new Dictionary<Guid, string[]>
            {
                {
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    new[]
                    {
                        "SEG_PERFILES",
                        "SEG_USUARIOS",
                        "CAT_CLIENTES",
                        "CAT_PROVEEDORES",
                        "CAT_PRODUCTOS",
                        "PEDIDOS_VENTAS",
                        "PEDIDOS_MUESTRAS",
                        "REPORTES_OPERATIVOS",
                        "REPORTES_VENTAS",
                        "REPORTES_FINANCIEROS"
                    }
                },
                {
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    new[]
                    {
                        "CAT_CLIENTES",
                        "CAT_PROVEEDORES",
                        "CAT_PRODUCTOS",
                        "PEDIDOS_VENTAS",
                        "PEDIDOS_MUESTRAS",
                        "REPORTES_OPERATIVOS",
                        "REPORTES_VENTAS"
                    }
                },
                {
                    Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    new[]
                    {
                        "PEDIDOS_VENTAS",
                        "REPORTES_FINANCIEROS",
                        "REPORTES_OPERATIVOS"
                    }
                },
                {
                    Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    new[]
                    {
                        "CAT_CLIENTES",
                        "PEDIDOS_VENTAS",
                        "PEDIDOS_MUESTRAS"
                    }
                }
            };
        }
    }
}