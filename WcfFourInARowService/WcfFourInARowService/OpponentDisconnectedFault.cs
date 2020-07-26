using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    internal class OpponentDisconnectedFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}