using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<Funcion> _funcionesDisponibles = new List<Funcion>();

        public Perfil PerfilGuardado { get; private set; }

        public PerfilForm(IPerfilService perfilService, Perfil perfil = null)
        {
            _perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
            _perfilOriginal = perfil;
            _esEdicion = perfil != null;

            InitializeComponent();
            Localization.LanguageChanged += OnLanguageChanged;
        }

        private void PerfilForm_Load(object sender, EventArgs e)
        {
            ApplyTexts();
            CargarFunciones();
            CargarDatos();
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            ApplyTexts();
            ActualizarFunciones();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Localization.LanguageChanged -= OnLanguageChanged;
            }
            base.Dispose(disposing);
        }

        private void ApplyTexts()
        {
            Text = _esEdicion ? "profile.form.title.edit".Traducir() : "profile.form.title.new".Traducir();
            lblNombre.Text = "profile.form.name".Traducir();
            lblDescripcion.Text = "profile.form.description".Traducir();
            chkActivo.Text = "profile.form.active".Traducir();
            lblFunciones.Text = "profile.form.functions".Traducir();
            btnGuardar.Text = "form.save".Traducir();
            btnCancelar.Text = "form.cancel".Traducir();
        }

        private void CargarFunciones()
        {
            clbFunciones.BeginUpdate();
            try
            {
                _funcionesDisponibles = _perfilService.ObtenerFuncionesDisponibles()?.ToList()
                    ?? new List<Funcion>();

                clbFunciones.Items.Clear();
                clbFunciones.DisplayMember = null; // Usaremos Format en lugar de DisplayMember

                // Agregar handler para traducir los nombres
                clbFunciones.Format += (s, e) =>
                {
                    if (e.ListItem is Funcion f)
                    {
                        e.Value = TraducirFuncion(f);
                    }
                };

                foreach (var funcion in _funcionesDisponibles)
                {
                    clbFunciones.Items.Add(funcion, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("profile.error.loadFunctions".Traducir(ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                clbFunciones.EndUpdate();
            }
        }

        private string TraducirFuncion(Funcion funcion)
        {
            if (funcion == null || string.IsNullOrEmpty(funcion.Codigo))
                return funcion?.Nombre ?? "";

            var key = $"function.{funcion.Codigo}";
            var traduccion = Localization.T(key);
            return traduccion == key ? funcion.Nombre : traduccion;
        }

        private void ActualizarFunciones()
        {
            if (clbFunciones.Items.Count == 0) return;

            // Refrescar los items para que se retraduzan
            clbFunciones.RefreshItems();
        }

        private void CargarDatos()
        {
            if (_esEdicion && _perfilOriginal != null)
            {
                txtNombre.Text = _perfilOriginal.NombrePerfil;
                txtDescripcion.Text = _perfilOriginal.Descripcion;
                chkActivo.Checked = _perfilOriginal.Activo;

                if (_perfilOriginal.Funciones != null)
                {
                    var idsSeleccionados = new HashSet<Guid>(_perfilOriginal.Funciones.Select(f => f.IdFuncion));
                    for (var i = 0; i < clbFunciones.Items.Count; i++)
                    {
                        if (clbFunciones.Items[i] is Funcion funcion && idsSeleccionados.Contains(funcion.IdFuncion))
                        {
                            clbFunciones.SetItemChecked(i, true);
                        }
                    }
                }
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

                var funcionesSeleccionadas = clbFunciones.CheckedItems
                    .Cast<Funcion>()
                    .Select(f => new Funcion { IdFuncion = f.IdFuncion })
                    .ToList();

                perfil.Funciones = funcionesSeleccionadas;

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