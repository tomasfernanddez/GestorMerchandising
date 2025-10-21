using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Base
{
    public interface IPerfilRepository : IRepository<Perfil>
    {
        // Métodos específicos para Perfil
        IEnumerable<Perfil> GetPerfilesActivos();
        Task<IEnumerable<Perfil>> GetPerfilesActivosAsync();

        Perfil GetPerfilPorNombre(string nombrePerfil);
        Task<Perfil> GetPerfilPorNombreAsync(string nombrePerfil);
    }
}
