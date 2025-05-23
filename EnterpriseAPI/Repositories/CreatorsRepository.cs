using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseAPI.Models;

namespace EnterpriseAPI.Repositories
{
    public class CreatorsRepository : ICreatorsRepository
    {
        private readonly AppDBContext _context;

        public CreatorsRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Creator creator)
        {
            _context.Creators.Add(creator);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Creator creator)
        {
            creator.IsActive = false;
            return await _context.SaveChangesAsync() > 0;
        }

        public IEnumerable<Creator> Get()
        {
            return _context.Creators.ToList();
        }

        public Creator GetById(int id)
        {
            var creator = _context.Creators.Where(x => x.Id == id).FirstOrDefault();
            return creator;
        }
    }
}
