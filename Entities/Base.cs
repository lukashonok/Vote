using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class Base
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }
}
