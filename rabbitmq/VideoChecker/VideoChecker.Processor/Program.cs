using VideoChecker.Processor;
using VideoChecker.Processor.Interfaces;
using VideoChecker.Processor.Repository;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IQueueRepository, QueueRepository>();

Console.WriteLine(builder.Environment.EnvironmentName);

var host = builder.Build();
host.Run();
