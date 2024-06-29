using System.Reflection;
using jyu.demo.Common.Extension;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace jyu.demo.WorkerDomain.Works.SampleServiceTask;

public class SampleWorkServiceTaskFactory : IWorkServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SampleWorkServiceTaskFactory(
        IServiceProvider serviceProvider
    )
    {
        _serviceProvider = serviceProvider
                           ?? throw new ArgumentNullException(
                               nameof(serviceProvider)
                           );
    }

    public IWorkBase GetServiceInstance(
        string serviceTaskTopicName
    )
    {
        SampleServiceTaskTopicName topicName = serviceTaskTopicName.ConvertStrToEnum<SampleServiceTaskTopicName>();

        IEnumerable<IWorkBase> services = _serviceProvider.GetServices<IWorkBase>();

        var serviceInstance = services.FirstOrDefault(item =>
            (
                item.GetType().GetCustomAttributes<SampleServiceTaskAttribute>().FirstOrDefault() as
                    SampleServiceTaskAttribute
            )?.TopicName == topicName
        ) ?? throw new ArgumentNullException(serviceTaskTopicName);

        return serviceInstance;
    }
}