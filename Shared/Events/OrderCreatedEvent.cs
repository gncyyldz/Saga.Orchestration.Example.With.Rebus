using Shared.Common;
using Shared.Datas;

namespace Shared.Events
{
    public class OrderCreatedEvent : BaseCorrelation
    {
        public OrderCreatedEvent(Guid correlationId) : base(correlationId) { }
        public List<OrderItem>? OrderItems { get; set; }
    }
}
