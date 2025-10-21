using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Migrations
{
    public partial class AgregarIdiomaUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "IdiomaPreferido", c => c.String(maxLength: 10));

            // Valor por defecto para usuarios existentes
            Sql("UPDATE dbo.Usuario SET IdiomaPreferido = 'es-AR' WHERE IdiomaPreferido IS NULL");
        }

        public override void Down()
        {
            DropColumn("dbo.Usuario", "IdiomaPreferido");
        }
    }
}
