using Shared.Common;
using Shared.Datas;

namespace Shared.Events
{
    public class OrderStartedEvent : BaseCorrelation
    {
        public OrderStartedEvent() { }
        public OrderStartedEvent(Guid correlationId) : base(correlationId) { }
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
