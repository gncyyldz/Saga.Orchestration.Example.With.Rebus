
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Shared.Events;
using Shared.Messages;
using Shared.Settings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddRebus(rebus => rebus
     .Routing(r =>
        r.TypeBased()
        .Map<OrderCreatedEvent>(RabbitMQQueueSettings.StockInputQueueName)
        .Map<PaymentStartedEvent>(RabbitMQQueueSettings.PaymentInputQueueName)
        .Map<OrderFailedEvent>(RabbitMQQueueSettings.OrderInputQueueName)
        .Map<OrderCompletedEvent>(RabbitMQQueueSettings.OrderInputQueueName)
        .Map<StockRollbackMessage>(RabbitMQQueueSettings.StockInputQueueName))
     .Transport(t =>
         t.UseRabbitMq(RabbitMQAMQPUrl.AMQPUrl,
         inputQueueName: RabbitMQQueueSettings.SagaOrchestrationInputQueueName))
     .Sagas(s =>
         s.StoreInSqlServer("Server=localhost, 1433;Database=SagaOrchestrationRebusDB;User ID=SA;Password=1q2w3e4r+!;TrustServerCertificate=True",
         dataTableName: "Sagas",
         indexTableName: "SagaIndexes",
         automaticallyCreateTables: true))
     .Timeouts(t =>
         t.StoreInSqlServer("Server=localhost, 1433;Database=SagaOrchestrationRebusDB;User ID=SA;Password=1q2w3e4r+!;TrustServerCertificate=True",
         tableName: "Timeouts",
         automaticallyCreateTables: true)));

builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

var host = builder.Build();
host.Run();
