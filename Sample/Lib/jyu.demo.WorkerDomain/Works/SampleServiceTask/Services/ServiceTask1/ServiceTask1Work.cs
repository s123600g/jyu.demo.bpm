using jyu.demo.Camunda.Models.CamundaEngineClient;
using jyu.demo.Camunda.Services;
using jyu.demo.Common.Extension;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Attributes;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Enums;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Interface;
using jyu.demo.WorkerDomain.Works.SampleServiceTask.Models;

namespace jyu.demo.WorkerDomain.Works.SampleServiceTask.Services.ServiceTask1;

[SampleServiceTask(SampleServiceTaskTopicName.ServiceTask1)]
public class ServiceTask1Work : ISampleServiceTaskWorkBase
{
    private readonly ICamundaEngineClient _camundaEngineClient;
    private readonly string _workerId;
    private readonly int _lockDuration;

    public ServiceTask1Work(
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
        SampleServiceTaskWorkData argSampleServiceTaskWorkData
    )
    {
        // 執行 Lock external task
        await _camundaEngineClient.LockExternalTaskAsync(
            argExternalTaskId: argSampleServiceTaskWorkData.ExternalTaskId
            , argLockExternalTaskRq: new LockExternalTaskRq
            {
                WorkerId = _workerId,
                LockDuration = _lockDuration,
            }
        );

        // do something.....

        // Complate external task
        await _camundaEngineClient.ComplateExternalTaskAsync(
            argExternalTaskId: argSampleServiceTaskWorkData.ExternalTaskId
            , argComplateExternalTaskRq: new ComplateExternalTaskRq
            {
                WorkerId = _workerId
            }
        );
    }
}