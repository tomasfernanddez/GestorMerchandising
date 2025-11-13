using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class AgregarPagosPedidoMuestra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PedidoMuestraPago",
                c => new
                {
                    IdPedidoMuestraPago = c.Guid(nullable: false),
                    IdPedidoMuestra = c.Guid(nullable: false),
                    Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Porcentaje = c.Decimal(precision: 18, scale: 2),
                    FechaRegistro = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                })
                .PrimaryKey(t => t.IdPedidoMuestraPago)
                .ForeignKey("dbo.PedidoMuestra", t => t.IdPedidoMuestra)
                .Index(t => t.IdPedidoMuestra);

            Sql(@"
                INSERT INTO dbo.PedidoMuestraPago (IdPedidoMuestraPago, IdPedidoMuestra, Monto, Porcentaje, FechaRegistro)
                SELECT NEWID(),
                       pm.IdPedidoMuestra,
                       pm.MontoPagado,
                       CASE WHEN pm.MontoTotal > 0 THEN CAST(ROUND((pm.MontoPagado / pm.MontoTotal) * 100.0, 2) AS DECIMAL(18, 2)) ELSE NULL END,
                       COALESCE(pm.FechaDevolucion, pm.FechaEntrega, pm.FechaCreacion, GETUTCDATE())
                FROM dbo.PedidoMuestra pm
                WHERE pm.MontoPagado > 0
            ");
        }

        public override void Down()
        {
            DropForeignKey("dbo.PedidoMuestraPago", "IdPedidoMuestra", "dbo.PedidoMuestra");
            DropIndex("dbo.PedidoMuestraPago", new[] { "IdPedidoMuestra" });
            DropTable("dbo.PedidoMuestraPago");
        }
    }
}