namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NumeroPedidoMuestra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetalleMuestra", "Cantidad", c => c.Int(nullable: false));
            AddColumn("dbo.DetalleMuestra", "PrecioUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.DetalleMuestra", "Subtotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.DetalleMuestra", "FechaDevolucion", c => c.DateTime());
            AddColumn("dbo.PedidoMuestra", "NumeroPedidoMuestra", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.PedidoMuestra", "FechaCreacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.PedidoMuestra", "FechaDevolucionEsperada", c => c.DateTime());
            AddColumn("dbo.PedidoMuestra", "Observaciones", c => c.String(maxLength: 500));
            AddColumn("dbo.PedidoMuestra", "Facturado", c => c.Boolean(nullable: false));
            AddColumn("dbo.PedidoMuestra", "RutaFacturaPdf", c => c.String(maxLength: 260));
            AddColumn("dbo.PedidoMuestra", "MontoTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PedidoMuestra", "MontoPagado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PedidoMuestra", "SaldoPendiente", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PedidoMuestra", "SaldoPendiente");
            DropColumn("dbo.PedidoMuestra", "MontoPagado");
            DropColumn("dbo.PedidoMuestra", "MontoTotal");
            DropColumn("dbo.PedidoMuestra", "RutaFacturaPdf");
            DropColumn("dbo.PedidoMuestra", "Facturado");
            DropColumn("dbo.PedidoMuestra", "Observaciones");
            DropColumn("dbo.PedidoMuestra", "FechaDevolucionEsperada");
            DropColumn("dbo.PedidoMuestra", "FechaCreacion");
            DropColumn("dbo.PedidoMuestra", "NumeroPedidoMuestra");
            DropColumn("dbo.DetalleMuestra", "FechaDevolucion");
            DropColumn("dbo.DetalleMuestra", "Subtotal");
            DropColumn("dbo.DetalleMuestra", "PrecioUnitario");
            DropColumn("dbo.DetalleMuestra", "Cantidad");
        }
    }
}
