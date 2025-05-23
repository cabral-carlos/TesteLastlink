using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseAPI.Models;
using EnterpriseAPI.Repositories;

namespace EnterpriseAPI.Business
{
    public class CreatorsBusiness : ICreatorsBusiness
    {
        private readonly ICreatorsRepository _creatorsRepository;

        public CreatorsBusiness(ICreatorsRepository creatorsRepository)
        {
            _creatorsRepository = creatorsRepository;
        }

        public async Task<bool> AddCreator(string name)
        {
            var creator = new Creator()
            {
                Name = name,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

           return await _creatorsRepository.Add(creator);
        }

        public async Task<bool> DeleteCreator(int id)
        {
            var creator = GetCreatorByID(id);
            if (creator == null)
            {
                return false;
            }

            return await _creatorsRepository.Delete(creator);
        }

        public IEnumerable<Creator> GetAll()
        {
            return _creatorsRepository.Get();
        }

        public Creator GetCreatorByID(int id)
        {
            return _creatorsRepository.GetByID(id);
        }
    }
}
