using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.VotePlaceModelService
{
    public class VotePlaceModelService : IVotePlaceModelService
    {
        private readonly IRepository<VotePlaceModel> repository;
        public VotePlaceModelService(IRepository<VotePlaceModel> repository)
        {
            this.repository = repository;
        }
        public void DeleteVotePlaceModel(int id)
        {
            repository.Delete(GetVotePlaceModel(id));
        }

        public IQueryable<VotePlaceModel> GetVotePlaceModels()
        {
            return repository.GetAll();
        }

        public VotePlaceModel GetVotePlaceModel(int id)
        {
            return repository.Get(id);
        }

        public EntityEntry InsertVotePlaceModel(VotePlaceModel voteModel)
        {
            return repository.Insert(voteModel);
        }

        public void UpdateVotePlaceModel(VotePlaceModel voteModel)
        {
            repository.Update(voteModel);
        }
    }
}
