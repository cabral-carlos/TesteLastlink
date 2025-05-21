using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IRequestsRepository
    {
        void Create(Request request);
        IEnumerable<Request> GetByCreatorId(int creatorId);
        int CountPendingByCreatorId(int creatorId);
        void Approve(Request request);
        void Deny(Request request);
        Request GetById(int requestId);
    }
}
