using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Vote.Areas.Identity.Data;
using Vote.Data;
using Vote.Forms;
using Vote.Models;
using Vote.ViewComponents;

namespace Vote.Controllers
{
    public class MapController : Controller
    {
        private readonly VoteContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<MapController> _logger;
        public MapController(ILogger<MapController> logger, VoteContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<ActionResult> Index()
        {
            var voteProcess = (await _context.VoteProcess.ToListAsync()).Last();
            ViewBag.showResults = voteProcess.showResults;
            ViewBag.EndAt = voteProcess.EndAt;
            return View();
        }

        public async Task<ActionResult> PlaceDetail(int id)
        {
            VotePlaceModel place = await _context.VotePlace.FindAsync(id);
            var evidences_raw = (from E in _context.CompromisingEvidence
                                 where E.VotePlaceId.Id == id
                                 select E).Include("UserId");
            List<CompromisingEvidenceModel> evidences_pre = evidences_raw.ToList();

            List<EvidenceEntity> evidences = new List<EvidenceEntity>();
            // TODO: Try to optimize image loading
            uint index = 0;
            foreach (var ev in evidences_pre)
            {
                //ev.UserId = await _userManager.GetUserAsync(HttpContext.User);
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
                        Email = ev.UserId?.Email,
                        index = index
                    });
            }

            var TotalVotes = (from V in _context.Vote
                              where V.VotePlaceId.Id == id
                              select V.TargetId).Count();

            ViewBag.TotalVotes = TotalVotes;
            ViewBag.TotalEvidences = evidences.Count();
            ViewBag.Region = place.Region;
            ViewBag.Town = place.Town;
            ViewBag.Street = place.Street;
            ViewBag.House = place.House;
            ViewBag.Id = place.Id;

            ViewBag.evidences = evidences;

            ViewBag.isAuthenticated = HttpContext.User.Identity.IsAuthenticated;

            var evidenceForm = new EvidenceForm()
            {
                PlaceId = place.Id
            };

            return View(evidenceForm);
        }

        [HttpPost]
        public async Task<ActionResult> PlaceDetail(EvidenceForm evidenceForm)
        {
            if (ModelState.IsValid)
            {
                CompromisingEvidenceModel evidence = new CompromisingEvidenceModel()
                {
                    Comment = evidenceForm.Comment,
                    UserId = await _userManager.GetUserAsync(HttpContext.User),
                    VotePlaceId = await _context.VotePlace.FindAsync(evidenceForm.PlaceId)
                };
                _context.CompromisingEvidence.Add(evidence);
                var files = new List<IFormFile>()
                {
                    evidenceForm.File1, evidenceForm.File2, evidenceForm.File3
                };
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        using var memoryStream = new MemoryStream();
                        if (memoryStream.Length < 10097152)
                        {
                            string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
                            string filename = Path.GetFileName(file.FileName);
                            if (filename != null)
                            {
                                await file.CopyToAsync(memoryStream);

                                var fileToSave = new CompromisingEvidenceFileModel()
                                {
                                    File = memoryStream.ToArray(),
                                    CompromisingEvidenceId = evidence
                                };

                                _context.CompromisingEvidenceFile.Add(fileToSave);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("File", "The file is too large.");
                        }
                    }
                }
                _logger.LogInformation($"Evidence was created for place with id = {evidenceForm.PlaceId}");
                await _context.SaveChangesAsync();
            }

            return await PlaceDetail(evidenceForm.PlaceId);
        }

        [HttpGet]
        public async Task<List<MapMarker>> GetMarks()
        {
            //SELECT VotePlace.Id, VotePlace.x, VotePlace.y, Count(VotePlace.Id) as 'Total'
            //FROM Vote INNER JOIN VotePlace on VotePlace.Id = Vote.VotePlace
            //GROUP BY VotePlace.x, VotePlace.y, VotePlace.Id;

            var places_raw = from V in _context.Vote
                             join VP in _context.VotePlace on V.VotePlaceId.Id equals VP.Id
                             group V by new { VP.Id, VP.x, VP.y, VP.Region, VP.Town, VP.Street, VP.House } into total
                             select new { total.Key.Id, total.Key.x, total.Key.y, total.Key.Region, total.Key.Town, total.Key.Street, total.Key.House, count = total.Count() };
            var places = await places_raw.ToListAsync();

            List<MapMarker> markers = new List<MapMarker>();


            foreach (var place in places)
            {
                var q = ViewComponent("PlaceInfo", new { id = place.Id, showDetail = true });

                markers.Add(
                    new MapMarker
                    {
                        x = place.x,
                        y = place.y,
                        balloonCloseButton = true,
                        balloonContent =
                            $"<div style=\" text-align: center\"> В этом месте проголоcовало: {place.count}</div><br>" +
                            $"Область: {place.Region}<br>" +
                            $"Город: {place.Town}<br>" +
                            $"Улица: {place.Street}<br>" +
                            $"Дом: {place.House}<br>" +
                            $"<a href=\"{@Url.Action("PlaceDetail", "Map", new { place.Id })}\">Подробнее</a><br>",
                        hideIconOnBalloonOpen = false,
                        iconContent = Convert.ToString(place.count),
                        preset = "islands#yellowStretchyIcon"
                    });
            }

            return markers;
        }

        [HttpGet]
        public async Task<VoteStat> GetVoteStat()
        {
            //SELECT Target.Name, Count(Vote.Target) as "Total" 
            //FROM Target LEFT JOIN  Vote ON Vote.Target = Target.Id 
            //GROUP BY Vote.Target, Target.Name;
            var voteStat_raw = (from T in _context.Target
                              join V in _context.Vote on T.Id equals V.TargetId.Id
                              group T by new { T.Name, T.Id } into total
                              select new { total.Key.Name, Count = total.Count() });
            var totalVotes = await  _context.Vote.CountAsync();

            VoteStat voteStat = new VoteStat()
            {
                VoteStats = new List<VotePropsForChart>(),
                Total = totalVotes
            };

            var voteStat_pre = await voteStat_raw.ToListAsync();
            foreach (var vote in voteStat_pre)
            {
                voteStat.VoteStats.Add(
                    new VotePropsForChart {
                        v = vote.Count,
                        f = Convert.ToString(vote.Count),
                        targetName = vote.Name
                    });
            }
            return voteStat;
        }
    }
    public class MapMarker
    {
        public float x { get; set; }
        public float y { get; set; }
        public string balloonContent { get; set; }
        public string iconContent { get; set; }
        public string preset { get; set; }
        public bool balloonCloseButton { get; set; }
        public bool hideIconOnBalloonOpen { get; set; }
    }
    public class EvidenceEntity
    {
        public CompromisingEvidenceModel Evidence;
        public List<CompromisingEvidenceFileModel> Files;
        public string Email;
        public uint index;
    }
}
