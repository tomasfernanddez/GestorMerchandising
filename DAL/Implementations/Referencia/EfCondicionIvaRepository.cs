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
        /// <summary>
        /// Inicializa el repositorio de condiciones de IVA con el contexto de datos proporcionado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfCondicionIvaRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Verifica si existe una condición de IVA con el identificador indicado.
        /// </summary>
        /// <param name="idCondicionIva">Identificador de la condición de IVA.</param>
        /// <returns>True si la condición existe.</returns>
        public bool Existe(Guid idCondicionIva)
        {
            return _dbSet.Any(ci => ci.IdCondicionIva == idCondicionIva);
        }

        /// <summary>
        /// Verifica de forma asíncrona si existe una condición de IVA con el identificador indicado.
        /// </summary>
        /// <param name="idCondicionIva">Identificador de la condición de IVA.</param>
        /// <returns>True si la condición existe.</returns>
        public async Task<bool> ExisteAsync(Guid idCondicionIva)
        {
            return await _dbSet.AnyAsync(ci => ci.IdCondicionIva == idCondicionIva);
        }

        /// <summary>
        /// Obtiene una condición de IVA por su nombre normalizado.
        /// </summary>
        /// <param name="nombre">Nombre de la condición de IVA.</param>
        /// <returns>Condición encontrada o null.</returns>
        public CondicionIva ObtenerPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return null;

            var normalizado = nombre.Trim().ToLower();
            return _dbSet.FirstOrDefault(ci => ci.Nombre.ToLower() == normalizado);
        }

        /// <summary>
        /// Obtiene todas las condiciones de IVA ordenadas por nombre.
        /// </summary>
        /// <returns>Colección de condiciones de IVA.</returns>
        public IEnumerable<CondicionIva> ObtenerTodas()
        {
            return _dbSet
                .OrderBy(ci => ci.Nombre)
                .ToList();
        }

        /// <summary>
        /// Obtiene de forma asíncrona todas las condiciones de IVA ordenadas por nombre.
        /// </summary>
        /// <returns>Colección de condiciones de IVA.</returns>
        public async Task<IEnumerable<CondicionIva>> ObtenerTodasAsync()
        {
            return await _dbSet
                .OrderBy(ci => ci.Nombre)
                .ToListAsync();
        }
    }
}