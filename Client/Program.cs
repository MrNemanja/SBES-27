using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contract;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Service";
            String follower = String.Empty;

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
                EndpointIdentity.CreateUpnIdentity("wcfServer"));

            using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
            {
               follower = proxy.CreateDatabase("Baza1.txt");
               Console.WriteLine(follower);
               follower = proxy.CreateDatabase("Baza2.txt");
               Console.WriteLine(follower);
               follower = proxy.DeleteDatabase("Baza1.txt");
               Console.WriteLine(follower);
               follower = proxy.CreateDatabase("Baza2.txt");
               Console.WriteLine(follower);
               follower = proxy.CreateDatabase("Baza1.txt");
               Console.WriteLine(follower);
               follower = proxy.DeleteDatabase("Baza2.txt");
               Console.WriteLine(follower);
            }

            Console.ReadLine();
        }
    }
}
