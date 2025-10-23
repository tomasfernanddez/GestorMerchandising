using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    partial class ProveedorForm
    {
        private IContainer components = null;
        private GroupBox grpGenerales;
        private Label lblRazonSocial;
        private TextBox txtRazonSocial;
        private Label lblCUIT;
        private TextBox txtCUIT;
        private Label lblCondicionIVA;
        private ComboBox cboCondicionIVA;
        private Label lblTipoProveedor;
        private ComboBox cboTipoProveedor;
        private Label lblCondicionesPago;
        private ComboBox cboCondicionesPago;
        private CheckBox chkActivo;
        private Label lblFechaAlta;
        private Label lblFechaAltaValor;
        private GroupBox grpUbicacion;
        private Label lblDomicilio;
        private TextBox txtDomicilio;
        private Label lblCodigoPostal;
        private TextBox txtCodigoPostal;
        private Label lblPais;
        private ComboBox cboPais;
        private Label lblProvincia;
        private ComboBox cboProvincia;
        private Label lblLocalidad;
        private ComboBox cboLocalidad;
        private GroupBox grpTecnicas;
        private CheckedListBox clbTecnicas;
        private GroupBox grpObservaciones;
        private TextBox txtObservaciones;
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
            this.grpGenerales = new System.Windows.Forms.GroupBox();
            this.lblFechaAltaValor = new System.Windows.Forms.Label();
            this.lblFechaAlta = new System.Windows.Forms.Label();
            this.chkActivo = new System.Windows.Forms.CheckBox();
            this.cboCondicionesPago = new System.Windows.Forms.ComboBox();
            this.lblCondicionesPago = new System.Windows.Forms.Label();
            this.cboTipoProveedor = new System.Windows.Forms.ComboBox();
            this.lblTipoProveedor = new System.Windows.Forms.Label();
            this.cboCondicionIVA = new System.Windows.Forms.ComboBox();
            this.lblCondicionIVA = new System.Windows.Forms.Label();
            this.txtCUIT = new System.Windows.Forms.TextBox();
            this.lblCUIT = new System.Windows.Forms.Label();
            this.txtRazonSocial = new System.Windows.Forms.TextBox();
            this.lblRazonSocial = new System.Windows.Forms.Label();
            this.grpUbicacion = new System.Windows.Forms.GroupBox();
            this.cboLocalidad = new System.Windows.Forms.ComboBox();
            this.lblLocalidad = new System.Windows.Forms.Label();
            this.cboProvincia = new System.Windows.Forms.ComboBox();
            this.lblProvincia = new System.Windows.Forms.Label();
            this.cboPais = new System.Windows.Forms.ComboBox();
            this.lblPais = new System.Windows.Forms.Label();
            this.txtCodigoPostal = new System.Windows.Forms.TextBox();
            this.lblCodigoPostal = new System.Windows.Forms.Label();
            this.txtDomicilio = new System.Windows.Forms.TextBox();
            this.lblDomicilio = new System.Windows.Forms.Label();
            this.grpTecnicas = new System.Windows.Forms.GroupBox();
            this.clbTecnicas = new System.Windows.Forms.CheckedListBox();
            this.grpObservaciones = new System.Windows.Forms.GroupBox();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpGenerales.SuspendLayout();
            this.grpUbicacion.SuspendLayout();
            this.grpTecnicas.SuspendLayout();
            this.grpObservaciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpGenerales
            // 
            this.grpGenerales.Controls.Add(this.lblFechaAltaValor);
            this.grpGenerales.Controls.Add(this.lblFechaAlta);
            this.grpGenerales.Controls.Add(this.chkActivo);
            this.grpGenerales.Controls.Add(this.cboCondicionesPago);
            this.grpGenerales.Controls.Add(this.lblCondicionesPago);
            this.grpGenerales.Controls.Add(this.cboTipoProveedor);
            this.grpGenerales.Controls.Add(this.lblTipoProveedor);
            this.grpGenerales.Controls.Add(this.cboCondicionIVA);
            this.grpGenerales.Controls.Add(this.lblCondicionIVA);
            this.grpGenerales.Controls.Add(this.txtCUIT);
            this.grpGenerales.Controls.Add(this.lblCUIT);
            this.grpGenerales.Controls.Add(this.txtRazonSocial);
            this.grpGenerales.Controls.Add(this.lblRazonSocial);
            this.grpGenerales.Location = new System.Drawing.Point(16, 15);
            this.grpGenerales.Name = "grpGenerales";
            this.grpGenerales.Size = new System.Drawing.Size(520, 220);
            this.grpGenerales.TabIndex = 0;
            this.grpGenerales.TabStop = false;
            this.grpGenerales.Text = "supplier.group.general";
            // 
            // lblFechaAltaValor
            // 
            this.lblFechaAltaValor.Location = new System.Drawing.Point(140, 180);
            this.lblFechaAltaValor.Name = "lblFechaAltaValor";
            this.lblFechaAltaValor.Size = new System.Drawing.Size(180, 20);
            this.lblFechaAltaValor.TabIndex = 12;
            this.lblFechaAltaValor.Text = "-";
            this.lblFechaAltaValor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFechaAlta
            // 
            this.lblFechaAlta.Location = new System.Drawing.Point(20, 180);
            this.lblFechaAlta.Name = "lblFechaAlta";
            this.lblFechaAlta.Size = new System.Drawing.Size(110, 20);
            this.lblFechaAlta.TabIndex = 11;
            this.lblFechaAlta.Text = "supplier.fechaAlta";
            this.lblFechaAlta.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkActivo
            // 
            this.chkActivo.AutoSize = true;
            this.chkActivo.Location = new System.Drawing.Point(324, 182);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new System.Drawing.Size(113, 17);
            this.chkActivo.TabIndex = 10;
            this.chkActivo.Text = "supplier.status.active";
            this.chkActivo.UseVisualStyleBackColor = true;
            // 
            // cboCondicionesPago
            // 
            this.cboCondicionesPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCondicionesPago.FormattingEnabled = true;
            this.cboCondicionesPago.Location = new System.Drawing.Point(140, 145);
            this.cboCondicionesPago.Name = "cboCondicionesPago";
            this.cboCondicionesPago.Size = new System.Drawing.Size(280, 21);
            this.cboCondicionesPago.TabIndex = 9;
            // 
            // lblCondicionesPago
            // 
            this.lblCondicionesPago.Location = new System.Drawing.Point(20, 145);
            this.lblCondicionesPago.Name = "lblCondicionesPago";
            this.lblCondicionesPago.Size = new System.Drawing.Size(110, 20);
            this.lblCondicionesPago.TabIndex = 8;
            this.lblCondicionesPago.Text = "supplier.condicionesPago";
            this.lblCondicionesPago.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboTipoProveedor
            // 
            this.cboTipoProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoProveedor.FormattingEnabled = true;
            this.cboTipoProveedor.Location = new System.Drawing.Point(140, 110);
            this.cboTipoProveedor.Name = "cboTipoProveedor";
            this.cboTipoProveedor.Size = new System.Drawing.Size(280, 21);
            this.cboTipoProveedor.TabIndex = 7;
            // 
            // lblTipoProveedor
            // 
            this.lblTipoProveedor.Location = new System.Drawing.Point(20, 110);
            this.lblTipoProveedor.Name = "lblTipoProveedor";
            this.lblTipoProveedor.Size = new System.Drawing.Size(110, 20);
            this.lblTipoProveedor.TabIndex = 6;
            this.lblTipoProveedor.Text = "supplier.tipoProveedor";
            this.lblTipoProveedor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboCondicionIVA
            // 
            this.cboCondicionIVA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCondicionIVA.FormattingEnabled = true;
            this.cboCondicionIVA.Location = new System.Drawing.Point(140, 75);
            this.cboCondicionIVA.Name = "cboCondicionIVA";
            this.cboCondicionIVA.Size = new System.Drawing.Size(280, 21);
            this.cboCondicionIVA.TabIndex = 5;
            // 
            // lblCondicionIVA
            // 
            this.lblCondicionIVA.Location = new System.Drawing.Point(20, 75);
            this.lblCondicionIVA.Name = "lblCondicionIVA";
            this.lblCondicionIVA.Size = new System.Drawing.Size(110, 20);
            this.lblCondicionIVA.TabIndex = 4;
            this.lblCondicionIVA.Text = "supplier.condicionIVA";
            this.lblCondicionIVA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCUIT
            // 
            this.txtCUIT.Location = new System.Drawing.Point(140, 45);
            this.txtCUIT.Name = "txtCUIT";
            this.txtCUIT.Size = new System.Drawing.Size(180, 20);
            this.txtCUIT.TabIndex = 3;
            // 
            // lblCUIT
            // 
            this.lblCUIT.Location = new System.Drawing.Point(20, 45);
            this.lblCUIT.Name = "lblCUIT";
            this.lblCUIT.Size = new System.Drawing.Size(110, 20);
            this.lblCUIT.TabIndex = 2;
            this.lblCUIT.Text = "supplier.cuit";
            this.lblCUIT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRazonSocial
            // 
            this.txtRazonSocial.Location = new System.Drawing.Point(140, 18);
            this.txtRazonSocial.Name = "txtRazonSocial";
            this.txtRazonSocial.Size = new System.Drawing.Size(360, 20);
            this.txtRazonSocial.TabIndex = 1;
            // 
            // lblRazonSocial
            // 
            this.lblRazonSocial.Location = new System.Drawing.Point(20, 18);
            this.lblRazonSocial.Name = "lblRazonSocial";
            this.lblRazonSocial.Size = new System.Drawing.Size(110, 20);
            this.lblRazonSocial.TabIndex = 0;
            this.lblRazonSocial.Text = "supplier.razonSocial";
            this.lblRazonSocial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpUbicacion
            // 
            this.grpUbicacion.Controls.Add(this.cboLocalidad);
            this.grpUbicacion.Controls.Add(this.lblLocalidad);
            this.grpUbicacion.Controls.Add(this.cboProvincia);
            this.grpUbicacion.Controls.Add(this.lblProvincia);
            this.grpUbicacion.Controls.Add(this.cboPais);
            this.grpUbicacion.Controls.Add(this.lblPais);
            this.grpUbicacion.Controls.Add(this.txtCodigoPostal);
            this.grpUbicacion.Controls.Add(this.lblCodigoPostal);
            this.grpUbicacion.Controls.Add(this.txtDomicilio);
            this.grpUbicacion.Controls.Add(this.lblDomicilio);
            this.grpUbicacion.Location = new System.Drawing.Point(16, 245);
            this.grpUbicacion.Name = "grpUbicacion";
            this.grpUbicacion.Size = new System.Drawing.Size(520, 190);
            this.grpUbicacion.TabIndex = 1;
            this.grpUbicacion.TabStop = false;
            this.grpUbicacion.Text = "supplier.group.ubicacion";
            // 
            // cboLocalidad
            // 
            this.cboLocalidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocalidad.FormattingEnabled = true;
            this.cboLocalidad.Location = new System.Drawing.Point(140, 150);
            this.cboLocalidad.Name = "cboLocalidad";
            this.cboLocalidad.Size = new System.Drawing.Size(260, 21);
            this.cboLocalidad.TabIndex = 9;
            // 
            // lblLocalidad
            // 
            this.lblLocalidad.Location = new System.Drawing.Point(20, 150);
            this.lblLocalidad.Name = "lblLocalidad";
            this.lblLocalidad.Size = new System.Drawing.Size(110, 20);
            this.lblLocalidad.TabIndex = 8;
            this.lblLocalidad.Text = "supplier.localidad";
            this.lblLocalidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboProvincia
            // 
            this.cboProvincia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProvincia.FormattingEnabled = true;
            this.cboProvincia.Location = new System.Drawing.Point(140, 115);
            this.cboProvincia.Name = "cboProvincia";
            this.cboProvincia.Size = new System.Drawing.Size(260, 21);
            this.cboProvincia.TabIndex = 7;
            // 
            // lblProvincia
            // 
            this.lblProvincia.Location = new System.Drawing.Point(20, 115);
            this.lblProvincia.Name = "lblProvincia";
            this.lblProvincia.Size = new System.Drawing.Size(110, 20);
            this.lblProvincia.TabIndex = 6;
            this.lblProvincia.Text = "supplier.provincia";
            this.lblProvincia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboPais
            // 
            this.cboPais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPais.FormattingEnabled = true;
            this.cboPais.Location = new System.Drawing.Point(140, 80);
            this.cboPais.Name = "cboPais";
            this.cboPais.Size = new System.Drawing.Size(260, 21);
            this.cboPais.TabIndex = 5;
            // 
            // lblPais
            // 
            this.lblPais.Location = new System.Drawing.Point(20, 80);
            this.lblPais.Name = "lblPais";
            this.lblPais.Size = new System.Drawing.Size(110, 20);
            this.lblPais.TabIndex = 4;
            this.lblPais.Text = "supplier.pais";
            this.lblPais.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCodigoPostal
            // 
            this.txtCodigoPostal.Location = new System.Drawing.Point(140, 50);
            this.txtCodigoPostal.Name = "txtCodigoPostal";
            this.txtCodigoPostal.Size = new System.Drawing.Size(120, 20);
            this.txtCodigoPostal.TabIndex = 3;
            // 
            // lblCodigoPostal
            // 
            this.lblCodigoPostal.Location = new System.Drawing.Point(20, 50);
            this.lblCodigoPostal.Name = "lblCodigoPostal";
            this.lblCodigoPostal.Size = new System.Drawing.Size(110, 20);
            this.lblCodigoPostal.TabIndex = 2;
            this.lblCodigoPostal.Text = "supplier.codigoPostal";
            this.lblCodigoPostal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDomicilio
            // 
            this.txtDomicilio.Location = new System.Drawing.Point(140, 20);
            this.txtDomicilio.Name = "txtDomicilio";
            this.txtDomicilio.Size = new System.Drawing.Size(360, 20);
            this.txtDomicilio.TabIndex = 1;
            // 
            // lblDomicilio
            // 
            this.lblDomicilio.Location = new System.Drawing.Point(20, 20);
            this.lblDomicilio.Name = "lblDomicilio";
            this.lblDomicilio.Size = new System.Drawing.Size(110, 20);
            this.lblDomicilio.TabIndex = 0;
            this.lblDomicilio.Text = "supplier.domicilio";
            this.lblDomicilio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpTecnicas
            // 
            this.grpTecnicas.Controls.Add(this.clbTecnicas);
            this.grpTecnicas.Location = new System.Drawing.Point(552, 15);
            this.grpTecnicas.Name = "grpTecnicas";
            this.grpTecnicas.Size = new System.Drawing.Size(280, 220);
            this.grpTecnicas.TabIndex = 2;
            this.grpTecnicas.TabStop = false;
            this.grpTecnicas.Text = "supplier.group.tecnicas";
            // 
            // clbTecnicas
            // 
            this.clbTecnicas.CheckOnClick = true;
            this.clbTecnicas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbTecnicas.FormattingEnabled = true;
            this.clbTecnicas.Location = new System.Drawing.Point(3, 16);
            this.clbTecnicas.Name = "clbTecnicas";
            this.clbTecnicas.Size = new System.Drawing.Size(274, 201);
            this.clbTecnicas.TabIndex = 0;
            // 
            // grpObservaciones
            // 
            this.grpObservaciones.Controls.Add(this.txtObservaciones);
            this.grpObservaciones.Location = new System.Drawing.Point(552, 245);
            this.grpObservaciones.Name = "grpObservaciones";
            this.grpObservaciones.Size = new System.Drawing.Size(280, 190);
            this.grpObservaciones.TabIndex = 3;
            this.grpObservaciones.TabStop = false;
            this.grpObservaciones.Text = "supplier.group.observaciones";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtObservaciones.Location = new System.Drawing.Point(3, 16);
            this.txtObservaciones.MaxLength = 500;
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservaciones.Size = new System.Drawing.Size(274, 171);
            this.txtObservaciones.TabIndex = 0;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(552, 450);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(120, 32);
            this.btnGuardar.TabIndex = 4;
            this.btnGuardar.Text = "form.save";
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(712, 450);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 32);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "form.cancel";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ProveedorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 500);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.grpObservaciones);
            this.Controls.Add(this.grpTecnicas);
            this.Controls.Add(this.grpUbicacion);
            this.Controls.Add(this.grpGenerales);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProveedorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "abm.suppliers.title";
            this.grpGenerales.ResumeLayout(false);
            this.grpGenerales.PerformLayout();
            this.grpUbicacion.ResumeLayout(false);
            this.grpUbicacion.PerformLayout();
            this.grpTecnicas.ResumeLayout(false);
            this.grpObservaciones.ResumeLayout(false);
            this.grpObservaciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}