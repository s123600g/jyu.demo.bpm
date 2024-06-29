namespace jyu.demo.SampleServiceTaskWorker;

using System.Reflection;
using BpmDomain;
using Camunda.Models;
using NLog.Extensions.Logging;
using Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        var config = builder.Configuration;
        var log = builder.Logging;
        var services = builder.Services;

        NLogLoggingConfiguration nlogConfig = new NLogLoggingConfiguration(
            config.GetSection("NLog")
        );
        log.AddNLog(nlogConfig);

        services.Configure<CamundaConfigOptions>(
            config.GetSection("CamundaSettings")
        );

        services.AddHttpClient();

        services.AddWorkerRelatedServices();
        
        // Work Instance注入
        services.AddHostedService<ServiceTaskWorker>();

        var host = builder.Build();
        host.Run();
    }
}