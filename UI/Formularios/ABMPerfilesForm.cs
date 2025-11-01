using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Services.BLL.Interfaces;
using Services.DomainModel.Entities;
using UI.Localization;

namespace UI.Formularios
{
    public partial class ABMPerfilesForm : Form
    {
        private readonly IPerfilService _perfilService;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogService _logService;

        private BindingList<PerfilRow> _rows;

        private sealed class PerfilRow
        {
            public Guid IdPerfil { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public bool Activo { get; set; }
        }

        public ABMPerfilesForm(IPerfilService perfilService, IBitacoraService bitacoraService, ILogService logService)
        {
            _perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
            _bitacoraService = bitacoraService ?? throw new ArgumentNullException(nameof(bitacoraService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));

            InitializeComponent();
        }

        private void ABMPerfilesForm_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ApplyTexts();
            WireEvents();
            CargarPerfiles();
        }

        private void ConfigurarGrid()
        {
            dgvPerfiles.AutoGenerateColumns = false;
            dgvPerfiles.Columns.Clear();

            dgvPerfiles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PerfilRow.Nombre),
                Name = nameof(PerfilRow.Nombre),
                HeaderText = "profile.column.name".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 45
            });

            dgvPerfiles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PerfilRow.Descripcion),
                Name = nameof(PerfilRow.Descripcion),
                HeaderText = "profile.column.description".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 45
            });

            dgvPerfiles.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(PerfilRow.Activo),
                Name = nameof(PerfilRow.Activo),
                HeaderText = "profile.column.active".Traducir(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });

            _rows = new BindingList<PerfilRow>();
            dgvPerfiles.DataSource = _rows;
            dgvPerfiles.SelectionChanged += (s, args) => ActualizarAcciones();
        }

        private void ApplyTexts()
        {
            Text = "profile.list.title".Traducir();
            tsbNuevo.Text = "profile.list.new".Traducir();
            tsbEditar.Text = "profile.list.edit".Traducir();
            tsbActivar.Text = "profile.list.activate".Traducir();
            tsbDesactivar.Text = "profile.list.deactivate".Traducir();
            tsbActualizar.Text = "form.refresh".Traducir();
            tslBuscar.Text = "form.search".Traducir();
            btnBuscar.Text = "form.filter".Traducir();

            if (dgvPerfiles.Columns.Count >= 3)
            {
                dgvPerfiles.Columns[nameof(PerfilRow.Nombre)].HeaderText = "profile.column.name".Traducir();
                dgvPerfiles.Columns[nameof(PerfilRow.Descripcion)].HeaderText = "profile.column.description".Traducir();
                dgvPerfiles.Columns[nameof(PerfilRow.Activo)].HeaderText = "profile.column.active".Traducir();
            }
        }

        private void WireEvents()
        {
            txtBuscar.TextChanged += (s, e) => Filtrar();
            txtBuscar.KeyDown += TxtBuscar_KeyDown;
        }

        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                Filtrar();
            }
        }

        private void Filtrar()
        {
            CargarPerfiles();
        }

        private void CargarPerfiles()
        {
            try
            {
                var seleccionado = ObtenerSeleccion()?.IdPerfil;
                var filtro = ObtenerTextoBusqueda();

                var perfiles = _perfilService.ObtenerTodos()?.ToList() ?? new List<Perfil>();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    perfiles = perfiles
                        .Where(p => Contiene(p.NombrePerfil, filtro) || Contiene(p.Descripcion, filtro))
                        .OrderBy(p => p.NombrePerfil)
                        .ToList();
                }

                _rows.RaiseListChangedEvents = false;
                _rows.Clear();

                foreach (var perfil in perfiles)
                {
                    _rows.Add(new PerfilRow
                    {
                        IdPerfil = perfil.IdPerfil,
                        Nombre = perfil.NombrePerfil,
                        Descripcion = perfil.Descripcion,
                        Activo = perfil.Activo
                    });
                }

                _rows.RaiseListChangedEvents = true;
                _rows.ResetBindings();

                if (seleccionado.HasValue)
                {
                    SeleccionarPerfil(seleccionado.Value);
                }

                ActualizarAcciones();
            }
            catch (Exception ex)
            {
                _logService.LogError("Error al cargar perfiles", ex, "Perfiles", SessionContext.NombreUsuario);
                MessageBox.Show("profile.error.load".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObtenerTextoBusqueda()
        {
            var texto = txtBuscar.Text?.Trim();
            return string.IsNullOrWhiteSpace(texto) ? null : texto;
        }

        private static bool Contiene(string origen, string filtro)
        {
            if (string.IsNullOrWhiteSpace(origen) || string.IsNullOrWhiteSpace(filtro))
                return false;

            return origen.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private PerfilRow ObtenerSeleccion()
        {
            return dgvPerfiles.CurrentRow?.DataBoundItem as PerfilRow;
        }

        private void SeleccionarPerfil(Guid idPerfil)
        {
            foreach (DataGridViewRow row in dgvPerfiles.Rows)
            {
                if (row.DataBoundItem is PerfilRow perfil && perfil.IdPerfil == idPerfil)
                {
                    row.Selected = true;
                    dgvPerfiles.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void ActualizarAcciones()
        {
            var seleccionado = ObtenerSeleccion();
            var haySeleccion = seleccionado != null;

            tsbEditar.Enabled = haySeleccion;
            tsbActivar.Enabled = haySeleccion && seleccionado != null && !seleccionado.Activo;
            tsbDesactivar.Enabled = haySeleccion && seleccionado != null && seleccionado.Activo;
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            AbrirEditor();
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            var seleccionado = ObtenerSeleccion();
            if (seleccionado == null)
                return;

            AbrirEditor(seleccionado.IdPerfil);
        }

        private void AbrirEditor(Guid? idPerfil = null)
        {
            try
            {
                Perfil perfil = null;
                if (idPerfil.HasValue)
                {
                    perfil = _perfilService.ObtenerPorId(idPerfil.Value);
                    if (perfil == null)
                    {
                        MessageBox.Show("profile.error.notFound".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                using (var form = new PerfilForm(_perfilService, perfil))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        var resultado = form.PerfilGuardado;
                        CargarPerfiles();
                        if (resultado != null)
                        {
                            SeleccionarPerfil(resultado.IdPerfil);
                        }

                        RegistrarAccion(idPerfil.HasValue ? "Perfil.Actualizar" : "Perfil.Crear",
                            idPerfil.HasValue
                                ? "profile.log.updated".Traducir(form.PerfilGuardado?.NombrePerfil ?? string.Empty)
                                : "profile.log.created".Traducir(form.PerfilGuardado?.NombrePerfil ?? string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error abriendo el editor de perfiles", ex, "Perfiles", SessionContext.NombreUsuario);
                MessageBox.Show("profile.error.load".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbActivar_Click(object sender, EventArgs e)
        {
            var seleccionado = ObtenerSeleccion();
            if (seleccionado == null)
                return;

            var confirm = MessageBox.Show(
                "profile.confirm.activate".Traducir(seleccionado.Nombre),
                Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                var resultado = _perfilService.ActivarPerfil(seleccionado.IdPerfil);
                if (resultado.EsValido)
                {
                    RegistrarAccion("Perfil.Activar", "profile.log.activated".Traducir(seleccionado.Nombre));
                    CargarPerfiles();
                    SeleccionarPerfil(seleccionado.IdPerfil);
                }
                else
                {
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error activando perfil", ex, "Perfiles", SessionContext.NombreUsuario);
                MessageBox.Show("profile.error.save".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbDesactivar_Click(object sender, EventArgs e)
        {
            var seleccionado = ObtenerSeleccion();
            if (seleccionado == null)
                return;

            var confirm = MessageBox.Show(
                "profile.confirm.deactivate".Traducir(seleccionado.Nombre),
                Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                var resultado = _perfilService.DesactivarPerfil(seleccionado.IdPerfil);
                if (resultado.EsValido)
                {
                    RegistrarAccion("Perfil.Desactivar", "profile.log.deactivated".Traducir(seleccionado.Nombre));
                    CargarPerfiles();
                }
                else
                {
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error desactivando perfil", ex, "Perfiles", SessionContext.NombreUsuario);
                MessageBox.Show("profile.error.save".Traducir(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            CargarPerfiles();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Filtrar();
        }

        private void dgvPerfiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var seleccionado = ObtenerSeleccion();
                if (seleccionado != null)
                {
                    AbrirEditor(seleccionado.IdPerfil);
                }
            }
        }

        private void RegistrarAccion(string accion, string descripcion)
        {
            try
            {
                _bitacoraService.RegistrarAccion(SessionContext.IdUsuario, accion, descripcion, "Perfiles");
                _logService.LogInfo(descripcion, "Perfiles", SessionContext.NombreUsuario);
            }
            catch
            {
                // Ignorar errores de logging
            }
        }
    }
}
