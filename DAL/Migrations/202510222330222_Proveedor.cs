namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Proveedor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Usuario", "IdPerfil", "dbo.Perfil");
            DropForeignKey("dbo.Bitacora", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.Sesion", "IdUsuario", "dbo.Usuario");
            DropIndex("dbo.Bitacora", new[] { "IdUsuario" });
            DropIndex("dbo.Usuario", new[] { "IdPerfil" });
            DropIndex("dbo.Sesion", new[] { "IdUsuario" });
            CreateTable(
                "dbo.CondicionIva",
                c => new
                    {
                        IdCondicionIva = c.Guid(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.IdCondicionIva);
            
            CreateTable(
                "dbo.Localidad",
                c => new
                    {
                        IdLocalidad = c.Guid(nullable: false),
                        IdProvincia = c.Guid(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.IdLocalidad)
                .ForeignKey("dbo.Provincia", t => t.IdProvincia)
                .Index(t => t.IdProvincia);
            
            CreateTable(
                "dbo.Provincia",
                c => new
                    {
                        IdProvincia = c.Guid(nullable: false),
                        IdPais = c.Guid(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.IdProvincia)
                .ForeignKey("dbo.Pais", t => t.IdPais)
                .Index(t => t.IdPais);
            
            CreateTable(
                "dbo.Pais",
                c => new
                    {
                        IdPais = c.Guid(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.IdPais);
            
            CreateTable(
                "dbo.GrupoPersonalizacion",
                c => new
                    {
                        IdGrupoPersonalizacion = c.Guid(nullable: false),
                        IdDetallePedido = c.Guid(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                        CostoAdicionalGrupo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Activo = c.Boolean(nullable: false),
                        IdGrupoPadre = c.Guid(),
                    })
                .PrimaryKey(t => t.IdGrupoPersonalizacion)
                .ForeignKey("dbo.PedidoDetalle", t => t.IdDetallePedido)
                .ForeignKey("dbo.GrupoPersonalizacion", t => t.IdGrupoPadre)
                .Index(t => t.IdDetallePedido)
                .Index(t => t.IdGrupoPadre);
            
            CreateTable(
                "dbo.GrupoPersonalizacionLogo",
                c => new
                    {
                        IdGrupo = c.Guid(nullable: false),
                        IdLogo = c.Guid(nullable: false),
                        Orden = c.Int(nullable: false),
                        FechaAgregado = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdGrupo, t.IdLogo })
                .ForeignKey("dbo.GrupoPersonalizacion", t => t.IdGrupo, cascadeDelete: true)
                .ForeignKey("dbo.LogosPedido", t => t.IdLogo, cascadeDelete: true)
                .Index(t => t.IdGrupo)
                .Index(t => t.IdLogo);
            
            CreateTable(
                "dbo.ProveedorTecnicaPersonalizacion",
                c => new
                    {
                        IdProveedor = c.Guid(nullable: false),
                        IdTecnicaPersonalizacion = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdProveedor, t.IdTecnicaPersonalizacion })
                .ForeignKey("dbo.Proveedor", t => t.IdProveedor)
                .ForeignKey("dbo.TecnicaPersonalizacion", t => t.IdTecnicaPersonalizacion)
                .Index(t => t.IdProveedor)
                .Index(t => t.IdTecnicaPersonalizacion);
            
            AddColumn("dbo.Cliente", "IdCondicionIva", c => c.Guid(nullable: false));
            AddColumn("dbo.Cliente", "IdPais", c => c.Guid());
            AddColumn("dbo.Cliente", "IdProvincia", c => c.Guid());
            AddColumn("dbo.Cliente", "IdLocalidad", c => c.Guid());
            AddColumn("dbo.LogosPedido", "CostoPersonalizacion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LogosPedido", "Descripcion", c => c.String(maxLength: 200));
            AddColumn("dbo.Proveedor", "IdCondicionIva", c => c.Guid(nullable: false));
            AddColumn("dbo.Proveedor", "CodigoPostal", c => c.String(maxLength: 20));
            AddColumn("dbo.Proveedor", "CondicionesPago", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Proveedor", "Observaciones", c => c.String(maxLength: 500));
            AddColumn("dbo.Proveedor", "FechaAlta", c => c.DateTime(nullable: false));
            AddColumn("dbo.Proveedor", "IdPais", c => c.Guid());
            AddColumn("dbo.Proveedor", "IdProvincia", c => c.Guid());
            AddColumn("dbo.Proveedor", "IdLocalidad", c => c.Guid());
            CreateIndex("dbo.Cliente", "IdCondicionIva");
            CreateIndex("dbo.Cliente", "IdPais");
            CreateIndex("dbo.Cliente", "IdProvincia");
            CreateIndex("dbo.Cliente", "IdLocalidad");
            CreateIndex("dbo.Proveedor", "IdCondicionIva");
            CreateIndex("dbo.Proveedor", "IdPais");
            CreateIndex("dbo.Proveedor", "IdProvincia");
            CreateIndex("dbo.Proveedor", "IdLocalidad");
            AddForeignKey("dbo.Proveedor", "IdCondicionIva", "dbo.CondicionIva", "IdCondicionIva");
            AddForeignKey("dbo.Proveedor", "IdLocalidad", "dbo.Localidad", "IdLocalidad");
            AddForeignKey("dbo.Proveedor", "IdPais", "dbo.Pais", "IdPais");
            AddForeignKey("dbo.Proveedor", "IdProvincia", "dbo.Provincia", "IdProvincia");
            AddForeignKey("dbo.Cliente", "IdCondicionIva", "dbo.CondicionIva", "IdCondicionIva");
            AddForeignKey("dbo.Cliente", "IdLocalidad", "dbo.Localidad", "IdLocalidad");
            AddForeignKey("dbo.Cliente", "IdPais", "dbo.Pais", "IdPais");
            AddForeignKey("dbo.Cliente", "IdProvincia", "dbo.Provincia", "IdProvincia");
            DropColumn("dbo.Cliente", "CondicionIva");
            DropColumn("dbo.Proveedor", "CondicionIva");
            DropTable("dbo.Bitacora");
            DropTable("dbo.Usuario");
            DropTable("dbo.Perfil");
            DropTable("dbo.Sesion");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Sesion",
                c => new
                    {
                        IdSesion = c.Guid(nullable: false),
                        IdUsuario = c.Guid(nullable: false),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(),
                        Activa = c.Boolean(nullable: false),
                        DireccionIP = c.String(maxLength: 100),
                        UserAgent = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.IdSesion);
            
            CreateTable(
                "dbo.Perfil",
                c => new
                    {
                        IdPerfil = c.Guid(nullable: false),
                        NombrePerfil = c.String(nullable: false, maxLength: 50),
                        Descripcion = c.String(maxLength: 200),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdPerfil);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        IdUsuario = c.Guid(nullable: false),
                        NombreUsuario = c.String(nullable: false, maxLength: 50),
                        PasswordHash = c.String(nullable: false, maxLength: 100),
                        NombreCompleto = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        Activo = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaUltimoAcceso = c.DateTime(),
                        IntentosLoginFallidos = c.Int(nullable: false),
                        Bloqueado = c.Boolean(nullable: false),
                        FechaBloqueo = c.DateTime(),
                        IdPerfil = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.IdUsuario);
            
            CreateTable(
                "dbo.Bitacora",
                c => new
                    {
                        IdBitacora = c.Guid(nullable: false),
                        IdUsuario = c.Guid(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Accion = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                        Modulo = c.String(maxLength: 50),
                        DireccionIP = c.String(maxLength: 100),
                        Exitoso = c.Boolean(nullable: false),
                        MensajeError = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.IdBitacora);
            
            AddColumn("dbo.Proveedor", "CondicionIva", c => c.String(maxLength: 50));
            AddColumn("dbo.Cliente", "CondicionIva", c => c.String(maxLength: 50));
            DropForeignKey("dbo.GrupoPersonalizacionLogo", "IdLogo", "dbo.LogosPedido");
            DropForeignKey("dbo.GrupoPersonalizacionLogo", "IdGrupo", "dbo.GrupoPersonalizacion");
            DropForeignKey("dbo.GrupoPersonalizacion", "IdGrupoPadre", "dbo.GrupoPersonalizacion");
            DropForeignKey("dbo.GrupoPersonalizacion", "IdDetallePedido", "dbo.PedidoDetalle");
            DropForeignKey("dbo.Cliente", "IdProvincia", "dbo.Provincia");
            DropForeignKey("dbo.Cliente", "IdPais", "dbo.Pais");
            DropForeignKey("dbo.Cliente", "IdLocalidad", "dbo.Localidad");
            DropForeignKey("dbo.Cliente", "IdCondicionIva", "dbo.CondicionIva");
            DropForeignKey("dbo.ProveedorTecnicaPersonalizacion", "IdTecnicaPersonalizacion", "dbo.TecnicaPersonalizacion");
            DropForeignKey("dbo.ProveedorTecnicaPersonalizacion", "IdProveedor", "dbo.Proveedor");
            DropForeignKey("dbo.Proveedor", "IdProvincia", "dbo.Provincia");
            DropForeignKey("dbo.Proveedor", "IdPais", "dbo.Pais");
            DropForeignKey("dbo.Proveedor", "IdLocalidad", "dbo.Localidad");
            DropForeignKey("dbo.Localidad", "IdProvincia", "dbo.Provincia");
            DropForeignKey("dbo.Provincia", "IdPais", "dbo.Pais");
            DropForeignKey("dbo.Proveedor", "IdCondicionIva", "dbo.CondicionIva");
            DropIndex("dbo.ProveedorTecnicaPersonalizacion", new[] { "IdTecnicaPersonalizacion" });
            DropIndex("dbo.ProveedorTecnicaPersonalizacion", new[] { "IdProveedor" });
            DropIndex("dbo.GrupoPersonalizacionLogo", new[] { "IdLogo" });
            DropIndex("dbo.GrupoPersonalizacionLogo", new[] { "IdGrupo" });
            DropIndex("dbo.GrupoPersonalizacion", new[] { "IdGrupoPadre" });
            DropIndex("dbo.GrupoPersonalizacion", new[] { "IdDetallePedido" });
            DropIndex("dbo.Provincia", new[] { "IdPais" });
            DropIndex("dbo.Localidad", new[] { "IdProvincia" });
            DropIndex("dbo.Proveedor", new[] { "IdLocalidad" });
            DropIndex("dbo.Proveedor", new[] { "IdProvincia" });
            DropIndex("dbo.Proveedor", new[] { "IdPais" });
            DropIndex("dbo.Proveedor", new[] { "IdCondicionIva" });
            DropIndex("dbo.Cliente", new[] { "IdLocalidad" });
            DropIndex("dbo.Cliente", new[] { "IdProvincia" });
            DropIndex("dbo.Cliente", new[] { "IdPais" });
            DropIndex("dbo.Cliente", new[] { "IdCondicionIva" });
            DropColumn("dbo.Proveedor", "IdLocalidad");
            DropColumn("dbo.Proveedor", "IdProvincia");
            DropColumn("dbo.Proveedor", "IdPais");
            DropColumn("dbo.Proveedor", "FechaAlta");
            DropColumn("dbo.Proveedor", "Observaciones");
            DropColumn("dbo.Proveedor", "CondicionesPago");
            DropColumn("dbo.Proveedor", "CodigoPostal");
            DropColumn("dbo.Proveedor", "IdCondicionIva");
            DropColumn("dbo.LogosPedido", "Descripcion");
            DropColumn("dbo.LogosPedido", "CostoPersonalizacion");
            DropColumn("dbo.Cliente", "IdLocalidad");
            DropColumn("dbo.Cliente", "IdProvincia");
            DropColumn("dbo.Cliente", "IdPais");
            DropColumn("dbo.Cliente", "IdCondicionIva");
            DropTable("dbo.ProveedorTecnicaPersonalizacion");
            DropTable("dbo.GrupoPersonalizacionLogo");
            DropTable("dbo.GrupoPersonalizacion");
            DropTable("dbo.Pais");
            DropTable("dbo.Provincia");
            DropTable("dbo.Localidad");
            DropTable("dbo.CondicionIva");
            CreateIndex("dbo.Sesion", "IdUsuario");
            CreateIndex("dbo.Usuario", "IdPerfil");
            CreateIndex("dbo.Bitacora", "IdUsuario");
            AddForeignKey("dbo.Sesion", "IdUsuario", "dbo.Usuario", "IdUsuario");
            AddForeignKey("dbo.Bitacora", "IdUsuario", "dbo.Usuario", "IdUsuario");
            AddForeignKey("dbo.Usuario", "IdPerfil", "dbo.Perfil", "IdPerfil");
        }
    }
}
