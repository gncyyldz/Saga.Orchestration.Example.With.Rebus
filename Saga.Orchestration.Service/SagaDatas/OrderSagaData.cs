using Rebus.Sagas;
using Shared.Common;
using Shared.Datas;

namespace Saga.Orchestration.Service.SagaDatas
{
    public class OrderSagaData : BaseCorrelation, ISagaData
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
