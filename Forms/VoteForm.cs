using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vote.Models;

namespace Vote.Forms
{
    public class VoteForm
    {
        [Required]
        [Phone]
        [Display( Name = "Ваш номер телефона")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Кандидат")]
        public string Target { get; set; }

        [Display(Name = "Место голосования")]
        public string Place { get; set; }
        public List<SelectListItem> Targets { get; set; }

    }
}
