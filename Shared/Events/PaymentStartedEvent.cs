using Shared.Common;
using Shared.Datas;

namespace Shared.Events
{
    public class PaymentStartedEvent : BaseCorrelation
    {
        public PaymentStartedEvent(Guid correlationId) : base(correlationId)
        {
        }
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
