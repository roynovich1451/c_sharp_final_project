using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    [DataContract]
    public class EmptyFieldFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}