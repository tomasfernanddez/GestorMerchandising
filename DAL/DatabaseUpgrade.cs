using DAL.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace DAL
{
    /// <summary>
    /// Aplica migraciones pendientes en la BD de este contexto.
    /// Requiere que ya exista Migrations.Configuration en DAL (Enable-Migrations).
    /// </summary>
    public static class DatabaseUpgrade
    {
        public static void EnsureUpToDate(string connectionString)
        {
            var cfg = new Configuration(); // internal en DAL, pero accesible desde acá
            cfg.TargetDatabase = new DbConnectionInfo(connectionString, "System.Data.SqlClient");

            var migrator = new DbMigrator(cfg);
            migrator.Update(); // aplica SOLO las migraciones pendientes
        }
    }
}
