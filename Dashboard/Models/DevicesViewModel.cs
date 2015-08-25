using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WoodStove.Core;

namespace Dashboard.Models
{
    public class DevicesViewModel
    {
        public List<Device> Devices { get; set; }
    }

    public class DeviceViewModel
    {
        public string Id { get; set; }
        public string NewId { get; set; }
        public string Name { get; set; }
    }
}