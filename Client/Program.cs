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
            string address = "net.tcp://localhost:9999/IDatabaseManagement";
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

               follower = proxy.Write("Baza1.txt","Backa","Novi Sad",2022,400);
               Console.WriteLine(follower);
               follower = proxy.Write("Baza1.txt", "Backa", "Sombor", 2022, 5500);
               Console.WriteLine(follower);
               follower = proxy.Write("Baza1.txt", "Backa", "Novi Sad", 2022, 7700);
               Console.WriteLine(follower);
               follower = proxy.Write("Baza1.txt", "Srem", "Sombor", 2022, 400);
               Console.WriteLine(follower);
               follower = proxy.Write("Baza.txt", "sss", "asda", 2022, 0);
               Console.WriteLine(follower);

               follower = proxy.Write("Baza2.txt", "Sumadija", "Lazarevac", 2022, 1500);
               Console.WriteLine(follower);
               follower = proxy.Write("Baza2.txt", "Sumadija", "Kragujevac", 2022, 4200);
               Console.WriteLine(follower);
               follower = proxy.Write("Baza2.txt", "Sumadija", "Beograd", 2022, 700);
               Console.WriteLine(follower);

               follower = proxy.Read("Baza1.txt", "Backa", "Novi Sad");
               Console.WriteLine(follower);

                follower = proxy.ModifyData("Baza1.txt", 3, "Backa", "Sombor", 2022, 400);
               Console.WriteLine(follower);
               follower = proxy.ModifyData("Baza1.txt", 0, "Backa", "Novi Sad", 2022, 8000);
               Console.WriteLine(follower);

               follower = proxy.ArchiveDatabases("Baza1.txt");
               Console.WriteLine(follower);
               follower = proxy.ArchiveDatabases("Baza2.txt");
               Console.WriteLine(follower);


            }

            Console.ReadLine();
        }
    }
}
