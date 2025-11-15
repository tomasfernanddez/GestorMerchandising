using DAL.Implementations.Principales;
using DAL.Implementations.Referencia;
using DAL.Interfaces.Base;
using DAL.Interfaces.Principales;
using DAL.Interfaces.Referencia;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DAL.Implementations.Base
{
    public class EfUnitOfWork : IUnitOfWork, IHasDbContext
    {
        /// <summary>
        /// Obtiene el contexto de Entity Framework asociado a la unidad de trabajo.
        /// </summary>
        public DbContext Context { get; private set; }

        private readonly GestorMerchandisingContext _context;
        private DbContextTransaction _transaction;

        // Repositorios principales
        private IClienteRepository _clientes;
        private IProveedorRepository _proveedores;
        private IProductoRepository _productos;
        private IPedidoRepository _pedidos;
        private IPedidoDetalleRepository _pedidoDetalles;
        private IPedidoMuestraRepository _pedidosMuestra;
        private IFacturaCabeceraRepository _facturasCabecera;
        private IArchivoAdjuntoRepository _archivosAdjuntos;

        // Repositorios de referencia
        private ITipoProveedorRepository _tiposProveedor;
        private ICondicionIvaRepository _condicionesIva;
        private ICategoriaProductoRepository _categoriasProducto;
        private IEstadoPedidoRepository _estadosPedido;
        private IEstadoProductoRepository _estadosProducto;
        private IEstadoPedidoMuestraRepository _estadosPedidoMuestra;
        private IEstadoMuestraRepository _estadosMuestra;
        private ITipoEmpresaRepository _tiposEmpresa;

        /// <summary>
        /// Inicializa la unidad de trabajo utilizando una instancia genérica de DbContext.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework compatible con la aplicación.</param>
        public EfUnitOfWork(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            _context = context as GestorMerchandisingContext ?? throw new ArgumentException("El DbContext debe ser GestorMerchandisingContext", nameof(context));
        }

        /// <summary>
        /// Inicializa la unidad de trabajo con el contexto específico de Gestor Merchandising.
        /// </summary>
        /// <param name="context">Contexto concreto de la aplicación.</param>
        public EfUnitOfWork(GestorMerchandisingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Context = context;
        }

        /// <summary>
        /// Proporciona acceso al repositorio de clientes.
        /// </summary>
        public IClienteRepository Clientes
        {
            get { return _clientes ?? (_clientes = new EfClienteRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de proveedores.
        /// </summary>
        public IProveedorRepository Proveedores
        {
            get { return _proveedores ?? (_proveedores = new EfProveedorRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de productos.
        /// </summary>
        public IProductoRepository Productos
        {
            get { return _productos ?? (_productos = new EfProductoRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de pedidos.
        /// </summary>
        public IPedidoRepository Pedidos
        {
            get { return _pedidos ?? (_pedidos = new EfPedidoRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de detalles de pedido.
        /// </summary>
        public IPedidoDetalleRepository PedidoDetalles
        {
            get { return _pedidoDetalles ?? (_pedidoDetalles = new EfPedidoDetalleRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de pedidos de muestra.
        /// </summary>
        public IPedidoMuestraRepository PedidosMuestra
        {
            get { return _pedidosMuestra ?? (_pedidosMuestra = new EfPedidoMuestraRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de facturas cabecera.
        /// </summary>
        public IFacturaCabeceraRepository FacturasCabecera
        {
            get { return _facturasCabecera ?? (_facturasCabecera = new EfFacturaCabeceraRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de archivos adjuntos.
        /// </summary>
        public IArchivoAdjuntoRepository ArchivosAdjuntos
        {
            get { return _archivosAdjuntos ?? (_archivosAdjuntos = new EfArchivoAdjuntoRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de tipos de empresa.
        /// </summary>
        public ITipoEmpresaRepository TiposEmpresa
        {
            get { return _tiposEmpresa ?? (_tiposEmpresa = new EfTipoEmpresaRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de tipos de proveedor.
        /// </summary>
        public ITipoProveedorRepository TiposProveedor
        {
            get { return _tiposProveedor ?? (_tiposProveedor = new EfTipoProveedorRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de condiciones de IVA.
        /// </summary>
        public ICondicionIvaRepository CondicionesIva
        {
            get { return _condicionesIva ?? (_condicionesIva = new EfCondicionIvaRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de categorías de producto.
        /// </summary>
        public ICategoriaProductoRepository CategoriasProducto
        {
            get { return _categoriasProducto ?? (_categoriasProducto = new EfCategoriaProductoRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de estados de pedido.
        /// </summary>
        public IEstadoPedidoRepository EstadosPedido
        {
            get { return _estadosPedido ?? (_estadosPedido = new EfEstadoPedidoRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de estados de producto.
        /// </summary>
        public IEstadoProductoRepository EstadosProducto
        {
            get { return _estadosProducto ?? (_estadosProducto = new EfEstadoProductoRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de estados de pedido de muestra.
        /// </summary>
        public IEstadoPedidoMuestraRepository EstadosPedidoMuestra
        {
            get { return _estadosPedidoMuestra ?? (_estadosPedidoMuestra = new EfEstadoPedidoMuestraRepository(_context)); }
        }

        /// <summary>
        /// Proporciona acceso al repositorio de estados de muestra.
        /// </summary>
        public IEstadoMuestraRepository EstadosMuestra
        {
            get { return _estadosMuestra ?? (_estadosMuestra = new EfEstadoMuestraRepository(_context)); }
        }

        /// <summary>
        /// Guarda los cambios realizados en el contexto de forma sincrónica.
        /// </summary>
        /// <returns>Número de entidades afectadas.</returns>
        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar los cambios en la base de datos.", ex);
            }
        }

        /// <summary>
        /// Guarda los cambios realizados en el contexto de forma asíncrona.
        /// </summary>
        /// <returns>Número de entidades afectadas.</returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar los cambios en la base de datos.", ex);
            }
        }

        /// <summary>
        /// Inicia una nueva transacción en la base de datos.
        /// </summary>
        public void BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Ya existe una transacción activa.");
            }
            _transaction = _context.Database.BeginTransaction();
        }

        /// <summary>
        /// Confirma la transacción activa guardando los cambios y aplicando el commit.
        /// </summary>
        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No hay ninguna transacción activa para confirmar.");
            }

            try
            {
                SaveChanges();
                _transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Revierte la transacción activa descartando los cambios pendientes.
        /// </summary>
        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Rollback();
                }
                finally
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        private bool _disposed = false;

        /// <summary>
        /// Libera los recursos administrados y no administrados utilizados por la unidad de trabajo.
        /// </summary>
        /// <param name="disposing">Indica si se deben liberar recursos administrados.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Libera los recursos asociados a la unidad de trabajo.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizador que asegura la liberación de recursos no administrados.
        /// </summary>
        ~EfUnitOfWork()
        {
            Dispose(false);
        }
    }
}