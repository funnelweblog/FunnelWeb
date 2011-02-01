using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace FunnelWeb.Extensions.WcfDemo.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class DemoService
    {
        [OperationContract]
        [WebGet(UriTemplate = "getdata")]
        public List<string> GetDemoData()
        {
            return new List<string>
                       {
                           "Result1",
                           "Result2"
                       };
        }
    }
}
