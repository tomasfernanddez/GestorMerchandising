using BLL.Factories;
using BLL.Interfaces;
using Services.BLL.Interfaces;
using Services.BLL.Services;
using Services.BLL.Factories;
using DomainModel;
using Services;
using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UI.Helpers;
using UI.Localization;

namespace UI
{

    public partial class ABMClientesForm : Form
    {
        private readonly IClienteService _clienteService;
        private readonly IBitacoraService _bitacora;
        private readonly IGeoService _geoSvc;
        private readonly ICondicionIvaService _condicionIvaService;
        private readonly Dictionary<string, (string Es, string En)> _diccionarioMensajes;
        private bool _puedeEliminar;
        private bool _cargandoFiltros;

        public ABMClientesForm()
        {
            InitializeComponent();
            _diccionarioMensajes = CrearDiccionarioMensajes();
            _cargandoFiltros = false;
        }

        public ABMClientesForm(IClienteService clienteService, IBitacoraService bitacora, IGeoService geoSvc, ICondicionIvaService condicionIvaService)
        {
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _bitacora = bitacora ?? throw new ArgumentNullException(nameof(bitacora));
            _geoSvc = geoSvc ?? throw new ArgumentNullException(nameof(geoSvc));
            _condicionIvaService = condicionIvaService ?? throw new ArgumentNullException(nameof(condicionIvaService));
            _diccionarioMensajes = CrearDiccionarioMensajes();
            _cargandoFiltros = false;

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

        private sealed class EstadoFiltroItem
        {
            public EstadoFiltroItem(bool? estado, string descripcion)
            {
                Estado = estado;
                Descripcion = descripcion;
            }

            public bool? Estado { get; }
            public string Descripcion { get; }

            public override string ToString() => Descripcion;
        }

        private void ABMClientesForm_Load(object sender, EventArgs e)
        {
            EnsureColumns();
            // permisos: solo Admin puede eliminar
            _puedeEliminar = (SessionContext.NombrePerfil ?? "")
                .Equals("Administrador", StringComparison.OrdinalIgnoreCase);
            tsbEliminar.Visible = _puedeEliminar;
            tsbActivar.Visible = _puedeEliminar;

            ApplyTexts();
            WireUp();
            ConfigurarFiltros();
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
            tsbActivar.Text = "client.tool.activate".Traducir();
            lblFiltroEstado.Text = "client.filter.status".Traducir();

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
            tsbActivar.Click += (s, e) => ActivarSeleccionado();
            dgvClientes.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };
            dgvClientes.SelectionChanged += (s, e) => ActualizarAcciones();
            if (cboFiltroEstado.ComboBox != null)
                cboFiltroEstado.ComboBox.SelectedIndexChanged += (s, e) => { if (!_cargandoFiltros) Buscar(); };
        }

        private void CargarClientes()
        {
            try
            {
                RegistrarLogInfo("clients.log.load.start");

                var estado = ObtenerFiltroEstado();
                var clientes = _clienteService.ObtenerClientesPorEstado(estado)?.ToList() ?? new List<Cliente>();

                var (dPais, dProv, dLoc) = ConstruirMapeosGeograficos();
                var rows = ProyectarClientes(clientes, dPais, dProv, dLoc);

                bsClientes.DataSource = rows;
                RegistrarLogInfo("clients.log.load.success", clientes.Count);
                ActualizarAcciones();
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarLogError("clients.log.load.error", ex);
                MessageBox.Show(friendly, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var estado = ObtenerFiltroEstado();
                var data = _clienteService.BuscarClientesPorRazonSocial(filtro) ?? Enumerable.Empty<Cliente>();
                if (estado.HasValue)
                    data = data.Where(c => c.Activo == estado.Value);

                var lista = data.ToList();
                var (dPais, dProv, dLoc) = ConstruirMapeosGeograficos();
                bsClientes.DataSource = ProyectarClientes(lista, dPais, dProv, dLoc);
                ActualizarAcciones();
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarLogError("clients.log.search.error", ex, filtro);
                MessageBox.Show(friendly, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void ActualizarAcciones()
        {
            if (!_puedeEliminar)
            {
                tsbEliminar.Enabled = false;
                tsbActivar.Enabled = false;
                return;
            }

            var row = GetSeleccionado();
            tsbEliminar.Enabled = row != null && row.Activo;
            tsbActivar.Enabled = row != null && !row.Activo;
        }

        private void ConfigurarFiltros()
        {
            if (cboFiltroEstado?.ComboBox == null)
                return;

            _cargandoFiltros = true;
            try
            {
                var items = new List<EstadoFiltroItem>
                {
                    new EstadoFiltroItem(null, "client.filter.status.all".Traducir()),
                    new EstadoFiltroItem(true, "client.filter.status.active".Traducir()),
                    new EstadoFiltroItem(false, "client.filter.status.inactive".Traducir())
                };

                cboFiltroEstado.ComboBox.Items.Clear();
                foreach (var item in items)
                {
                    cboFiltroEstado.ComboBox.Items.Add(item);
                }

                var seleccionado = items.FirstOrDefault(i => i.Estado == true) ?? items.First();
                cboFiltroEstado.ComboBox.SelectedItem = seleccionado;
            }
            finally
            {
                _cargandoFiltros = false;
            }

            ActualizarAcciones();
        }

        private bool? ObtenerFiltroEstado()
        {
            if (cboFiltroEstado?.ComboBox?.SelectedItem is EstadoFiltroItem item)
                return item.Estado;

            return null;
        }

        private void NuevoCliente()
        {
            RegistrarLogInfo("clients.log.create.open");

            using (var f = new ClienteForm(_clienteService, _bitacora, _geoSvc, _condicionIvaService, null))
            {
                var dialogResult = f.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    RegistrarLogInfo("clients.log.create.success");
                    CargarClientes();
                }
                else
                {
                    RegistrarLogInfo("clients.log.create.cancel");
                }
            }
        }

        private void EditarSeleccionado()
        {
            var row = GetSeleccionado();
            if (row == null) return;

            var nombreCliente = DisplayNameHelper.FormatearNombreConAlias(row.RazonSocial, row.Alias);

            try
            {
                RegistrarLogInfo("clients.log.edit.start", nombreCliente);

                var cliente = _clienteService.ObtenerClientePorId(row.IdCliente);
                if (cliente == null)
                {
                    RegistrarLogWarning("clients.log.edit.notFound", nombreCliente);
                    return;
                }

                using (var f = new ClienteForm(_clienteService, _bitacora, _geoSvc, _condicionIvaService, cliente))
                {
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        RegistrarLogInfo("clients.log.edit.success", nombreCliente);
                        CargarClientes();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarLogError("clients.log.edit.error", ex, nombreCliente);
                MessageBox.Show(ErrorMessageHelper.GetFriendlyMessage(ex), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarSeleccionado()
        {
            if (!_puedeEliminar) return;

            var row = GetSeleccionado();
            if (row == null || !row.Activo) return;

            if (MessageBox.Show("client.confirm.deactivate".Traducir(), Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            var nombreCliente = DisplayNameHelper.FormatearNombreConAlias(row.RazonSocial, row.Alias);

            try
            {
                RegistrarLogInfo("clients.log.deactivate.start", nombreCliente);

                var resultado = _clienteService.DesactivarCliente(row.IdCliente);
                if (!resultado.EsValido)
                {
                    RegistrarBitacora("Cliente.Baja", "clients.audit.deactivate.failure", false, resultado.Mensaje, nombreCliente, row.IdCliente);
                    RegistrarLogWarning("clients.log.deactivate.failure", nombreCliente, resultado.Mensaje);
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RegistrarBitacora("Cliente.Baja", "clients.audit.deactivate.success", true, null, nombreCliente, row.IdCliente);
                RegistrarLogInfo("clients.log.deactivate.success", nombreCliente);
                CargarClientes();
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarBitacora("Cliente.Baja", "clients.audit.deactivate.failure", false, friendly, nombreCliente, row.IdCliente);
                RegistrarLogError("clients.log.deactivate.error", ex, nombreCliente);
                MessageBox.Show(friendly, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActivarSeleccionado()
        {
            if (!_puedeEliminar)
                return;

            var row = GetSeleccionado();
            if (row == null || row.Activo)
                return;

            if (MessageBox.Show("client.confirm.activate".Traducir(), Text,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var nombreCliente = DisplayNameHelper.FormatearNombreConAlias(row.RazonSocial, row.Alias);

            try
            {
                RegistrarLogInfo("clients.log.activate.start", nombreCliente);

                var resultado = _clienteService.ActivarCliente(row.IdCliente);
                if (!resultado.EsValido)
                {
                    RegistrarBitacora("Cliente.Activar", "clients.audit.activate.failure", false, resultado.Mensaje, nombreCliente, row.IdCliente);
                    RegistrarLogWarning("clients.log.activate.failure", nombreCliente, resultado.Mensaje);
                    MessageBox.Show(resultado.Mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RegistrarBitacora("Cliente.Activar", "clients.audit.activate.success", true, null, nombreCliente, row.IdCliente);
                RegistrarLogInfo("clients.log.activate.success", nombreCliente);
                CargarClientes();
            }
            catch (Exception ex)
            {
                var friendly = ErrorMessageHelper.GetFriendlyMessage(ex);
                RegistrarBitacora("Cliente.Activar", "clients.audit.activate.failure", false, friendly, nombreCliente, row.IdCliente);
                RegistrarLogError("clients.log.activate.error", ex, nombreCliente);
                MessageBox.Show(friendly, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObtenerMensaje(string clave, params object[] args)
        {
            if (_diccionarioMensajes != null && _diccionarioMensajes.TryGetValue(clave, out var textos))
            {
                var mensajeEs = args != null && args.Length > 0 ? string.Format(textos.Es, args) : textos.Es;
                var mensajeEn = args != null && args.Length > 0 ? string.Format(textos.En, args) : textos.En;
                return string.Concat(mensajeEs, " / ", mensajeEn);
            }

            return args != null && args.Length > 0 ? string.Format(clave, args) : clave;
        }

        private void RegistrarLogInfo(string claveMensaje, params object[] args)
        {
            var logSvc = ServicesFactory.CrearLogService();
            logSvc.LogInfo(ObtenerMensaje(claveMensaje, args), "Clientes", SessionContext.NombreUsuario);
        }

        private void RegistrarLogWarning(string claveMensaje, params object[] args)
        {
            var logSvc = ServicesFactory.CrearLogService();
            logSvc.LogWarning(ObtenerMensaje(claveMensaje, args), "Clientes", SessionContext.NombreUsuario);
        }

        private void RegistrarLogError(string claveMensaje, Exception ex, params object[] args)
        {
            var logSvc = ServicesFactory.CrearLogService();
            logSvc.LogError(ObtenerMensaje(claveMensaje, args), ex, "Clientes", SessionContext.NombreUsuario);
        }

        private void RegistrarBitacora(string accion, string claveMensaje, bool exitoso, string detalle, params object[] args)
        {
            var mensaje = ObtenerMensaje(claveMensaje, args);
            _bitacora?.RegistrarAccion(SessionContext.IdUsuario, accion, mensaje, "Clientes", exitoso, exitoso ? null : detalle);
        }

        private Dictionary<string, (string Es, string En)> CrearDiccionarioMensajes()
        {
            return new Dictionary<string, (string Es, string En)>
            {
                ["clients.log.load.start"] = ("Iniciando carga de clientes.", "Starting customer load."),
                ["clients.log.load.success"] = ("Se cargaron {0} clientes.", "Loaded {0} customers."),
                ["clients.log.load.error"] = ("Error al cargar los clientes.", "Error loading customers."),
                ["clients.log.search.error"] = ("Error buscando clientes con el filtro '{0}'.", "Error searching customers with filter '{0}'."),
                ["clients.log.create.open"] = ("Abriendo formulario de alta de cliente.", "Opening customer creation form."),
                ["clients.log.create.success"] = ("Cliente creado correctamente.", "Customer created successfully."),
                ["clients.log.create.cancel"] = ("Creación de cliente cancelada.", "Customer creation cancelled."),
                ["clients.log.edit.start"] = ("Iniciando edición del cliente {0}.", "Starting edition of customer {0}."),
                ["clients.log.edit.success"] = ("Cliente {0} actualizado.", "Customer {0} updated."),
                ["clients.log.edit.error"] = ("Error editando al cliente {0}.", "Error editing customer {0}."),
                ["clients.log.edit.notFound"] = ("El cliente {0} ya no se encuentra disponible.", "Customer {0} is no longer available."),
                ["clients.log.deactivate.start"] = ("Desactivando cliente {0}.", "Deactivating customer {0}."),
                ["clients.log.deactivate.success"] = ("Cliente {0} desactivado.", "Customer {0} deactivated."),
                ["clients.log.deactivate.failure"] = ("No se pudo desactivar al cliente {0}: {1}.", "Could not deactivate customer {0}: {1}."),
                ["clients.log.deactivate.error"] = ("Error desactivando al cliente {0}.", "Error deactivating customer {0}."),
                ["clients.log.activate.start"] = ("Activando cliente {0}.", "Activating customer {0}."),
                ["clients.log.activate.success"] = ("Cliente {0} activado.", "Customer {0} activated."),
                ["clients.log.activate.failure"] = ("No se pudo activar al cliente {0}: {1}.", "Could not activate customer {0}: {1}."),
                ["clients.log.activate.error"] = ("Error activando al cliente {0}.", "Error activating customer {0}."),
                ["clients.audit.deactivate.success"] = ("Se desactivó el cliente {0} (ID: {1}).", "Customer {0} (ID: {1}) was deactivated."),
                ["clients.audit.deactivate.failure"] = ("No se pudo desactivar el cliente {0} (ID: {1}).", "Could not deactivate customer {0} (ID: {1})."),
                ["clients.audit.activate.success"] = ("Se activó el cliente {0} (ID: {1}).", "Customer {0} (ID: {1}) was activated."),
                ["clients.audit.activate.failure"] = ("No se pudo activar el cliente {0} (ID: {1}).", "Could not activate customer {0} (ID: {1}).")
            };
        }
    }
}