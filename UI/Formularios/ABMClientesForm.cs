using BLL.Factories;
using BLL.Interfaces;
using Services.BLL.Interfaces;
using Services.BLL.Services;
using Services.BLL.Factories;
using DomainModel;
using Services.DomainModel.Entities;
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
        private readonly ICondicionIvaService _condicionIvaService;
        private bool _puedeEliminar;

        public ABMClientesForm()
        {
            InitializeComponent();
        }

        public ABMClientesForm(IClienteService clienteService, IBitacoraService bitacora, IGeoService geoSvc, ICondicionIvaService condicionIvaService)
        {
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _bitacora = bitacora ?? throw new ArgumentNullException(nameof(bitacora));
            _geoSvc = geoSvc ?? throw new ArgumentNullException(nameof(geoSvc));
            _condicionIvaService = condicionIvaService ?? throw new ArgumentNullException(nameof(condicionIvaService));

            InitializeComponent();

            // Engancho la lógica solo en runtime
            this.Load += ABMClientesForm_Load;
        }

        private sealed class ClienteGridRow
        {
            public Guid IdCliente { get; set; }
            public string RazonSocial { get; set; }
            public string CUIT { get; set; }
            public string Alias { get; set; }
            public string Domicilio { get; set; }
            public string Localidad { get; set; }
            public string Provincia { get; set; }
            public string Pais { get; set; }
            public string CondicionIva { get; set; }
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
                DataPropertyName = nameof(ClienteGridRow.Alias),
                Name = "Alias",
                HeaderText = "Alias",
                FillWeight = 160,
                MinimumWidth = 140
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
                DataPropertyName = nameof(ClienteGridRow.CondicionIva),
                Name = "CondicionIva",
                HeaderText = "CondicionIVA",
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
            dgvClientes.Columns["Alias"].HeaderText = "cliente.alias".Traducir();
            dgvClientes.Columns["CUIT"].HeaderText = "cliente.cuit".Traducir();
            dgvClientes.Columns["Domicilio"].HeaderText = "cliente.domicilio".Traducir();
            dgvClientes.Columns["Localidad"].HeaderText = "cliente.localidad".Traducir();
            dgvClientes.Columns["Provincia"].HeaderText = "cliente.provincia".Traducir();
            dgvClientes.Columns["Pais"].HeaderText = "cliente.pais".Traducir();
            dgvClientes.Columns["CondicionIva"].HeaderText = "cliente.condicionIVA".Traducir();
            dgvClientes.Columns["Activo"].HeaderText = "cliente.activo".Traducir();

            tslBuscar.Text = "form.search".Traducir();

            // Usa un helper que no rompe si la columna no existe
            SetHeaderSafe("RazonSocial", "cliente.razonSocial");
            SetHeaderSafe("Alias", "cliente.alias");
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
            txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Buscar(); } };
            txtBuscar.TextChanged += (s, e) => Buscar();

            tsbNuevo.Click += (s, e) => NuevoCliente();
            tsbEditar.Click += (s, e) => EditarSeleccionado();
            tsbEliminar.Click += (s, e) => EliminarSeleccionado();
            dgvClientes.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };
        }

        private void CargarClientes()
        {
            try
            {
                var logSvc = ServicesFactory.CrearLogService();
                logSvc.LogInfo("Iniciando carga de clientes", "Clientes", SessionContext.NombreUsuario);

                var clientes = _clienteService.ObtenerClientesActivos()?.ToList() ?? new List<Cliente>();

                var (dPais, dProv, dLoc) = ConstruirMapeosGeograficos();
                var rows = ProyectarClientes(clientes, dPais, dProv, dLoc);

                bsClientes.DataSource = rows;

                logSvc.LogInfo($"Cargados {clientes.Count} clientes activos exitosamente", "Clientes", SessionContext.NombreUsuario);
            }
            catch (Exception ex)
            {
                var logSvc = ServicesFactory.CrearLogService();
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
                var (dPais, dProv, dLoc) = ConstruirMapeosGeograficos();
                bsClientes.DataSource = ProyectarClientes(data, dPais, dProv, dLoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private (Dictionary<Guid, string> dPais, Dictionary<Guid, string> dProv, Dictionary<Guid, string> dLoc) ConstruirMapeosGeograficos()
        {
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

            return (dPais, dProv, dLoc);
        }

        private static List<ClienteGridRow> ProyectarClientes(IEnumerable<Cliente> clientes,
            Dictionary<Guid, string> dPais,
            Dictionary<Guid, string> dProv,
            Dictionary<Guid, string> dLoc)
        {
            var resultado = new List<ClienteGridRow>();

            foreach (var c in clientes ?? Enumerable.Empty<Cliente>())
            {
                var localidad = string.Empty;
                if (c.IdLocalidad.HasValue && dLoc.TryGetValue(c.IdLocalidad.Value, out var locNombre))
                    localidad = locNombre;
                else if (!string.IsNullOrWhiteSpace(c.Localidad))
                    localidad = c.Localidad;

                var provincia = string.Empty;
                if (c.IdProvincia.HasValue && dProv.TryGetValue(c.IdProvincia.Value, out var provNombre))
                    provincia = provNombre;

                var pais = string.Empty;
                if (c.IdPais.HasValue && dPais.TryGetValue(c.IdPais.Value, out var paisNombre))
                    pais = paisNombre;

                resultado.Add(new ClienteGridRow
                {
                    IdCliente = c.IdCliente,
                    RazonSocial = c.RazonSocial,
                    Alias = c.Alias,
                    CUIT = c.CUIT,
                    Domicilio = c.Domicilio,
                    Localidad = localidad,
                    Provincia = provincia,
                    Pais = pais,
                    CondicionIva = c.CondicionIva?.Nombre ?? string.Empty,
                    Activo = c.Activo
                });
            }

            return resultado;
        }

        private ClienteGridRow GetSeleccionado()
        {
            return bsClientes.Current as ClienteGridRow;
        }

        private void NuevoCliente()
        {
            var logSvc = ServicesFactory.CrearLogService();
            logSvc.LogInfo("Abriendo formulario nuevo cliente", "Clientes", SessionContext.NombreUsuario);

            using (var f = new ClienteForm(_clienteService, _bitacora, _geoSvc, _condicionIvaService, null))
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

            using (var f = new ClienteForm(_clienteService, _bitacora, _geoSvc, _condicionIvaService, cliente))
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