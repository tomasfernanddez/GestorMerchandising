namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Proveedor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Usuario", "IdPerfil", "dbo.Perfil");
            DropForeignKey("dbo.Bitacora", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.Sesion", "IdUsuario", "dbo.Usuario");
            DropIndex("dbo.Bitacora", new[] { "IdUsuario" });
            DropIndex("dbo.Usuario", new[] { "IdPerfil" });
            DropIndex("dbo.Sesion", new[] { "IdUsuario" });
            CreateTable(
                "dbo.CondicionIva",
                c => new
                {
                    IdCondicionIva = c.Guid(nullable: false),
                    Nombre = c.String(nullable: false, maxLength: 100),
                    Descripcion = c.String(maxLength: 250),
                })
                .PrimaryKey(t => t.IdCondicionIva);

            CreateTable(
                "dbo.GrupoPersonalizacion",
                c => new
                    {
                        IdGrupoPersonalizacion = c.Guid(nullable: false),
                        IdDetallePedido = c.Guid(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                        CostoAdicionalGrupo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Activo = c.Boolean(nullable: false),
                        IdGrupoPadre = c.Guid(),
                    })
                .PrimaryKey(t => t.IdGrupoPersonalizacion)
                .ForeignKey("dbo.PedidoDetalle", t => t.IdDetallePedido)
                .ForeignKey("dbo.GrupoPersonalizacion", t => t.IdGrupoPadre)
                .Index(t => t.IdDetallePedido)
                .Index(t => t.IdGrupoPadre);
            
            CreateTable(
                "dbo.GrupoPersonalizacionLogo",
                c => new
                    {
                        IdGrupo = c.Guid(nullable: false),
                        IdLogo = c.Guid(nullable: false),
                        Orden = c.Int(nullable: false),
                        FechaAgregado = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdGrupo, t.IdLogo })
                .ForeignKey("dbo.GrupoPersonalizacion", t => t.IdGrupo, cascadeDelete: true)
                .ForeignKey("dbo.LogosPedido", t => t.IdLogo, cascadeDelete: true)
                .Index(t => t.IdGrupo)
                .Index(t => t.IdLogo);
            
            CreateTable(
                "dbo.ProveedorTecnicaPersonalizacion",
                c => new
                    {
                        IdProveedor = c.Guid(nullable: false),
                        IdTecnicaPersonalizacion = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdProveedor, t.IdTecnicaPersonalizacion })
                .ForeignKey("dbo.Proveedor", t => t.IdProveedor)
                .ForeignKey("dbo.TecnicaPersonalizacion", t => t.IdTecnicaPersonalizacion)
                .Index(t => t.IdProveedor)
                .Index(t => t.IdTecnicaPersonalizacion);

            AddColumn("dbo.Cliente", "IdCondicionIva", c => c.Guid()); // nullable
            AddColumn("dbo.LogosPedido", "CostoPersonalizacion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LogosPedido", "Descripcion", c => c.String(maxLength: 200));
            AddColumn("dbo.Proveedor", "IdCondicionIva", c => c.Guid()); // nullable
            AddColumn("dbo.Proveedor", "CondicionesPago", c => c.String(maxLength: 50)); // nullable ahora
            AddColumn("dbo.Proveedor", "FechaAlta", c => c.DateTime());            // nullable ahora
            AddColumn("dbo.Proveedor", "CodigoPostal", c => c.String(maxLength: 20));
            AddColumn("dbo.Proveedor", "Observaciones", c => c.String(maxLength: 500));
            AddColumn("dbo.Proveedor", "IdPais", c => c.Guid());
            AddColumn("dbo.Proveedor", "IdProvincia", c => c.Guid());
            AddColumn("dbo.Proveedor", "IdLocalidad", c => c.Guid());

            // 3) Sembrar los mismos GUID del Seed (idempotente)
            Sql(@"
IF NOT EXISTS (SELECT 1 FROM dbo.CondicionIva WHERE IdCondicionIva = '0C0E2E49-8AC7-45E9-9F13-5F69C09D5E0F')
INSERT dbo.CondicionIva(IdCondicionIva,Nombre) VALUES('0C0E2E49-8AC7-45E9-9F13-5F69C09D5E0F','Responsable Inscripto');

IF NOT EXISTS (SELECT 1 FROM dbo.CondicionIva WHERE IdCondicionIva = '1B5D0E6B-62B6-4C45-89AB-6E7FDD11830F')
INSERT dbo.CondicionIva(IdCondicionIva,Nombre) VALUES('1B5D0E6B-62B6-4C45-89AB-6E7FDD11830F','Monotributista');

IF NOT EXISTS (SELECT 1 FROM dbo.CondicionIva WHERE IdCondicionIva = '2C1406F4-7EEA-4F9B-9408-06F6561B60DF')
INSERT dbo.CondicionIva(IdCondicionIva,Nombre) VALUES('2C1406F4-7EEA-4F9B-9408-06F6561B60DF','Exento');

IF NOT EXISTS (SELECT 1 FROM dbo.CondicionIva WHERE IdCondicionIva = '3F9F0B5C-46BA-49F6-8AA7-CE33FC49A2F1')
INSERT dbo.CondicionIva(IdCondicionIva,Nombre) VALUES('3F9F0B5C-46BA-49F6-8AA7-CE33FC49A2F1','Consumidor Final');

IF NOT EXISTS (SELECT 1 FROM dbo.CondicionIva WHERE IdCondicionIva = '4A1C3D5E-0F89-4AE2-93BE-43D9F16B8760')
INSERT dbo.CondicionIva(IdCondicionIva,Nombre) VALUES('4A1C3D5E-0F89-4AE2-93BE-43D9F16B8760','No Responsable');

IF NOT EXISTS (SELECT 1 FROM dbo.CondicionIva WHERE IdCondicionIva = '5BB7BCF5-65E8-4C49-84A5-3F6E1C13F5E8')
INSERT dbo.CondicionIva(IdCondicionIva,Nombre) VALUES('5BB7BCF5-65E8-4C49-84A5-3F6E1C13F5E8','Responsable No Inscripto');
");

            // 4) Backfill desde el texto viejo (si existía) o default a “Consumidor Final”
            // Cliente: tenía columna nvarchar(50) [CondicionIva] en tu script
            Sql(@"
UPDATE C SET C.IdCondicionIva =
    CASE 
        WHEN C.CondicionIva IN ('Responsable Inscripto','RI') THEN '0C0E2E49-8AC7-45E9-9F13-5F69C09D5E0F'
        WHEN C.CondicionIva IN ('Monotributista','Mono')     THEN '1B5D0E6B-62B6-4C45-89AB-6E7FDD11830F'
        WHEN C.CondicionIva IN ('Exento')                     THEN '2C1406F4-7EEA-4F9B-9408-06F6561B60DF'
        WHEN C.CondicionIva IN ('No Responsable')             THEN '4A1C3D5E-0F89-4AE2-93BE-43D9F16B8760'
        WHEN C.CondicionIva IN ('Responsable No Inscripto')   THEN '5BB7BCF5-65E8-4C49-84A5-3F6E1C13F5E8'
        ELSE '3F9F0B5C-46BA-49F6-8AA7-CE33FC49A2F1' -- Consumidor Final
    END
FROM dbo.Cliente C
WHERE C.IdCondicionIva IS NULL;
");

            // Proveedor: si también tenía texto (en tu migración lo eliminás)
            Sql(@"
UPDATE P SET P.IdCondicionIva =
    CASE 
        WHEN P.CondicionIva IN ('Responsable Inscripto','RI') THEN '0C0E2E49-8AC7-45E9-9F13-5F69C09D5E0F'
        WHEN P.CondicionIva IN ('Monotributista','Mono')     THEN '1B5D0E6B-62B6-4C45-89AB-6E7FDD11830F'
        WHEN P.CondicionIva IN ('Exento')                     THEN '2C1406F4-7EEA-4F9B-9408-06F6561B60DF'
        WHEN P.CondicionIva IN ('No Responsable')             THEN '4A1C3D5E-0F89-4AE2-93BE-43D9F16B8760'
        WHEN P.CondicionIva IN ('Responsable No Inscripto')   THEN '5BB7BCF5-65E8-4C49-84A5-3F6E1C13F5E8'
        ELSE '3F9F0B5C-46BA-49F6-8AA7-CE33FC49A2F1'
    END
FROM dbo.Proveedor P
WHERE P.IdCondicionIva IS NULL;
");

            // 1) Endurecer a NOT NULL (ya no hay NULLs)
            AlterColumn("dbo.Cliente", "IdCondicionIva", c => c.Guid(nullable: false));
            AlterColumn("dbo.Proveedor", "IdCondicionIva", c => c.Guid(nullable: false));

            // 2) (Si corresponde) backfill y endurecer otros campos
            Sql("UPDATE dbo.Proveedor SET CondicionesPago = ISNULL(CondicionesPago,'Contado');");
            Sql("UPDATE dbo.Proveedor SET FechaAlta = ISNULL(FechaAlta, GETDATE());");
            AlterColumn("dbo.Proveedor", "CondicionesPago", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Proveedor", "FechaAlta", c => c.DateTime(nullable: false));

            // 3) Índices y FKs recién ahora
            CreateIndex("dbo.Cliente", "IdCondicionIva");
            CreateIndex("dbo.Proveedor", "IdCondicionIva");
            CreateIndex("dbo.Proveedor", "IdPais");
            CreateIndex("dbo.Proveedor", "IdProvincia");
            CreateIndex("dbo.Proveedor", "IdLocalidad");

            AddForeignKey("dbo.Cliente", "IdCondicionIva", "dbo.CondicionIva", "IdCondicionIva");
            AddForeignKey("dbo.Proveedor", "IdCondicionIva", "dbo.CondicionIva", "IdCondicionIva");
            AddForeignKey("dbo.Proveedor", "IdPais", "dbo.Pais", "IdPais");
            AddForeignKey("dbo.Proveedor", "IdProvincia", "dbo.Provincia", "IdProvincia");
            AddForeignKey("dbo.Proveedor", "IdLocalidad", "dbo.Localidad", "IdLocalidad");

            // 7) Eliminar campos de texto viejos (recién ahora)
            DropColumn("dbo.Cliente", "CondicionIva");
            DropColumn("dbo.Proveedor", "CondicionIva");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Sesion",
                c => new
                    {
                        IdSesion = c.Guid(nullable: false),
                        IdUsuario = c.Guid(nullable: false),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(),
                        Activa = c.Boolean(nullable: false),
                        DireccionIP = c.String(maxLength: 100),
                        UserAgent = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.IdSesion);
            
            CreateTable(
                "dbo.Perfil",
                c => new
                    {
                        IdPerfil = c.Guid(nullable: false),
                        NombrePerfil = c.String(nullable: false, maxLength: 50),
                        Descripcion = c.String(maxLength: 200),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdPerfil);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        IdUsuario = c.Guid(nullable: false),
                        NombreUsuario = c.String(nullable: false, maxLength: 50),
                        PasswordHash = c.String(nullable: false, maxLength: 100),
                        NombreCompleto = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        Activo = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaUltimoAcceso = c.DateTime(),
                        IntentosLoginFallidos = c.Int(nullable: false),
                        Bloqueado = c.Boolean(nullable: false),
                        FechaBloqueo = c.DateTime(),
                        IdPerfil = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.IdUsuario);
            
            CreateTable(
                "dbo.Bitacora",
                c => new
                    {
                        IdBitacora = c.Guid(nullable: false),
                        IdUsuario = c.Guid(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Accion = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                        Modulo = c.String(maxLength: 50),
                        DireccionIP = c.String(maxLength: 100),
                        Exitoso = c.Boolean(nullable: false),
                        MensajeError = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.IdBitacora);
            
            AddColumn("dbo.Proveedor", "CondicionIva", c => c.String(maxLength: 50));
            AddColumn("dbo.Cliente", "CondicionIva", c => c.String(maxLength: 50));
            DropForeignKey("dbo.GrupoPersonalizacionLogo", "IdLogo", "dbo.LogosPedido");
            DropForeignKey("dbo.GrupoPersonalizacionLogo", "IdGrupo", "dbo.GrupoPersonalizacion");
            DropForeignKey("dbo.GrupoPersonalizacion", "IdGrupoPadre", "dbo.GrupoPersonalizacion");
            DropForeignKey("dbo.GrupoPersonalizacion", "IdDetallePedido", "dbo.PedidoDetalle");
            DropForeignKey("dbo.Cliente", "IdCondicionIva", "dbo.CondicionIva");
            DropForeignKey("dbo.ProveedorTecnicaPersonalizacion", "IdTecnicaPersonalizacion", "dbo.TecnicaPersonalizacion");
            DropForeignKey("dbo.ProveedorTecnicaPersonalizacion", "IdProveedor", "dbo.Proveedor");
            DropForeignKey("dbo.Proveedor", "IdProvincia", "dbo.Provincia");
            DropForeignKey("dbo.Proveedor", "IdPais", "dbo.Pais");
            DropForeignKey("dbo.Proveedor", "IdLocalidad", "dbo.Localidad");
            DropForeignKey("dbo.Localidad", "IdProvincia", "dbo.Provincia");
            DropForeignKey("dbo.Provincia", "IdPais", "dbo.Pais");
            DropForeignKey("dbo.Proveedor", "IdCondicionIva", "dbo.CondicionIva");
            DropIndex("dbo.ProveedorTecnicaPersonalizacion", new[] { "IdTecnicaPersonalizacion" });
            DropIndex("dbo.ProveedorTecnicaPersonalizacion", new[] { "IdProveedor" });
            DropIndex("dbo.GrupoPersonalizacionLogo", new[] { "IdLogo" });
            DropIndex("dbo.GrupoPersonalizacionLogo", new[] { "IdGrupo" });
            DropIndex("dbo.GrupoPersonalizacion", new[] { "IdGrupoPadre" });
            DropIndex("dbo.GrupoPersonalizacion", new[] { "IdDetallePedido" });
            DropIndex("dbo.Proveedor", new[] { "IdLocalidad" });
            DropIndex("dbo.Proveedor", new[] { "IdProvincia" });
            DropIndex("dbo.Proveedor", new[] { "IdPais" });
            DropIndex("dbo.Proveedor", new[] { "IdCondicionIva" });
            DropIndex("dbo.Cliente", new[] { "IdCondicionIva" });
            DropColumn("dbo.Proveedor", "IdLocalidad");
            DropColumn("dbo.Proveedor", "IdProvincia");
            DropColumn("dbo.Proveedor", "IdPais");
            DropColumn("dbo.Proveedor", "FechaAlta");
            DropColumn("dbo.Proveedor", "Observaciones");
            DropColumn("dbo.Proveedor", "CondicionesPago");
            DropColumn("dbo.Proveedor", "CodigoPostal");
            DropColumn("dbo.Proveedor", "IdCondicionIva");
            DropColumn("dbo.LogosPedido", "Descripcion");
            DropColumn("dbo.LogosPedido", "CostoPersonalizacion");
            DropColumn("dbo.Cliente", "IdLocalidad");
            DropColumn("dbo.Cliente", "IdProvincia");
            DropColumn("dbo.Cliente", "IdPais");
            DropColumn("dbo.Cliente", "IdCondicionIva");
            DropTable("dbo.ProveedorTecnicaPersonalizacion");
            DropTable("dbo.GrupoPersonalizacionLogo");
            DropTable("dbo.GrupoPersonalizacion");
            DropTable("dbo.Pais");
            DropTable("dbo.Provincia");
            DropTable("dbo.Localidad");
            DropTable("dbo.CondicionIva");
            CreateIndex("dbo.Sesion", "IdUsuario");
            CreateIndex("dbo.Usuario", "IdPerfil");
            CreateIndex("dbo.Bitacora", "IdUsuario");
            AddForeignKey("dbo.Sesion", "IdUsuario", "dbo.Usuario", "IdUsuario");
            AddForeignKey("dbo.Bitacora", "IdUsuario", "dbo.Usuario", "IdUsuario");
            AddForeignKey("dbo.Usuario", "IdPerfil", "dbo.Perfil", "IdPerfil");
        }
    }
}
