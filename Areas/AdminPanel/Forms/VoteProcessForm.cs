using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vote.Areas.AdminPanel
{
    public class VoteProcessForm
    {
        public int Id { get; set; }
        [Required]
        [DataType("Boolean")]
        [Display(Name = "Показать текущие результаты голосования")]
        public Boolean ShowResults { get; set; }

        [Required]
        [DataType("Date")]
        [Display(Name = "Дата начала голосования")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [DataType("Date")]
        [Display(Name = "Дата конца голосования")]
        public DateTime EndAt { get; set; }
    }
}
