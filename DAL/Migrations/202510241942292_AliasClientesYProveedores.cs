namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AliasClientesYProveedores : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "Alias", c => c.String(maxLength: 100));
            AddColumn("dbo.Proveedor", "Alias", c => c.String(maxLength: 100));

            Sql("UPDATE dbo.Cliente SET Alias = NULL WHERE LTRIM(RTRIM(ISNULL(Alias,''))) = '';");
            Sql("UPDATE dbo.Proveedor SET Alias = NULL WHERE LTRIM(RTRIM(ISNULL(Alias,''))) = '';");
        }

        public override void Down()
        {
            DropColumn("dbo.Proveedor", "Alias");
            DropColumn("dbo.Cliente", "Alias");
        }
    }
}