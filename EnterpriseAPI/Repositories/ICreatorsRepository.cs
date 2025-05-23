using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseAPI.Models;

namespace EnterpriseAPI.Repositories
{
    public interface ICreatorsRepository
    {
        Task<bool> Add(Creator Creator);
        IEnumerable<Creator> Get();
        Creator GetByID(int id);
        Task<bool> Delete(Creator creator);
    }
}
