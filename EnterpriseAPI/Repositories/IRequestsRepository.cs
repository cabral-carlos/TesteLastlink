using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseAPI.Models;

namespace EnterpriseAPI.Repositories
{
    public interface IRequestsRepository
    {
        Task<bool> Create(Request request);
        IEnumerable<Request> GetByCreatorId(int creatorId);
        int CountPendingByCreatorId(int creatorId);
        Task<bool> Approve(Request request);
        Task<bool> Deny(Request request);
        Request GetById(int requestId);
    }
}
