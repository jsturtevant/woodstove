namespace WebJob1
{
    using System.Diagnostics;
    using System.Runtime.Serialization.Json;
    using System.Threading;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using WoodStove.Core;

    public class SimpleEventProcessor : IEventProcessor
    {

        PartitionContext partitionContext;

        SignalrTemprature signalrTemprature;

        public SimpleEventProcessor()
        {
            signalrTemprature = new SignalrTemprature();

            signalrTemprature.SetupSignalRConnection();
           

        }

        public async Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine(string.Format("SimpleEventProcessor initialize.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset));

            await signalrTemprature.Start();

            this.partitionContext = context;
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> events)
        {
            try
            {
                foreach (EventData eventData in events)
                {
                    MetricEvent newData = this.DeserializeEventData(eventData);
                  
                    Console.WriteLine(string.Format("Message received.  Partition: '{0}', Device: '{1}', Data: '{2}', SecondaryData: '{3}'",
                        this.partitionContext.Lease.PartitionId, newData.DeviceId, newData.Temperature, newData.RoomTemperature));

                    signalrTemprature.SendMessage(new DeviceReading(newData.DeviceId, newData.Temperature, newData.RoomTemperature,eventData.EnqueuedTimeUtc));

                    // tell event hubs which event you processed last (save game)
                    await context.CheckpointAsync(eventData);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error in processing: " + exp.Message);
            }
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine(string.Format("Processor Shuting Down.  Partition '{0}', Reason: '{1}'.", this.partitionContext.Lease.PartitionId, reason.ToString()));

            signalrTemprature.Stop();
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        MetricEvent DeserializeEventData(EventData eventData)
        {
            return JsonConvert.DeserializeObject<MetricEvent>(Encoding.UTF8.GetString(eventData.GetBytes()));
        }
    }


    public class MetricEvent
    {
        public string DeviceId { get; set; }
        public int RoomTemperature { get;  set; }
        public int Temperature { get; set; }
    }
}
