using Shared.Common;

namespace Shared.Events
{
    public class StockNotReservedEvent : BaseCorrelation
    {
        public StockNotReservedEvent(Guid correlationId) : base(correlationId) { }
        public string Message { get; set; }
    }
}
