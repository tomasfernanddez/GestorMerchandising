using BLL.Helpers;
using BLL.Interfaces;
using BLL.Services;
using Services.BLL.Services;
using Services.BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UI.Localization;
using System.Linq;          // <- necesario para .Select(...)

namespace UI
{
    public partial class ClienteForm : Form
    {
        private readonly IClienteService _clienteService;
        private readonly IBitacoraService _bitacora;
        private readonly IGeoService _geoService;
        private readonly ICondicionIvaService _condicionIvaService;
        private readonly bool _esEdicion;
        private Cliente _model;
        private IList<CondicionIva> _condicionesIva = new List<CondicionIva>();
        private IList<TipoEmpresa> _tiposEmpresa = new List<TipoEmpresa>();

        private class Item { public Guid Id { get; set; } public string Nombre { get; set; } }

        public ClienteForm()
        {
            InitializeComponent();
        }

        public ClienteForm(IClienteService clienteService, IBitacoraService bitacora, IGeoService geoService, ICondicionIvaService condicionIvaService, Cliente cliente)
        {
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _bitacora = bitacora ?? throw new ArgumentNullException(nameof(bitacora));
            _geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
            _condicionIvaService = condicionIvaService ?? throw new ArgumentNullException(nameof(condicionIvaService));
            _model = cliente;
            _esEdicion = cliente != null;

            InitializeComponent();
            this.Load += ClienteForm_Load;   // lógica en Load
        }

        private void ClienteForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarCondicionIVA();
            CargarTiposEmpresa();
            CargarCombosGeo();  // País -> Prov -> Loc
            CargarModelo();
            WireUp();
        }

        private void ApplyTexts()
        {
            Text = _esEdicion ? "abm.common.edit".Traducir() + " - " + "abm.clients.title".Traducir()
                              : "abm.common.new".Traducir() + " - " + "abm.clients.title".Traducir();

            // traducir etiquetas y botones
            foreach (Control c in this.Controls)
            {
                if (c is Label || c is CheckBox || c is Button)
                    c.Text = (c.Text ?? "").Traducir();
            }
        }

        private void CargarModelo()
        {
            if (_model == null)
            {
                _model = new Cliente { Activo = true };
                chkActivo.Checked = true;
                if (cboCondIVA.Items.Count > 0)
                    cboCondIVA.SelectedIndex = 0;
                if (cboTipoEmpresa.Items.Count > 0)
                    cboTipoEmpresa.SelectedIndex = 0;
                return;
            }

            txtRazon.Text = _model.RazonSocial;
            txtAlias.Text = _model.Alias;
            txtCUIT.Text = _model.CUIT;
            txtDomicilio.Text = _model.Domicilio;
            SeleccionarCondicionIva(_model.IdCondicionIva);
            chkActivo.Checked = _model.Activo;

            if (_model.IdTipoEmpresa.HasValue)
            {
                for (int i = 0; i < cboTipoEmpresa.Items.Count; i++)
                    if (cboTipoEmpresa.Items[i] is Item item && item.Id == _model.IdTipoEmpresa.Value)
                    {
                        cboTipoEmpresa.SelectedIndex = i;
                        break;
                    }
            }
            else if (cboTipoEmpresa.Items.Count > 0)
            {
                cboTipoEmpresa.SelectedIndex = 0;
            }

            // País (dispara provincias)
            if (_model.IdPais.HasValue)
            {
                for (int i = 0; i < cboPais.Items.Count; i++)
                    if (((Item)cboPais.Items[i]).Id == _model.IdPais.Value) { cboPais.SelectedIndex = i; break; }
            }
            else if (cboPais.Items.Count > 0) cboPais.SelectedIndex = 0;

            // Fuerzo carga de provincias según país seleccionado
            CargarProvincias();

            // ===== Provincia =====
            if (_model.IdProvincia.HasValue)
            {
                for (int i = 0; i < cboProvincia.Items.Count; i++)
                    if (((Item)cboProvincia.Items[i]).Id == _model.IdProvincia.Value) { cboProvincia.SelectedIndex = i; break; }
            }
            else if (cboProvincia.Items.Count > 0) cboProvincia.SelectedIndex = 0;

            // Fuerzo carga de localidades según provincia seleccionada
            CargarLocalidades();

            // ===== Localidad =====
            if (_model.IdLocalidad.HasValue)
            {
                for (int i = 0; i < cboLocalidad.Items.Count; i++)
                    if (((Item)cboLocalidad.Items[i]).Id == _model.IdLocalidad.Value) { cboLocalidad.SelectedIndex = i; break; }
            }
            else if (cboLocalidad.Items.Count > 0) cboLocalidad.SelectedIndex = 0;

            // (Opcional) el string Localidad viejo lo podés mostrar como tooltip si querés:
            if (!string.IsNullOrWhiteSpace(_model.Localidad))
                cboLocalidad.Tag = _model.Localidad; // referencia histórica
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

            if (string.IsNullOrWhiteSpace(txtRazon.Text)) { errorProvider1.SetError(txtRazon, "msg.required".Traducir()); ok = false; }
            if (string.IsNullOrWhiteSpace(txtCUIT.Text))
            { errorProvider1.SetError(txtCUIT, "msg.required".Traducir()); ok = false; }
            else
            {
                var digits = Regex.Replace(txtCUIT.Text, "[^0-9]", "");
                if (digits.Length != 11) { errorProvider1.SetError(txtCUIT, "msg.cuit_invalid".Traducir()); ok = false; }
            }
            var cond = cboCondIVA.SelectedItem as Item;
            if (cond == null || cond.Id == Guid.Empty) { errorProvider1.SetError(cboCondIVA, "msg.required".Traducir()); ok = false; }

            var pais = cboPais.SelectedItem as Item;
            if (pais == null || pais.Id == Guid.Empty) { errorProvider1.SetError(cboPais, "msg.required".Traducir()); ok = false; }

            var prov = cboProvincia.SelectedItem as Item;
            if (prov == null || prov.Id == Guid.Empty) { errorProvider1.SetError(cboProvincia, "msg.required".Traducir()); ok = false; }

            var loc = cboLocalidad.SelectedItem as Item;
            if (loc == null || loc.Id == Guid.Empty) { errorProvider1.SetError(cboLocalidad, "msg.required".Traducir()); ok = false; }

            return ok;
        }

        private void Guardar()
        {
            if (!ValidarCampos()) return;

            try
            {
                var itPais = cboPais.SelectedItem as Item;
                var itProv = cboProvincia.SelectedItem as Item;
                var itLoc = cboLocalidad.SelectedItem as Item;
                var itTipo = cboTipoEmpresa.SelectedItem as Item;
                var condicionSeleccionada = cboCondIVA.SelectedItem as Item;

                _model.RazonSocial = txtRazon.Text.Trim();
                _model.Alias = string.IsNullOrWhiteSpace(txtAlias.Text) ? null : txtAlias.Text.Trim();
                _model.CUIT = Regex.Replace(txtCUIT.Text, "[^0-9]", "");
                _model.Domicilio = txtDomicilio.Text.Trim();
                _model.IdCondicionIva = condicionSeleccionada?.Id ?? Guid.Empty;
                _model.Activo = chkActivo.Checked;
                _model.IdTipoEmpresa = itTipo != null && itTipo.Id != Guid.Empty ? itTipo.Id : (Guid?)null;
                _model.IdPais = itPais != null && itPais.Id != Guid.Empty ? itPais.Id : (Guid?)null;
                _model.IdProvincia = itProv != null && itProv.Id != Guid.Empty ? itProv.Id : (Guid?)null;
                _model.IdLocalidad = itLoc != null && itLoc.Id != Guid.Empty ? itLoc.Id : (Guid?)null;
                _model.Localidad = itLoc != null && itLoc.Id != Guid.Empty ? itLoc.Nombre : null;

                ResultadoOperacion res;
                if (_esEdicion)
                {
                    res = _clienteService.ActualizarCliente(_model); // ya implementado en tu BLL
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Cliente.Editar",
                        res.EsValido ? $"Id={_model.IdCliente}" : res.Mensaje, "Clientes", res.EsValido);
                }
                else
                {
                    res = _clienteService.CrearCliente(_model); // valida CUIT duplicado, setea Id y Activo
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Cliente.Alta",
                        res.EsValido ? $"Id={res.IdGenerado}" : res.Mensaje, "Clientes", res.EsValido);
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
                _bitacora.RegistrarAccion(SessionContext.IdUsuario, _esEdicion ? "Cliente.Editar" : "Cliente.Alta",
                    ex.Message, "Clientes", false);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarCombosGeo()
        {
            // País
            var paises = _geoService.ListarPaises() ?? new List<GeoDTO>();
            var paisItems = paises
                .Select(p => new Item { Id = p.Id, Nombre = p.Nombre })
                .ToList();

            paisItems.Insert(0, new Item { Id = Guid.Empty, Nombre = "-- Seleccione --".Traducir() });

            cboPais.DisplayMember = nameof(Item.Nombre);
            cboPais.ValueMember = nameof(Item.Id);
            cboPais.DataSource = paisItems;

            cboPais.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProvincia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocalidad.DropDownStyle = ComboBoxStyle.DropDownList;

            cboPais.SelectedIndexChanged += (s, e) => CargarProvincias();
            cboProvincia.SelectedIndexChanged += (s, e) => CargarLocalidades();

            if (cboPais.Items.Count > 0) cboPais.SelectedIndex = 0;
        }

        private void CargarCondicionIVA()
        {
            _condicionesIva = _condicionIvaService.ObtenerTodas()?.ToList() ?? new List<CondicionIva>();
            var items = _condicionesIva
                .Select(ci => new Item { Id = ci.IdCondicionIva, Nombre = ci.Nombre })
                .ToList();

            items.Insert(0, new Item { Id = Guid.Empty, Nombre = "-- Seleccione --".Traducir() });

            cboCondIVA.DisplayMember = nameof(Item.Nombre);
            cboCondIVA.ValueMember = nameof(Item.Id);
            cboCondIVA.DataSource = items;
            cboCondIVA.SelectedIndex = items.Count > 1 ? 1 : 0;
        }

        private void CargarTiposEmpresa()
        {
            _tiposEmpresa = _clienteService.ObtenerTiposEmpresa()?.ToList() ?? new List<TipoEmpresa>();

            var items = _tiposEmpresa
                .Select(te => new Item { Id = te.IdTipoEmpresa, Nombre = te.TipoEmpresaNombre })
                .ToList();

            items.Insert(0, new Item { Id = Guid.Empty, Nombre = "-- Seleccione --".Traducir() });

            cboTipoEmpresa.DisplayMember = nameof(Item.Nombre);
            cboTipoEmpresa.ValueMember = nameof(Item.Id);
            cboTipoEmpresa.DataSource = items;
            cboTipoEmpresa.SelectedIndex = 0;
        }

        private void CargarProvincias()
        {
            cboProvincia.DataSource = null; cboLocalidad.DataSource = null;

            var idPais = (cboPais.SelectedItem as Item)?.Id ?? Guid.Empty;
            var provincias = idPais != Guid.Empty ? _geoService.ListarProvinciasPorPais(idPais) : new List<GeoDTO>();

            var provinciaItems = provincias
                .Select(x => new Item { Id = x.Id, Nombre = x.Nombre })
                .ToList();

            provinciaItems.Insert(0, new Item { Id = Guid.Empty, Nombre = "-- Seleccione --".Traducir() });

            cboProvincia.DisplayMember = nameof(Item.Nombre);
            cboProvincia.ValueMember = nameof(Item.Id);
            cboProvincia.DataSource = provinciaItems;

            if (cboProvincia.Items.Count > 0) cboProvincia.SelectedIndex = 0;
        }

        private void CargarLocalidades()
        {
            cboLocalidad.DataSource = null;

            var idProv = (cboProvincia.SelectedItem as Item)?.Id ?? Guid.Empty;
            var localidades = idProv != Guid.Empty ? _geoService.ListarLocalidadesPorProvincia(idProv) : new List<GeoDTO>();

            var localidadItems = localidades
                .Select(x => new Item { Id = x.Id, Nombre = x.Nombre })
                .ToList();

            localidadItems.Insert(0, new Item { Id = Guid.Empty, Nombre = "-- Seleccione --".Traducir() });

            cboLocalidad.DisplayMember = nameof(Item.Nombre);
            cboLocalidad.ValueMember = nameof(Item.Id);
            cboLocalidad.DataSource = localidadItems;

            if (cboLocalidad.Items.Count > 0) cboLocalidad.SelectedIndex = 0;
        }

        private void SeleccionarCondicionIva(Guid idCondicion)
        {
            if (idCondicion == Guid.Empty)
                return;

            for (int i = 0; i < cboCondIVA.Items.Count; i++)
            {
                if (cboCondIVA.Items[i] is Item item && item.Id == idCondicion)
                {
                    cboCondIVA.SelectedIndex = i;
                    return;
                }
            }
        }
    }
}