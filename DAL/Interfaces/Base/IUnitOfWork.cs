using DAL.Interfaces.Principales;
using DAL.Interfaces.Referencia;
using System;
using System.Threading.Tasks;

namespace DAL.Interfaces.Base
{
    /// <summary>
    /// Define el contrato para coordinar múltiples repositorios dentro de una transacción.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {

        /// <summary>
        /// Repositorio para operaciones sobre clientes.
        /// </summary>
        IClienteRepository Clientes { get; }

        /// <summary>
        /// Repositorio para operaciones sobre proveedores.
        /// </summary>
        IProveedorRepository Proveedores { get; }

        /// <summary>
        /// Repositorio para operaciones sobre productos.
        /// </summary>
        IProductoRepository Productos { get; }

        /// <summary>
        /// Repositorio para operaciones sobre pedidos.
        /// </summary>
        IPedidoRepository Pedidos { get; }

        /// <summary>
        /// Repositorio para operaciones sobre los detalles de los pedidos.
        /// </summary>
        IPedidoDetalleRepository PedidoDetalles { get; }

        /// <summary>
        /// Repositorio para operaciones sobre pedidos de muestra.
        /// </summary>
        IPedidoMuestraRepository PedidosMuestra { get; }

        /// <summary>
        /// Repositorio para operaciones sobre las facturas.
        /// </summary>
        IFacturaCabeceraRepository FacturasCabecera { get; }

        /// <summary>
        /// Repositorio para operaciones sobre archivos adjuntos.
        /// </summary>
        IArchivoAdjuntoRepository ArchivosAdjuntos { get; }

        /// <summary>
        /// Repositorio de tipos de empresa de referencia.
        /// </summary>
        ITipoEmpresaRepository TiposEmpresa { get; }

        /// <summary>
        /// Repositorio de tipos de proveedor de referencia.
        /// </summary>
        ITipoProveedorRepository TiposProveedor { get; }

        /// <summary>
        /// Repositorio de condiciones de IVA de referencia.
        /// </summary>
        ICondicionIvaRepository CondicionesIva { get; }

        /// <summary>
        /// Repositorio de categorías de producto de referencia.
        /// </summary>
        ICategoriaProductoRepository CategoriasProducto { get; }

        /// <summary>
        /// Repositorio de estados de pedido de referencia.
        /// </summary>
        IEstadoPedidoRepository EstadosPedido { get; }

        /// <summary>
        /// Repositorio de estados de producto de referencia.
        /// </summary>
        IEstadoProductoRepository EstadosProducto { get; }

        /// <summary>
        /// Repositorio de estados de pedido de muestra de referencia.
        /// </summary>
        IEstadoPedidoMuestraRepository EstadosPedidoMuestra { get; }

        /// <summary>
        /// Repositorio de estados de muestra de referencia.
        /// </summary>
        IEstadoMuestraRepository EstadosMuestra { get; }

        /// <summary>
        /// Guarda los cambios realizados en el contexto asociado.
        /// </summary>
        /// <returns>Número de entidades afectadas.</returns>
        int SaveChanges();

        /// <summary>
        /// Guarda de forma asíncrona los cambios realizados en el contexto asociado.
        /// </summary>
        /// <returns>Número de entidades afectadas.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Inicia una transacción explícita.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Confirma la transacción activa aplicando los cambios.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Revierte la transacción activa descartando los cambios.
        /// </summary>
        void RollbackTransaction();
    }
}
