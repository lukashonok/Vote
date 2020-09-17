using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Vote.Models;

namespace Vote.Controllers
{
    public class MapController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public MapController(ILogger<HomeController> logger)
        {
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
            List<MapMarker> markers = new List<MapMarker> {
                new MapMarker { 
                    x = 53.31f, y = 28.02f, balloonCloseButton = false, 
                    balloonContent = "<img src=\"http://img-fotki.yandex.ru/get/6114/82599242.2d6/0_88b97_ec425cf5_M\" />", 
                    hideIconOnBalloonOpen = false, iconContent = "123", preset = "islands#yellowStretchyIcon"},
                new MapMarker {
                    x = 53.4f, y = 28.04f, balloonCloseButton = false,
                    balloonContent = "<img src=\"http://img-fotki.yandex.ru/get/6114/82599242.2d6/0_88b97_ec425cf5_M\" />",
                    hideIconOnBalloonOpen = false, iconContent = "Kirill privet", preset = "islands#yellowStretchyIcon"},
                new MapMarker {
                    x = 53.45f, y = 28.00f,balloonCloseButton = false,
                    balloonContent = "<img src=\"http://img-fotki.yandex.ru/get/6114/82599242.2d6/0_88b97_ec425cf5_M\" />",
                    hideIconOnBalloonOpen = false, iconContent = "123", preset = "islands#yellowStretchyIcon"},
            };
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
