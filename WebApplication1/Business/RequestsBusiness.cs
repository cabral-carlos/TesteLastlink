using System.Collections.Generic;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Business
{
    public class RequestsBusiness : IRequestsBusiness
    {
        private readonly IRequestsRepository _requestsRepository;

        public RequestsBusiness(IRequestsRepository requestsRepository)
        {
            _requestsRepository = requestsRepository;
        }

        public void ApproveRequest(Request request)
        {
            throw new System.NotImplementedException();
        }

        public void CreateRequest(Request request)
        {
            throw new System.NotImplementedException();
        }

        public void DenyRequest(Request request)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Request> GetRequestsByCreatorId(int creatorId)
        {
            throw new System.NotImplementedException();
        }

        public bool ValidatePendingRequestsByCreatorId(int creatorId)
        {
            throw new System.NotImplementedException();
        }
    }
}
