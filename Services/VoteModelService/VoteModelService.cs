using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.VoteModelService
{
    public class VoteModelService : IVoteModelService
    {
        private readonly IRepository<VoteModel> repository;
        public VoteModelService(IRepository<VoteModel> repository)
        {
            this.repository = repository;
        }
        public void DeleteVoteModel(int id)
        {
            repository.Delete(GetVoteModel(id));
        }

        public IQueryable<VoteModel> GetVoteModels()
        {
            return repository.GetAll();
        }

        public VoteModel GetVoteModel(int id)
        {
            return repository.Get(id);
        }

        public EntityEntry InsertVoteModel(VoteModel voteModel)
        {
            return repository.Insert(voteModel);
        }

        public void UpdateVoteModel(VoteModel voteModel)
        {
            repository.Update(voteModel);
        }
    }
}
