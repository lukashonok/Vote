using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vote.Models
{
    public class VoteProcessModel
    {
        [Key]
        public int Id { get; set; }
        public bool showResults { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndAt { get; set; }
    }
}
