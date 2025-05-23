using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseAPI.Models;

namespace EnterpriseAPI.Repositories
{
    public interface IRequestsRepository
    {
        Task<bool> Approve(Request request);
        int CountPendingByCreatorId(int creatorId);
        Task<bool> Create(Request request);
        Task<bool> Deny(Request request);
        IEnumerable<Request> GetByCreatorId(int creatorId);                   
        Request GetById(int requestId);
    }
}
