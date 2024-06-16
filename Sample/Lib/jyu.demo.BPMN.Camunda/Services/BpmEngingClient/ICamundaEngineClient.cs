using jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;

namespace jyu.demo.BPMN.Camunda.Services.BpmEngingClient;

public interface ICamundaEngineClient
{
    public Task<string?> StartNewProcessInstanceAsync(
        string argProcessDefinitionKey
        , StartNewProcessInstanceRq argStartNewProcessInstanceRq
    );

    public Task<string?>  QueryProcessCurrentTaskIdAsync(
        string argProcessInstanceId
    );

    public string CompleteProcessCurrentTaskAsync(
        string argProcessInstanceTaskId
    );
}