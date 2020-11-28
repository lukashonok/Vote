using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.CompromisingEvidenceFileModelService;
using Services.CompromisingEvidenceModelService;
using Services.TargetModelService;
using Services.VoteProcessModelService;
using Vote.Areas.AdminPanel.Forms;
using Vote.Services;

namespace Vote.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVoteProcessModelService _voteProcessModelService;
        private readonly INotificationModelService _notificationModelService;
        private readonly ICompromisingEvidenceModelService _compromisingEvidenceModelService;
        private readonly ICompromisingEvidenceFileModelService _compromisingEvidenceFileModelService;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            IVoteProcessModelService voteProcessModelService,
            INotificationModelService notificationModelService,
            ICompromisingEvidenceModelService compromisingEvidenceModelService,
            ICompromisingEvidenceFileModelService compromisingEvidenceFileModelService)
        {
            _userManager = userManager;
            _voteProcessModelService = voteProcessModelService;
            _notificationModelService = notificationModelService;
            _compromisingEvidenceFileModelService = compromisingEvidenceFileModelService;
            _compromisingEvidenceModelService = compromisingEvidenceModelService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            return await UserDetail(currentUser.Id);
        }
        public async Task<IActionResult> UserDetail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var evidences_raw = (from E in _compromisingEvidenceModelService.GetCompromisingEvidenceModels()
                                 where E.UserId.Id == id
                                 select E).Include("UserId").Include("VotePlaceId");
            List<CompromisingEvidenceModel> evidences_pre = evidences_raw.ToList();
            List<EvidenceEntity> evidences = new List<EvidenceEntity>();
            uint index = 0;
            foreach (var ev in evidences_pre)
            {
                index++;
                evidences.Add(
                    new EvidenceEntity()
                    {
                        Evidence = ev,
                        Files = await (
                            from F in _compromisingEvidenceFileModelService.GetCompromisingEvidenceFileModels()
                            where F.CompromisingEvidenceId.Id == ev.Id
                            select F
                        ).ToListAsync(),
                        Email = user.Email,
                        index = index
                    });
            }
            List<NotificationModel> notifications = await _notificationModelService.GetNotificationModels().Where(model => model.ApplicationGetterId.Id == user.Id).ToListAsync();
            UserForm userForm = new UserForm()
            {
                User = user,
                Evidences = evidences,
                Notifications = notifications
            };
            return View(userForm);
        }

        [HttpPost]
        public async Task<IActionResult> UserDetail(UserForm userForm)
        {
            var userToChange = await _userManager.GetUserAsync(HttpContext.User);
            userToChange.Email = userForm.User.Email;
            userToChange.EmailConfirmed = userForm.User.EmailConfirmed;
            userToChange.PhoneNumber = userForm.User.PhoneNumber;
            await _userManager.UpdateAsync(userToChange);
            Notificator.UserEdited(_notificationModelService, userToChange);

            return await UserDetail(userToChange.Id);
        }
    }
}
