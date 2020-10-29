using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.VoteModelService
{
    public interface IVoteModelService
    {
        VoteModel GetVoteModel(int id);
        IQueryable<VoteModel> GetVoteModels();
        EntityEntry InsertVoteModel(VoteModel category);
        void UpdateVoteModel(VoteModel category);
        void DeleteVoteModel(int id);
    }
}
