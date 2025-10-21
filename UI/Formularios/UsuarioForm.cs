using BLL.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Localization;

namespace UI.Formularios
{
    public partial class UsuarioForm : Form
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPerfilService _perfilService;
        private readonly Usuario _usuario;
        private readonly bool _esEdicion;

        public UsuarioForm(IUsuarioService usuarioService, IPerfilService perfilService, Usuario usuario)
        {
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
            _usuario = usuario;
            _esEdicion = usuario != null;

            InitializeComponent();
            this.Load += UsuarioForm_Load;
        }

        private void UsuarioForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarPerfiles();
            if (_esEdicion)
                CargarDatos();
            WireUp();
        }

        private void ApplyTexts()
        {
            Text = _esEdicion ? "abm.usuarios.editar".Traducir() : "abm.usuarios.nuevo".Traducir();

            lblNombreUsuario.Text = "usuario.nombre_usuario".Traducir();
            lblNombreCompleto.Text = "usuario.nombre_completo".Traducir();
            lblEmail.Text = "usuario.email".Traducir();
            lblPerfil.Text = "usuario.perfil".Traducir();
            lblPassword.Text = "usuario.password_new".Traducir();
            lblPasswordConfirm.Text = "usuario.password_confirm".Traducir();
            chkActivo.Text = "usuario.activo".Traducir();
            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();

            // Tooltips para los checkboxes de mostrar contraseña
            var tooltip = new ToolTip();
            tooltip.SetToolTip(chkMostrarPassword, "login.show_password".Traducir());
            tooltip.SetToolTip(chkMostrarPasswordConfirm, "login.show_password".Traducir());
        }

        private void CargarPerfiles()
        {
            try
            {
                var perfiles = _perfilService.ObtenerPerfilesActivos().ToList();
                cboPerfil.DataSource = perfiles;
                cboPerfil.DisplayMember = "NombrePerfil";
                cboPerfil.ValueMember = "IdPerfil";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando perfiles: {ex.Message}", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatos()
        {
            if (_usuario == null) return;

            txtNombreUsuario.Text = _usuario.NombreUsuario;
            txtNombreCompleto.Text = _usuario.NombreCompleto;
            txtEmail.Text = _usuario.Email;
            chkActivo.Checked = _usuario.Activo;

            // En edición, la contraseña es OPCIONAL
            lblPassword.Text = "usuario.password_new".Traducir() + " (opcional)";
            lblPasswordConfirm.Text = "usuario.password_confirm".Traducir() + " (opcional)";

            cboPerfil.SelectedValue = _usuario.IdPerfil;
        }

        private void WireUp()
        {
            btnGuardar.Click += (s, e) => Guardar();
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            // Eventos para mostrar/ocultar contraseñas
            chkMostrarPassword.CheckedChanged += (s, e) =>
            {
                if (chkMostrarPassword.Checked)
                {
                    txtPassword.UseSystemPasswordChar = false;
                    txtPassword.PasswordChar = '\0';
                }
                else
                {
                    txtPassword.UseSystemPasswordChar = true;
                    txtPassword.PasswordChar = default(char);
                }
            };

            chkMostrarPasswordConfirm.CheckedChanged += (s, e) =>
            {
                if (chkMostrarPasswordConfirm.Checked)
                {
                    txtPasswordConfirm.UseSystemPasswordChar = false;
                    txtPasswordConfirm.PasswordChar = '\0';
                }
                else
                {
                    txtPasswordConfirm.UseSystemPasswordChar = true;
                    txtPasswordConfirm.PasswordChar = default(char);
                }
            };

            // Tooltips
            var tooltip = new ToolTip();
            tooltip.SetToolTip(chkMostrarPassword, "Mostrar contraseña");
            tooltip.SetToolTip(chkMostrarPasswordConfirm, "Mostrar contraseña");
        }

        private void Guardar()
        {
            errorProvider1.Clear();

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
            {
                errorProvider1.SetError(txtNombreUsuario, "msg.required".Traducir());
                return;
            }

            if (cboPerfil.SelectedValue == null)
            {
                errorProvider1.SetError(cboPerfil, "msg.required".Traducir());
                return;
            }

            // Validación de contraseñas
            bool hayPassword = !string.IsNullOrWhiteSpace(txtPassword.Text);
            bool hayPasswordConfirm = !string.IsNullOrWhiteSpace(txtPasswordConfirm.Text);

            if (!_esEdicion && !hayPassword)
            {
                errorProvider1.SetError(txtPassword, "usuario.password_required".Traducir());
                return;
            }

            if (hayPassword || hayPasswordConfirm)
            {
                if (txtPassword.Text != txtPasswordConfirm.Text)
                {
                    errorProvider1.SetError(txtPasswordConfirm, "usuario.password_mismatch".Traducir());
                    return;
                }

                if (txtPassword.Text.Length < 4)
                {
                    errorProvider1.SetError(txtPassword, "La contraseña debe tener al menos 4 caracteres");
                    return;
                }
            }

            try
            {
                if (_esEdicion)
                {
                    _usuario.NombreUsuario = txtNombreUsuario.Text.Trim();
                    _usuario.NombreCompleto = txtNombreCompleto.Text.Trim();
                    _usuario.Email = txtEmail.Text.Trim();
                    _usuario.IdPerfil = (Guid)cboPerfil.SelectedValue;
                    _usuario.Activo = chkActivo.Checked;

                    var resultado = _usuarioService.ActualizarUsuario(_usuario);

                    if (!resultado.EsValido)
                    {
                        MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Si ingresó nueva contraseña, cambiarla
                    if (hayPassword)
                    {
                        var resultadoPass = _usuarioService.CambiarPassword(_usuario.IdUsuario, txtPassword.Text);
                        if (!resultadoPass.EsValido)
                        {
                            MessageBox.Show(resultadoPass.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                else
                {
                    var nuevoUsuario = new Usuario
                    {
                        NombreUsuario = txtNombreUsuario.Text.Trim(),
                        NombreCompleto = txtNombreCompleto.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        IdPerfil = (Guid)cboPerfil.SelectedValue,
                        Activo = chkActivo.Checked
                    };

                    var resultado = _usuarioService.CrearUsuario(nuevoUsuario, txtPassword.Text);

                    if (!resultado.EsValido)
                    {
                        MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
