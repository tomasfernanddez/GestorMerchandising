using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class AgregarFechaEntregaPedidoMuestra : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'FechaEntrega' AND Object_ID = Object_ID('dbo.PedidoMuestra'))
BEGIN
    ALTER TABLE dbo.PedidoMuestra ADD FechaEntrega DATETIME NULL;
END");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'FechaEntrega' AND Object_ID = Object_ID('dbo.PedidoMuestra'))
BEGIN
    ALTER TABLE dbo.PedidoMuestra DROP COLUMN FechaEntrega;
END");
        }
    }
}