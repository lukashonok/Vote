using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vote.Data;
using Vote.Models;

namespace Vote.ViewComponents
{
    public class PlaceInfoViewComponent : ViewComponent
    {
        private readonly VoteContext _context;

        public PlaceInfoViewComponent(VoteContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id, bool showDetail)
        {
            VotePlaceModel place = await _context.VotePlace.FindAsync(id);
            var evidences_raw = (from E in _context.CompromisingEvidence
                                 where E.VotePlaceId.Id == id
                                 select E).Include("UserId");
            List<CompromisingEvidenceModel> evidences_pre = evidences_raw.ToList();

            var TotalVotes = (from V in _context.Vote
                              where V.VotePlaceId.Id == id
                              select V.TargetId).Count();

            ViewBag.TotalVotes = TotalVotes;
            ViewBag.TotalEvidences = evidences_pre.Count();
            ViewBag.Region = place.Region;
            ViewBag.Town = place.Town;
            ViewBag.Street = place.Street;
            ViewBag.House = place.House;
            ViewBag.Id = place.Id;
            ViewBag.showDetail = showDetail;
            return View();
        }
    }
}