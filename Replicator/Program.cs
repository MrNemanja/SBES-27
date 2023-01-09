using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using System.ServiceModel;
using System.Threading;

namespace Replicator
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    ChannelFactory<IDatabaseManagement> cfSource = new ChannelFactory<IDatabaseManagement>("Source");
                    ChannelFactory<IDatabaseManagement> cfDestination = new ChannelFactory<IDatabaseManagement>("Destination");
                    IDatabaseManagement kSource = cfSource.CreateChannel();
                    IDatabaseManagement kDestination = cfDestination.CreateChannel();

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
