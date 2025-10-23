using BLL.Helpers;
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
    public partial class ProductoForm : Form
    {
        private readonly IProductoService _productoService;
        private readonly IProveedorService _proveedorService;
        private readonly IBitacoraService _bitacora;
        private readonly bool _esEdicion;
        private Producto _model;

        private class Item { public Guid Id { get; set; } public string Nombre { get; set; } }

        public ProductoForm()
        {
            InitializeComponent();
        }

        public ProductoForm(IProductoService productoService, IProveedorService proveedorService, IBitacoraService bitacora, Producto producto)
        {
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacora = bitacora ?? throw new ArgumentNullException(nameof(bitacora));
            _model = producto;
            _esEdicion = producto != null;

            InitializeComponent();
            this.Load += ProductoForm_Load;
        }

        private void ProductoForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarCategorias();
            CargarProveedores();
            CargarModelo();
            WireUp();
        }

        private void ApplyTexts()
        {
            Text = _esEdicion ? "abm.common.edit".Traducir() + " - " + "abm.products.title".Traducir()
                              : "abm.common.new".Traducir() + " - " + "abm.products.title".Traducir();

            // traducir etiquetas y botones
            lblNombre.Text = "product.name".Traducir();
            lblCategoria.Text = "product.category".Traducir();
            lblProveedor.Text = "product.provider".Traducir();
            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
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

            if (cboCategoria.Items.Count > 0)
                cboCategoria.SelectedIndex = 0;
        }

        private void CargarProveedores()
        {
            // Filtrar solo proveedores de productos (no de personalización)
            var proveedores = _proveedorService.ObtenerProveedoresActivos()?.ToList() ?? new List<Proveedor>();

            // Filtrar solo proveedores que tienen el tipo "Productos"
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

            if (cboProveedor.Items.Count > 0)
                cboProveedor.SelectedIndex = 0;
        }

        private void CargarModelo()
        {
            if (_model == null)
            {
                _model = new Producto();
                return;
            }

            txtNombre.Text = _model.NombreProducto;

            // Seleccionar categoría
            if (_model.IdCategoria.HasValue)
            {
                for (int i = 0; i < cboCategoria.Items.Count; i++)
                {
                    if (((Item)cboCategoria.Items[i]).Id == _model.IdCategoria.Value)
                    {
                        cboCategoria.SelectedIndex = i;
                        break;
                    }
                }
            }

            // Seleccionar proveedor
            if (_model.IdProveedor.HasValue)
            {
                for (int i = 0; i < cboProveedor.Items.Count; i++)
                {
                    if (((Item)cboProveedor.Items[i]).Id == _model.IdProveedor.Value)
                    {
                        cboProveedor.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void WireUp()
        {
            btnGuardar.Click += (s, e) => Guardar();
            btnCancelar.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
        }

        private bool ValidarCampos()
        {
            errorProvider1.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "msg.required".Traducir());
                ok = false;
            }
            else if (txtNombre.Text.Trim().Length > 150)
            {
                errorProvider1.SetError(txtNombre, "product.name.maxlength".Traducir());
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

            return ok;
        }

        private void Guardar()
        {
            if (!ValidarCampos()) return;

            try
            {
                var categoriaSeleccionada = cboCategoria.SelectedItem as Item;
                var proveedorSeleccionado = cboProveedor.SelectedItem as Item;

                _model.NombreProducto = txtNombre.Text.Trim();
                _model.IdCategoria = categoriaSeleccionada?.Id;
                _model.IdProveedor = proveedorSeleccionado?.Id;

                ResultadoOperacion res;
                if (_esEdicion)
                {
                    res = _productoService.ActualizarProducto(_model);
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Producto.Editar",
                        res.EsValido ? $"Id={_model.IdProducto}, Nombre={_model.NombreProducto}" : res.Mensaje,
                        "Productos", res.EsValido);
                }
                else
                {
                    res = _productoService.CrearProducto(_model);
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Producto.Alta",
                        res.EsValido ? $"Id={res.IdGenerado}, Nombre={_model.NombreProducto}" : res.Mensaje,
                        "Productos", res.EsValido);
                }

                if (!res.EsValido)
                {
                    MessageBox.Show(res.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarAccion(SessionContext.IdUsuario, _esEdicion ? "Producto.Editar" : "Producto.Alta",
                    ex.Message, "Productos", false);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
