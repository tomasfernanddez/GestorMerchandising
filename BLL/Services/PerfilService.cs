using BLL.Interfaces;
using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PerfilService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Perfil> ObtenerPerfilesActivos()
        {
            try
            {
                return _unitOfWork.Perfiles.GetPerfilesActivos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener perfiles activos: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesActivosAsync()
        {
            try
            {
                return await _unitOfWork.Perfiles.GetPerfilesActivosAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener perfiles activos: {ex.Message}", ex);
            }
        }

        public Perfil ObtenerPorId(Guid idPerfil)
        {
            if (idPerfil == Guid.Empty)
                throw new ArgumentException("El ID del perfil no puede estar vacío", nameof(idPerfil));

            try
            {
                return _unitOfWork.Perfiles.GetById(idPerfil);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener perfil por ID: {ex.Message}", ex);
            }
        }

        public Perfil ObtenerPorNombre(string nombrePerfil)
        {
            if (string.IsNullOrWhiteSpace(nombrePerfil))
                throw new ArgumentException("El nombre del perfil no puede estar vacío", nameof(nombrePerfil));

            try
            {
                return _unitOfWork.Perfiles.GetPerfilPorNombre(nombrePerfil);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener perfil por nombre: {ex.Message}", ex);
            }
        }
    }
}
