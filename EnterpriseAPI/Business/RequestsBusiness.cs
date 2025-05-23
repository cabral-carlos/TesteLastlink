using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseAPI.Models;
using EnterpriseAPI.Repositories;

namespace EnterpriseAPI.Business
{
    public class RequestsBusiness : IRequestsBusiness
    {
        private readonly IRequestsRepository _requestsRepository;
        private readonly ICreatorsRepository _creatorsRepository;
        private decimal anticipationFee = 5;

        public RequestsBusiness(IRequestsRepository requestsRepository, ICreatorsRepository creatorsRepository)
        {
            _requestsRepository = requestsRepository;
            _creatorsRepository = creatorsRepository;
        }

        public async Task<bool> ApproveRequest(int id)
        {
            var request = GetById(id);
            if (request == null)
            {
                return false;
            }

            if (request.Status != Status.Pending)
            {
                return false;
            }

            return await _requestsRepository.Approve(request);
        }

        public async Task<bool> CreateRequest(Request request)
        {
            if (ValidateNewRequest(request))
            {
                CalculateNetValue(request);
                return await _requestsRepository.Create(request);
            }

            return false;
        }

        public async Task<bool> DenyRequest(int id)
        {
            var request = GetById(id);
            if (request == null)
            {
                return false;
            }

            if (request.Status != Status.Pending)
            {
                return false;
            }

            return await _requestsRepository.Deny(request);
        }

        public Request GetById(int id)
        {
            return _requestsRepository.GetById(id);
        }

        public IEnumerable<Request> GetRequestsByCreatorId(int creatorId)
        {
            return _requestsRepository.GetByCreatorId(creatorId);
        }

        private bool ValidateNewRequest(Request request)
        {
            if (_creatorsRepository.GetById(request.CreatorId) == null)
            {
                return false;
            }

            if (_requestsRepository.CountPendingByCreatorId(request.CreatorId) > 1)
            {
                return false;
            }

            if (request.GrossValue < 100)
            {
                return false;
            }

            return true;
        }

        private void CalculateNetValue(Request request)
        {
            switch (request.Type)
            {
                case Models.Type.Regular:
                    request.Fee = 0;
                    request.NetValue = request.GrossValue - request.Fee;
                        break;
                case Models.Type.Anticipation:
                    request.Fee = anticipationFee;
                    request.NetValue = request.GrossValue - Math.Round(request.GrossValue * request.Fee / 100, 2);
                    break;
                default:
                    request.Fee = 0;
                    request.NetValue = request.GrossValue - request.Fee;
                    break;
            }
        }
    }
}
