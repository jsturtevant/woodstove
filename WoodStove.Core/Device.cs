using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WoodStove.Core
{
    public class Device
    {
        public Device()
        {
            Display = true;
        }

        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }


        [RegularExpression("^[0-9]{5}$", ErrorMessage = "Invalid Zip")]
        public string ZipCode { get; set; }

        public bool Display { get; set; }
    }
}
