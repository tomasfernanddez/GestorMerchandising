using System;
using System.Collections.Generic;

namespace UI.ViewModels
{
    public class PedidoDetalleViewModel
    {
        public Guid IdDetallePedido { get; set; }
        public Guid? IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public Guid? IdCategoria { get; set; }
        public string Categoria { get; set; }
        public Guid? IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public Guid? IdEstadoProducto { get; set; }
        public string EstadoProducto { get; set; }
        public DateTime? FechaLimite { get; set; }
        public bool FichaAplicacion { get; set; }
        public string Notas { get; set; }
        public Guid? IdProveedorPersonalizacion { get; set; }
        public string ProveedorPersonalizacion { get; set; }
        public List<PedidoLogoViewModel> Logos { get; set; } = new List<PedidoLogoViewModel>();
    }

    public class PedidoLogoViewModel
    {
        public Guid IdLogoPedido { get; set; }
        public Guid? IdTecnica { get; set; }
        public string Tecnica { get; set; }
        public Guid? IdUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public Guid? IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public int Cantidad { get; set; } = 1;
        public decimal Costo { get; set; }
        public string Descripcion { get; set; }
    }
}