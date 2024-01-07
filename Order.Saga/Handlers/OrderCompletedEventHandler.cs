using Order.Saga.Models;
using Rebus.Handlers;
using Shared.Events;

namespace Order.Saga.Handlers
{
    public class OrderCompletedEventHandler(ApplicationDbContext applicationDbContext) : IHandleMessages<OrderCompletedEvent>
    {
        public async Task Handle(OrderCompletedEvent message)
        {
            Order.Saga.Models.Entities.Order order = await applicationDbContext.Orders.FindAsync(message.OrderId);
            if (order != null)
            {
                order.OrderStatus = Models.Enums.OrderStatus.Completed;
                await applicationDbContext.SaveChangesAsync();
            }
        }
    }
}
