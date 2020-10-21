using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Vote.Areas.Identity.Data;
using Vote.Controllers;

namespace Vote.Areas.AdminPanel.Forms
{
    public class UserForm
    {
        public ApplicationUser User { get; set; }
        public List<EvidenceEntity> Evidences { get; set; }
    }
}
