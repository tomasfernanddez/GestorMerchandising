using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL.Interfaces;
using Services;
using Services.BLL.Factories;
using Services.BLL.Interfaces;
using UI.Localization;
using UI.Helpers;

namespace UI
{
    public partial class ABMProveedoresForm : Form
    {
        private readonly IProveedorService _proveedorService;
        private readonly IBitacoraService _bitacoraService;
        private readonly IGeoService _geoService;
        private readonly ICondicionIvaService _condicionIvaService;
        private bool _puedeEliminar;
        private IList<DomainModel.Entidades.TipoProveedor> _tiposProveedor = new List<DomainModel.Entidades.TipoProveedor>();
        private bool _cargandoFiltros;

        private sealed class ProveedorGridRow
        {
            public Guid IdProveedor { get; set; }
            public string RazonSocial { get; set; }
            public string Alias { get; set; }
            public string CUIT { get; set; }
            public string TipoProveedor { get; set; }
            public string Localidad { get; set; }
            public string Estado { get; set; }
            public bool Activo { get; set; }
        }

        public ABMProveedoresForm()
        {
            InitializeComponent();
        }

        public ABMProveedoresForm(IProveedorService proveedorService, IBitacoraService bitacoraService, IGeoService geoService, ICondicionIvaService condicionIvaService)
        {
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
            _condicionIvaService = condicionIvaService ?? throw new ArgumentNullException(nameof(condicionIvaService));

            InitializeComponent();

            Load += ABMProveedoresForm_Load;
        }

        private void ABMProveedoresForm_Load(object sender, EventArgs e)
        {
            EnsureColumns();

            _puedeEliminar = (SessionContext.NombrePerfil ?? string.Empty)
                .Equals("Administrador", StringComparison.OrdinalIgnoreCase);

            tsbEliminar.Visible = _puedeEliminar;
            tsbActivar.Visible = _puedeEliminar;

            ApplyTexts();
            WireUp();
            CargarFiltros();
            CargarProveedores();
        }

        private void EnsureColumns()
        {
            if (dgvProveedores.Columns.Count > 0)
                return;

            dgvProveedores.AutoGenerateColumns = false;
            dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProveedorGridRow.IdProveedor),
                Name = nameof(ProveedorGridRow.IdProveedor),
                Visible = false
            });
            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProveedorGridRow.RazonSocial),
                Name = nameof(ProveedorGridRow.RazonSocial),
                HeaderText = "supplier.column.razon".Traducir(),
                FillWeight = 220,
                MinimumWidth = 160
            });
            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProveedorGridRow.Alias),
                Name = nameof(ProveedorGridRow.Alias),
                HeaderText = "supplier.column.alias".Traducir(),
                FillWeight = 160,
                MinimumWidth = 140
            });
            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProveedorGridRow.CUIT),
                Name = nameof(ProveedorGridRow.CUIT),
                HeaderText = "supplier.column.cuit".Traducir(),
                FillWeight = 120,
                MinimumWidth = 110
            });
            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProveedorGridRow.TipoProveedor),
                Name = nameof(ProveedorGridRow.TipoProveedor),
                HeaderText = "supplier.column.type".Traducir(),
                FillWeight = 140,
                MinimumWidth = 130
            });
            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProveedorGridRow.Localidad),
                Name = nameof(ProveedorGridRow.Localidad),
                HeaderText = "supplier.column.location".Traducir(),
                FillWeight = 160,
                MinimumWidth = 140
            });
            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProveedorGridRow.Estado),
                Name = nameof(ProveedorGridRow.Estado),
                HeaderText = "supplier.column.status".Traducir(),
                FillWeight = 90,
                MinimumWidth = 80
            });
        }

        private void ApplyTexts()
        {
            Text = "abm.suppliers.title".Traducir();

            tsbNuevo.Text = "abm.common.new".Traducir();
            tsbEditar.Text = "abm.common.edit".Traducir();
            tsbEliminar.Text = "abm.common.delete".Traducir();
            tsbActivar.Text = "supplier.tool.activate".Traducir();
            tsbActualizar.Text = "abm.common.refresh".Traducir();

            lblBuscarRazon.Text = "supplier.filter.razon".Traducir();
            lblFiltroTipo.Text = "supplier.filter.type".Traducir();
            lblFiltroEstado.Text = "supplier.filter.status".Traducir();

            SetHeaderSafe(nameof(ProveedorGridRow.RazonSocial), "supplier.column.razon");
            SetHeaderSafe(nameof(ProveedorGridRow.Alias), "supplier.column.alias");
            SetHeaderSafe(nameof(ProveedorGridRow.CUIT), "supplier.column.cuit");
            SetHeaderSafe(nameof(ProveedorGridRow.TipoProveedor), "supplier.column.type");
            SetHeaderSafe(nameof(ProveedorGridRow.Localidad), "supplier.column.location");
            SetHeaderSafe(nameof(ProveedorGridRow.Estado), "supplier.column.status");
        }

        private void SetHeaderSafe(string columnName, string key)
        {
            var col = dgvProveedores?.Columns?[columnName];
            if (col != null)
                col.HeaderText = key.Traducir();
        }

        private void WireUp()
        {
            tsbNuevo.Click += (s, e) => NuevoProveedor();
            tsbEditar.Click += (s, e) => EditarSeleccionado();
            tsbEliminar.Click += (s, e) => EliminarSeleccionado();
            tsbActivar.Click += (s, e) => ActivarSeleccionado();
            tsbActualizar.Click += (s, e) => CargarProveedores();
            dgvProveedores.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };
            txtBuscarRazon.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Buscar(); } };
            txtBuscarRazon.TextChanged += (s, e) => Buscar();
            cboFiltroTipo.ComboBox.SelectedIndexChanged += (s, e) => { if (!_cargandoFiltros) Buscar(); };
            cboFiltroEstado.ComboBox.SelectedIndexChanged += (s, e) => { if (!_cargandoFiltros) Buscar(); };
        }

        private void CargarFiltros()
        {
            try
            {
                _cargandoFiltros = true;
                _tiposProveedor = _proveedorService.ObtenerTiposProveedor()?.ToList() ?? new List<DomainModel.Entidades.TipoProveedor>();

                cboFiltroTipo.ComboBox.DisplayMember = "TipoProveedorNombre";
                cboFiltroTipo.ComboBox.ValueMember = "IdTipoProveedor";

                var tipos = new List<DomainModel.Entidades.TipoProveedor>
                {
                    new DomainModel.Entidades.TipoProveedor { IdTipoProveedor = Guid.Empty, TipoProveedorNombre = "form.select.optional".Traducir() }
                };
                tipos.AddRange(_tiposProveedor);
                cboFiltroTipo.ComboBox.DataSource = tipos;
                cboFiltroTipo.ComboBox.SelectedIndex = 0;

                cboFiltroEstado.ComboBox.Items.Clear();
                cboFiltroEstado.ComboBox.Items.Add(new ComboItem(null, "supplier.status.all".Traducir()));
                cboFiltroEstado.ComboBox.Items.Add(new ComboItem(null, "form.select.optional".Traducir()));
                cboFiltroEstado.ComboBox.Items.Add(new ComboItem(true, "supplier.status.active".Traducir()));
                cboFiltroEstado.ComboBox.Items.Add(new ComboItem(false, "supplier.status.inactive".Traducir()));
                cboFiltroEstado.ComboBox.SelectedIndex = 1; // Activos por defecto
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error cargando filtros de proveedores / Error loading supplier filters", ex, "Proveedores", SessionContext.NombreUsuario);
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                MessageBox.Show(friendly, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _cargandoFiltros = false;
            }
        }

        private void CargarProveedores()
        {
            try
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogInfo("Cargando proveedores / Loading suppliers", "Proveedores", SessionContext.NombreUsuario);

                var estadoSeleccionado = (cboFiltroEstado.ComboBox.SelectedItem as ComboItem)?.Estado;
                var tipoSeleccionado = cboFiltroTipo.ComboBox.SelectedItem as DomainModel.Entidades.TipoProveedor;
                Guid? idTipo = tipoSeleccionado != null && tipoSeleccionado.IdTipoProveedor != Guid.Empty
                    ? tipoSeleccionado.IdTipoProveedor
                    : (Guid?)null;

                var proveedores = _proveedorService.BuscarProveedores(
                    txtBuscarRazon.Text,
                    idTipo,
                    estadoSeleccionado);

                var rows = (proveedores ?? Enumerable.Empty<DomainModel.Proveedor>())
                    .Select(p => new ProveedorGridRow
                    {
                        IdProveedor = p.IdProveedor,
                        RazonSocial = p.RazonSocial,
                        Alias = p.Alias,
                        CUIT = FormatearCuit(p.CUIT),
                        TipoProveedor = string.Join(", ",
                            (p.TiposProveedor ?? Enumerable.Empty<DomainModel.Entidades.TipoProveedor>())
                                .Select(tp => tp.TipoProveedorNombre)) ?? string.Empty,
                        Localidad = p.LocalidadRef?.Nombre ?? p.Localidad,
                        Estado = p.Activo ? "supplier.status.active".Traducir() : "supplier.status.inactive".Traducir(),
                        Activo = p.Activo
                    })
                    .ToList();

                bsProveedores.DataSource = rows;

                logSvc.LogInfo("Proveedores cargados correctamente / Suppliers loaded successfully", "Proveedores", SessionContext.NombreUsuario);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error cargando proveedores / Error loading suppliers", ex, "Proveedores", SessionContext.NombreUsuario);
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                MessageBox.Show(friendly, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Buscar()
        {
            CargarProveedores();
        }

        private void NuevoProveedor()
        {
            try
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogInfo("Alta de proveedor / Creating supplier", "Proveedores", SessionContext.NombreUsuario);

                using (var frm = new ProveedorForm(_proveedorService, _bitacoraService, _geoService, _condicionIvaService, null))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        logSvc.LogInfo("Proveedor creado / Supplier created", "Proveedores", SessionContext.NombreUsuario);
                        CargarProveedores();
                    }
                }
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error al crear proveedor / Error creating supplier", ex, "Proveedores", SessionContext.NombreUsuario);
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                MessageBox.Show(friendly, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditarSeleccionado()
        {
            var row = ObtenerFilaSeleccionada();
            if (row == null)
                return;

            try
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogInfo("Edición de proveedor / Editing supplier", "Proveedores", SessionContext.NombreUsuario);

                var proveedor = _proveedorService.ObtenerProveedorPorId(row.IdProveedor);

                using (var frm = new ProveedorForm(_proveedorService, _bitacoraService, _geoService, _condicionIvaService, proveedor))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        logSvc.LogInfo("Proveedor actualizado / Supplier updated", "Proveedores", SessionContext.NombreUsuario);
                        CargarProveedores();
                    }
                }
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error editando proveedor / Error editing supplier", ex, "Proveedores", SessionContext.NombreUsuario);
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                MessageBox.Show(friendly, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarSeleccionado()
        {
            if (!_puedeEliminar)
                return;

            var row = ObtenerFilaSeleccionada();
            if (row == null)
                return;

            var mensaje = "supplier.confirm.deactivate".Traducir();
            if (MessageBox.Show(mensaje, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogInfo("Desactivando proveedor / Deactivating supplier", "Proveedores", SessionContext.NombreUsuario);

                var resultado = _proveedorService.DesactivarProveedor(row.IdProveedor);
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, "Proveedor.Baja",
                    resultado.EsValido ? $"Id={row.IdProveedor}" : resultado.Mensaje,
                    "Proveedores", resultado.EsValido);

                if (!resultado.EsValido)
                {
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CargarProveedores();
                logSvc.LogInfo("Proveedor desactivado / Supplier deactivated", "Proveedores", SessionContext.NombreUsuario);
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, "Proveedor.Baja", friendly, "Proveedores", false);
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error desactivando proveedor / Error deactivating supplier", ex, "Proveedores", SessionContext.NombreUsuario);
                MessageBox.Show(friendly, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActivarSeleccionado()
        {
            if (!_puedeEliminar)
                return;

            var row = ObtenerFilaSeleccionada();
            if (row == null)
                return;

            var mensaje = "supplier.confirm.activate".Traducir();
            if (MessageBox.Show(mensaje, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogInfo("Activando proveedor / Activating supplier", "Proveedores", SessionContext.NombreUsuario);

                var resultado = _proveedorService.ActivarProveedor(row.IdProveedor);
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, "Proveedor.Activar",
                    resultado.EsValido ? $"Id={row.IdProveedor}" : resultado.Mensaje,
                    "Proveedores", resultado.EsValido);

                if (!resultado.EsValido)
                {
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CargarProveedores();
                logSvc.LogInfo("Proveedor activado / Supplier activated", "Proveedores", SessionContext.NombreUsuario);
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, "Proveedor.Activar", friendly, "Proveedores", false);
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error activando proveedor / Error activating supplier", ex, "Proveedores", SessionContext.NombreUsuario);
                MessageBox.Show(friendly, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ProveedorGridRow ObtenerFilaSeleccionada()
        {
            if (dgvProveedores.CurrentRow?.DataBoundItem is ProveedorGridRow row)
                return row;

            return null;
        }

        private static string FormatearCuit(string cuit)
        {
            var limpio = new string((cuit ?? string.Empty).Where(char.IsDigit).ToArray());
            if (limpio.Length != 11)
                return limpio;

            return $"{limpio.Substring(0, 2)}-{limpio.Substring(2, 8)}-{limpio.Substring(10, 1)}";
        }

        private sealed class ComboItem
        {
            public ComboItem(bool? estado, string descripcion)
            {
                Estado = estado;
                Descripcion = descripcion;
            }

            public bool? Estado { get; }
            public string Descripcion { get; }

            public override string ToString() => Descripcion;
        }
    }
}