using BLL.Factories;
using BLL.Interfaces;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UI.Localization;

namespace UI
{

    public partial class ABMClientesForm : Form
    {
        private readonly IClienteService _clienteService;
        private readonly IBitacoraService _bitacora;
        private readonly IGeoService _geoSvc;
        private bool _puedeEliminar;

        public ABMClientesForm()
        {
            InitializeComponent();
        }

        public ABMClientesForm(IClienteService clienteService, IBitacoraService bitacora, IGeoService geoSvc)
        {
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _bitacora = bitacora ?? throw new ArgumentNullException(nameof(bitacora));
            _geoSvc = geoSvc ?? throw new ArgumentNullException(nameof(geoSvc));

            InitializeComponent();

            // Engancho la lógica solo en runtime
            this.Load += ABMClientesForm_Load;
        }

        private sealed class ClienteGridRow
        {
            public Guid IdCliente { get; set; }
            public string RazonSocial { get; set; }
            public string CUIT { get; set; }
            public string Domicilio { get; set; }
            public string Localidad { get; set; }
            public string Provincia { get; set; }
            public string Pais { get; set; }
            public bool Activo { get; set; }
        }

        private void ABMClientesForm_Load(object sender, EventArgs e)
        {
            EnsureColumns();
            // permisos: solo Admin puede eliminar
            _puedeEliminar = (SessionContext.NombrePerfil ?? "")
                .Equals("Administrador", StringComparison.OrdinalIgnoreCase);
            tsbEliminar.Visible = _puedeEliminar;

            ApplyTexts();
            WireUp();
            CargarClientes();
        }

        private void EnsureColumns()
        {
            if (dgvClientes.Columns.Count > 0) return;

            dgvClientes.AutoGenerateColumns = false;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // <- responsive
            dgvClientes.AllowUserToResizeColumns = true;

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ClienteGridRow.IdCliente),
                Name = "IdCliente",
                Visible = false
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ClienteGridRow.RazonSocial),
                Name = "RazonSocial",
                HeaderText = "RazonSocial",
                FillWeight = 220,
                MinimumWidth = 160
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ClienteGridRow.CUIT),
                Name = "CUIT",
                HeaderText = "CUIT",
                FillWeight = 90,
                MinimumWidth = 90
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ClienteGridRow.Domicilio),
                Name = "Domicilio",
                HeaderText = "Domicilio",
                FillWeight = 160,
                MinimumWidth = 140
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ClienteGridRow.Localidad),
                Name = "Localidad",
                HeaderText = "Localidad",
                FillWeight = 120,
                MinimumWidth = 110
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ClienteGridRow.Provincia),
                Name = "Provincia",
                HeaderText = "Provincia",
                FillWeight = 120,
                MinimumWidth = 110
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ClienteGridRow.Pais),
                Name = "Pais",
                HeaderText = "Pais",
                FillWeight = 100,
                MinimumWidth = 90
            });

            // El check conviene “auto” por celdas para que no se deforme
            dgvClientes.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(ClienteGridRow.Activo),
                Name = "Activo",
                HeaderText = "Activo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                MinimumWidth = 70
            });
        }


        private void ApplyTexts()
        {
            dgvClientes.Columns["RazonSocial"].HeaderText = "cliente.razonSocial".Traducir();
            dgvClientes.Columns["CUIT"].HeaderText = "cliente.cuit".Traducir();
            dgvClientes.Columns["Domicilio"].HeaderText = "cliente.domicilio".Traducir();
            dgvClientes.Columns["Localidad"].HeaderText = "cliente.localidad".Traducir();
            dgvClientes.Columns["Provincia"].HeaderText = "cliente.provincia".Traducir();
            dgvClientes.Columns["Pais"].HeaderText = "cliente.pais".Traducir();
            dgvClientes.Columns["Activo"].HeaderText = "cliente.activo".Traducir();

            // Usa un helper que no rompe si la columna no existe
            SetHeaderSafe("RazonSocial", "cliente.razonSocial");
            SetHeaderSafe("CUIT", "cliente.cuit");
            SetHeaderSafe("Domicilio", "cliente.domicilio");
            SetHeaderSafe("Localidad", "cliente.localidad");
            SetHeaderSafe("CondicionIva", "cliente.condicionIVA"); // ojo: Name = CondicionIva
            SetHeaderSafe("Activo", "cliente.activo");
        }

        private void SetHeaderSafe(string columnName, string i18nKey)
        {
            var col = dgvClientes?.Columns?[columnName];
            if (col != null)
                col.HeaderText = i18nKey.Traducir();
            // si no existe, no hacemos nada (evita NullReference)
        }

        private void WireUp()
        {
            tsbActualizar.Click += (s, e) => CargarClientes();
            tsbBuscar.Click += (s, e) => Buscar();
            txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Buscar(); } };

            tsbNuevo.Click += (s, e) => NuevoCliente();
            tsbEditar.Click += (s, e) => EditarSeleccionado();
            tsbEliminar.Click += (s, e) => EliminarSeleccionado();
            dgvClientes.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };
        }

        private void CargarClientes()
        {
            try
            {
                var logSvc = ServiceFactory.CrearLogService();
                logSvc.LogInfo("Iniciando carga de clientes", "Clientes", SessionContext.NombreUsuario);

                var clientes = _clienteService.ObtenerClientesActivos()?.ToList() ?? new List<Cliente>();

                // Cache de catálogos
                var paises = _geoSvc.ListarPaises() ?? new List<GeoDTO>();
                var dPais = paises.ToDictionary(x => x.Id, x => x.Nombre);

                var dProv = new Dictionary<Guid, string>();
                var dLoc = new Dictionary<Guid, string>();

                foreach (var pais in paises)
                {
                    var provincias = _geoSvc.ListarProvinciasPorPais(pais.Id) ?? new List<GeoDTO>();
                    foreach (var prov in provincias)
                    {
                        if (!dProv.ContainsKey(prov.Id))
                            dProv[prov.Id] = prov.Nombre;

                        var localidades = _geoSvc.ListarLocalidadesPorProvincia(prov.Id) ?? new List<GeoDTO>();
                        foreach (var loc in localidades)
                        {
                            if (!dLoc.ContainsKey(loc.Id))
                                dLoc[loc.Id] = loc.Nombre;
                        }
                    }
                }

                // Proyección para el grid
                var rows = clientes.Select(c => new ClienteGridRow
                {
                    IdCliente = c.IdCliente,
                    RazonSocial = c.RazonSocial,
                    CUIT = c.CUIT,
                    Domicilio = c.Domicilio,
                    Localidad = (c.IdLocalidad.HasValue && dLoc.ContainsKey(c.IdLocalidad.Value)) ? dLoc[c.IdLocalidad.Value] : (c.Localidad ?? ""),
                    Provincia = (c.IdProvincia.HasValue && dProv.ContainsKey(c.IdProvincia.Value)) ? dProv[c.IdProvincia.Value] : "",
                    Pais = (c.IdPais.HasValue && dPais.ContainsKey(c.IdPais.Value)) ? dPais[c.IdPais.Value] : "",
                    Activo = c.Activo
                }).ToList();

                bsClientes.DataSource = rows;

                logSvc.LogInfo($"Cargados {clientes.Count} clientes activos exitosamente", "Clientes", SessionContext.NombreUsuario);
            }
            catch (Exception ex)
            {
                var logSvc = ServiceFactory.CrearLogService();
                logSvc.LogError("Error cargando clientes", ex, "Clientes", SessionContext.NombreUsuario);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Buscar()
        {
            try
            {
                var filtro = (txtBuscar.Text ?? "").Trim();
                if (string.IsNullOrEmpty(filtro))
                {
                    CargarClientes();
                    return;
                }

                // búsqueda por razón social desde BLL
                var data = _clienteService.BuscarClientesPorRazonSocial(filtro) ?? Enumerable.Empty<Cliente>();
                bsClientes.DataSource = data.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ClienteGridRow GetSeleccionado()
        {
            return bsClientes.Current as ClienteGridRow;
        }

        private void NuevoCliente()
        {
            var logSvc = ServiceFactory.CrearLogService();
            logSvc.LogInfo("Abriendo formulario nuevo cliente", "Clientes", SessionContext.NombreUsuario);

            using (var f = new ClienteForm(_clienteService, _bitacora, _geoSvc, null))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    logSvc.LogInfo("Cliente creado exitosamente desde formulario", "Clientes", SessionContext.NombreUsuario);
                    CargarClientes();
                }
                else
                {
                    logSvc.LogInfo("Creación de cliente cancelada", "Clientes", SessionContext.NombreUsuario);
                }
            }
        }

        private void EditarSeleccionado()
        {
            var row = GetSeleccionado();
            if (row == null) return;

            var cliente = _clienteService.ObtenerClientePorId(row.IdCliente);
            if (cliente == null) return;

            using (var f = new ClienteForm(_clienteService, _bitacora, _geoSvc, cliente))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                    CargarClientes();
            }
        }

        private void EliminarSeleccionado()
        {
            if (!_puedeEliminar) return;

            var row = GetSeleccionado();
            if (row == null) return;

            if (MessageBox.Show("msg.confirm.delete".Traducir(), Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                var res = _clienteService.DesactivarCliente(row.IdCliente);
                if (!res.EsValido)
                {
                    MessageBox.Show(res.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Cliente.Baja", res.Mensaje, "Clientes", false);
                }
                else
                {
                    _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Cliente.Baja", $"Id={row.IdCliente}", "Clientes", true);
                    CargarClientes();
                }
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarAccion(SessionContext.IdUsuario, "Cliente.Baja", ex.Message, "Clientes", false);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}