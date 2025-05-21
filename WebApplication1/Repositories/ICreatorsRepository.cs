using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface ICreatorsRepository
    {
        void Add(Creator Creator);
        IEnumerable<Creator> Get();
        Creator GetByID(int id);
        void Delete(Creator creator);
    }
}
