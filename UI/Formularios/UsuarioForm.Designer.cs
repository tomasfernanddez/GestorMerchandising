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
            this.components = new Container();
            this.lblNombreUsuario = new Label();
            this.txtNombreUsuario = new TextBox();
            this.lblNombreCompleto = new Label();
            this.txtNombreCompleto = new TextBox();
            this.lblEmail = new Label();
            this.txtEmail = new TextBox();
            this.lblPerfil = new Label();
            this.cboPerfil = new ComboBox();
            this.lblPassword = new Label();
            this.txtPassword = new TextBox();
            this.lblPasswordConfirm = new Label();
            this.txtPasswordConfirm = new TextBox();
            this.chkActivo = new CheckBox();
            this.btnGuardar = new Button();
            this.btnCancelar = new Button();
            this.errorProvider1 = new ErrorProvider(this.components);

            ((ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();

            // lblNombreUsuario
            this.lblNombreUsuario.Location = new Point(20, 20);
            this.lblNombreUsuario.Size = new Size(120, 22);
            this.lblNombreUsuario.Text = "Nombre Usuario:";
            this.lblNombreUsuario.TextAlign = ContentAlignment.MiddleLeft;

            // txtNombreUsuario
            this.txtNombreUsuario.Location = new Point(150, 20);
            this.txtNombreUsuario.Size = new Size(300, 20);
            this.txtNombreUsuario.MaxLength = 50;

            // lblNombreCompleto
            this.lblNombreCompleto.Location = new Point(20, 54);
            this.lblNombreCompleto.Size = new Size(120, 22);
            this.lblNombreCompleto.Text = "Nombre Completo:";
            this.lblNombreCompleto.TextAlign = ContentAlignment.MiddleLeft;

            // txtNombreCompleto
            this.txtNombreCompleto.Location = new Point(150, 54);
            this.txtNombreCompleto.Size = new Size(300, 20);
            this.txtNombreCompleto.MaxLength = 100;

            // lblEmail
            this.lblEmail.Location = new Point(20, 88);
            this.lblEmail.Size = new Size(120, 22);
            this.lblEmail.Text = "Email:";
            this.lblEmail.TextAlign = ContentAlignment.MiddleLeft;

            // txtEmail
            this.txtEmail.Location = new Point(150, 88);
            this.txtEmail.Size = new Size(300, 20);
            this.txtEmail.MaxLength = 100;

            // lblPerfil
            this.lblPerfil.Location = new Point(20, 122);
            this.lblPerfil.Size = new Size(120, 22);
            this.lblPerfil.Text = "Perfil:";
            this.lblPerfil.TextAlign = ContentAlignment.MiddleLeft;

            // cboPerfil
            this.cboPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboPerfil.Location = new Point(150, 122);
            this.cboPerfil.Size = new Size(200, 21);

            // lblPassword
            this.lblPassword.Location = new Point(20, 156);
            this.lblPassword.Size = new Size(120, 22);
            this.lblPassword.Text = "Nueva Contraseña:";
            this.lblPassword.TextAlign = ContentAlignment.MiddleLeft;

            // txtPassword
            this.txtPassword.Location = new Point(150, 156);
            this.txtPassword.Size = new Size(200, 20);
            this.txtPassword.UseSystemPasswordChar = true;

            // lblPasswordConfirm - NUEVO
            this.lblPasswordConfirm.Location = new Point(20, 190);
            this.lblPasswordConfirm.Size = new Size(120, 22);
            this.lblPasswordConfirm.Text = "Confirmar Contraseña:";
            this.lblPasswordConfirm.TextAlign = ContentAlignment.MiddleLeft;

            // txtPasswordConfirm - NUEVO
            this.txtPasswordConfirm.Location = new Point(150, 190);
            this.txtPasswordConfirm.Size = new Size(200, 20);
            this.txtPasswordConfirm.UseSystemPasswordChar = true;

            // chkMostrarPassword - NUEVO
            this.chkMostrarPassword = new CheckBox();
            this.chkMostrarPassword.Location = new Point(360, 156);
            this.chkMostrarPassword.Size = new Size(20, 20);
            this.chkMostrarPassword.TabIndex = 100;
            this.chkMostrarPassword.UseVisualStyleBackColor = true;

            // chkMostrarPasswordConfirm - NUEVO
            this.chkMostrarPasswordConfirm = new CheckBox();
            this.chkMostrarPasswordConfirm.Location = new Point(360, 190);
            this.chkMostrarPasswordConfirm.Size = new Size(20, 20);
            this.chkMostrarPasswordConfirm.TabIndex = 101;
            this.chkMostrarPasswordConfirm.UseVisualStyleBackColor = true;

            // chkActivo
            this.chkActivo.Location = new Point(150, 224);
            this.chkActivo.Size = new Size(100, 24);
            this.chkActivo.Text = "Activo";
            this.chkActivo.Checked = true;

            // btnGuardar
            this.btnGuardar.Location = new Point(150, 264);
            this.btnGuardar.Size = new Size(110, 28);
            this.btnGuardar.Text = "Guardar";

            // btnCancelar
            this.btnCancelar.Location = new Point(270, 264);
            this.btnCancelar.Size = new Size(110, 28);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.DialogResult = DialogResult.Cancel;

            // errorProvider1
            this.errorProvider1.ContainerControl = this;

            // UsuarioForm
            this.AcceptButton = this.btnGuardar;
            this.CancelButton = this.btnCancelar;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(490, 320);
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
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UsuarioForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Usuario";

            ((ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblPasswordConfirm;
        private TextBox txtPasswordConfirm;
    }
}