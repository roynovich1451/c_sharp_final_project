using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    public class InccorectPasswordFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}