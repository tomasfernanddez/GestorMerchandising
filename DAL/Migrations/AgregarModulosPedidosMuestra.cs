using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    /// <summary>
    /// Crea las tablas y catálogos necesarios para el módulo de pedidos de muestra.
    /// </summary>
    public partial class AgregarModuloPedidosMuestra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstadoPedidoMuestra",
                c => new
                {
                    IdEstadoPedidoMuestra = c.Guid(nullable: false),
                    NombreEstadoPedidoMuestra = c.String(nullable: false, maxLength: 50)
                })
                .PrimaryKey(t => t.IdEstadoPedidoMuestra);

            CreateTable(
                "dbo.EstadoMuestra",
                c => new
                {
                    IdEstadoMuestra = c.Guid(nullable: false),
                    NombreEstadoMuestra = c.String(nullable: false, maxLength: 50)
                })
                .PrimaryKey(t => t.IdEstadoMuestra);

            CreateTable(
                "dbo.PedidoMuestra",
                c => new
                {
                    IdPedidoMuestra = c.Guid(nullable: false),
                    IdCliente = c.Guid(nullable: false),
                    FechaCreacion = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaEntrega = c.DateTime(),
                    FechaDevolucionEsperada = c.DateTime(),
                    FechaDevolucion = c.DateTime(),
                    DireccionEntrega = c.String(maxLength: 150),
                    PersonaContacto = c.String(maxLength: 100),
                    EmailContacto = c.String(maxLength: 100),
                    TelefonoContacto = c.String(maxLength: 30),
                    Observaciones = c.String(maxLength: 500),
                    Facturado = c.Boolean(nullable: false, defaultValue: false),
                    RutaFacturaPdf = c.String(maxLength: 260),
                    MontoTotal = c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m),
                    MontoPagado = c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m),
                    SaldoPendiente = c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m),
                    IdEstadoPedidoMuestra = c.Guid(),
                })
                .PrimaryKey(t => t.IdPedidoMuestra)
                .ForeignKey("dbo.Cliente", t => t.IdCliente, cascadeDelete: false)
                .ForeignKey("dbo.EstadoPedidoMuestra", t => t.IdEstadoPedidoMuestra)
                .Index(t => t.IdCliente, name: "IX_PedidoMuestra_IdCliente")
                .Index(t => t.IdEstadoPedidoMuestra, name: "IX_PedidoMuestra_IdEstado");

            CreateTable(
                "dbo.DetalleMuestra",
                c => new
                {
                    IdDetalleMuestra = c.Guid(nullable: false),
                    IdPedidoMuestra = c.Guid(nullable: false),
                    IdProducto = c.Guid(nullable: false),
                    Cantidad = c.Int(nullable: false, defaultValue: 1),
                    PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m),
                    Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m),
                    IdEstadoMuestra = c.Guid(),
                    FechaDevolucion = c.DateTime(),
                })
                .PrimaryKey(t => t.IdDetalleMuestra)
                .ForeignKey("dbo.PedidoMuestra", t => t.IdPedidoMuestra, cascadeDelete: false)
                .ForeignKey("dbo.Producto", t => t.IdProducto, cascadeDelete: false)
                .ForeignKey("dbo.EstadoMuestra", t => t.IdEstadoMuestra)
                .Index(t => t.IdPedidoMuestra, name: "IX_DetalleMuestra_IdPedido")
                .Index(t => t.IdProducto, name: "IX_DetalleMuestra_IdProducto")
                .Index(t => t.IdEstadoMuestra, name: "IX_DetalleMuestra_IdEstado");

            // Estados por defecto de pedidos de muestra
            SeedEstadosPedidoMuestra();
            SeedEstadosMuestra();
        }

        public override void Down()
        {
            DropForeignKey("dbo.DetalleMuestra", "IdEstadoMuestra", "dbo.EstadoMuestra");
            DropForeignKey("dbo.DetalleMuestra", "IdProducto", "dbo.Producto");
            DropForeignKey("dbo.DetalleMuestra", "IdPedidoMuestra", "dbo.PedidoMuestra");
            DropForeignKey("dbo.PedidoMuestra", "IdEstadoPedidoMuestra", "dbo.EstadoPedidoMuestra");
            DropForeignKey("dbo.PedidoMuestra", "IdCliente", "dbo.Cliente");
            DropIndex("dbo.DetalleMuestra", "IX_DetalleMuestra_IdEstado");
            DropIndex("dbo.DetalleMuestra", "IX_DetalleMuestra_IdProducto");
            DropIndex("dbo.DetalleMuestra", "IX_DetalleMuestra_IdPedido");
            DropIndex("dbo.PedidoMuestra", "IX_PedidoMuestra_IdEstado");
            DropIndex("dbo.PedidoMuestra", "IX_PedidoMuestra_IdCliente");
            DropTable("dbo.DetalleMuestra");
            DropTable("dbo.PedidoMuestra");
            DropTable("dbo.EstadoMuestra");
            DropTable("dbo.EstadoPedidoMuestra");
        }

        private void SeedEstadosPedidoMuestra()
        {
            var estadosPedido = new (Guid Id, string Nombre)[]
            {
                (Guid.Parse("7D4D7789-5FA9-4E55-B074-8B0DF0F2C1CF"), "Pendiente"),
                (Guid.Parse("E5D5C6F4-85A3-4BAE-8C8D-3A3A7F6643AC"), "En Proceso"),
                (Guid.Parse("6E2A00E0-88E2-4D8A-8CF6-81C712A2AB8C"), "Completado"),
                (Guid.Parse("8C8F8D54-3C3B-4D7C-A84D-1D3F8D0FA2C4"), "Facturación Pendiente")
            };

            foreach (var estado in estadosPedido)
            {
                Sql($@"IF NOT EXISTS (SELECT 1 FROM dbo.EstadoPedidoMuestra WHERE IdEstadoPedidoMuestra = '{estado.Id}')
BEGIN
    INSERT INTO dbo.EstadoPedidoMuestra (IdEstadoPedidoMuestra, NombreEstadoPedidoMuestra)
    VALUES ('{estado.Id}', '{estado.Nombre}')
END");
            }
        }

        private void SeedEstadosMuestra()
        {
            var estadosDetalle = new (Guid Id, string Nombre)[]
            {
                (Guid.Parse("5FE3C9D8-52DF-4B50-BD2B-0AE25C8E4419"), "Pendiente de Envío"),
                (Guid.Parse("B5D0F04F-5E43-4ED2-8D09-36E32D7C99F7"), "En Cliente"),
                (Guid.Parse("0E7C91C9-4F30-4B6D-95B2-07F4F21E1D47"), "Devuelto"),
                (Guid.Parse("14C94961-6B5C-4A65-AB8E-BCA6D10B2A9D"), "A Facturar")
            };

            foreach (var estado in estadosDetalle)
            {
                Sql($@"IF NOT EXISTS (SELECT 1 FROM dbo.EstadoMuestra WHERE IdEstadoMuestra = '{estado.Id}')
BEGIN
    INSERT INTO dbo.EstadoMuestra (IdEstadoMuestra, NombreEstadoMuestra)
    VALUES ('{estado.Id}', '{estado.Nombre}')
END");
            }
        }
    }
}