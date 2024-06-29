namespace jyu.demo.WorkerDomain;

public interface IWorkServiceFactory
{
    IWorkBase GetServiceInstance(
        string serviceTaskTopicName
    );
}