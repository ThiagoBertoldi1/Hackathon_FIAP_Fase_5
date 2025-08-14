using VideoChecker.Processor;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

Console.WriteLine(builder.Environment.EnvironmentName);

var host = builder.Build();
host.Run();
