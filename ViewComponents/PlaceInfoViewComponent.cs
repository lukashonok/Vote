using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vote.ViewComponents
{
    public class PlaceInfoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;


        public PlaceInfoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id, bool showDetail)
        {
            VotePlaceModel place = await (from VP in _context.VotePlace
                                   where VP.Id == id
                                   select VP).FirstAsync();
            var evidences_raw = (from E in _context.CompromisingEvidence
                                 where E.VotePlaceId.Id == id
                                 select E).Include("UserId").Include("VotePlaceId");
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