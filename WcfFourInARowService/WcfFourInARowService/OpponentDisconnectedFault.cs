using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace WcfFourInARowService
{
    [DataContract]
    internal class OpponentDisconnectedFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}