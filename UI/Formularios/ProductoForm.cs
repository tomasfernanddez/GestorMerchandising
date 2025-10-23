using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL.Helpers;
using BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using Services.BLL.Interfaces;
using UI.Localization;

namespace UI
{
    public partial class ProductoForm : Form
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaProductoService _categoriaService;
        private readonly IProveedorService _proveedorService;
        private readonly IBitacoraService _bitacoraService;
        private readonly bool _esEdicion;
        private Producto _producto;
        private IList<CategoriaProducto> _categorias = new List<CategoriaProducto>();
        private IList<Proveedor> _proveedores = new List<Proveedor>();

        private sealed class ComboItem
        {
            public Guid Id { get; set; }
            public string Nombre { get; set; }
            public override string ToString() => Nombre;
        }

        public ProductoForm(
            IProductoService productoService,
            ICategoriaProductoService categoriaService,
            IProveedorService proveedorService,
            IBitacoraService bitacoraService,
            Producto producto = null)
        {
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _categoriaService = categoriaService ?? throw new ArgumentNullException(nameof(categoriaService));
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _producto = producto;
            _esEdicion = producto != null;

            InitializeComponent();
            Load += ProductoForm_Load;
        }

        private void ProductoForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarCombos();
            CargarModelo();
            WireUp();
        }

        private void ApplyTexts()
        {
            Text = _esEdicion ? "abm.common.edit".Traducir() + " - " + "product.singular".Traducir() : "abm.common.new".Traducir() + " - " + "product.singular".Traducir();
            lblNombre.Text = "product.name".Traducir();
            lblCategoria.Text = "product.category".Traducir();
            lblProveedor.Text = "product.provider".Traducir();
            chkActivo.Text = "product.active".Traducir();
            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
        }

        private void WireUp()
        {
            btnGuardar.Click += (s, e) => Guardar();
            btnCancelar.Click += (s, e) => DialogResult = DialogResult.Cancel;
        }

        private void CargarCombos()
        {
            _categorias = _categoriaService.ObtenerTodas()?.OrderBy(c => c.Orden).ToList() ?? new List<CategoriaProducto>();
            _proveedores = _proveedorService.ObtenerProveedoresActivos()?.OrderBy(p => p.RazonSocial).ToList() ?? new List<Proveedor>();

            cboCategoria.DisplayMember = nameof(ComboItem.Nombre);
            cboCategoria.ValueMember = nameof(ComboItem.Id);
            cboCategoria.DataSource = _categorias
                .Select(c => new ComboItem { Id = c.IdCategoria, Nombre = c.NombreCategoria })
                .ToList();

            cboProveedor.DisplayMember = nameof(ComboItem.Nombre);
            cboProveedor.ValueMember = nameof(ComboItem.Id);
            cboProveedor.DataSource = _proveedores
                .Select(p => new ComboItem { Id = p.IdProveedor, Nombre = p.RazonSocial })
                .ToList();
        }

        private void CargarModelo()
        {
            if (_esEdicion && _producto != null)
            {
                txtNombre.Text = _producto.NombreProducto;
                if (_producto.IdCategoria.HasValue)
                {
                    SeleccionarEnCombo(cboCategoria, _producto.IdCategoria.Value);
                }
                if (_producto.IdProveedor.HasValue)
                {
                    SeleccionarEnCombo(cboProveedor, _producto.IdProveedor.Value);
                }
                chkActivo.Checked = _producto.Activo;
            }
            else
            {
                chkActivo.Checked = true;
            }
        }

        private void SeleccionarEnCombo(ComboBox combo, Guid id)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (combo.Items[i] is ComboItem item && item.Id == id)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        private void Guardar()
        {
            var nombre = txtNombre.Text?.Trim();
            var categoriaId = (cboCategoria.SelectedItem as ComboItem)?.Id ?? Guid.Empty;
            var proveedorId = (cboProveedor.SelectedItem as ComboItem)?.Id ?? Guid.Empty;
            var activo = chkActivo.Checked;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show(this, "product.validation.name".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (categoriaId == Guid.Empty)
            {
                MessageBox.Show(this, "product.validation.category".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCategoria.Focus();
                return;
            }

            if (proveedorId == Guid.Empty)
            {
                MessageBox.Show(this, "product.validation.provider".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboProveedor.Focus();
                return;
            }

            ResultadoOperacion resultado;
            if (_esEdicion)
            {
                _producto.NombreProducto = nombre;
                _producto.IdCategoria = categoriaId;
                _producto.IdProveedor = proveedorId;
                _producto.Activo = activo;

                resultado = _productoService.ActualizarProducto(_producto);
                RegistrarBitacora(resultado, "Producto.Editar", nombre);
            }
            else
            {
                var nuevo = new Producto
                {
                    NombreProducto = nombre,
                    IdCategoria = categoriaId,
                    IdProveedor = proveedorId,
                    Activo = activo
                };

                resultado = _productoService.CrearProductoManual(nuevo);
                RegistrarBitacora(resultado, "Producto.Alta", nombre);
            }

            if (!resultado.EsValido)
            {
                MessageBox.Show(this, resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void RegistrarBitacora(ResultadoOperacion resultado, string accion, string nombreProducto)
        {
            var descripcion = resultado.EsValido
                ? $"{resultado.Mensaje} ({nombreProducto})"
                : resultado.Mensaje;

            _bitacoraService.RegistrarAccion(
                SessionContext.IdUsuario,
                accion,
                descripcion,
                "Productos",
                resultado.EsValido,
                resultado.EsValido ? null : resultado.Mensaje);
        }
    }
}