using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.VotePlaceModelService
{
    public interface IVotePlaceModelService
    {
        VotePlaceModel GetVotePlaceModel(int id);
        IQueryable<VotePlaceModel> GetVotePlaceModels();
        EntityEntry InsertVotePlaceModel(VotePlaceModel category);
        void UpdateVotePlaceModel(VotePlaceModel category);
        void DeleteVotePlaceModel(int id);
    }
}
