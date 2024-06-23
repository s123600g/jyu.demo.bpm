namespace jyu.demo.SampleServiceTaskWorker;

using BpmDomain;
using BpmDomain.SampleServiceTask.Enum;
using BPMN.Camunda.Models.CamundaEngineProcessClient;
using BPMN.Camunda.Services.BpmEngingClient;
using Common.Extension;

public class ServiceTaskWorker : BackgroundService
{
    private readonly ILogger<ServiceTaskWorker> _log;
    private readonly ICamundaEngineClient _camundaEngineClient;
    private readonly IServiceTaskWorkFactory _serviceTaskWorkFactory;

    public ServiceTaskWorker(
        ILogger<ServiceTaskWorker> logger
        , ICamundaEngineClient camundaEngineClient
        , IServiceTaskWorkFactory serviceTaskWorkFactory
    )
    {
        _log = logger
               ?? throw new ArgumentNullException(
                   nameof(logger)
               );

        _camundaEngineClient = camundaEngineClient
                               ?? throw new ArgumentNullException(
                                   nameof(camundaEngineClient)
                               );

        _serviceTaskWorkFactory = serviceTaskWorkFactory
                                  ?? throw new ArgumentNullException(
                                      nameof(serviceTaskWorkFactory)
                                  );
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken
    )
    {
        while (
            !stoppingToken.IsCancellationRequested
        )
        {
            _log.LogInformation("ServiceTaskWorker running at: {time}", DateTimeOffset.Now);

            List<QueryExternalTaskRs> externalTasks = await GetNolockExternalTasks();

            Task.WaitAll(
                ExectueServiceTaskWork(
                    sampleServiceTaskTopicName: SampleServiceTaskTopicName.ServiceTask1
                    , serviceTasks: externalTasks
                )
                , ExectueServiceTaskWork(
                    sampleServiceTaskTopicName: SampleServiceTaskTopicName.ServiceTask2
                    , serviceTasks: externalTasks
                )
            );

            await Task.Delay(5000, stoppingToken);
        }
    }

    /// <summary>
    /// 取得當前External Task清單
    /// </summary>
    /// <returns></returns>
    private async Task<List<QueryExternalTaskRs>> GetNolockExternalTasks()
    {
        List<QueryExternalTaskRs> result = await _camundaEngineClient.QueryExternalTaskAsync();

        return result;
    }

    /// <summary>
    /// 執行未被指派運行External Task作業
    /// </summary>
    /// <param name="sampleServiceTaskTopicName"></param>
    /// <param name="serviceTasks"></param>
    private async Task ExectueServiceTaskWork(
        SampleServiceTaskTopicName sampleServiceTaskTopicName
        , List<QueryExternalTaskRs> serviceTasks
    )
    {
        string topicName = sampleServiceTaskTopicName.GetEnumMemberAttributeValue();

        List<QueryExternalTaskRs> waitExecuteServiceTasks = serviceTasks.Where(item =>
            item.WorkerId == topicName
        ).ToList();

        int count = waitExecuteServiceTasks.Count;

        if (
            count == 0
        )
        {
            _log.LogInformation(
                "[{TopicName}]當前無需執行Service Task。"
                , topicName
            );

            return;
        }
        else
        {
            _log.LogInformation(
                "[{TopicName}]當前有需執行Service Task，待處理筆數{Count}。"
                , topicName
                , count
            );
        }

        foreach (
            var item in waitExecuteServiceTasks
        )
        {
            _log.LogInformation(
                "Service Task Id：{Id}({WorkId}), 執行鎖定和指派對應Work服務：{WorkName}"
                , item.ExternalTaskId
                , item.WorkerId
                , topicName
            );

            var serviceInstance = _serviceTaskWorkFactory.GetServiceInstance(
                serviceTaskTopicName: SampleServiceTaskTopicName.ServiceTask1.GetEnumMemberAttributeValue()
            );

            await serviceInstance.Execute();
        }
    }
}