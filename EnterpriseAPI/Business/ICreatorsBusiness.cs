using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseAPI.Models;

namespace EnterpriseAPI.Business
{
    public interface ICreatorsBusiness
    {
        Task<bool> AddCreator(string name);
        Task<bool> DeleteCreator(int id);
        IEnumerable<Creator> GetAll();
        Creator GetCreatorById(int id);       
    }
}
