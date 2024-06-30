using System.Reflection;
using jyu.demo.Common.Extension;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Attributes;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Enums;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace jyu.demo.WorkerDomain.Works.SampleServiceTask;

public class SampleWorkServiceTaskFactory : IWorkServiceFactory<ISampleServiceTaskWorkBase>
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

    public ISampleServiceTaskWorkBase GetServiceInstance(
        string serviceTaskTopicName
    )
    {
        SampleServiceTaskTopicName topicName = serviceTaskTopicName.ConvertStrToEnum<SampleServiceTaskTopicName>();

        IEnumerable<ISampleServiceTaskWorkBase> services = _serviceProvider.GetServices<ISampleServiceTaskWorkBase>();

        var serviceInstance = services.FirstOrDefault(item =>
            (
                item.GetType().GetCustomAttributes<SampleServiceTaskAttribute>().FirstOrDefault() as
                    SampleServiceTaskAttribute
            )?.TopicName == topicName
        ) ?? throw new ArgumentNullException(serviceTaskTopicName);

        return serviceInstance;
    }
}