using System;
using System.Windows.Forms;
using Services.BLL.Helpers;
using Services.BLL.Interfaces;
using Services.DomainModel.Entities;
using UI.Localization;

namespace UI.Formularios
{
    public partial class PerfilForm : Form
    {
        private readonly IPerfilService _perfilService;
        private readonly Perfil _perfilOriginal;
        private readonly bool _esEdicion;

        public Perfil PerfilGuardado { get; private set; }

        public PerfilForm(IPerfilService perfilService, Perfil perfil = null)
        {
            _perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
            _perfilOriginal = perfil;
            _esEdicion = perfil != null;

            InitializeComponent();
        }

        private void PerfilForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarDatos();
        }

        private void ApplyTexts()
        {
            Text = _esEdicion ? "profile.form.title.edit".Traducir() : "profile.form.title.new".Traducir();
            lblNombre.Text = "profile.form.name".Traducir();
            lblDescripcion.Text = "profile.form.description".Traducir();
            chkActivo.Text = "profile.form.active".Traducir();
            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
        }

        private void CargarDatos()
        {
            if (_esEdicion && _perfilOriginal != null)
            {
                txtNombre.Text = _perfilOriginal.NombrePerfil;
                txtDescripcion.Text = _perfilOriginal.Descripcion;
                chkActivo.Checked = _perfilOriginal.Activo;
            }
            else
            {
                chkActivo.Checked = true;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void Guardar()
        {
            errorProvider1.Clear();

            var nombre = txtNombre.Text?.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                errorProvider1.SetError(txtNombre, "msg.required".Traducir());
                txtNombre.Focus();
                return;
            }

            if (nombre.Length > 50)
            {
                errorProvider1.SetError(txtNombre, "profile.validation.nameLength".Traducir());
                txtNombre.Focus();
                return;
            }

            var descripcion = txtDescripcion.Text?.Trim();
            if (!string.IsNullOrEmpty(descripcion) && descripcion.Length > 200)
            {
                errorProvider1.SetError(txtDescripcion, "profile.validation.descriptionLength".Traducir());
                txtDescripcion.Focus();
                return;
            }

            try
            {
                var perfil = new Perfil
                {
                    IdPerfil = _esEdicion ? _perfilOriginal.IdPerfil : Guid.Empty,
                    NombrePerfil = nombre,
                    Descripcion = descripcion,
                    Activo = chkActivo.Checked
                };

                ResultadoOperacion resultado = _esEdicion
                    ? _perfilService.ActualizarPerfil(perfil)
                    : _perfilService.CrearPerfil(perfil);

                if (!resultado.EsValido)
                {
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var idResultado = resultado.IdGenerado;
                if (!idResultado.HasValue || idResultado == Guid.Empty)
                {
                    idResultado = _esEdicion ? _perfilOriginal.IdPerfil : (Guid?)null;
                }

                if (idResultado.HasValue)
                {
                    PerfilGuardado = _perfilService.ObtenerPorId(idResultado.Value);
                }

                if (PerfilGuardado == null)
                {
                    PerfilGuardado = new Perfil
                    {
                        IdPerfil = idResultado ?? Guid.Empty,
                        NombrePerfil = nombre,
                        Descripcion = descripcion,
                        Activo = chkActivo.Checked
                    };
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"profile.error.saveWithDetail".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}