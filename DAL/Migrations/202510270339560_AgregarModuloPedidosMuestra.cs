using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    /// <summary>
    /// Migración para habilitar el módulo de pedidos de muestra: crea tablas y catálogos base.
    /// </summary>
    public partial class AgregarModuloPedidosMuestra : DbMigration
    {
        public override void Up()
        {
            // Asegura que la migración pueda ejecutarse sobre bases que tengan tablas
            // creadas manualmente o por scripts previos.
            Sql("IF OBJECT_ID('dbo.DetalleMuestra', 'U') IS NOT NULL DROP TABLE dbo.DetalleMuestra;");
            Sql("IF OBJECT_ID('dbo.PedidoMuestra', 'U') IS NOT NULL DROP TABLE dbo.PedidoMuestra;");
            Sql("IF OBJECT_ID('dbo.EstadoMuestra', 'U') IS NOT NULL DROP TABLE dbo.EstadoMuestra;");
            Sql("IF OBJECT_ID('dbo.EstadoPedidoMuestra', 'U') IS NOT NULL DROP TABLE dbo.EstadoPedidoMuestra;");

            CreateTable(
                "dbo.EstadoPedidoMuestra",
                c => new
                {
                    IdEstadoPedidoMuestra = c.Guid(nullable: false),
                    NombreEstadoPedidoMuestra = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.IdEstadoPedidoMuestra)
                .Index(t => t.NombreEstadoPedidoMuestra, unique: true, name: "IX_EstadoPedidoMuestra_Nombre");

            CreateTable(
                "dbo.EstadoMuestra",
                c => new
                {
                    IdEstadoMuestra = c.Guid(nullable: false),
                    NombreEstadoMuestra = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.IdEstadoMuestra)
                .Index(t => t.NombreEstadoMuestra, unique: true, name: "IX_EstadoMuestra_Nombre");

            CreateTable(
                "dbo.PedidoMuestra",
                c => new
                {
                    IdPedidoMuestra = c.Guid(nullable: false),
                    IdCliente = c.Guid(nullable: false),
                    FechaEntrega = c.DateTime(),
                    FechaDevolucion = c.DateTime(),
                    DireccionEntrega = c.String(maxLength: 150),
                    PersonaContacto = c.String(maxLength: 100),
                    EmailContacto = c.String(maxLength: 100),
                    TelefonoContacto = c.String(maxLength: 30),
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
                    IdEstadoMuestra = c.Guid(),
                })
                .PrimaryKey(t => t.IdDetalleMuestra)
                .ForeignKey("dbo.PedidoMuestra", t => t.IdPedidoMuestra, cascadeDelete: false)
                .ForeignKey("dbo.Producto", t => t.IdProducto, cascadeDelete: false)
                .ForeignKey("dbo.EstadoMuestra", t => t.IdEstadoMuestra)
                .Index(t => t.IdPedidoMuestra, name: "IX_DetalleMuestra_IdPedido")
                .Index(t => t.IdProducto, name: "IX_DetalleMuestra_IdProducto")
                .Index(t => t.IdEstadoMuestra, name: "IX_DetalleMuestra_IdEstado");

            var estadosPedidoMuestra = new[]
            {
                new { Id = "58C671C8-0C4E-4F16-9C34-64B5BFC0AC91", Nombre = "Solicitado" },
                new { Id = "1A8C6338-7AB9-4CF4-A0EF-E2BB6F91527E", Nombre = "En Preparación" },
                new { Id = "69A5C669-1E44-4E4E-A341-8C0535A62140", Nombre = "Despachado" },
                new { Id = "5A34F4D2-4BAE-4C6F-9F7B-0E48A6380163", Nombre = "Entregado" },
                new { Id = "81FA4A4D-951D-4C17-9E18-7F7F3BC6CDAA", Nombre = "Cerrado" }
            };

            foreach (var estado in estadosPedidoMuestra)
            {
                Sql($@"IF NOT EXISTS (SELECT 1 FROM dbo.EstadoPedidoMuestra WHERE IdEstadoPedidoMuestra = '{estado.Id}')
                        INSERT INTO dbo.EstadoPedidoMuestra (IdEstadoPedidoMuestra, NombreEstadoPedidoMuestra)
                        VALUES ('{estado.Id}', '{estado.Nombre}');");
            }

            var estadosMuestra = new[]
            {
                new { Id = "F4E46C08-0ED9-4F4B-8B0D-BC7A905E78C4", Nombre = "Pendiente de envío" },
                new { Id = "D0D39E8A-B560-4B1D-A5BD-34D6D6BFA646", Nombre = "En tránsito" },
                new { Id = "9E1F1F0A-5B6E-4B8B-9C52-DA3C7C6E8A7C", Nombre = "En evaluación" },
                new { Id = "2C9B6F34-34F4-4D80-9EE8-3F62FE7A020F", Nombre = "Devuelta" },
                new { Id = "4EE5FEF4-9026-4C3F-8A8F-FE29F1D383AD", Nombre = "Perdida" }
            };

            foreach (var estado in estadosMuestra)
            {
                Sql($@"IF NOT EXISTS (SELECT 1 FROM dbo.EstadoMuestra WHERE IdEstadoMuestra = '{estado.Id}')
                        INSERT INTO dbo.EstadoMuestra (IdEstadoMuestra, NombreEstadoMuestra)
                        VALUES ('{estado.Id}', '{estado.Nombre}');");
            }
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
            DropIndex("dbo.EstadoMuestra", "IX_EstadoMuestra_Nombre");
            DropIndex("dbo.EstadoPedidoMuestra", "IX_EstadoPedidoMuestra_Nombre");
            DropTable("dbo.DetalleMuestra");
            DropTable("dbo.PedidoMuestra");
            DropTable("dbo.EstadoMuestra");
            DropTable("dbo.EstadoPedidoMuestra");
        }
    }
}