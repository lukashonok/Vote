using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    
    public class VoteStat
    {
        public int Total { get; set; }
        public List<VotePropsForChart> VoteStats { get; set; }
    }

    public class VotePropsForChart
    {
        public string targetName { get; set; }
        public int v { get; set; }
        public string f { get; set; }
    }
}
