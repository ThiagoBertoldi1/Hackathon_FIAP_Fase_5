using SharedEntities;
using System.Text.Json;
using VideoChecker.Processor;
using VideoChecker.Processor.Interfaces;
using VideoChecker.Processor.Repository;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IQueueRepository, QueueRepository>();

var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
jsonOptions.Converters.Add(new ObjectIdSystemTextConverter());
builder.Services.AddSingleton(jsonOptions);

Console.WriteLine(builder.Environment.EnvironmentName);

var host = builder.Build();
host.Run();
