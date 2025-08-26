namespace VideoChecker.Infra.RabbitMQ;

public interface IQueueService
{
    Task Publish<T>(string? queue, T data);
}
