using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DomainModel;
using DomainModel.Entidades;
using UI.Localization;
using UI.ViewModels;

namespace UI
{
    public partial class PedidoLogoForm : Form
    {
        private readonly List<TecnicaPersonalizacion> _tecnicas;
        private readonly List<UbicacionLogo> _ubicaciones;
        private readonly List<Proveedor> _proveedores;
        private readonly PedidoLogoViewModel _logoOriginal;

        public PedidoLogoViewModel LogoResult { get; private set; }

        public PedidoLogoForm(
            IEnumerable<TecnicaPersonalizacion> tecnicas,
            IEnumerable<UbicacionLogo> ubicaciones,
            IEnumerable<Proveedor> proveedores,
            PedidoLogoViewModel logo = null)
        {
            _tecnicas = tecnicas?.OrderBy(t => t.NombreTecnicaPersonalizacion).ToList() ?? new List<TecnicaPersonalizacion>();
            _ubicaciones = ubicaciones?.OrderBy(u => u.NombreUbicacionLogo).ToList() ?? new List<UbicacionLogo>();
            _proveedores = proveedores?.OrderBy(p => p.RazonSocial).ToList() ?? new List<Proveedor>();
            _logoOriginal = logo;

            InitializeComponent();
        }

        private void PedidoLogoForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarCombos();
            if (_logoOriginal != null)
            {
                cmbTecnica.SelectedValue = _logoOriginal.IdTecnica ?? Guid.Empty;
                cmbUbicacion.SelectedValue = _logoOriginal.IdUbicacion ?? Guid.Empty;
                cmbProveedor.SelectedValue = _logoOriginal.IdProveedor ?? Guid.Empty;
                nudCantidad.Value = Math.Max(1, _logoOriginal.Cantidad);
                nudCosto.Value = _logoOriginal.Costo >= nudCosto.Minimum && _logoOriginal.Costo <= nudCosto.Maximum
                    ? _logoOriginal.Costo
                    : 0;
                txtDescripcion.Text = _logoOriginal.Descripcion ?? string.Empty;
            }
        }

        private void ApplyTexts()
        {
            Text = "order.logo.title".Traducir();
            lblTecnica.Text = "order.logo.technique".Traducir();
            lblUbicacion.Text = "order.logo.location".Traducir();
            lblProveedor.Text = "order.logo.provider".Traducir();
            lblCantidad.Text = "order.logo.quantity".Traducir();
            lblCosto.Text = "order.logo.cost".Traducir();
            lblDescripcion.Text = "order.logo.notes".Traducir();
            btnAceptar.Text = "form.accept".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
        }

        private void CargarCombos()
        {
            cmbTecnica.DisplayMember = nameof(TecnicaPersonalizacion.NombreTecnicaPersonalizacion);
            cmbTecnica.ValueMember = nameof(TecnicaPersonalizacion.IdTecnicaPersonalizacion);
            var tecnicas = new List<TecnicaPersonalizacion>
            {
                new TecnicaPersonalizacion { IdTecnicaPersonalizacion = Guid.Empty, NombreTecnicaPersonalizacion = "form.select.optional".Traducir() }
            };
            tecnicas.AddRange(_tecnicas);
            cmbTecnica.DataSource = tecnicas;

            cmbUbicacion.DisplayMember = nameof(UbicacionLogo.NombreUbicacionLogo);
            cmbUbicacion.ValueMember = nameof(UbicacionLogo.IdUbicacionLogo);
            var ubicaciones = new List<UbicacionLogo>
            {
                new UbicacionLogo { IdUbicacionLogo = Guid.Empty, NombreUbicacionLogo = "form.select.optional".Traducir() }
            };
            ubicaciones.AddRange(_ubicaciones);
            cmbUbicacion.DataSource = ubicaciones;

            cmbProveedor.DisplayMember = nameof(Proveedor.RazonSocial);
            cmbProveedor.ValueMember = nameof(Proveedor.IdProveedor);
            var proveedores = new List<Proveedor>
            {
                new Proveedor { IdProveedor = Guid.Empty, RazonSocial = "form.select.optional".Traducir() }
            };
            proveedores.AddRange(_proveedores);
            cmbProveedor.DataSource = proveedores;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            LogoResult = new PedidoLogoViewModel
            {
                IdLogoPedido = _logoOriginal?.IdLogoPedido ?? Guid.NewGuid(),
                IdTecnica = ObtenerGuidSeleccionado(cmbTecnica),
                Tecnica = (cmbTecnica.SelectedItem as TecnicaPersonalizacion)?.NombreTecnicaPersonalizacion,
                IdUbicacion = ObtenerGuidSeleccionado(cmbUbicacion),
                Ubicacion = (cmbUbicacion.SelectedItem as UbicacionLogo)?.NombreUbicacionLogo,
                IdProveedor = ObtenerGuidSeleccionado(cmbProveedor),
                Proveedor = (cmbProveedor.SelectedItem as Proveedor)?.RazonSocial,
                Cantidad = (int)nudCantidad.Value,
                Costo = nudCosto.Value,
                Descripcion = txtDescripcion.Text?.Trim()
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private Guid? ObtenerGuidSeleccionado(ComboBox combo)
        {
            if (combo.SelectedValue is Guid guid && guid != Guid.Empty)
            {
                return guid;
            }

            return null;
        }
    }
}