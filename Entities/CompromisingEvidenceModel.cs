using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class CompromisingEvidenceModel : Base
    {
        [ForeignKey("VotePlace")]
        public VotePlaceModel VotePlaceId { get; set; }
        [ForeignKey("ApplicationUser")]
        public ApplicationUser UserId { get; set; }
        public string Comment { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}
