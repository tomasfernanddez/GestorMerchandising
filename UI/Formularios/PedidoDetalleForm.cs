using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
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
        private string _textoProveedorCorto;
        private readonly DateTime? _fechaLimitePedido;
        private bool _combosInicializados;
        private ListBox _listaSugerencias;
        private bool _seleccionandoSugerencia;
        private bool _formateandoPrecio;

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
            PedidoDetalleViewModel detalle = null,
            DateTime? fechaLimitePedido = null)
        {
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _categorias = categorias?.OrderBy(c => c.NombreCategoria).ToList() ?? new List<CategoriaProducto>();
            _proveedoresProductos = proveedoresProductos?.OrderBy(p => p.RazonSocial).ToList() ?? new List<Proveedor>();
            _estadosProducto = estadosProducto?.OrderBy(e => e.NombreEstadoProducto).ToList() ?? new List<EstadoProducto>();
            _tecnicas = tecnicas?.OrderBy(t => t.NombreTecnicaPersonalizacion).ToList() ?? new List<TecnicaPersonalizacion>();
            _ubicaciones = ubicaciones?.OrderBy(u => u.NombreUbicacionLogo).ToList() ?? new List<UbicacionLogo>();
            _proveedoresPersonalizacion = proveedoresPersonalizacion?.OrderBy(p => p.RazonSocial).ToList() ?? new List<Proveedor>();
            _detalleOriginal = detalle;
            _fechaLimitePedido = fechaLimitePedido;
            _textoProveedorCorto = "order.detail.provider.short".Traducir();

            InitializeComponent();
            InicializarControlesPersonalizados();
        }

        private void PedidoDetalleForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            ConfigurarCombos();
            ConfigurarGrillaLogos();
            CargarDetalle();
            ActualizarUbicacionSugerencias();
            FormatearPrecio(true);
        }

        private void InicializarControlesPersonalizados()
        {
            _listaSugerencias = new ListBox
            {
                Visible = false,
                IntegralHeight = false,
                Height = 150,
                Width = cmbProducto.Width,
                SelectionMode = SelectionMode.One,
                Font = cmbProducto.Font,
                TabStop = false
            };
            _listaSugerencias.Click += ListaSugerencias_Click;
            _listaSugerencias.KeyDown += ListaSugerencias_KeyDown;
            _listaSugerencias.Leave += ListaSugerencias_Leave;

            Controls.Add(_listaSugerencias);
            _listaSugerencias.BringToFront();

            cmbProducto.LocationChanged += (s, e) => ActualizarUbicacionSugerencias();
            cmbProducto.SizeChanged += (s, e) => ActualizarUbicacionSugerencias();
            tableLayoutPanel1.LocationChanged += (s, e) => ActualizarUbicacionSugerencias();
            tableLayoutPanel1.SizeChanged += (s, e) => ActualizarUbicacionSugerencias();
            SizeChanged += (s, e) => ActualizarUbicacionSugerencias();
            Move += (s, e) => ActualizarUbicacionSugerencias();

            nudPrecio.ThousandsSeparator = true;
            nudPrecio.ValueChanged += nudPrecio_ValueChanged;
            nudPrecio.Leave += nudPrecio_Leave;
        }

        private void ActualizarUbicacionSugerencias()
        {
            if (_listaSugerencias == null || _listaSugerencias.IsDisposed)
                return;
            if (cmbProducto == null || cmbProducto.IsDisposed)
                return;

            var ubicacionPantalla = cmbProducto.PointToScreen(new Point(0, cmbProducto.Height));
            var ubicacionCliente = PointToClient(ubicacionPantalla);
            _listaSugerencias.Location = ubicacionCliente;
            _listaSugerencias.Width = cmbProducto.Width;

            if (_listaSugerencias.Visible)
                _listaSugerencias.BringToFront();
        }

        private void MostrarSugerencias(List<ProductoSuggestion> sugerencias)
        {
            if (_listaSugerencias == null)
                return;

            ActualizarUbicacionSugerencias();

            _listaSugerencias.BeginUpdate();
            _listaSugerencias.Items.Clear();
            foreach (var sugerencia in sugerencias)
            {
                _listaSugerencias.Items.Add(sugerencia);
            }
            _listaSugerencias.EndUpdate();

            if (_listaSugerencias.Items.Count > 0)
            {
                var itemHeight = _listaSugerencias.ItemHeight <= 0 ? 18 : _listaSugerencias.ItemHeight;
                var visibles = Math.Min(8, _listaSugerencias.Items.Count);
                _listaSugerencias.Height = Math.Max(60, itemHeight * visibles + 6);
                _listaSugerencias.SelectedIndex = 0;
                _listaSugerencias.Visible = true;
                _listaSugerencias.BringToFront();
            }
            else
            {
                _listaSugerencias.Visible = false;
            }
        }

        private void OcultarSugerencias()
        {
            if (_listaSugerencias == null)
                return;

            _listaSugerencias.Visible = false;
            _listaSugerencias.SelectedIndex = -1;
        }

        private void AplicarSugerencia(ProductoSuggestion sugerencia)
        {
            if (sugerencia == null)
                return;

            _seleccionandoSugerencia = true;
            try
            {
                _productoSeleccionado = sugerencia.Producto;
                cmbProducto.Text = sugerencia.Producto.NombreProducto;
                cmbProducto.SelectionStart = cmbProducto.Text.Length;
                cmbProducto.SelectionLength = 0;

                if (_productoSeleccionado.IdCategoria.HasValue && cmbCategoria.Items.Count > 0)
                {
                    cmbCategoria.SelectedValue = _productoSeleccionado.IdCategoria.Value;
                }

                if (_productoSeleccionado.IdProveedor.HasValue && cmbProveedor.Items.Count > 0)
                {
                    cmbProveedor.SelectedValue = _productoSeleccionado.IdProveedor.Value;
                }
            }
            finally
            {
                _seleccionandoSugerencia = false;
            }

            OcultarSugerencias();
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
            lblNotas.Text = "order.detail.notes".Traducir();
            gbLogos.Text = "order.detail.logos".Traducir();
            btnAgregarLogo.Text = "order.detail.logo.add".Traducir();
            btnEditarLogo.Text = "order.detail.logo.edit".Traducir();
            btnEliminarLogo.Text = "order.detail.logo.delete".Traducir();
            btnAceptar.Text = "form.accept".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();

            _textoProveedorCorto = "order.detail.provider.short".Traducir();

            if (_combosInicializados)
            {
                ConfigurarCombos(true);
            }
        }

        private void ConfigurarCombos(bool mantenerSeleccion = false)
        {
            cmbProducto.AutoCompleteMode = AutoCompleteMode.None;
            cmbProducto.AutoCompleteSource = AutoCompleteSource.None;

            var categoriaSeleccionada = mantenerSeleccion && cmbCategoria.SelectedValue is Guid cat && cat != Guid.Empty
                ? cat
                : Guid.Empty;
            var proveedorSeleccionado = mantenerSeleccion && cmbProveedor.SelectedValue is Guid prov && prov != Guid.Empty
                ? prov
                : Guid.Empty;
            var estadoSeleccionado = mantenerSeleccion && cmbEstado.SelectedValue is Guid estado && estado != Guid.Empty
                ? (Guid?)estado
                : null;

            var categorias = new List<CategoriaProducto>
            {
                new CategoriaProducto { IdCategoria = Guid.Empty, NombreCategoria = "form.select.optional".Traducir() }
            };
            categorias.AddRange(_categorias);
            cmbCategoria.DisplayMember = nameof(CategoriaProducto.NombreCategoria);
            cmbCategoria.ValueMember = nameof(CategoriaProducto.IdCategoria);
            cmbCategoria.DataSource = categorias;
            if (mantenerSeleccion && categoriaSeleccionada != Guid.Empty && categorias.Any(c => c.IdCategoria == categoriaSeleccionada))
                cmbCategoria.SelectedValue = categoriaSeleccionada;
            else
                cmbCategoria.SelectedIndex = 0;

            var proveedores = new List<Proveedor>
            {
                new Proveedor { IdProveedor = Guid.Empty, RazonSocial = "form.select.optional".Traducir() }
            };
            proveedores.AddRange(_proveedoresProductos);
            cmbProveedor.DisplayMember = nameof(Proveedor.RazonSocial);
            cmbProveedor.ValueMember = nameof(Proveedor.IdProveedor);
            cmbProveedor.DataSource = proveedores;
            if (mantenerSeleccion && proveedorSeleccionado != Guid.Empty && proveedores.Any(p => p.IdProveedor == proveedorSeleccionado))
                cmbProveedor.SelectedValue = proveedorSeleccionado;
            else
                cmbProveedor.SelectedIndex = 0;

            cmbEstado.DisplayMember = nameof(EstadoProducto.NombreEstadoProducto);
            cmbEstado.ValueMember = nameof(EstadoProducto.IdEstadoProducto);
            cmbEstado.DataSource = _estadosProducto;
            if (mantenerSeleccion && estadoSeleccionado.HasValue && _estadosProducto.Any(e => e.IdEstadoProducto == estadoSeleccionado.Value))
                cmbEstado.SelectedValue = estadoSeleccionado.Value;
            else
                SeleccionarEstadoProductoProduccion();

            _combosInicializados = true;
        }

        private void SeleccionarEstadoProductoProduccion()
        {
            var estadoProduccion = _estadosProducto?.FirstOrDefault(e => EsEstadoProduccion(e.NombreEstadoProducto));
            if (estadoProduccion != null)
            {
                cmbEstado.SelectedValue = estadoProduccion.IdEstadoProducto;
            }
            else if (cmbEstado.Items.Count > 0)
            {
                cmbEstado.SelectedIndex = 0;
            }
        }

        private static bool EsEstadoProduccion(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var compare = CultureInfo.InvariantCulture.CompareInfo;
            return compare.IndexOf(nombre, "producción", CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0
                || compare.IndexOf(nombre, "produccion", CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
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
                DataPropertyName = nameof(PedidoLogoViewModel.Descripcion),
                HeaderText = "order.detail.logo.notes".Traducir(),
                FillWeight = 160,
                MinimumWidth = 140
            });
            dgvLogos.DataSource = _logos;
            dgvLogos.CellDoubleClick += dgvLogos_CellDoubleClick;
        }

        private void dgvLogos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            btnEditarLogo.PerformClick();
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
            FormatearPrecio(true);

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
            if (_seleccionandoSugerencia)
                return;

            var textoActual = cmbProducto.Text ?? string.Empty;
            var terminoBusqueda = textoActual.Trim();
            if (terminoBusqueda.Length < 2)
            {
                _productoSeleccionado = null;
                OcultarSugerencias();
                return;
            }

            try
            {
                _productoSeleccionado = null;
                var sugerencias = _productoService.BuscarParaAutocomplete(terminoBusqueda, 12)
                    .Where(p => p != null && p.Activo)
                    .Select(p => new ProductoSuggestion(p, FormatearDescripcionProducto(p)))
                    .ToList();

                MostrarSugerencias(sugerencias);
            }
            catch
            {
                OcultarSugerencias();
            }
        }

        private string FormatearDescripcionProducto(Producto producto)
        {
            var categoria = producto.Categoria?.NombreCategoria;
            var proveedor = producto.Proveedor?.RazonSocial;
            if (string.IsNullOrWhiteSpace(categoria) && string.IsNullOrWhiteSpace(proveedor))
            {
                return producto.NombreProducto;
            }

            return $"{producto.NombreProducto} ({categoria}) - {_textoProveedorCorto}: {proveedor}";
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_seleccionandoSugerencia)
                return;

            if (cmbProducto.SelectedItem is ProductoSuggestion sugerencia)
            {
                AplicarSugerencia(sugerencia);
            }
        }

        private void cmbProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (_listaSugerencias == null || !_listaSugerencias.Visible)
                return;

            if (e.KeyCode == Keys.Down)
            {
                if (_listaSugerencias.SelectedIndex < _listaSugerencias.Items.Count - 1)
                    _listaSugerencias.SelectedIndex++;
                else if (_listaSugerencias.Items.Count > 0)
                    _listaSugerencias.SelectedIndex = 0;

                _listaSugerencias.Focus();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (_listaSugerencias.SelectedIndex > 0)
                    _listaSugerencias.SelectedIndex--;
                else if (_listaSugerencias.Items.Count > 0)
                    _listaSugerencias.SelectedIndex = _listaSugerencias.Items.Count - 1;

                _listaSugerencias.Focus();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                ProductoSuggestion sugerencia = null;
                if (_listaSugerencias.SelectedItem is ProductoSuggestion seleccionada)
                {
                    sugerencia = seleccionada;
                }
                else if (_listaSugerencias.Items.Count > 0)
                {
                    sugerencia = _listaSugerencias.Items[0] as ProductoSuggestion;
                }

                if (sugerencia != null)
                {
                    AplicarSugerencia(sugerencia);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                OcultarSugerencias();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void cmbProducto_Leave(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                if (_listaSugerencias == null)
                    return;

                if (!_listaSugerencias.Focused)
                {
                    OcultarSugerencias();
                }
            }));
        }

        private void ListaSugerencias_Click(object sender, EventArgs e)
        {
            if (_listaSugerencias?.SelectedItem is ProductoSuggestion sugerencia)
            {
                AplicarSugerencia(sugerencia);
            }
        }

        private void ListaSugerencias_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_listaSugerencias?.SelectedItem is ProductoSuggestion sugerencia)
                {
                    AplicarSugerencia(sugerencia);
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                OcultarSugerencias();
                cmbProducto.Focus();
                e.Handled = true;
            }
        }

        private void ListaSugerencias_Leave(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                if (!cmbProducto.Focused)
                {
                    OcultarSugerencias();
                }
            }));
        }

        private void nudPrecio_ValueChanged(object sender, EventArgs e)
        {
            if (!nudPrecio.Focused)
            {
                FormatearPrecio(true);
            }
        }

        private void nudPrecio_Leave(object sender, EventArgs e)
        {
            FormatearPrecio(true);
        }

        private void FormatearPrecio(bool forzar = false)
        {
            if (_formateandoPrecio)
                return;

            if (!forzar && nudPrecio.Focused)
                return;

            if (!(nudPrecio.Controls.OfType<TextBox>().FirstOrDefault() is TextBox txt))
                return;

            _formateandoPrecio = true;
            try
            {
                txt.Text = nudPrecio.Value.ToString("N2", CultureInfo.CurrentCulture);
                txt.SelectionStart = txt.Text.Length;
                txt.SelectionLength = 0;
            }
            finally
            {
                _formateandoPrecio = false;
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
                if (!ValidarCantidadLogo(form.LogoResult))
                    return;

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
                if (!ValidarCantidadLogo(form.LogoResult))
                    return;

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

        private bool ValidarCantidadLogo(PedidoLogoViewModel logo)
        {
            if (logo == null)
                return true;

            if (logo.Cantidad > (int)nudCantidad.Value)
            {
                MessageBox.Show("order.detail.logo.validation.quantity".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            OcultarSugerencias();
            var textoIngresado = cmbProducto.Text ?? string.Empty;
            var nombreProducto = textoIngresado.Trim();
            if (_productoSeleccionado != null && !string.IsNullOrWhiteSpace(_productoSeleccionado.NombreProducto))
            {
                nombreProducto = _productoSeleccionado.NombreProducto;
            }

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

            var idProveedorPersonalizacion = _detalleOriginal?.IdProveedorPersonalizacion;
            var proveedorPersonalizacion = _detalleOriginal?.ProveedorPersonalizacion;
            DateTime? fechaLimite = chkFechaLimite.Checked ? dtpFechaLimite.Value.Date : (DateTime?)null;

            if (_fechaLimitePedido.HasValue && fechaLimite.HasValue && fechaLimite.Value >= _fechaLimitePedido.Value)
            {
                MessageBox.Show("order.detail.validation.deadlineOrder".Traducir(_fechaLimitePedido.Value.ToString("d")), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFechaLimite.Focus();
                return;
            }

            if (_logos.Any(l => l.Cantidad > (int)nudCantidad.Value))
            {
                MessageBox.Show("order.detail.logo.validation.quantity".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                EstadoProducto = idEstado.HasValue ? _estadosProducto.FirstOrDefault(estado => estado.IdEstadoProducto == idEstado)?.NombreEstadoProducto : null,
                FechaLimite = fechaLimite,
                FichaAplicacion = chkFicha.Checked,
                Notas = txtNotas.Text?.Trim(),
                IdProveedorPersonalizacion = idProveedorPersonalizacion,
                ProveedorPersonalizacion = proveedorPersonalizacion,
                Logos = _logos.Select(CloneLogo).ToList()
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}