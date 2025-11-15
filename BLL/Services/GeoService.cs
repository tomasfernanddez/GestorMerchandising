using BLL.Interfaces;
using DAL;
using DAL.Extensions;
using DAL.Interfaces.Base;
using DomainModel;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class GeoService : IGeoService
    {
        private readonly IUnitOfWork _uow;
        /// <summary>
        /// Inicializa una nueva instancia de GeoService.
        /// </summary>
        public GeoService(IUnitOfWork uow) { _uow = uow; }
        /// <summary>
        /// Lista paises.
        /// </summary>
        public IList<GeoDTO> ListarPaises()
        {
            return _uow.Set<Pais>()
                       .OrderBy(p => p.Nombre)
                       .Select(p => new GeoDTO { Id = p.IdPais, Nombre = p.Nombre })
                       .ToList();
        }

        /// <summary>
        /// Lista provincias por pais.
        /// </summary>
        public IList<GeoDTO> ListarProvinciasPorPais(Guid idPais)
        {
            return _uow.Set<Provincia>()
                       .Where(x => x.IdPais == idPais)
                       .OrderBy(x => x.Nombre)
                       .Select(x => new GeoDTO { Id = x.IdProvincia, Nombre = x.Nombre })
                       .ToList();
        }

        /// <summary>
        /// Lista localidades por provincia.
        /// </summary>
        public IList<GeoDTO> ListarLocalidadesPorProvincia(Guid idProvincia)
        {
            return _uow.Set<Localidad>()
                       .Where(l => l.IdProvincia == idProvincia)
                       .OrderBy(l => l.Nombre)
                       .Select(l => new GeoDTO { Id = l.IdLocalidad, Nombre = l.Nombre })
                       .ToList();
        }
    }
}