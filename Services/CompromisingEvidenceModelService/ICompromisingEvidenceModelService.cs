using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.CompromisingEvidenceModelService
{
    public interface ICompromisingEvidenceModelService
    {
        CompromisingEvidenceModel GetCompromisingEvidenceModel(int id);
        IQueryable<CompromisingEvidenceModel> GetCompromisingEvidenceModels();
        EntityEntry InsertCompromisingEvidenceModel(CompromisingEvidenceModel compromisingEvidence);
        void UpdateCompromisingEvidenceModel(CompromisingEvidenceModel compromisingEvidence);
        void DeleteCompromisingEvidenceModel(int id);
    }
}
