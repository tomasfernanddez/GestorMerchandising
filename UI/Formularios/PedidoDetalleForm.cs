using BLL.Interfaces;
using Services.BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UI.Localization;

namespace UI
{
    public partial class PedidoDetalleForm : Form
    {
        private readonly IProductoService _productoService;
        private readonly IProveedorService _proveedorService;
        private readonly IPedidoService _pedidoService;
        private readonly bool _esEdicion;
        private PedidoDetalle _detalle;
        private List<LogosPedido> _logos;

        private class Item { public Guid Id { get; set; } public string Nombre { get; set; } }

        public PedidoDetalle Detalle => _detalle;

        public PedidoDetalleForm()
        {
            InitializeComponent();
        }

        public PedidoDetalleForm(IProductoService productoService, IProveedorService proveedorService,
            IPedidoService pedidoService, PedidoDetalle detalle)
        {
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
            _detalle = detalle;
            _esEdicion = detalle != null;
            _logos = new List<LogosPedido>();

            InitializeComponent();
            this.Load += PedidoDetalleForm_Load;
        }

        private void PedidoDetalleForm_Load(object sender, EventArgs e)
        {
            ConfigurarAutocompletado();
            ApplyTexts();
            CargarCategorias();
            CargarProveedores();
            CargarEstadosProducto();
            CargarModelo();
            WireUp();
        }

        private void ApplyTexts()
        {
            Text = _esEdicion ? "Editar Producto en Pedido" : "Agregar Producto al Pedido";

            lblProducto.Text = "order.detail.product".Traducir();
            lblCategoria.Text = "product.category".Traducir();
            lblProveedor.Text = "product.provider".Traducir();
            lblCantidad.Text = "order.detail.quantity".Traducir();
            lblPrecio.Text = "order.detail.price".Traducir();
            lblEstado.Text = "order.detail.status".Traducir();
            lblFechaLimite.Text = "order.detail.deadline".Traducir();

            grpLogos.Text = "Logos / Personalización";
            btnAgregarLogo.Text = "Agregar Logo";
            btnEditarLogo.Text = "Editar Logo";
            btnEliminarLogo.Text = "Eliminar Logo";

            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
        }

        private void ConfigurarAutocompletado()
        {
            // Configurar autocompletado para el nombre del producto
            cboProducto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProducto.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var productos = _productoService.ObtenerTodosLosProductos()?.ToList() ?? new List<Producto>();
            var nombres = new AutoCompleteStringCollection();
            nombres.AddRange(productos.Select(p => p.NombreProducto).ToArray());
            cboProducto.AutoCompleteCustomSource = nombres;
        }

        private void CargarCategorias()
        {
            var categorias = _productoService.ObtenerCategorias()?.ToList() ?? new List<CategoriaProducto>();
            cboCategoria.DisplayMember = "Nombre";
            cboCategoria.ValueMember = "Id";
            cboCategoria.DataSource = categorias.Select(c => new Item
            {
                Id = c.IdCategoria,
                Nombre = c.NombreCategoria
            }).ToList();

            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarProveedores()
        {
            var proveedores = _proveedorService.ObtenerProveedoresActivos()?.ToList() ?? new List<Proveedor>();
            var proveedoresProductos = proveedores.Where(p =>
                p.TiposProveedor != null &&
                p.TiposProveedor.Any(tp => tp.NombreTipoProveedor.Contains("Producto", StringComparison.OrdinalIgnoreCase))
            ).ToList();

            cboProveedor.DisplayMember = "Nombre";
            cboProveedor.ValueMember = "Id";
            cboProveedor.DataSource = proveedoresProductos.Select(p => new Item
            {
                Id = p.IdProveedor,
                Nombre = p.RazonSocial
            }).ToList();

            cboProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarEstadosProducto()
        {
            var estados = _pedidoService.ObtenerEstadosProducto()?.ToList() ?? new List<EstadoProducto>();
            cboEstado.DisplayMember = "Nombre";
            cboEstado.ValueMember = "Id";
            cboEstado.DataSource = estados.Select(e => new Item
            {
                Id = e.IdEstadoProducto,
                Nombre = e.NombreEstadoProducto
            }).ToList();

            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarModelo()
        {
            if (_detalle == null)
            {
                _detalle = new PedidoDetalle
                {
                    IdDetallePedido = Guid.NewGuid(),
                    Cantidad = 1,
                    PrecioUnitario = 0
                };
                nudCantidad.Value = 1;
                nudPrecio.Value = 0;
                chkTieneFechaLimite.Checked = false;
                dtpFechaLimite.Enabled = false;
                return;
            }

            // Cargar detalle existente
            if (_detalle.Producto != null)
            {
                cboProducto.Text = _detalle.Producto.NombreProducto;

                if (_detalle.Producto.IdCategoria.HasValue)
                    SeleccionarCombo(cboCategoria, _detalle.Producto.IdCategoria.Value);

                if (_detalle.Producto.IdProveedor.HasValue)
                    SeleccionarCombo(cboProveedor, _detalle.Producto.IdProveedor.Value);
            }

            nudCantidad.Value = _detalle.Cantidad;
            nudPrecio.Value = _detalle.PrecioUnitario;

            if (_detalle.IdEstadoProducto.HasValue)
                SeleccionarCombo(cboEstado, _detalle.IdEstadoProducto.Value);

            if (_detalle.FechaLimiteProducto.HasValue)
            {
                chkTieneFechaLimite.Checked = true;
                dtpFechaLimite.Value = _detalle.FechaLimiteProducto.Value;
                dtpFechaLimite.Enabled = true;
            }

            // Cargar logos
            if (_detalle.LogosPedido != null && _detalle.LogosPedido.Any())
            {
                _logos = _detalle.LogosPedido.ToList();
                ActualizarGridLogos();
            }
        }

        private void SeleccionarCombo(ComboBox combo, Guid id)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (((Item)combo.Items[i]).Id == id)
                {
                    combo.SelectedIndex = i;
                    break;
                }
            }
        }

        private void WireUp()
        {
            chkTieneFechaLimite.CheckedChanged += (s, e) => dtpFechaLimite.Enabled = chkTieneFechaLimite.Checked;

            btnAgregarLogo.Click += (s, e) => AgregarLogo();
            btnEditarLogo.Click += (s, e) => EditarLogo();
            btnEliminarLogo.Click += (s, e) => EliminarLogo();

            btnGuardar.Click += (s, e) => Guardar();
            btnCancelar.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
        }

        private void AgregarLogo()
        {
            var form = new LogoPedidoForm(_proveedorService, logo: null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                var nuevoLogo = form.Logo;
                if (nuevoLogo != null)
                {
                    _logos.Add(nuevoLogo);
                    ActualizarGridLogos();
                }
            }
        }

        private void EditarLogo()
        {
            if (dgvLogos.CurrentRow == null) return;
            var index = dgvLogos.CurrentRow.Index;
            if (index < 0 || index >= _logos.Count) return;

            var logo = _logos[index];
            var form = new LogoPedidoForm(_proveedorService, logo);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _logos[index] = form.Logo;
                ActualizarGridLogos();
            }
        }

        private void EliminarLogo()
        {
            if (dgvLogos.CurrentRow == null) return;
            var index = dgvLogos.CurrentRow.Index;
            if (index < 0 || index >= _logos.Count) return;

            var resultado = MessageBox.Show("¿Confirma eliminar este logo?", Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                _logos.RemoveAt(index);
                ActualizarGridLogos();
            }
        }

        private void ActualizarGridLogos()
        {
            var rows = _logos.Select(l => new
            {
                Tecnica = l.TecnicaPersonalizacion?.NombreTecnicaPersonalizacion ?? "-",
                Ubicacion = l.UbicacionLogo?.NombreUbicacionLogo ?? "-",
                Proveedor = l.ProveedorPersonalizacion?.RazonSocial ?? "-",
                Costo = l.CostoPersonalizacion,
                Descripcion = l.Descripcion ?? ""
            }).ToList();

            dgvLogos.DataSource = rows;
        }

        private bool ValidarCampos()
        {
            errorProvider1.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(cboProducto.Text))
            {
                errorProvider1.SetError(cboProducto, "msg.required".Traducir());
                ok = false;
            }

            if (cboCategoria.SelectedItem == null)
            {
                errorProvider1.SetError(cboCategoria, "msg.required".Traducir());
                ok = false;
            }

            if (cboProveedor.SelectedItem == null)
            {
                errorProvider1.SetError(cboProveedor, "msg.required".Traducir());
                ok = false;
            }

            if (nudCantidad.Value <= 0)
            {
                errorProvider1.SetError(nudCantidad, "La cantidad debe ser mayor a cero");
                ok = false;
            }

            if (nudPrecio.Value < 0)
            {
                errorProvider1.SetError(nudPrecio, "El precio no puede ser negativo");
                ok = false;
            }

            return ok;
        }

        private void Guardar()
        {
            if (!ValidarCampos()) return;

            try
            {
                var categoriaSeleccionada = cboCategoria.SelectedItem as Item;
                var proveedorSeleccionado = cboProveedor.SelectedItem as Item;
                var estadoSeleccionado = cboEstado.SelectedItem as Item;

                // Crear o obtener el producto automáticamente
                var resultadoProducto = _productoService.CrearOObtenerProducto(
                    cboProducto.Text.Trim(),
                    categoriaSeleccionada.Id,
                    proveedorSeleccionado.Id
                );

                if (!resultadoProducto.EsValido)
                {
                    MessageBox.Show(resultadoProducto.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Actualizar detalle
                _detalle.IdProducto = resultadoProducto.IdGenerado.Value;
                _detalle.Cantidad = (int)nudCantidad.Value;
                _detalle.PrecioUnitario = nudPrecio.Value;
                _detalle.IdEstadoProducto = estadoSeleccionado?.Id;
                _detalle.FechaLimiteProducto = chkTieneFechaLimite.Checked ? (DateTime?)dtpFechaLimite.Value : null;
                _detalle.LogosPedido = _logos;

                // Cargar el producto para mostrar en el grid
                _detalle.Producto = _productoService.ObtenerProductoPorId(_detalle.IdProducto);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
