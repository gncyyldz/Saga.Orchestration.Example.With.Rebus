
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Shared.Common;
using Shared.Settings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddRebus(rebus => rebus
     .Routing(r => r.TypeBased().MapAssemblyOf<BaseCorrelation>(RabbitMQQueueSettings.SagaOrchestrationInputQueueName))
     .Transport(t => t.UseRabbitMq(RabbitMQAMQPUrl.AMQPUrl, inputQueueName: RabbitMQQueueSettings.PaymentInputQueueName)));

builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

var host = builder.Build();
host.Run();
