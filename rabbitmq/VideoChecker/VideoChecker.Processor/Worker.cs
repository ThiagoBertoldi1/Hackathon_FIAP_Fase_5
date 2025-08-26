using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using SharedEntities;
using SharedEntities.Enums;
using System.Text;
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
            var factory = new ConnectionFactory { HostName = _configuration["RabbitMQ:Host"]! };
            using var conn = await factory.CreateConnectionAsync(stoppingToken);
            using var channel = await conn.CreateChannelAsync(cancellationToken: stoppingToken);

            _logger.LogInformation("Conectado ao RabbitMQ");

            await Processar(channel, "Queue.CreatedJob", stoppingToken);

            while (!stoppingToken.IsCancellationRequested) { }
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

    private async Task Processar(IChannel channel, string queue, CancellationToken cancellationToken)
    {
        await channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await channel.BasicQosAsync(0, prefetchCount: 10, global: false, cancellationToken);

        _logger.LogInformation("Waiting messages...");

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var entity = JsonConvert.DeserializeObject<VideoJobCreated>(message)!;

            _logger.LogInformation("{m}", message);

            try
            {
                var videoInserted = await _repository.SaveVideo(entity.Video);
                if (!videoInserted)
                    throw new Exception("Erro ao salvar vídeo");

                var statusChanged = await _repository.UpdateJobStatus(entity.JobId, StatusEnum.Processing, "Vídeo sendo processado");
                if (!statusChanged)
                    throw new Exception("Erro ao atualizar status do job");

                await ProcessaVideo();

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

                await consumer.Channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await channel.BasicConsumeAsync(
            queue,
            autoAck: false,
            consumer,
            cancellationToken);
    }

    private async Task ProcessaVideo()
    {
        // processar vídeo
        // // quebrar vídeo em frames
        // // salvar qr code encontrado
        // // trocar status do job
    }
}