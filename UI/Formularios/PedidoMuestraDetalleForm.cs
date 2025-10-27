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
    public partial class PedidoMuestraDetalleForm : Form
    {
        private readonly List<Producto> _productos;
        private readonly List<EstadoMuestra> _estados;
        private readonly PedidoMuestraDetalleViewModel _detalleOriginal;

        public PedidoMuestraDetalleViewModel DetalleResult { get; private set; }

        public PedidoMuestraDetalleForm(
            IEnumerable<Producto> productos,
            IEnumerable<EstadoMuestra> estados,
            PedidoMuestraDetalleViewModel detalle = null)
        {
            _productos = productos?.OrderBy(p => p.NombreProducto).ToList() ?? new List<Producto>();
            _estados = estados?.OrderBy(e => e.NombreEstadoMuestra).ToList() ?? new List<EstadoMuestra>();
            _detalleOriginal = detalle;

            InitializeComponent();
        }

        private void PedidoMuestraDetalleForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarCombos();
            CargarDetalle();
        }

        private void ApplyTexts()
        {
            Text = _detalleOriginal == null
                ? "sampleOrder.detail.new".Traducir()
                : "sampleOrder.detail.edit".Traducir();

            lblProducto.Text = "sampleOrder.detail.product".Traducir();
            lblEstado.Text = "sampleOrder.detail.state".Traducir();
            lblPrecio.Text = "sampleOrder.detail.price".Traducir();
            lblDevolucion.Text = "sampleOrder.detail.returnDate".Traducir();
            lblCantidad.Text = "sampleOrder.detail.quantity".Traducir();
            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
        }

        private void CargarCombos()
        {
            cmbProducto.DisplayMember = nameof(Producto.NombreProducto);
            cmbProducto.ValueMember = nameof(Producto.IdProducto);
            cmbProducto.DataSource = _productos;

            cmbEstado.DisplayMember = nameof(EstadoMuestra.NombreEstadoMuestra);
            cmbEstado.ValueMember = nameof(EstadoMuestra.IdEstadoMuestra);
            cmbEstado.DataSource = _estados;
        }

        private void CargarDetalle()
        {
            nudCantidad.Value = 1;
            nudCantidad.Enabled = false;

            if (_detalleOriginal == null)
            {
                return;
            }

            if (_detalleOriginal.IdProducto.HasValue)
            {
                cmbProducto.SelectedValue = _detalleOriginal.IdProducto.Value;
            }

            if (_detalleOriginal.IdEstadoMuestra.HasValue)
            {
                cmbEstado.SelectedValue = _detalleOriginal.IdEstadoMuestra.Value;
            }

            nudPrecio.Value = _detalleOriginal.PrecioUnitario <= 0
                ? 0
                : _detalleOriginal.PrecioUnitario;

            if (_detalleOriginal.FechaDevolucion.HasValue)
            {
                dtpDevolucion.Checked = true;
                dtpDevolucion.Value = _detalleOriginal.FechaDevolucion.Value;
            }
            else
            {
                dtpDevolucion.Checked = false;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem == null)
            {
                MessageBox.Show("sampleOrder.detail.product.required".Traducir(), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nudPrecio.Value < 0)
            {
                MessageBox.Show("sampleOrder.detail.price.required".Traducir(), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var producto = (Producto)cmbProducto.SelectedItem;
            var estado = cmbEstado.SelectedItem as EstadoMuestra;

            var idDetalle = _detalleOriginal?.IdDetalleMuestra ?? Guid.Empty;

            var fechaDevolucion = dtpDevolucion.Checked ? dtpDevolucion.Value : (DateTime?)null;
            if (estado != null && string.Equals(estado.NombreEstadoMuestra, "Devuelto", StringComparison.OrdinalIgnoreCase) && !fechaDevolucion.HasValue)
            {
                fechaDevolucion = DateTime.UtcNow;
            }

            DetalleResult = new PedidoMuestraDetalleViewModel
            {
                IdDetalleMuestra = idDetalle,
                IdProducto = producto?.IdProducto,
                NombreProducto = producto?.NombreProducto,
                Cantidad = 1,
                PrecioUnitario = nudPrecio.Value,
                Subtotal = nudPrecio.Value,
                IdEstadoMuestra = estado?.IdEstadoMuestra,
                EstadoMuestra = estado?.NombreEstadoMuestra,
                FechaDevolucion = fechaDevolucion
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}