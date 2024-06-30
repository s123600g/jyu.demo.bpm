using System.Reflection;
using jyu.demo.Camunda.Services;
using jyu.demo.WorkerDomain;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Attributes;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Interface;

namespace jyu.demo.ReviewProcessFlowWorker;

public static class WorkerServiceCollectionExtension
{
    public static IServiceCollection AddWorkerRelatedServices(
        this IServiceCollection services
    )
    {
        services.AddScoped<ICamundaEngineClient, CamundaEngineClient>();

        #region 對應Topic Service注入

        var type = typeof(IReviewProcessFlowWorkBase);

        IEnumerable<Type> serviceTypes = type.Assembly.GetTypes()
            .Where(item =>
                item.GetInterfaces().Contains(type)
            );

        foreach (
            var item in serviceTypes
        )
        {
            var attribute = item.GetCustomAttribute<ReviewProcessFlowAttribute>();

            if (
                attribute != null
            )
            {
                services.Add(
                    item: new ServiceDescriptor(
                        serviceType: typeof(IReviewProcessFlowWorkBase)
                        , implementationType: item
                        , lifetime: ServiceLifetime.Scoped
                    )
                );
            }
        }

        #endregion

        // 對應Topic Service Factory注入
        services.AddScoped<IWorkServiceFactory<IReviewProcessFlowWorkBase>, ReviewProcessFlowFactory>();

        return services;
    }
}