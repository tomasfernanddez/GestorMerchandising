namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AgregarEstadosMuestras : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstadoMuestra",
                c => new
                {
                    IdEstadoMuestra = c.Guid(nullable: false),
                    NombreEstadoMuestra = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.IdEstadoMuestra);

            AddColumn("dbo.DetalleMuestra", "IdEstadoMuestra", c => c.Guid());
            CreateIndex("dbo.DetalleMuestra", "IdEstadoMuestra");
            AddForeignKey("dbo.DetalleMuestra", "IdEstadoMuestra", "dbo.EstadoMuestra", "IdEstadoMuestra");

            Sql(@"IF NOT EXISTS (SELECT 1 FROM dbo.EstadoMuestra WHERE IdEstadoMuestra = '9D4D3E7C-6C4B-4A1E-AF8E-5A9E0A7E7D9E')
BEGIN
    INSERT INTO dbo.EstadoMuestra (IdEstadoMuestra, NombreEstadoMuestra)
    VALUES ('9D4D3E7C-6C4B-4A1E-AF8E-5A9E0A7E7D9E', 'Pendiente');
END");

            Sql(@"IF NOT EXISTS (SELECT 1 FROM dbo.EstadoMuestra WHERE IdEstadoMuestra = '4F9E34B9-37C4-4D35-B1A5-4B0AB176F4BE')
BEGIN
    INSERT INTO dbo.EstadoMuestra (IdEstadoMuestra, NombreEstadoMuestra)
    VALUES ('4F9E34B9-37C4-4D35-B1A5-4B0AB176F4BE', 'Entregado');
END");

            Sql(@"IF NOT EXISTS (SELECT 1 FROM dbo.EstadoMuestra WHERE IdEstadoMuestra = '1CB9F9F0-DF5D-45A7-8A33-53E0FF6F82BE')
BEGIN
    INSERT INTO dbo.EstadoMuestra (IdEstadoMuestra, NombreEstadoMuestra)
    VALUES ('1CB9F9F0-DF5D-45A7-8A33-53E0FF6F82BE', 'Devuelto');
END");

            Sql(@"IF NOT EXISTS (SELECT 1 FROM dbo.EstadoMuestra WHERE IdEstadoMuestra = '6E6C6A59-1036-4F88-9B67-9F7856F04334')
BEGIN
    INSERT INTO dbo.EstadoMuestra (IdEstadoMuestra, NombreEstadoMuestra)
    VALUES ('6E6C6A59-1036-4F88-9B67-9F7856F04334', 'Facturado');
END");
        }

        public override void Down()
        {
            DropForeignKey("dbo.DetalleMuestra", "IdEstadoMuestra", "dbo.EstadoMuestra");
            DropIndex("dbo.DetalleMuestra", new[] { "IdEstadoMuestra" });
            DropColumn("dbo.DetalleMuestra", "IdEstadoMuestra");
            DropTable("dbo.EstadoMuestra");
        }
    }
}