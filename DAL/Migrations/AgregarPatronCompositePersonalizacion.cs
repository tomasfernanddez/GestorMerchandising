using System;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    /// <summary>
    /// Migración para implementar el Patrón Composite en el sistema de personalización
    /// Permite agrupar logos en paquetes y tratarlos de manera uniforme
    /// </summary>
    public partial class AgregarPatronCompositePersonalizacion : DbMigration
    {
        public override void Up()
        {
            // 1. Agregar campos a LogosPedido para implementar el patrón Composite (Leaf)
            AddColumn("dbo.LogosPedido", "CostoPersonalizacion", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
            AddColumn("dbo.LogosPedido", "Descripcion", c => c.String(maxLength: 200));

            // 2. Crear tabla GrupoPersonalizacion (Composite)
            CreateTable(
                "dbo.GrupoPersonalizacion",
                c => new
                {
                    IdGrupoPersonalizacion = c.Guid(nullable: false, identity: false, defaultValueSql: "NEWID()"),
                    IdDetallePedido = c.Guid(nullable: false),
                    Nombre = c.String(nullable: false, maxLength: 100),
                    Descripcion = c.String(maxLength: 500),
                    CostoAdicionalGrupo = c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0),
                    Activo = c.Boolean(nullable: false, defaultValue: true),
                    IdGrupoPadre = c.Guid(nullable: true),
                })
                .PrimaryKey(t => t.IdGrupoPersonalizacion)
                .ForeignKey("dbo.PedidoDetalle", t => t.IdDetallePedido, cascadeDelete: false)
                .ForeignKey("dbo.GrupoPersonalizacion", t => t.IdGrupoPadre, cascadeDelete: false)
                .Index(t => t.IdDetallePedido, name: "IX_GrupoPersonalizacion_IdDetallePedido")
                .Index(t => t.IdGrupoPadre, name: "IX_GrupoPersonalizacion_IdGrupoPadre");

            // 3. Crear tabla de relación many-to-many GrupoPersonalizacionLogo
            CreateTable(
                "dbo.GrupoPersonalizacionLogo",
                c => new
                {
                    IdGrupo = c.Guid(nullable: false),
                    IdLogo = c.Guid(nullable: false),
                    Orden = c.Int(nullable: false, defaultValue: 0),
                    FechaAgregado = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                })
                .PrimaryKey(t => new { t.IdGrupo, t.IdLogo })
                .ForeignKey("dbo.GrupoPersonalizacion", t => t.IdGrupo, cascadeDelete: true)
                .ForeignKey("dbo.LogosPedido", t => t.IdLogo, cascadeDelete: true)
                .Index(t => t.IdGrupo, name: "IX_GrupoPersonalizacionLogo_IdGrupo")
                .Index(t => t.IdLogo, name: "IX_GrupoPersonalizacionLogo_IdLogo");

            // 4. Inicializar valores por defecto para registros existentes
            Sql("UPDATE dbo.LogosPedido SET CostoPersonalizacion = 0 WHERE CostoPersonalizacion IS NULL");
        }

        public override void Down()
        {
            // Eliminar en orden inverso para respetar foreign keys
            DropForeignKey("dbo.GrupoPersonalizacionLogo", "IdLogo", "dbo.LogosPedido");
            DropForeignKey("dbo.GrupoPersonalizacionLogo", "IdGrupo", "dbo.GrupoPersonalizacion");
            DropForeignKey("dbo.GrupoPersonalizacion", "IdGrupoPadre", "dbo.GrupoPersonalizacion");
            DropForeignKey("dbo.GrupoPersonalizacion", "IdDetallePedido", "dbo.PedidoDetalle");

            DropIndex("dbo.GrupoPersonalizacionLogo", "IX_GrupoPersonalizacionLogo_IdLogo");
            DropIndex("dbo.GrupoPersonalizacionLogo", "IX_GrupoPersonalizacionLogo_IdGrupo");
            DropIndex("dbo.GrupoPersonalizacion", "IX_GrupoPersonalizacion_IdGrupoPadre");
            DropIndex("dbo.GrupoPersonalizacion", "IX_GrupoPersonalizacion_IdDetallePedido");

            DropTable("dbo.GrupoPersonalizacionLogo");
            DropTable("dbo.GrupoPersonalizacion");

            DropColumn("dbo.LogosPedido", "Descripcion");
            DropColumn("dbo.LogosPedido", "CostoPersonalizacion");
        }
    }
}
