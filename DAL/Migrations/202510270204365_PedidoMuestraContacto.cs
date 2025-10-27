namespace DAL.Migrations
{
    using System;
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;

    public partial class PedidoMuestraContacto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstadoPedidoMuestra",
                c => new
                {
                    IdEstadoPedidoMuestra = c.Guid(nullable: false),
                    NombreEstadoPedidoMuestra = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.IdEstadoPedidoMuestra);

            AddColumn("dbo.PedidoMuestra", "DireccionEntrega", c => c.String(maxLength: 150));
            AddColumn("dbo.PedidoMuestra", "PersonaContacto", c => c.String(maxLength: 100));
            AddColumn("dbo.PedidoMuestra", "EmailContacto", c => c.String(maxLength: 100));
            AddColumn("dbo.PedidoMuestra", "TelefonoContacto", c => c.String(maxLength: 30));
            AddColumn("dbo.PedidoMuestra", "IdEstadoPedidoMuestra", c => c.Guid());

            CreateIndex("dbo.PedidoMuestra", "IdEstadoPedidoMuestra");
            AddForeignKey("dbo.PedidoMuestra", "IdEstadoPedidoMuestra", "dbo.EstadoPedidoMuestra", "IdEstadoPedidoMuestra");
        }

        public override void Down()
        {
            DropForeignKey("dbo.PedidoMuestra", "IdEstadoPedidoMuestra", "dbo.EstadoPedidoMuestra");
            DropIndex("dbo.PedidoMuestra", new[] { "IdEstadoPedidoMuestra" });
            DropColumn("dbo.PedidoMuestra", "IdEstadoPedidoMuestra");
            DropColumn("dbo.PedidoMuestra", "TelefonoContacto");
            DropColumn("dbo.PedidoMuestra", "EmailContacto");
            DropColumn("dbo.PedidoMuestra", "PersonaContacto");
            DropColumn("dbo.PedidoMuestra", "DireccionEntrega");
            DropTable("dbo.EstadoPedidoMuestra");
        }
    }

    [GeneratedCode("Manual", "1.0.0")]
    public sealed partial class PedidoMuestraContacto : IMigrationMetadata
    {
        string IMigrationMetadata.Id => "202510270900000_PedidoMuestraContacto";

        string IMigrationMetadata.Source => null;

        string IMigrationMetadata.Target => null;
    }
}