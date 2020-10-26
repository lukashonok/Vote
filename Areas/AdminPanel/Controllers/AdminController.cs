using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services.CompromisingEvidenceFileModelService;
using Services.CompromisingEvidenceModelService;
using Services.VoteProcessModelService;
using Vote.Areas.AdminPanel.Forms;
using Vote.Controllers;
using Vote.Forms;

namespace Vote.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVoteProcessModelService _voteProcessModelService;
        private readonly ICompromisingEvidenceModelService _compromisingEvidenceModelService;
        private readonly ICompromisingEvidenceFileModelService _compromisingEvidenceFileModelService;
        public AdminController(
            UserManager<ApplicationUser> userManager,
            IVoteProcessModelService voteProcessModelService,
            ICompromisingEvidenceModelService compromisingEvidenceModelService,
            ICompromisingEvidenceFileModelService compromisingEvidenceFileModelService)
        {
            _userManager = userManager;
            _voteProcessModelService = voteProcessModelService;
            _compromisingEvidenceFileModelService = compromisingEvidenceFileModelService;
            _compromisingEvidenceModelService = compromisingEvidenceModelService;
        }

        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public ActionResult Index()
        {
            var voteProcess = _voteProcessModelService.GetVoteProcessModels().ToList().Last();
            var voteProcessForm = new VoteProcessForm()
            {
                Id = voteProcess.Id,
                CreatedAt = voteProcess.CreatedAt,
                EndAt = voteProcess.EndAt,
                ShowResults = voteProcess.ShowResults
            };
            ViewBag.CreatedAt = voteProcess.CreatedAt;
            ViewBag.EndAt = voteProcess.EndAt;
            ViewBag.showResults = voteProcess.ShowResults;
            return View(voteProcessForm);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Index(VoteProcessForm voteProcessForm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var voteProcessModel = _voteProcessModelService.GetVoteProcessModels().First(process => process.Id == voteProcessForm.Id);

                    voteProcessModel.ShowResults = voteProcessForm.ShowResults;
                    voteProcessModel.CreatedAt = voteProcessForm.CreatedAt;
                    voteProcessModel.EndAt = voteProcessForm.EndAt;

                    _voteProcessModelService.UpdateVoteProcessModel(voteProcessModel);
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                ViewBag.Message = "Процесс изменён!";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<ActionResult> Users()
        {
            UsersForm UsersForm = new UsersForm()
            {
                Users = new List<AdminPanelUser>()
            };
            foreach (var user in await _userManager.Users.ToListAsync())
            {
                var roles = await _userManager.GetRolesAsync(user);
                UsersForm.Users.Add(
                    new AdminPanelUser()
                    {
                        Email = user.Email,
                        AccessFailedCount = user.AccessFailedCount,
                        EmailConfirmed = user.EmailConfirmed,
                        Id = user.Id,
                        Phone = user.PhoneNumber,
                        Roles = (await _userManager.GetRolesAsync(user)).ToList()
                    });
            }
            return View(UsersForm.Users);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<ActionResult> UserDetail(string id)
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
                        index = index
                    });
            }
            UserForm userForm = new UserForm() {
                User = user,
                Evidences = evidences
            };
            return View(userForm);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> UserDelete(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.DeleteAsync(user);
            return await Users();
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> UserRoleSet(string Id, string Role)
        {
            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.AddToRoleAsync(user, Role);
            var roles = await _userManager.GetRolesAsync(user);
            return RedirectToAction(nameof(Users));
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> UserRoleUnset(string Id, string Role)
        {
            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.RemoveFromRoleAsync(user, Role);
            var roles = await _userManager.GetRolesAsync(user);
            return RedirectToAction(nameof(Users));
        }
    }
}
