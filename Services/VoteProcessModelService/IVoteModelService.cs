using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.VoteProcessModelService
{
    public interface IVoteProcessModelService
    {
        VoteProcessModel GetVoteProcessModel(int id);
        IQueryable<VoteProcessModel> GetVoteProcessModels();
        EntityEntry InsertVoteProcessModel(VoteProcessModel category);
        void UpdateVoteProcessModel(VoteProcessModel category);
        void DeleteVoteProcessModel(int id);
    }
}
