using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Vote.Data;
using Vote.Models;

namespace Vote.Controllers
{
    public class MapController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VoteContext _context;
        public MapController(ILogger<HomeController> logger, VoteContext context)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            ViewBag.Items = new List<int>{ 1, 2, 3};
            return View();
        }

        [HttpGet]
        public List<MapMarker> ReturnMarks()
        {
            //SELECT VotePlace.Id, VotePlace.x, VotePlace.y, Count(VotePlace.Id) as 'Total'
            //FROM Vote INNER JOIN VotePlace on VotePlace.Id = Vote.VotePlace
            //GROUP BY VotePlace.x, VotePlace.y, VotePlace.Id;

            var places_raw = from V in _context.Vote
                     join VP in _context.VotePlace on V.VotePlaceId.Id equals VP.Id
                     group V by new { VP.Id, VP.x, VP.y, VP.Region, VP.Town, VP.Street, VP.House } into total
                     select new {total.Key.Id, total.Key.x, total.Key.y, total.Key.Region, total.Key.Town, total.Key.Street, total.Key.House, count = total.Count()};
            var places = places_raw.ToList();
            List<MapMarker> markers = new List<MapMarker>();
            foreach (var place in places)
            {
                markers.Add(
                    new MapMarker
                    {
                        x = place.x, y = place.y, balloonCloseButton = true,
                        balloonContent = 
                        $"<div style=\" text-align: center\"> В этом месте проголоcовало: {place.count}</div><br>" +
                        $"Область: {place.Region}<br>"+
                        $"Город: {place.Town}<br>"+
                        $"Улица: {place.Street}<br>"+
                        $"Дом: {place.House}<br>"+
                        $"<a href=\"{@Url.Action("PlaceDetail", "Map")}\">Подробнее</a><br>",
                        hideIconOnBalloonOpen = false,
                        iconContent = Convert.ToString(place.count),
                        preset = "islands#yellowStretchyIcon"
                    });
            }

            return markers;
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
}
