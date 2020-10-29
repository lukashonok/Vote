using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class NotificationModel : Base
    {
        [ForeignKey("ApplicationUser")]
        public ApplicationUser ApplicationGetterId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}