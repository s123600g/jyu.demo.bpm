namespace jyu.demo.BpmDomain;

using SampleServiceTaskWorker.Services;

public interface IWorkServiceFactory
{
    IWorkBase GetServiceInstance(
        string serviceTaskTopicName
    );
}