using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Order.API.Models.Entities;
using Order.API.Models.Enums;
using Order.API.ViewModels;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Shared.Common;
using Shared.Events;
using Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));

builder.Services.AddRebus(rebus => rebus
     .Routing(r => r.TypeBased().MapAssemblyOf<BaseCorrelation>(RabbitMQQueueSettings.SagaOrchestrationInputQueueName))
     .Transport(t => t.UseRabbitMq(RabbitMQAMQPUrl.AMQPUrl, inputQueueName: RabbitMQQueueSettings.OrderInputQueueName)));

builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/create-order", async (IBus bus, ApplicationDbContext context, CreateOrderVM model) =>
{
    Order.API.Models.Entities.Order order = new()
    {
        BuyerId = model.BuyerId,
        OrderItems = model.OrderItems.Select(oi => new OrderItem
        {
            Count = oi.Count,
            Price = oi.Price,
            ProductId = oi.ProductId
        }).ToList(),
        OrderStatus = OrderStatus.Suspend,
        TotalPrice = model.OrderItems.Sum(oi => oi.Count * oi.Price),
        CreatedDate = DateTime.Now
    };

    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();

    OrderStartedEvent orderStartedEvent = new()
    {
        OrderId = order.Id,
        BuyerId = model.BuyerId,
        TotalPrice = order.TotalPrice,
        OrderItems = model.OrderItems.Select(oi => new Shared.Datas.OrderItem
        {
            Count = oi.Count,
            Price = oi.Price,
            ProductId = oi.ProductId
        }).ToList(),
        CorrelationId = Guid.NewGuid(),
    };

    await bus.Send(orderStartedEvent);

    return Results.Created();
});

app.Run();
