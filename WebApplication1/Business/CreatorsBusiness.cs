using System;
using System.Collections.Generic;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Business
{
    public class CreatorsBusiness : ICreatorsBusiness
    {
        private readonly ICreatorsRepository _creatorsRepository;

        public CreatorsBusiness(ICreatorsRepository creatorsRepository)
        {
            _creatorsRepository = creatorsRepository;
        }

        public void AddCreator(Creator Creator)
        {
            throw new NotImplementedException();
        }

        public void DeleteCreator(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Creator> GetAll()
        {
            throw new NotImplementedException();
        }

        public Creator GetCreatorByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
