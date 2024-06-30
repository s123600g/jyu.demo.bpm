using System.Reflection;
using jyu.demo.Common.Extension;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Attributes;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Enums;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace jyu.demo.WorkerDomain.Works.ReviewProcessFlow;

public class ReviewProcessFlowFactory : IWorkServiceFactory<IReviewProcessFlowWorkBase>
{
    private readonly IServiceProvider _serviceProvider;

    public ReviewProcessFlowFactory(
        IServiceProvider serviceProvider
    )
    {
        _serviceProvider = serviceProvider
                           ?? throw new ArgumentNullException(
                               nameof(serviceProvider)
                           );
    }

    public IReviewProcessFlowWorkBase GetServiceInstance(
        string serviceTaskTopicName
    )
    {
        ReviewProcessFlowTopicName topicName = serviceTaskTopicName.ConvertStrToEnum<ReviewProcessFlowTopicName>();

        IEnumerable<IReviewProcessFlowWorkBase> services = _serviceProvider.GetServices<IReviewProcessFlowWorkBase>();

        var serviceInstance = services.FirstOrDefault(item =>
            (
                item.GetType().GetCustomAttributes<ReviewProcessFlowAttribute>().FirstOrDefault()
            )?.TopicName == topicName
        ) ?? throw new ArgumentNullException(serviceTaskTopicName);

        return serviceInstance;
    }
}