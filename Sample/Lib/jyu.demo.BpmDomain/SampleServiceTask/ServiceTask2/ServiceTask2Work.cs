using jyu.demo.BpmDomain.SampleServiceTask.Models;
using jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;
using jyu.demo.BPMN.Camunda.Services;
using jyu.demo.Common.Extension;

namespace jyu.demo.BpmDomain.SampleServiceTask.ServiceTask2;

using SampleServiceTaskWorker.Services;
using ServiceTaskAttribute;

[SampleServiceTask(SampleServiceTaskTopicName.ServiceTask2)]
public class ServiceTask2Work : IServiceTaskWork
{
    private readonly ICamundaEngineClient _camundaEngineClient;
    private readonly string _workerId;
    private readonly int _lockDuration;

    public ServiceTask2Work(
        ICamundaEngineClient camundaEngineClient
    )
    {
        _camundaEngineClient = camundaEngineClient
                               ?? throw new ArgumentNullException(
                                   nameof(camundaEngineClient)
                               );

        _workerId = SampleServiceTaskTopicName.ServiceTask2.GetEnumMemberAttributeValue();

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