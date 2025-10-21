using DAL.Implementations.Principales;
using DAL.Interfaces.Base;
using DAL.Interfaces.Principales;
using DAL.Interfaces.Referencia;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations.Base
{
    public class EfUnitOfWork : IHasDbContext
    {

        public DbContext Context { get; private set; }

        private readonly GestorMerchandisingContext _context;
        private DbContextTransaction _transaction;

        // Repositorios de arquitectura base
        private IUsuarioRepository _usuarios;
        private IBitacoraRepository _bitacoras;
        private IPerfilRepository _perfiles;

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

        // Propiedades de repositorios de arquitectura base
        public IUsuarioRepository Usuarios
        {
            get { return _usuarios ?? (_usuarios = new EfUsuarioRepository(_context)); }
        }

        public IBitacoraRepository Bitacoras
        {
            get { return _bitacoras ?? (_bitacoras = new EfBitacoraRepository(_context)); }
        }

        public IPerfilRepository Perfiles
        {
            get { return _perfiles ?? (_perfiles = new EfPerfilRepository(_context)); }
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

        // Métodos especiales para arquitectura base
        public void InicializarSistema()
        {
            _context.InicializarDatos();
        }

        public void RegistrarAccion(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null)
        {
            Bitacoras.RegistrarAccion(idUsuario, accion, descripcion, modulo, exitoso, mensajeError, direccionIP);
            SaveChanges();
        }

        public async Task RegistrarAccionAsync(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null)
        {
            Bitacoras.RegistrarAccion(idUsuario, accion, descripcion, modulo, exitoso, mensajeError, direccionIP);
            await SaveChangesAsync();
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