using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.VotePlaceModelService;
using Services.VoteProcessModelService;
using Services.CompromisingEvidenceModelService;
using Services.CompromisingEvidenceFileModelService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Vote.ViewComponents
{
    public class EvidenceEditViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}