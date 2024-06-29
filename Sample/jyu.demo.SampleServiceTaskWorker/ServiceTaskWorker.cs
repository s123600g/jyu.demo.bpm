using jyu.demo.BpmDomain;
using jyu.demo.BpmDomain.SampleServiceTask.Enum;
using jyu.demo.BpmDomain.SampleServiceTask.Models;
using jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;
using jyu.demo.BPMN.Camunda.Services;
using jyu.demo.Common.Extension;
using Microsoft.Extensions.Options;

namespace jyu.demo.SampleServiceTaskWorker;

public class ServiceTaskWorker : BackgroundService
{
    private readonly ILogger<ServiceTaskWorker> _log;
    private readonly IServiceProvider _serviceProvider;

    public ServiceTaskWorker(
        ILogger<ServiceTaskWorker> logger
        , IServiceProvider serviceProvider
    )
    {
        _log = logger
               ?? throw new ArgumentNullException(
                   nameof(logger)
               );

        _serviceProvider = serviceProvider
                           ?? throw new ArgumentNullException(
                               nameof(serviceProvider)
                           );
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken
    )
    {
        using (
            var scope = _serviceProvider.CreateScope()
        )
        {
            ICamundaEngineClient camundaEngineClient = scope.ServiceProvider.GetRequiredService<ICamundaEngineClient>();
            IServiceTaskWorkFactory serviceTaskWorkFactory =
                scope.ServiceProvider.GetRequiredService<IServiceTaskWorkFactory>();

            while (
                !stoppingToken.IsCancellationRequested
            )
            {
                // _log.LogInformation("ServiceTaskWorker running at: {time}", DateTimeOffset.Now);

                List<QueryExternalTaskRs> externalTasks = await camundaEngineClient.QueryExternalTaskAsync();

                await Task.WhenAll(
                    ExectueServiceTaskWork(
                        sampleServiceTaskTopicName: SampleServiceTaskTopicName.ServiceTask1
                        , serviceTasks: externalTasks
                        , serviceTaskWorkFactory: serviceTaskWorkFactory
                    )
                    , ExectueServiceTaskWork(
                        sampleServiceTaskTopicName: SampleServiceTaskTopicName.ServiceTask2
                        , serviceTasks: externalTasks
                        , serviceTaskWorkFactory: serviceTaskWorkFactory
                    )
                );

                await Task.Delay(3000, stoppingToken);
            }
        }
    }

    /// <summary>
    /// 執行未被指派運行External Task作業
    /// </summary>
    /// <param name="sampleServiceTaskTopicName"></param>
    /// <param name="serviceTasks"></param>
    private async Task ExectueServiceTaskWork(
        SampleServiceTaskTopicName sampleServiceTaskTopicName
        , List<QueryExternalTaskRs> serviceTasks
        , IServiceTaskWorkFactory serviceTaskWorkFactory
    )
    {
        string topicName = sampleServiceTaskTopicName.GetEnumMemberAttributeValue();

        List<QueryExternalTaskRs> waitExecuteServiceTasks = serviceTasks.Where(item =>
            item.TopicName == topicName
        ).ToList();

        int count = waitExecuteServiceTasks.Count;

        if (
            count != 0
        )
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
                "Service Task Id：{Id}, 執行鎖定和指派對應Work服務：{WorkName}"
                , item.ExternalTaskId
                , topicName
            );

            var serviceInstance = serviceTaskWorkFactory.GetServiceInstance(
                serviceTaskTopicName: sampleServiceTaskTopicName.GetEnumMemberAttributeValue()
            );

            await serviceInstance.ExecuteAsync(
                argServiceTaskWorkData: new ServiceTaskWorkData
                {
                    ExternalTaskId = item.ExternalTaskId
                }
            );
        }
    }
}