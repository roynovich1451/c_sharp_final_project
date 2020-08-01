using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    public class UserNameTakenFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}