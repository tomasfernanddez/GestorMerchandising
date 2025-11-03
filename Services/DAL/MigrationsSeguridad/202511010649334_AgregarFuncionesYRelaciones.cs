namespace Services.DAL.MigrationsSeguridad
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AgregarFuncionesYRelaciones : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Funcion",
                c => new
                {
                    IdFuncion = c.Guid(nullable: false),
                    Codigo = c.String(nullable: false, maxLength: 100),
                    Nombre = c.String(nullable: false, maxLength: 200),
                    Descripcion = c.String(maxLength: 500),
                    Activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IdFuncion);

            CreateIndex("dbo.Funcion", "Codigo", unique: true, name: "IX_Funcion_Codigo");

            CreateTable(
                "dbo.PerfilFuncion",
                c => new
                {
                    IdPerfil = c.Guid(nullable: false),
                    IdFuncion = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.IdPerfil, t.IdFuncion })
                .ForeignKey("dbo.Perfil", t => t.IdPerfil, cascadeDelete: true)
                .ForeignKey("dbo.Funcion", t => t.IdFuncion, cascadeDelete: true)
                .Index(t => t.IdPerfil)
                .Index(t => t.IdFuncion);
        }

        public override void Down()
        {
            DropForeignKey("dbo.PerfilFuncion", "IdFuncion", "dbo.Funcion");
            DropForeignKey("dbo.PerfilFuncion", "IdPerfil", "dbo.Perfil");
            DropIndex("dbo.PerfilFuncion", new[] { "IdFuncion" });
            DropIndex("dbo.PerfilFuncion", new[] { "IdPerfil" });
            DropIndex("dbo.Funcion", "IX_Funcion_Codigo");
            DropTable("dbo.PerfilFuncion");
            DropTable("dbo.Funcion");
        }
    }
}