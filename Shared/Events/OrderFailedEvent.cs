using Shared.Common;

namespace Shared.Events
{
    public class OrderFailedEvent : BaseCorrelation
    {
        public OrderFailedEvent(Guid correlationId) : base(correlationId)
        {
        }

        public int OrderId { get; set; }
        public string Message { get; set; }
    }
}
