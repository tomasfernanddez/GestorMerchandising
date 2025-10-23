namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoriaProducto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PedidoEstadoHistorial",
                c => new
                    {
                        IdHistorial = c.Guid(nullable: false),
                        IdPedido = c.Guid(nullable: false),
                        IdEstadoPedido = c.Guid(nullable: false),
                        FechaCambio = c.DateTime(nullable: false),
                        Comentario = c.String(maxLength: 250),
                        Usuario = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.IdHistorial)
                .ForeignKey("dbo.EstadoPedido", t => t.IdEstadoPedido)
                .ForeignKey("dbo.Pedido", t => t.IdPedido)
                .Index(t => t.IdPedido)
                .Index(t => t.IdEstadoPedido);
            
            CreateTable(
                "dbo.PedidoNota",
                c => new
                    {
                        IdNota = c.Guid(nullable: false),
                        IdPedido = c.Guid(nullable: false),
                        Nota = c.String(maxLength: 500),
                        Fecha = c.DateTime(nullable: false),
                        Usuario = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.IdNota)
                .ForeignKey("dbo.Pedido", t => t.IdPedido)
                .Index(t => t.IdPedido);
            
            AddColumn("dbo.CategoriaProducto", "Activo", c => c.Boolean(nullable: false));
            AddColumn("dbo.CategoriaProducto", "Orden", c => c.Int(nullable: false));
            AddColumn("dbo.CategoriaProducto", "FechaCreacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Producto", "Activo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Producto", "FechaCreacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Producto", "FechaUltimoUso", c => c.DateTime());
            AddColumn("dbo.Producto", "VecesUsado", c => c.Int(nullable: false));
            AddColumn("dbo.LogosPedido", "IdProveedor", c => c.Guid());
            AddColumn("dbo.LogosPedido", "Cantidad", c => c.Int(nullable: false));
            AddColumn("dbo.PedidoDetalle", "FechaLimiteProduccion", c => c.DateTime());
            AddColumn("dbo.PedidoDetalle", "Notas", c => c.String(maxLength: 250));
            AddColumn("dbo.PedidoDetalle", "IdProveedorPersonalizacion", c => c.Guid());
            AddColumn("dbo.PedidoDetalle", "ProveedorPersonalizacion_IdProveedor", c => c.Guid());
            AddColumn("dbo.Pedido", "NumeroPedido", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Pedido", "FechaCreacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Pedido", "FechaConfirmacion", c => c.DateTime());
            AddColumn("dbo.Pedido", "FechaProduccion", c => c.DateTime());
            AddColumn("dbo.Pedido", "FechaFinalizacion", c => c.DateTime());
            AddColumn("dbo.Pedido", "FechaEnvio", c => c.DateTime());
            AddColumn("dbo.Pedido", "FechaEntrega", c => c.DateTime());
            AddColumn("dbo.Pedido", "FechaLimiteEntrega", c => c.DateTime());
            AddColumn("dbo.Pedido", "Observaciones", c => c.String(maxLength: 500));
            AddColumn("dbo.Pedido", "Facturado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pedido", "RutaFacturaPdf", c => c.String(maxLength: 260));
            AddColumn("dbo.Pedido", "TotalSinIva", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pedido", "MontoIva", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pedido", "TotalConIva", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pedido", "MontoPagado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pedido", "SaldoPendiente", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Producto", "NombreProducto", c => c.String(nullable: false, maxLength: 150));
            CreateIndex("dbo.LogosPedido", "IdProveedor");
            CreateIndex("dbo.PedidoDetalle", "ProveedorPersonalizacion_IdProveedor");
            AddForeignKey("dbo.PedidoDetalle", "ProveedorPersonalizacion_IdProveedor", "dbo.Proveedor", "IdProveedor");
            AddForeignKey("dbo.LogosPedido", "IdProveedor", "dbo.Proveedor", "IdProveedor");
            DropIndex("dbo.Pedido", "IX_Pedido_Fecha");
            DropColumn("dbo.Pedido", "Fecha");
            DropColumn("dbo.Pedido", "FechaLimite");
            DropColumn("dbo.Pedido", "TieneFechaLimite");
        }

        public override void Down()
        {
            AddColumn("dbo.Pedido", "TieneFechaLimite", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pedido", "FechaLimite", c => c.DateTime());
            AddColumn("dbo.Pedido", "Fecha", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Pedido", "Fecha", name: "IX_Pedido_Fecha");
            DropForeignKey("dbo.LogosPedido", "IdProveedor", "dbo.Proveedor");
            DropForeignKey("dbo.PedidoDetalle", "ProveedorPersonalizacion_IdProveedor", "dbo.Proveedor");
            DropForeignKey("dbo.PedidoNota", "IdPedido", "dbo.Pedido");
            DropForeignKey("dbo.PedidoEstadoHistorial", "IdPedido", "dbo.Pedido");
            DropForeignKey("dbo.PedidoEstadoHistorial", "IdEstadoPedido", "dbo.EstadoPedido");
            DropIndex("dbo.PedidoNota", new[] { "IdPedido" });
            DropIndex("dbo.PedidoEstadoHistorial", new[] { "IdEstadoPedido" });
            DropIndex("dbo.PedidoEstadoHistorial", new[] { "IdPedido" });
            DropIndex("dbo.PedidoDetalle", new[] { "ProveedorPersonalizacion_IdProveedor" });
            DropIndex("dbo.LogosPedido", new[] { "IdProveedor" });
            AlterColumn("dbo.Producto", "NombreProducto", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Pedido", "SaldoPendiente");
            DropColumn("dbo.Pedido", "MontoPagado");
            DropColumn("dbo.Pedido", "TotalConIva");
            DropColumn("dbo.Pedido", "MontoIva");
            DropColumn("dbo.Pedido", "TotalSinIva");
            DropColumn("dbo.Pedido", "RutaFacturaPdf");
            DropColumn("dbo.Pedido", "Facturado");
            DropColumn("dbo.Pedido", "Observaciones");
            DropColumn("dbo.Pedido", "FechaLimiteEntrega");
            DropColumn("dbo.Pedido", "FechaEntrega");
            DropColumn("dbo.Pedido", "FechaEnvio");
            DropColumn("dbo.Pedido", "FechaFinalizacion");
            DropColumn("dbo.Pedido", "FechaProduccion");
            DropColumn("dbo.Pedido", "FechaConfirmacion");
            DropColumn("dbo.Pedido", "FechaCreacion");
            DropColumn("dbo.Pedido", "NumeroPedido");
            DropColumn("dbo.PedidoDetalle", "ProveedorPersonalizacion_IdProveedor");
            DropColumn("dbo.PedidoDetalle", "IdProveedorPersonalizacion");
            DropColumn("dbo.PedidoDetalle", "Notas");
            DropColumn("dbo.PedidoDetalle", "FechaLimiteProduccion");
            DropColumn("dbo.LogosPedido", "Cantidad");
            DropColumn("dbo.LogosPedido", "IdProveedor");
            DropColumn("dbo.Producto", "VecesUsado");
            DropColumn("dbo.Producto", "FechaUltimoUso");
            DropColumn("dbo.Producto", "FechaCreacion");
            DropColumn("dbo.Producto", "Activo");
            DropColumn("dbo.CategoriaProducto", "FechaCreacion");
            DropColumn("dbo.CategoriaProducto", "Orden");
            DropColumn("dbo.CategoriaProducto", "Activo");
            DropTable("dbo.PedidoNota");
            DropTable("dbo.PedidoEstadoHistorial");
        }
    }
}
