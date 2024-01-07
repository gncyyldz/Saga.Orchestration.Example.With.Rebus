using Order.Saga.Models;
using Rebus.Handlers;
using Shared.Events;
using System;

namespace Order.Saga.Handlers
{
    public class OrderFailedEventHandler(ApplicationDbContext applicationDbContext) : IHandleMessages<OrderFailedEvent>
    {
        public async Task Handle(OrderFailedEvent message)
        {
            Order.Saga.Models.Entities.Order order = await applicationDbContext.Orders.FindAsync(message.OrderId);
            if (order != null)
            {
                order.OrderStatus = Order.Saga.Models.Enums.OrderStatus.Fail;
                await applicationDbContext.SaveChangesAsync();
            }
        }
    }
}
