using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class TargetModel : Base
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
