using MongoDB.Driver;
using Rebus.Handlers;
using Shared.Messages;
using Stock.Service.Services;

namespace Stock.Service.Handlers
{
    public class StockRollbackMessageHandler(MongoDBService mongoDBService) : IHandleMessages<StockRollbackMessage>
    {
        public async Task Handle(StockRollbackMessage message)
        {
            var stockCollection = mongoDBService.GetCollection<Models.Stock>();

            foreach (var orderItem in message.OrderItems)
            {
                var stock = await (await stockCollection.FindAsync(x => x.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();

                stock.Count += orderItem.Count;
                await stockCollection.FindOneAndReplaceAsync(x => x.ProductId == orderItem.ProductId, stock);
            }
        }
    }
}
