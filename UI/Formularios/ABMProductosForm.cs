using BLL.Factories;
using BLL.Interfaces;
using Services.BLL.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UI.Localization;

namespace UI
{
    public partial class ABMProductosForm : Form
    {
        private readonly IProductoService _productoService;
        private readonly IProveedorService _proveedorService;
        private readonly IBitacoraService _bitacora;

        public ABMProductosForm()
        {
            InitializeComponent();
        }

        public ABMProductosForm(IProductoService productoService, IProveedorService proveedorService, IBitacoraService bitacora)
        {
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacora = bitacora ?? throw new ArgumentNullException(nameof(bitacora));

            InitializeComponent();
            this.Load += ABMProductosForm_Load;
        }

        private sealed class ProductoGridRow
        {
            public Guid IdProducto { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public string Proveedor { get; set; }
        }

        private void ABMProductosForm_Load(object sender, EventArgs e)
        {
            EnsureColumns();
            ApplyTexts();
            WireUp();
            CargarProductos();
        }

        private void EnsureColumns()
        {
            if (dgvProductos.Columns.Count > 0) return;

            dgvProductos.AutoGenerateColumns = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.AllowUserToResizeColumns = true;

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductoGridRow.IdProducto),
                Name = "IdProducto",
                Visible = false
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductoGridRow.Nombre),
                Name = "Nombre",
                HeaderText = "Nombre",
                FillWeight = 200,
                MinimumWidth = 160
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductoGridRow.Categoria),
                Name = "Categoria",
                HeaderText = "Categoría",
                FillWeight = 120,
                MinimumWidth = 110
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductoGridRow.Proveedor),
                Name = "Proveedor",
                HeaderText = "Proveedor",
                FillWeight = 180,
                MinimumWidth = 150
            });
        }

        private void ApplyTexts()
        {
            Text = "abm.products.title".Traducir();
            SetHeaderSafe("Nombre", "product.name");
            SetHeaderSafe("Categoria", "product.category");
            SetHeaderSafe("Proveedor", "product.provider");
        }

        private void SetHeaderSafe(string columnName, string i18nKey)
        {
            var col = dgvProductos?.Columns?[columnName];
            if (col != null)
                col.HeaderText = i18nKey.Traducir();
        }

        private void WireUp()
        {
            tsbActualizar.Click += (s, e) => CargarProductos();
            tsbBuscar.Click += (s, e) => Buscar();
            txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Buscar(); } };

            tsbNuevo.Click += (s, e) => NuevoProducto();
            tsbEditar.Click += (s, e) => EditarSeleccionado();
            dgvProductos.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };
        }

        private void CargarProductos()
        {
            try
            {
                var productos = _productoService.ObtenerTodosLosProductos()?.ToList() ?? new List<Producto>();
                var rows = productos.Select(p => new ProductoGridRow
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.NombreProducto,
                    Categoria = p.Categoria?.NombreCategoria ?? "-",
                    Proveedor = p.Proveedor?.RazonSocial ?? "-"
                }).ToList();

                dgvProductos.DataSource = rows;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Buscar()
        {
            try
            {
                var texto = txtBuscar.Text?.Trim();
                if (string.IsNullOrWhiteSpace(texto))
                {
                    CargarProductos();
                    return;
                }

                var productos = _productoService.BuscarPorNombre(texto)?.ToList() ?? new List<Producto>();
                var rows = productos.Select(p => new ProductoGridRow
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.NombreProducto,
                    Categoria = p.Categoria?.NombreCategoria ?? "-",
                    Proveedor = p.Proveedor?.RazonSocial ?? "-"
                }).ToList();

                dgvProductos.DataSource = rows;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NuevoProducto()
        {
            var form = new ProductoForm(_productoService, _proveedorService, _bitacora, producto: null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                CargarProductos();
            }
        }

        private void EditarSeleccionado()
        {
            if (dgvProductos.CurrentRow == null) return;
            var row = dgvProductos.CurrentRow.DataBoundItem as ProductoGridRow;
            if (row == null) return;

            try
            {
                var producto = _productoService.ObtenerProductoPorId(row.IdProducto);
                if (producto == null)
                {
                    MessageBox.Show("Producto no encontrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var form = new ProductoForm(_productoService, _proveedorService, _bitacora, producto);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    CargarProductos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
