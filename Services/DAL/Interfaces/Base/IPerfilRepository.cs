using Services.DomainModel.Entities;
using Services.DAL.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DAL.Interfaces.Base
{
    public interface IPerfilRepository : IRepository<Perfil>
    {
        // Métodos específicos para Perfil
        /// <summary>
        /// Obtiene los perfiles que se encuentran activos.
        /// </summary>
        IEnumerable<Perfil> GetPerfilesActivos();

        /// <summary>
        /// Obtiene de forma asíncrona los perfiles que se encuentran activos.
        /// </summary>
        Task<IEnumerable<Perfil>> GetPerfilesActivosAsync();

        /// <summary>
        /// Busca un perfil por su nombre.
        /// </summary>
        Perfil GetPerfilPorNombre(string nombrePerfil);

        /// <summary>
        /// Busca de forma asíncrona un perfil por su nombre.
        /// </summary>
        Task<Perfil> GetPerfilPorNombreAsync(string nombrePerfil);

        /// <summary>
        /// Obtiene todos los perfiles incluyendo sus funciones asociadas.
        /// </summary>
        IEnumerable<Perfil> GetAllWithFunciones();

        /// <summary>
        /// Obtiene un perfil por ID incluyendo sus funciones asociadas.
        /// </summary>
        Perfil GetByIdWithFunciones(Guid idPerfil);
    }
}