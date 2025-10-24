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
        public EfProveedorRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        private IQueryable<Proveedor> QueryBase()
        {
            return _dbSet
                .Include(p => p.TiposProveedor)
                .Include(p => p.Pais)
                .Include(p => p.Provincia)
                .Include(p => p.LocalidadRef)
                .Include(p => p.TecnicasPersonalizacion);
        }

        public IEnumerable<Proveedor> GetProveedoresActivos()
        {
            return QueryBase()
                .Where(p => p.Activo)
                .OrderBy(p => p.RazonSocial)
                .ToList();
        }

        public async Task<IEnumerable<Proveedor>> GetProveedoresActivosAsync()
        {
            return await QueryBase()
                .Where(p => p.Activo)
                .OrderBy(p => p.RazonSocial)
                .ToListAsync();
        }

        public IEnumerable<Proveedor> GetProveedoresPorTipo(Guid idTipoProveedor)
        {
            return QueryBase()
                .Where(p => p.TiposProveedor.Any(tp => tp.IdTipoProveedor == idTipoProveedor) && p.Activo)
                .OrderBy(p => p.RazonSocial)
                .ToList();
        }

        public async Task<IEnumerable<Proveedor>> GetProveedoresPorTipoAsync(Guid idTipoProveedor)
        {
            return await QueryBase()
                .Where(p => p.TiposProveedor.Any(tp => tp.IdTipoProveedor == idTipoProveedor) && p.Activo)
                .OrderBy(p => p.RazonSocial)
                .ToListAsync();
        }

        public Proveedor GetProveedorPorCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return null;

            var limpio = LimpiarCuit(cuit);
            return QueryBase().FirstOrDefault(p => p.CUIT == limpio);
        }

        public async Task<Proveedor> GetProveedorPorCUITAsync(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return null;

            var limpio = LimpiarCuit(cuit);
            return await QueryBase().FirstOrDefaultAsync(p => p.CUIT == limpio);
        }

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

        public bool ExisteCUIT(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            var limpio = LimpiarCuit(cuit);
            return _dbSet.Any(p => p.CUIT == limpio);
        }

        public async Task<bool> ExisteCUITAsync(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            var limpio = LimpiarCuit(cuit);
            return await _dbSet.AnyAsync(p => p.CUIT == limpio);
        }

        public void DesactivarProveedor(Guid idProveedor)
        {
            var proveedor = GetById(idProveedor);
            if (proveedor != null)
            {
                proveedor.Activo = false;
                Update(proveedor);
            }
        }

        public void ActivarProveedor(Guid idProveedor)
        {
            var proveedor = GetById(idProveedor);
            if (proveedor != null)
            {
                proveedor.Activo = true;
                Update(proveedor);
            }
        }

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

        public Proveedor ObtenerConDetalles(Guid idProveedor)
        {
            return QueryBase().FirstOrDefault(p => p.IdProveedor == idProveedor);
        }

        public async Task<Proveedor> ObtenerConDetallesAsync(Guid idProveedor)
        {
            return await QueryBase().FirstOrDefaultAsync(p => p.IdProveedor == idProveedor);
        }

        private static string LimpiarCuit(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return string.Empty;

            return new string(cuit.Where(char.IsDigit).ToArray());
        }
    }
}