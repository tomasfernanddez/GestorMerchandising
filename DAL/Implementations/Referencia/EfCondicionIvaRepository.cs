using DAL.Implementations.Base;
using DAL.Interfaces.Referencia;
using DomainModel.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Implementations.Referencia
{
    public class EfCondicionIvaRepository : EfRepository<CondicionIva>, ICondicionIvaRepository
    {
        public EfCondicionIvaRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        public bool Existe(Guid idCondicionIva)
        {
            return _dbSet.Any(ci => ci.IdCondicionIva == idCondicionIva);
        }

        public async Task<bool> ExisteAsync(Guid idCondicionIva)
        {
            return await _dbSet.AnyAsync(ci => ci.IdCondicionIva == idCondicionIva);
        }

        public CondicionIva ObtenerPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return null;

            var normalizado = nombre.Trim().ToLower();
            return _dbSet.FirstOrDefault(ci => ci.Nombre.ToLower() == normalizado);
        }

        public IEnumerable<CondicionIva> ObtenerTodas()
        {
            return _dbSet
                .OrderBy(ci => ci.Nombre)
                .ToList();
        }

        public async Task<IEnumerable<CondicionIva>> ObtenerTodasAsync()
        {
            return await _dbSet
                .OrderBy(ci => ci.Nombre)
                .ToListAsync();
        }
    }
}