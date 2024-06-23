namespace jyu.demo.BpmDomain.SampleServiceTask.ServiceTask2;

using SampleServiceTaskWorker.Services;
using ServiceTaskAttribute;

[SampleServiceTask(SampleServiceTaskTopicName.ServiceTask2)]
public class ServiceTask2Work : IServiceTaskWork
{
    public Task Execute()
    {
        // do something.....

        return Task.CompletedTask;
    }
}