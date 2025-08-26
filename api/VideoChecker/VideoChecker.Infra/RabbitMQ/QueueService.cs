﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace VideoChecker.Infra.RabbitMQ;

public class QueueService : IQueueService
{
    public async Task Publish<T>(string? queue, T data)
    {
        if (string.IsNullOrEmpty(queue))
            throw new ArgumentNullException(nameof(queue));

        var factory = new ConnectionFactory { HostName = "rabbitmq" };
        using var conn = await factory.CreateConnectionAsync();
        using var channel = await conn.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue!,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

        await channel.BasicPublishAsync(
               exchange: string.Empty,
               routingKey: queue!,
               mandatory: true,
               basicProperties: new BasicProperties { Persistent = true },
               body);
    }
}
