namespace DAL.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;

    [GeneratedCode("EntityFramework.Migrations", "6.5.1")]
    public sealed partial class AliasClientesYProveedores : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AliasClientesYProveedores));

        string IMigrationMetadata.Id => "202511012359000_AliasClientesYProveedores";

        string IMigrationMetadata.Source => null;

        string IMigrationMetadata.Target => Resources.GetString("Target");
    }
}