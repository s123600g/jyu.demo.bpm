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

    public string CompleteProcessCurrentTaskAsync(
        string argProcessInstanceTaskId
    );

    public Task<List<QueryExternalTaskRs>> QueryExternalTaskAsync();
}