using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    public class UserNameInUse
    {
        [DataMember]
        public string Details { get; set; }
    }
}