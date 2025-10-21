using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BLL.Interfaces
{
    public interface IPerfilService
    {
        /* Consultas básicas */
        IEnumerable<Perfil> ObtenerPerfilesActivos();
        Task<IEnumerable<Perfil>> ObtenerPerfilesActivosAsync();
        Perfil ObtenerPorId(Guid idPerfil);
        Perfil ObtenerPorNombre(string nombrePerfil);
    }
}
