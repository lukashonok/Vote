using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.CompromisingEvidenceModelService
{
    public class CompromisingEvidenceModelService : ICompromisingEvidenceModelService
    {
        private readonly IRepository<CompromisingEvidenceModel> repository;
        public CompromisingEvidenceModelService(IRepository<CompromisingEvidenceModel> repository)
        {
            this.repository = repository;
        }
        public void DeleteCompromisingEvidenceModel(int id)
        {
            repository.Delete(GetCompromisingEvidenceModel(id));
        }

        public IQueryable<CompromisingEvidenceModel> GetCompromisingEvidenceModels()
        {
            return repository.GetAll();
        }

        public CompromisingEvidenceModel GetCompromisingEvidenceModel(int id)
        {
            return repository.Get(id);
        }

        public EntityEntry InsertCompromisingEvidenceModel(CompromisingEvidenceModel voteModel)
        {
            return repository.Insert(voteModel);
        }

        public void UpdateCompromisingEvidenceModel(CompromisingEvidenceModel voteModel)
        {
            repository.Update(voteModel);
        }
    }
}
