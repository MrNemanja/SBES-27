using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using System.ServiceModel;
using System.Threading;
using SecurityManager;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace Replicator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<String, Dictionary<int, Data>> repdata = new Dictionary<string, Dictionary<int, Data>>();

            while (true)
            {
                try
                {
                    ChannelFactory<IDatabaseManagement> cfSource = new ChannelFactory<IDatabaseManagement>("Source");
                    ChannelFactory<IDatabaseManagement> cfDestination = new ChannelFactory<IDatabaseManagement>("Destination");

                    IDatabaseManagement kSource = cfSource.CreateChannel();
                    IDatabaseManagement kDestination = cfDestination.CreateChannel();

                    repdata = kSource.ReadData();
                    Dictionary<String, Dictionary<int, Data>> data = kSource.ReadData();
                    kDestination.WriteData(data);

                    Thread.Sleep(5000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
