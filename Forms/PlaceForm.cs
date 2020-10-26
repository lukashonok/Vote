using Entities;
using Microsoft.AspNetCore.Http;
using Services.VotePlaceModelService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vote.Forms
{
    public class EvidenceForm
    {
        [Required]
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
        [Display(Name = "Прикреплённые файлы")]
        public IFormFileCollection Files { get; set; }
        public int PlaceId { get; set; }
    }
    public class PlacesSearch
    {
        public int Total { get; set; }
        public List<int> PlacesId { get; set; }
    }
}
