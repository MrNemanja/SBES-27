using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Contract
{
    [ServiceContract]
    public interface IDatabaseManagement
    {
        [OperationContract]
        String CreateDatabase(String DatabaseName);

        [OperationContract]
        String DeleteDatabase(String DatabaseName);
    }
}
