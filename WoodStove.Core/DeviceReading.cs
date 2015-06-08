using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodStove.Core
{
    using System.Diagnostics;
    using System.Dynamic;

    [DebuggerDisplay("Device {DeviceId} has Temp {Value} at {Stamp}")]
    public class DeviceReading
    {
        private DateTime timeStamp;

        private int value;

        private string deviceId;

        public DeviceReading(string deviceId, int value, DateTime timeStamp)
        {
            this.deviceId = deviceId;
            this.value = value;
            this.timeStamp = timeStamp;
        }

        public string DeviceId
        {
            get
            {
                return this.deviceId;
            }

        }

        public int Value
        {
            get
            {
                return this.value;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return this.timeStamp;
            }
        }

        public override string ToString()
        {
            return string.Format("Device {0} has Temp {1} at {2}",
                                    this.DeviceId, this.Value, this.TimeStamp);
        }

        public static DeviceReading Create(string deviceId, int value)
        {
            return new DeviceReading(deviceId, value, DateTime.Now);
        }
    }
}