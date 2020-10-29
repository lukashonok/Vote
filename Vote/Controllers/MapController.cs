using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repositories;
using Services.CompromisingEvidenceFileModelService;
using Services.CompromisingEvidenceModelService;
using Services.TargetModelService;
using Services.VoteModelService;
using Services.VotePlaceModelService;
using Services.VoteProcessModelService;
using Vote.Forms;
using Vote.ViewComponents;
using Microsoft.Extensions.Localization;

namespace Vote.Controllers
{
    public class MapController : Controller
    {
        
        private readonly ILogger<MapController> _logger;
        private readonly IVoteModelService _voteModelService;
        private readonly ITargetModelService _targetModelService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IStringLocalizer<MapController> _localizer;
        private readonly IVotePlaceModelService _votePlaceModelService;
        private readonly IVoteProcessModelService _voteProcessModelService;
        private readonly ICompromisingEvidenceModelService _compromisingEvidenceModelService;
        private readonly ICompromisingEvidenceFileModelService _compromisingEvidenceFileModelService;


        public MapController(
            ILogger<MapController> logger,
            IVoteModelService voteModelService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext applicationDbContext,
            IStringLocalizer<MapController> localizer,
            IVotePlaceModelService votePlaceModelService,
            IVoteProcessModelService voteProcessModelService,
            ICompromisingEvidenceModelService compromisingEvidenceModelService,
            ICompromisingEvidenceFileModelService compromisingEvidenceFileModelService,
            ITargetModelService targetModelService)
        {
            _logger = logger;
            _localizer = localizer;
            _userManager = userManager;
            _voteModelService = voteModelService;
            _targetModelService = targetModelService;
            _applicationDbContext = applicationDbContext;
            _votePlaceModelService = votePlaceModelService;
            _voteProcessModelService = voteProcessModelService;
            _compromisingEvidenceModelService = compromisingEvidenceModelService;
            _compromisingEvidenceFileModelService = compromisingEvidenceFileModelService;

        }
        public async Task<ActionResult> Index()
        {
            var voteProcess = (await _voteProcessModelService.GetVoteProcessModels().ToListAsync()).Last();
            ViewBag.ShowResults = voteProcess.ShowResults;
            ViewBag.EndAt = voteProcess.EndAt;
            return View();
        }

        public async Task<ActionResult> PlaceDetail(int id)
        {
            VotePlaceModel place = _votePlaceModelService.GetVotePlaceModel(id);
            var evidences_raw = (from E in _compromisingEvidenceModelService.GetCompromisingEvidenceModels()
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
                        from F in _compromisingEvidenceFileModelService.GetCompromisingEvidenceFileModels()
                        where F.CompromisingEvidenceId.Id == ev.Id
                        select F
                    ).ToListAsync(),
                    Email = ev.UserId?.Email,
                    index = index
                });
            }

            var TotalVotes = (from V in _voteModelService.GetVoteModels()
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



        [HttpGet]
        public async Task<List<MapMarker>> GetMarks()
        {
            //SELECT VotePlace.Id, VotePlace.x, VotePlace.y, Count(VotePlace.Id) as 'Total'
            //FROM Vote INNER JOIN VotePlace on VotePlace.Id = Vote.VotePlace
            //GROUP BY VotePlace.x, VotePlace.y, VotePlace.Id;

            var places_raw = from V in _applicationDbContext.Vote
                             join VP in _applicationDbContext.VotePlace on V.VotePlaceId.Id equals VP.Id
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
        public async Task<IActionResult> VotePlaces()
        {
            PlacesSearch placesSearch = await GetVotePlaces(null);

            return View(placesSearch);
        }
        public async Task<PlacesSearch> GetVotePlaces(string searchString)
        {
            var places = _votePlaceModelService.GetVotePlaceModels();
            IQueryable<int> places_id;
            if (searchString == null)
            {
                places_id = (from VP in places select VP.Id);
            } else
            {
                places_id = 
                    from VP in places 
                    where
                        VP.Region.Contains($"{searchString}") ||
                        VP.Town.Contains($"{searchString}") ||
                        VP.Street.Contains($"{searchString}") ||
                        VP.House.Contains($"{searchString}")
                    select VP.Id;
            }
            var totalPlaces = await places_id.CountAsync();

            return new PlacesSearch() { PlacesId = places_id.ToList(), Total = totalPlaces};
        }

        [HttpGet]
        public IActionResult GetPlaceInfoViewComponent(int id)
        {
            var a = ViewComponent(typeof(PlaceInfoViewComponent), new { id, showDetail = true });
            return a;
        }

        [HttpGet]
        public async Task<VoteStat> GetVoteStat()
        {   
            //SELECT Target.Name, Count(Vote.Target) as "Total" 
            //FROM Target LEFT JOIN  Vote ON Vote.Target = Target.Id 
            //GROUP BY Vote.Target, Target.Name;
            var votes = _voteModelService.GetVoteModels();
            var voteStat_raw = (from T in _targetModelService.GetTargetModels()
                                join V in votes on T.Id equals V.TargetId.Id
                                group T by new { T.Name, T.Id } into total
                                select new { total.Key.Name, Count = total.Count() });
            var totalVotes = votes.Count();

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
