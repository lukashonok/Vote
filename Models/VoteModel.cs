using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vote.Areas.Identity.Data;

namespace Vote.Models
{
    public class VoteModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("VotePlace")]
        public VotePlaceModel VotePlaceId { get; set; }
        [ForeignKey("ApplicationUser")]
        public ApplicationUser UserId { get; set; }
        [ForeignKey("Target")]
        public TargetModel TargetId { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}