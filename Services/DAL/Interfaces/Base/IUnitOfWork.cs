using System;
using System.Threading.Tasks;

namespace Services.DAL.Interfaces.Base
{
    public interface IUnitOfWork : IDisposable
    {
        //    ============================================================================
        // REPOSITORIOS DE ARQUITECTURA BASE (NUEVOS)
        //    ============================================================================
        IUsuarioRepository Usuarios { get; }
        IBitacoraRepository Bitacoras { get; }
        IPerfilRepository Perfiles { get; }
        IFuncionRepository Funciones { get; }

        //    ============================================================================
        // OPERACIONES DE TRANSACCIÓN
        //    ============================================================================
        /// <summary>
        /// Persiste los cambios pendientes en la base de datos.
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Persiste de forma asíncrona los cambios pendientes en la base de datos.
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Inicia una transacción de base de datos.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Confirma la transacción activa guardando los cambios.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Revierte la transacción activa descartando los cambios.
        /// </summary>
        void RollbackTransaction();

        //    ============================================================================
        // MÉTODOS ESPECIALES PARA ARQUITECTURA BASE (NUEVOS)
        //    ============================================================================

        /// <summary>
        /// Inicializa los datos básicos del sistema (perfiles y usuario admin)
        /// </summary>
        void InicializarSistema();

        /// <summary>
        /// Registra una acción en la bitácora automáticamente
        /// </summary>
        void RegistrarAccion(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null);

        /// <summary>
        /// Registra una acción en la bitácora automáticamente (async)
        /// </summary>
        Task RegistrarAccionAsync(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null);
    }
}