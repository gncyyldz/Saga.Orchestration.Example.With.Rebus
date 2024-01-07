using Shared.Common;
using Shared.Datas;

namespace Shared.Events
{
    public class PaymentFailedEvent : BaseCorrelation
    {
        public List<OrderItem> OrderItems { get; set; }
        public string Messages { get; set; }
    }
}
