using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class AgregarCatalogoCondicionIva : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CondicionIva",
                c => new
                {
                    IdCondicionIva = c.Guid(nullable: false),
                    Nombre = c.String(nullable: false, maxLength: 100),
                    Descripcion = c.String(maxLength: 250)
                })
                .PrimaryKey(t => t.IdCondicionIva);

            // Valores base conocidos
            Sql("INSERT INTO dbo.CondicionIva (IdCondicionIva, Nombre) VALUES ('0C0E2E49-8AC7-45E9-9F13-5F69C09D5E0F', 'Responsable Inscripto')");
            Sql("INSERT INTO dbo.CondicionIva (IdCondicionIva, Nombre) VALUES ('1B5D0E6B-62B6-4C45-89AB-6E7FDD11830F', 'Monotributista')");
            Sql("INSERT INTO dbo.CondicionIva (IdCondicionIva, Nombre) VALUES ('2C1406F4-7EEA-4F9B-9408-06F6561B60DF', 'Exento')");
            Sql("INSERT INTO dbo.CondicionIva (IdCondicionIva, Nombre) VALUES ('3F9F0B5C-46BA-49F6-8AA7-CE33FC49A2F1', 'Consumidor Final')");
            Sql("INSERT INTO dbo.CondicionIva (IdCondicionIva, Nombre) VALUES ('4A1C3D5E-0F89-4AE2-93BE-43D9F16B8760', 'No Responsable')");
            Sql("INSERT INTO dbo.CondicionIva (IdCondicionIva, Nombre) VALUES ('5BB7BCF5-65E8-4C49-84A5-3F6E1C13F5E8', 'Responsable No Inscripto')");

            AddColumn("dbo.Cliente", "IdCondicionIva", c => c.Guid());
            AddColumn("dbo.Proveedor", "IdCondicionIva", c => c.Guid());

            // Insertar dinámicamente valores faltantes detectados en datos existentes
            Sql(@"
                INSERT INTO dbo.CondicionIva (IdCondicionIva, Nombre)
                SELECT NEWID(), Distintos.NombreExistente
                FROM (
                    SELECT DISTINCT LTRIM(RTRIM(CondicionIva)) AS NombreExistente
                    FROM dbo.Cliente
                    WHERE CondicionIva IS NOT NULL AND CondicionIva <> ''
                    UNION
                    SELECT DISTINCT LTRIM(RTRIM(CondicionIva)) AS NombreExistente
                    FROM dbo.Proveedor
                    WHERE CondicionIva IS NOT NULL AND CondicionIva <> ''
                ) AS Distintos
                WHERE NOT EXISTS (
                    SELECT 1 FROM dbo.CondicionIva ci WHERE ci.Nombre = Distintos.NombreExistente
                );
            ");

            // Asociar registros existentes
            Sql(@"
                UPDATE c SET IdCondicionIva = ci.IdCondicionIva
                FROM dbo.Cliente c
                INNER JOIN dbo.CondicionIva ci ON ci.Nombre = LTRIM(RTRIM(c.CondicionIva))
            ");

            Sql(@"
                UPDATE p SET IdCondicionIva = ci.IdCondicionIva
                FROM dbo.Proveedor p
                INNER JOIN dbo.CondicionIva ci ON ci.Nombre = LTRIM(RTRIM(p.CondicionIva))
            ");

            // Asignar valor por defecto a los que quedaron nulos
            Sql("UPDATE dbo.Cliente SET IdCondicionIva = '0C0E2E49-8AC7-45E9-9F13-5F69C09D5E0F' WHERE IdCondicionIva IS NULL");
            Sql("UPDATE dbo.Proveedor SET IdCondicionIva = '0C0E2E49-8AC7-45E9-9F13-5F69C09D5E0F' WHERE IdCondicionIva IS NULL");

            AlterColumn("dbo.Cliente", "IdCondicionIva", c => c.Guid(nullable: false));
            AlterColumn("dbo.Proveedor", "IdCondicionIva", c => c.Guid(nullable: false));

            CreateIndex("dbo.Cliente", "IdCondicionIva");
            CreateIndex("dbo.Proveedor", "IdCondicionIva");

            AddForeignKey("dbo.Cliente", "IdCondicionIva", "dbo.CondicionIva", "IdCondicionIva");
            AddForeignKey("dbo.Proveedor", "IdCondicionIva", "dbo.CondicionIva", "IdCondicionIva");

            DropColumn("dbo.Cliente", "CondicionIva");
            DropColumn("dbo.Proveedor", "CondicionIva");
        }

        public override void Down()
        {
            AddColumn("dbo.Proveedor", "CondicionIva", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Cliente", "CondicionIva", c => c.String(maxLength: 50));

            Sql(@"
                UPDATE c SET CondicionIva = ci.Nombre
                FROM dbo.Cliente c
                INNER JOIN dbo.CondicionIva ci ON ci.IdCondicionIva = c.IdCondicionIva
            ");

            Sql(@"
                UPDATE p SET CondicionIva = ci.Nombre
                FROM dbo.Proveedor p
                INNER JOIN dbo.CondicionIva ci ON ci.IdCondicionIva = p.IdCondicionIva
            ");

            DropForeignKey("dbo.Proveedor", "IdCondicionIva", "dbo.CondicionIva");
            DropForeignKey("dbo.Cliente", "IdCondicionIva", "dbo.CondicionIva");

            DropIndex("dbo.Proveedor", new[] { "IdCondicionIva" });
            DropIndex("dbo.Cliente", new[] { "IdCondicionIva" });

            DropColumn("dbo.Proveedor", "IdCondicionIva");
            DropColumn("dbo.Cliente", "IdCondicionIva");

            DropTable("dbo.CondicionIva");
        }
    }
}