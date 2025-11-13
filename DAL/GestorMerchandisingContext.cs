using DomainModel.Entidades;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class GestorMerchandisingContext : DbContext
    {
        // Constructor - usa el connection string "GestorMerchandisingDB"
        public GestorMerchandisingContext() : base("GestorMerchandisingDB")
        {
            Database.SetInitializer<GestorMerchandisingContext>(null);
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public GestorMerchandisingContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer<GestorMerchandisingContext>(null);
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public DbSet<Pais> Paises { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Localidad> Localidades { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Proveedor> Proveedores { get; set; }
        public virtual DbSet<CondicionIva> CondicionesIva { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }
        public virtual DbSet<PedidoEstadoHistorial> PedidosEstadoHistorial { get; set; }
        public virtual DbSet<PedidoNota> PedidoNotas { get; set; }
        public virtual DbSet<PedidoPago> PedidoPagos { get; set; }
        public virtual DbSet<PedidoMuestraPago> PedidoMuestraPagos { get; set; }
        public virtual DbSet<PedidoMuestra> PedidosMuestra { get; set; }
        public virtual DbSet<DetalleMuestra> DetalleMuestras { get; set; }
        public virtual DbSet<ArchivoAdjunto> ArchivosAdjuntos { get; set; }
        public virtual DbSet<FacturaCabecera> FacturasCabecera { get; set; }
        public virtual DbSet<FacturaDetalle> FacturasDetalle { get; set; }
        public virtual DbSet<LogosPedido> LogosPedidos { get; set; }
        public virtual DbSet<EmisorFactura> EmisoresFactura { get; set; }
        public virtual DbSet<TipoEmpresa> TiposEmpresa { get; set; }
        public virtual DbSet<TipoProveedor> TiposProveedor { get; set; }
        public virtual DbSet<CategoriaProducto> CategoriasProducto { get; set; }
        public virtual DbSet<UnidadMedida> UnidadesMedida { get; set; }
        public virtual DbSet<EstadoPedido> EstadosPedido { get; set; }
        public virtual DbSet<EstadoProducto> EstadosProducto { get; set; }
        public virtual DbSet<EstadoPedidoMuestra> EstadosPedidoMuestra { get; set; }
        public virtual DbSet<EstadoMuestra> EstadosMuestra { get; set; }
        public virtual DbSet<TipoPago> TiposPago { get; set; }
        public virtual DbSet<TecnicaPersonalizacion> TecnicasPersonalizacion { get; set; }
        public virtual DbSet<UbicacionLogo> UbicacionesLogo { get; set; }

        // Patrón Composite - Personalización
        public virtual DbSet<GrupoPersonalizacion> GruposPersonalizacion { get; set; }
        public virtual DbSet<GrupoPersonalizacionLogo> GruposPersonalizacionLogos { get; set; }

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
        // CONFIGURACIÓN DE COLUMNAS CALCULADAS (EXISTENTES)
        //        ============================================================================

        // PedidoDetalle - Subtotal calculado
        modelBuilder.Entity<PedidoDetalle>()
            .Property(pd => pd.Subtotal)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            // FacturaDetalle - Subtotal calculado
            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.Subtotal)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            // FacturaCabecera - MontoIVA y MontoTotal calculados
            modelBuilder.Entity<FacturaCabecera>()
                .Property(fc => fc.MontoIVA)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<FacturaCabecera>()
                .Property(fc => fc.MontoTotal)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

        //        ============================================================================
        // Configuración de relaciones principales (EXISTENTES)
        //        ============================================================================

        // Cliente -> Pedidos
        modelBuilder.Entity<Pedido>()
            .HasRequired(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.IdCliente)
            .WillCascadeOnDelete(false);

            // Cliente -> PedidosMuestra
            modelBuilder.Entity<PedidoMuestra>()
                .HasRequired(pm => pm.Cliente)
                .WithMany(c => c.PedidosMuestra)
                .HasForeignKey(pm => pm.IdCliente)
                .WillCascadeOnDelete(false);

            // Cliente -> Facturas
            modelBuilder.Entity<FacturaCabecera>()
                .HasRequired(f => f.Cliente)
                .WithMany(c => c.Facturas)
                .HasForeignKey(f => f.IdCliente)
                .WillCascadeOnDelete(false);

            // Proveedor -> Productos
            modelBuilder.Entity<Producto>()
                .HasOptional(p => p.Proveedor)
                .WithMany(pr => pr.Productos)
                .HasForeignKey(p => p.IdProveedor)
                .WillCascadeOnDelete(false);

            // Pedido -> PedidoDetalles
            modelBuilder.Entity<PedidoDetalle>()
                .HasRequired(pd => pd.Pedido)
                .WithMany(p => p.Detalles)
                .HasForeignKey(pd => pd.IdPedido)
                .WillCascadeOnDelete(false);

            // Producto -> PedidoDetalles
            modelBuilder.Entity<PedidoDetalle>()
                .HasRequired(pd => pd.Producto)
                .WithMany(p => p.PedidoDetalles)
                .HasForeignKey(pd => pd.IdProducto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PedidoEstadoHistorial>()
                .HasRequired(ph => ph.Pedido)
                .WithMany(p => p.HistorialEstados)
                .HasForeignKey(ph => ph.IdPedido)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PedidoEstadoHistorial>()
                .HasRequired(ph => ph.EstadoPedido)
                .WithMany()
                .HasForeignKey(ph => ph.IdEstadoPedido)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PedidoNota>()
                .HasRequired(pn => pn.Pedido)
                .WithMany(p => p.Notas)
                .HasForeignKey(pn => pn.IdPedido)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PedidoPago>()
                .HasRequired(pp => pp.Pedido)
                .WithMany(p => p.Pagos)
                .HasForeignKey(pp => pp.IdPedido)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PedidoPago>()
                .Property(pp => pp.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PedidoPago>()
                .Property(pp => pp.Porcentaje)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PedidoMuestraPago>()
               .HasRequired(pp => pp.PedidoMuestra)
               .WithMany(pm => pm.Pagos)
               .HasForeignKey(pp => pp.IdPedidoMuestra)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<PedidoMuestraPago>()
                .Property(pp => pp.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PedidoMuestraPago>()
                .Property(pp => pp.Porcentaje)
                .HasPrecision(18, 2);

            // PedidoMuestra -> DetalleMuestras
            modelBuilder.Entity<DetalleMuestra>()
                .HasRequired(dm => dm.PedidoMuestra)
                .WithMany(pm => pm.Detalles)
                .HasForeignKey(dm => dm.IdPedidoMuestra)
                .WillCascadeOnDelete(false);

            // Producto -> DetalleMuestras
            modelBuilder.Entity<DetalleMuestra>()
                .HasRequired(dm => dm.Producto)
                .WithMany(p => p.DetalleMuestras)
                .HasForeignKey(dm => dm.IdProducto)
                .WillCascadeOnDelete(false);

            // FacturaCabecera -> FacturaDetalles
            modelBuilder.Entity<FacturaDetalle>()
                .HasRequired(fd => fd.Factura)
                .WithMany(f => f.Detalles)
                .HasForeignKey(fd => fd.IdFactura)
                .WillCascadeOnDelete(false);

            // Producto -> FacturaDetalles
            modelBuilder.Entity<FacturaDetalle>()
                .HasRequired(fd => fd.Producto)
                .WithMany(p => p.FacturaDetalles)
                .HasForeignKey(fd => fd.IdProducto)
                .WillCascadeOnDelete(false);

            // PedidoDetalle -> LogosPedido
            modelBuilder.Entity<LogosPedido>()
                .HasRequired(lp => lp.DetallePedido)
                .WithMany(pd => pd.LogosPedido)
                .HasForeignKey(lp => lp.IdDetallePedido)
                .WillCascadeOnDelete(false);

            // Archivos adjuntos -> Pedido / PedidoMuestra
            modelBuilder.Entity<ArchivoAdjunto>()
                .HasOptional(a => a.Pedido)
                .WithMany(p => p.Adjuntos)
                .HasForeignKey(a => a.IdPedido)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ArchivoAdjunto>()
                .HasOptional(a => a.PedidoMuestra)
                .WithMany(pm => pm.Adjuntos)
                .HasForeignKey(a => a.IdPedidoMuestra)
                .WillCascadeOnDelete(false);

            // EmisorFactura -> FacturasCabecera
            modelBuilder.Entity<FacturaCabecera>()
                .HasRequired(f => f.Emisor)
                .WithMany(e => e.Facturas)
                .HasForeignKey(f => f.IdEmisor)
                .WillCascadeOnDelete(false);

            //        ============================================================================
            // Configuración de relaciones opcionales (Foreign Keys opcionales) (EXISTENTES)
            //        ============================================================================

            // Cliente -> Condicion IVA
            modelBuilder.Entity<Cliente>()
                .HasRequired(c => c.CondicionIva)
                .WithMany(ci => ci.Clientes)
                .HasForeignKey(c => c.IdCondicionIva)
                .WillCascadeOnDelete(false);

            // Cliente -> TipoEmpresa
            modelBuilder.Entity<Cliente>()
                .HasOptional(c => c.TipoEmpresa)
                .WithMany(te => te.Clientes)
                .HasForeignKey(c => c.IdTipoEmpresa);

            // Proveedor -> Tipos de proveedor (Many-to-Many)
            modelBuilder.Entity<Proveedor>()
                .HasMany(p => p.TiposProveedor)
                .WithMany(tp => tp.Proveedores)
                .Map(m =>
                {
                    m.ToTable("ProveedorTipoProveedor");
                    m.MapLeftKey("IdProveedor");
                    m.MapRightKey("IdTipoProveedor");
                });

            // Proveedor -> Condicion IVA
            modelBuilder.Entity<Proveedor>()
                .HasRequired(p => p.CondicionIva)
                .WithMany(ci => ci.Proveedores)
                .HasForeignKey(p => p.IdCondicionIva)
                .WillCascadeOnDelete(false);

            // Proveedor -> País / Provincia / Localidad (opcionales)
            modelBuilder.Entity<Proveedor>()
                .HasOptional(p => p.Pais)
                .WithMany()
                .HasForeignKey(p => p.IdPais);

            modelBuilder.Entity<Proveedor>()
                .HasOptional(p => p.Provincia)
                .WithMany()
                .HasForeignKey(p => p.IdProvincia);

            modelBuilder.Entity<Proveedor>()
                .HasOptional(p => p.LocalidadRef)
                .WithMany()
                .HasForeignKey(p => p.IdLocalidad);

            // Proveedor -> Técnicas de personalización (Many-to-Many)
            modelBuilder.Entity<Proveedor>()
                .HasMany(p => p.TecnicasPersonalizacion)
                .WithMany(t => t.Proveedores)
                .Map(m =>
                {
                    m.ToTable("ProveedorTecnicaPersonalizacion");
                    m.MapLeftKey("IdProveedor");
                    m.MapRightKey("IdTecnicaPersonalizacion");
                });

            // Producto -> CategoriaProducto
            modelBuilder.Entity<Producto>()
                .HasOptional(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.IdCategoria);

            // Producto -> UnidadMedida
            modelBuilder.Entity<Producto>()
                .HasOptional(p => p.UnidadMedida)
                .WithMany(um => um.Productos)
                .HasForeignKey(p => p.IdUnidadMedida);

            // Pedido -> EstadoPedido
            modelBuilder.Entity<Pedido>()
                .HasOptional(p => p.EstadoPedido)
                .WithMany(ep => ep.Pedidos)
                .HasForeignKey(p => p.IdEstadoPedido);

            // Pedido -> TipoPago
            modelBuilder.Entity<Pedido>()
                .HasOptional(p => p.TipoPago)
                .WithMany(tp => tp.Pedidos)
                .HasForeignKey(p => p.IdTipoPago);

            // PedidoDetalle -> EstadoProducto
            modelBuilder.Entity<PedidoDetalle>()
                .HasOptional(pd => pd.EstadoProducto)
                .WithMany(ep => ep.PedidoDetalles)
                .HasForeignKey(pd => pd.IdEstadoProducto);

            // PedidoMuestra -> EstadoPedidoMuestra
            modelBuilder.Entity<PedidoMuestra>()
                .HasOptional(pm => pm.EstadoPedidoMuestra)
                .WithMany(epm => epm.PedidosMuestra)
                .HasForeignKey(pm => pm.IdEstadoPedidoMuestra);

            // DetalleMuestra -> EstadoMuestra
            modelBuilder.Entity<DetalleMuestra>()
                .HasOptional(dm => dm.EstadoMuestra)
                .WithMany(em => em.DetalleMuestras)
                .HasForeignKey(dm => dm.IdEstadoMuestra);

            // LogosPedido -> TecnicaPersonalizacion
            modelBuilder.Entity<LogosPedido>()
                .HasOptional(lp => lp.TecnicaPersonalizacion)
                .WithMany(tp => tp.LogosPedido)
                .HasForeignKey(lp => lp.IdTecnicaPersonalizacion);

            // LogosPedido -> UbicacionLogo
            modelBuilder.Entity<LogosPedido>()
                .HasOptional(lp => lp.UbicacionLogo)
                .WithMany(ul => ul.LogosPedido)
                .HasForeignKey(lp => lp.IdUbicacionLogo);

        //        ============================================================================
        // Configuración del Patrón Composite - Personalización
        //        ============================================================================

        // GrupoPersonalizacion -> PedidoDetalle
        modelBuilder.Entity<GrupoPersonalizacion>()
            .HasRequired(gp => gp.DetallePedido)
            .WithMany()
            .HasForeignKey(gp => gp.IdDetallePedido)
            .WillCascadeOnDelete(false);

            // GrupoPersonalizacion -> GrupoPadre (auto-referencia)
            modelBuilder.Entity<GrupoPersonalizacion>()
                .HasOptional(gp => gp.GrupoPadre)
                .WithMany(gp => gp.GruposHijos)
                .HasForeignKey(gp => gp.IdGrupoPadre)
                .WillCascadeOnDelete(false);

            // GrupoPersonalizacionLogo - Relación many-to-many con clave compuesta
            modelBuilder.Entity<GrupoPersonalizacionLogo>()
                .HasKey(gpl => new { gpl.IdGrupo, gpl.IdLogo });

            modelBuilder.Entity<GrupoPersonalizacionLogo>()
                .HasRequired(gpl => gpl.Grupo)
                .WithMany(gp => gp.LogosAsociados)
                .HasForeignKey(gpl => gpl.IdGrupo)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<GrupoPersonalizacionLogo>()
                .HasRequired(gpl => gpl.Logo)
                .WithMany()
                .HasForeignKey(gpl => gpl.IdLogo)
                .WillCascadeOnDelete(true);

            // Catálogos
            modelBuilder.Entity<Pais>()
                .HasKey(p => p.IdPais)
                .Property(p => p.Nombre).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Provincia>()
                .HasKey(p => p.IdProvincia);
            modelBuilder.Entity<Provincia>()
                .HasRequired(p => p.Pais)
                .WithMany(p => p.Provincias)
                .HasForeignKey(p => p.IdPais);
            modelBuilder.Entity<Provincia>()
                .Property(p => p.Nombre).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Localidad>()
                .HasKey(l => l.IdLocalidad);
            modelBuilder.Entity<Localidad>()
                .HasRequired(l => l.Provincia)
                .WithMany(p => p.Localidades)
                .HasForeignKey(l => l.IdProvincia);
            modelBuilder.Entity<Localidad>()
                .Property(l => l.Nombre).IsRequired().HasMaxLength(100);

            // Cliente (solo agrego FKs opcionales)
            modelBuilder.Entity<Cliente>()
                .HasOptional(c => c.Pais)
                .WithMany()
                .HasForeignKey(c => c.IdPais);

            modelBuilder.Entity<Cliente>()
                .HasOptional(c => c.Provincia)
                .WithMany()
                .HasForeignKey(c => c.IdProvincia);

            modelBuilder.Entity<Cliente>()
                .HasOptional(c => c.LocalidadRef)
                .WithMany()
                .HasForeignKey(c => c.IdLocalidad);

            base.OnModelCreating(modelBuilder);
        }
    }
}