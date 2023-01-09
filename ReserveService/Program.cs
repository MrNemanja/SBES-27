using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contract;
using Service;
using System.ServiceModel.Description;
using System.IdentityModel.Policy;
using SecurityManager;

namespace ReserveService
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:10000/IDatabaseManagement";

            ServiceHost host = new ServiceHost(typeof(ImplDatabaseManag));
            host.AddServiceEndpoint(typeof(IDatabaseManagement), binding, address);

            // podesavamo custom polisu, odnosno nas objekat principala
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            host.Open();

            Console.WriteLine("Servis je pokrenut.");

            Console.ReadLine();
        }
    }
}
