using Ardalis.GuardClauses;
using Azure.Messaging.ServiceBus;
using BlazorShared.Models;
using Microsoft.eShopWeb.Web.Interfaces;
using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Web.Services;

public class OrderReservator : IOrderReservator
{
    private const string QueueName = "sbq-savingorder";

    private readonly ServiceBusClient _serviceBusClient;

    public OrderReservator(ServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;
    }

    public async Task Reserve(OrderDto order)
    {
        Guard.Against.Null(order, nameof(order));

        await using var sender = _serviceBusClient.CreateSender(QueueName);
        var message = new ServiceBusMessage(JsonConvert.SerializeObject(order))
        {
            ContentType = "application/json"
        };
        await sender.SendMessageAsync(message);
    }
}
