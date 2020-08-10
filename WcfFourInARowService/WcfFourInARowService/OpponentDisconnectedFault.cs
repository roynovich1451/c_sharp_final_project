using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    [DataContract]
    internal class OpponentDisconnectedFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}