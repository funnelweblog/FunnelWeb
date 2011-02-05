using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using FunnelWeb.Extensions.WcfDemo.Model;
using NHibernate;
using NHibernate.Linq;

namespace FunnelWeb.Extensions.WcfDemo.Services
{
    [ServiceContract]
    public class DemoService
    {
        private readonly ISession _session;

        public DemoService(ISession session)
        {
            _session = session;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getdata")]
        public List<WcfDemoData> GetDemoData()
        {
            return _session
                .Linq<WcfDemoData>()
                .ToList();
        }
    }
}
