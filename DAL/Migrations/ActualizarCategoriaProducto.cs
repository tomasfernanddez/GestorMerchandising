using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    /// <summary>
    /// Sincroniza la tabla CategoriaProducto con la entidad de dominio actual.
    /// Maneja entornos existentes verificando cada cambio antes de aplicarlo.
    /// </summary>
    public partial class ActualizarCategoriaProducto : DbMigration
    {
        public override void Up()
        {
            // Renombrar la columna antigua si todavía existe
            Sql(@"
IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'NombreCategoriaProducto' AND Object_ID = OBJECT_ID(N'dbo.CategoriaProducto'))
BEGIN
    EXEC sp_rename 'dbo.CategoriaProducto.NombreCategoriaProducto', 'NombreCategoria', 'COLUMN';
END");

            // Asegurar longitud y obligatoriedad del nombre
            Sql(@"
IF COL_LENGTH('dbo.CategoriaProducto', 'NombreCategoria') IS NOT NULL
BEGIN
    ALTER TABLE dbo.CategoriaProducto ALTER COLUMN NombreCategoria NVARCHAR(100) NOT NULL;
END");

            // Agregar columna Activo si no existe
            Sql(@"
IF COL_LENGTH('dbo.CategoriaProducto', 'Activo') IS NULL
BEGIN
    ALTER TABLE dbo.CategoriaProducto ADD Activo BIT NOT NULL CONSTRAINT DF_CategoriaProducto_Activo DEFAULT(1);
END
UPDATE dbo.CategoriaProducto SET Activo = 1 WHERE Activo IS NULL;");

            // Agregar columna Orden si no existe
            Sql(@"
IF COL_LENGTH('dbo.CategoriaProducto', 'Orden') IS NULL
BEGIN
    ALTER TABLE dbo.CategoriaProducto ADD Orden INT NOT NULL CONSTRAINT DF_CategoriaProducto_Orden DEFAULT(0);
END");

            // Asignar un orden incremental a las categorías sin ordenar
            Sql(@"
IF COL_LENGTH('dbo.CategoriaProducto', 'Orden') IS NOT NULL
BEGIN
    WITH OrderedCategorias AS (
        SELECT IdCategoria, ROW_NUMBER() OVER (ORDER BY FechaCreacion, NombreCategoria, IdCategoria) AS OrdenCalculado
        FROM dbo.CategoriaProducto
    )
    UPDATE c
    SET Orden = CASE WHEN c.Orden = 0 THEN o.OrdenCalculado ELSE c.Orden END
    FROM dbo.CategoriaProducto c
    INNER JOIN OrderedCategorias o ON c.IdCategoria = o.IdCategoria;
END");

            // Agregar columna FechaCreacion si no existe
            Sql(@"
IF COL_LENGTH('dbo.CategoriaProducto', 'FechaCreacion') IS NULL
BEGIN
    ALTER TABLE dbo.CategoriaProducto ADD FechaCreacion DATETIME NOT NULL CONSTRAINT DF_CategoriaProducto_FechaCreacion DEFAULT (GETUTCDATE());
END
UPDATE dbo.CategoriaProducto SET FechaCreacion = ISNULL(FechaCreacion, GETUTCDATE());");
        }

        public override void Down()
        {
            // Revertir FechaCreacion
            Sql(@"
IF COL_LENGTH('dbo.CategoriaProducto', 'FechaCreacion') IS NOT NULL
BEGIN
    DECLARE @constraint NVARCHAR(128);
    SELECT @constraint = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.CategoriaProducto') AND c.name = 'FechaCreacion';
    IF @constraint IS NOT NULL EXEC('ALTER TABLE dbo.CategoriaProducto DROP CONSTRAINT ' + QUOTENAME(@constraint));
    ALTER TABLE dbo.CategoriaProducto DROP COLUMN FechaCreacion;
END");

            // Revertir Orden
            Sql(@"
IF COL_LENGTH('dbo.CategoriaProducto', 'Orden') IS NOT NULL
BEGIN
    DECLARE @constraint NVARCHAR(128);
    SELECT @constraint = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.CategoriaProducto') AND c.name = 'Orden';
    IF @constraint IS NOT NULL EXEC('ALTER TABLE dbo.CategoriaProducto DROP CONSTRAINT ' + QUOTENAME(@constraint));
    ALTER TABLE dbo.CategoriaProducto DROP COLUMN Orden;
END");

            // Revertir Activo
            Sql(@"
IF COL_LENGTH('dbo.CategoriaProducto', 'Activo') IS NOT NULL
BEGIN
    DECLARE @constraint NVARCHAR(128);
    SELECT @constraint = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.CategoriaProducto') AND c.name = 'Activo';
    IF @constraint IS NOT NULL EXEC('ALTER TABLE dbo.CategoriaProducto DROP CONSTRAINT ' + QUOTENAME(@constraint));
    ALTER TABLE dbo.CategoriaProducto DROP COLUMN Activo;
END");

            // Intentar restaurar el nombre anterior
            Sql(@"
IF COL_LENGTH('dbo.CategoriaProducto', 'NombreCategoria') IS NOT NULL
BEGIN
    EXEC sp_rename 'dbo.CategoriaProducto.NombreCategoria', 'NombreCategoriaProducto', 'COLUMN';
END");
        }
    }
}