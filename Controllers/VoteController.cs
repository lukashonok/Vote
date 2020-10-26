using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories;
using Vote.Forms;
using Services.VoteModelService;
using Services.TargetModelService;
using Microsoft.AspNetCore.Identity;
using Services.PhoneNumberModelService;
using Services.VotePlaceModelService;
using Services.VoteProcessModelService;
using System.Data.Common;
using System.Reflection.Metadata;

namespace Vote.Controllers
{
    public class VoteController : Controller
    {
        private readonly ILogger<VoteController> _logger;
        private readonly IVoteModelService _voteModelService;
        private readonly ITargetModelService _targetModelService;
        private readonly IPhoneNumberModelService _phoneNumberModelService;
        private readonly IVotePlaceModelService _votePlaceModelService;
        private readonly IVoteProcessModelService _voteProcessModelService;
        private readonly UserManager<ApplicationUser> _userManager;

        public VoteController(
            ILogger<VoteController> logger, 
            IVoteModelService voteModelService,
            ITargetModelService targetModelService,
            IPhoneNumberModelService phoneNumberModelService,
            IVotePlaceModelService votePlaceModelService,
            IVoteProcessModelService voteProcessModelService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _voteModelService = voteModelService;
            _targetModelService = targetModelService;
            _userManager = userManager;
            _phoneNumberModelService = phoneNumberModelService;
            _votePlaceModelService = votePlaceModelService;
            _voteProcessModelService = voteProcessModelService;
        }
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            SelectList places = GetVotePlacesForSelect("Region");
            SelectList targets = new SelectList(_targetModelService.GetTargetModels(), "Id", "Name");

            ViewBag.Places = places;
            ViewBag.Targets = targets;

            var voteForm = new VoteForm();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                voteForm.PhoneNumber = user.PhoneNumber;
            }

            return View(voteForm);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Index(VoteForm voteForm)
        {
            if (_phoneNumberModelService.GetPhoneNumberModels().Where(phone => phone.PhoneNumber == voteForm.PhoneNumber).ToList().Count != 0)
            {
                ModelState.AddModelError("Телефонный номер", "Голос с использованием такого номера уже есть!");
            }
            if (voteForm.Place == null)
            {
                ModelState.AddModelError("Место для голосования", "Пожалуйста, выберите место для голосования.");
            }

            if (ModelState.IsValid)
            {
                TargetModel target = _targetModelService.GetTargetModel(Convert.ToInt32(voteForm.Target));
                VotePlaceModel place = _votePlaceModelService.GetVotePlaceModel(Convert.ToInt32(voteForm.Place));
                VoteProcessModel process = _voteProcessModelService.GetVoteProcessModels().ToList().Last();
                PhoneNumberModel phoneNumber;

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    ApplicationUser user = await _userManager.GetUserAsync(User);
                    phoneNumber = new PhoneNumberModel() { PhoneNumber = user.PhoneNumber };
                } else
                {
                    phoneNumber = new PhoneNumberModel() { PhoneNumber = voteForm.PhoneNumber };
                }

                _voteModelService.InsertVoteModel(
                    new VoteModel()
                    {
                        CreatedAt = DateTime.Now,
                        TargetId = target,
                        VotePlaceId = place,
                        PhoneNumberId = phoneNumber,
                        VoteProcessId = process
                    });

                //await _context.SaveChangesAsync();
                _logger.LogInformation($"{voteForm.PhoneNumber} voted");
                return RedirectToAction(nameof(VoteSuccess));
            } else {
                return await Index();
            }
        }

        public ActionResult VoteSuccess()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public SelectList GetVotePlacesForSelect(string field, string text = null)
        {
            IQueryable<VotePlaceModel> places_raw = _votePlaceModelService.GetVotePlaceModels();
            SelectList places = new SelectList(places_raw);

            if (text == null)
            {
                places = new SelectList(places_raw.ToList(), "Id", field, places_raw.First().Id);
            } else
            {
                switch (field)
                {
                    case "Region":
                        places_raw = places_raw.Where(place => place.Region == text);
                        places = new SelectList(places_raw.ToList(), "Id", "Town", places_raw.First().Id);
                        break;
                    case "Town":
                        places_raw = places_raw.Where(place => place.Town == text);
                        places = new SelectList(places_raw.ToList(), "Id", "Street", places_raw.First().Id);
                        break;
                    case "Street":
                        places_raw = places_raw.Where(place => place.Street == text);
                        places = new SelectList(places_raw.ToList(), "Id", "House", places_raw.First().Id);
                        break;
                    case "House":
                        places_raw = places_raw.Where(place => place.House == text);
                        places = new SelectList(places_raw.ToList(), "Id", "House", places_raw.First().Id);
                        break;
                    default:
                        break;
                }
                
            }

            var placesNoDuplicates = places.ToList();
            foreach (var place in placesNoDuplicates)
            {
                if (placesNoDuplicates.Where(p => p.Text == place.Text).Count() > 1)
                {
                    place.Text = "";
                }
            }
            places = new SelectList(placesNoDuplicates.Where(p => p.Text != "").ToList(), "Value", "Text");

            return places;
        }
    }
}
