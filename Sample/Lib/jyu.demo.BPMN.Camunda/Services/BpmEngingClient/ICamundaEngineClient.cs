namespace jyu.demo.BPMN.Camunda.Services.BpmEngingClient;

using Models.CamundaEngineProcessClient;

public interface ICamundaEngineClient
{
    public Task<string?> StartNewProcessInstanceAsync(
        string argProcessDefinitionKey
        , StartNewProcessInstanceRq argStartNewProcessInstanceRq
    );

    public Task<List<QueryProcessCurrentTaskInfoRs>> QueryProcessCurrentTaskInfoAsync(
        string argProcessInstanceId
    );

    public Task<CompleteTaskRs> CompleteTaskAsync(
        string argProcessInstanceTaskId
        , CompleteTaskRq argCompleteTaskRq
    );

    public Task<List<QueryExternalTaskRs>> QueryExternalTaskAsync();
}