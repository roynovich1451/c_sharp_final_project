using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    public class IncorrectPasswordFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}