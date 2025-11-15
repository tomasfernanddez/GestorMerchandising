using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGeoService
    {
        // <summary>
        /// Lista paises.
        /// </summary>
        IList<GeoDTO> ListarPaises();
        /// <summary>
        /// Lista provincias por pais.
        /// </summary>
        IList<GeoDTO> ListarProvinciasPorPais(Guid idPais);
        /// <summary>
        /// Lista localidades por provincia.
        /// </summary>
        IList<GeoDTO> ListarLocalidadesPorProvincia(Guid idProvincia);
    }

    public class GeoDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
    }
}
