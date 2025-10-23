using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    /// <summary>
    /// Migración para ampliar la tabla de proveedores con información comercial y de ubicación.
    /// </summary>
    public partial class AgregarModuloProveedores : DbMigration
    {
        public override void Up()
        {
            // 0) Backfill por si existen proveedores con CondicionIva = NULL (evita error al NOT NULL)
            Sql("UPDATE dbo.Proveedor SET CondicionIva = 'Consumidor Final' WHERE CondicionIva IS NULL");

            // 1) Ajustar condición de IVA a no nulo
            AlterColumn("dbo.Proveedor", "CondicionIva", c => c.String(nullable: false, maxLength: 50));

            // 2) Datos adicionales y relaciones geográficas (columnas FK opcionales)
            AddColumn("dbo.Proveedor", "CodigoPostal", c => c.String(maxLength: 20));
            AddColumn("dbo.Proveedor", "CondicionesPago", c => c.String(nullable: false, maxLength: 50, defaultValue: "Contado"));
            AddColumn("dbo.Proveedor", "Observaciones", c => c.String(maxLength: 500));
            AddColumn("dbo.Proveedor", "FechaAlta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AddColumn("dbo.Proveedor", "IdPais", c => c.Guid());
            AddColumn("dbo.Proveedor", "IdProvincia", c => c.Guid());
            AddColumn("dbo.Proveedor", "IdLocalidad", c => c.Guid());

            // 3) Tabla intermedia para Técnicas de Personalización
            CreateTable(
                "dbo.ProveedorTecnicaPersonalizacion",
                c => new
                {
                    IdProveedor = c.Guid(nullable: false),
                    IdTecnicaPersonalizacion = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.IdProveedor, t.IdTecnicaPersonalizacion })
                .ForeignKey("dbo.Proveedor", t => t.IdProveedor, cascadeDelete: true)
                .ForeignKey("dbo.TecnicaPersonalizacion", t => t.IdTecnicaPersonalizacion, cascadeDelete: true)
                .Index(t => t.IdProveedor, name: "IX_ProveedorTecnica_IdProveedor")
                .Index(t => t.IdTecnicaPersonalizacion, name: "IX_ProveedorTecnica_IdTecnica");

            // 4) Índices para relaciones geográficas (mejoran FK lookups)
            CreateIndex("dbo.Proveedor", "IdPais", name: "IX_Proveedor_IdPais");
            CreateIndex("dbo.Proveedor", "IdProvincia", name: "IX_Proveedor_IdProvincia");
            CreateIndex("dbo.Proveedor", "IdLocalidad", name: "IX_Proveedor_IdLocalidad");

            // 5) FKs a tablas existentes (sin cascade delete)
            AddForeignKey("dbo.Proveedor", "IdPais", "dbo.Pais", "IdPais", cascadeDelete: false);
            AddForeignKey("dbo.Proveedor", "IdProvincia", "dbo.Provincia", "IdProvincia", cascadeDelete: false);
            AddForeignKey("dbo.Proveedor", "IdLocalidad", "dbo.Localidad", "IdLocalidad", cascadeDelete: false);
        }

        public override void Down()
        {
            // Revertir en orden inverso: FKs -> índices -> tabla intermedia -> columnas -> altercolumn

            // 1) FKs geográficas
            DropForeignKey("dbo.Proveedor", "IdLocalidad", "dbo.Localidad");
            DropForeignKey("dbo.Proveedor", "IdProvincia", "dbo.Provincia");
            DropForeignKey("dbo.Proveedor", "IdPais", "dbo.Pais");

            // 2) Índices geográficos
            DropIndex("dbo.Proveedor", "IX_Proveedor_IdLocalidad");
            DropIndex("dbo.Proveedor", "IX_Proveedor_IdProvincia");
            DropIndex("dbo.Proveedor", "IX_Proveedor_IdPais");

            // 3) Tabla intermedia
            DropForeignKey("dbo.ProveedorTecnicaPersonalizacion", "IdTecnicaPersonalizacion", "dbo.TecnicaPersonalizacion");
            DropForeignKey("dbo.ProveedorTecnicaPersonalizacion", "IdProveedor", "dbo.Proveedor");
            DropIndex("dbo.ProveedorTecnicaPersonalizacion", "IX_ProveedorTecnica_IdTecnica");
            DropIndex("dbo.ProveedorTecnicaPersonalizacion", "IX_ProveedorTecnica_IdProveedor");
            DropTable("dbo.ProveedorTecnicaPersonalizacion");

            // 4) Columnas agregadas
            DropColumn("dbo.Proveedor", "IdLocalidad");
            DropColumn("dbo.Proveedor", "IdProvincia");
            DropColumn("dbo.Proveedor", "IdPais");
            DropColumn("dbo.Proveedor", "FechaAlta");
            DropColumn("dbo.Proveedor", "Observaciones");
            DropColumn("dbo.Proveedor", "CondicionesPago");
            DropColumn("dbo.Proveedor", "CodigoPostal");

            // 5) Volver CondicionIva a nullable
            AlterColumn("dbo.Proveedor", "CondicionIva", c => c.String(maxLength: 50));
        }
    }
}