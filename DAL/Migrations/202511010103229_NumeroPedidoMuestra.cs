namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NumeroPedidoMuestra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PedidoMuestra", "NumeroPedidoMuestra", c => c.String(maxLength: 20));

            Sql(@"
                WITH Numeros AS (
                    SELECT
                        IdPedidoMuestra,
                        ROW_NUMBER() OVER (ORDER BY FechaCreacion, IdPedidoMuestra) AS NumeroSecuencial
                    FROM dbo.PedidoMuestra
                )
                UPDATE pm
                SET NumeroPedidoMuestra = 'PM-' + RIGHT('000000' + CAST(n.NumeroSecuencial AS VARCHAR(6)), 6)
                FROM dbo.PedidoMuestra pm
                INNER JOIN Numeros n ON pm.IdPedidoMuestra = n.IdPedidoMuestra
                WHERE pm.NumeroPedidoMuestra IS NULL;
            ");

            AlterColumn("dbo.PedidoMuestra", "NumeroPedidoMuestra", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PedidoMuestra", "NumeroPedidoMuestra");
        }
    }
}
