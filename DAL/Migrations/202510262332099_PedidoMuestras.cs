namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PedidoMuestras : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetalleMuestra", "FechaDevolucion", c => c.DateTime());
            AddColumn("dbo.DetalleMuestra", "ComentarioDevolucion", c => c.String(maxLength: 200));
            AddColumn("dbo.DetalleMuestra", "PrecioFacturacion", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DetalleMuestra", "Facturado", c => c.Boolean(nullable: false));
            AddColumn("dbo.DetalleMuestra", "FechaFacturacion", c => c.DateTime());
            AddColumn("dbo.PedidoMuestra", "NumeroCorrelativo", c => c.String(maxLength: 20));
            AddColumn("dbo.PedidoMuestra", "FechaCreacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.PedidoMuestra", "FechaDevolucionEsperada", c => c.DateTime());
            AddColumn("dbo.PedidoMuestra", "FechaDevolucionReal", c => c.DateTime());
            AddColumn("dbo.PedidoMuestra", "Observaciones", c => c.String(maxLength: 300));
            AddColumn("dbo.PedidoMuestra", "DiasProrroga", c => c.Int(nullable: false));
            AddColumn("dbo.PedidoMuestra", "Facturado", c => c.Boolean(nullable: false));
            AddColumn("dbo.PedidoMuestra", "TotalFacturado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.PedidoMuestra", "FechaDevolucion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PedidoMuestra", "FechaDevolucion", c => c.DateTime());
            DropColumn("dbo.PedidoMuestra", "TotalFacturado");
            DropColumn("dbo.PedidoMuestra", "Facturado");
            DropColumn("dbo.PedidoMuestra", "DiasProrroga");
            DropColumn("dbo.PedidoMuestra", "Observaciones");
            DropColumn("dbo.PedidoMuestra", "FechaDevolucionReal");
            DropColumn("dbo.PedidoMuestra", "FechaDevolucionEsperada");
            DropColumn("dbo.PedidoMuestra", "FechaCreacion");
            DropColumn("dbo.PedidoMuestra", "NumeroCorrelativo");
            DropColumn("dbo.DetalleMuestra", "FechaFacturacion");
            DropColumn("dbo.DetalleMuestra", "Facturado");
            DropColumn("dbo.DetalleMuestra", "PrecioFacturacion");
            DropColumn("dbo.DetalleMuestra", "ComentarioDevolucion");
            DropColumn("dbo.DetalleMuestra", "FechaDevolucion");
        }
    }
}
