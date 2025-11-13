using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class AgregarPagosPedido : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PedidoPago",
                c => new
                {
                    IdPedidoPago = c.Guid(nullable: false),
                    IdPedido = c.Guid(nullable: false),
                    Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Porcentaje = c.Decimal(precision: 18, scale: 2),
                    FechaRegistro = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                })
                .PrimaryKey(t => t.IdPedidoPago)
                .ForeignKey("dbo.Pedido", t => t.IdPedido)
                .Index(t => t.IdPedido);

            Sql(@"
                INSERT INTO dbo.PedidoPago (IdPedidoPago, IdPedido, Monto, Porcentaje, FechaRegistro)
                SELECT NEWID(),
                       p.IdPedido,
                       p.MontoPagado,
                       CASE WHEN p.TotalConIva > 0 THEN CAST(ROUND((p.MontoPagado / p.TotalConIva) * 100.0, 2) AS DECIMAL(18, 2)) ELSE NULL END,
                       COALESCE(p.FechaEntrega, p.FechaEnvio, p.FechaFinalizacion, p.FechaProduccion, p.FechaConfirmacion, p.FechaCreacion, GETUTCDATE())
                FROM dbo.Pedido p
                WHERE p.MontoPagado > 0
            ");
        }

        public override void Down()
        {
            DropForeignKey("dbo.PedidoPago", "IdPedido", "dbo.Pedido");
            DropIndex("dbo.PedidoPago", new[] { "IdPedido" });
            DropTable("dbo.PedidoPago");
        }
    }
}