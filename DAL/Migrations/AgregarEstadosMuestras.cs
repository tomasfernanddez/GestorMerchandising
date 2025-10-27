using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class AgregarEstadosMuestra : DbMigration
    {
        public override void Up()
        {
            Sql(@"
IF OBJECT_ID(N'dbo.EstadoMuestra', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[EstadoMuestra]
    (
        [IdEstadoMuestra] UNIQUEIDENTIFIER NOT NULL,
        [NombreEstadoMuestra] NVARCHAR(50) NOT NULL,
        CONSTRAINT [PK_dbo.EstadoMuestra] PRIMARY KEY CLUSTERED ([IdEstadoMuestra] ASC)
    );
END");

            Sql(@"
IF COL_LENGTH('dbo.DetalleMuestra', 'IdEstadoMuestra') IS NULL
BEGIN
    ALTER TABLE [dbo].[DetalleMuestra] ADD [IdEstadoMuestra] UNIQUEIDENTIFIER NULL;
END");

            Sql(@"
IF COL_LENGTH('dbo.DetalleMuestra', 'IdEstadoMuestra') IS NOT NULL
    AND NOT EXISTS (
        SELECT 1
        FROM sys.indexes
        WHERE name = N'IX_IdEstadoMuestra'
          AND object_id = OBJECT_ID(N'[dbo].[DetalleMuestra]'))
BEGIN
    CREATE INDEX [IX_IdEstadoMuestra] ON [dbo].[DetalleMuestra]([IdEstadoMuestra]);
END");

            Sql(@"
IF COL_LENGTH('dbo.DetalleMuestra', 'IdEstadoMuestra') IS NOT NULL
    AND OBJECT_ID(N'dbo.EstadoMuestra', 'U') IS NOT NULL
    AND NOT EXISTS (
        SELECT 1
        FROM sys.foreign_keys
        WHERE name = N'FK_dbo.DetalleMuestra_dbo.EstadoMuestra_IdEstadoMuestra')
BEGIN
    ALTER TABLE [dbo].[DetalleMuestra]
        ADD CONSTRAINT [FK_dbo.DetalleMuestra_dbo.EstadoMuestra_IdEstadoMuestra]
        FOREIGN KEY ([IdEstadoMuestra]) REFERENCES [dbo].[EstadoMuestra]([IdEstadoMuestra]);
END");

            Sql(@"
IF OBJECT_ID(N'dbo.EstadoMuestra', 'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[EstadoMuestra])
    BEGIN
        INSERT INTO [dbo].[EstadoMuestra] ([IdEstadoMuestra], [NombreEstadoMuestra]) VALUES
            ('9D4D3E7C-6C4B-4A1E-AF8E-5A9E0A7E7D9E', 'Pendiente'),
            ('4F9E34B9-37C4-4D35-B1A5-4B0AB176F4BE', 'Entregado'),
            ('1CB9F9F0-DF5D-45A7-8A33-53E0FF6F82BE', 'Devuelto'),
            ('6E6C6A59-1036-4F88-9B67-9F7856F04334', 'Facturado');
    END
END");
        }

        public override void Down()
        {
            Sql(@"
IF OBJECT_ID(N'dbo.DetalleMuestra', 'U') IS NOT NULL
BEGIN
    IF EXISTS (
        SELECT 1
        FROM sys.foreign_keys
        WHERE name = N'FK_dbo.DetalleMuestra_dbo.EstadoMuestra_IdEstadoMuestra')
    BEGIN
        ALTER TABLE [dbo].[DetalleMuestra]
            DROP CONSTRAINT [FK_dbo.DetalleMuestra_dbo.EstadoMuestra_IdEstadoMuestra];
    END

    IF EXISTS (
        SELECT 1
        FROM sys.indexes
        WHERE name = N'IX_IdEstadoMuestra'
          AND object_id = OBJECT_ID(N'[dbo].[DetalleMuestra]'))
    BEGIN
        DROP INDEX [IX_IdEstadoMuestra] ON [dbo].[DetalleMuestra];
    END

    IF COL_LENGTH('dbo.DetalleMuestra', 'IdEstadoMuestra') IS NOT NULL
    BEGIN
        ALTER TABLE [dbo].[DetalleMuestra] DROP COLUMN [IdEstadoMuestra];
    END
END");

            Sql(@"
IF OBJECT_ID(N'dbo.EstadoMuestra', 'U') IS NOT NULL
    AND EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = N'PK_dbo.EstadoMuestra')
BEGIN
    DROP TABLE [dbo].[EstadoMuestra];
END");
        }
    }
}
