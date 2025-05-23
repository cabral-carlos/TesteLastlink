using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseAPI.Models;

namespace EnterpriseAPI.Business
{
    public interface IRequestsBusiness
    {
        Task<bool> CreateRequest(Request request);
        Request GetById(int id);
        IEnumerable<Request> GetRequestsByCreatorId(int creatorId);
        Task<bool> ApproveRequest(int id);
        Task<bool> DenyRequest(int id);
    }
}
