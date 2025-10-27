namespace DAL.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;

    [GeneratedCode("EntityFramework.Migrations", "6.5.1")]
    public sealed partial class AgregarEstadosMuestras : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AgregarEstadosMuestras));

        string IMigrationMetadata.Id
        {
            get { return "202510270247039_AgregarEstadosMuestras"; }
        }

        string IMigrationMetadata.Source
        {
            get { return null; }
        }

        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}