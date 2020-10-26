using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Services.CompromisingEvidenceFileModelService;
using Services.CompromisingEvidenceModelService;
using Services.VotePlaceModelService;
using Services.VoteProcessModelService;
using Vote.Forms;

namespace Vote.Controllers
{
    public class EvidenceController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IVotePlaceModelService _votePlaceModelService;
        private readonly IVoteProcessModelService _voteProcessModelService;
        private readonly ICompromisingEvidenceModelService _compromisingEvidenceModelService;
        private readonly ICompromisingEvidenceFileModelService _compromisingEvidenceFileModelService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EvidenceController(
            
            IWebHostEnvironment appEnvironment,
            UserManager<ApplicationUser> userManager,
            IVotePlaceModelService votePlaceModelService,
            IVoteProcessModelService voteProcessModelService,
            ICompromisingEvidenceModelService compromisingEvidenceModelService,
            ICompromisingEvidenceFileModelService compromisingEvidenceFileModelService)
        {
            _userManager = userManager;
            _appEnvironment = appEnvironment;
            _votePlaceModelService = votePlaceModelService;
            _voteProcessModelService = voteProcessModelService;
            _compromisingEvidenceModelService = compromisingEvidenceModelService;
            _compromisingEvidenceFileModelService = compromisingEvidenceFileModelService;            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditEvidence(int id)
        {
            CompromisingEvidenceModel compromisingEvidenceModel = _compromisingEvidenceModelService.GetCompromisingEvidenceModel(id);
            var files = (
                        from F in _compromisingEvidenceFileModelService.GetCompromisingEvidenceFileModels()
                        where F.CompromisingEvidenceId.Id == id
                        select F
                    ) as IFormFileCollection;
            EvidenceForm evidenceForm = new EvidenceForm()
            {
                Comment = compromisingEvidenceModel.Comment,
                PlaceId = compromisingEvidenceModel.VotePlaceId.Id,
                Files = files
            };
            return View(evidenceForm);
        }

            [HttpPost]
        public async Task<IActionResult> EditEvidence(EvidenceForm evidenceForm)
        {
            if (evidenceForm.Files != null)
            {
                foreach (var uploadedFile in evidenceForm.Files)
                {
                    if (uploadedFile.Length > 10097152)
                    {
                        ModelState.AddModelError("Прикрепляемые файлы", $"Размер {uploadedFile.FileName} превышает 10 мб");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var evidence = new CompromisingEvidenceModel
                {
                    CreatedAt = DateTime.Now,
                    Comment = evidenceForm.Comment,
                    UserId = await _userManager.GetUserAsync(User),
                    VotePlaceId = _votePlaceModelService.GetVotePlaceModel(evidenceForm.PlaceId)
                };
                int Id = (_compromisingEvidenceModelService.InsertCompromisingEvidenceModel(evidence).Entity as CompromisingEvidenceModel).Id;
                if (evidenceForm.Files != null)
                {
                    var compromisingEvidenceForFiles = _compromisingEvidenceModelService.GetCompromisingEvidenceModel(Id);
                    foreach (var uploadedFile in evidenceForm.Files)
                    {
                        string path = "/Files/" + $@"{Guid.NewGuid()}" + uploadedFile.FileName;

                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }
                        CompromisingEvidenceFileModel file = new CompromisingEvidenceFileModel { 
                            Name = uploadedFile.FileName, 
                            Path = path,
                            CompromisingEvidenceId = compromisingEvidenceForFiles
                        };
                        _compromisingEvidenceFileModelService.InsertCompromisingEvidenceFileModel(file);   
                    }
                }
            }
            return RedirectToAction(nameof(MapController.Index));
        }
    }
}