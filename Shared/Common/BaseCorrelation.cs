using System.Runtime.Serialization;

namespace Shared.Common
{
    [Serializable]
    public class BaseCorrelation
    {
        public BaseCorrelation() { }
        public BaseCorrelation(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        [DataMember]
        public Guid CorrelationId { get; set; }
    }
}
