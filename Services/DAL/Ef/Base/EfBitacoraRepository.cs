using Services.DAL.Interfaces.Base;
using Services.DomainModel.Entities;
using Services.DAL.Ef.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.DAL.Ef.Base
{
    public class EfBitacoraRepository : EfRepository<Bitacora>, IBitacoraRepository
    {
        /// <summary>
        /// Inicializa el repositorio de bitácora con el contexto proporcionado.
        /// </summary>
        public EfBitacoraRepository(ServicesContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene bitácoras por usuario
        /// </summary>
        public IEnumerable<Bitacora> GetBitacorasPorUsuario(Guid idUsuario)
        {
            return _dbSet.Where(b => b.IdUsuario == idUsuario)
                        .Include(b => b.Usuario)
                        .OrderByDescending(b => b.Fecha)
                        .ToList();
        }

        /// <summary>
        /// Obtiene bitácoras por usuario (async)
        /// </summary>
        public async Task<IEnumerable<Bitacora>> GetBitacorasPorUsuarioAsync(Guid idUsuario)
        {
            return await _dbSet.Where(b => b.IdUsuario == idUsuario)
                              .Include(b => b.Usuario)
                              .OrderByDescending(b => b.Fecha)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene bitácoras por rango de fechas
        /// </summary>
        public IEnumerable<Bitacora> GetBitacorasPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _dbSet.Where(b => b.Fecha >= fechaDesde && b.Fecha <= fechaHasta)
                        .Include(b => b.Usuario)
                        .OrderByDescending(b => b.Fecha)
                        .ToList();
        }

        /// <summary>
        /// Obtiene bitácoras por rango de fechas (async)
        /// </summary>
        public async Task<IEnumerable<Bitacora>> GetBitacorasPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            return await _dbSet.Where(b => b.Fecha >= fechaDesde && b.Fecha <= fechaHasta)
                              .Include(b => b.Usuario)
                              .OrderByDescending(b => b.Fecha)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene bitácoras por módulo
        /// </summary>
        public IEnumerable<Bitacora> GetBitacorasPorModulo(string modulo)
        {
            if (string.IsNullOrWhiteSpace(modulo))
                return new List<Bitacora>();

            return _dbSet.Where(b => b.Modulo == modulo)
                        .Include(b => b.Usuario)
                        .OrderByDescending(b => b.Fecha)
                        .ToList();
        }

        /// <summary>
        /// Obtiene bitácoras por módulo (async)
        /// </summary>
        public async Task<IEnumerable<Bitacora>> GetBitacorasPorModuloAsync(string modulo)
        {
            if (string.IsNullOrWhiteSpace(modulo))
                return new List<Bitacora>();

            return await _dbSet.Where(b => b.Modulo == modulo)
                              .Include(b => b.Usuario)
                              .OrderByDescending(b => b.Fecha)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene bitácoras con errores
        /// </summary>
        public IEnumerable<Bitacora> GetBitacorasConError()
        {
            return _dbSet.Where(b => b.Exitoso == false)
                        .Include(b => b.Usuario)
                        .OrderByDescending(b => b.Fecha)
                        .ToList();
        }

        /// <summary>
        /// Obtiene bitácoras con errores (async)
        /// </summary>
        public async Task<IEnumerable<Bitacora>> GetBitacorasConErrorAsync()
        {
            return await _dbSet.Where(b => b.Exitoso == false)
                              .Include(b => b.Usuario)
                              .OrderByDescending(b => b.Fecha)
                              .ToListAsync();
        }

        /// <summary>
        /// Registra una acción en la bitácora
        /// </summary>
        public void RegistrarAccion(Guid idUsuario, string accion, string descripcion, string modulo = null, bool exitoso = true, string mensajeError = null, string direccionIP = null)
        {
            var bitacora = new Bitacora
            {
                IdUsuario = idUsuario,
                Fecha = DateTime.Now,
                Accion = accion,
                Descripcion = descripcion,
                Modulo = modulo,
                DireccionIP = direccionIP,
                Exitoso = exitoso,
                MensajeError = mensajeError
            };

            Add(bitacora);
        }
    }
}