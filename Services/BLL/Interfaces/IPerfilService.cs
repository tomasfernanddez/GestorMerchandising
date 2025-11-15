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
        /// <summary>
        /// Obtiene todos los perfiles registrados.
        /// </summary>
        IEnumerable<Perfil> ObtenerTodos();

        /// <summary>
        /// Recupera únicamente los perfiles que están activos.
        /// </summary>
        IEnumerable<Perfil> ObtenerPerfilesActivos();

        /// <summary>
        /// Recupera de forma asíncrona los perfiles que están activos.
        /// </summary>
        Task<IEnumerable<Perfil>> ObtenerPerfilesActivosAsync();

        /// <summary>
        /// Busca un perfil a partir de su identificador.
        /// </summary>
        Perfil ObtenerPorId(Guid idPerfil);

        /// <summary>
        /// Obtiene un perfil por su nombre.
        /// </summary>
        Perfil ObtenerPorNombre(string nombrePerfil);

        /// <summary>
        /// Lista todas las funciones disponibles para asignar a perfiles.
        /// </summary>
        IEnumerable<Funcion> ObtenerFuncionesDisponibles();

        /// <summary>
        /// Crea un nuevo perfil con sus funciones asociadas.
        /// </summary>
        ResultadoOperacion CrearPerfil(Perfil perfil);

        /// <summary>
        /// Actualiza la información y funciones de un perfil existente.
        /// </summary>
        ResultadoOperacion ActualizarPerfil(Perfil perfil);

        /// <summary>
        /// Activa un perfil previamente desactivado.
        /// </summary>
        ResultadoOperacion ActivarPerfil(Guid idPerfil);

        /// <summary>
        /// Desactiva un perfil para impedir su asignación.
        /// </summary>
        ResultadoOperacion DesactivarPerfil(Guid idPerfil);
    }
}