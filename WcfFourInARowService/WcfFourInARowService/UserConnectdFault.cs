using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    [DataContract]
    public class UserConnectdFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}