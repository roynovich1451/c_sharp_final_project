using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfFourInARowService
{
    [DataContract]
    public class CallbackFailureFault
    {
        [DataMember]
        public string Details { get; set; }
    }
}
