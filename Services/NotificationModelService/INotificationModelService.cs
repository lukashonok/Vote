using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.TargetModelService
{
    public interface INotificationModelService
    {
        NotificationModel GetNotificationModel(int id);
        IQueryable<NotificationModel> GetNotificationModels();
        EntityEntry InsertNotificationModel(NotificationModel category);
        void UpdateNotificationModel(NotificationModel category);
        void DeleteNotificationModel(int id);
    }
}
