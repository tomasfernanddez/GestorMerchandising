using BLL.Helpers;
using BLL.Interfaces;
using DAL;
using DAL.Implementations.Base;
using DAL.Interfaces.Base;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Cliente ObtenerClientePorId(Guid idCliente)
        {
            if (idCliente == Guid.Empty)
                throw new ArgumentException("El ID del cliente no puede estar vacío", nameof(idCliente));

            return _unitOfWork.Clientes.GetById(idCliente);
        }

        public async Task<Cliente> ObtenerClientePorIdAsync(Guid idCliente)
        {
            if (idCliente == Guid.Empty)
                throw new ArgumentException("El ID del cliente no puede estar vacío", nameof(idCliente));

            return await _unitOfWork.Clientes.GetByIdAsync(idCliente);
        }

        public IEnumerable<Cliente> ObtenerClientesActivos()
        {
            return _unitOfWork.Clientes.GetClientesActivos();
        }

        public async Task<IEnumerable<Cliente>> ObtenerClientesActivosAsync()
        {
            return await _unitOfWork.Clientes.GetClientesActivosAsync();
        }

        public Cliente ObtenerClientePorCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                throw new ArgumentException("El CUIT no puede estar vacío", nameof(cuit));

            if (!ValidarFormatoCUIT(cuit))
                throw new ArgumentException("El formato del CUIT no es válido", nameof(cuit));

            return _unitOfWork.Clientes.GetClientePorCUIT(cuit);
        }

        public async Task<Cliente> ObtenerClientePorCUITAsync(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                throw new ArgumentException("El CUIT no puede estar vacío", nameof(cuit));

            if (!ValidarFormatoCUIT(cuit))
                throw new ArgumentException("El formato del CUIT no es válido", nameof(cuit));

            return await _unitOfWork.Clientes.GetClientePorCUITAsync(cuit);
        }

        public IEnumerable<Cliente> BuscarClientesPorRazonSocial(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return new List<Cliente>();

            return _unitOfWork.Clientes.BuscarPorRazonSocial(razonSocial);
        }

        public async Task<IEnumerable<Cliente>> BuscarClientesPorRazonSocialAsync(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return new List<Cliente>();

            return await _unitOfWork.Clientes.BuscarPorRazonSocialAsync(razonSocial);
        }

        public ResultadoOperacion CrearCliente(Cliente cliente)
        {
            try
            {
                NormalizarCliente(cliente);
                var validacion = ValidarCliente(cliente);
                if (!validacion.EsValido)
                    return validacion;

                if (_unitOfWork.Clientes.ExisteCUIT(cliente.CUIT))
                {
                    return ResultadoOperacion.Error($"Ya existe un cliente con el CUIT {FormatearCuit(cliente.CUIT)}");
                }

                cliente.IdCliente = Guid.NewGuid();
                cliente.Activo = true;

                _unitOfWork.Clientes.Add(cliente);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Cliente creado exitosamente", cliente.IdCliente);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el cliente: {ex.Message}");
            }
        }

        public async Task<ResultadoOperacion> CrearClienteAsync(Cliente cliente)
        {
            try
            {
                NormalizarCliente(cliente);
                var validacion = ValidarCliente(cliente);
                if (!validacion.EsValido)
                    return validacion;

                if (await _unitOfWork.Clientes.ExisteCUITAsync(cliente.CUIT))
                {
                    return ResultadoOperacion.Error($"Ya existe un cliente con el CUIT {FormatearCuit(cliente.CUIT)}");
                }

                cliente.IdCliente = Guid.NewGuid();
                cliente.Activo = true;

                _unitOfWork.Clientes.Add(cliente);
                await _unitOfWork.SaveChangesAsync();

                return ResultadoOperacion.Exitoso("Cliente creado exitosamente", cliente.IdCliente);
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al crear el cliente: {ex.Message}");
            }
        }

        public ResultadoOperacion ActualizarCliente(Cliente cliente)
        {
            try
            {
                NormalizarCliente(cliente);
                var validacion = ValidarCliente(cliente);
                if (!validacion.EsValido)
                    return validacion;

                var clienteExistente = _unitOfWork.Clientes.GetById(cliente.IdCliente);
                if (clienteExistente == null)
                {
                    return ResultadoOperacion.Error("El cliente no existe");
                }

                var clienteConMismoCUIT = _unitOfWork.Clientes.GetClientePorCUIT(cliente.CUIT);
                if (clienteConMismoCUIT != null && clienteConMismoCUIT.IdCliente != cliente.IdCliente)
                {
                    return ResultadoOperacion.Error($"Ya existe otro cliente con el CUIT {FormatearCuit(cliente.CUIT)}");
                }

                _unitOfWork.Clientes.Update(cliente);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Cliente actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al actualizar el cliente: {ex.Message}");
            }
        }

        public ResultadoOperacion DesactivarCliente(Guid idCliente)
        {
            try
            {
                if (idCliente == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del cliente no puede estar vacío");

                var cliente = _unitOfWork.Clientes.GetById(idCliente);
                if (cliente == null)
                    return ResultadoOperacion.Error("El cliente no existe");

                if (!cliente.Activo)
                    return ResultadoOperacion.Error("El cliente ya está desactivado");

                _unitOfWork.Clientes.DesactivarCliente(idCliente);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Cliente desactivado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al desactivar el cliente: {ex.Message}");
            }
        }

        public ResultadoOperacion ActivarCliente(Guid idCliente)
        {
            try
            {
                if (idCliente == Guid.Empty)
                    return ResultadoOperacion.Error("El ID del cliente no puede estar vacío");

                var cliente = _unitOfWork.Clientes.GetById(idCliente);
                if (cliente == null)
                    return ResultadoOperacion.Error("El cliente no existe");

                if (cliente.Activo)
                    return ResultadoOperacion.Error("El cliente ya está activo");

                _unitOfWork.Clientes.ActivarCliente(idCliente);
                _unitOfWork.SaveChanges();

                return ResultadoOperacion.Exitoso("Cliente activado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultadoOperacion.Error($"Error al activar el cliente: {ex.Message}");
            }
        }

        public IEnumerable<TipoEmpresa> ObtenerTiposEmpresa()
        {
            if (_unitOfWork.TiposEmpresa == null)
                return new List<TipoEmpresa>();

            return _unitOfWork.TiposEmpresa.GetTiposOrdenados();
        }

        public object ObtenerEstadisticasClientes()
        {
            var totalClientes = _unitOfWork.Clientes.Count();
            var clientesActivos = _unitOfWork.Clientes.Count(c => c.Activo == true);
            var clientesInactivos = totalClientes - clientesActivos;

            return new
            {
                TotalClientes = totalClientes,
                ClientesActivos = clientesActivos,
                ClientesInactivos = clientesInactivos,
                PorcentajeActivos = totalClientes > 0 ? (clientesActivos * 100.0 / totalClientes) : 0
            };
        }

        private void NormalizarCliente(Cliente cliente)
        {
            if (cliente == null)
                return;

            cliente.RazonSocial = cliente.RazonSocial?.Trim();
            cliente.Alias = string.IsNullOrWhiteSpace(cliente.Alias) ? null : cliente.Alias.Trim();

            if (!string.IsNullOrWhiteSpace(cliente.CUIT))
                cliente.CUIT = new string(cliente.CUIT.Where(char.IsDigit).ToArray());

            cliente.Domicilio = cliente.Domicilio?.Trim();
            cliente.Localidad = cliente.Localidad?.Trim();

        }

        private ResultadoOperacion ValidarCliente(Cliente cliente)
        {
            if (cliente == null)
                return ResultadoOperacion.Error("El cliente no puede ser nulo");

            if (string.IsNullOrWhiteSpace(cliente.RazonSocial))
                return ResultadoOperacion.Error("La razón social es obligatoria");

            if (cliente.RazonSocial.Length > 100)
                return ResultadoOperacion.Error("La razón social no puede superar los 100 caracteres");

            if (string.IsNullOrWhiteSpace(cliente.CUIT))
                return ResultadoOperacion.Error("El CUIT es obligatorio");

            if (!ValidarFormatoCUIT(cliente.CUIT))
                return ResultadoOperacion.Error("El formato del CUIT no es válido");

            if (cliente.IdCondicionIva == Guid.Empty)
                return ResultadoOperacion.Error("La condición de IVA es obligatoria");

            if (!_unitOfWork.CondicionesIva.Existe(cliente.IdCondicionIva))
                return ResultadoOperacion.Error("La condición de IVA seleccionada no existe");

            if (!string.IsNullOrWhiteSpace(cliente.Domicilio) && cliente.Domicilio.Length > 150)
                return ResultadoOperacion.Error("El domicilio no puede superar los 150 caracteres");

            if (!string.IsNullOrWhiteSpace(cliente.Alias) && cliente.Alias.Length > 100)
                return ResultadoOperacion.Error("El alias no puede superar los 100 caracteres");

            if (!string.IsNullOrWhiteSpace(cliente.Localidad) && cliente.Localidad.Length > 100)
                return ResultadoOperacion.Error("La localidad no puede superar los 100 caracteres");

            return ResultadoOperacion.Exitoso("Validación exitosa");
        }

        private static string FormatearCuit(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return string.Empty;

            var limpio = new string(cuit.Where(char.IsDigit).ToArray());
            if (limpio.Length != 11)
                return limpio;

            return $"{limpio.Substring(0, 2)}-{limpio.Substring(2, 8)}-{limpio.Substring(10, 1)}";
        }

        private bool ValidarFormatoCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            string cuitLimpio = cuit.Replace("-", "").Replace(" ", "");
            if (cuitLimpio.Length != 11)
                return false;

            if (!cuitLimpio.All(char.IsDigit))
                return false;

            return true;
        }
    }
}
