using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WoodStove.Core;
using ForecastIO;
using GoogleMaps.LocationServices;
using Dashboard.Models;

namespace Dashboard.Hubs
{
    public class TempHub : Hub
    {
        private string apikey = "";


        public TempHub()
        {
           apikey =  System.Configuration.ConfigurationManager.AppSettings["weatherapi"];
        }

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
            // TODO move to cache
            var db = new ApplicationDbContext();
            var device = db.Devices.Find(temp.DeviceId);

            AddressData address = new AddressData
            {
                Zip = device.ZipCode
            };


            // move look up to a local cache and build up as it goes
            var gls = new GoogleLocationService();

            try
            {
                var latlong = gls.GetLatLongFromAddress(address);

                if (latlong != null)
                {
                    var request = new ForecastIORequest(apikey, (float)latlong.Latitude, (float)latlong.Longitude, Unit.us);
                    var response = request.Get();

                    temp.TemperatureOutside = response.currently.temperature;
                }

            }
            catch (System.Net.WebException ex)
            {
                //dont worry about the exception right now.  Just try again next time.  probably should log
            }


            Clients.Group(temp.DeviceId).currentTemp(temp);
        }
    }
}