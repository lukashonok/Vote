using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class PhoneNumberModel : Base
    {
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

    }
}
