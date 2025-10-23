using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    partial class PedidoForm
    {
        private IContainer components = null;
        private Label lblNumeroPedido;
        private TextBox txtNumeroPedido;
        private Label lblCliente;
        private ComboBox cboCliente;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
        private CheckBox chkTieneFechaLimite;
        private DateTimePicker dtpFechaLimite;
        private Label lblTipoPago;
        private ComboBox cboTipoPago;
        private GroupBox grpContacto;
        private Label lblNombreContacto;
        private TextBox txtNombreContacto;
        private Label lblTelefonoContacto;
        private TextBox txtTelefonoContacto;
        private Label lblEmailContacto;
        private TextBox txtEmailContacto;
        private GroupBox grpDireccion;
        private Label lblDireccionEntrega;
        private TextBox txtDireccionEntrega;
        private Label lblLocalidadEntrega;
        private TextBox txtLocalidadEntrega;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private GroupBox grpDetalles;
        private DataGridView dgvDetalles;
        private ToolStrip toolStripDetalles;
        private ToolStripButton tsbAgregarProducto;
        private ToolStripButton tsbEditarProducto;
        private ToolStripButton tsbEliminarProducto;
        private GroupBox grpFacturacion;
        private Button btnAdjuntarFactura;
        private Label lblRutaFactura;
        private Panel panelTotales;
        private Label lblTotal;
        private Label lblTotalValor;
        private Label lblTotalIVA;
        private Label lblTotalIVAValor;
        private Button btnGuardar;
        private Button btnCancelar;
        private ErrorProvider errorProvider1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblNumeroPedido = new System.Windows.Forms.Label();
            this.txtNumeroPedido = new System.Windows.Forms.TextBox();
            this.lblCliente = new System.Windows.Forms.Label();
            this.cboCliente = new System.Windows.Forms.ComboBox();
            this.lblFecha = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.chkTieneFechaLimite = new System.Windows.Forms.CheckBox();
            this.dtpFechaLimite = new System.Windows.Forms.DateTimePicker();
            this.lblTipoPago = new System.Windows.Forms.Label();
            this.cboTipoPago = new System.Windows.Forms.ComboBox();
            this.grpContacto = new System.Windows.Forms.GroupBox();
            this.lblNombreContacto = new System.Windows.Forms.Label();
            this.txtNombreContacto = new System.Windows.Forms.TextBox();
            this.lblTelefonoContacto = new System.Windows.Forms.Label();
            this.txtTelefonoContacto = new System.Windows.Forms.TextBox();
            this.lblEmailContacto = new System.Windows.Forms.Label();
            this.txtEmailContacto = new System.Windows.Forms.TextBox();
            this.grpDireccion = new System.Windows.Forms.GroupBox();
            this.lblDireccionEntrega = new System.Windows.Forms.Label();
            this.txtDireccionEntrega = new System.Windows.Forms.TextBox();
            this.lblLocalidadEntrega = new System.Windows.Forms.Label();
            this.txtLocalidadEntrega = new System.Windows.Forms.TextBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.grpDetalles = new System.Windows.Forms.GroupBox();
            this.dgvDetalles = new System.Windows.Forms.DataGridView();
            this.toolStripDetalles = new System.Windows.Forms.ToolStrip();
            this.tsbAgregarProducto = new System.Windows.Forms.ToolStripButton();
            this.tsbEditarProducto = new System.Windows.Forms.ToolStripButton();
            this.tsbEliminarProducto = new System.Windows.Forms.ToolStripButton();
            this.grpFacturacion = new System.Windows.Forms.GroupBox();
            this.btnAdjuntarFactura = new System.Windows.Forms.Button();
            this.lblRutaFactura = new System.Windows.Forms.Label();
            this.panelTotales = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalValor = new System.Windows.Forms.Label();
            this.lblTotalIVA = new System.Windows.Forms.Label();
            this.lblTotalIVAValor = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpContacto.SuspendLayout();
            this.grpDireccion.SuspendLayout();
            this.grpDetalles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).BeginInit();
            this.toolStripDetalles.SuspendLayout();
            this.grpFacturacion.SuspendLayout();
            this.panelTotales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            //
            // lblNumeroPedido
            //
            this.lblNumeroPedido.Location = new System.Drawing.Point(20, 20);
            this.lblNumeroPedido.Name = "lblNumeroPedido";
            this.lblNumeroPedido.Size = new System.Drawing.Size(120, 22);
            this.lblNumeroPedido.TabIndex = 0;
            this.lblNumeroPedido.Text = "order.number";
            this.lblNumeroPedido.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtNumeroPedido
            //
            this.txtNumeroPedido.Location = new System.Drawing.Point(140, 20);
            this.txtNumeroPedido.Name = "txtNumeroPedido";
            this.txtNumeroPedido.ReadOnly = true;
            this.txtNumeroPedido.Size = new System.Drawing.Size(150, 20);
            this.txtNumeroPedido.TabIndex = 1;
            this.txtNumeroPedido.BackColor = System.Drawing.SystemColors.Control;
            //
            // lblCliente
            //
            this.lblCliente.Location = new System.Drawing.Point(20, 54);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(120, 22);
            this.lblCliente.TabIndex = 2;
            this.lblCliente.Text = "order.client";
            this.lblCliente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cboCliente
            //
            this.cboCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCliente.FormattingEnabled = true;
            this.cboCliente.Location = new System.Drawing.Point(140, 54);
            this.cboCliente.Name = "cboCliente";
            this.cboCliente.Size = new System.Drawing.Size(360, 21);
            this.cboCliente.TabIndex = 3;
            //
            // lblFecha
            //
            this.lblFecha.Location = new System.Drawing.Point(20, 88);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(120, 22);
            this.lblFecha.TabIndex = 4;
            this.lblFecha.Text = "order.date";
            this.lblFecha.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // dtpFecha
            //
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(140, 88);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(150, 20);
            this.dtpFecha.TabIndex = 5;
            //
            // chkTieneFechaLimite
            //
            this.chkTieneFechaLimite.Location = new System.Drawing.Point(310, 88);
            this.chkTieneFechaLimite.Name = "chkTieneFechaLimite";
            this.chkTieneFechaLimite.Size = new System.Drawing.Size(120, 22);
            this.chkTieneFechaLimite.TabIndex = 6;
            this.chkTieneFechaLimite.Text = "order.has_deadline";
            this.chkTieneFechaLimite.UseVisualStyleBackColor = true;
            //
            // dtpFechaLimite
            //
            this.dtpFechaLimite.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaLimite.Location = new System.Drawing.Point(440, 88);
            this.dtpFechaLimite.Name = "dtpFechaLimite";
            this.dtpFechaLimite.Size = new System.Drawing.Size(150, 20);
            this.dtpFechaLimite.TabIndex = 7;
            this.dtpFechaLimite.Enabled = false;
            //
            // lblTipoPago
            //
            this.lblTipoPago.Location = new System.Drawing.Point(20, 122);
            this.lblTipoPago.Name = "lblTipoPago";
            this.lblTipoPago.Size = new System.Drawing.Size(120, 22);
            this.lblTipoPago.TabIndex = 8;
            this.lblTipoPago.Text = "order.payment_type";
            this.lblTipoPago.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cboTipoPago
            //
            this.cboTipoPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoPago.FormattingEnabled = true;
            this.cboTipoPago.Location = new System.Drawing.Point(140, 122);
            this.cboTipoPago.Name = "cboTipoPago";
            this.cboTipoPago.Size = new System.Drawing.Size(260, 21);
            this.cboTipoPago.TabIndex = 9;
            //
            // grpContacto
            //
            this.grpContacto.Controls.Add(this.lblNombreContacto);
            this.grpContacto.Controls.Add(this.txtNombreContacto);
            this.grpContacto.Controls.Add(this.lblTelefonoContacto);
            this.grpContacto.Controls.Add(this.txtTelefonoContacto);
            this.grpContacto.Controls.Add(this.lblEmailContacto);
            this.grpContacto.Controls.Add(this.txtEmailContacto);
            this.grpContacto.Location = new System.Drawing.Point(20, 160);
            this.grpContacto.Name = "grpContacto";
            this.grpContacto.Size = new System.Drawing.Size(580, 120);
            this.grpContacto.TabIndex = 10;
            this.grpContacto.TabStop = false;
            this.grpContacto.Text = "order.contact_info";
            //
            // lblNombreContacto
            //
            this.lblNombreContacto.Location = new System.Drawing.Point(15, 25);
            this.lblNombreContacto.Name = "lblNombreContacto";
            this.lblNombreContacto.Size = new System.Drawing.Size(100, 22);
            this.lblNombreContacto.TabIndex = 0;
            this.lblNombreContacto.Text = "order.contact_name";
            this.lblNombreContacto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtNombreContacto
            //
            this.txtNombreContacto.Location = new System.Drawing.Point(120, 25);
            this.txtNombreContacto.Name = "txtNombreContacto";
            this.txtNombreContacto.Size = new System.Drawing.Size(440, 20);
            this.txtNombreContacto.TabIndex = 1;
            this.txtNombreContacto.MaxLength = 200;
            //
            // lblTelefonoContacto
            //
            this.lblTelefonoContacto.Location = new System.Drawing.Point(15, 55);
            this.lblTelefonoContacto.Name = "lblTelefonoContacto";
            this.lblTelefonoContacto.Size = new System.Drawing.Size(100, 22);
            this.lblTelefonoContacto.TabIndex = 2;
            this.lblTelefonoContacto.Text = "order.contact_phone";
            this.lblTelefonoContacto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtTelefonoContacto
            //
            this.txtTelefonoContacto.Location = new System.Drawing.Point(120, 55);
            this.txtTelefonoContacto.Name = "txtTelefonoContacto";
            this.txtTelefonoContacto.Size = new System.Drawing.Size(200, 20);
            this.txtTelefonoContacto.TabIndex = 3;
            this.txtTelefonoContacto.MaxLength = 50;
            //
            // lblEmailContacto
            //
            this.lblEmailContacto.Location = new System.Drawing.Point(15, 85);
            this.lblEmailContacto.Name = "lblEmailContacto";
            this.lblEmailContacto.Size = new System.Drawing.Size(100, 22);
            this.lblEmailContacto.TabIndex = 4;
            this.lblEmailContacto.Text = "order.contact_email";
            this.lblEmailContacto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtEmailContacto
            //
            this.txtEmailContacto.Location = new System.Drawing.Point(120, 85);
            this.txtEmailContacto.Name = "txtEmailContacto";
            this.txtEmailContacto.Size = new System.Drawing.Size(300, 20);
            this.txtEmailContacto.TabIndex = 5;
            this.txtEmailContacto.MaxLength = 200;
            //
            // grpDireccion
            //
            this.grpDireccion.Controls.Add(this.lblDireccionEntrega);
            this.grpDireccion.Controls.Add(this.txtDireccionEntrega);
            this.grpDireccion.Controls.Add(this.lblLocalidadEntrega);
            this.grpDireccion.Controls.Add(this.txtLocalidadEntrega);
            this.grpDireccion.Location = new System.Drawing.Point(620, 160);
            this.grpDireccion.Name = "grpDireccion";
            this.grpDireccion.Size = new System.Drawing.Size(400, 120);
            this.grpDireccion.TabIndex = 11;
            this.grpDireccion.TabStop = false;
            this.grpDireccion.Text = "order.delivery_address";
            //
            // lblDireccionEntrega
            //
            this.lblDireccionEntrega.Location = new System.Drawing.Point(15, 25);
            this.lblDireccionEntrega.Name = "lblDireccionEntrega";
            this.lblDireccionEntrega.Size = new System.Drawing.Size(100, 22);
            this.lblDireccionEntrega.TabIndex = 0;
            this.lblDireccionEntrega.Text = "order.address";
            this.lblDireccionEntrega.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtDireccionEntrega
            //
            this.txtDireccionEntrega.Location = new System.Drawing.Point(120, 25);
            this.txtDireccionEntrega.Multiline = true;
            this.txtDireccionEntrega.Name = "txtDireccionEntrega";
            this.txtDireccionEntrega.Size = new System.Drawing.Size(260, 40);
            this.txtDireccionEntrega.TabIndex = 1;
            this.txtDireccionEntrega.MaxLength = 500;
            //
            // lblLocalidadEntrega
            //
            this.lblLocalidadEntrega.Location = new System.Drawing.Point(15, 75);
            this.lblLocalidadEntrega.Name = "lblLocalidadEntrega";
            this.lblLocalidadEntrega.Size = new System.Drawing.Size(100, 22);
            this.lblLocalidadEntrega.TabIndex = 2;
            this.lblLocalidadEntrega.Text = "order.locality";
            this.lblLocalidadEntrega.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtLocalidadEntrega
            //
            this.txtLocalidadEntrega.Location = new System.Drawing.Point(120, 75);
            this.txtLocalidadEntrega.Name = "txtLocalidadEntrega";
            this.txtLocalidadEntrega.Size = new System.Drawing.Size(260, 20);
            this.txtLocalidadEntrega.TabIndex = 3;
            this.txtLocalidadEntrega.MaxLength = 200;
            //
            // lblObservaciones
            //
            this.lblObservaciones.Location = new System.Drawing.Point(20, 290);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(120, 22);
            this.lblObservaciones.TabIndex = 12;
            this.lblObservaciones.Text = "order.observations";
            this.lblObservaciones.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtObservaciones
            //
            this.txtObservaciones.Location = new System.Drawing.Point(140, 290);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservaciones.Size = new System.Drawing.Size(880, 60);
            this.txtObservaciones.TabIndex = 13;
            this.txtObservaciones.MaxLength = 1000;
            //
            // grpDetalles
            //
            this.grpDetalles.Controls.Add(this.toolStripDetalles);
            this.grpDetalles.Controls.Add(this.dgvDetalles);
            this.grpDetalles.Location = new System.Drawing.Point(20, 360);
            this.grpDetalles.Name = "grpDetalles";
            this.grpDetalles.Size = new System.Drawing.Size(1000, 280);
            this.grpDetalles.TabIndex = 14;
            this.grpDetalles.TabStop = false;
            this.grpDetalles.Text = "order.products";
            //
            // toolStripDetalles
            //
            this.toolStripDetalles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAgregarProducto,
            this.tsbEditarProducto,
            this.tsbEliminarProducto});
            this.toolStripDetalles.Location = new System.Drawing.Point(3, 16);
            this.toolStripDetalles.Name = "toolStripDetalles";
            this.toolStripDetalles.Size = new System.Drawing.Size(994, 25);
            this.toolStripDetalles.TabIndex = 0;
            //
            // tsbAgregarProducto
            //
            this.tsbAgregarProducto.Text = "order.add_product";
            this.tsbAgregarProducto.Name = "tsbAgregarProducto";
            //
            // tsbEditarProducto
            //
            this.tsbEditarProducto.Text = "order.edit_product";
            this.tsbEditarProducto.Name = "tsbEditarProducto";
            //
            // tsbEliminarProducto
            //
            this.tsbEliminarProducto.Text = "order.remove_product";
            this.tsbEliminarProducto.Name = "tsbEliminarProducto";
            //
            // dgvDetalles
            //
            this.dgvDetalles.AllowUserToAddRows = false;
            this.dgvDetalles.AllowUserToDeleteRows = false;
            this.dgvDetalles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalles.Location = new System.Drawing.Point(3, 41);
            this.dgvDetalles.Name = "dgvDetalles";
            this.dgvDetalles.ReadOnly = true;
            this.dgvDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalles.Size = new System.Drawing.Size(994, 236);
            this.dgvDetalles.TabIndex = 1;
            //
            // grpFacturacion
            //
            this.grpFacturacion.Controls.Add(this.btnAdjuntarFactura);
            this.grpFacturacion.Controls.Add(this.lblRutaFactura);
            this.grpFacturacion.Location = new System.Drawing.Point(20, 650);
            this.grpFacturacion.Name = "grpFacturacion";
            this.grpFacturacion.Size = new System.Drawing.Size(600, 80);
            this.grpFacturacion.TabIndex = 15;
            this.grpFacturacion.TabStop = false;
            this.grpFacturacion.Text = "order.invoice";
            //
            // btnAdjuntarFactura
            //
            this.btnAdjuntarFactura.Location = new System.Drawing.Point(15, 25);
            this.btnAdjuntarFactura.Name = "btnAdjuntarFactura";
            this.btnAdjuntarFactura.Size = new System.Drawing.Size(150, 30);
            this.btnAdjuntarFactura.TabIndex = 0;
            this.btnAdjuntarFactura.Text = "order.attach_invoice";
            this.btnAdjuntarFactura.UseVisualStyleBackColor = true;
            //
            // lblRutaFactura
            //
            this.lblRutaFactura.Location = new System.Drawing.Point(180, 25);
            this.lblRutaFactura.Name = "lblRutaFactura";
            this.lblRutaFactura.Size = new System.Drawing.Size(400, 30);
            this.lblRutaFactura.TabIndex = 1;
            this.lblRutaFactura.Text = "-";
            this.lblRutaFactura.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // panelTotales
            //
            this.panelTotales.Controls.Add(this.lblTotal);
            this.panelTotales.Controls.Add(this.lblTotalValor);
            this.panelTotales.Controls.Add(this.lblTotalIVA);
            this.panelTotales.Controls.Add(this.lblTotalIVAValor);
            this.panelTotales.Location = new System.Drawing.Point(640, 650);
            this.panelTotales.Name = "panelTotales";
            this.panelTotales.Size = new System.Drawing.Size(380, 80);
            this.panelTotales.TabIndex = 16;
            //
            // lblTotal
            //
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(15, 15);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(180, 22);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "order.total";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblTotalValor
            //
            this.lblTotalValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalValor.Location = new System.Drawing.Point(200, 15);
            this.lblTotalValor.Name = "lblTotalValor";
            this.lblTotalValor.Size = new System.Drawing.Size(160, 22);
            this.lblTotalValor.TabIndex = 1;
            this.lblTotalValor.Text = "$ 0.00";
            this.lblTotalValor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblTotalIVA
            //
            this.lblTotalIVA.Location = new System.Drawing.Point(15, 45);
            this.lblTotalIVA.Name = "lblTotalIVA";
            this.lblTotalIVA.Size = new System.Drawing.Size(180, 22);
            this.lblTotalIVA.TabIndex = 2;
            this.lblTotalIVA.Text = "order.total_with_iva";
            this.lblTotalIVA.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblTotalIVAValor
            //
            this.lblTotalIVAValor.Location = new System.Drawing.Point(200, 45);
            this.lblTotalIVAValor.Name = "lblTotalIVAValor";
            this.lblTotalIVAValor.Size = new System.Drawing.Size(160, 22);
            this.lblTotalIVAValor.TabIndex = 3;
            this.lblTotalIVAValor.Text = "$ 0.00";
            this.lblTotalIVAValor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(810, 750);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(100, 35);
            this.btnGuardar.TabIndex = 17;
            this.btnGuardar.Text = "form.save";
            this.btnGuardar.UseVisualStyleBackColor = true;
            //
            // btnCancelar
            //
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(920, 750);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 35);
            this.btnCancelar.TabIndex = 18;
            this.btnCancelar.Text = "form.cancel";
            this.btnCancelar.UseVisualStyleBackColor = true;
            //
            // errorProvider1
            //
            this.errorProvider1.ContainerControl = this;
            //
            // PedidoForm
            //
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(1040, 800);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.panelTotales);
            this.Controls.Add(this.grpFacturacion);
            this.Controls.Add(this.grpDetalles);
            this.Controls.Add(this.txtObservaciones);
            this.Controls.Add(this.lblObservaciones);
            this.Controls.Add(this.grpDireccion);
            this.Controls.Add(this.grpContacto);
            this.Controls.Add(this.cboTipoPago);
            this.Controls.Add(this.lblTipoPago);
            this.Controls.Add(this.dtpFechaLimite);
            this.Controls.Add(this.chkTieneFechaLimite);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.cboCliente);
            this.Controls.Add(this.lblCliente);
            this.Controls.Add(this.txtNumeroPedido);
            this.Controls.Add(this.lblNumeroPedido);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PedidoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pedido";
            this.grpContacto.ResumeLayout(false);
            this.grpContacto.PerformLayout();
            this.grpDireccion.ResumeLayout(false);
            this.grpDireccion.PerformLayout();
            this.grpDetalles.ResumeLayout(false);
            this.grpDetalles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).EndInit();
            this.toolStripDetalles.ResumeLayout(false);
            this.toolStripDetalles.PerformLayout();
            this.grpFacturacion.ResumeLayout(false);
            this.panelTotales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
