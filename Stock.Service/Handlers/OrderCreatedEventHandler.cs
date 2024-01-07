using MongoDB.Driver;
using Rebus.Bus;
using Rebus.Handlers;
using Shared.Events;
using Stock.Service.Services;

namespace Stock.Service.Handlers
{
    public class OrderCreatedEventHandler(MongoDBService mongoDBService, IBus bus) : IHandleMessages<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent message)
        {
            List<bool> stockResults = new();
            var stockCollection = mongoDBService.GetCollection<Models.Stock>();

            foreach (var orderItem in message.OrderItems)
                stockResults.Add(await (await stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count >=
(long)orderItem.Count)).AnyAsync());

            if (stockResults.TrueForAll(s => s.Equals(true)))
            {
                foreach (var orderItem in message.OrderItems)
                {
                    var stock = await (await stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();

                    stock.Count -= orderItem.Count;

                    await stockCollection.FindOneAndReplaceAsync(x => x.ProductId == orderItem.ProductId, stock);
                }

                StockReservedEvent stockReservedEvent = new(message.CorrelationId)
                {
                    OrderItems = message.OrderItems
                };

                await bus.Send(stockReservedEvent);
            }
            else
            {
                StockNotReservedEvent stockNotReservedEvent = new(message.CorrelationId)
                {
                    Message = "Stok yetersiz...."
                };

                await bus.Send(stockNotReservedEvent);
            }
        }
    }
}
