using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Saga.Orchestration.Service.SagaDatas;
using Shared.Events;
using Shared.Messages;

namespace Saga.Orchestration.Service.Sagas
{
    public class OrderSaga(IBus bus) :
        Saga<OrderSagaData>,
        IAmInitiatedBy<OrderStartedEvent>,
        IHandleMessages<StockReservedEvent>,
        IHandleMessages<StockNotReservedEvent>,
        IHandleMessages<PaymentCompletedEvent>,
        IHandleMessages<PaymentFailedEvent>
    {
        public async Task Handle(OrderStartedEvent message)
        {
            if (!IsNew)
                return;

            Data.OrderId = message.OrderId;
            Data.BuyerId = message.BuyerId;
            Data.OrderItems = message.OrderItems;
            Data.TotalPrice = message.TotalPrice;

            await bus.Send(new OrderCreatedEvent(message.CorrelationId)
            {
                OrderItems = message.OrderItems,
            });
        }

        public async Task Handle(StockReservedEvent message)
        {
            await bus.Send(new PaymentStartedEvent(message.CorrelationId)
            {
                OrderItems = message.OrderItems,
                TotalPrice = Data.TotalPrice
            });
        }

        public async Task Handle(StockNotReservedEvent message)
        {
            await bus.Send(new OrderFailedEvent(message.CorrelationId)
            {
                Message = message.Message,
                OrderId = Data.OrderId
            });
        }

        public async Task Handle(PaymentCompletedEvent message)
        {
            await bus.Send(new OrderCompletedEvent()
            {
                OrderId = Data.OrderId
            });

            MarkAsComplete();
        }

        public async Task Handle(PaymentFailedEvent message)
        {
            await bus.Send(new OrderFailedEvent(message.CorrelationId)
            {
                OrderId = Data.OrderId,
                Message = message.Messages
            });

            await bus.Send(new StockRollbackMessage(message.CorrelationId)
            {
                OrderItems = message.OrderItems
            });
        }

        protected override void CorrelateMessages(ICorrelationConfig<OrderSagaData> config)
        {
            config.Correlate<OrderStartedEvent>(
                orderStartedEvent => orderStartedEvent.CorrelationId,
                orderSagaData => orderSagaData.CorrelationId);

            config.Correlate<StockReservedEvent>(
                stockReservedEvent => stockReservedEvent.CorrelationId,
                orderSagaData => orderSagaData.CorrelationId);

            config.Correlate<StockNotReservedEvent>(
                stockNotReservedEvent => stockNotReservedEvent.CorrelationId,
                orderSagaData => orderSagaData.CorrelationId);

            config.Correlate<PaymentCompletedEvent>(
                paymentCompletedEvent => paymentCompletedEvent.CorrelationId,
                orderSagaData => orderSagaData.CorrelationId);

            config.Correlate<PaymentFailedEvent>(
                paymentFailedEvent => paymentFailedEvent.CorrelationId,
                orderSagaData => orderSagaData.CorrelationId);
        }
    }
}
