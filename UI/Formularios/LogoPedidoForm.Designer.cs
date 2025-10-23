using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    partial class LogoPedidoForm
    {
        private IContainer components = null;
        private Label lblTecnica;
        private ComboBox cboTecnica;
        private Label lblUbicacion;
        private ComboBox cboUbicacion;
        private Label lblProveedor;
        private ComboBox cboProveedor;
        private Label lblCosto;
        private NumericUpDown nudCosto;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
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
            this.lblTecnica = new System.Windows.Forms.Label();
            this.cboTecnica = new System.Windows.Forms.ComboBox();
            this.lblUbicacion = new System.Windows.Forms.Label();
            this.cboUbicacion = new System.Windows.Forms.ComboBox();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.cboProveedor = new System.Windows.Forms.ComboBox();
            this.lblCosto = new System.Windows.Forms.Label();
            this.nudCosto = new System.Windows.Forms.NumericUpDown();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudCosto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            //
            // lblTecnica
            //
            this.lblTecnica.Location = new System.Drawing.Point(20, 20);
            this.lblTecnica.Name = "lblTecnica";
            this.lblTecnica.Size = new System.Drawing.Size(120, 22);
            this.lblTecnica.TabIndex = 0;
            this.lblTecnica.Text = "order.logo.technique";
            this.lblTecnica.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cboTecnica
            //
            this.cboTecnica.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTecnica.FormattingEnabled = true;
            this.cboTecnica.Location = new System.Drawing.Point(140, 20);
            this.cboTecnica.Name = "cboTecnica";
            this.cboTecnica.Size = new System.Drawing.Size(260, 21);
            this.cboTecnica.TabIndex = 1;
            //
            // lblUbicacion
            //
            this.lblUbicacion.Location = new System.Drawing.Point(20, 54);
            this.lblUbicacion.Name = "lblUbicacion";
            this.lblUbicacion.Size = new System.Drawing.Size(120, 22);
            this.lblUbicacion.TabIndex = 2;
            this.lblUbicacion.Text = "order.logo.location";
            this.lblUbicacion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cboUbicacion
            //
            this.cboUbicacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUbicacion.FormattingEnabled = true;
            this.cboUbicacion.Location = new System.Drawing.Point(140, 54);
            this.cboUbicacion.Name = "cboUbicacion";
            this.cboUbicacion.Size = new System.Drawing.Size(260, 21);
            this.cboUbicacion.TabIndex = 3;
            //
            // lblProveedor
            //
            this.lblProveedor.Location = new System.Drawing.Point(20, 88);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new System.Drawing.Size(120, 22);
            this.lblProveedor.TabIndex = 4;
            this.lblProveedor.Text = "order.logo.provider";
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
            // lblCosto
            //
            this.lblCosto.Location = new System.Drawing.Point(20, 122);
            this.lblCosto.Name = "lblCosto";
            this.lblCosto.Size = new System.Drawing.Size(120, 22);
            this.lblCosto.TabIndex = 6;
            this.lblCosto.Text = "order.logo.cost";
            this.lblCosto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudCosto
            //
            this.nudCosto.DecimalPlaces = 2;
            this.nudCosto.Location = new System.Drawing.Point(140, 122);
            this.nudCosto.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.nudCosto.Name = "nudCosto";
            this.nudCosto.Size = new System.Drawing.Size(150, 20);
            this.nudCosto.TabIndex = 7;
            this.nudCosto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // lblDescripcion
            //
            this.lblDescripcion.Location = new System.Drawing.Point(20, 156);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(120, 22);
            this.lblDescripcion.TabIndex = 8;
            this.lblDescripcion.Text = "order.logo.description";
            this.lblDescripcion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(140, 156);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescripcion.Size = new System.Drawing.Size(360, 80);
            this.txtDescripcion.TabIndex = 9;
            this.txtDescripcion.MaxLength = 500;
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(300, 260);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(90, 30);
            this.btnGuardar.TabIndex = 10;
            this.btnGuardar.Text = "form.save";
            this.btnGuardar.UseVisualStyleBackColor = true;
            //
            // btnCancelar
            //
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(410, 260);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 30);
            this.btnCancelar.TabIndex = 11;
            this.btnCancelar.Text = "form.cancel";
            this.btnCancelar.UseVisualStyleBackColor = true;
            //
            // errorProvider1
            //
            this.errorProvider1.ContainerControl = this;
            //
            // LogoPedidoForm
            //
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(520, 310);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.nudCosto);
            this.Controls.Add(this.lblCosto);
            this.Controls.Add(this.cboProveedor);
            this.Controls.Add(this.lblProveedor);
            this.Controls.Add(this.cboUbicacion);
            this.Controls.Add(this.lblUbicacion);
            this.Controls.Add(this.cboTecnica);
            this.Controls.Add(this.lblTecnica);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogoPedidoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Logo del Pedido";
            ((System.ComponentModel.ISupportInitialize)(this.nudCosto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
