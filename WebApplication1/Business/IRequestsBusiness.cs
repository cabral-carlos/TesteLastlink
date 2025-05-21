using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Business
{
    public interface IRequestsBusiness
    {
        void CreateRequest(Request request);
        IEnumerable<Request> GetRequestsByCreatorId(int creatorId);
        bool ValidatePendingRequestsByCreatorId(int creatorId);
        void ApproveRequest(Request request);
        void DenyRequest(Request request);
    }
}
