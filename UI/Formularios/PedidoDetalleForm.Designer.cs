using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    partial class PedidoDetalleForm
    {
        private IContainer components = null;
        private Label lblProducto;
        private ComboBox cboProducto;
        private Label lblCategoria;
        private ComboBox cboCategoria;
        private Label lblProveedor;
        private ComboBox cboProveedor;
        private Label lblCantidad;
        private NumericUpDown nudCantidad;
        private Label lblPrecio;
        private NumericUpDown nudPrecio;
        private CheckBox chkTieneFechaLimite;
        private DateTimePicker dtpFechaLimiteProducto;
        private Label lblEstadoProducto;
        private ComboBox cboEstadoProducto;
        private GroupBox grpLogos;
        private DataGridView dgvLogos;
        private Button btnAgregarLogo;
        private Button btnEditarLogo;
        private Button btnEliminarLogo;
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
            this.lblProducto = new System.Windows.Forms.Label();
            this.cboProducto = new System.Windows.Forms.ComboBox();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.cboCategoria = new System.Windows.Forms.ComboBox();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.cboProveedor = new System.Windows.Forms.ComboBox();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.nudCantidad = new System.Windows.Forms.NumericUpDown();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.nudPrecio = new System.Windows.Forms.NumericUpDown();
            this.chkTieneFechaLimite = new System.Windows.Forms.CheckBox();
            this.dtpFechaLimiteProducto = new System.Windows.Forms.DateTimePicker();
            this.lblEstadoProducto = new System.Windows.Forms.Label();
            this.cboEstadoProducto = new System.Windows.Forms.ComboBox();
            this.grpLogos = new System.Windows.Forms.GroupBox();
            this.dgvLogos = new System.Windows.Forms.DataGridView();
            this.btnAgregarLogo = new System.Windows.Forms.Button();
            this.btnEditarLogo = new System.Windows.Forms.Button();
            this.btnEliminarLogo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecio)).BeginInit();
            this.grpLogos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            //
            // lblProducto
            //
            this.lblProducto.Location = new System.Drawing.Point(20, 20);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(120, 22);
            this.lblProducto.TabIndex = 0;
            this.lblProducto.Text = "order.detail.product";
            this.lblProducto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cboProducto
            //
            this.cboProducto.FormattingEnabled = true;
            this.cboProducto.Location = new System.Drawing.Point(140, 20);
            this.cboProducto.Name = "cboProducto";
            this.cboProducto.Size = new System.Drawing.Size(460, 21);
            this.cboProducto.TabIndex = 1;
            //
            // lblCategoria
            //
            this.lblCategoria.Location = new System.Drawing.Point(20, 54);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(120, 22);
            this.lblCategoria.TabIndex = 2;
            this.lblCategoria.Text = "product.category";
            this.lblCategoria.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cboCategoria
            //
            this.cboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoria.FormattingEnabled = true;
            this.cboCategoria.Location = new System.Drawing.Point(140, 54);
            this.cboCategoria.Name = "cboCategoria";
            this.cboCategoria.Size = new System.Drawing.Size(260, 21);
            this.cboCategoria.TabIndex = 3;
            //
            // lblProveedor
            //
            this.lblProveedor.Location = new System.Drawing.Point(20, 88);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new System.Drawing.Size(120, 22);
            this.lblProveedor.TabIndex = 4;
            this.lblProveedor.Text = "product.provider";
            this.lblProveedor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cboProveedor
            //
            this.cboProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProveedor.FormattingEnabled = true;
            this.cboProveedor.Location = new System.Drawing.Point(140, 88);
            this.cboProveedor.Name = "cboProveedor";
            this.cboProveedor.Size = new System.Drawing.Size(360, 21);
            this.cboProveedor.TabIndex = 5;
            //
            // lblCantidad
            //
            this.lblCantidad.Location = new System.Drawing.Point(20, 122);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(120, 22);
            this.lblCantidad.TabIndex = 6;
            this.lblCantidad.Text = "order.detail.quantity";
            this.lblCantidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudCantidad
            //
            this.nudCantidad.Location = new System.Drawing.Point(140, 122);
            this.nudCantidad.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.nudCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudCantidad.Name = "nudCantidad";
            this.nudCantidad.Size = new System.Drawing.Size(120, 20);
            this.nudCantidad.TabIndex = 7;
            this.nudCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudCantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // lblPrecio
            //
            this.lblPrecio.Location = new System.Drawing.Point(280, 122);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(100, 22);
            this.lblPrecio.TabIndex = 8;
            this.lblPrecio.Text = "order.detail.price";
            this.lblPrecio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudPrecio
            //
            this.nudPrecio.DecimalPlaces = 2;
            this.nudPrecio.Location = new System.Drawing.Point(380, 122);
            this.nudPrecio.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.nudPrecio.Name = "nudPrecio";
            this.nudPrecio.Size = new System.Drawing.Size(120, 20);
            this.nudPrecio.TabIndex = 9;
            this.nudPrecio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // chkTieneFechaLimite
            //
            this.chkTieneFechaLimite.Location = new System.Drawing.Point(20, 156);
            this.chkTieneFechaLimite.Name = "chkTieneFechaLimite";
            this.chkTieneFechaLimite.Size = new System.Drawing.Size(180, 22);
            this.chkTieneFechaLimite.TabIndex = 10;
            this.chkTieneFechaLimite.Text = "order.detail.has_deadline";
            this.chkTieneFechaLimite.UseVisualStyleBackColor = true;
            //
            // dtpFechaLimiteProducto
            //
            this.dtpFechaLimiteProducto.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaLimiteProducto.Location = new System.Drawing.Point(200, 156);
            this.dtpFechaLimiteProducto.Name = "dtpFechaLimiteProducto";
            this.dtpFechaLimiteProducto.Size = new System.Drawing.Size(150, 20);
            this.dtpFechaLimiteProducto.TabIndex = 11;
            this.dtpFechaLimiteProducto.Enabled = false;
            //
            // lblEstadoProducto
            //
            this.lblEstadoProducto.Location = new System.Drawing.Point(20, 190);
            this.lblEstadoProducto.Name = "lblEstadoProducto";
            this.lblEstadoProducto.Size = new System.Drawing.Size(120, 22);
            this.lblEstadoProducto.TabIndex = 12;
            this.lblEstadoProducto.Text = "order.detail.status";
            this.lblEstadoProducto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cboEstadoProducto
            //
            this.cboEstadoProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEstadoProducto.FormattingEnabled = true;
            this.cboEstadoProducto.Location = new System.Drawing.Point(140, 190);
            this.cboEstadoProducto.Name = "cboEstadoProducto";
            this.cboEstadoProducto.Size = new System.Drawing.Size(260, 21);
            this.cboEstadoProducto.TabIndex = 13;
            //
            // grpLogos
            //
            this.grpLogos.Controls.Add(this.dgvLogos);
            this.grpLogos.Controls.Add(this.btnAgregarLogo);
            this.grpLogos.Controls.Add(this.btnEditarLogo);
            this.grpLogos.Controls.Add(this.btnEliminarLogo);
            this.grpLogos.Location = new System.Drawing.Point(20, 230);
            this.grpLogos.Name = "grpLogos";
            this.grpLogos.Size = new System.Drawing.Size(700, 280);
            this.grpLogos.TabIndex = 14;
            this.grpLogos.TabStop = false;
            this.grpLogos.Text = "order.logos";
            //
            // dgvLogos
            //
            this.dgvLogos.AllowUserToAddRows = false;
            this.dgvLogos.AllowUserToDeleteRows = false;
            this.dgvLogos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogos.Location = new System.Drawing.Point(15, 25);
            this.dgvLogos.Name = "dgvLogos";
            this.dgvLogos.ReadOnly = true;
            this.dgvLogos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogos.Size = new System.Drawing.Size(670, 200);
            this.dgvLogos.TabIndex = 0;
            //
            // btnAgregarLogo
            //
            this.btnAgregarLogo.Location = new System.Drawing.Point(15, 235);
            this.btnAgregarLogo.Name = "btnAgregarLogo";
            this.btnAgregarLogo.Size = new System.Drawing.Size(100, 30);
            this.btnAgregarLogo.TabIndex = 1;
            this.btnAgregarLogo.Text = "order.add_logo";
            this.btnAgregarLogo.UseVisualStyleBackColor = true;
            //
            // btnEditarLogo
            //
            this.btnEditarLogo.Location = new System.Drawing.Point(125, 235);
            this.btnEditarLogo.Name = "btnEditarLogo";
            this.btnEditarLogo.Size = new System.Drawing.Size(100, 30);
            this.btnEditarLogo.TabIndex = 2;
            this.btnEditarLogo.Text = "order.edit_logo";
            this.btnEditarLogo.UseVisualStyleBackColor = true;
            //
            // btnEliminarLogo
            //
            this.btnEliminarLogo.Location = new System.Drawing.Point(235, 235);
            this.btnEliminarLogo.Name = "btnEliminarLogo";
            this.btnEliminarLogo.Size = new System.Drawing.Size(100, 30);
            this.btnEliminarLogo.TabIndex = 3;
            this.btnEliminarLogo.Text = "order.remove_logo";
            this.btnEliminarLogo.UseVisualStyleBackColor = true;
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(510, 530);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(100, 35);
            this.btnGuardar.TabIndex = 15;
            this.btnGuardar.Text = "form.save";
            this.btnGuardar.UseVisualStyleBackColor = true;
            //
            // btnCancelar
            //
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(620, 530);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 35);
            this.btnCancelar.TabIndex = 16;
            this.btnCancelar.Text = "form.cancel";
            this.btnCancelar.UseVisualStyleBackColor = true;
            //
            // errorProvider1
            //
            this.errorProvider1.ContainerControl = this;
            //
            // PedidoDetalleForm
            //
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(740, 580);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.grpLogos);
            this.Controls.Add(this.cboEstadoProducto);
            this.Controls.Add(this.lblEstadoProducto);
            this.Controls.Add(this.dtpFechaLimiteProducto);
            this.Controls.Add(this.chkTieneFechaLimite);
            this.Controls.Add(this.nudPrecio);
            this.Controls.Add(this.lblPrecio);
            this.Controls.Add(this.nudCantidad);
            this.Controls.Add(this.lblCantidad);
            this.Controls.Add(this.cboProveedor);
            this.Controls.Add(this.lblProveedor);
            this.Controls.Add(this.cboCategoria);
            this.Controls.Add(this.lblCategoria);
            this.Controls.Add(this.cboProducto);
            this.Controls.Add(this.lblProducto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PedidoDetalleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detalle del Pedido";
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecio)).EndInit();
            this.grpLogos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
