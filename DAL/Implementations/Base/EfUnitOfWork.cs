using DAL.Implementations.Principales;
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

        // Repositorios futuros (por implementar)
        // private IProveedorRepository _proveedores;
        // private IProductoRepository _productos;
        // etc.

        public EfUnitOfWork(DbContext context)
        {
            Context = context;
        }

        public EfUnitOfWork(GestorMerchandisingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Propiedades de repositorios principales
        public IClienteRepository Clientes
        {
            get { return _clientes ?? (_clientes = new EfClienteRepository(_context)); }
        }

        // Implementaciones temporales (NotImplementedException hasta que creemos las clases)
        public IProveedorRepository Proveedores => throw new NotImplementedException();
        public IProductoRepository Productos => throw new NotImplementedException();
        public IPedidoRepository Pedidos => throw new NotImplementedException();
        public IPedidoDetalleRepository PedidoDetalles => throw new NotImplementedException();
        public IPedidoMuestraRepository PedidosMuestra => throw new NotImplementedException();
        public IFacturaCabeceraRepository FacturasCabecera => throw new NotImplementedException();
        public ITipoEmpresaRepository TiposEmpresa => throw new NotImplementedException();
        public ITipoProveedorRepository TiposProveedor => throw new NotImplementedException();
        public ICategoriaProductoRepository CategoriasProducto => throw new NotImplementedException();
        public IEstadoPedidoRepository EstadosPedido => throw new NotImplementedException();
        public IEstadoProductoRepository EstadosProducto => throw new NotImplementedException();

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