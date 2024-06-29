using jyu.demo.BpmDomain.Models;
using jyu.demo.BpmDomain.Works.SampleServiceTask.Attributes;
using jyu.demo.Camunda.Models.CamundaEngineClient;
using jyu.demo.Camunda.Services;
using jyu.demo.Common.Extension;
using jyu.demo.SampleServiceTaskWorker.Services;

namespace jyu.demo.BpmDomain.Works.SampleServiceTask.ServiceTask1;

[SampleServiceTask(SampleServiceTaskTopicName.ServiceTask1)]
public class ServiceTask1WorkBase : IWorkBase
{
    private readonly ICamundaEngineClient _camundaEngineClient;
    private readonly string _workerId;
    private readonly int _lockDuration;

    public ServiceTask1WorkBase(
        ICamundaEngineClient camundaEngineClient
    )
    {
        _camundaEngineClient = camundaEngineClient
                               ?? throw new ArgumentNullException(
                                   nameof(camundaEngineClient)
                               );

        _workerId = SampleServiceTaskTopicName.ServiceTask1.GetEnumMemberAttributeValue();
        
        _lockDuration = 100000;
    }

    public async Task ExecuteAsync(
        ServiceTaskWorkData argServiceTaskWorkData
    )
    {
        // 執行 Lock external task
        await _camundaEngineClient.LockExternalTaskAsync(
            argExternalTaskId: argServiceTaskWorkData.ExternalTaskId
            , argLockExternalTaskRq: new LockExternalTaskRq
            {
                WorkerId = _workerId,
                LockDuration = _lockDuration,
            }
        );

        // do something.....

        // Complate external task
        await _camundaEngineClient.ComplateExternalTaskAsync(
            argExternalTaskId: argServiceTaskWorkData.ExternalTaskId
            , argComplateExternalTaskRq: new ComplateExternalTaskRq
            {
                WorkerId = _workerId
            }
        );
    }
}