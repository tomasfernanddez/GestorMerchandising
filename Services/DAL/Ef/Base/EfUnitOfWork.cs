using Services.DAL.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DAL.Ef.Base
{
    public class EfUnitOfWork : IUnitOfWork, IHasDbContext
    {

        public DbContext Context { get; private set; }

        private readonly ServicesContext _context;
        private DbContextTransaction _transaction;

        // Repositorios de arquitectura base
        private IUsuarioRepository _usuarios;
        private IBitacoraRepository _bitacoras;
        private IPerfilRepository _perfiles;
        private IFuncionRepository _funciones;

        // Repositorios principales

        // Repositorios futuros (por implementar)
        // private IProveedorRepository _proveedores;
        // private IProductoRepository _productos;
        // etc.

        /// <summary>
        /// Inicializa la unidad de trabajo utilizando un contexto genérico de Entity Framework.
        /// </summary>
        public EfUnitOfWork(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Inicializa la unidad de trabajo con el contexto específico del módulo de servicios.
        /// </summary>
        public EfUnitOfWork(ServicesContext context)
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

        public IFuncionRepository Funciones
        {
            get { return _funciones ?? (_funciones = new EfFuncionRepository(_context)); }
        }

        // Operaciones de guardado
        /// <summary>
        /// Persiste los cambios pendientes en la base de datos.
        /// </summary>
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
        /// Persiste de forma asíncrona los cambios pendientes en la base de datos.
        /// </summary>
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
        /// <summary>
        /// Inicia una transacción de base de datos.
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
        /// Confirma la transacción activa guardando los cambios.
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
        /// Revierte la transacción activa descartando los cambios.
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

        // Métodos especiales para arquitectura base
        /// <summary>
        /// Ejecuta la inicialización de datos base del sistema.
        /// </summary>
        public void InicializarSistema()
        {
            _context.InicializarDatos();
        }

        /// <summary>
        /// Registra una acción en la bitácora y persiste los cambios.
        /// </summary>
        public void RegistrarAccion(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null)
        {
            Bitacoras.RegistrarAccion(idUsuario, accion, descripcion, modulo, exitoso, mensajeError, direccionIP);
            SaveChanges();
        }

        /// <summary>
        /// Registra de forma asíncrona una acción en la bitácora y persiste los cambios.
        /// </summary>
        public async Task RegistrarAccionAsync(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null)
        {
            Bitacoras.RegistrarAccion(idUsuario, accion, descripcion, modulo, exitoso, mensajeError, direccionIP);
            await SaveChangesAsync();
        }

        // Dispose pattern
        private bool _disposed = false;

        /// <summary>
        /// Libera los recursos administrados y no administrados utilizados por la unidad de trabajo.
        /// </summary>
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
        /// Libera los recursos asociados y suprime el finalizador.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizador que garantiza la liberación de recursos.
        /// </summary>
        ~EfUnitOfWork()
        {
            Dispose(false);
        }
    }
}
