namespace jyu.demo.SampleServiceTaskWorker;

using System.Reflection;
using BpmDomain;
using BpmDomain.SampleServiceTask;
using BpmDomain.SampleServiceTask.ServiceTaskAttribute;
using BPMN.Camunda.Models;
using BPMN.Camunda.Services.BpmEngingClient;
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
        services.AddSingleton<ICamundaEngineClient, CamundaEngineClient>();

        #region 對應Topic Service注入

        var type = typeof(IServiceTaskWork);

        IEnumerable<Type> serviceTypes = type.Assembly.GetTypes()
                                             .Where(item =>
                                                 item.GetInterfaces().Contains(type)
                                             );

        foreach (
            var item in serviceTypes
        )
        {
            var attribute = item.GetCustomAttribute<SampleServiceTaskAttribute>();

            if (
                attribute != null
            )
            {
                services.Add(
                    item: new ServiceDescriptor(
                        serviceType: typeof(IServiceTaskWork)
                        , implementationType: item
                        , lifetime: ServiceLifetime.Singleton
                    )
                );
            }
        }

        #endregion

        // 對應Topic Service Factory注入
        services.AddSingleton<IServiceTaskWorkFactory, SampleServiceTaskFactory>();

        // Work Instance注入
        services.AddHostedService<ServiceTaskWorker>();

        var host = builder.Build();
        host.Run();
    }
}