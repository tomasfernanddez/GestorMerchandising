using BLL.Helpers;
using BLL.Interfaces;
using Services.BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UI.Localization;
using System.IO;

namespace UI
{
    public partial class PedidoForm : Form
    {
        private readonly IPedidoService _pedidoService;
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly IProveedorService _proveedorService;
        private readonly IBitacoraService _bitacora;
        private readonly bool _esEdicion;
        private Pedido _model;
        private List<PedidoDetalle> _detalles;

        private class Item { public Guid Id { get; set; } public string Nombre { get; set; } }

        public PedidoForm()
        {
            InitializeComponent();
        }

        public PedidoForm(IPedidoService pedidoService, IClienteService clienteService,
            IProductoService productoService, IProveedorService proveedorService,
            IBitacoraService bitacora, Pedido pedido)
        {
            _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _proveedorService = proveedorService ?? throw new ArgumentNullException(nameof(proveedorService));
            _bitacora = bitacora ?? throw new ArgumentNullException(nameof(bitacora));
            _model = pedido;
            _esEdicion = pedido != null;
            _detalles = new List<PedidoDetalle>();

            InitializeComponent();
            this.Load += PedidoForm_Load;
        }

        private void PedidoForm_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ApplyTexts();
            CargarClientes();
            CargarEstados();
            CargarTiposPago();
            CargarModelo();
            WireUp();
            ActualizarTotales();
        }

        private void ApplyTexts()
        {
            Text = _esEdicion ? "abm.common.edit".Traducir() + " - " + "abm.orders.title".Traducir()
                              : "abm.common.new".Traducir() + " - " + "abm.orders.title".Traducir();

            grpGeneral.Text = "Datos Generales";
            grpContacto.Text = "Contacto y Entrega";
            grpProductos.Text = "Productos del Pedido";

            lblNumero.Text = "order.number".Traducir();
            lblCliente.Text = "order.client".Traducir();
            lblFecha.Text = "order.date".Traducir();
            lblFechaLimite.Text = "order.deadline".Traducir();
            lblEstado.Text = "order.status".Traducir();
            lblTipoPago.Text = "order.payment.type".Traducir();
            lblOC.Text = "order.oc".Traducir();
            lblContactoNombre.Text = "order.contact.name".Traducir();
            lblContactoEmail.Text = "order.contact.email".Traducir();
            lblContactoTelefono.Text = "order.contact.phone".Traducir();
            lblDireccionEntrega.Text = "order.delivery.address".Traducir();
            lblRemito.Text = "order.remito".Traducir();
            lblObservaciones.Text = "order.observations".Traducir();
            chkFacturado.Text = "order.invoiced".Traducir();

            btnAgregarProducto.Text = "Agregar Producto";
            btnEditarProducto.Text = "Editar Producto";
            btnEliminarProducto.Text = "Eliminar Producto";
            btnAdjuntarFactura.Text = "Adjuntar Factura PDF";
            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
        }

        private void ConfigurarGrid()
        {
            dgvDetalles.AutoGenerateColumns = false;
            dgvDetalles.AllowUserToAddRows = false;
            dgvDetalles.AllowUserToDeleteRows = false;
            dgvDetalles.ReadOnly = true;
            dgvDetalles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalles.MultiSelect = false;

            dgvDetalles.Columns.Clear();

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Producto",
                HeaderText = "order.detail.product".Traducir(),
                DataPropertyName = "NombreProducto",
                Width = 200
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cantidad",
                HeaderText = "order.detail.quantity".Traducir(),
                DataPropertyName = "Cantidad",
                Width = 80
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PrecioUnitario",
                HeaderText = "order.detail.price".Traducir(),
                DataPropertyName = "PrecioUnitario",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Subtotal",
                HeaderText = "order.detail.subtotal".Traducir(),
                DataPropertyName = "SubtotalCalculado",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Estado",
                HeaderText = "order.detail.status".Traducir(),
                DataPropertyName = "NombreEstado",
                Width = 120
            });
        }

        private void CargarClientes()
        {
            var clientes = _clienteService.ObtenerClientesActivos()?.ToList() ?? new List<Cliente>();
            cboCliente.DisplayMember = "Nombre";
            cboCliente.ValueMember = "Id";
            cboCliente.DataSource = clientes.Select(c => new Item
            {
                Id = c.IdCliente,
                Nombre = c.RazonSocial
            }).ToList();

            cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarEstados()
        {
            var estados = _pedidoService.ObtenerEstadosPedido()?.ToList() ?? new List<EstadoPedido>();
            cboEstado.DisplayMember = "Nombre";
            cboEstado.ValueMember = "Id";
            cboEstado.DataSource = estados.Select(e => new Item
            {
                Id = e.IdEstadoPedido,
                Nombre = e.NombreEstadoPedido
            }).ToList();

            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarTiposPago()
        {
            var tipos = _pedidoService.ObtenerTiposPago()?.ToList() ?? new List<TipoPago>();
            cboTipoPago.DisplayMember = "Nombre";
            cboTipoPago.ValueMember = "Id";
            cboTipoPago.DataSource = tipos.Select(t => new Item
            {
                Id = t.IdTipoPago,
                Nombre = t.NombreTipoPago
            }).ToList();

            cboTipoPago.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarModelo()
        {
            if (_model == null)
            {
                _model = new Pedido
                {
                    Fecha = DateTime.Now,
                    NumeroPedido = "[Se generará automáticamente]"
                };
                txtNumero.Text = _model.NumeroPedido;
                dtpFecha.Value = _model.Fecha;
                chkTieneFechaLimite.Checked = false;
                dtpFechaLimite.Enabled = false;
                chkFacturado.Checked = false;
                return;
            }

            // Cargar pedido existente
            txtNumero.Text = _model.NumeroPedido;
            dtpFecha.Value = _model.Fecha;

            if (_model.TieneFechaLimite && _model.FechaLimite.HasValue)
            {
                chkTieneFechaLimite.Checked = true;
                dtpFechaLimite.Value = _model.FechaLimite.Value;
                dtpFechaLimite.Enabled = true;
            }

            txtOC.Text = _model.Cliente_OC;
            txtContactoNombre.Text = _model.Cliente_PersonaNombre;
            txtContactoEmail.Text = _model.Cliente_PersonaEmail;
            txtContactoTelefono.Text = _model.Cliente_PersonaTelefono;
            txtDireccionEntrega.Text = _model.Cliente_DireccionEntrega;
            txtRemito.Text = _model.NumeroRemito;
            txtObservaciones.Text = _model.Observaciones;
            chkFacturado.Checked = _model.Facturado;
            txtRutaFactura.Text = _model.RutaFacturaPDF;

            // Seleccionar combos
            SeleccionarCombo(cboCliente, _model.IdCliente);
            if (_model.IdEstadoPedido.HasValue)
                SeleccionarCombo(cboEstado, _model.IdEstadoPedido.Value);
            if (_model.IdTipoPago.HasValue)
                SeleccionarCombo(cboTipoPago, _model.IdTipoPago.Value);

            // Cargar detalles
            if (_model.Detalles != null && _model.Detalles.Any())
            {
                _detalles = _model.Detalles.ToList();
                ActualizarGridDetalles();
            }
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
            chkTieneFechaLimite.CheckedChanged += (s, e) => dtpFechaLimite.Enabled = chkTieneFechaLimite.Checked;

            btnAgregarProducto.Click += (s, e) => AgregarProducto();
            btnEditarProducto.Click += (s, e) => EditarProducto();
            btnEliminarProducto.Click += (s, e) => EliminarProducto();
            btnAdjuntarFactura.Click += (s, e) => AdjuntarFactura();

            dgvDetalles.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarProducto(); };

            btnGuardar.Click += (s, e) => Guardar();
            btnCancelar.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
        }

        private void AgregarProducto()
        {
            var form = new PedidoDetalleForm(_productoService, _proveedorService, _pedidoService, detalle: null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                var nuevoDetalle = form.Detalle;
                if (nuevoDetalle != null)
                {
                    _detalles.Add(nuevoDetalle);
                    ActualizarGridDetalles();
                    ActualizarTotales();
                }
            }
        }

        private void EditarProducto()
        {
            if (dgvDetalles.CurrentRow == null) return;
            var index = dgvDetalles.CurrentRow.Index;
            if (index < 0 || index >= _detalles.Count) return;

            var detalle = _detalles[index];
            var form = new PedidoDetalleForm(_productoService, _proveedorService, _pedidoService, detalle);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _detalles[index] = form.Detalle;
                ActualizarGridDetalles();
                ActualizarTotales();
            }
        }

        private void EliminarProducto()
        {
            if (dgvDetalles.CurrentRow == null) return;
            var index = dgvDetalles.CurrentRow.Index;
            if (index < 0 || index >= _detalles.Count) return;

            var resultado = MessageBox.Show("¿Confirma eliminar este producto del pedido?", Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                _detalles.RemoveAt(index);
                ActualizarGridDetalles();
                ActualizarTotales();
            }
        }

        private void AdjuntarFactura()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "PDF Files|*.pdf";
                dialog.Title = "Seleccionar Factura PDF";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtRutaFactura.Text = dialog.FileName;
                    chkFacturado.Checked = true;
                }
            }
        }

        private void ActualizarGridDetalles()
        {
            var rows = _detalles.Select(d => new
            {
                NombreProducto = d.Producto?.NombreProducto ?? "[Producto]",
                d.Cantidad,
                d.PrecioUnitario,
                SubtotalCalculado = d.Cantidad * d.PrecioUnitario,
                NombreEstado = d.EstadoProducto?.NombreEstadoProducto ?? "-"
            }).ToList();

            dgvDetalles.DataSource = rows;
        }

        private void ActualizarTotales()
        {
            decimal total = _detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            decimal totalConIVA = total * 1.21m;

            lblTotalValor.Text = total.ToString("C2");
            lblTotalIVAValor.Text = totalConIVA.ToString("C2");
        }

        private bool ValidarCampos()
        {
            errorProvider1.Clear();
            bool ok = true;

            if (cboCliente.SelectedItem == null)
            {
                errorProvider1.SetError(cboCliente, "msg.required".Traducir());
                ok = false;
            }

            if (!_detalles.Any())
            {
                MessageBox.Show("Debe agregar al menos un producto al pedido", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ok = false;
            }

            return ok;
        }

        private void Guardar()
        {
            if (!ValidarCampos()) return;

            try
            {
                var clienteSeleccionado = cboCliente.SelectedItem as Item;
                var estadoSeleccionado = cboEstado.SelectedItem as Item;
                var tipoPagoSeleccionado = cboTipoPago.SelectedItem as Item;

                _model.IdCliente = clienteSeleccionado.Id;
                _model.Fecha = dtpFecha.Value;
                _model.TieneFechaLimite = chkTieneFechaLimite.Checked;
                _model.FechaLimite = chkTieneFechaLimite.Checked ? (DateTime?)dtpFechaLimite.Value : null;
                _model.IdEstadoPedido = estadoSeleccionado?.Id;
                _model.IdTipoPago = tipoPagoSeleccionado?.Id;
                _model.Cliente_OC = txtOC.Text?.Trim();
                _model.Cliente_PersonaNombre = txtContactoNombre.Text?.Trim();
                _model.Cliente_PersonaEmail = txtContactoEmail.Text?.Trim();
                _model.Cliente_PersonaTelefono = txtContactoTelefono.Text?.Trim();
                _model.Cliente_DireccionEntrega = txtDireccionEntrega.Text?.Trim();
                _model.NumeroRemito = txtRemito.Text?.Trim();
                _model.Observaciones = txtObservaciones.Text?.Trim();
                _model.Facturado = chkFacturado.Checked;
                _model.RutaFacturaPDF = txtRutaFactura.Text?.Trim();
                _model.Detalles = _detalles;

                ResultadoOperacion res;
                if (_esEdicion)
                {
                    res = _pedidoService.ActualizarPedido(_model);
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Pedido.Editar",
                        res.EsValido ? $"Número={_model.NumeroPedido}, Id={_model.IdPedido}" : res.Mensaje,
                        "Pedidos", res.EsValido);
                }
                else
                {
                    res = _pedidoService.CrearPedido(_model);
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Pedido.Alta",
                        res.EsValido ? $"Número={_model.NumeroPedido}, Id={res.IdGenerado}" : res.Mensaje,
                        "Pedidos", res.EsValido);
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
                _bitacora.RegistrarAccion(SessionContext.IdUsuario, _esEdicion ? "Pedido.Editar" : "Pedido.Alta",
                    ex.Message, "Pedidos", false);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
