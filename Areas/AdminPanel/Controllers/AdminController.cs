using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vote.Areas.AdminPanel.Forms;
using Vote.Areas.Identity.Data;
using Vote.Controllers;
using Vote.Data;
using Vote.Forms;
using Vote.Models;

namespace Vote.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AdminController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        private readonly VoteContext _context;
        public AdminController(UserManager<ApplicationUser> userManager, VoteContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Index()
        {
            var voteProcess = await _context.VoteProcess.FindAsync(1);
            var voteProcessForm = new VoteProcessForm()
            {
                Id = voteProcess.Id,
                CreatedAt = voteProcess.CreatedAt,
                EndAt = voteProcess.EndAt,
                showResults = voteProcess.showResults
            };
            ViewBag.CreatedAt = voteProcess.CreatedAt;
            ViewBag.EndAt = voteProcess.EndAt;
            ViewBag.showResults = voteProcess.showResults;
            return View(voteProcessForm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(VoteProcessForm voteProcessForm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var voteProcessModel = await _context.VoteProcess.FindAsync(voteProcessForm.Id);
                    voteProcessModel.showResults = voteProcessForm.showResults;
                    voteProcessModel.CreatedAt = voteProcessForm.CreatedAt;
                    voteProcessModel.EndAt = voteProcessForm.EndAt;
                    _context.Entry(voteProcessModel).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
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

        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Users()
        {
            UsersForm UsersForm = new UsersForm()
            {
                Users = new List<AdminPanelUser>()
            };
            foreach (var user in await _userManager.Users.ToListAsync())
            {
                UsersForm.Users.Add(
                    new AdminPanelUser()
                    {
                        Email = user.Email,
                        AccessFailedCount = user.AccessFailedCount,
                        EmailConfirmed = user.EmailConfirmed,
                        Id = user.Id,
                        Phone = user.PhoneNumber,
                    });
            }
            return View(UsersForm.Users);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> UserDetail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var evidences_raw = (from E in _context.CompromisingEvidence
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
                            from F in _context.CompromisingEvidenceFile
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

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UserDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            return await Users();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UserRoleSet(string id, string Role)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.AddToRoleAsync(user, Role);
            return await Users();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UserRoleUnset(string id, string Role)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, Role);
            return await Users();
        }
    }
}
