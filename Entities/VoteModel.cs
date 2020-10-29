using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class VoteModel : Base
    {
        [ForeignKey("VotePlace")]
        public VotePlaceModel VotePlaceId { get; set; }
        [ForeignKey("ApplicationUser")]
        public ApplicationUser UserId { get; set; }
        [ForeignKey("Target")]
        public TargetModel TargetId { get; set; }
        [ForeignKey("VoteProcess")]
        public VoteProcessModel VoteProcessId { get; set; }
        [ForeignKey("PhoneNumberModel")]
        public PhoneNumberModel PhoneNumberId { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}