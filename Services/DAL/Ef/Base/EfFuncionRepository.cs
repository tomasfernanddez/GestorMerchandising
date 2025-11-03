using Services.DAL.Interfaces.Base;
using Services.DomainModel.Entities;

namespace Services.DAL.Ef.Base
{
    public class EfFuncionRepository : EfRepository<Funcion>, IFuncionRepository
    {
        public EfFuncionRepository(ServicesContext context) : base(context)
        {
        }
    }
}