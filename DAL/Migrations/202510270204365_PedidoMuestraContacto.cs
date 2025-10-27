namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class PedidoMuestraContacto : DbMigration
    {
        public override void Up()
        {
            Sql(@"
IF OBJECT_ID(N'dbo.EstadoPedidoMuestra', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[EstadoPedidoMuestra]
    (
        [IdEstadoPedidoMuestra] UNIQUEIDENTIFIER NOT NULL,
        [NombreEstadoPedidoMuestra] NVARCHAR(50) NOT NULL,
        CONSTRAINT [PK_dbo.EstadoPedidoMuestra] PRIMARY KEY CLUSTERED ([IdEstadoPedidoMuestra] ASC)
    );
END
");

            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'DireccionEntrega') IS NULL
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra] ADD [DireccionEntrega] NVARCHAR(150) NULL;
END
");

            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'PersonaContacto') IS NULL
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra] ADD [PersonaContacto] NVARCHAR(100) NULL;
END
");

            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'EmailContacto') IS NULL
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra] ADD [EmailContacto] NVARCHAR(100) NULL;
END
");

            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'TelefonoContacto') IS NULL
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra] ADD [TelefonoContacto] NVARCHAR(30) NULL;
END
");

            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'IdEstadoPedidoMuestra') IS NULL
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra] ADD [IdEstadoPedidoMuestra] UNIQUEIDENTIFIER NULL;
END
");

            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'IdEstadoPedidoMuestra') IS NOT NULL
   AND NOT EXISTS (
        SELECT 1
        FROM sys.indexes
        WHERE name = N'IX_IdEstadoPedidoMuestra'
          AND object_id = OBJECT_ID(N'[dbo].[PedidoMuestra]')
    )
BEGIN
    CREATE INDEX [IX_IdEstadoPedidoMuestra] ON [dbo].[PedidoMuestra]([IdEstadoPedidoMuestra]);
END
");

            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'IdEstadoPedidoMuestra') IS NOT NULL
   AND OBJECT_ID(N'dbo.EstadoPedidoMuestra', 'U') IS NOT NULL
   AND NOT EXISTS (
        SELECT 1
        FROM sys.foreign_keys
        WHERE name = N'FK_dbo.PedidoMuestra_dbo.EstadoPedidoMuestra_IdEstadoPedidoMuestra'
    )
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra]
        ADD CONSTRAINT [FK_dbo.PedidoMuestra_dbo.EstadoPedidoMuestra_IdEstadoPedidoMuestra]
        FOREIGN KEY ([IdEstadoPedidoMuestra]) REFERENCES [dbo].[EstadoPedidoMuestra]([IdEstadoPedidoMuestra]);
END
");
        }

        public override void Down()
        {
            Sql(@"
IF EXISTS (
        SELECT 1
        FROM sys.foreign_keys
        WHERE name = N'FK_dbo.PedidoMuestra_dbo.EstadoPedidoMuestra_IdEstadoPedidoMuestra'
    )
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra]
        DROP CONSTRAINT [FK_dbo.PedidoMuestra_dbo.EstadoPedidoMuestra_IdEstadoPedidoMuestra];
END
");
            Sql(@"
IF EXISTS (
        SELECT 1
        FROM sys.indexes
        WHERE name = N'IX_IdEstadoPedidoMuestra'
          AND object_id = OBJECT_ID(N'[dbo].[PedidoMuestra]')
    )
BEGIN
    DROP INDEX [IX_IdEstadoPedidoMuestra] ON [dbo].[PedidoMuestra];
END
");

            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'IdEstadoPedidoMuestra') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra] DROP COLUMN [IdEstadoPedidoMuestra];
END
");

            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'TelefonoContacto') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra] DROP COLUMN [TelefonoContacto];
END
");
            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'EmailContacto') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra] DROP COLUMN [EmailContacto];
END
");
            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'PersonaContacto') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra] DROP COLUMN [PersonaContacto];
END
");

            Sql(@"
IF COL_LENGTH('dbo.PedidoMuestra', 'DireccionEntrega') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[PedidoMuestra] DROP COLUMN [DireccionEntrega];
END
");

            Sql(@"
IF OBJECT_ID(N'dbo.EstadoPedidoMuestra', 'U') IS NOT NULL
   AND EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = N'PK_dbo.EstadoPedidoMuestra')
BEGIN
    DROP TABLE [dbo].[EstadoPedidoMuestra];
END
");
        }
    }
}