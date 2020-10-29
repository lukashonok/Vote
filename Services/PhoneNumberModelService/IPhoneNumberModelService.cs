using Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.PhoneNumberModelService
{
    public interface IPhoneNumberModelService
    {
        PhoneNumberModel GetPhoneNumberModel(int id);
        IQueryable<PhoneNumberModel> GetPhoneNumberModels();
        EntityEntry InsertPhoneNumberModel(PhoneNumberModel category);
        void UpdatePhoneNumberModel(PhoneNumberModel category);
        void DeletePhoneNumberModel(int id);
    }
}
