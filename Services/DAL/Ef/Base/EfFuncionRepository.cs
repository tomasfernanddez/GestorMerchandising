using Services.DAL.Interfaces.Base;
using Services.DomainModel.Entities;

namespace Services.DAL.Ef.Base
{
    public class EfFuncionRepository : EfRepository<Funcion>, IFuncionRepository
    {
        /// <summary>
        /// Inicializa el repositorio de funciones con el contexto indicado.
        /// </summary>
        public EfFuncionRepository(ServicesContext context) : base(context)
        {
        }
    }
}