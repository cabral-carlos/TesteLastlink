using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Business
{
    public interface ICreatorsBusiness
    {
        void AddCreator(Creator Creator);
        IEnumerable<Creator> GetAll();
        Creator GetCreatorByID(int id);
        void DeleteCreator(int id);
    }
}
