using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using SharedEntities;
using SharedEntities.Enums;
using System.Text;
using System.Text.Json;
using VideoChecker.Processor.Interfaces;

namespace VideoChecker.Processor;

public class Worker(
    IConfiguration configuration,
    ILogger<Worker> logger,
    IQueueRepository repository) : BackgroundService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<Worker> _logger = logger;
    private readonly IQueueRepository _repository = repository;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) => await InitAsync(stoppingToken);

    private async Task InitAsync(CancellationToken stoppingToken, int tries = 1)
    {
        try
        {
#if DEBUG
            var factory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@localhost:5672/") };
#else
            var factory = new ConnectionFactory { HostName = _configuration["RabbitMQ:Host"]! };
#endif
            using var conn = await factory.CreateConnectionAsync(stoppingToken);
            using var channel = await conn.CreateChannelAsync(cancellationToken: stoppingToken);

            _logger.LogInformation("Conectado ao RabbitMQ");

            await Processar(channel, "Queue.CreatedJob", stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (BrokerUnreachableException)
        {
            _logger.LogInformation("Falha ao conectar ao RabbitMQ - Tentativa {t}", tries);

            if (tries >= 5)
                throw new Exception("Erro ao conectar no rabbitMQ, tentativas excedidas");

            Thread.Sleep(15_000);

            await InitAsync(stoppingToken, tries + 1);
        }
    }

    private async Task Processar(IChannel channel, string queue, CancellationToken stoppingToken)
    {
        await channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await channel.BasicQosAsync(0, prefetchCount: 10, global: false, stoppingToken);

        _logger.LogInformation("Waiting messages...");

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var entity = JsonSerializer.Deserialize<VideoJobCreated>(message)!;

                _logger.LogInformation("{m}", message);

                try
                {
                    await ProcessaVideo(entity.JobId, stoppingToken);

                    await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem");
                    try
                    {
                        await _repository.UpdateJobStatus(entity.JobId, StatusEnum.Failed, $"Falha: {ex.Message}");
                    }
                    catch (Exception uex)
                    {
                        _logger.LogError(uex, "Erro ao atualizar status para Failed");
                    }

                    await consumer.Channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desserializar objeto");
                await consumer.Channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await channel.BasicConsumeAsync(
            queue,
            autoAck: false,
            consumer,
            stoppingToken);
    }

    private async Task ProcessaVideo(ObjectId jobId, CancellationToken stoppingToken)
    {
        var statusChanged = await _repository.UpdateJobStatus(jobId, StatusEnum.Processing, "Vídeo sendo processado");
        if (!statusChanged)
            throw new Exception("Erro ao atualizar status do job");

        using var videoStream = await _repository.DownloadVideo(jobId);

        // Salva o vídeo em frames PNG
        await VideoFrames.SaveFramesAsPngAsync(videoStream);
        // TODO:
        // Identificar QRCode nos frames
        // Salvar os frames com QRCode no banco

        statusChanged = await _repository.UpdateJobStatus(jobId, StatusEnum.Completed, "Vídeo processado");
        if (!statusChanged)
            throw new Exception("Erro ao atualizar status do job");
    }
}