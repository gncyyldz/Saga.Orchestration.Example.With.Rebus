using Microsoft.EntityFrameworkCore;
using Order.Saga.Models;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Shared.Common;
using Shared.Settings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));

builder.Services.AddRebus(rebus => rebus
     .Routing(r => r.TypeBased().MapAssemblyOf<BaseCorrelation>(RabbitMQQueueSettings.SagaOrchestrationInputQueueName))
     .Transport(t => t.UseRabbitMq(RabbitMQAMQPUrl.AMQPUrl, inputQueueName: RabbitMQQueueSettings.OrderInputQueueName)));

builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

var host = builder.Build();
host.Run();
