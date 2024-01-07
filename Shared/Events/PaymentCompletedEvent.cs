using Shared.Common;

namespace Shared.Events
{
    public class PaymentCompletedEvent : BaseCorrelation
    {
        public PaymentCompletedEvent(Guid correlationId) : base(correlationId)
        {
        }
    }
}
