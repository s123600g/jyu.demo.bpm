using jyu.demo.Camunda.Models.CamundaEngineClient;
using jyu.demo.Camunda.Services;
using jyu.demo.Common.Extension;
using jyu.demo.WorkerDomain.Models;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Attributes;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Enum;

namespace jyu.demo.WorkerDomain.Works.SampleServiceTask.ServiceTask2;

[SampleServiceTask(SampleServiceTaskTopicName.ServiceTask2)]
public class ServiceTask2WorkBase : IWorkBase
{
    private readonly ICamundaEngineClient _camundaEngineClient;
    private readonly string _workerId;
    private readonly int _lockDuration;

    public ServiceTask2WorkBase(
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