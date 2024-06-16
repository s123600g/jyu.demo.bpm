using jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;

namespace jyu.demo.BPMN.Services;

public interface IBpmnFlow
{
    public Task Execute(
        StartNewProcessInstanceRq argStartNewProcessInstancePayload
    );
}