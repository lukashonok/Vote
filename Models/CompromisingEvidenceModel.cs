using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Vote.Areas.Identity.Data;

namespace Vote.Models
{
    public class CompromisingEvidenceModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("VotePlace")]
        public VotePlaceModel VotePlaceId { get; set; }
        [ForeignKey("ApplicationUser")]
        public ApplicationUser UserId { get; set; }
        public string Comment { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}
