using DAL.Implementations.Referencia;
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
        ICondicionIvaRepository CondicionesIva { get; }
        ICategoriaProductoRepository CategoriasProducto { get; }
        IEstadoPedidoRepository EstadosPedido { get; }
        IEstadoProductoRepository EstadosProducto { get; }
        IEstadoPedidoMuestraRepository EstadosPedidoMuestra { get; }
        IEstadoMuestraRepository EstadosMuestra { get; }

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
    

    }
}
