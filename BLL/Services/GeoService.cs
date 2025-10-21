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
        public GeoService(IUnitOfWork uow) { _uow = uow; }

        public IList<GeoDTO> ListarPaises()
        {
            return _uow.Set<Pais>()
                       .OrderBy(p => p.Nombre)
                       .Select(p => new GeoDTO { Id = p.IdPais, Nombre = p.Nombre })
                       .ToList();
        }

        public IList<GeoDTO> ListarProvinciasPorPais(Guid idPais)
        {
            return _uow.Set<Provincia>()
                       .Where(x => x.IdPais == idPais)
                       .OrderBy(x => x.Nombre)
                       .Select(x => new GeoDTO { Id = x.IdProvincia, Nombre = x.Nombre })
                       .ToList();
        }

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