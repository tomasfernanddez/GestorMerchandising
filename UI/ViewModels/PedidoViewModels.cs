using System;
using System.Collections.Generic;

namespace UI.ViewModels
{
    /// <summary>
    /// Representa la información de un detalle de pedido para la interfaz.
    /// </summary>
    public class PedidoDetalleViewModel
    {
        /// <summary>
        /// Identificador del detalle de pedido.
        /// </summary>
        public Guid IdDetallePedido { get; set; }
        /// <summary>
        /// Identificador del producto seleccionado.
        /// </summary>
        public Guid? IdProducto { get; set; }
        /// <summary>
        /// Nombre del producto asociado.
        /// </summary>
        public string NombreProducto { get; set; }
        /// <summary>
        /// Identificador de la categoría del producto.
        /// </summary>
        public Guid? IdCategoria { get; set; }
        /// <summary>
        /// Nombre de la categoría del producto.
        /// </summary>
        public string Categoria { get; set; }
        /// <summary>
        /// Identificador del proveedor.
        /// </summary>
        public Guid? IdProveedor { get; set; }
        /// <summary>
        /// Nombre del proveedor.
        /// </summary>
        public string Proveedor { get; set; }
        /// <summary>
        /// Cantidad solicitada.
        /// </summary>
        public int Cantidad { get; set; }
        /// <summary>
        /// Precio unitario acordado.
        /// </summary>
        public decimal PrecioUnitario { get; set; }
        /// <summary>
        /// Identificador del estado del producto.
        /// </summary>
        public Guid? IdEstadoProducto { get; set; }
        /// <summary>
        /// Estado del producto.
        /// </summary>
        public string EstadoProducto { get; set; }
        /// <summary>
        /// Fecha límite estimada.
        /// </summary>
        public DateTime? FechaLimite { get; set; }
        /// <summary>
        /// Indica si requiere ficha de aplicación.
        /// </summary>
        public bool FichaAplicacion { get; set; }
        /// <summary>
        /// Observaciones asociadas.
        /// </summary>
        public string Notas { get; set; }
        /// <summary>
        /// Proveedor encargado de la personalización, si aplica.
        /// </summary>
        public Guid? IdProveedorPersonalizacion { get; set; }
        /// <summary>
        /// Nombre del proveedor de personalización.
        /// </summary>
        public string ProveedorPersonalizacion { get; set; }
        /// <summary>
        /// Logos asociados al detalle.
        /// </summary>
        public List<PedidoLogoViewModel> Logos { get; set; } = new List<PedidoLogoViewModel>();
        /// <summary>
        /// Cantidad total de logos asociados.
        /// </summary>
        public int CantidadLogos => Logos?.Count ?? 0;
    }

    /// <summary>
    /// Representa un logo configurado dentro de un pedido.
    /// </summary>
    public class PedidoLogoViewModel
    {
        /// <summary>
        /// Identificador del logo asociado al pedido.
        /// </summary>
        public Guid IdLogoPedido { get; set; }
        /// <summary>
        /// Identificador de la técnica de personalización.
        /// </summary>
        public Guid? IdTecnica { get; set; }
        /// <summary>
        /// Descripción de la técnica utilizada.
        /// </summary>
        public string Tecnica { get; set; }
        /// <summary>
        /// Identificador de la ubicación del logo.
        /// </summary>
        public Guid? IdUbicacion { get; set; }
        /// <summary>
        /// Descripción de la ubicación.
        /// </summary>
        public string Ubicacion { get; set; }
        /// <summary>
        /// Identificador del proveedor responsable del logo.
        /// </summary>
        public Guid? IdProveedor { get; set; }
        /// <summary>
        /// Nombre del proveedor del logo.
        /// </summary>
        public string Proveedor { get; set; }
        /// <summary>
        /// Cantidad de logos solicitados.
        /// </summary>
        public int Cantidad { get; set; } = 1;
        /// <summary>
        /// Costo asociado a la personalización del logo.
        /// </summary>
        public decimal Costo { get; set; }
        /// <summary>
        /// Descripción adicional del logo.
        /// </summary>
        public string Descripcion { get; set; }
    }
}