namespace jyu.demo.BpmDomain;

using SampleServiceTaskWorker.Services;

public interface IServiceTaskWorkFactory
{
    IServiceTaskWork GetServiceInstance(
        string serviceTaskTopicName
    );
}