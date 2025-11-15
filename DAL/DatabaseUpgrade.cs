using DAL.Migrations;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

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
            if (!DatabaseExists(connectionString))
            {
                Trace.TraceInformation("La base de datos indicada en la cadena de conexión no existe todavía. Se omite la ejecución automática de migraciones.");
                return;
            }

            if (ShouldSkipAutomaticUpgrade(connectionString))
            {
                Trace.TraceWarning("Se omitió la ejecución automática de migraciones porque la base existente no posee historial de migraciones pero sí tablas de usuario. Revise la configuración de Entity Framework.");
                return;
            }

            var cfg = new Configuration(); // internal en DAL, pero accesible desde acá
            cfg.TargetDatabase = new DbConnectionInfo(connectionString, "System.Data.SqlClient");

            var migrator = new DbMigrator(cfg);
            var pendingMigrations = migrator.GetPendingMigrations().ToList();

            if (!pendingMigrations.Any())
            {
                Trace.TraceInformation("No hay migraciones pendientes para la base de datos destino.");
                return;
            }

            try
            {
                migrator.Update(); // aplica SOLO las migraciones pendientes
            }
            catch (AutomaticMigrationsDisabledException ex)
            {
                var mensaje = "Existen cambios en el modelo que no cuentan con una migración explícita. " +
                              "Genere una nueva migración (Add-Migration) y ejecútela antes de iniciar la aplicación.";
                throw new InvalidOperationException(mensaje, ex);
            }
        }

        private static bool ShouldSkipAutomaticUpgrade(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var hasMigrationHistory = TableExists(connection, "__MigrationHistory");
                var userTableCount = CountUserTables(connection);

                return !hasMigrationHistory && userTableCount > 0;
            }
        }

        private static bool TableExists(SqlConnection connection, string tableName)
        {
            using (var command = new SqlCommand("SELECT COUNT(*) FROM sys.tables WHERE name = @tableName", connection))
            {
                command.Parameters.AddWithValue("@tableName", tableName);
                var result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }

        private static int CountUserTables(SqlConnection connection)
        {
            const string sql = "SELECT COUNT(*) FROM sys.tables WHERE is_ms_shipped = 0";
            using (var command = new SqlCommand(sql, connection))
            {
                return (int)command.ExecuteScalar();
            }
        }
        private static bool DatabaseExists(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                // Si no se especifica base de datos asumimos que la conexión es válida.
                return true;
            }

            try
            {
                builder.InitialCatalog = "master";

                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT COUNT(*) FROM sys.databases WHERE name = @database", connection))
                    {
                        command.Parameters.AddWithValue("@database", databaseName);
                        var result = (int)command.ExecuteScalar();
                        return result > 0;
                    }
                }
            }
            catch (SqlException)
            {
                // Si no podemos conectar al servidor asumimos que la base no existe o no es accesible.
                return false;
            }
        }
    }
}