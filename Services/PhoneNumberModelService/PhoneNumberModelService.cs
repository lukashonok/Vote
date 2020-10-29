using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.PhoneNumberModelService
{
    public class PhoneNumberModelService : IPhoneNumberModelService
    {
        private readonly IRepository<PhoneNumberModel> repository;
        public PhoneNumberModelService(IRepository<PhoneNumberModel> repository)
        {
            this.repository = repository;
        }
        public void DeletePhoneNumberModel(int id)
        {
            repository.Delete(GetPhoneNumberModel(id));
        }

        public IQueryable<PhoneNumberModel> GetPhoneNumberModels()
        {
            return repository.GetAll();
        }

        public PhoneNumberModel GetPhoneNumberModel(int id)
        {
            return repository.Get(id);
        }

        public EntityEntry InsertPhoneNumberModel(PhoneNumberModel voteModel)
        {
            return repository.Insert(voteModel);
        }

        public void UpdatePhoneNumberModel(PhoneNumberModel voteModel)
        {
            repository.Update(voteModel);
        }
    }
}
