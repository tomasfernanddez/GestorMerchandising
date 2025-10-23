using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using UI.Localization;
using UI.ViewModels;

namespace UI
{
    public partial class PedidoDetalleForm : Form
    {
        private readonly IProductoService _productoService;
        private readonly List<CategoriaProducto> _categorias;
        private readonly List<Proveedor> _proveedoresProductos;
        private readonly List<EstadoProducto> _estadosProducto;
        private readonly List<TecnicaPersonalizacion> _tecnicas;
        private readonly List<UbicacionLogo> _ubicaciones;
        private readonly List<Proveedor> _proveedoresPersonalizacion;
        private readonly PedidoDetalleViewModel _detalleOriginal;

        private BindingList<PedidoLogoViewModel> _logos;
        private Producto _productoSeleccionado;
        private readonly string _textoProveedorCorto;

        public PedidoDetalleViewModel DetalleResult { get; private set; }

        private sealed class ProductoSuggestion
        {
            public ProductoSuggestion(Producto producto, string descripcion)
            {
                Producto = producto;
                Descripcion = descripcion;
            }

            public Producto Producto { get; }
            public string Descripcion { get; }
            public override string ToString() => Descripcion;
        }

        public PedidoDetalleForm(
            IProductoService productoService,
            IEnumerable<CategoriaProducto> categorias,
            IEnumerable<Proveedor> proveedoresProductos,
            IEnumerable<EstadoProducto> estadosProducto,
            IEnumerable<TecnicaPersonalizacion> tecnicas,
            IEnumerable<UbicacionLogo> ubicaciones,
            IEnumerable<Proveedor> proveedoresPersonalizacion,
            PedidoDetalleViewModel detalle = null)
        {
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _categorias = categorias?.OrderBy(c => c.NombreCategoria).ToList() ?? new List<CategoriaProducto>();
            _proveedoresProductos = proveedoresProductos?.OrderBy(p => p.RazonSocial).ToList() ?? new List<Proveedor>();
            _estadosProducto = estadosProducto?.OrderBy(e => e.NombreEstadoProducto).ToList() ?? new List<EstadoProducto>();
            _tecnicas = tecnicas?.OrderBy(t => t.NombreTecnicaPersonalizacion).ToList() ?? new List<TecnicaPersonalizacion>();
            _ubicaciones = ubicaciones?.OrderBy(u => u.NombreUbicacionLogo).ToList() ?? new List<UbicacionLogo>();
            _proveedoresPersonalizacion = proveedoresPersonalizacion?.OrderBy(p => p.RazonSocial).ToList() ?? new List<Proveedor>();
            _detalleOriginal = detalle;
            _textoProveedorCorto = "order.detail.provider.short".Traducir();

            InitializeComponent();
        }

        private void PedidoDetalleForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            ConfigurarCombos();
            ConfigurarGrillaLogos();
            CargarDetalle();
        }

        private void ApplyTexts()
        {
            Text = _detalleOriginal == null ? "order.detail.addTitle".Traducir() : "order.detail.editTitle".Traducir();
            lblProducto.Text = "order.detail.product".Traducir();
            lblCategoria.Text = "order.detail.category".Traducir();
            lblProveedor.Text = "order.detail.provider".Traducir();
            lblCantidad.Text = "order.detail.quantity".Traducir();
            lblPrecio.Text = "order.detail.price".Traducir();
            lblEstado.Text = "order.detail.state".Traducir();
            lblFechaLimite.Text = "order.detail.deadline".Traducir();
            chkFechaLimite.Text = "order.detail.deadline.enable".Traducir();
            lblFicha.Text = "order.detail.applicationSheet".Traducir();
            lblProveedorPersonalizacion.Text = "order.detail.provider.personalization".Traducir();
            lblNotas.Text = "order.detail.notes".Traducir();
            gbLogos.Text = "order.detail.logos".Traducir();
            btnAgregarLogo.Text = "order.detail.logo.add".Traducir();
            btnEditarLogo.Text = "order.detail.logo.edit".Traducir();
            btnEliminarLogo.Text = "order.detail.logo.delete".Traducir();
            btnAceptar.Text = "form.accept".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
        }

        private void ConfigurarCombos()
        {
            cmbProducto.AutoCompleteMode = AutoCompleteMode.None;
            cmbProducto.AutoCompleteSource = AutoCompleteSource.None;

            cmbCategoria.DisplayMember = nameof(CategoriaProducto.NombreCategoria);
            cmbCategoria.ValueMember = nameof(CategoriaProducto.IdCategoria);
            cmbCategoria.DataSource = _categorias;

            cmbProveedor.DisplayMember = nameof(Proveedor.RazonSocial);
            cmbProveedor.ValueMember = nameof(Proveedor.IdProveedor);
            cmbProveedor.DataSource = _proveedoresProductos;

            cmbEstado.DisplayMember = nameof(EstadoProducto.NombreEstadoProducto);
            cmbEstado.ValueMember = nameof(EstadoProducto.IdEstadoProducto);
            cmbEstado.DataSource = _estadosProducto;

            var proveedoresPersonalizacion = new List<Proveedor>
            {
                new Proveedor { IdProveedor = Guid.Empty, RazonSocial = "form.select.optional".Traducir() }
            };
            proveedoresPersonalizacion.AddRange(_proveedoresPersonalizacion);
            cmbProveedorPersonalizacion.DisplayMember = nameof(Proveedor.RazonSocial);
            cmbProveedorPersonalizacion.ValueMember = nameof(Proveedor.IdProveedor);
            cmbProveedorPersonalizacion.DataSource = proveedoresPersonalizacion;
        }

        private void ConfigurarGrillaLogos()
        {
            _logos = new BindingList<PedidoLogoViewModel>(_detalleOriginal?.Logos?.Select(CloneLogo).ToList() ?? new List<PedidoLogoViewModel>());
            dgvLogos.AutoGenerateColumns = false;
            dgvLogos.Columns.Clear();
            dgvLogos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoLogoViewModel.Tecnica),
                HeaderText = "order.detail.logo.technique".Traducir(),
                FillWeight = 120,
                MinimumWidth = 100
            });
            dgvLogos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoLogoViewModel.Ubicacion),
                HeaderText = "order.detail.logo.location".Traducir(),
                FillWeight = 120,
                MinimumWidth = 100
            });
            dgvLogos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoLogoViewModel.Proveedor),
                HeaderText = "order.detail.logo.provider".Traducir(),
                FillWeight = 140,
                MinimumWidth = 120
            });
            dgvLogos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoLogoViewModel.Cantidad),
                HeaderText = "order.detail.logo.quantity".Traducir(),
                FillWeight = 60,
                MinimumWidth = 60
            });
            dgvLogos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoLogoViewModel.Costo),
                HeaderText = "order.detail.logo.cost".Traducir(),
                FillWeight = 80,
                MinimumWidth = 70,
                DefaultCellStyle = { Format = "C2" }
            });
            dgvLogos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PedidoLogoViewModel.Descripcion),
                HeaderText = "order.detail.logo.notes".Traducir(),
                FillWeight = 160,
                MinimumWidth = 140
            });
            dgvLogos.DataSource = _logos;
        }

        private void CargarDetalle()
        {
            if (_detalleOriginal == null)
            {
                return;
            }

            cmbProducto.Text = _detalleOriginal.NombreProducto;
            if (_detalleOriginal.IdProducto.HasValue)
            {
                _productoSeleccionado = _productoService.ObtenerPorId(_detalleOriginal.IdProducto.Value);
            }

            if (_detalleOriginal.IdCategoria.HasValue)
            {
                cmbCategoria.SelectedValue = _detalleOriginal.IdCategoria.Value;
            }

            if (_detalleOriginal.IdProveedor.HasValue)
            {
                cmbProveedor.SelectedValue = _detalleOriginal.IdProveedor.Value;
            }

            nudCantidad.Value = Math.Max(1, _detalleOriginal.Cantidad);
            nudPrecio.Value = _detalleOriginal.PrecioUnitario >= 0 ? _detalleOriginal.PrecioUnitario : 0;

            if (_detalleOriginal.IdEstadoProducto.HasValue)
            {
                cmbEstado.SelectedValue = _detalleOriginal.IdEstadoProducto.Value;
            }

            if (_detalleOriginal.FechaLimite.HasValue)
            {
                chkFechaLimite.Checked = true;
                dtpFechaLimite.Value = _detalleOriginal.FechaLimite.Value;
            }

            chkFicha.Checked = _detalleOriginal.FichaAplicacion;

            if (_detalleOriginal.IdProveedorPersonalizacion.HasValue)
            {
                cmbProveedorPersonalizacion.SelectedValue = _detalleOriginal.IdProveedorPersonalizacion.Value;
            }

            txtNotas.Text = _detalleOriginal.Notas;
        }

        private PedidoLogoViewModel CloneLogo(PedidoLogoViewModel logo)
        {
            return new PedidoLogoViewModel
            {
                IdLogoPedido = logo.IdLogoPedido,
                IdTecnica = logo.IdTecnica,
                Tecnica = logo.Tecnica,
                IdUbicacion = logo.IdUbicacion,
                Ubicacion = logo.Ubicacion,
                IdProveedor = logo.IdProveedor,
                Proveedor = logo.Proveedor,
                Cantidad = logo.Cantidad,
                Costo = logo.Costo,
                Descripcion = logo.Descripcion
            };
        }

        private void cmbProducto_TextUpdate(object sender, EventArgs e)
        {
            var texto = cmbProducto.Text?.Trim() ?? string.Empty;
            if (texto.Length < 2)
            {
                _productoSeleccionado = null;
                return;
            }

            try
            {
                _productoSeleccionado = null;
                var sugerencias = _productoService.BuscarParaAutocomplete(texto, 12)
                    .Where(p => p != null)
                    .ToList();

                cmbProducto.BeginUpdate();
                var caret = cmbProducto.SelectionStart;
                cmbProducto.Items.Clear();
                foreach (var producto in sugerencias)
                {
                    var descripcion = FormatearDescripcionProducto(producto);
                    cmbProducto.Items.Add(new ProductoSuggestion(producto, descripcion));
                }
                cmbProducto.EndUpdate();

                cmbProducto.DroppedDown = sugerencias.Any();
                cmbProducto.Text = texto;
                cmbProducto.SelectionStart = caret;
                cmbProducto.SelectionLength = texto.Length - caret;
            }
            catch
            {
                // Ignorar errores de autocompletado para no afectar la carga del formulario
            }
        }

        private string FormatearDescripcionProducto(Producto producto)
        {
            var categoria = producto.Categoria?.NombreCategoriaProducto;
            var proveedor = producto.Proveedor?.RazonSocial;
            if (string.IsNullOrWhiteSpace(categoria) && string.IsNullOrWhiteSpace(proveedor))
            {
                return producto.NombreProducto;
            }

            return $"{producto.NombreProducto} ({categoria}) - {_textoProveedorCorto}: {proveedor}";
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem is ProductoSuggestion sugerencia)
            {
                _productoSeleccionado = sugerencia.Producto;
                cmbProducto.Text = sugerencia.Producto.NombreProducto;
                if (_productoSeleccionado.IdCategoria.HasValue)
                {
                    cmbCategoria.SelectedValue = _productoSeleccionado.IdCategoria.Value;
                }
                if (_productoSeleccionado.IdProveedor.HasValue)
                {
                    cmbProveedor.SelectedValue = _productoSeleccionado.IdProveedor.Value;
                }
            }
        }

        private void chkFechaLimite_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaLimite.Enabled = chkFechaLimite.Checked;
        }

        private void btnAgregarLogo_Click(object sender, EventArgs e)
        {
            var form = new PedidoLogoForm(_tecnicas, _ubicaciones, _proveedoresPersonalizacion);
            if (form.ShowDialog(this) == DialogResult.OK && form.LogoResult != null)
            {
                _logos.Add(form.LogoResult);
            }
        }

        private void btnEditarLogo_Click(object sender, EventArgs e)
        {
            var logo = ObtenerLogoSeleccionado();
            if (logo == null)
                return;

            var form = new PedidoLogoForm(_tecnicas, _ubicaciones, _proveedoresPersonalizacion, CloneLogo(logo));
            if (form.ShowDialog(this) == DialogResult.OK && form.LogoResult != null)
            {
                var index = _logos.IndexOf(logo);
                _logos[index] = form.LogoResult;
            }
        }

        private void btnEliminarLogo_Click(object sender, EventArgs e)
        {
            var logo = ObtenerLogoSeleccionado();
            if (logo == null)
                return;

            if (MessageBox.Show("order.detail.logo.delete.confirm".Traducir(), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _logos.Remove(logo);
            }
        }

        private PedidoLogoViewModel ObtenerLogoSeleccionado()
        {
            if (dgvLogos.CurrentRow?.DataBoundItem is PedidoLogoViewModel logo)
            {
                return logo;
            }
            return null;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var nombreProducto = cmbProducto.Text?.Trim();
            if (string.IsNullOrWhiteSpace(nombreProducto))
            {
                MessageBox.Show("order.detail.validation.product".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProducto.Focus();
                return;
            }

            if (!(cmbCategoria.SelectedValue is Guid categoriaId) || categoriaId == Guid.Empty)
            {
                MessageBox.Show("order.detail.validation.category".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategoria.Focus();
                return;
            }

            if (!(cmbProveedor.SelectedValue is Guid proveedorId) || proveedorId == Guid.Empty)
            {
                MessageBox.Show("order.detail.validation.provider".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProveedor.Focus();
                return;
            }

            if (nudCantidad.Value <= 0)
            {
                MessageBox.Show("order.detail.validation.quantity".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudCantidad.Focus();
                return;
            }

            if (nudPrecio.Value < 0)
            {
                MessageBox.Show("order.detail.validation.price".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudPrecio.Focus();
                return;
            }

            Guid? idEstado = null;
            if (cmbEstado.SelectedValue is Guid estadoId && estadoId != Guid.Empty)
            {
                idEstado = estadoId;
            }

            Guid? idProveedorPersonalizacion = null;
            if (cmbProveedorPersonalizacion.SelectedValue is Guid provPersId && provPersId != Guid.Empty)
            {
                idProveedorPersonalizacion = provPersId;
            }

            DateTime? fechaLimite = chkFechaLimite.Checked ? dtpFechaLimite.Value.Date : (DateTime?)null;

            DetalleResult = new PedidoDetalleViewModel
            {
                IdDetallePedido = _detalleOriginal?.IdDetallePedido ?? Guid.Empty,
                IdProducto = _productoSeleccionado?.IdProducto,
                NombreProducto = nombreProducto,
                IdCategoria = categoriaId,
                Categoria = (_categorias.FirstOrDefault(c => c.IdCategoria == categoriaId)?.NombreCategoria),
                IdProveedor = proveedorId,
                Proveedor = (_proveedoresProductos.FirstOrDefault(p => p.IdProveedor == proveedorId)?.RazonSocial),
                Cantidad = (int)nudCantidad.Value,
                PrecioUnitario = nudPrecio.Value,
                IdEstadoProducto = idEstado,
                EstadoProducto = idEstado.HasValue ? _estadosProducto.FirstOrDefault(e => e.IdEstadoProducto == idEstado)?.NombreEstadoProducto : null,
                FechaLimite = fechaLimite,
                FichaAplicacion = chkFicha.Checked,
                Notas = txtNotas.Text?.Trim(),
                IdProveedorPersonalizacion = idProveedorPersonalizacion,
                ProveedorPersonalizacion = idProveedorPersonalizacion.HasValue ? _proveedoresPersonalizacion.FirstOrDefault(p => p.IdProveedor == idProveedorPersonalizacion)?.RazonSocial : null,
                Logos = _logos.Select(CloneLogo).ToList()
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}