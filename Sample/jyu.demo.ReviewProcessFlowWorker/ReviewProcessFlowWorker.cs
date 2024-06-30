using jyu.demo.Camunda.Enums;
using jyu.demo.Camunda.Models;
using jyu.demo.Camunda.Models.CamundaEngineClient;
using jyu.demo.Camunda.Services;
using jyu.demo.Common.Extension;
using jyu.demo.WorkerDomain;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Enums;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Interface;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Models;
using Microsoft.Extensions.Options;

namespace jyu.demo.ReviewProcessFlowWorker;

public class ReviewProcessFlowWorker : BackgroundService
{
    private readonly ILogger<ReviewProcessFlowWorker> _log;
    private readonly IServiceProvider _serviceProvider;

    private readonly ProcessDefinitionOptions _processDefinitionOptions;

    public ReviewProcessFlowWorker(
        ILogger<ReviewProcessFlowWorker> logger
        , IServiceProvider serviceProvider
        , IOptions<ProcessDefinitionOptions> options
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

        _processDefinitionOptions = options.Value;
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

            var workServiceFactory =
                scope.ServiceProvider.GetRequiredService<IWorkServiceFactory<IReviewProcessFlowWorkBase>>();

            while (
                !stoppingToken.IsCancellationRequested
            )
            {
                // _log.LogInformation("ServiceTaskWorker running at: {time}", DateTimeOffset.Now);

                List<QueryExternalTaskRs> externalTasks = await camundaEngineClient.QueryExternalTaskAsync(
                    new QueryExternalTaskRq
                    {
                        NotLocked = QueryNotLockedType.NoLock,
                        ProcessDefinitionId = _processDefinitionOptions.ProcessDefinitionId
                    }
                );

                await Task.WhenAll(
                    ExectueServiceTaskWork(
                        argProcessFlowTopicName: ReviewProcessFlowTopicName.AssignApprovalCheckpoint
                        , argServiceTasks: externalTasks
                        , argWorkServiceFactory: workServiceFactory
                    )
                    , ExectueServiceTaskWork(
                        argProcessFlowTopicName: ReviewProcessFlowTopicName.ProcessApprovalResults
                        , argServiceTasks: externalTasks
                        , argWorkServiceFactory: workServiceFactory
                    )
                );

                await Task.Delay(3000, stoppingToken);
            }
        }
    }

    private async Task ExectueServiceTaskWork(
        ReviewProcessFlowTopicName argProcessFlowTopicName
        , List<QueryExternalTaskRs> argServiceTasks
        , IWorkServiceFactory<IReviewProcessFlowWorkBase> argWorkServiceFactory
    )
    {
        string topicName = argProcessFlowTopicName.GetEnumMemberAttributeValue();

        List<QueryExternalTaskRs> waitExecuteServiceTasks = argServiceTasks.Where(item =>
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

            var serviceInstance = argWorkServiceFactory.GetServiceInstance(
                serviceTaskTopicName: topicName
            );

            await serviceInstance.ExecuteAsync(
                argReviewProcessFlowWorkData: new ReviewProcessFlowWorkData
                {
                    ExternalTaskId = item.ExternalTaskId,
                    ProcessInstanceId = item.ProcessInstanceId
                }
            );
        }
    }
}