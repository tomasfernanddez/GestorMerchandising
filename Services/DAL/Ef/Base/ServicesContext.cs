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
        public ServicesContext() : base("GestorMerchandisingDB")
        {
            Database.SetInitializer<ServicesContext>(null);
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public ServicesContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer<ServicesContext>(null);
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet<Bitacora> Bitacoras { get; set; }
        public virtual DbSet<Sesion> Sesiones { get; set; }

        //    ============================================================================
        // Configuración del modelo
        //    ============================================================================
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

        }

        //    ============================================================================
        // INICIALIZACIÓN DE DATOS - MÉTODO PARA EF6
        //    ============================================================================
        public void InicializarDatos()
        {

            // Verificar si ya existen datos para no duplicar
            if (Perfiles.Any()) return;

            // Crear perfiles básicos del sistema
            var perfiles = new List<Perfil>
        {
            new Perfil
            {
                IdPerfil = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                NombrePerfil = "Administrador",
                Descripcion = "Acceso completo al sistema",
                Activo = true
            },
            new Perfil
            {
                IdPerfil = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                NombrePerfil = "Gerente de Ventas",
                Descripcion = "Gestión de clientes, pedidos y ventas",
                Activo = true
            },
            new Perfil
            {
                IdPerfil = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                NombrePerfil = "Gerente de Finanzas",
                Descripcion = "Gestión financiera y facturación",
                Activo = true
            },
            new Perfil
            {
                IdPerfil = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                NombrePerfil = "Vendedor",
                Descripcion = "Gestión básica de pedidos y clientes",
                Activo = true
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