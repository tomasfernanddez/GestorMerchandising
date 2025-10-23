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
    public partial class ABMProductosForm : Form
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaProductoService _categoriaService;
        private readonly IProveedorService _proveedorService;
        private readonly IBitacoraService _bitacoraService;
        private readonly BindingSource _bindingSource = new BindingSource();
        private bool _cargandoFiltros;

        private sealed class ProductoGridRow
        {
            public Guid IdProducto { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public string Proveedor { get; set; }
            public DateTime? UltimoUso { get; set; }
            public int VecesUsado { get; set; }
            public bool Activo { get; set; }
        }

        private sealed class ComboItem
        {
            public Guid? Id { get; set; }
            public string Texto { get; set; }
            public override string ToString() => Texto;
        }

        public ABMProductosForm(
            IProductoService productoService,
            ICategoriaProductoService categoriaService,
            IProveedorService proveedorService,
            IBitacoraService bitacoraService)
        {
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _categoriaService = categoriaService ?? throw new ArgumentNullException(nameof(categoriaService));
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));

            InitializeComponent();
            Load += ABMProductosForm_Load;
        }

        private void ABMProductosForm_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ApplyTexts();
            WireUp();
            CargarFiltros();
            BuscarProductos();
        }

        private void ConfigurarGrid()
        {
            dgvProductos.AutoGenerateColumns = false;
            dgvProductos.Columns.Clear();
            dgvProductos.ReadOnly = true;
            dgvProductos.MultiSelect = false;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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
                HeaderText = "Producto",
                FillWeight = 200,
                MinimumWidth = 180
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
                FillWeight = 150,
                MinimumWidth = 130
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductoGridRow.UltimoUso),
                Name = "UltimoUso",
                HeaderText = "Último Uso",
                FillWeight = 110,
                MinimumWidth = 100,
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductoGridRow.VecesUsado),
                Name = "VecesUsado",
                HeaderText = "Usos",
                FillWeight = 60,
                MinimumWidth = 60
            });

            dgvProductos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(ProductoGridRow.Activo),
                Name = "Activo",
                HeaderText = "Activo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });

            dgvProductos.DataSource = _bindingSource;
        }

        private void ApplyTexts()
        {
            Text = "abm.products.title".Traducir();
            tsbNuevo.Text = "abm.common.new".Traducir();
            tsbEditar.Text = "abm.common.edit".Traducir();
            tsbActualizar.Text = "form.refresh".Traducir();
            tsbActivar.Text = "abm.common.activate".Traducir();
            tsbDesactivar.Text = "abm.common.deactivate".Traducir();
            tslBuscar.Text = "form.search".Traducir();
            tslCategoria.Text = "product.category".Traducir();
            tslProveedor.Text = "product.provider".Traducir();

            SetHeaderSafe("Nombre", "product.name");
            SetHeaderSafe("Categoria", "product.category");
            SetHeaderSafe("Proveedor", "product.provider");
            SetHeaderSafe("UltimoUso", "product.lastUsed");
            SetHeaderSafe("VecesUsado", "product.usageCount");
            SetHeaderSafe("Activo", "product.active");
        }

        private void SetHeaderSafe(string columnName, string resourceKey)
        {
            var col = dgvProductos.Columns[columnName];
            if (col != null)
                col.HeaderText = resourceKey.Traducir();
        }

        private void WireUp()
        {
            tsbNuevo.Click += (s, e) => NuevoProducto();
            tsbEditar.Click += (s, e) => EditarSeleccionado();
            tsbActivar.Click += (s, e) => CambiarEstadoSeleccionado(true);
            tsbDesactivar.Click += (s, e) => CambiarEstadoSeleccionado(false);
            tsbActualizar.Click += (s, e) => BuscarProductos();
            txtBuscar.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    BuscarProductos();
                }
            };
            txtBuscar.TextChanged += (s, e) => BuscarProductos();
            cboCategorias.SelectedIndexChanged += (s, e) => { if (!_cargandoFiltros) BuscarProductos(); };
            cboProveedores.SelectedIndexChanged += (s, e) => { if (!_cargandoFiltros) BuscarProductos(); };
            dgvProductos.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };
        }

        private void CargarFiltros()
        {
            _cargandoFiltros = true;
            try
            {
                var categorias = _categoriaService.ObtenerTodas()?.ToList() ?? new List<DomainModel.Entidades.CategoriaProducto>();
                var proveedores = _proveedorService.ObtenerProveedoresActivos()?.ToList() ?? new List<Proveedor>();

                var categoriaItems = new List<ComboItem>
                {
                    new ComboItem { Id = null, Texto = "form.filter.all".Traducir() }
                };

                categoriaItems.AddRange(categorias
                    .OrderBy(c => c.Orden)
                    .Select(c => new ComboItem
                    {
                        Id = c.IdCategoria,
                        Texto = c.NombreCategoria
                    }));

                cboCategorias.ComboBox.DisplayMember = nameof(ComboItem.Texto);
                cboCategorias.ComboBox.ValueMember = nameof(ComboItem.Id);
                cboCategorias.ComboBox.DataSource = categoriaItems;

                var proveedorItems = new List<ComboItem>
                {
                    new ComboItem { Id = null, Texto = "form.filter.all".Traducir() }
                };

                proveedorItems.AddRange(proveedores
                    .OrderBy(p => p.RazonSocial)
                    .Select(p => new ComboItem
                    {
                        Id = p.IdProveedor,
                        Texto = p.RazonSocial
                    }));

                cboProveedores.ComboBox.DisplayMember = nameof(ComboItem.Texto);
                cboProveedores.ComboBox.ValueMember = nameof(ComboItem.Id);
                cboProveedores.ComboBox.DataSource = proveedorItems;

                cboCategorias.SelectedIndex = 0;
                cboProveedores.SelectedIndex = 0;
            }
            finally
            {
                _cargandoFiltros = false;
            }
        }

        private void BuscarProductos()
        {
            if (_cargandoFiltros)
                return;

            IEnumerable<Producto> productos;
            var termino = txtBuscar.Text?.Trim();
            if (string.IsNullOrWhiteSpace(termino))
            {
                productos = _productoService.ObtenerTodos();
            }
            else
            {
                productos = _productoService.Buscar(termino);
            }

            var categoriaSeleccionada = (cboCategorias.SelectedItem as ComboItem)?.Id;
            var proveedorSeleccionado = (cboProveedores.SelectedItem as ComboItem)?.Id;

            if (categoriaSeleccionada.HasValue)
                productos = productos.Where(p => p.IdCategoria == categoriaSeleccionada);
            if (proveedorSeleccionado.HasValue)
                productos = productos.Where(p => p.IdProveedor == proveedorSeleccionado);

            var rows = productos
                .Select(p => new ProductoGridRow
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.NombreProducto,
                    Categoria = p.Categoria?.NombreCategoria ?? "-",
                    Proveedor = p.Proveedor?.RazonSocial ?? "-",
                    UltimoUso = p.FechaUltimoUso,
                    VecesUsado = p.VecesUsado,
                    Activo = p.Activo
                })
                .OrderByDescending(r => r.UltimoUso.HasValue)
                .ThenByDescending(r => r.UltimoUso)
                .ThenBy(r => r.Nombre)
                .ToList();

            _bindingSource.DataSource = rows;
        }

        private ProductoGridRow ObtenerSeleccionado()
        {
            if (_bindingSource.Current is ProductoGridRow row)
                return row;

            if (dgvProductos.CurrentRow?.DataBoundItem is ProductoGridRow currentRow)
                return currentRow;

            return null;
        }

        private void NuevoProducto()
        {
            using (var form = new ProductoForm(_productoService, _categoriaService, _proveedorService, _bitacoraService))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    BuscarProductos();
                }
            }
        }

        private void EditarSeleccionado()
        {
            var seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
                return;

            var producto = _productoService.ObtenerPorId(seleccionado.IdProducto);
            if (producto == null)
                return;

            using (var form = new ProductoForm(_productoService, _categoriaService, _proveedorService, _bitacoraService, producto))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    BuscarProductos();
                }
            }
        }

        private void CambiarEstadoSeleccionado(bool activo)
        {
            var seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
                return;

            if (seleccionado.Activo == activo)
                return;

            var confirmKey = activo ? "product.confirm.activate" : "product.confirm.deactivate";
            var confirmText = confirmKey.Traducir();
            if (MessageBox.Show(this, confirmText, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var resultado = _productoService.CambiarEstado(seleccionado.IdProducto, activo);
            _bitacoraService.RegistrarAccion(
                SessionContext.IdUsuario,
                activo ? "Producto.Activar" : "Producto.Desactivar",
                resultado.Mensaje,
                "Productos",
                resultado.EsValido);

            if (!resultado.EsValido)
            {
                MessageBox.Show(this, resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BuscarProductos();
        }
    }
}