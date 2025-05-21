using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class RequestsRepository : IRequestsRepository
    {
        private readonly AppDBContext _context;

        public RequestsRepository(AppDBContext context)
        {
            _context = context;
        }

        public void Approve(Request request)
        {
            request.Status = Status.Approved;
            request.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        }

        public int CountPendingByCreatorId(int creatorId)
        {
            return GetByCreatorId(creatorId).Where(x => x.Status == Status.Pending).Count();
        }

        public void Create(Request request)
        {
            _context.Requests.Add(request);
            _context.SaveChanges();
        }

        public void Deny(Request request)
        {
            request.Status = Status.Denied;
            request.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        }

        public IEnumerable<Request> GetByCreatorId(int creatorId)
        {
            return _context.Requests.Where(x => x.CreatorId == creatorId).ToList();
        }

        public Request GetById(int requestId)
        {
            return _context.Requests.Where(x => x.Id == requestId).FirstOrDefault();
        }
    }
}
