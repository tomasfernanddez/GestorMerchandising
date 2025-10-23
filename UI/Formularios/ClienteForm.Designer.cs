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
        private Label lblCUIT;
        private TextBox txtCUIT;
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
            this.lblCUIT = new System.Windows.Forms.Label();
            this.txtCUIT = new System.Windows.Forms.TextBox();
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
            // lblCUIT
            // 
            this.lblCUIT.Location = new System.Drawing.Point(20, 54);
            this.lblCUIT.Name = "lblCUIT";
            this.lblCUIT.Size = new System.Drawing.Size(120, 22);
            this.lblCUIT.TabIndex = 2;
            this.lblCUIT.Text = "cliente.cuit";
            this.lblCUIT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCUIT
            // 
            this.txtCUIT.Location = new System.Drawing.Point(140, 54);
            this.txtCUIT.Name = "txtCUIT";
            this.txtCUIT.Size = new System.Drawing.Size(180, 20);
            this.txtCUIT.TabIndex = 3;
            // 
            // lblDomicilio
            // 
            this.lblDomicilio.Location = new System.Drawing.Point(20, 88);
            this.lblDomicilio.Name = "lblDomicilio";
            this.lblDomicilio.Size = new System.Drawing.Size(120, 22);
            this.lblDomicilio.TabIndex = 4;
            this.lblDomicilio.Text = "cliente.domicilio";
            this.lblDomicilio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDomicilio
            // 
            this.txtDomicilio.Location = new System.Drawing.Point(140, 88);
            this.txtDomicilio.Name = "txtDomicilio";
            this.txtDomicilio.Size = new System.Drawing.Size(360, 20);
            this.txtDomicilio.TabIndex = 5;
            // 
            // lblPais
            // 
            this.lblPais.Location = new System.Drawing.Point(20, 122);
            this.lblPais.Name = "lblPais";
            this.lblPais.Size = new System.Drawing.Size(120, 22);
            this.lblPais.TabIndex = 6;
            this.lblPais.Text = "cliente.pais";
            this.lblPais.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboPais
            // 
            this.cboPais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPais.FormattingEnabled = true;
            this.cboPais.Location = new System.Drawing.Point(140, 122);
            this.cboPais.Name = "cboPais";
            this.cboPais.Size = new System.Drawing.Size(220, 21);
            this.cboPais.TabIndex = 7;
            // 
            // lblProvincia
            // 
            this.lblProvincia.Location = new System.Drawing.Point(20, 156);
            this.lblProvincia.Name = "lblProvincia";
            this.lblProvincia.Size = new System.Drawing.Size(120, 22);
            this.lblProvincia.TabIndex = 8;
            this.lblProvincia.Text = "cliente.provincia";
            this.lblProvincia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboProvincia
            // 
            this.cboProvincia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProvincia.FormattingEnabled = true;
            this.cboProvincia.Location = new System.Drawing.Point(140, 156);
            this.cboProvincia.Name = "cboProvincia";
            this.cboProvincia.Size = new System.Drawing.Size(220, 21);
            this.cboProvincia.TabIndex = 9;
            // 
            // lblLocalidad
            // 
            this.lblLocalidad.Location = new System.Drawing.Point(20, 190);
            this.lblLocalidad.Name = "lblLocalidad";
            this.lblLocalidad.Size = new System.Drawing.Size(120, 22);
            this.lblLocalidad.TabIndex = 10;
            this.lblLocalidad.Text = "cliente.localidad";
            this.lblLocalidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboLocalidad
            // 
            this.cboLocalidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocalidad.FormattingEnabled = true;
            this.cboLocalidad.Location = new System.Drawing.Point(140, 190);
            this.cboLocalidad.Name = "cboLocalidad";
            this.cboLocalidad.Size = new System.Drawing.Size(220, 21);
            this.cboLocalidad.TabIndex = 11;
            // 
            // lblCondIVA
            // 
            this.lblCondIVA.Location = new System.Drawing.Point(20, 224);
            this.lblCondIVA.Name = "lblCondIVA";
            this.lblCondIVA.Size = new System.Drawing.Size(120, 22);
            this.lblCondIVA.TabIndex = 12;
            this.lblCondIVA.Text = "cliente.condicionIVA";
            this.lblCondIVA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboCondIVA
            //
            this.cboCondIVA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCondIVA.FormattingEnabled = true;
            this.cboCondIVA.Location = new System.Drawing.Point(140, 224);
            this.cboCondIVA.Name = "cboCondIVA";
            this.cboCondIVA.Size = new System.Drawing.Size(220, 21);
            this.cboCondIVA.TabIndex = 13;
            // 
            // chkActivo
            // 
            this.chkActivo.AutoSize = true;
            this.chkActivo.Location = new System.Drawing.Point(140, 258);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new System.Drawing.Size(90, 17);
            this.chkActivo.TabIndex = 14;
            this.chkActivo.Text = "cliente.activo";
            this.chkActivo.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(140, 295);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(110, 28);
            this.btnGuardar.TabIndex = 15;
            this.btnGuardar.Text = "form.save";
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(260, 295);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 28);
            this.btnCancelar.TabIndex = 16;
            this.btnCancelar.Text = "form.cancel";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ClienteForm
            // 
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(560, 355);
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
            this.Controls.Add(this.txtCUIT);
            this.Controls.Add(this.lblCUIT);
            this.Controls.Add(this.txtRazon);
            this.Controls.Add(this.lblRazon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClienteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clientes";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private bool IsDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime
                   || (Site != null && Site.DesignMode)
                   || DesignMode;
        }
    }
}