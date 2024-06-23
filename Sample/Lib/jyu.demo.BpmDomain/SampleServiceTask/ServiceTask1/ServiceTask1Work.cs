namespace jyu.demo.BpmDomain.SampleServiceTask.ServiceTask1;

using SampleServiceTaskWorker.Services;
using ServiceTaskAttribute;

[SampleServiceTask(SampleServiceTaskTopicName.ServiceTask1)]
public class ServiceTask1Work : IServiceTaskWork
{
    public Task Execute()
    {
        // do something.....
        
        return Task.CompletedTask;
    }
}