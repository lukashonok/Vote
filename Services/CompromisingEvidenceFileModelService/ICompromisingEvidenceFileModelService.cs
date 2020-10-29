using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.CompromisingEvidenceFileModelService
{
    public interface ICompromisingEvidenceFileModelService
    {
        CompromisingEvidenceFileModel GetCompromisingEvidenceFileModel(int id);
        IQueryable<CompromisingEvidenceFileModel> GetCompromisingEvidenceFileModels();
        EntityEntry InsertCompromisingEvidenceFileModel(CompromisingEvidenceFileModel category);
        void UpdateCompromisingEvidenceFileModel(CompromisingEvidenceFileModel category);
        void DeleteCompromisingEvidenceFileModel(int id);
    }
}
