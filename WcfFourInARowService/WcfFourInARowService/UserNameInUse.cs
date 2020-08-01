using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    internal class UserNameInUse
    {
        [DataMember]
        public string Details { get; set; }
    }
}