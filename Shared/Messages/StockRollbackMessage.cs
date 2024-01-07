using Shared.Common;
using Shared.Datas;

namespace Shared.Messages
{
    public class StockRollbackMessage : BaseCorrelation
    {
        public StockRollbackMessage(Guid correlationId) : base(correlationId) { }
        public List<OrderItem> OrderItems { get; set; }
    }
}
