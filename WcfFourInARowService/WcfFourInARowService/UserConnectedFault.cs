using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    [DataContract]
    public class UserConnectedFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}