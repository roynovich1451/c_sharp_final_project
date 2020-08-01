using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    public class UserNotRegisteredFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}