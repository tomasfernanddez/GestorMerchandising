using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Implementations.Base;
using DAL.Interfaces.Principales;
using DomainModel;

namespace DAL.Implementations.Principales
{
    public class EfProveedorRepository : EfRepository<Proveedor>, IProveedorRepository
    {
        /// <summary>
        /// Inicializa el repositorio de proveedores con el contexto de datos indicado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework de la aplicación.</param>
        public EfProveedorRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Construye la consulta base incluyendo las relaciones necesarias para el proveedor.
        /// </summary>
        /// <returns>Consulta enriquecida para reutilizar en métodos públicos.</returns>
        private IQueryable<Proveedor> QueryBase()
        {
            return _dbSet
                .Include(p => p.TiposProveedor)
                .Include(p => p.Pais)
                .Include(p => p.Provincia)
                .Include(p => p.LocalidadRef)
                .Include(p => p.TecnicasPersonalizacion);
        }

        /// <summary>
        /// Obtiene los proveedores activos ordenados por razón social.
        /// </summary>
        /// <returns>Colección de proveedores activos.</returns>
        public IEnumerable<Proveedor> GetProveedoresActivos()
        {
            return QueryBase()
                .Where(p => p.Activo)
                .OrderBy(p => p.RazonSocial)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona los proveedores activos ordenados por razón social.
        /// </summary>
        /// <returns>Colección de proveedores activos.</returns>
        public async Task<IEnumerable<Proveedor>> GetProveedoresActivosAsync()
        {
            return await QueryBase()
                .Where(p => p.Activo)
                .OrderBy(p => p.RazonSocial)
                .ToListAsync();
        }

        /// <summary>
        /// Recupera los proveedores activos asociados a un tipo de proveedor específico.
        /// </summary>
        /// <param name="idTipoProveedor">Identificador del tipo de proveedor.</param>
        /// <returns>Proveedores que pertenecen al tipo indicado.</returns>
        public IEnumerable<Proveedor> GetProveedoresPorTipo(Guid idTipoProveedor)
        {
            return QueryBase()
                .Where(p => p.TiposProveedor.Any(tp => tp.IdTipoProveedor == idTipoProveedor) && p.Activo)
                .OrderBy(p => p.RazonSocial)
                .ToList();
        }

        /// <summary>
        /// Recupera de forma asíncrona los proveedores activos asociados a un tipo de proveedor específico.
        /// </summary>
        /// <param name="idTipoProveedor">Identificador del tipo de proveedor.</param>
        /// <returns>Proveedores que pertenecen al tipo indicado.</returns>
        public async Task<IEnumerable<Proveedor>> GetProveedoresPorTipoAsync(Guid idTipoProveedor)
        {
            return await QueryBase()
                .Where(p => p.TiposProveedor.Any(tp => tp.IdTipoProveedor == idTipoProveedor) && p.Activo)
                .OrderBy(p => p.RazonSocial)
                .ToListAsync();
        }

        /// <summary>
        /// Busca un proveedor por su CUIT normalizado.
        /// </summary>
        /// <param name="cuit">Número de CUIT ingresado por el usuario.</param>
        /// <returns>Proveedor encontrado o null.</returns>
        public Proveedor GetProveedorPorCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return null;

            var limpio = LimpiarCuit(cuit);
            return QueryBase().FirstOrDefault(p => p.CUIT == limpio);
        }

        /// <summary>
        /// Busca de forma asíncrona un proveedor por su CUIT normalizado.
        /// </summary>
        /// <param name="cuit">Número de CUIT ingresado por el usuario.</param>
        /// <returns>Proveedor encontrado o null.</returns>
        public async Task<Proveedor> GetProveedorPorCUITAsync(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return null;

            var limpio = LimpiarCuit(cuit);
            return await QueryBase().FirstOrDefaultAsync(p => p.CUIT == limpio);
        }

        /// <summary>
        /// Busca proveedores por razón social o alias aplicando coincidencias parciales.
        /// </summary>
        /// <param name="razonSocial">Texto a buscar.</param>
        /// <returns>Colección de proveedores coincidentes.</returns>
        public IEnumerable<Proveedor> BuscarPorRazonSocial(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return new List<Proveedor>();

            var termino = razonSocial.Trim().ToLower();
            return QueryBase()
                .Where(p => p.RazonSocial.ToLower().Contains(termino)
                        || (p.Alias != null && p.Alias.ToLower().Contains(termino)))
                .OrderBy(p => p.RazonSocial)
                .ToList();
        }

        /// <summary>
        /// Busca de forma asíncrona proveedores por razón social o alias.
        /// </summary>
        /// <param name="razonSocial">Texto a buscar.</param>
        /// <returns>Colección de proveedores coincidentes.</returns>
        public async Task<IEnumerable<Proveedor>> BuscarPorRazonSocialAsync(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return new List<Proveedor>();

            var termino = razonSocial.Trim().ToLower();
            return await QueryBase()
                .Where(p => p.RazonSocial.ToLower().Contains(termino)
                        || (p.Alias != null && p.Alias.ToLower().Contains(termino)))
                .OrderBy(p => p.RazonSocial)
                .ToListAsync();
        }

        /// <summary>
        /// Determina si existe un proveedor registrado con el CUIT indicado.
        /// </summary>
        /// <param name="cuit">Número de CUIT a verificar.</param>
        /// <returns>True si el CUIT ya existe.</returns>
        public bool ExisteCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            var limpio = LimpiarCuit(cuit);
            return _dbSet.Any(p => p.CUIT == limpio);
        }

        /// <summary>
        /// Determina de forma asíncrona si existe un proveedor registrado con el CUIT indicado.
        /// </summary>
        /// <param name="cuit">Número de CUIT a verificar.</param>
        /// <returns>True si el CUIT ya existe.</returns>
        public async Task<bool> ExisteCUITAsync(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            var limpio = LimpiarCuit(cuit);
            return await _dbSet.AnyAsync(p => p.CUIT == limpio);
        }

        /// <summary>
        /// Desactiva un proveedor marcándolo como inactivo.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        public void DesactivarProveedor(Guid idProveedor)
        {
            var proveedor = GetById(idProveedor);
            if (proveedor != null)
            {
                proveedor.Activo = false;
                Update(proveedor);
            }
        }

        /// <summary>
        /// Activa un proveedor marcándolo como disponible nuevamente.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        public void ActivarProveedor(Guid idProveedor)
        {
            var proveedor = GetById(idProveedor);
            if (proveedor != null)
            {
                proveedor.Activo = true;
                Update(proveedor);
            }
        }

        /// <summary>
        /// Realiza una búsqueda avanzada de proveedores aplicando múltiples filtros opcionales.
        /// </summary>
        /// <param name="razonSocial">Razon social o alias a buscar.</param>
        /// <param name="cuit">Número de CUIT parcial o completo.</param>
        /// <param name="idTipoProveedor">Tipo de proveedor requerido.</param>
        /// <param name="activo">Estado de actividad deseado.</param>
        /// <returns>Colección de proveedores que cumplen con los filtros.</returns>
        public IEnumerable<Proveedor> Buscar(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo)
        {
            var query = QueryBase();

            if (!string.IsNullOrWhiteSpace(razonSocial))
            {
                var termino = razonSocial.Trim().ToLower();
                query = query.Where(p => p.RazonSocial.ToLower().Contains(termino)
                                         || (p.Alias != null && p.Alias.ToLower().Contains(termino)));
            }

            if (!string.IsNullOrWhiteSpace(cuit))
            {
                var limpio = LimpiarCuit(cuit);
                query = query.Where(p => p.CUIT.Contains(limpio));
            }

            if (idTipoProveedor.HasValue && idTipoProveedor.Value != Guid.Empty)
            {
                var id = idTipoProveedor.Value;
                query = query.Where(p => p.TiposProveedor.Any(tp => tp.IdTipoProveedor == id));
            }

            if (activo.HasValue)
            {
                query = query.Where(p => p.Activo == activo.Value);
            }

            return query
                .OrderBy(p => p.RazonSocial)
                .ToList();
        }

        /// <summary>
        /// Realiza de forma asíncrona una búsqueda avanzada de proveedores aplicando múltiples filtros opcionales.
        /// </summary>
        /// <param name="razonSocial">Razon social o alias a buscar.</param>
        /// <param name="cuit">Número de CUIT parcial o completo.</param>
        /// <param name="idTipoProveedor">Tipo de proveedor requerido.</param>
        /// <param name="activo">Estado de actividad deseado.</param>
        /// <returns>Colección de proveedores que cumplen con los filtros.</returns>
        public async Task<IEnumerable<Proveedor>> BuscarAsync(string razonSocial, string cuit, Guid? idTipoProveedor, bool? activo)
        {
            var query = QueryBase();

            if (!string.IsNullOrWhiteSpace(razonSocial))
            {
                var termino = razonSocial.Trim().ToLower();
                query = query.Where(p => p.RazonSocial.ToLower().Contains(termino)
                                         || (p.Alias != null && p.Alias.ToLower().Contains(termino)));
            }

            if (!string.IsNullOrWhiteSpace(cuit))
            {
                var limpio = LimpiarCuit(cuit);
                query = query.Where(p => p.CUIT.Contains(limpio));
            }

            if (idTipoProveedor.HasValue && idTipoProveedor.Value != Guid.Empty)
            {
                var id = idTipoProveedor.Value;
                query = query.Where(p => p.TiposProveedor.Any(tp => tp.IdTipoProveedor == id));
            }

            if (activo.HasValue)
            {
                query = query.Where(p => p.Activo == activo.Value);
            }

            return await query
                .OrderBy(p => p.RazonSocial)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un proveedor incluyendo todas las relaciones de detalle.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        /// <returns>Proveedor con sus colecciones relacionadas.</returns>
        public Proveedor ObtenerConDetalles(Guid idProveedor)
        {
            return QueryBase().FirstOrDefault(p => p.IdProveedor == idProveedor);
        }

        /// <summary>
        /// Obtiene de forma asíncrona un proveedor incluyendo todas las relaciones de detalle.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor.</param>
        /// <returns>Proveedor con sus colecciones relacionadas.</returns>
        public async Task<Proveedor> ObtenerConDetallesAsync(Guid idProveedor)
        {
            return await QueryBase().FirstOrDefaultAsync(p => p.IdProveedor == idProveedor);
        }

        /// <summary>
        /// Normaliza un CUIT eliminando cualquier carácter que no sea numérico.
        /// </summary>
        /// <param name="cuit">Texto a normalizar.</param>
        /// <returns>CUIT compuesto únicamente por dígitos.</returns>
        private static string LimpiarCuit(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return string.Empty;

            return new string(cuit.Where(char.IsDigit).ToArray());
        }
    }
}