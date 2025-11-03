using Services.BLL.Helpers;
using Services.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.BLL.Interfaces
{
    public interface IPerfilService
    {
        /* Consultas básicas */
        IEnumerable<Perfil> ObtenerTodos();
        IEnumerable<Perfil> ObtenerPerfilesActivos();
        Task<IEnumerable<Perfil>> ObtenerPerfilesActivosAsync();
        Perfil ObtenerPorId(Guid idPerfil);
        Perfil ObtenerPorNombre(string nombrePerfil);
        IEnumerable<Funcion> ObtenerFuncionesDisponibles();
        ResultadoOperacion CrearPerfil(Perfil perfil);
        ResultadoOperacion ActualizarPerfil(Perfil perfil);
        ResultadoOperacion ActivarPerfil(Guid idPerfil);
        ResultadoOperacion DesactivarPerfil(Guid idPerfil);
    }
}