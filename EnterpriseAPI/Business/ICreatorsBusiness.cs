using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseAPI.Models;

namespace EnterpriseAPI.Business
{
    public interface ICreatorsBusiness
    {
        Task<bool> AddCreator(string name);
        IEnumerable<Creator> GetAll();
        Creator GetCreatorByID(int id);
        Task<bool> DeleteCreator(int id);
    }
}
