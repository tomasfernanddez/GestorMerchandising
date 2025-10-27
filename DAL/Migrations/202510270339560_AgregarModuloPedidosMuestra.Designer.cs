using System.CodeDom.Compiler;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace DAL.Migrations
{
    [GeneratedCode("EntityFramework.Migrations", "6.5.1")]
    public sealed partial class AgregarModuloPedidosMuestra : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AgregarModuloPedidosMuestra));

        string IMigrationMetadata.Id
        {
            get { return "202510270339560_AgregarModuloPedidosMuestra"; }
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