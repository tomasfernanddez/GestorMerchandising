using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    partial class ClienteForm
    {
        private IContainer components = null;
        private Label lblRazon;
        private TextBox txtRazon;
        private Label lblAlias;
        private TextBox txtAlias;
        private Label lblCUIT;
        private TextBox txtCUIT;
        private Label lblTipoEmpresa;
        private ComboBox cboTipoEmpresa;
        private Label lblDomicilio;
        private TextBox txtDomicilio;
        private Label lblPais;
        private ComboBox cboPais;
        private Label lblProvincia;
        private ComboBox cboProvincia;
        private Label lblLocalidad;
        private ComboBox cboLocalidad;
        private Label lblCondIVA;
        private ComboBox cboCondIVA;
        private CheckBox chkActivo;
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
            this.lblRazon = new System.Windows.Forms.Label();
            this.txtRazon = new System.Windows.Forms.TextBox();
            this.lblAlias = new System.Windows.Forms.Label();
            this.txtAlias = new System.Windows.Forms.TextBox();
            this.lblCUIT = new System.Windows.Forms.Label();
            this.txtCUIT = new System.Windows.Forms.TextBox();
            this.lblTipoEmpresa = new System.Windows.Forms.Label();
            this.cboTipoEmpresa = new System.Windows.Forms.ComboBox();
            this.lblDomicilio = new System.Windows.Forms.Label();
            this.txtDomicilio = new System.Windows.Forms.TextBox();
            this.lblPais = new System.Windows.Forms.Label();
            this.cboPais = new System.Windows.Forms.ComboBox();
            this.lblProvincia = new System.Windows.Forms.Label();
            this.cboProvincia = new System.Windows.Forms.ComboBox();
            this.lblLocalidad = new System.Windows.Forms.Label();
            this.cboLocalidad = new System.Windows.Forms.ComboBox();
            this.lblCondIVA = new System.Windows.Forms.Label();
            this.cboCondIVA = new System.Windows.Forms.ComboBox();
            this.chkActivo = new System.Windows.Forms.CheckBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRazon
            // 
            this.lblRazon.Location = new System.Drawing.Point(20, 20);
            this.lblRazon.Name = "lblRazon";
            this.lblRazon.Size = new System.Drawing.Size(120, 22);
            this.lblRazon.TabIndex = 0;
            this.lblRazon.Text = "cliente.razonSocial";
            this.lblRazon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRazon
            // 
            this.txtRazon.Location = new System.Drawing.Point(140, 20);
            this.txtRazon.Name = "txtRazon";
            this.txtRazon.Size = new System.Drawing.Size(360, 20);
            this.txtRazon.TabIndex = 1;
            // 
            // lblAlias
            // 
            this.lblAlias.Location = new System.Drawing.Point(20, 54);
            this.lblAlias.Name = "lblAlias";
            this.lblAlias.Size = new System.Drawing.Size(120, 22);
            this.lblAlias.TabIndex = 2;
            this.lblAlias.Text = "cliente.alias";
            this.lblAlias.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point(140, 54);
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size(360, 20);
            this.txtAlias.TabIndex = 3;
            // 
            // lblCUIT
            // 
            this.lblCUIT.Location = new System.Drawing.Point(20, 88);
            this.lblCUIT.Name = "lblCUIT";
            this.lblCUIT.Size = new System.Drawing.Size(120, 22);
            this.lblCUIT.TabIndex = 4;
            this.lblCUIT.Text = "cliente.cuit";
            this.lblCUIT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCUIT
            // 
            this.txtCUIT.Location = new System.Drawing.Point(140, 88);
            this.txtCUIT.Name = "txtCUIT";
            this.txtCUIT.Size = new System.Drawing.Size(180, 20);
            this.txtCUIT.TabIndex = 5;
            // 
            // lblTipoEmpresa
            // 
            this.lblTipoEmpresa.Location = new System.Drawing.Point(20, 122);
            this.lblTipoEmpresa.Name = "lblTipoEmpresa";
            this.lblTipoEmpresa.Size = new System.Drawing.Size(120, 22);
            this.lblTipoEmpresa.TabIndex = 6;
            this.lblTipoEmpresa.Text = "cliente.tipoEmpresa";
            this.lblTipoEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboTipoEmpresa
            // 
            this.cboTipoEmpresa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoEmpresa.FormattingEnabled = true;
            this.cboTipoEmpresa.Location = new System.Drawing.Point(140, 122);
            this.cboTipoEmpresa.Name = "cboTipoEmpresa";
            this.cboTipoEmpresa.Size = new System.Drawing.Size(220, 21);
            this.cboTipoEmpresa.TabIndex = 7;
            // 
            // lblDomicilio
            // 
            this.lblDomicilio.Location = new System.Drawing.Point(20, 156);
            this.lblDomicilio.Name = "lblDomicilio";
            this.lblDomicilio.Size = new System.Drawing.Size(120, 22);
            this.lblDomicilio.TabIndex = 8;
            this.lblDomicilio.Text = "cliente.domicilio";
            this.lblDomicilio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDomicilio
            // 
            this.txtDomicilio.Location = new System.Drawing.Point(140, 156);
            this.txtDomicilio.Name = "txtDomicilio";
            this.txtDomicilio.Size = new System.Drawing.Size(360, 20);
            this.txtDomicilio.TabIndex = 9;
            // 
            // lblPais
            // 
            this.lblPais.Location = new System.Drawing.Point(20, 190);
            this.lblPais.Name = "lblPais";
            this.lblPais.Size = new System.Drawing.Size(120, 22);
            this.lblPais.TabIndex = 10;
            this.lblPais.Text = "cliente.pais";
            this.lblPais.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboPais
            // 
            this.cboPais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPais.FormattingEnabled = true;
            this.cboPais.Location = new System.Drawing.Point(140, 190);
            this.cboPais.Name = "cboPais";
            this.cboPais.Size = new System.Drawing.Size(220, 21);
            this.cboPais.TabIndex = 11;
            // 
            // lblProvincia
            // 
            this.lblProvincia.Location = new System.Drawing.Point(20, 224);
            this.lblProvincia.Name = "lblProvincia";
            this.lblProvincia.Size = new System.Drawing.Size(120, 22);
            this.lblProvincia.TabIndex = 12;
            this.lblProvincia.Text = "cliente.provincia";
            this.lblProvincia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboProvincia
            // 
            this.cboProvincia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProvincia.FormattingEnabled = true;
            this.cboProvincia.Location = new System.Drawing.Point(140, 224);
            this.cboProvincia.Name = "cboProvincia";
            this.cboProvincia.Size = new System.Drawing.Size(220, 21);
            this.cboProvincia.TabIndex = 13;
            // 
            // lblLocalidad
            // 
            this.lblLocalidad.Location = new System.Drawing.Point(20, 258);
            this.lblLocalidad.Name = "lblLocalidad";
            this.lblLocalidad.Size = new System.Drawing.Size(120, 22);
            this.lblLocalidad.TabIndex = 14;
            this.lblLocalidad.Text = "cliente.localidad";
            this.lblLocalidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboLocalidad
            // 
            this.cboLocalidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocalidad.FormattingEnabled = true;
            this.cboLocalidad.Location = new System.Drawing.Point(140, 258);
            this.cboLocalidad.Name = "cboLocalidad";
            this.cboLocalidad.Size = new System.Drawing.Size(220, 21);
            this.cboLocalidad.TabIndex = 15;
            // 
            // lblCondIVA
            // 
            this.lblCondIVA.Location = new System.Drawing.Point(20, 292);
            this.lblCondIVA.Name = "lblCondIVA";
            this.lblCondIVA.Size = new System.Drawing.Size(120, 22);
            this.lblCondIVA.TabIndex = 16;
            this.lblCondIVA.Text = "cliente.condicionIVA";
            this.lblCondIVA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboCondIVA
            //
            this.cboCondIVA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCondIVA.FormattingEnabled = true;
            this.cboCondIVA.Location = new System.Drawing.Point(140, 292);
            this.cboCondIVA.Name = "cboCondIVA";
            this.cboCondIVA.Size = new System.Drawing.Size(220, 21);
            this.cboCondIVA.TabIndex = 17;
            // 
            // chkActivo
            // 
            this.chkActivo.AutoSize = true;
            this.chkActivo.Location = new System.Drawing.Point(140, 328);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new System.Drawing.Size(90, 17);
            this.chkActivo.TabIndex = 18;
            this.chkActivo.Text = "cliente.activo";
            this.chkActivo.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(140, 364);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(110, 28);
            this.btnGuardar.TabIndex = 19;
            this.btnGuardar.Text = "form.save";
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(260, 364);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 28);
            this.btnCancelar.TabIndex = 20;
            this.btnCancelar.Text = "form.cancel";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ClienteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 420);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.chkActivo);
            this.Controls.Add(this.cboCondIVA);
            this.Controls.Add(this.lblCondIVA);
            this.Controls.Add(this.cboLocalidad);
            this.Controls.Add(this.lblLocalidad);
            this.Controls.Add(this.cboProvincia);
            this.Controls.Add(this.lblProvincia);
            this.Controls.Add(this.cboPais);
            this.Controls.Add(this.lblPais);
            this.Controls.Add(this.txtDomicilio);
            this.Controls.Add(this.lblDomicilio);
            this.Controls.Add(this.cboTipoEmpresa);
            this.Controls.Add(this.lblTipoEmpresa);
            this.Controls.Add(this.txtCUIT);
            this.Controls.Add(this.lblCUIT);
            this.Controls.Add(this.txtAlias);
            this.Controls.Add(this.lblAlias);
            this.Controls.Add(this.txtRazon);
            this.Controls.Add(this.lblRazon);
            this.Name = "ClienteForm";
            this.Text = "cliente.title";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}