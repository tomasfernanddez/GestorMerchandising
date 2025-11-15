using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Extensions;
using DAL.Interfaces.Base;
using DomainModel;
using DomainModel.Entidades;

namespace BLL.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Inicializa una nueva instancia de ProveedorService.
        /// </summary>
        public ProveedorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Obtiene proveedor por id.
        /// </summary>
        public Proveedor ObtenerProveedorPorId(Guid idProveedor)
        {
            if (idProveedor == Guid.Empty)
                throw new ArgumentException("El ID del proveedor no puede estar vacío", nameof(idProveedor));

            return _unitOfWork.Proveedores.ObtenerConDetalles(idProveedor);
        }

        /// <summary>
        /// Obtiene asincrónicamente proveedor por id.
        /// </summary>
        public async Task<Proveedor> ObtenerProveedorPorIdAsync(Guid idProveedor)
        {
            if (idProveedor == Guid.Empty)
                throw new ArgumentException("El ID del proveedor no puede estar vacío", nameof(idProveedor));

            return await _unitOfWork.Proveedores.ObtenerConDetallesAsync(idProveedor);
        }

        /// <summary>
        /// Obtiene proveedores activos.
        /// </summary>
        public IEnumerable<Proveedor> ObtenerProveedoresActivos()
        {
            return _unitOfWork.Proveedores.GetProveedoresActivos();
        }

        /// <summary>
        /// Obtiene asincrónicamente proveedores activos.
        /// </summary>
        public async Task<IEnumerable<Proveedor>> ObtenerProveedoresActivosAsync()
        {
            return await _unitOfWork.Proveedores.GetProveedoresActivosAsync();
        }

        /// <summary>
        /// Busca proveedores.
        /// </summary>
        public IEnumerable<Proveedor> BuscarProveedores(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo)
        {
            return _unitOfWork.Proveedores.Buscar(razonSocial, cuit, idTipoProveedor, activo);
        }

        /// <summary>
        /// Busca proveedores.
        /// </summary>
        public IEnumerable<Proveedor> BuscarProveedores(string razonSocial, Guid? idTipoProveedor, bool? activo)
        {
            return BuscarProveedores(razonSocial, null, idTipoProveedor, activo);
        }

        /// <summary>
        /// Busca asincrónicamente proveedores.
        /// </summary>
        public async Task<IEnumerable<Proveedor>> BuscarProveedoresAsync(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo)
        {
            return await _unitOfWork.Proveedores.BuscarAsync(razonSocial, cuit, idTipoProveedor, activo);
        }

        /// <summary>
        /// Busca asincrónicamente proveedores.
        /// </summary>
        public async Task<IEnumerable<Proveedor>> BuscarProveedoresAsync(string razonSocial, Guid? idTipoProveedor, bool? activo)
        {
            return await BuscarProveedoresAsync(razonSocial, null, idTipoProveedor, activo);
        }

        /// <summary>
        /// Obtiene proveedor por cuit.
        /// </summary>
        public Proveedor ObtenerProveedorPorCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                throw new ArgumentException("El CUIT no puede estar vacío", nameof(cuit));

            var limpio = LimpiarCuit(cuit);
            return _unitOfWork.Proveedores.GetProveedorPorCUIT(limpio);
        }

        /// <summary>
        /// Obtiene asincrónicamente proveedor por cuit.
        /// </summary>
        public async Task<Proveedor> ObtenerProveedorPorCUITAsync(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                throw new ArgumentException("El CUIT no puede estar vacío", nameof(cuit));

            var limpio = LimpiarCuit(cuit);
            return await _unitOfWork.Proveedores.GetProveedorPorCUITAsync(limpio);
        }

        /// <summary>
        /// Crea proveedor.
        /// </summary>
        public ResultadoOperacion CrearProveedor(Proveedor proveedor, IEnumerable<Guid> tiposProveedor, IEnumerable<Guid> tecnicasPersonalizacion)
        {
            try
            {
                var resultadoValidacion = ValidarProveedor(proveedor, tiposProveedor, tecnicasPersonalizacion);
                if (!resultadoValidacion.EsValido)
                    return resultadoValidacion;

                var cuitLimpio = LimpiarCuit(proveedor.CUIT);
                if (_unitOfWork.Proveedores.ExisteCUIT(cuitLimpio))
                    return ResultadoOperacion.Error($"Ya existe un proveedor con el CUIT {FormatearCuit(cuitLimpio)}");

                proveedor.IdProveedor = Guid.NewGuid();
                proveedor.CUIT = cuitLimpio;
                proveedor.FechaAlta = DateTime.Now;
                proveedor.Activo = true;
                proveedor.CondicionesPago = NormalizarCondicionPago(proveedor.CondicionesPago);
                proveedor.Observaciones = proveedor.Observaciones?.Trim();
                proveedor.Domicilio = proveedor.Domicilio?.Trim();
                proveedor.CodigoPostal = proveedor.CodigoPostal?.Trim();
                proveedor.Localidad = proveedor.Localidad?.Trim();
                proveedor.Alias = string.IsNullOrWhiteSpace(proveedor.Alias) ? null : proveedor.Alias.Trim();

                SincronizarTipos(proveedor, tiposProveedor);
                SincronizarTecnicas(proveedor, tecnicasPersonalizacion);

                _unitOfWork.Proveedores.Add(proveedor);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Proveedor creado correctamente", proveedor.IdProveedor);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el proveedor: {ex.Message}");
            }
        }

        /// <summary>
        /// Crea asincrónicamente proveedor.
        /// </summary>
        public async Task<ResultadoOperacion> CrearProveedorAsync(Proveedor proveedor, IEnumerable<Guid> tiposProveedor, IEnumerable<Guid> tecnicasPersonalizacion)
        {
            try
            {
                var resultadoValidacion = ValidarProveedor(proveedor, tiposProveedor, tecnicasPersonalizacion);
                if (!resultadoValidacion.EsValido)
                    return resultadoValidacion;

                var cuitLimpio = LimpiarCuit(proveedor.CUIT);
                if (await _unitOfWork.Proveedores.ExisteCUITAsync(cuitLimpio))
                    return ResultadoOperacion.Error($"Ya existe un proveedor con el CUIT {FormatearCuit(cuitLimpio)}");

                proveedor.IdProveedor = Guid.NewGuid();
                proveedor.CUIT = cuitLimpio;
                proveedor.FechaAlta = DateTime.Now;
                proveedor.Activo = true;
                proveedor.CondicionesPago = NormalizarCondicionPago(proveedor.CondicionesPago);
                proveedor.Observaciones = proveedor.Observaciones?.Trim();
                proveedor.Domicilio = proveedor.Domicilio?.Trim();
                proveedor.CodigoPostal = proveedor.CodigoPostal?.Trim();
                proveedor.Localidad = proveedor.Localidad?.Trim();
                proveedor.Alias = string.IsNullOrWhiteSpace(proveedor.Alias) ? null : proveedor.Alias.Trim();

                SincronizarTipos(proveedor, tiposProveedor);
                SincronizarTecnicas(proveedor, tecnicasPersonalizacion);

                _unitOfWork.Proveedores.Add(proveedor);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Proveedor creado correctamente", proveedor.IdProveedor);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el proveedor: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza proveedor.
        /// </summary>
        public ResultadoOperacion ActualizarProveedor(Proveedor proveedor, IEnumerable<Guid> tiposProveedor, IEnumerable<Guid> tecnicasPersonalizacion)
        {
            try
            {
                var resultadoValidacion = ValidarProveedor(proveedor, tiposProveedor, tecnicasPersonalizacion, esActualizacion: true);
                if (!resultadoValidacion.EsValido)
                    return resultadoValidacion;

                var proveedorExistente = _unitOfWork.Set<Proveedor>()
                    .Include(p => p.TecnicasPersonalizacion)
                    .Include(p => p.TiposProveedor)
                    .FirstOrDefault(p => p.IdProveedor == proveedor.IdProveedor);

                if (proveedorExistente == null)
                    return ResultadoOperacion.Error("El proveedor no existe");

                var cuitLimpio = LimpiarCuit(proveedor.CUIT);
                var proveedorConMismoCuit = _unitOfWork.Proveedores.GetProveedorPorCUIT(cuitLimpio);
                if (proveedorConMismoCuit != null && proveedorConMismoCuit.IdProveedor != proveedor.IdProveedor)
                    return ResultadoOperacion.Error($"Ya existe otro proveedor con el CUIT {FormatearCuit(cuitLimpio)}");

                proveedorExistente.RazonSocial = proveedor.RazonSocial.Trim();
                proveedorExistente.CUIT = cuitLimpio;
                proveedorExistente.IdCondicionIva = proveedor.IdCondicionIva;
                proveedorExistente.Domicilio = proveedor.Domicilio?.Trim();
                proveedorExistente.CodigoPostal = proveedor.CodigoPostal?.Trim();
                proveedorExistente.Localidad = proveedor.Localidad?.Trim();
                proveedorExistente.IdPais = proveedor.IdPais;
                proveedorExistente.IdProvincia = proveedor.IdProvincia;
                proveedorExistente.IdLocalidad = proveedor.IdLocalidad;
                proveedorExistente.CondicionesPago = NormalizarCondicionPago(proveedor.CondicionesPago);
                proveedorExistente.Observaciones = proveedor.Observaciones?.Trim();
                proveedorExistente.Activo = proveedor.Activo;
                proveedorExistente.Alias = string.IsNullOrWhiteSpace(proveedor.Alias) ? null : proveedor.Alias.Trim();

                SincronizarTipos(proveedorExistente, tiposProveedor);
                ActualizarTecnicas(proveedorExistente, tecnicasPersonalizacion);

                _unitOfWork.Proveedores.Update(proveedorExistente);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Proveedor actualizado correctamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el proveedor: {ex.Message}");
            }
        }

        /// <summary>
        /// Desactiva proveedor.
        /// </summary>
        public ResultadoOperacion DesactivarProveedor(Guid idProveedor)
        {
            try
            {
                if (idProveedor == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del proveedor no puede estar vacío");

                var proveedor = _unitOfWork.Proveedores.GetById(idProveedor);
                if (proveedor == null)
                    return ResultadoOperacion.Error("El proveedor no existe");

                if (!proveedor.Activo)
                    return ResultadoOperacion.Error("El proveedor ya está inactivo");

                _unitOfWork.Proveedores.DesactivarProveedor(idProveedor);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Proveedor desactivado correctamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al desactivar el proveedor: {ex.Message}");
            }
        }

        /// <summary>
        /// Activa proveedor.
        /// </summary>
        public ResultadoOperacion ActivarProveedor(Guid idProveedor)
        {
            try
            {
                if (idProveedor == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del proveedor no puede estar vacío");

                var proveedor = _unitOfWork.Proveedores.GetById(idProveedor);
                if (proveedor == null)
                    return ResultadoOperacion.Error("El proveedor no existe");

                if (proveedor.Activo)
                    return ResultadoOperacion.Error("El proveedor ya está activo");

                _unitOfWork.Proveedores.ActivarProveedor(idProveedor);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Proveedor activado correctamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al activar el proveedor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene tipos de proveedor.
        /// </summary>
        public IEnumerable<TipoProveedor> ObtenerTiposProveedor()
        {
            return _unitOfWork.TiposProveedor.GetTiposOrdenados();
        }

        /// <summary>
        /// Obtiene tipos de proveedor asincrónicamente.
        /// </summary>
        public async Task<IEnumerable<TipoProveedor>> ObtenerTiposProveedorAsync()
        {
            return await _unitOfWork.TiposProveedor.GetTiposOrdenadosAsync();
        }

        /// <summary>
        /// Obtiene técnicas de personalización.
        /// </summary>
        public IEnumerable<TecnicaPersonalizacion> ObtenerTecnicasPersonalizacion()
        {
            return _unitOfWork.Set<TecnicaPersonalizacion>()
                .OrderBy(t => t.NombreTecnicaPersonalizacion)
                .ToList();
        }

        /// <summary>
        /// Obtiene técnicas de personalización asincrónicamente.
        /// </summary>
        public async Task<IEnumerable<TecnicaPersonalizacion>> ObtenerTecnicasPersonalizacionAsync()
        {
            return await _unitOfWork.Set<TecnicaPersonalizacion>()
                .OrderBy(t => t.NombreTecnicaPersonalizacion)
                .ToListAsync();
        }

        /// <summary>
        /// Valida los datos del proveedor.
        /// </summary>
        private ResultadoOperacion ValidarProveedor(Proveedor proveedor, IEnumerable<Guid> tiposSeleccionados, IEnumerable<Guid> tecnicasSeleccionadas, bool esActualizacion = false)
        {
            if (proveedor == null)
                return ResultadoOperacion.Error("El proveedor no puede ser nulo");

            var razonSocial = proveedor.RazonSocial?.Trim();
            if (string.IsNullOrWhiteSpace(razonSocial) || razonSocial.Length < 3 || razonSocial.Length > 100)
                return ResultadoOperacion.Error("La razón social debe tener entre 3 y 100 caracteres");

            var (cuitValido, mensajeCuit) = ValidationHelper.ValidarCUIT(proveedor.CUIT);
            if (!cuitValido)
                return ResultadoOperacion.Error(mensajeCuit);

            if (proveedor.IdCondicionIva == Guid.Empty)
                return ResultadoOperacion.Error("La condición de IVA es obligatoria");

            if (!_unitOfWork.CondicionesIva.Existe(proveedor.IdCondicionIva))
                return ResultadoOperacion.Error("La condición de IVA seleccionada no existe");

            if (string.IsNullOrWhiteSpace(proveedor.CondicionesPago))
                return ResultadoOperacion.Error("Las condiciones de pago son obligatorias");

            if (!ProveedorCatalogoHelper.EsCondicionPagoValida(proveedor.CondicionesPago))
                return ResultadoOperacion.Error("Las condiciones de pago seleccionadas no son válidas");

            if (proveedor.IdPais == null || proveedor.IdPais == Guid.Empty)
                return ResultadoOperacion.Error("Debe seleccionar un país");

            if (proveedor.IdProvincia == null || proveedor.IdProvincia == Guid.Empty)
                return ResultadoOperacion.Error("Debe seleccionar una provincia");

            if (proveedor.IdLocalidad == null || proveedor.IdLocalidad == Guid.Empty)
                return ResultadoOperacion.Error("Debe seleccionar una localidad");

            var idsTipos = tiposSeleccionados?
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (idsTipos == null || !idsTipos.Any())
                return ResultadoOperacion.Error("Debe seleccionar al menos un tipo de proveedor");

            var tipos = _unitOfWork.Set<TipoProveedor>()
                .Where(tp => idsTipos.Contains(tp.IdTipoProveedor))
                .ToList();

            if (tipos.Count != idsTipos.Count)
                return ResultadoOperacion.Error("Alguno de los tipos de proveedor seleccionados no existe");

            if (tipos.Any(ProveedorCatalogoHelper.EsTipoPersonalizador))
            {
                if (tecnicasSeleccionadas == null || !tecnicasSeleccionadas.Any())
                    return ResultadoOperacion.Error("Debe seleccionar al menos una técnica de personalización");
            }

            if (!string.IsNullOrWhiteSpace(proveedor.Domicilio) && proveedor.Domicilio.Length > 150)
                return ResultadoOperacion.Error("El domicilio no puede superar los 150 caracteres");

            if (!string.IsNullOrWhiteSpace(proveedor.CodigoPostal) && proveedor.CodigoPostal.Length > 20)
                return ResultadoOperacion.Error("El código postal no puede superar los 20 caracteres");

            if (!string.IsNullOrWhiteSpace(proveedor.Observaciones) && proveedor.Observaciones.Length > 500)
                return ResultadoOperacion.Error("Las observaciones no pueden superar los 500 caracteres");

            if (!string.IsNullOrWhiteSpace(proveedor.Alias) && proveedor.Alias.Length > 100)
                return ResultadoOperacion.Error("El alias no puede superar los 100 caracteres");

            return ResultadoOperacion.Exitoso("Validación exitosa");
        }

        /// <summary>
        /// Normaliza la condición de pago.
        /// </summary>
        private static string NormalizarCondicionPago(string condicion)
        {
            if (string.IsNullOrWhiteSpace(condicion))
                return condicion;

            var encontrado = ProveedorCatalogoHelper.CondicionesPago
                .FirstOrDefault(cp => cp.Equals(condicion.Trim(), StringComparison.OrdinalIgnoreCase));

            return encontrado ?? condicion.Trim();
        }

        /// <summary>
        /// Sincroniza las técnicas de personalización del proveedor.
        /// </summary>
        private void SincronizarTecnicas(Proveedor proveedor, IEnumerable<Guid> tecnicasSeleccionadas)
        {
            if (proveedor == null)
                return;

            proveedor.TecnicasPersonalizacion.Clear();
            if (tecnicasSeleccionadas == null)
                return;

            var ids = tecnicasSeleccionadas
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (!ids.Any())
                return;

            var tecnicas = _unitOfWork.Set<TecnicaPersonalizacion>()
                .Where(t => ids.Contains(t.IdTecnicaPersonalizacion))
                .ToList();

            foreach (var tecnica in tecnicas)
            {
                proveedor.TecnicasPersonalizacion.Add(tecnica);
            }
        }

        /// <summary>
        /// Sincroniza los tipos de proveedor del proveedor.
        /// </summary>
        private void SincronizarTipos(Proveedor proveedor, IEnumerable<Guid> tiposSeleccionados)
        {
            if (proveedor == null)
                return;

            proveedor.TiposProveedor.Clear();

            if (tiposSeleccionados == null)
                return;

            var ids = tiposSeleccionados
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (!ids.Any())
                return;

            var tipos = _unitOfWork.Set<TipoProveedor>()
                .Where(tp => ids.Contains(tp.IdTipoProveedor))
                .ToList();

            foreach (var tipo in tipos)
            {
                proveedor.TiposProveedor.Add(tipo);
            }
        }

        /// <summary>
        /// Actualiza las técnicas de personalización del proveedor existente.
        /// </summary>
        private void ActualizarTecnicas(Proveedor proveedorExistente, IEnumerable<Guid> tecnicasSeleccionadas)
        {
            if (proveedorExistente == null)
                return;

            proveedorExistente.TecnicasPersonalizacion.Clear();

            if (tecnicasSeleccionadas == null)
                return;

            var ids = tecnicasSeleccionadas
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (!ids.Any())
                return;

            var tecnicas = _unitOfWork.Set<TecnicaPersonalizacion>()
                .Where(t => ids.Contains(t.IdTecnicaPersonalizacion))
                .ToList();

            foreach (var tecnica in tecnicas)
            {
                proveedorExistente.TecnicasPersonalizacion.Add(tecnica);
            }
        }

        /// <summary>
        /// Limpia el CUIT dejando solo los dígitos.
        /// </summary>
        private static string LimpiarCuit(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return string.Empty;

            return new string(cuit.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Formatea el CUIT en el formato XX-XXXXXXXX-X.
        /// </summary>
        private static string FormatearCuit(string cuit)
        {
            var limpio = LimpiarCuit(cuit);
            if (limpio.Length != 11)
                return limpio;

            return $"{limpio.Substring(0, 2)}-{limpio.Substring(2, 8)}-{limpio.Substring(10, 1)}";
        }
    }
}