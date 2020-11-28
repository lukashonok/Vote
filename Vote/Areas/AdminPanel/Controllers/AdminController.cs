using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.CompromisingEvidenceFileModelService;
using Services.CompromisingEvidenceModelService;
using Services.TargetModelService;
using Services.VotePlaceModelService;
using Services.VoteProcessModelService;
using Vote.Areas.AdminPanel.Forms;
using Vote.Controllers;
using Vote.Services;

namespace Vote.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AdminController : Controller
    {
        private readonly ITargetModelService _targetModelService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVotePlaceModelService _votePlaceModelService;
        private readonly IVoteProcessModelService _voteProcessModelService;
        private readonly INotificationModelService _notificationModelService;
        private readonly ICompromisingEvidenceModelService _compromisingEvidenceModelService;
        private readonly ICompromisingEvidenceFileModelService _compromisingEvidenceFileModelService;
        public AdminController(
            ITargetModelService targetModelService,
            UserManager<ApplicationUser> userManager,
            IVotePlaceModelService votePlaceModelService,
            IVoteProcessModelService voteProcessModelService,
            INotificationModelService notificationModelService,
            ICompromisingEvidenceModelService compromisingEvidenceModelService,
            ICompromisingEvidenceFileModelService compromisingEvidenceFileModelService)
        {
            _userManager = userManager;
            _targetModelService = targetModelService;
            _votePlaceModelService = votePlaceModelService;
            _voteProcessModelService = voteProcessModelService;
            _notificationModelService = notificationModelService;
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

                    //Notificator.VoteProcessChanged(_notificationModelService, _userManager);
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
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<ActionResult> UserDetail(UserForm userForm)
        {
            var userToChange = await _userManager.FindByIdAsync(userForm.User.Id);
            userToChange.Email = userForm.User.Email;
            userToChange.EmailConfirmed = userForm.User.EmailConfirmed;
            userToChange.PhoneNumber = userForm.User.PhoneNumber;
            await _userManager.UpdateAsync(userToChange);
            Notificator.UserEdited(_notificationModelService, userToChange);
            return await UserDetail(userToChange.Id);
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

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Places()
        {
            var places = _votePlaceModelService.GetVotePlaceModels();
            return View(places.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult PlaceDetail(string id)
        {
            VotePlaceModel place = new VotePlaceModel() {
                Id = 0, House = "", Region = "", Street = "", Town = "", x = 0, y = 0
            };
            if(id != null)
            {
                place = _votePlaceModelService.GetVotePlaceModel(int.Parse(id));
            }
            return View(place);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult PlaceDelete(int id)
        {
            _votePlaceModelService.DeleteVotePlaceModel(id);
            return RedirectToAction(nameof(Places));
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult PlaceDetail(VotePlaceModel place)
        {
            if (ModelState.IsValid)
            {
                if (place.Id != 0)
                {
                    _votePlaceModelService.UpdateVotePlaceModel(place);
                    return RedirectToAction(nameof(PlaceDetail), new { id = place.Id.ToString() });
                }
                else
                {
                    _votePlaceModelService.InsertVotePlaceModel(place);
                    return RedirectToAction(nameof(Places));
                }
            } else
            {
                return RedirectToAction(nameof(PlaceDetail), new { id = place.Id.ToString() });
            }
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Targets()
        {
            var targets = _targetModelService.GetTargetModels();
            return View(targets.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult TargetDetail(string id)
        {
            var target = _targetModelService.GetTargetModel(int.Parse(id));
            return View(target);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult TargetDelete(int id)
        {
            _targetModelService.DeleteTargetModel(id);
            return RedirectToAction(nameof(Targets));
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult TargetDetail(TargetModel target)
        {
            if (ModelState.IsValid)
            {
                if (target.Id != 0)
                {
                    _targetModelService.UpdateTargetModel(target);
                    return RedirectToAction(nameof(Targets), new { id = target.Id.ToString() });
                }
                else
                {
                    _targetModelService.InsertTargetModel(target);
                    return RedirectToAction(nameof(Targets));
                }
            }
            else
            {
                return RedirectToAction(nameof(TargetDetail), new { id = target.Id.ToString() });
            }
        }
    }
}
