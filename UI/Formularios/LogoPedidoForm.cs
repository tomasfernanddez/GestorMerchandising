using BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UI.Localization;

namespace UI
{
    public partial class LogoPedidoForm : Form
    {
        private readonly IProveedorService _proveedorService;
        private readonly bool _esEdicion;
        private LogosPedido _logo;

        private class Item { public Guid Id { get; set; } public string Nombre { get; set; } }

        public LogosPedido Logo => _logo;

        public LogoPedidoForm()
        {
            InitializeComponent();
        }

        public LogoPedidoForm(IProveedorService proveedorService, LogosPedido logo)
        {
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _logo = logo;
            _esEdicion = logo != null;

            InitializeComponent();
            this.Load += LogoPedidoForm_Load;
        }

        private void LogoPedidoForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarTecnicas();
            CargarUbicaciones();
            CargarProveedoresPersonalizacion();
            CargarModelo();
            WireUp();
        }

        private void ApplyTexts()
        {
            Text = _esEdicion ? "Editar Logo" : "Agregar Logo";

            lblTecnica.Text = "order.logo.technique".Traducir();
            lblUbicacion.Text = "order.logo.location".Traducir();
            lblProveedor.Text = "order.logo.provider".Traducir();
            lblCosto.Text = "order.logo.cost".Traducir();
            lblDescripcion.Text = "order.logo.description".Traducir();

            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
        }

        private void CargarTecnicas()
        {
            // Esto debe venir de un catálogo. Por ahora hardcodeamos algunas comunes
            var tecnicas = new List<Item>
            {
                new Item { Id = Guid.NewGuid(), Nombre = "Serigrafía" },
                new Item { Id = Guid.NewGuid(), Nombre = "Bordado" },
                new Item { Id = Guid.NewGuid(), Nombre = "Sublimación" },
                new Item { Id = Guid.NewGuid(), Nombre = "Grabado Láser" },
                new Item { Id = Guid.NewGuid(), Nombre = "Estampado" },
                new Item { Id = Guid.NewGuid(), Nombre = "Transfer" },
                new Item { Id = Guid.NewGuid(), Nombre = "Tampografía" }
            };

            cboTecnica.DisplayMember = "Nombre";
            cboTecnica.ValueMember = "Id";
            cboTecnica.DataSource = tecnicas;
            cboTecnica.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarUbicaciones()
        {
            // Esto debe venir de un catálogo. Por ahora hardcodeamos ubicaciones comunes
            var ubicaciones = new List<Item>
            {
                new Item { Id = Guid.NewGuid(), Nombre = "Frente" },
                new Item { Id = Guid.NewGuid(), Nombre = "Espalda" },
                new Item { Id = Guid.NewGuid(), Nombre = "Manga Izquierda" },
                new Item { Id = Guid.NewGuid(), Nombre = "Manga Derecha" },
                new Item { Id = Guid.NewGuid(), Nombre = "Bolsillo" },
                new Item { Id = Guid.NewGuid(), Nombre = "Lateral" },
                new Item { Id = Guid.NewGuid(), Nombre = "Tapa" },
                new Item { Id = Guid.NewGuid(), Nombre = "Base" }
            };

            cboUbicacion.DisplayMember = "Nombre";
            cboUbicacion.ValueMember = "Id";
            cboUbicacion.DataSource = ubicaciones;
            cboUbicacion.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarProveedoresPersonalizacion()
        {
            var proveedores = _proveedorService.ObtenerProveedoresActivos()?.ToList() ?? new List<Proveedor>();

            // Filtrar proveedores que tienen el tipo "Personalización"
            var proveedoresPersonalizacion = proveedores.Where(p =>
                p.TiposProveedor != null &&
                p.TiposProveedor.Any(tp => tp.NombreTipoProveedor.Contains("Personalización", StringComparison.OrdinalIgnoreCase) ||
                                          tp.NombreTipoProveedor.Contains("Personalizacion", StringComparison.OrdinalIgnoreCase))
            ).ToList();

            cboProveedor.DisplayMember = "Nombre";
            cboProveedor.ValueMember = "Id";
            cboProveedor.DataSource = proveedoresPersonalizacion.Select(p => new Item
            {
                Id = p.IdProveedor,
                Nombre = p.RazonSocial
            }).ToList();

            cboProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarModelo()
        {
            if (_logo == null)
            {
                _logo = new LogosPedido
                {
                    IdLogoPedido = Guid.NewGuid(),
                    CostoPersonalizacion = 0
                };
                nudCosto.Value = 0;
                return;
            }

            // Cargar logo existente
            if (_logo.IdTecnicaPersonalizacion.HasValue)
                SeleccionarCombo(cboTecnica, _logo.IdTecnicaPersonalizacion.Value);

            if (_logo.IdUbicacionLogo.HasValue)
                SeleccionarCombo(cboUbicacion, _logo.IdUbicacionLogo.Value);

            if (_logo.IdProveedorPersonalizacion.HasValue)
                SeleccionarCombo(cboProveedor, _logo.IdProveedorPersonalizacion.Value);

            nudCosto.Value = _logo.CostoPersonalizacion;
            txtDescripcion.Text = _logo.Descripcion;
        }

        private void SeleccionarCombo(ComboBox combo, Guid id)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (((Item)combo.Items[i]).Id == id)
                {
                    combo.SelectedIndex = i;
                    break;
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

            if (cboTecnica.SelectedItem == null)
            {
                errorProvider1.SetError(cboTecnica, "msg.required".Traducir());
                ok = false;
            }

            if (cboUbicacion.SelectedItem == null)
            {
                errorProvider1.SetError(cboUbicacion, "msg.required".Traducir());
                ok = false;
            }

            if (nudCosto.Value < 0)
            {
                errorProvider1.SetError(nudCosto, "El costo no puede ser negativo");
                ok = false;
            }

            return ok;
        }

        private void Guardar()
        {
            if (!ValidarCampos()) return;

            try
            {
                var tecnicaSeleccionada = cboTecnica.SelectedItem as Item;
                var ubicacionSeleccionada = cboUbicacion.SelectedItem as Item;
                var proveedorSeleccionado = cboProveedor.SelectedItem as Item;

                _logo.IdTecnicaPersonalizacion = tecnicaSeleccionada?.Id;
                _logo.IdUbicacionLogo = ubicacionSeleccionada?.Id;
                _logo.IdProveedorPersonalizacion = proveedorSeleccionado?.Id;
                _logo.CostoPersonalizacion = nudCosto.Value;
                _logo.Descripcion = txtDescripcion.Text?.Trim();

                // Cargar propiedades de navegación para mostrar en el grid
                _logo.TecnicaPersonalizacion = new TecnicaPersonalizacion
                {
                    IdTecnicaPersonalizacion = tecnicaSeleccionada.Id,
                    NombreTecnicaPersonalizacion = tecnicaSeleccionada.Nombre
                };

                _logo.UbicacionLogo = new UbicacionLogo
                {
                    IdUbicacionLogo = ubicacionSeleccionada.Id,
                    NombreUbicacionLogo = ubicacionSeleccionada.Nombre
                };

                if (proveedorSeleccionado != null)
                {
                    _logo.ProveedorPersonalizacion = _proveedorService.ObtenerProveedorPorId(proveedorSeleccionado.Id);
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
