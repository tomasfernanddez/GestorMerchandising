using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BLL.Helpers;
using BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using Services;
using Services.BLL.Factories;
using Services.BLL.Interfaces;
using Services.DomainModel.Entities;
using UI.Localization;

namespace UI
{
    public partial class ProveedorForm : Form
    {
        private readonly IProveedorService _proveedorService;
        private readonly IBitacoraService _bitacoraService;
        private readonly IGeoService _geoService;
        private readonly ICondicionIvaService _condicionIvaService;
        private readonly bool _esEdicion;
        private Proveedor _model;

        private IList<GeoDTO> _paises = new List<GeoDTO>();
        private IList<GeoDTO> _provincias = new List<GeoDTO>();
        private IList<GeoDTO> _localidades = new List<GeoDTO>();
        private IList<TipoProveedor> _tiposProveedor = new List<TipoProveedor>();
        private IList<TecnicaPersonalizacion> _tecnicasDisponibles = new List<TecnicaPersonalizacion>();
        private IList<CondicionIva> _condicionesIva = new List<CondicionIva>();

        private class Item
        {
            public Guid Id { get; set; }
            public string Nombre { get; set; }
            public override string ToString() => Nombre;
        }

        private class TecnicaItem
        {
            public Guid Id { get; set; }
            public string Nombre { get; set; }
            public override string ToString() => Nombre;
        }

        public ProveedorForm()
        {
            InitializeComponent();
        }

        public ProveedorForm(IProveedorService proveedorService, IBitacoraService bitacoraService, IGeoService geoService, ICondicionIvaService condicionIvaService, Proveedor proveedor)
        {
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
            _condicionIvaService = condicionIvaService ?? throw new ArgumentNullException(nameof(condicionIvaService));
            _model = proveedor;
            _esEdicion = proveedor != null;

            InitializeComponent();
            Load += ProveedorForm_Load;
        }

        private void ProveedorForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            ConfigurarControles();
            CargarCatalogos();
            CargarCombosGeo();
            CargarModelo();
            WireUp();
        }

        private void ApplyTexts()
        {
            Text = _esEdicion
                ? "abm.common.edit".Traducir() + " - " + "abm.suppliers.title".Traducir()
                : "abm.common.new".Traducir() + " - " + "abm.suppliers.title".Traducir();

            grpGenerales.Text = "supplier.group.general".Traducir();
            grpUbicacion.Text = "supplier.group.ubicacion".Traducir();
            grpTecnicas.Text = "supplier.group.tecnicas".Traducir();
            grpObservaciones.Text = "supplier.group.observaciones".Traducir();

            lblRazonSocial.Text = "supplier.razonSocial".Traducir();
            lblAlias.Text = "supplier.alias".Traducir();
            lblCUIT.Text = "supplier.cuit".Traducir();
            lblCondicionIVA.Text = "supplier.condicionIVA".Traducir();
            lblTipoProveedor.Text = "supplier.tipoProveedor".Traducir();
            lblCondicionesPago.Text = "supplier.condicionesPago".Traducir();
            chkActivo.Text = "supplier.status.active".Traducir();
            lblFechaAlta.Text = "supplier.fechaAlta".Traducir();

            lblDomicilio.Text = "supplier.domicilio".Traducir();
            lblCodigoPostal.Text = "supplier.codigoPostal".Traducir();
            lblPais.Text = "supplier.pais".Traducir();
            lblProvincia.Text = "supplier.provincia".Traducir();
            lblLocalidad.Text = "supplier.localidad".Traducir();

            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
        }

        private void ConfigurarControles()
        {
            cboCondicionIVA.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCondicionesPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPais.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProvincia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocalidad.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void WireUp()
        {
            btnGuardar.Click += (s, e) => Guardar();
            btnCancelar.Click += (s, e) => DialogResult = DialogResult.Cancel;
            cboPais.SelectedIndexChanged += (s, e) => CargarProvincias();
            cboProvincia.SelectedIndexChanged += (s, e) => CargarLocalidades();
            clbTiposProveedor.ItemCheck += (s, e) => BeginInvoke((Action)ActualizarVisibilidadTecnicas);
        }

        private void CargarCatalogos()
        {
            _condicionesIva = _condicionIvaService.ObtenerTodas()?.ToList() ?? new List<CondicionIva>();
            cboCondicionIVA.DisplayMember = nameof(Item.Nombre);
            cboCondicionIVA.ValueMember = nameof(Item.Id);
            var condiciones = _condicionesIva
                .Select(ci => new Item { Id = ci.IdCondicionIva, Nombre = ci.Nombre })
                .ToList();
            condiciones.Insert(0, new Item { Id = Guid.Empty, Nombre = "form.select.optional".Traducir() });
            cboCondicionIVA.DataSource = condiciones;
            if (cboCondicionIVA.Items.Count > 0)
                cboCondicionIVA.SelectedIndex = 0;

            cboCondicionesPago.Items.Clear();
            cboCondicionesPago.Items.Add("form.select.optional".Traducir());
            foreach (var item in ProveedorCatalogoHelper.CondicionesPago)
            {
                cboCondicionesPago.Items.Add(item);
            }
            if (cboCondicionesPago.Items.Count > 0)
                cboCondicionesPago.SelectedIndex = 0;

            _tiposProveedor = _proveedorService.ObtenerTiposProveedor()?.ToList() ?? new List<TipoProveedor>();
            clbTiposProveedor.Items.Clear();
            foreach (var tipo in _tiposProveedor)
            {
                clbTiposProveedor.Items.Add(new Item { Id = tipo.IdTipoProveedor, Nombre = tipo.TipoProveedorNombre }, false);
            }

            _tecnicasDisponibles = _proveedorService.ObtenerTecnicasPersonalizacion()?.ToList() ?? new List<TecnicaPersonalizacion>();
            clbTecnicas.Items.Clear();
            foreach (var tecnica in _tecnicasDisponibles)
            {
                clbTecnicas.Items.Add(new TecnicaItem { Id = tecnica.IdTecnicaPersonalizacion, Nombre = tecnica.NombreTecnicaPersonalizacion });
            }
        }

        private void CargarCombosGeo()
        {
            _paises = _geoService.ListarPaises() ?? new List<GeoDTO>();
            var paises = new List<Item> { new Item { Id = Guid.Empty, Nombre = "form.select.optional".Traducir() } };
            paises.AddRange(_paises.Select(p => new Item { Id = p.Id, Nombre = p.Nombre }));
            cboPais.DataSource = paises;
            cboPais.DisplayMember = nameof(Item.Nombre);
            cboPais.ValueMember = nameof(Item.Id);

            if (cboPais.Items.Count > 0)
                cboPais.SelectedIndex = 0;

            CargarProvincias();
            CargarLocalidades();
        }

        private void CargarProvincias()
        {
            if (cboPais.SelectedItem is Item pais)
            {
                _provincias = _geoService.ListarProvinciasPorPais(pais.Id) ?? new List<GeoDTO>();
                var provincias = new List<Item> { new Item { Id = Guid.Empty, Nombre = "form.select.optional".Traducir() } };
                provincias.AddRange(_provincias.Select(p => new Item { Id = p.Id, Nombre = p.Nombre }));
                cboProvincia.DataSource = provincias;
                cboProvincia.DisplayMember = nameof(Item.Nombre);
                cboProvincia.ValueMember = nameof(Item.Id);
                if (cboProvincia.Items.Count > 0)
                    cboProvincia.SelectedIndex = 0;
            }
            else
            {
                cboProvincia.DataSource = null;
            }
        }

        private void CargarLocalidades()
        {
            if (cboProvincia.SelectedItem is Item provincia)
            {
                _localidades = _geoService.ListarLocalidadesPorProvincia(provincia.Id) ?? new List<GeoDTO>();
                var localidades = new List<Item> { new Item { Id = Guid.Empty, Nombre = "form.select.optional".Traducir() } };
                localidades.AddRange(_localidades.Select(l => new Item { Id = l.Id, Nombre = l.Nombre }));
                cboLocalidad.DataSource = localidades;
                cboLocalidad.DisplayMember = nameof(Item.Nombre);
                cboLocalidad.ValueMember = nameof(Item.Id);
                if (cboLocalidad.Items.Count > 0)
                    cboLocalidad.SelectedIndex = 0;
            }
            else
            {
                cboLocalidad.DataSource = null;
            }
        }

        private void CargarModelo()
        {
            if (_model == null)
            {
                _model = new Proveedor
                {
                    Activo = true,
                    FechaAlta = DateTime.Now
                };

                chkActivo.Checked = true;
                lblFechaAltaValor.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtAlias.Text = string.Empty;
                ActualizarVisibilidadTecnicas();
                return;
            }

            txtRazonSocial.Text = _model.RazonSocial;
            txtAlias.Text = _model.Alias;
            txtCUIT.Text = FormatearCuit(_model.CUIT);
            txtDomicilio.Text = _model.Domicilio;
            txtCodigoPostal.Text = _model.CodigoPostal;
            txtObservaciones.Text = _model.Observaciones;
            chkActivo.Checked = _model.Activo;
            lblFechaAltaValor.Text = _model.FechaAlta.ToString("dd/MM/yyyy");

            if (_model.IdCondicionIva != Guid.Empty)
            {
                SeleccionarItemCombo(cboCondicionIVA, _model.IdCondicionIva);
            }
            SeleccionarItemCombo(cboCondicionesPago, _model.CondicionesPago);

            if (_model.TiposProveedor != null)
            {
                var seleccionadas = _model.TiposProveedor
                    .Select(tp => tp.IdTipoProveedor)
                    .ToHashSet();

                for (int i = 0; i < clbTiposProveedor.Items.Count; i++)
                {
                    if (clbTiposProveedor.Items[i] is Item item && seleccionadas.Contains(item.Id))
                    {
                        clbTiposProveedor.SetItemChecked(i, true);
                    }
                }
            }

            if (_model.IdPais.HasValue)
            {
                SeleccionarItemCombo(cboPais, _model.IdPais.Value);
            }

            CargarProvincias();

            if (_model.IdProvincia.HasValue)
            {
                SeleccionarItemCombo(cboProvincia, _model.IdProvincia.Value);
            }

            CargarLocalidades();

            if (_model.IdLocalidad.HasValue)
            {
                SeleccionarItemCombo(cboLocalidad, _model.IdLocalidad.Value);
            }

            if (_model.TecnicasPersonalizacion != null)
            {
                var seleccionadas = _model.TecnicasPersonalizacion.Select(t => t.IdTecnicaPersonalizacion).ToHashSet();
                for (int i = 0; i < clbTecnicas.Items.Count; i++)
                {
                    if (clbTecnicas.Items[i] is TecnicaItem ti && seleccionadas.Contains(ti.Id))
                    {
                        clbTecnicas.SetItemChecked(i, true);
                    }
                }
            }

            ActualizarVisibilidadTecnicas();
        }

        private void SeleccionarItemCombo(ComboBox combo, Guid id)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if ((combo.Items[i] as Item)?.Id == id)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        private void SeleccionarItemCombo(ComboBox combo, string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return;

            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (string.Equals(combo.Items[i]?.ToString(), texto, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        private void Guardar()
        {
            if (!ValidarCampos())
                return;

            try
            {
                var itPais = cboPais.SelectedItem as Item;
                var itProvincia = cboProvincia.SelectedItem as Item;
                var itLocalidad = cboLocalidad.SelectedItem as Item;
                var tiposSeleccionados = clbTiposProveedor.CheckedItems
                    .OfType<Item>()
                    .Select(t => t.Id)
                    .Distinct()
                    .ToList();

                _model.RazonSocial = txtRazonSocial.Text.Trim();
                _model.Alias = string.IsNullOrWhiteSpace(txtAlias.Text) ? null : txtAlias.Text.Trim();
                _model.CUIT = ObtenerCuitLimpio();
                var condicionSeleccionada = cboCondicionIVA.SelectedItem as Item;
                _model.IdCondicionIva = condicionSeleccionada != null && condicionSeleccionada.Id != Guid.Empty
                    ? condicionSeleccionada.Id
                    : Guid.Empty;
                _model.CondicionesPago = cboCondicionesPago.SelectedIndex > 0
                    ? cboCondicionesPago.SelectedItem?.ToString()
                    : null;
                _model.Domicilio = txtDomicilio.Text.Trim();
                _model.CodigoPostal = txtCodigoPostal.Text.Trim();
                _model.IdPais = itPais != null && itPais.Id != Guid.Empty ? itPais.Id : (Guid?)null;
                _model.IdProvincia = itProvincia != null && itProvincia.Id != Guid.Empty ? itProvincia.Id : (Guid?)null;
                _model.IdLocalidad = itLocalidad != null && itLocalidad.Id != Guid.Empty ? itLocalidad.Id : (Guid?)null;
                _model.Localidad = itLocalidad != null && itLocalidad.Id != Guid.Empty ? itLocalidad.Nombre : null;
                _model.Observaciones = txtObservaciones.Text.Trim();
                _model.Activo = chkActivo.Checked;
                _model.TiposProveedor = _tiposProveedor
                    .Where(tp => tiposSeleccionados.Contains(tp.IdTipoProveedor))
                    .ToList();
                if (!_esEdicion)
                {
                    _model.FechaAlta = DateTime.Now;
                }

                var tecnicasSeleccionadas = clbTecnicas.CheckedItems
                    .OfType<TecnicaItem>()
                    .Select(t => t.Id)
                    .ToList();

                ResultadoOperacion resultado;
                if (_esEdicion)
                {
                    resultado = _proveedorService.ActualizarProveedor(_model, tiposSeleccionados, tecnicasSeleccionadas);
                    _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, "Proveedor.Editar",
                        resultado.EsValido ? $"Id={_model.IdProveedor}" : resultado.Mensaje,
                        "Proveedores", resultado.EsValido);
                }
                else
                {
                    resultado = _proveedorService.CrearProveedor(_model, tiposSeleccionados, tecnicasSeleccionadas);
                    _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, "Proveedor.Alta",
                        resultado.EsValido ? $"Id={resultado.IdGenerado}" : resultado.Mensaje,
                        "Proveedores", resultado.EsValido);
                }

                if (!resultado.EsValido)
                {
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var logSvc = ServicesFactory.CrearLogService();
                var accion = _esEdicion ? "Proveedor actualizado / Supplier updated" : "Proveedor creado / Supplier created";
                logSvc.LogInfo(accion, "Proveedores", SessionContext.NombreUsuario);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, _esEdicion ? "Proveedor.Editar" : "Proveedor.Alta", ex.Message, "Proveedores", false);
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogError("Error guardando proveedor / Error saving supplier", ex, "Proveedores", SessionContext.NombreUsuario);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCampos()
        {
            errorProvider1.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtRazonSocial.Text) || txtRazonSocial.Text.Trim().Length < 3)
            {
                errorProvider1.SetError(txtRazonSocial, "msg.required".Traducir());
                ok = false;
            }

            var (cuitValido, mensajeCuit) = ValidationHelper.ValidarCUIT(txtCUIT.Text);
            if (!cuitValido)
            {
                errorProvider1.SetError(txtCUIT, mensajeCuit);
                ok = false;
            }

            if (cboCondicionIVA.SelectedItem == null)
            {
                errorProvider1.SetError(cboCondicionIVA, "msg.required".Traducir());
                ok = false;
            }

            if (clbTiposProveedor.CheckedItems.Count == 0)
            {
                errorProvider1.SetError(clbTiposProveedor, "msg.required".Traducir());
                ok = false;
            }

            if (cboCondicionesPago.SelectedItem == null)
            {
                errorProvider1.SetError(cboCondicionesPago, "msg.required".Traducir());
                ok = false;
            }

            if (cboPais.SelectedItem == null)
            {
                errorProvider1.SetError(cboPais, "msg.required".Traducir());
                ok = false;
            }

            if (cboProvincia.SelectedItem == null)
            {
                errorProvider1.SetError(cboProvincia, "msg.required".Traducir());
                ok = false;
            }

            if (cboLocalidad.SelectedItem == null)
            {
                errorProvider1.SetError(cboLocalidad, "msg.required".Traducir());
                ok = false;
            }

            var tiposSeleccionados = ObtenerTiposProveedorSeleccionados();

            if (tiposSeleccionados.Any(ProveedorCatalogoHelper.EsTipoPersonalizador))
            {
                if (clbTecnicas.CheckedItems.Count == 0)
                {
                    errorProvider1.SetError(clbTecnicas, "supplier.validation.tecnica".Traducir());
                    ok = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtObservaciones.Text) && txtObservaciones.Text.Length > 500)
            {
                errorProvider1.SetError(txtObservaciones, "supplier.validation.observaciones".Traducir());
                ok = false;
            }

            return ok;
        }

        private void ActualizarVisibilidadTecnicas()
        {
            var tipos = ObtenerTiposProveedorSeleccionados();
            bool esPersonalizador = tipos.Any(ProveedorCatalogoHelper.EsTipoPersonalizador);

            grpTecnicas.Enabled = esPersonalizador;
            grpTecnicas.Visible = esPersonalizador || clbTecnicas.CheckedItems.Count > 0;
        }

        private List<TipoProveedor> ObtenerTiposProveedorSeleccionados()
        {
            if (clbTiposProveedor.CheckedItems.Count == 0)
                return new List<TipoProveedor>();

            var seleccionados = clbTiposProveedor.CheckedItems
                .OfType<Item>()
                .Select(i => i.Id)
                .ToHashSet();

            return _tiposProveedor
                .Where(tp => seleccionados.Contains(tp.IdTipoProveedor))
                .ToList();
        }

        private string ObtenerCuitLimpio()
        {
            return Regex.Replace(txtCUIT.Text ?? string.Empty, "[^0-9]", string.Empty);
        }

        private static string FormatearCuit(string cuit)
        {
            var limpio = Regex.Replace(cuit ?? string.Empty, "[^0-9]", string.Empty);
            if (limpio.Length != 11)
                return limpio;
            return $"{limpio.Substring(0, 2)}-{limpio.Substring(2, 8)}-{limpio.Substring(10)}";
        }
    }
}