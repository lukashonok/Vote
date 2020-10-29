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
using Services.TargetModelService;
using Services.VotePlaceModelService;
using Services.VoteProcessModelService;
using Vote.Forms;
using Vote.Services;

namespace Vote.Controllers
{
    public class EvidenceController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVotePlaceModelService _votePlaceModelService;
        private readonly IVoteProcessModelService _voteProcessModelService;
        private readonly INotificationModelService _notificationModelService;
        private readonly ICompromisingEvidenceModelService _compromisingEvidenceModelService;
        private readonly ICompromisingEvidenceFileModelService _compromisingEvidenceFileModelService;

        public EvidenceController(
            IWebHostEnvironment appEnvironment,
            UserManager<ApplicationUser> userManager,
            IVotePlaceModelService votePlaceModelService,
            IVoteProcessModelService voteProcessModelService,
            INotificationModelService notificationModelService,
            ICompromisingEvidenceModelService compromisingEvidenceModelService,
            ICompromisingEvidenceFileModelService compromisingEvidenceFileModelService)
        {
            _userManager = userManager;
            _appEnvironment = appEnvironment;
            _votePlaceModelService = votePlaceModelService;
            _voteProcessModelService = voteProcessModelService;
            _notificationModelService = notificationModelService;
            _compromisingEvidenceModelService = compromisingEvidenceModelService;
            _compromisingEvidenceFileModelService = compromisingEvidenceFileModelService;            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditEvidence(int id)
        {
            ViewBag.Id = id;
            return View();
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
                CompromisingEvidenceModel evidence = new CompromisingEvidenceModel();
                int Id;
                if (evidenceForm.EvidenceId == 0)
                {
                    evidence.UserId = await _userManager.GetUserAsync(HttpContext.User);
                    evidence.VotePlaceId = _votePlaceModelService.GetVotePlaceModel(evidenceForm.PlaceId);
                    evidence.CreatedAt = DateTime.Now;
                    evidence.Comment = evidenceForm.Comment;
                    Id = (_compromisingEvidenceModelService.InsertCompromisingEvidenceModel(evidence).Entity as CompromisingEvidenceModel).Id;
                } 
                else
                {
                    evidence = _compromisingEvidenceModelService.GetCompromisingEvidenceModel(evidenceForm.EvidenceId);
                    Id = evidence.Id;
                    evidence.Comment = evidenceForm.Comment;
                    if ((await _userManager.GetUserAsync(HttpContext.User)) != evidence.UserId)
                    {
                        Notificator.EvidenceEdited(_notificationModelService, evidence);
                    }
                    _compromisingEvidenceModelService.UpdateCompromisingEvidenceModel(evidence);
                }
                if (evidenceForm.Files != null)
                {
                    List<int> oldFilesId = (
                        from F in _compromisingEvidenceFileModelService.GetCompromisingEvidenceFileModels()
                        where F.CompromisingEvidenceId.Id == Id
                        select F.Id
                    ).ToList();
                    foreach (int oldFileId in oldFilesId)
                    {
                        _compromisingEvidenceFileModelService.DeleteCompromisingEvidenceFileModel(oldFileId);
                    }

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
                            CompromisingEvidenceId = evidence
                        };
                        _compromisingEvidenceFileModelService.InsertCompromisingEvidenceFileModel(file);   
                    }
                }
            }
            return RedirectToAction(nameof(MapController.PlaceDetail), "Map", new { id = evidenceForm.PlaceId });
        }
    }
}