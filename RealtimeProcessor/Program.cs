using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using System.Diagnostics;
using WoodStove.Core;

namespace WebJob1
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            Receiver r = new Receiver("woodstove2", ConfigurationManager.ConnectionStrings["EventHubConnectionString"].ConnectionString);
            r.MessageProcessingWithPartitionDistribution();

            Console.WriteLine("Press enter key to stop worker.");
            Console.ReadLine();

            var host =  new JobHost();
            host.RunAndBlock();
        }

     
    }
}
