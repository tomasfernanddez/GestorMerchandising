using DAL;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LocalizacionService
    {
        private readonly GestorMerchandisingContext _ctx;
        public LocalizacionService(GestorMerchandisingContext ctx) { _ctx = ctx; }

        public List<Pais> ListarPaises() =>
            _ctx.Paises.OrderBy(x => x.Nombre).ToList();

        public List<Provincia> ListarProvincias(Guid idPais) =>
            _ctx.Provincias.Where(p => p.IdPais == idPais)
                .OrderBy(p => p.Nombre).ToList();

        public List<Localidad> ListarLocalidades(Guid idProvincia) =>
            _ctx.Localidades.Where(l => l.IdProvincia == idProvincia)
                .OrderBy(l => l.Nombre).ToList();
    }
}
