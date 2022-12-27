using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contract;

namespace Client
{
    public class ClientProxy: ChannelFactory<IDatabaseManagement>, IDatabaseManagement, IDisposable
    {
        IDatabaseManagement factory;

        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public string CreateDatabase(string DatabaseName)
        {
            String retstr = String.Empty;
            try
            {
                retstr = factory.CreateDatabase(DatabaseName);
                Console.WriteLine();
                return retstr;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return e.Message;
            }
        }

        public string DeleteDatabase(string DatabaseName)
        {
            String retstr = String.Empty;
            try
            {
                retstr = factory.DeleteDatabase(DatabaseName);
                return retstr;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return e.Message;
            }
        }

        public string Read(string DatabaseName, string region, string grad)
        {
            string retstr = String.Empty;
            try
            {
                retstr = factory.Read(DatabaseName, region, grad);
                Console.WriteLine();
                return retstr;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return e.Message;
            }
        }

        public string Write(string DatabaseName, string region, string grad, int godina, int potrosnja)
        {
            string retstr = String.Empty;
            try
            {
                retstr = factory.Write(DatabaseName, region, grad,godina,potrosnja);
                Console.WriteLine();
                return retstr;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return e.Message;
            }
        }
    }
}
