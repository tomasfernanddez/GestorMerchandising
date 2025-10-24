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

        public DbContext Context { get; private set; }

        private readonly GestorMerchandisingContext _context;
        private DbContextTransaction _transaction;

        // Repositorios principales
        private IClienteRepository _clientes;
        private IProveedorRepository _proveedores;
        private IProductoRepository _productos;
        private IPedidoRepository _pedidos;
        private IPedidoDetalleRepository _pedidoDetalles;

        // Repositorios de referencia
        private ITipoProveedorRepository _tiposProveedor;
        private ICondicionIvaRepository _condicionesIva;
        private ICategoriaProductoRepository _categoriasProducto;
        private IEstadoPedidoRepository _estadosPedido;
        private IEstadoProductoRepository _estadosProducto;

        // Repositorios futuros (por implementar)
        // private IProductoRepository _productos;
        // etc.

        public EfUnitOfWork(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            _context = context as GestorMerchandisingContext ?? throw new ArgumentException("El DbContext debe ser GestorMerchandisingContext", nameof(context));
        }

        public EfUnitOfWork(GestorMerchandisingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Context = context;
        }

        // Propiedades de repositorios principales
        public IClienteRepository Clientes
        {
            get { return _clientes ?? (_clientes = new EfClienteRepository(_context)); }
        }

        public IProveedorRepository Proveedores
        {
            get { return _proveedores ?? (_proveedores = new EfProveedorRepository(_context)); }
        }
        public IProductoRepository Productos
        {
            get { return _productos ?? (_productos = new EfProductoRepository(_context)); }
        }

        public IPedidoRepository Pedidos
        {
            get { return _pedidos ?? (_pedidos = new EfPedidoRepository(_context)); }
        }

        public IPedidoDetalleRepository PedidoDetalles
        {
            get { return _pedidoDetalles ?? (_pedidoDetalles = new EfPedidoDetalleRepository(_context)); }
        }
        public IPedidoMuestraRepository PedidosMuestra => throw new NotImplementedException();
        public IFacturaCabeceraRepository FacturasCabecera => throw new NotImplementedException();

        private ITipoEmpresaRepository _tiposEmpresa;
        public ITipoEmpresaRepository TiposEmpresa
        {
            get { return _tiposEmpresa ?? (_tiposEmpresa = new EfTipoEmpresaRepository(_context)); }
        }

        public ITipoProveedorRepository TiposProveedor
        {
            get { return _tiposProveedor ?? (_tiposProveedor = new EfTipoProveedorRepository(_context)); }
        }
        public ICondicionIvaRepository CondicionesIva
        {
            get { return _condicionesIva ?? (_condicionesIva = new EfCondicionIvaRepository(_context)); }
        }
        public ICategoriaProductoRepository CategoriasProducto
        {
            get { return _categoriasProducto ?? (_categoriasProducto = new EfCategoriaProductoRepository(_context)); }
        }

        public IEstadoPedidoRepository EstadosPedido
        {
            get { return _estadosPedido ?? (_estadosPedido = new EfEstadoPedidoRepository(_context)); }
        }

        public IEstadoProductoRepository EstadosProducto
        {
            get { return _estadosProducto ?? (_estadosProducto = new EfEstadoProductoRepository(_context)); }
        }

        // Operaciones de guardado
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

        // Manejo de transacciones
        public void BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Ya existe una transacción activa.");
            }
            _transaction = _context.Database.BeginTransaction();
        }

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

        // Dispose pattern
        private bool _disposed = false;

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EfUnitOfWork()
        {
            Dispose(false);
        }
    }
}