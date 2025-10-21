using System;
using System.Drawing;
using System.Windows.Forms;
using BLL.Interfaces;
using UI.Localization; // para "clave".Traducir()

namespace UI
{
    public partial class LoginForm : Form
    {
        private readonly IAutenticacionService _auth;
        private LinkLabel lnkRecuperacion;
        private Button btnIniciar;
        private TextBox txtContraseña;
        private TextBox txtUsuario;
        private Label lblContraseña;
        private Label lblUsuario;
        private System.ComponentModel.IContainer components;
        private Button btnIdioma;
        private CheckBox chkMostrarPassword;
        private Label lblMostrar;
        private ErrorProvider errorProvider1;

        public LoginForm(IAutenticacionService auth)
        {
            _auth = auth;

            InitializeComponent();          // crea los controles
            WireUp();                       // engancha eventos y estilos
            ApplyTexts();                   // aplica multiidioma (si no hay clave, muestra la clave)
        }

        private void WireUp()
        {
            // estética básica
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9.5f);

            // comportamiento
            txtContraseña.UseSystemPasswordChar = true;
            AcceptButton = btnIniciar;          // Enter = iniciar

            // eventos
            Shown += (s, e) => txtUsuario.Focus();
            btnIniciar.Click += btnIniciar_Click;
            lnkRecuperacion.LinkClicked += lnkRecuperacion_LinkClicked;
        }

        private void ApplyTexts()
        {
            // claves del diccionario de idioma.*.txt
            Text = "login.title".Traducir();
            lblUsuario.Text = "login.user".Traducir();
            lblContraseña.Text = "login.pass".Traducir();
            btnIniciar.Text = "login.button".Traducir();
            lnkRecuperacion.Text = "login.forgot".Traducir();
            lnkRecuperacion.Text = "login.forgot".Traducir();
            lblMostrar.Text = "login.show".Traducir();
            var tooltip = new ToolTip();
            tooltip.SetToolTip(chkMostrarPassword, "login.show_password".Traducir());
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            var user = txtUsuario.Text?.Trim();
            var pass = txtContraseña.Text;

            if (string.IsNullOrWhiteSpace(user))
            { errorProvider1.SetError(txtUsuario, "msg.user_required".Traducir()); return; }

            if (string.IsNullOrWhiteSpace(pass))
            { errorProvider1.SetError(txtContraseña, "msg.pass_required".Traducir()); return; }

            try
            {
                btnIniciar.Enabled = false;

                var r = _auth.Login(user, pass);         // tu servicio existente
                if (r == null || !r.EsValido || r.Usuario == null)
                {
                    MessageBox.Show(r?.Mensaje ?? "msg.login_failed".Traducir(),
                                    "login.title".Traducir(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContraseña.Clear();
                    txtContraseña.Focus();
                    return;
                }

                SessionContext.Set(r.Usuario);           // tu helper de sesión
                Hide();
                using (var main = new MainForm())        // tu MainForm
                    main.ShowDialog(this);
                Close();
            }
            catch
            {
                MessageBox.Show("msg.login_error".Traducir(),
                                "login.title".Traducir(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { btnIniciar.Enabled = true; }
        }

        private void lnkRecuperacion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("msg.pass_reset".Traducir(),
                            "login.title".Traducir(), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --------------------------------------------------------------------
        // InitializeComponent: crea los controles con TUS nombres
        // --------------------------------------------------------------------
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lnkRecuperacion = new System.Windows.Forms.LinkLabel();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.txtContraseña = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblContraseña = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnIdioma = new System.Windows.Forms.Button();
            this.chkMostrarPassword = new System.Windows.Forms.CheckBox();
            this.lblMostrar = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lnkRecuperacion
            // 
            this.lnkRecuperacion.AutoSize = true;
            this.lnkRecuperacion.Location = new System.Drawing.Point(256, 281);
            this.lnkRecuperacion.Name = "lnkRecuperacion";
            this.lnkRecuperacion.Size = new System.Drawing.Size(119, 13);
            this.lnkRecuperacion.TabIndex = 6;
            this.lnkRecuperacion.TabStop = true;
            this.lnkRecuperacion.Text = "¿Olvidó su contraseña?";
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(184, 212);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(95, 25);
            this.btnIniciar.TabIndex = 5;
            this.btnIniciar.Text = "Iniciar sesión";
            this.btnIniciar.UseVisualStyleBackColor = true;
            // 
            // txtContraseña
            // 
            this.txtContraseña.Location = new System.Drawing.Point(164, 118);
            this.txtContraseña.Name = "txtContraseña";
            this.txtContraseña.PasswordChar = '*';
            this.txtContraseña.Size = new System.Drawing.Size(139, 20);
            this.txtContraseña.TabIndex = 3;
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(164, 58);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(139, 20);
            this.txtUsuario.TabIndex = 1;
            // 
            // lblContraseña
            // 
            this.lblContraseña.AutoSize = true;
            this.lblContraseña.Location = new System.Drawing.Point(86, 120);
            this.lblContraseña.Name = "lblContraseña";
            this.lblContraseña.Size = new System.Drawing.Size(64, 13);
            this.lblContraseña.TabIndex = 2;
            this.lblContraseña.Text = "Contraseña:";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(86, 61);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuario:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnIdioma
            // 
            this.btnIdioma.Location = new System.Drawing.Point(12, 275);
            this.btnIdioma.Name = "btnIdioma";
            this.btnIdioma.Size = new System.Drawing.Size(53, 25);
            this.btnIdioma.TabIndex = 7;
            this.btnIdioma.Text = "ES/EN";
            this.btnIdioma.UseVisualStyleBackColor = true;
            this.btnIdioma.Click += new System.EventHandler(this.btnIdioma_Click);
            // 
            // chkMostrarPassword
            // 
            this.chkMostrarPassword.Location = new System.Drawing.Point(164, 147);
            this.chkMostrarPassword.Name = "chkMostrarPassword";
            this.chkMostrarPassword.Size = new System.Drawing.Size(20, 20);
            this.chkMostrarPassword.TabIndex = 4;
            this.chkMostrarPassword.UseVisualStyleBackColor = true;
            this.chkMostrarPassword.CheckedChanged += new System.EventHandler(this.chkMostrarPassword_CheckedChanged_1);
            // 
            // lblMostrar
            // 
            this.lblMostrar.AutoSize = true;
            this.lblMostrar.Location = new System.Drawing.Point(181, 150);
            this.lblMostrar.Name = "lblMostrar";
            this.lblMostrar.Size = new System.Drawing.Size(98, 13);
            this.lblMostrar.TabIndex = 8;
            this.lblMostrar.Text = "Mostrar contraseña";
            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(405, 312);
            this.Controls.Add(this.lblMostrar);
            this.Controls.Add(this.chkMostrarPassword);
            this.Controls.Add(this.btnIdioma);
            this.Controls.Add(this.lnkRecuperacion);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.txtContraseña);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.lblContraseña);
            this.Controls.Add(this.lblUsuario);
            this.Name = "LoginForm";
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnIdioma_Click(object sender, EventArgs e)
        {
            // Cultura actual
            var current = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;

            // Alternar idioma
            var nuevo = current.StartsWith("es") ? "en-US" : "es-AR";

            // Cambiar cultura de la app
            var ci = new System.Globalization.CultureInfo(nuevo);
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            // Recargar diccionario de traducción
            UI.Localization.Localization.Load(ci.Name);

            // Guardar preferencia (ejemplo en AppSettings, o en Settings.Default)
            //Settings.Default.UICulture = nuevo;
            //Settings.Default.Save();

            // Refrescar textos de este form
            ApplyTexts();
        }

        private void chkMostrarPassword_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkMostrarPassword.Checked)
            {
                // Mostrar la contraseña en texto plano
                txtContraseña.UseSystemPasswordChar = false;
                txtContraseña.PasswordChar = '\0';
            }
            else
            {
                // Ocultar la contraseña
                txtContraseña.UseSystemPasswordChar = true;
                txtContraseña.PasswordChar = default(char);
            }
        }
    }
}