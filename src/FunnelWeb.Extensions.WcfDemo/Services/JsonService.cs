using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using FunnelWeb.Extensions.WcfDemo.Model;
using NHibernate;
using NHibernate.Linq;

namespace FunnelWeb.Extensions.WcfDemo.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class JsonService
    {
        private readonly ISession _session;

        public JsonService(ISession session)
        {
            _session = session;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getdata", ResponseFormat = WebMessageFormat.Json)]
        public List<WcfDemoData> GetDemoData()
        {
            return _session
                .Linq<WcfDemoData>()
                .ToList();
        }
    }
}
