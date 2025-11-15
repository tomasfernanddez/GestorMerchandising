using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI.Formularios
{
    partial class UsuarioForm
    {
        private IContainer components = null;
        private Label lblNombreUsuario;
        private TextBox txtNombreUsuario;
        private Label lblNombreCompleto;
        private TextBox txtNombreCompleto;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPerfil;
        private ComboBox cboPerfil;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkMostrarPassword;
        private CheckBox chkMostrarPasswordConfirm;
        private CheckBox chkActivo;
        private Button btnGuardar;
        private Button btnCancelar;
        private ErrorProvider errorProvider1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblNombreUsuario = new System.Windows.Forms.Label();
            this.txtNombreUsuario = new System.Windows.Forms.TextBox();
            this.lblNombreCompleto = new System.Windows.Forms.Label();
            this.txtNombreCompleto = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPerfil = new System.Windows.Forms.Label();
            this.cboPerfil = new System.Windows.Forms.ComboBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPasswordConfirm = new System.Windows.Forms.Label();
            this.txtPasswordConfirm = new System.Windows.Forms.TextBox();
            this.chkActivo = new System.Windows.Forms.CheckBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkMostrarPassword = new System.Windows.Forms.CheckBox();
            this.chkMostrarPasswordConfirm = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNombreUsuario
            // 
            this.lblNombreUsuario.Location = new System.Drawing.Point(20, 20);
            this.lblNombreUsuario.Name = "lblNombreUsuario";
            this.lblNombreUsuario.Size = new System.Drawing.Size(120, 22);
            this.lblNombreUsuario.TabIndex = 113;
            this.lblNombreUsuario.Text = "Nombre Usuario:";
            this.lblNombreUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNombreUsuario
            // 
            this.txtNombreUsuario.Location = new System.Drawing.Point(150, 20);
            this.txtNombreUsuario.MaxLength = 50;
            this.txtNombreUsuario.Name = "txtNombreUsuario";
            this.txtNombreUsuario.Size = new System.Drawing.Size(300, 20);
            this.txtNombreUsuario.TabIndex = 112;
            // 
            // lblNombreCompleto
            // 
            this.lblNombreCompleto.Location = new System.Drawing.Point(20, 54);
            this.lblNombreCompleto.Name = "lblNombreCompleto";
            this.lblNombreCompleto.Size = new System.Drawing.Size(120, 22);
            this.lblNombreCompleto.TabIndex = 111;
            this.lblNombreCompleto.Text = "Nombre Completo:";
            this.lblNombreCompleto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNombreCompleto
            // 
            this.txtNombreCompleto.Location = new System.Drawing.Point(150, 54);
            this.txtNombreCompleto.MaxLength = 100;
            this.txtNombreCompleto.Name = "txtNombreCompleto";
            this.txtNombreCompleto.Size = new System.Drawing.Size(300, 20);
            this.txtNombreCompleto.TabIndex = 110;
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(20, 88);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(120, 22);
            this.lblEmail.TabIndex = 109;
            this.lblEmail.Text = "Email:";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(150, 88);
            this.txtEmail.MaxLength = 100;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(300, 20);
            this.txtEmail.TabIndex = 108;
            // 
            // lblPerfil
            // 
            this.lblPerfil.Location = new System.Drawing.Point(20, 122);
            this.lblPerfil.Name = "lblPerfil";
            this.lblPerfil.Size = new System.Drawing.Size(120, 22);
            this.lblPerfil.TabIndex = 107;
            this.lblPerfil.Text = "Perfil:";
            this.lblPerfil.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboPerfil
            // 
            this.cboPerfil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPerfil.Location = new System.Drawing.Point(150, 122);
            this.cboPerfil.Name = "cboPerfil";
            this.cboPerfil.Size = new System.Drawing.Size(200, 21);
            this.cboPerfil.TabIndex = 106;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(20, 156);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(120, 22);
            this.lblPassword.TabIndex = 105;
            this.lblPassword.Text = "Nueva Contraseña:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(150, 156);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 20);
            this.txtPassword.TabIndex = 104;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPasswordConfirm
            // 
            this.lblPasswordConfirm.Location = new System.Drawing.Point(20, 190);
            this.lblPasswordConfirm.Name = "lblPasswordConfirm";
            this.lblPasswordConfirm.Size = new System.Drawing.Size(120, 22);
            this.lblPasswordConfirm.TabIndex = 103;
            this.lblPasswordConfirm.Text = "Confirmar Contraseña:";
            this.lblPasswordConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPasswordConfirm
            // 
            this.txtPasswordConfirm.Location = new System.Drawing.Point(150, 190);
            this.txtPasswordConfirm.Name = "txtPasswordConfirm";
            this.txtPasswordConfirm.Size = new System.Drawing.Size(200, 20);
            this.txtPasswordConfirm.TabIndex = 102;
            this.txtPasswordConfirm.UseSystemPasswordChar = true;
            // 
            // chkActivo
            // 
            this.chkActivo.Checked = true;
            this.chkActivo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActivo.Location = new System.Drawing.Point(150, 224);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new System.Drawing.Size(100, 24);
            this.chkActivo.TabIndex = 2;
            this.chkActivo.Text = "Activo";
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(270, 268);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(110, 28);
            this.btnGuardar.TabIndex = 1;
            this.btnGuardar.Text = "Guardar";
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(150, 268);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 28);
            this.btnCancelar.TabIndex = 0;
            this.btnCancelar.Text = "Cancelar";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // chkMostrarPassword
            // 
            this.chkMostrarPassword.Location = new System.Drawing.Point(360, 156);
            this.chkMostrarPassword.Name = "chkMostrarPassword";
            this.chkMostrarPassword.Size = new System.Drawing.Size(20, 20);
            this.chkMostrarPassword.TabIndex = 100;
            this.chkMostrarPassword.UseVisualStyleBackColor = true;
            // 
            // chkMostrarPasswordConfirm
            // 
            this.chkMostrarPasswordConfirm.Location = new System.Drawing.Point(360, 190);
            this.chkMostrarPasswordConfirm.Name = "chkMostrarPasswordConfirm";
            this.chkMostrarPasswordConfirm.Size = new System.Drawing.Size(20, 20);
            this.chkMostrarPasswordConfirm.TabIndex = 101;
            this.chkMostrarPasswordConfirm.UseVisualStyleBackColor = true;
            // 
            // UsuarioForm
            // 
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(490, 320);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.chkActivo);
            this.Controls.Add(this.chkMostrarPassword);
            this.Controls.Add(this.chkMostrarPasswordConfirm);
            this.Controls.Add(this.txtPasswordConfirm);
            this.Controls.Add(this.lblPasswordConfirm);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.cboPerfil);
            this.Controls.Add(this.lblPerfil);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtNombreCompleto);
            this.Controls.Add(this.lblNombreCompleto);
            this.Controls.Add(this.txtNombreUsuario);
            this.Controls.Add(this.lblNombreUsuario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UsuarioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Usuario";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label lblPasswordConfirm;
        private TextBox txtPasswordConfirm;
    }
}