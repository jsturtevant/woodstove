using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WoodStove.Core;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class DevicesViewModel
    {
        public List<Device> Devices { get; set; }
    }

    public class DeviceViewModel
    {
        public string NewId { get; set; }
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }


        [RegularExpression("^[0-9]{5}$", ErrorMessage = "Invalid Zip")]
        public string ZipCode { get; set; }
    }
}