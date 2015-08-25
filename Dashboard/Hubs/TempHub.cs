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


        public void SubscribeToDevice(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return;
            }

            //TODO security check for permission to device id

            //todo remove from any previously assigned connections (no easy api. we must track it)
            this.Groups.Add(this.Context.ConnectionId, deviceId);
        }

        public void RemoveDeviceSubscription(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return;
            }

            this.Groups.Remove(this.Context.ConnectionId, deviceId);
        }

        public void UpdateDeviceReading(DeviceReading temp)
        {
            Clients.Group(temp.DeviceId).currentTemp(temp);
        }
    }
}