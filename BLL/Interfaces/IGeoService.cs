using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGeoService
    {
        IList<GeoDTO> ListarPaises();
        IList<GeoDTO> ListarProvinciasPorPais(Guid idPais);
        IList<GeoDTO> ListarLocalidadesPorProvincia(Guid idProvincia);
    }

    public class GeoDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
    }
}
