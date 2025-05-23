using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseAPI.Models;

namespace EnterpriseAPI.Repositories
{
    public interface ICreatorsRepository
    {
        Task<bool> Add(Creator Creator);
        Task<bool> Delete(Creator creator);
        IEnumerable<Creator> Get();
        Creator GetById(int id);       
    }
}
