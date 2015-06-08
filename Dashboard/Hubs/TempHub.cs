using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WoodStove.Core;

namespace Dashboard.Hubs
{
    public class TempHub : Hub
    {
        public void Hello()
        {
            Clients.All.currentTemp(0);
        }

        

        public void UpdateDeviceReading(DeviceReading temp)
        {
           // Clients.Group(temp.DeviceId).currentTemp(temp.Value);

            // leaving here until we get all the clients signed up and using device id
            Clients.All.currentTemp(temp.Value);
        }

    }
}