using Shared.Common;
using Shared.Datas;

namespace Shared.Events
{
    public class StockReservedEvent : BaseCorrelation
    {
        public StockReservedEvent(Guid correlationId) : base(correlationId) { }
        public List<OrderItem> OrderItems { get; set; }
    }
}
