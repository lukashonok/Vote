using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.TargetModelService
{
    public interface ITargetModelService
    {
        TargetModel GetTargetModel(int id);
        IQueryable<TargetModel> GetTargetModels();
        EntityEntry InsertTargetModel(TargetModel category);
        void UpdateTargetModel(TargetModel category);
        void DeleteTargetModel(int id);
    }
}
