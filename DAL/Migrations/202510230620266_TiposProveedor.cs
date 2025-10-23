namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TiposProveedor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Proveedor", "IdTipoProveedor", "dbo.TipoProveedor");
            DropIndex("dbo.Proveedor", new[] { "IdTipoProveedor" });
            CreateTable(
                "dbo.ProveedorTipoProveedor",
                c => new
                    {
                        IdProveedor = c.Guid(nullable: false),
                        IdTipoProveedor = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdProveedor, t.IdTipoProveedor })
                .ForeignKey("dbo.Proveedor", t => t.IdProveedor)
                .ForeignKey("dbo.TipoProveedor", t => t.IdTipoProveedor)
                .Index(t => t.IdProveedor)
                .Index(t => t.IdTipoProveedor);
            
            DropColumn("dbo.Proveedor", "IdTipoProveedor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Proveedor", "IdTipoProveedor", c => c.Guid());
            DropForeignKey("dbo.ProveedorTipoProveedor", "IdTipoProveedor", "dbo.TipoProveedor");
            DropForeignKey("dbo.ProveedorTipoProveedor", "IdProveedor", "dbo.Proveedor");
            DropIndex("dbo.ProveedorTipoProveedor", new[] { "IdTipoProveedor" });
            DropIndex("dbo.ProveedorTipoProveedor", new[] { "IdProveedor" });
            DropTable("dbo.ProveedorTipoProveedor");
            CreateIndex("dbo.Proveedor", "IdTipoProveedor");
            AddForeignKey("dbo.Proveedor", "IdTipoProveedor", "dbo.TipoProveedor", "IdTipoProveedor");
        }
    }
}
