namespace DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DomainModel.Entidades;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.GestorMerchandisingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.GestorMerchandisingContext context)
        {
            //  This method will be called after migrating to the latest version.

            var condicionesBase = new[]
            {
                new { Id = Guid.Parse("0C0E2E49-8AC7-45E9-9F13-5F69C09D5E0F"), Nombre = "Responsable Inscripto" },
                new { Id = Guid.Parse("1B5D0E6B-62B6-4C45-89AB-6E7FDD11830F"), Nombre = "Monotributista" },
                new { Id = Guid.Parse("2C1406F4-7EEA-4F9B-9408-06F6561B60DF"), Nombre = "Exento" },
                new { Id = Guid.Parse("3F9F0B5C-46BA-49F6-8AA7-CE33FC49A2F1"), Nombre = "Consumidor Final" },
                new { Id = Guid.Parse("4A1C3D5E-0F89-4AE2-93BE-43D9F16B8760"), Nombre = "No Responsable" },
                new { Id = Guid.Parse("5BB7BCF5-65E8-4C49-84A5-3F6E1C13F5E8"), Nombre = "Responsable No Inscripto" }
            };

            foreach (var cond in condicionesBase)
            {
                if (!context.CondicionesIva.Any(ci => ci.IdCondicionIva == cond.Id))
                {
                    context.CondicionesIva.Add(new CondicionIva
                    {
                        IdCondicionIva = cond.Id,
                        Nombre = cond.Nombre
                    });
                }
            }

            context.SaveChanges();
        }
    }
}
