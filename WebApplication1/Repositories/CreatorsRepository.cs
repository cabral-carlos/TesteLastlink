using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CreatorsRepository : ICreatorsRepository
    {
        private readonly AppDBContext _context;

        public CreatorsRepository(AppDBContext context)
        {
            _context = context;
        }

        public void Add(Creator creator)
        {
            _context.Creators.Add(creator);
            _context.SaveChanges();
        }

        public void Delete(Creator creator)
        {
            creator.IsActive = false;
            _context.SaveChanges();
        }

        public IEnumerable<Creator> Get()
        {
            return _context.Creators.ToList();
        }

        public Creator GetByID(int id)
        {
            var creator = _context.Creators.Where(x => x.Id == id).FirstOrDefault();
            return creator;
        }
    }
}
