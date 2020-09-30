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

namespace Vote.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AdminController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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
                        Email             = user.Email,
                        AccessFailedCount = user.AccessFailedCount,
                        EmailConfirmed    = user.EmailConfirmed,
                        Id                = user.Id,
                        Phone             = user.PhoneNumber,
                    });
            }
            return View(UsersForm.Users);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Users(List<AdminPanelUser> Users)
        {
            
            return View();
        }
    }
}
