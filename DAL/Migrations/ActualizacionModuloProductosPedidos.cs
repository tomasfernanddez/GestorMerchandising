namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ActualizacionModuloProductosPedidos : DbMigration
    {
        public override void Up()
        {
            // ============================================================================
            // ACTUALIZACIÓN DE PRODUCTO
            // ============================================================================

            // Ampliar NombreProducto de 100 a 150 caracteres
            AlterColumn("dbo.Producto", "NombreProducto", c => c.String(nullable: false, maxLength: 150));

            // ============================================================================
            // ACTUALIZACIÓN DE PEDIDO
            // ============================================================================

            // Agregar NumeroPedido (correlativo: PED-0001, PED-0002, etc.)
            AddColumn("dbo.Pedido", "NumeroPedido", c => c.String(maxLength: 20));

            // Agregar Observaciones
            AddColumn("dbo.Pedido", "Observaciones", c => c.String(maxLength: 1000));

            // Agregar Facturado
            AddColumn("dbo.Pedido", "Facturado", c => c.Boolean(nullable: false, defaultValue: false));

            // Agregar RutaFacturaPDF
            AddColumn("dbo.Pedido", "RutaFacturaPDF", c => c.String(maxLength: 500));

            // Generar números de pedido para pedidos existentes
            Sql(@"
                DECLARE @counter INT = 1;
                DECLARE @idPedido UNIQUEIDENTIFIER;
                DECLARE @numeroPedido NVARCHAR(20);

                DECLARE pedido_cursor CURSOR FOR
                SELECT IdPedido FROM dbo.Pedido WHERE NumeroPedido IS NULL ORDER BY Fecha;

                OPEN pedido_cursor;
                FETCH NEXT FROM pedido_cursor INTO @idPedido;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    SET @numeroPedido = 'PED-' + RIGHT('0000' + CAST(@counter AS NVARCHAR(4)), 4);
                    UPDATE dbo.Pedido SET NumeroPedido = @numeroPedido WHERE IdPedido = @idPedido;
                    SET @counter = @counter + 1;
                    FETCH NEXT FROM pedido_cursor INTO @idPedido;
                END

                CLOSE pedido_cursor;
                DEALLOCATE pedido_cursor;
            ");

            // Hacer NumeroPedido NOT NULL después de backfill
            AlterColumn("dbo.Pedido", "NumeroPedido", c => c.String(nullable: false, maxLength: 20));

            // Crear índice único para NumeroPedido
            CreateIndex("dbo.Pedido", "NumeroPedido", unique: true);

            // ============================================================================
            // ACTUALIZACIÓN DE PEDIDODETALLE
            // ============================================================================

            // Agregar FechaLimiteProducto
            AddColumn("dbo.PedidoDetalle", "FechaLimiteProducto", c => c.DateTime());

            // ============================================================================
            // ACTUALIZACIÓN DE LOGOSPEDIDO
            // ============================================================================

            // Agregar IdProveedorPersonalizacion (proveedor que aplica el logo)
            AddColumn("dbo.LogosPedido", "IdProveedorPersonalizacion", c => c.Guid());

            // Crear índice y FK para ProveedorPersonalizacion
            CreateIndex("dbo.LogosPedido", "IdProveedorPersonalizacion");
            AddForeignKey("dbo.LogosPedido", "IdProveedorPersonalizacion", "dbo.Proveedor", "IdProveedor");
        }

        public override void Down()
        {
            // Revertir en orden inverso

            // LogosPedido
            DropForeignKey("dbo.LogosPedido", "IdProveedorPersonalizacion", "dbo.Proveedor");
            DropIndex("dbo.LogosPedido", new[] { "IdProveedorPersonalizacion" });
            DropColumn("dbo.LogosPedido", "IdProveedorPersonalizacion");

            // PedidoDetalle
            DropColumn("dbo.PedidoDetalle", "FechaLimiteProducto");

            // Pedido
            DropIndex("dbo.Pedido", new[] { "NumeroPedido" });
            DropColumn("dbo.Pedido", "RutaFacturaPDF");
            DropColumn("dbo.Pedido", "Facturado");
            DropColumn("dbo.Pedido", "Observaciones");
            DropColumn("dbo.Pedido", "NumeroPedido");

            // Producto
            AlterColumn("dbo.Producto", "NombreProducto", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
