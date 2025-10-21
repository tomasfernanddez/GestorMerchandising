using DAL.Interfaces.Principales;
using DAL.Interfaces.Referencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Base
{
    public interface IUnitOfWork : IDisposable
    {
    //    ============================================================================
    // REPOSITORIOS DE ARQUITECTURA BASE (NUEVOS)
    //    ============================================================================
        IUsuarioRepository Usuarios { get; }
        IBitacoraRepository Bitacoras { get; }
        IPerfilRepository Perfiles { get; }

    //    ============================================================================
    // REPOSITORIOS PRINCIPALES
    //    ============================================================================
        IClienteRepository Clientes { get; }
        IProveedorRepository Proveedores { get; }
        IProductoRepository Productos { get; }
        IPedidoRepository Pedidos { get; }
        IPedidoDetalleRepository PedidoDetalles { get; }
        IPedidoMuestraRepository PedidosMuestra { get; }
        IFacturaCabeceraRepository FacturasCabecera { get; }

    //    ============================================================================
    // REPOSITORIOS DE REFERENCIA
    //    ============================================================================
        ITipoEmpresaRepository TiposEmpresa { get; }
        ITipoProveedorRepository TiposProveedor { get; }
        ICategoriaProductoRepository CategoriasProducto { get; }
        IEstadoPedidoRepository EstadosPedido { get; }
        IEstadoProductoRepository EstadosProducto { get; }

    //    ============================================================================
    // OPERACIONES DE TRANSACCIÓN
    //    ============================================================================
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        void CommitTransaction();
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
