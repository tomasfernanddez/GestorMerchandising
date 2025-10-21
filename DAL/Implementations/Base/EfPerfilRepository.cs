using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations.Base
{
    public class EfPerfilRepository : EfRepository<Perfil>, IPerfilRepository
    {
        public EfPerfilRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene todos los perfiles activos
        /// </summary>
        public IEnumerable<Perfil> GetPerfilesActivos()
        {
            return _dbSet.Where(p => p.Activo == true)
                        .OrderBy(p => p.NombrePerfil)
                        .ToList();
        }

        /// <summary>
        /// Obtiene todos los perfiles activos (async)
        /// </summary>
        public async Task<IEnumerable<Perfil>> GetPerfilesActivosAsync()
        {
            return await _dbSet.Where(p => p.Activo == true)
                              .OrderBy(p => p.NombrePerfil)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene un perfil por su nombre
        /// </summary>
        public Perfil GetPerfilPorNombre(string nombrePerfil)
        {
            if (string.IsNullOrWhiteSpace(nombrePerfil))
                return null;

            return _dbSet.FirstOrDefault(p => p.NombrePerfil == nombrePerfil.Trim());
        }

        /// <summary>
        /// Obtiene un perfil por su nombre (async)
        /// </summary>
        public async Task<Perfil> GetPerfilPorNombreAsync(string nombrePerfil)
        {
            if (string.IsNullOrWhiteSpace(nombrePerfil))
                return null;

            return await _dbSet.FirstOrDefaultAsync(p => p.NombrePerfil == nombrePerfil.Trim());
        }
    }
}
