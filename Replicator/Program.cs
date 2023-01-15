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

                    cfDestination.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
                    cfDestination.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
                    cfDestination.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
                    cfDestination.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfclient");

                    cfSource.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
                    cfSource.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();
                    cfSource.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
                    cfSource.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfservice");

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
