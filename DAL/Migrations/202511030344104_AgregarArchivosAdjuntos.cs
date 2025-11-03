namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AgregarArchivosAdjuntos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArchivoAdjunto",
                c => new
                {
                    IdArchivoAdjunto = c.Guid(nullable: false),
                    IdPedido = c.Guid(),
                    IdPedidoMuestra = c.Guid(),
                    NombreArchivo = c.String(nullable: false, maxLength: 200),
                    Extension = c.String(nullable: false, maxLength: 10),
                    TipoContenido = c.String(nullable: false, maxLength: 100),
                    TamanoBytes = c.Long(nullable: false),
                    FechaSubida = c.DateTime(nullable: false),
                    IdUsuario = c.Guid(nullable: false),
                    NombreUsuario = c.String(maxLength: 100),
                    Descripcion = c.String(maxLength: 500),
                    Contenido = c.Binary(nullable: false),
                })
                .PrimaryKey(t => t.IdArchivoAdjunto)
                .ForeignKey("dbo.Pedido", t => t.IdPedido)
                .ForeignKey("dbo.PedidoMuestra", t => t.IdPedidoMuestra)
                .Index(t => t.IdPedido)
                .Index(t => t.IdPedidoMuestra);
        }

        public override void Down()
        {
            DropForeignKey("dbo.ArchivoAdjunto", "IdPedidoMuestra", "dbo.PedidoMuestra");
            DropForeignKey("dbo.ArchivoAdjunto", "IdPedido", "dbo.Pedido");
            DropIndex("dbo.ArchivoAdjunto", new[] { "IdPedidoMuestra" });
            DropIndex("dbo.ArchivoAdjunto", new[] { "IdPedido" });
            DropTable("dbo.ArchivoAdjunto");
        }
    }
}