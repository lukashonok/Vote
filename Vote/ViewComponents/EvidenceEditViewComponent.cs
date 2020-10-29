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
using Services.VoteModelService;
using Microsoft.AspNetCore.Identity;
using Services.TargetModelService;
using Vote.Forms;

namespace Vote.ViewComponents
{
    public class EvidenceEditViewComponent : ViewComponent
    {
        private readonly IVoteModelService _voteModelService;
        private readonly ITargetModelService _targetModelService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVotePlaceModelService _votePlaceModelService;
        private readonly IVoteProcessModelService _voteProcessModelService;
        private readonly ICompromisingEvidenceModelService _compromisingEvidenceModelService;
        private readonly ICompromisingEvidenceFileModelService _compromisingEvidenceFileModelService;

        public EvidenceEditViewComponent(
            IVoteModelService voteModelService,
            ITargetModelService targetModelService,
            UserManager<ApplicationUser> userManager,
            IVotePlaceModelService votePlaceModelService,
            IVoteProcessModelService voteProcessModelService,
            ICompromisingEvidenceModelService compromisingEvidenceModelService,
            ICompromisingEvidenceFileModelService compromisingEvidenceFileModelService
            )
        {
            _userManager = userManager;
            _voteModelService = voteModelService;
            _targetModelService = targetModelService;
            _votePlaceModelService = votePlaceModelService;
            _voteProcessModelService = voteProcessModelService;
            _compromisingEvidenceModelService = compromisingEvidenceModelService;
            _compromisingEvidenceFileModelService = compromisingEvidenceFileModelService;

        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            if (id != 0)
            {
                CompromisingEvidenceModel compromisingEvidenceModel = _compromisingEvidenceModelService.GetCompromisingEvidenceModel(id);
                var files = await (
                            from F in _compromisingEvidenceFileModelService.GetCompromisingEvidenceFileModels()
                            where F.CompromisingEvidenceId.Id == id
                            select F
                        ).ToListAsync();
                EvidenceForm evidenceForm = new EvidenceForm()
                {
                    Comment = compromisingEvidenceModel.Comment,
                    PlaceId = compromisingEvidenceModel.VotePlaceId.Id,
                    EvidenceId = compromisingEvidenceModel.Id
                };
                ViewBag.isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
                ViewBag.Files = files;
                ViewBag.Create = false;
                return View(evidenceForm);
            } else
            {
                ViewBag.Create = true;
                return View();
            }
        }
    }
}