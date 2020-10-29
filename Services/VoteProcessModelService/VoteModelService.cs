using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.VoteProcessModelService
{
    public class VoteProcessModelService : IVoteProcessModelService
    {
        private readonly IRepository<VoteProcessModel> repository;
        public VoteProcessModelService(IRepository<VoteProcessModel> repository)
        {
            this.repository = repository;
        }
        public void DeleteVoteProcessModel(int id)
        {
            repository.Delete(GetVoteProcessModel(id));
        }

        public IQueryable<VoteProcessModel> GetVoteProcessModels()
        {
            return repository.GetAll();
        }

        public VoteProcessModel GetVoteProcessModel(int id)
        {
            return repository.Get(id);
        }

        public EntityEntry InsertVoteProcessModel(VoteProcessModel voteModel)
        {
            return repository.Insert(voteModel);
        }

        public void UpdateVoteProcessModel(VoteProcessModel voteModel)
        {
            repository.Update(voteModel);
        }
    }
}
