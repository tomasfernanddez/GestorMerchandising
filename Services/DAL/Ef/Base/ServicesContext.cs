using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.DAL.Ef.Base
{
    public class ServicesContext : DbContext
    {
        // Constructor - usa el connection string "GestorMerchandisingDB"
        /// <summary>
        /// Crea un contexto utilizando el connection string por defecto de la aplicación.
        /// </summary>
        public ServicesContext() : base("GestorMerchandisingDB")
        {
            Database.SetInitializer<ServicesContext>(null);
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        /// <summary>
        /// Crea un contexto utilizando el connection string proporcionado.
        /// </summary>
        public ServicesContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer<ServicesContext>(null);
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet<Funcion> Funciones { get; set; }
        public virtual DbSet<Bitacora> Bitacoras { get; set; }
        public virtual DbSet<Sesion> Sesiones { get; set; }

        //    ============================================================================
        // Configuración del modelo
        //    ============================================================================
        /// <summary>
        /// Configura el modelo de Entity Framework para las entidades del módulo de seguridad.
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Remover convenciones que no queremos
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //        ============================================================================
            // CONFIGURACIÓN DE ARQUITECTURA BASE (NUEVAS)
            //        ============================================================================

            // Usuario -> Perfil
            modelBuilder.Entity<Usuario>()
                .HasRequired(u => u.Perfil)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(u => u.IdPerfil)
                .WillCascadeOnDelete(false);

            // Usuario -> Bitacoras
            modelBuilder.Entity<Bitacora>()
                .HasRequired(b => b.Usuario)
                .WithMany(u => u.BitacoraRegistros)
                .HasForeignKey(b => b.IdUsuario)
                .WillCascadeOnDelete(false);

            // Usuario -> Sesiones
            modelBuilder.Entity<Sesion>()
                .HasRequired(s => s.Usuario)
                .WithMany()
                .HasForeignKey(s => s.IdUsuario)
                .WillCascadeOnDelete(false);

            // Perfil -> Funciones (many-to-many)
            modelBuilder.Entity<Perfil>()
                .HasMany(p => p.Funciones)
                .WithMany(f => f.Perfiles)
                .Map(m =>
                {
                    m.ToTable("PerfilFuncion");
                    m.MapLeftKey("IdPerfil");
                    m.MapRightKey("IdFuncion");
                });

        }

        //    ============================================================================
        // INICIALIZACIÓN DE DATOS - MÉTODO PARA EF6
        //    ============================================================================
        /// <summary>
        /// Carga los datos iniciales necesarios para el funcionamiento del módulo de seguridad.
        /// </summary>
        public void InicializarDatos()
        {

            var hayFunciones = Funciones.Any();
            if (!hayFunciones)
            {
                var funciones = new List<Funcion>
                {
                    new Funcion
                    {
                        IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                        Codigo = "SEG_PERFILES",
                        Nombre = "Administrar perfiles",
                        Descripcion = "Acceso al módulo de perfiles",
                        Activo = true
                    },
                    new Funcion
                    {
                        IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                        Codigo = "SEG_USUARIOS",
                        Nombre = "Administrar usuarios",
                        Descripcion = "Acceso al módulo de usuarios",
                        Activo = true
                    },
                    new Funcion
                    {
                        IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                        Codigo = "CAT_CLIENTES",
                        Nombre = "Gestión de clientes",
                        Descripcion = "Acceso al mantenimiento de clientes",
                        Activo = true
                    },
                    new Funcion
                    {
                        IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                        Codigo = "CAT_PROVEEDORES",
                        Nombre = "Gestión de proveedores",
                        Descripcion = "Acceso al mantenimiento de proveedores",
                        Activo = true
                    },
                    new Funcion
                    {
                        IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                        Codigo = "CAT_PRODUCTOS",
                        Nombre = "Gestión de productos",
                        Descripcion = "Acceso al mantenimiento de productos",
                        Activo = true
                    },
                    new Funcion
                    {
                        IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000006"),
                        Codigo = "PEDIDOS_VENTAS",
                        Nombre = "Gestión de pedidos",
                        Descripcion = "Acceso al módulo de pedidos",
                        Activo = true
                    },
                    new Funcion
                    {
                        IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000007"),
                        Codigo = "PEDIDOS_MUESTRAS",
                        Nombre = "Gestión de pedidos de muestra",
                        Descripcion = "Acceso al módulo de pedidos de muestra",
                        Activo = true
                    },
                    new Funcion
                    {
                        IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000008"),
                        Codigo = "REPORTES_OPERATIVOS",
                        Nombre = "Reportes operativos",
                        Descripcion = "Acceso a reportes operativos",
                        Activo = true
                    },
                    new Funcion
                    {
                        IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000009"),
                        Codigo = "REPORTES_VENTAS",
                        Nombre = "Reportes de ventas",
                        Descripcion = "Acceso a reportes de ventas",
                        Activo = true
                    },
                    new Funcion
                    {
                        IdFuncion = Guid.Parse("10000000-0000-0000-0000-000000000010"),
                        Codigo = "REPORTES_FINANCIEROS",
                        Nombre = "Reportes financieros",
                        Descripcion = "Acceso a reportes financieros",
                        Activo = true
                    }
                };

                Funciones.AddRange(funciones);
                SaveChanges();
            }

            if (Perfiles.Any()) return;

            var funcionesDisponibles = Funciones.ToList();

            // Crear perfiles básicos del sistema
            var perfiles = new List<Perfil>
            {
                new Perfil
                {
                    IdPerfil = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    NombrePerfil = "Administrador",
                    Descripcion = "Acceso completo al sistema",
                    Activo = true,
                    Funciones = funcionesDisponibles.ToList()
                },
                new Perfil
                {
                    IdPerfil = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    NombrePerfil = "Gerente de Ventas",
                    Descripcion = "Gestión de clientes, pedidos y ventas",
                    Activo = true,
                    Funciones = funcionesDisponibles.Where(f =>
                        f.Codigo == "CAT_CLIENTES" ||
                        f.Codigo == "CAT_PROVEEDORES" ||
                        f.Codigo == "CAT_PRODUCTOS" ||
                        f.Codigo == "PEDIDOS_VENTAS" ||
                        f.Codigo == "PEDIDOS_MUESTRAS" ||
                        f.Codigo == "REPORTES_OPERATIVOS" ||
                        f.Codigo == "REPORTES_VENTAS").ToList()
                },
                new Perfil
                {
                    IdPerfil = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    NombrePerfil = "Gerente de Finanzas",
                    Descripcion = "Gestión financiera y facturación",
                    Activo = true,
                    Funciones = funcionesDisponibles.Where(f =>
                        f.Codigo == "PEDIDOS_VENTAS" ||
                        f.Codigo == "REPORTES_FINANCIEROS" ||
                        f.Codigo == "REPORTES_OPERATIVOS").ToList()
                },
                new Perfil
                {
                    IdPerfil = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    NombrePerfil = "Vendedor",
                    Descripcion = "Gestión básica de pedidos y clientes",
                    Activo = true,
                    Funciones = funcionesDisponibles.Where(f =>
                        f.Codigo == "CAT_CLIENTES" ||
                        f.Codigo == "PEDIDOS_VENTAS" ||
                        f.Codigo == "PEDIDOS_MUESTRAS").ToList()
                }
            };

            // Agregar perfiles
            Perfiles.AddRange(perfiles);
            SaveChanges();

            // Crear usuario administrador por defecto
            var usuarioAdmin = new Usuario
            {
                IdUsuario = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                NombreUsuario = "admin",
                PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", // Hash de "admin"
                NombreCompleto = "Administrador del Sistema",
                Email = "admin@gestormerchandising.com",
                Activo = true,
                FechaCreacion = DateTime.Now,
                IdPerfil = Guid.Parse("11111111-1111-1111-1111-111111111111")
            };

            Usuarios.Add(usuarioAdmin);
            SaveChanges();
        }
    }
}