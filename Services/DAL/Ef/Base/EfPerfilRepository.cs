using Services.DAL.Interfaces.Base;
using Services.DomainModel.Entities;
using Services.DAL.Ef.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DAL.Ef.Base
{
    public class EfPerfilRepository : EfRepository<Perfil>, IPerfilRepository
    {
        /// <summary>
        /// Inicializa el repositorio de perfiles con el contexto especificado.
        /// </summary>
        public EfPerfilRepository(ServicesContext context) : base(context)
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

        /// <summary>
        /// Obtiene todos los perfiles incluyendo sus funciones asociadas.
        /// </summary>
        public IEnumerable<Perfil> GetAllWithFunciones()
        {
            return _dbSet
                .Include(p => p.Funciones)
                .ToList();
        }

        /// <summary>
        /// Obtiene un perfil por ID incluyendo sus funciones asociadas.
        /// </summary>
        public Perfil GetByIdWithFunciones(Guid idPerfil)
        {
            if (idPerfil == Guid.Empty)
            {
                return null;
            }

            return _dbSet
                .Include(p => p.Funciones)
                .FirstOrDefault(p => p.IdPerfil == idPerfil);
        }
    }
}
