using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseAPI.Models;

namespace EnterpriseAPI.Repositories
{
    public class RequestsRepository : IRequestsRepository
    {
        private readonly AppDBContext _context;

        public RequestsRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Approve(Request request)
        {
            request.Status = Status.Approved;
            request.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public int CountPendingByCreatorId(int creatorId)
        {
            return GetByCreatorId(creatorId).Where(x => x.Status == Status.Pending).Count();
        }

        public async Task<bool> Create(Request request)
        {
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            _context.Requests.Add(request);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Deny(Request request)
        {
            request.Status = Status.Denied;
            request.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public IEnumerable<Request> GetByCreatorId(int creatorId)
        {
            return _context.Requests.Where(x => x.CreatorId == creatorId).Include(x => x.Creator).ToList();
        }

        public Request GetById(int requestId)
        {
            return _context.Requests.Where(x => x.Id == requestId).FirstOrDefault();
        }
    }
}
