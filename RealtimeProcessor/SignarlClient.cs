using Microsoft.AspNet.SignalR.Client;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodStove.Core;

namespace WebJob1
{
    public class SignalrTemprature
    {
        private IHubProxy tempHubProxy;
        private HubConnection hubConnection;


        public async Task Start()
        {
            await this.hubConnection.Start();
        }

        public void Stop()
        {
            this.hubConnection.Stop();
        }

        public void SendMessage(DeviceReading reading)
        {
            this.tempHubProxy.Invoke("UpdateDeviceReading", reading);
        }

        public void SetupSignalRConnection()
        {
            try
            {

                string signalrHub = CloudConfigurationManager.GetSetting("Signalr.TempHub");

                Trace.TraceInformation("signalr connection " + signalrHub);
                this.hubConnection = new HubConnection(signalrHub);
                this.tempHubProxy = this.hubConnection.CreateHubProxy("TempHub");

                this.hubConnection.StateChanged += hubConnection_StateChanged;
            }
            catch (Exception ex)
            {
                Trace.TraceError(String.Format("Error connecting to signalr: {0}", ex));
            }

        }

        private static void hubConnection_StateChanged(StateChange obj)
        {
            switch (obj.NewState)
            {
                case ConnectionState.Connecting:
                    break;
                case ConnectionState.Connected:
                    Console.WriteLine("Connected");
                    break;
                case ConnectionState.Reconnecting:
                    break;
                case ConnectionState.Disconnected:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


        }
    }
}
