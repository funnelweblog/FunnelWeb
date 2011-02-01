using System.Runtime.Serialization;

namespace FunnelWeb.Extensions.WcfDemo.Model
{
    [DataContract]
    public class WcfDemoData
    {
        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual string Data { get; set; }
    }
}
