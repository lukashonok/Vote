using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class Repository<T> : IRepository<T> where T : Base
    {
        private readonly ApplicationDbContext applicationDbContext;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            this.entities = applicationDbContext.Set<T>();
        }

        public T Get(int id)
        {
            switch (typeof(T).Name)
            {
                case "CompromisingEvidenceModel":
                    return entities
                        .Include("VotePlaceId")
                        .Include("UserId")
                        .SingleOrDefault(s => s.Id == id);
                case "VoteModel":
                    return entities
                        .Include("VotePlaceId")
                        .Include("UserId")
                        .Include("TargetId")
                        .Include("VoteProcessId")
                        .Include("PhoneNumberId")
                        .SingleOrDefault(s => s.Id == id);
                case "CompromisingEvidenceFileModel":
                    return entities
                        .Include("CompromisingEvidenceId")
                        .SingleOrDefault(s => s.Id == id);
                case "NotificationModel":
                    return entities
                        .Include("ApplicationGetterId")
                        .SingleOrDefault(s => s.Id == id);
                default:
                    return entities
                        .SingleOrDefault(s => s.Id == id);
            }
        }

        public IQueryable<T> GetAll()
        {
            switch (typeof(T).Name)
            {
                case "CompromisingEvidenceModel":
                    return entities
                        .Include("VotePlaceId")
                        .Include("UserId")
                        .AsQueryable();
                case "VoteModel":
                    return entities
                        .Include("VotePlaceId")
                        .Include("UserId")
                        .Include("TargetId")
                        .Include("VoteProcessId")
                        .Include("PhoneNumberId")
                        .AsQueryable();
                case "CompromisingEvidenceFileModel":
                    return entities
                        .Include("CompromisingEvidenceId")
                        .AsQueryable();
                case "NotificationModel":
                    return entities
                        .Include("ApplicationGetterId")
                        .AsQueryable();
                default:
                    return entities
                        .AsQueryable();
            }
        }

        public EntityEntry Insert(T entity)
        {
            var newEntity = entities.Add(entity);
            applicationDbContext.SaveChanges();
            return newEntity;
        }

        public void Update(T entity)
        {
            entities.Update(entity);
            applicationDbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            entities.Remove(entity);
            applicationDbContext.SaveChanges();
        }

        public void Remove(T entity)
        {
            entities.Remove(entity);
        }

        public void SaveChanges()
        {
            applicationDbContext.SaveChanges();
        }
    }
}
