using System.Runtime.Serialization;

namespace WcfFourInARowService
{
    internal class WeakPassword
    {
        [DataMember]
        public string Details { get; set; }
    }
}