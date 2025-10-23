using DAL.Interfaces.Base;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces.Referencia
{
    public interface ICondicionIvaRepository : IRepository<CondicionIva>
    {
        IEnumerable<CondicionIva> ObtenerTodas();
        Task<IEnumerable<CondicionIva>> ObtenerTodasAsync();
        bool Existe(Guid idCondicionIva);
        Task<bool> ExisteAsync(Guid idCondicionIva);
        CondicionIva ObtenerPorNombre(string nombre);
    }
}