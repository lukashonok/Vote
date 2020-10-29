using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.CompromisingEvidenceFileModelService
{
    public class CompromisingEvidenceFileModelService : ICompromisingEvidenceFileModelService
    {
        private readonly IRepository<CompromisingEvidenceFileModel> repository;
        public CompromisingEvidenceFileModelService(IRepository<CompromisingEvidenceFileModel> repository)
        {
            this.repository = repository;
        }
        public void DeleteCompromisingEvidenceFileModel(int id)
        {
            repository.Delete(GetCompromisingEvidenceFileModel(id));
        }

        public IQueryable<CompromisingEvidenceFileModel> GetCompromisingEvidenceFileModels()
        {
            return repository.GetAll();
        }

        public CompromisingEvidenceFileModel GetCompromisingEvidenceFileModel(int id)
        {
            return repository.Get(id);
        }

        public EntityEntry InsertCompromisingEvidenceFileModel(CompromisingEvidenceFileModel voteModel)
        {
            return repository.Insert(voteModel);
        }

        public void UpdateCompromisingEvidenceFileModel(CompromisingEvidenceFileModel voteModel)
        {
            repository.Update(voteModel);
        }
    }
}
