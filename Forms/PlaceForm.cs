using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Vote.Areas.Identity.Data;
using Vote.Models;

namespace Vote.Forms
{
    public class EvidenceForm
    {
        [Required]
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
        [Display(Name = "Файл")]
        public IFormFile File1 { get; set; }
        public IFormFile File2 { get; set; }
        public IFormFile File3 { get; set; }

        public int PlaceId { get; set; }
    }
}
