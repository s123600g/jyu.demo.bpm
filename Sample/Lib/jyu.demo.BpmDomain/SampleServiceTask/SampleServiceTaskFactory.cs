namespace jyu.demo.BpmDomain.SampleServiceTask;

using System.Reflection;
using SampleServiceTaskWorker.Services;
using Common.Extension;
using Microsoft.Extensions.DependencyInjection;
using ServiceTaskAttribute;

public class SampleServiceTaskFactory : IServiceTaskWorkFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SampleServiceTaskFactory(
        IServiceProvider serviceProvider
    )
    {
        _serviceProvider = serviceProvider
                           ?? throw new ArgumentNullException(
                               nameof(serviceProvider)
                           );
    }

    public IServiceTaskWork GetServiceInstance(
        string serviceTaskTopicName
    )
    {
        SampleServiceTaskTopicName topicName = serviceTaskTopicName.ConvertStrToEnum<SampleServiceTaskTopicName>();

        IEnumerable<IServiceTaskWork> services = _serviceProvider.GetServices<IServiceTaskWork>();

        var serviceInstance = services.FirstOrDefault(item =>
            (
                item.GetType().GetCustomAttributes<SampleServiceTaskAttribute>().FirstOrDefault() as SampleServiceTaskAttribute
            )?.TopicName == topicName
        ) ?? throw new ArgumentNullException(serviceTaskTopicName);

        return serviceInstance;
    }
}