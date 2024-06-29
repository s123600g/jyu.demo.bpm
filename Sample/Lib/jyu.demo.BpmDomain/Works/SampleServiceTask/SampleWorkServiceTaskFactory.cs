using System.Reflection;
using jyu.demo.BpmDomain.Works.SampleServiceTask.Attributes;
using jyu.demo.Common.Extension;
using jyu.demo.SampleServiceTaskWorker.Services;
using Microsoft.Extensions.DependencyInjection;

namespace jyu.demo.BpmDomain.Works.SampleServiceTask;

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