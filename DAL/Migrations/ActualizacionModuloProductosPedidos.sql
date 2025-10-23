-- ============================================================================
-- SCRIPT SQL PARA APLICAR MANUALMENTE LA MIGRACIÓN
-- ActualizacionModuloProductosPedidos
-- ============================================================================
-- Este script puede ejecutarse manualmente si prefieres no usar la migración automática
-- IMPORTANTE: Ejecutar SOLO si la migración automática no funcionó
-- ============================================================================

BEGIN TRANSACTION;

-- ============================================================================
-- ACTUALIZACIÓN DE PRODUCTO
-- ============================================================================

-- Ampliar NombreProducto de 100 a 150 caracteres
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Producto') AND name = 'NombreProducto')
BEGIN
    ALTER TABLE dbo.Producto ALTER COLUMN NombreProducto NVARCHAR(150) NOT NULL;
END
GO

-- ============================================================================
-- ACTUALIZACIÓN DE PEDIDO
-- ============================================================================

-- Agregar NumeroPedido (correlativo: PED-0001, PED-0002, etc.)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Pedido') AND name = 'NumeroPedido')
BEGIN
    ALTER TABLE dbo.Pedido ADD NumeroPedido NVARCHAR(20) NULL;
END
GO

-- Agregar Observaciones
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Pedido') AND name = 'Observaciones')
BEGIN
    ALTER TABLE dbo.Pedido ADD Observaciones NVARCHAR(1000) NULL;
END
GO

-- Agregar Facturado
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Pedido') AND name = 'Facturado')
BEGIN
    ALTER TABLE dbo.Pedido ADD Facturado BIT NOT NULL DEFAULT 0;
END
GO

-- Agregar RutaFacturaPDF
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Pedido') AND name = 'RutaFacturaPDF')
BEGIN
    ALTER TABLE dbo.Pedido ADD RutaFacturaPDF NVARCHAR(500) NULL;
END
GO

-- Backfill NumeroPedido para pedidos existentes
UPDATE dbo.Pedido
SET NumeroPedido = 'PED-' + RIGHT('0000' + CAST(ROW_NUMBER() OVER (ORDER BY Fecha) AS VARCHAR), 4)
WHERE NumeroPedido IS NULL;
GO

-- Hacer NumeroPedido NOT NULL después del backfill
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Pedido') AND name = 'NumeroPedido' AND is_nullable = 1)
BEGIN
    ALTER TABLE dbo.Pedido ALTER COLUMN NumeroPedido NVARCHAR(20) NOT NULL;
END
GO

-- Crear índice único en NumeroPedido
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Pedido') AND name = 'IX_NumeroPedido')
BEGIN
    CREATE UNIQUE INDEX IX_NumeroPedido ON dbo.Pedido(NumeroPedido);
END
GO

-- ============================================================================
-- ACTUALIZACIÓN DE PEDIDO DETALLE
-- ============================================================================

-- Agregar FechaLimiteProducto (fecha límite individual por producto)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.PedidoDetalle') AND name = 'FechaLimiteProducto')
BEGIN
    ALTER TABLE dbo.PedidoDetalle ADD FechaLimiteProducto DATETIME NULL;
END
GO

-- ============================================================================
-- ACTUALIZACIÓN DE LOGOS PEDIDO
-- ============================================================================

-- Agregar IdProveedorPersonalizacion (proveedor que hace la personalización)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.LogosPedido') AND name = 'IdProveedorPersonalizacion')
BEGIN
    ALTER TABLE dbo.LogosPedido ADD IdProveedorPersonalizacion UNIQUEIDENTIFIER NULL;
END
GO

-- Crear foreign key a Proveedor
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_LogosPedido_Proveedor_IdProveedorPersonalizacion'))
BEGIN
    ALTER TABLE dbo.LogosPedido
    ADD CONSTRAINT FK_LogosPedido_Proveedor_IdProveedorPersonalizacion
    FOREIGN KEY (IdProveedorPersonalizacion)
    REFERENCES dbo.Proveedor(IdProveedor);
END
GO

-- Crear índice en IdProveedorPersonalizacion
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.LogosPedido') AND name = 'IX_IdProveedorPersonalizacion')
BEGIN
    CREATE INDEX IX_IdProveedorPersonalizacion ON dbo.LogosPedido(IdProveedorPersonalizacion);
END
GO

COMMIT TRANSACTION;
PRINT 'Migración ActualizacionModuloProductosPedidos aplicada correctamente.';
