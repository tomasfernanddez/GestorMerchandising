using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICondicionIvaService
    {
        IEnumerable<CondicionIva> ObtenerTodas();
        Task<IEnumerable<CondicionIva>> ObtenerTodasAsync();
        CondicionIva ObtenerPorId(Guid id);
    }
}