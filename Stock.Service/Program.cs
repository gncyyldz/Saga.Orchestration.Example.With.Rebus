
using MongoDB.Driver;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Shared.Common;
using Shared.Settings;
using Stock.Service.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<MongoDBService>();

builder.Services.AddRebus(rebus => rebus
     .Routing(r => r.TypeBased().MapAssemblyOf<BaseCorrelation>(RabbitMQQueueSettings.SagaOrchestrationInputQueueName))
     .Transport(t => t.UseRabbitMq(RabbitMQAMQPUrl.AMQPUrl, inputQueueName: RabbitMQQueueSettings.StockInputQueueName)));

builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

var host = builder.Build();

using var scope = builder.Services.BuildServiceProvider().CreateScope();
var mongoDbService = scope.ServiceProvider.GetRequiredService<MongoDBService>();
if (!await (await mongoDbService.GetCollection<Stock.Service.Models.Stock>().FindAsync(x => true)).AnyAsync())
{
    mongoDbService.GetCollection<Stock.Service.Models.Stock>().InsertOne(new()
    {
        ProductId = 1,
        Count = 200
    });
    mongoDbService.GetCollection<Stock.Service.Models.Stock>().InsertOne(new()
    {
        ProductId = 2,
        Count = 300
    });
    mongoDbService.GetCollection<Stock.Service.Models.Stock>().InsertOne(new()
    {
        ProductId = 3,
        Count = 50
    });
    mongoDbService.GetCollection<Stock.Service.Models.Stock>().InsertOne(new()
    {
        ProductId = 4,
        Count = 10
    });
    mongoDbService.GetCollection<Stock.Service.Models.Stock>().InsertOne(new()
    {
        ProductId = 5,
        Count = 60
    });
}


host.Run();
