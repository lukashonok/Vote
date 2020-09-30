using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vote.Data;
using Vote.Forms;
using Vote.Models;

namespace Vote.Controllers
{
    public class VoteController : Controller
    {
        private readonly VoteContext _context;
        private readonly ILogger<VoteController> _logger;
        public VoteController(VoteContext context, ILogger<VoteController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            SelectList places = await GetVotePlacesForSelect("Region");
            SelectList targets = new SelectList(await _context.Target.ToListAsync(), "Id", "Name");

            ViewBag.Places = places;
            ViewBag.Targets = targets;

            var voteForm = new VoteForm();

            return View(voteForm);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Index(VoteForm voteForm)
        {
            if (ModelState.IsValid)
            {
                TargetModel target = await _context.Target.FindAsync(Convert.ToInt32(voteForm.Target));
                VotePlaceModel place = await _context.VotePlace.FindAsync(Convert.ToInt32(voteForm.Place));
                _context.Vote.Add(
                    new VoteModel()
                    {
                        CreatedAt = DateTime.Now,
                        TargetId = target,
                        VotePlaceId = place,
                        PhoneNumber = voteForm.PhoneNumber
                    }
                );
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(VoteSuccess));
            } else
            {
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

        // POST: VoteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<SelectList> GetVotePlacesForSelect(string field, string text = null)
        {
            List<VotePlaceModel> places_raw = new List<VotePlaceModel>();
            SelectList places = new SelectList(places_raw);
            if (text == null)
            {
                places_raw = await _context.VotePlace.ToListAsync();
                places = new SelectList(places_raw, "Id", field, places_raw[0].Id);
            } else
            {
                switch (field)
                {
                    case "Region":
                        places_raw = await _context.VotePlace.Where(place => place.Region == text).ToListAsync();
                        places = new SelectList(places_raw, "Id", "Town", places_raw[0].Id);
                        break;
                    case "Town":
                        places_raw = await _context.VotePlace.Where(place => place.Town == text).ToListAsync();
                        places = new SelectList(places_raw, "Id", "Street", places_raw[0].Id);
                        break;
                    case "Street":
                        places_raw = await _context.VotePlace.Where(place => place.Street == text).ToListAsync();
                        places = new SelectList(places_raw, "Id", "House", places_raw[0].Id);
                        break;
                    case "House":
                        places_raw = await _context.VotePlace.Where(place => place.House == text).ToListAsync();
                        places = new SelectList(places_raw, "Id", "House", places_raw[0].Id);
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
