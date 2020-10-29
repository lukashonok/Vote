using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class VotePlaceModel : Base
    {
        public float x { get; set; }
        public float y { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}