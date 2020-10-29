using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.TargetModelService
{
    public class TargetModelService : ITargetModelService
    {
        private readonly IRepository<TargetModel> repository;
        public TargetModelService(IRepository<TargetModel> repository)
        {
            this.repository = repository;
        }
        public void DeleteTargetModel(int id)
        {
            repository.Delete(GetTargetModel(id));
        }

        public IQueryable<TargetModel> GetTargetModels()
        {
            return repository.GetAll();
        }

        public TargetModel GetTargetModel(int id)
        {
            return repository.Get(id);
        }

        public EntityEntry InsertTargetModel(TargetModel targetModel)
        {
            return repository.Insert(targetModel);
        }

        public void UpdateTargetModel(TargetModel targetModel)
        {
            repository.Update(targetModel);
        }
    }
}
