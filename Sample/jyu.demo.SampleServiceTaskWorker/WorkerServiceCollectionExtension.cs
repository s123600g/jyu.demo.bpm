using System.Reflection;
using jyu.demo.WorkerDomain;
using jyu.demo.WorkerDomain.Works.SampleServiceTask;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Attributes;
using jyu.demo.Camunda.Services;

namespace jyu.demo.SampleServiceTaskWorker;

public static class WorkerServiceCollectionExtension
{
    public static IServiceCollection AddWorkerRelatedServices(
        this IServiceCollection services
    )
    {
        services.AddSingleton<ICamundaEngineClient, CamundaEngineClient>();

        #region 對應Topic Service注入

        var type = typeof(IWorkBase);

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
                        serviceType: typeof(IWorkBase)
                        , implementationType: item
                        , lifetime: ServiceLifetime.Singleton
                    )
                );
            }
        }

        #endregion

        // 對應Topic Service Factory注入
        services.AddSingleton<IWorkServiceFactory, SampleWorkServiceTaskFactory>();

        return services;
    }
}