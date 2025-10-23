using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class PermitirMultiplesTiposProveedor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProveedorTipoProveedor",
                c => new
                {
                    IdProveedor = c.Guid(nullable: false),
                    IdTipoProveedor = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.IdProveedor, t.IdTipoProveedor })
                .ForeignKey("dbo.Proveedor", t => t.IdProveedor, cascadeDelete: true)
                .ForeignKey("dbo.TipoProveedor", t => t.IdTipoProveedor, cascadeDelete: true)
                .Index(t => t.IdTipoProveedor, name: "IX_ProveedorTipo_IdTipo");

            Sql("INSERT INTO dbo.ProveedorTipoProveedor (IdProveedor, IdTipoProveedor) SELECT IdProveedor, IdTipoProveedor FROM dbo.Proveedor WHERE IdTipoProveedor IS NOT NULL");

            DropForeignKey("dbo.Proveedor", "IdTipoProveedor", "dbo.TipoProveedor");
            DropIndex("dbo.Proveedor", new[] { "IdTipoProveedor" });
            DropColumn("dbo.Proveedor", "IdTipoProveedor");
        }

        public override void Down()
        {
            AddColumn("dbo.Proveedor", "IdTipoProveedor", c => c.Guid());
            CreateIndex("dbo.Proveedor", "IdTipoProveedor");
            AddForeignKey("dbo.Proveedor", "IdTipoProveedor", "dbo.TipoProveedor", "IdTipoProveedor");

            Sql("UPDATE p SET p.IdTipoProveedor = pt.IdTipoProveedor FROM dbo.Proveedor p INNER JOIN dbo.ProveedorTipoProveedor pt ON p.IdProveedor = pt.IdProveedor");

            DropForeignKey("dbo.ProveedorTipoProveedor", "IdTipoProveedor", "dbo.TipoProveedor");
            DropForeignKey("dbo.ProveedorTipoProveedor", "IdProveedor", "dbo.Proveedor");
            DropIndex("dbo.ProveedorTipoProveedor", "IX_ProveedorTipo_IdTipo");
            DropTable("dbo.ProveedorTipoProveedor");
        }
    }
}