using Entities;
using System;
using System.Collections.Generic;
using Vote.Controllers;

namespace Vote.Areas.AdminPanel.Forms
{
    public class UserForm
    {
        public ApplicationUser User { get; set; }
        public List<EvidenceEntity> Evidences { get; set; }
    }
}
