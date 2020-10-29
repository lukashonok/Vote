using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.TargetModelService
{
    public class NotificationModelService : INotificationModelService
    {
        private readonly IRepository<NotificationModel> repository;
        public NotificationModelService(IRepository<NotificationModel> repository)
        {
            this.repository = repository;
        }
        public void DeleteNotificationModel(int id)
        {
            repository.Delete(GetNotificationModel(id));
        }

        public IQueryable<NotificationModel> GetNotificationModels()
        {
            return repository.GetAll();
        }

        public NotificationModel GetNotificationModel(int id)
        {
            return repository.Get(id);
        }

        public EntityEntry InsertNotificationModel(NotificationModel targetModel)
        {
            return repository.Insert(targetModel);
        }

        public void UpdateNotificationModel(NotificationModel targetModel)
        {
            repository.Update(targetModel);
        }
    }
}
