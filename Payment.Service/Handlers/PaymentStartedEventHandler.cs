using Rebus.Bus;
using Rebus.Handlers;
using Shared.Events;

namespace Payment.Service.Handlers
{
    public class PaymentStartedEventHandler(IBus bus) : IHandleMessages<PaymentStartedEvent>
    {
        public async Task Handle(PaymentStartedEvent message)
        {
            //Ödeme başarılı...
            await bus.Send(new PaymentCompletedEvent(message.CorrelationId));
        }
    }
}
