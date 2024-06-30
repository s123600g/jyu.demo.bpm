using jyu.demo.WorkerDomain.Works.SampleServiceTask.Interface;

namespace jyu.demo.WorkerDomain;

public interface IWorkServiceFactory<T>
{
    T GetServiceInstance(
        string serviceTaskTopicName
    );
}