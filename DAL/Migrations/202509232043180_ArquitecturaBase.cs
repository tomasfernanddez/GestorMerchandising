namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArquitecturaBase : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.IdBitacora)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdUsuario);
            
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
                .PrimaryKey(t => t.IdUsuario)
                .ForeignKey("dbo.Perfil", t => t.IdPerfil)
                .Index(t => t.IdPerfil);
            
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
                .PrimaryKey(t => t.IdSesion)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdUsuario);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sesion", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.Bitacora", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.Usuario", "IdPerfil", "dbo.Perfil");
            DropIndex("dbo.Sesion", new[] { "IdUsuario" });
            DropIndex("dbo.Usuario", new[] { "IdPerfil" });
            DropIndex("dbo.Bitacora", new[] { "IdUsuario" });
            DropTable("dbo.Sesion");
            DropTable("dbo.Perfil");
            DropTable("dbo.Usuario");
            DropTable("dbo.Bitacora");
        }
    }
}
